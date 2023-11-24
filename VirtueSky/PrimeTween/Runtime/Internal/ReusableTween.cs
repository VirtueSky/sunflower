using System;
using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween
{
    [Serializable]
    internal class ReusableTween
    {
#if UNITY_EDITOR
        [SerializeField, HideInInspector] internal string debugDescription;
#endif
        internal int id = -1;

        /// Holds a reference to tween's target. If the target is UnityEngine.Object, the tween will gracefully stop when the target is destroyed. That is, destroying object with running tweens is perfectly ok.
        /// Keep in mind: when animating plain C# objects (not derived from UnityEngine.Object), the plugin will hold a strong reference to the object for the entire tween duration.
        ///     If plain C# target holds a reference to UnityEngine.Object and animates its properties, then it's user's responsibility to ensure that UnityEngine.Object still exists.
        [CanBeNull] internal object target;

        [SerializeField, CanBeNull] internal UnityEngine.Object unityTarget;
        [SerializeField] internal bool _isPaused;
        internal bool _isAlive;
        [SerializeField] internal float elapsedTime;
        internal float elapsedTimeInCurrentCycle => elapsedTime;
        internal float easedInterpolationFactor;
        internal float totalDuration;
        internal PropType propType;
        internal TweenType tweenType;
        [SerializeField] internal ValueContainer startValue;
        [SerializeField] internal ValueContainer endValue;
        internal ValueContainer diff;
        internal bool isAdditive;
        internal ValueContainer prevVal;
        [SerializeField] internal TweenSettings settings;
        [SerializeField] internal int cyclesDone;

        internal object customOnValueChange;
        internal int intParam;
        Action<ReusableTween> onValueChange;

        [CanBeNull] internal Action<ReusableTween> onComplete;
        [CanBeNull] object onCompleteCallback;
        [CanBeNull] object onCompleteTarget;

        internal Tween waitFor;
        internal Sequence sequence;
        internal Tween nextInSequence;
        internal int sequenceCycles;
        internal int sequenceCyclesDone;
        internal Sequence parentSequence;
        internal Sequence childSequence;

        internal Func<ReusableTween, ValueContainer> getter;
        internal bool startFromCurrent;

        bool stoppedEmergently;
        internal bool isInterpolationCompleted;
        internal readonly TweenCoroutineEnumerator coroutineEnumerator = new TweenCoroutineEnumerator();
        internal float timeScale = 1;
        bool warnIgnoredOnCompleteIfTargetDestroyed = true;
        internal Tween.ShakeData shakeData = new Tween.ShakeData { frequency = float.NaN };

        internal bool updateAndCheckIfRunning(float dt)
        {
            dt *= timeScale;
            const bool isRunning = true;
            const bool shouldRemove = !isRunning;
            if (!_isAlive)
            {
                if (sequence.IsCreated && sequence.first.id == id && !_isPaused)
                {
                    elapsedTime += dt; // update elapsedTime after death because Sequence relies on elapsedTime when calculates Sequence.elapsedTime and Sequence.elapsedTimeTotal
                }

                return shouldRemove;
            }

            if (waitFor.isAlive)
            {
                return isRunning;
            }

            if (_isPaused)
            {
                return isRunning;
            }

            elapsedTime += dt;
            var isWaitingForStartDelay = elapsedTime < settings.startDelay;
            if (isWaitingForStartDelay)
            {
                return isRunning;
            }

            if (isUnityTargetDestroyed())
            {
                // No need to warn that target was destroyed during animation, this is normal.
                // 'target' is the tween's 'owner' so tweens should be tied to the target's lifetime.
                EmergencyStop(true);
                return shouldRemove;
            }

            var halfDt = dt * 0.5f;
            if (!isInterpolationCompleted && tweenType != TweenType.Delay)
            {
                var duration = settings.duration;
                var startDelayAndDuration = settings.startDelay + duration;
                isInterpolationCompleted = elapsedTime >= startDelayAndDuration - halfDt;
                float interpolationFactor;
                if (isInterpolationCompleted)
                {
                    interpolationFactor = 1;
                }
                else
                {
                    var _elapsedTimeInterpolating = elapsedTime - settings.startDelay;
                    Assert.IsTrue(duration > 0 && _elapsedTimeInterpolating >= 0 && _elapsedTimeInterpolating <= duration);
                    interpolationFactor = _elapsedTimeInterpolating / duration;
                }

                Assert.IsTrue(interpolationFactor <= 1);
                ReportOnValueChange(interpolationFactor);
                // ReportOnValueChange() calls onValueChange(), and onValueChange() can execute any code, including Tween.StopAll() or Tween.Stop().
                // So we have to check if a tween wasn't killed after the calling ReportOnValueChange()
                if (stoppedEmergently || !_isAlive)
                {
                    return shouldRemove;
                }
            }

            if (onUpdate != null)
            {
                onUpdate(this);
                if (stoppedEmergently || !_isAlive)
                {
                    return shouldRemove;
                }
            }

            var isWaitingForEndDelay = elapsedTime < totalDuration - halfDt;
            if (isWaitingForEndDelay)
            {
                return isRunning;
            }

            Assert.AreNotEqual(0, settings.cycles);
            cyclesDone++;
            elapsedTime = 0; // after completing a cycle it's reasonable that elapsedTime should be reset to 0 because new cycle has begun
            if (cyclesDone == settings.cycles)
            {
                kill();
                ReportOnComplete();
                updateSequenceAfterKill();
                return shouldRemove;
            }

            // no need to reset startFromCurrent here because getter should be used only once on tween start
            // var isCustomTween = getter == null;
            // startFromCurrent = settings.startFromCurrentValue && !isCustomTween;

            Assert.IsFalse(startFromCurrent);
            onCycleComplete();
            return isRunning;
        }

        internal void onCycleComplete()
        {
            if (settings.cycleMode == CycleMode.Incremental)
            {
                increment();
            }

            shakeData.onCycleComplete();
            isInterpolationCompleted = false;
        }

        void increment()
        {
            if (propType == PropType.Quaternion)
            {
                startValue.CopyFrom(ref endValue.QuaternionVal);
                endValue.QuaternionVal =
                    (endValue.QuaternionVal * diff.QuaternionVal)
                    .normalized; // normalize the result because the float imprecision accumulates over time turning the quaternion to zero
            }
            else
            {
                startValue = endValue;
                endValue.Vector4Val += diff.Vector4Val;
            }
        }

        internal void rewindIncrementalTween()
        {
            Assert.AreEqual(settings.cycles, cyclesDone);
            if (settings.cycleMode != CycleMode.Incremental)
            {
                return;
            }

            var oldDiff = diff;
            ValueContainer invertedDiff = default;
            if (propType == PropType.Quaternion)
            {
                invertedDiff.QuaternionVal = Quaternion.Inverse(oldDiff.QuaternionVal);
            }
            else
            {
                invertedDiff.Vector4Val = -oldDiff.Vector4Val;
            }

            diff = invertedDiff;
            for (int i = 0; i < settings.cycles; i++)
            {
                increment();
            }

            diff = oldDiff;
        }

        internal void Reset()
        {
            Assert.AreEqual(0, aliveTweensInSequence);
            Assert.AreEqual(0, sequenceCycles);
            Assert.AreEqual(0, sequenceCyclesDone);
            Assert.IsFalse(_isAlive);
            Assert.IsFalse(sequence.IsCreated);
            Assert.IsFalse(nextInSequence.IsCreated);
            Assert.IsFalse(IsInSequence());
            Assert.IsFalse(parentSequence.IsCreated);
            Assert.IsFalse(childSequence.IsCreated);
#if UNITY_EDITOR
            debugDescription = null;
#endif
            id = -1;
            target = null;
            unityTarget = null;
            propType = PropType.None;
            settings.customEase = null;
            customOnValueChange = null;
            shakeData.Reset();
            onValueChange = null;
            onComplete = null;
            onCompleteCallback = null;
            onCompleteTarget = null;
            getter = null;
            stoppedEmergently = false;
            isInterpolationCompleted = false;
            waitFor = default;
            coroutineEnumerator.resetEnumerator();
            tweenType = TweenType.None;
            timeScale = 1;
            warnIgnoredOnCompleteIfTargetDestroyed = true;
            clearOnUpdate();
        }

        /// <param name="warnIfTargetDestroyed">https://github.com/KyryloKuzyk/PrimeTween/discussions/4</param>
        internal void OnComplete([NotNull] Action _onComplete, bool warnIfTargetDestroyed)
        {
            Assert.IsNotNull(_onComplete);
            validateOnCompleteAssignment();
            warnIgnoredOnCompleteIfTargetDestroyed = warnIfTargetDestroyed;
            onCompleteCallback = _onComplete;
            onComplete = tween =>
            {
                var callback = tween.onCompleteCallback as Action;
                Assert.IsNotNull(callback);
                try
                {
                    callback();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Tween's onComplete callback raised exception, tween: {tween.GetDescription()}, exception:\n{e}", tween.unityTarget);
                }
            };
        }

        internal void OnComplete<T>([NotNull] T _target, [NotNull] Action<T> _onComplete, bool warnIfTargetDestroyed) where T : class
        {
            if (isDestroyedUnityObject(_target))
            {
                Debug.LogError(Constants.targetDestroyed);
                return;
            }

            Assert.IsNotNull(_onComplete);
            validateOnCompleteAssignment();
            warnIgnoredOnCompleteIfTargetDestroyed = warnIfTargetDestroyed;
            onCompleteTarget = _target;
            onCompleteCallback = _onComplete;
            onComplete = tween =>
            {
                var callback = tween.onCompleteCallback as Action<T>;
                Assert.IsNotNull(callback);
                var _onCompleteTarget = tween.onCompleteTarget as T;
                if (isDestroyedUnityObject(_onCompleteTarget))
                {
                    tween.warnOnCompleteIgnored(true);
                    return;
                }

                try
                {
                    callback(_onCompleteTarget);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Tween's onComplete callback raised exception, target: {tween.GetDescription()}, exception:\n{e}", tween.unityTarget);
                }
            };
        }

        internal static bool isDestroyedUnityObject<T>(T obj) where T : class => obj is UnityEngine.Object unityObject && unityObject == null;

        void validateOnCompleteAssignment()
        {
            const string msg = "Tween already has an onComplete callback. Adding more callbacks is not allowed.\n" +
                               "Workaround: wrap a tween in a Sequence by calling Sequence.Create(tween) and use multiple ChainCallback().\n";
            Assert.IsNull(onCompleteTarget, msg);
            Assert.IsNull(onCompleteCallback, msg);
            Assert.IsNull(onComplete, msg);
        }

        /// _getter is null for custom tweens
        internal void Setup([CanBeNull] object _target, ref TweenSettings _settings, [NotNull] Action<ReusableTween> _onValueChange,
            [CanBeNull] Func<ReusableTween, ValueContainer> _getter, bool _startFromCurrent)
        {
#if UNITY_EDITOR
            if (Constants.noInstance)
            {
                return;
            }
#endif
            Assert.IsTrue(_settings.cycles >= -1);
            Assert.IsNotNull(_onValueChange);
            Assert.IsNull(getter);
            Assert.AreNotEqual(PropType.None, propType);
            if (_settings.ease == Ease.Default)
            {
                _settings.ease = PrimeTweenManager.Instance.defaultEase;
            }
            else if (_settings.ease == Ease.Custom && _settings.parametricEase == ParametricEase.None)
            {
                if (_settings.customEase == null || !TweenSettings.ValidateCustomCurveKeyframes(_settings.customEase))
                {
                    Debug.LogError($"Ease type is Ease.Custom, but {nameof(TweenSettings.customEase)} is not configured correctly.");
                    _settings.ease = PrimeTweenManager.Instance.defaultEase;
                }
            }

            target = _target;
            setUnityTarget(_target);
            elapsedTime = 0f;
            easedInterpolationFactor = 0f;
            _isPaused = false;
            revive();

            cyclesDone = 0;
            _settings.SetValidValues();
            settings.CopyFrom(ref _settings);
            recalculateTotalDuration();
            Assert.IsTrue(totalDuration >= 0);
            onValueChange = _onValueChange;
            Assert.IsFalse(_startFromCurrent && _getter == null);
            startFromCurrent = _startFromCurrent;
            getter = _getter;
            if (!_startFromCurrent)
            {
                cacheDiff();
            }

            if (propType == PropType.Quaternion)
            {
                prevVal.QuaternionVal = Quaternion.identity;
            }
            else
            {
                prevVal.Reset();
            }
        }

        internal void setUnityTarget(object _target)
        {
            var unityObject = _target as UnityEngine.Object;
            unityTarget = unityObject;
        }

        internal void ReportOnValueChangeIfAnimation(float _interpolationFactor)
        {
            if (tweenType != TweenType.Delay)
            {
                Assert.IsFalse(isUnityTargetDestroyed());
                ReportOnValueChange(_interpolationFactor);
            }
        }

        /// Tween.Custom and Tween.ShakeCustom try-catch the <see cref="onValueChange"/> and calls <see cref="ReusableTween.EmergencyStop"/> if an exception occurs.
        /// <see cref="ReusableTween.EmergencyStop"/> sets <see cref="stoppedEmergently"/> to true.
        void ReportOnValueChange(float _interpolationFactor)
        {
            if (startFromCurrent)
            {
                startFromCurrent = false;
                startValue = Tween.tryGetStartValueFromOtherShake(this) ?? getter(this);
                cacheDiff();
            }

            easedInterpolationFactor = calcEasedT(_interpolationFactor);
            onValueChange(this);
        }

        void ReportOnComplete()
        {
            Assert.IsTrue(tweenType == TweenType.Delay || isInterpolationCompleted);
            Assert.IsFalse(startFromCurrent);
            Assert.AreEqual(settings.cycles, cyclesDone);
            Assert.IsFalse(_isAlive);
            onComplete?.Invoke(this);
        }

        internal bool tryManipulate()
        {
            if (warnIfTargetDestroyed())
            {
                EmergencyStop(true);
                return false;
            }

            return true;
        }

        internal bool warnIfTargetDestroyed()
        {
            if (isUnityTargetDestroyed())
            {
                Debug.LogError($"{Constants.targetDestroyed} Tween: {GetDescription()}");
                warnOnCompleteIgnored(true);
                return true;
            }

            return false;
        }

        internal bool isUnityTargetDestroyed() => isDestroyedUnityObject(unityTarget);

        internal bool HasOnComplete => onComplete != null;

        [NotNull]
        internal string GetDescription()
        {
            string result = "";
            if (target != null)
            {
                result += $"{(unityTarget != null ? unityTarget.name : target.GetType().Name)} / ";
            }

            var duration = settings.duration;
            if (tweenType == TweenType.Delay && duration == 0)
            {
            }
            else
            {
                result += $"{(tweenType != TweenType.None ? tweenType.ToString() : propType.ToString())} / duration {duration:0.##} / ";
            }

            result += $"id {id}";
            if (sequence.IsCreated)
            {
                result += $" / sequence {sequence.id}";
            }

            return result;
        }

        internal float calcDurationWithWaitDependencies()
        {
            var result = 0f;
            var current = new Tween(this);
            while (true)
            {
                var currentTween = current.tween;
                var cycles = currentTween.settings.cycles;
                Assert.AreNotEqual(-1, cycles, "It's impossible to calculate the duration of an infinite tween (cycles == -1).");
                Assert.AreNotEqual(0, cycles);
                result += currentTween.totalDuration * cycles;
                var waitDependency = currentTween.waitFor;
                if (!waitDependency.IsCreated)
                {
                    break;
                }

                current = waitDependency;
            }

            return result;
        }

        internal void recalculateTotalDuration()
        {
            totalDuration = settings.startDelay + settings.duration + settings.endDelay;
        }

        internal void updateSequenceAfterKill()
        {
            if (sequence.IsCreated)
            {
                sequence.onTweenKilled(id);
            }
        }

        internal float FloatVal => startValue.x + diff.x * easedInterpolationFactor;

        internal Vector2 Vector2Val
        {
            get
            {
                var easedT = easedInterpolationFactor;
                return new Vector2(
                    startValue.x + diff.x * easedT,
                    startValue.y + diff.y * easedT);
            }
        }

        internal Vector3 Vector3Val
        {
            get
            {
                var easedT = easedInterpolationFactor;
                return new Vector3(
                    startValue.x + diff.x * easedT,
                    startValue.y + diff.y * easedT,
                    startValue.z + diff.z * easedT);
            }
        }

        internal Vector4 Vector4Val
        {
            get
            {
                var easedT = easedInterpolationFactor;
                return new Vector4(
                    startValue.x + diff.x * easedT,
                    startValue.y + diff.y * easedT,
                    startValue.z + diff.z * easedT,
                    startValue.w + diff.w * easedT);
            }
        }

        internal Color ColorVal
        {
            get
            {
                var easedT = easedInterpolationFactor;
                return new Color(
                    startValue.x + diff.x * easedT,
                    startValue.y + diff.y * easedT,
                    startValue.z + diff.z * easedT,
                    startValue.w + diff.w * easedT);
            }
        }

        internal Rect RectVal
        {
            get
            {
                var easedT = easedInterpolationFactor;
                return new Rect(
                    startValue.x + diff.x * easedT,
                    startValue.y + diff.y * easedT,
                    startValue.z + diff.z * easedT,
                    startValue.w + diff.w * easedT);
            }
        }

        internal Quaternion QuaternionVal => Quaternion.SlerpUnclamped(startValue.QuaternionVal, endValue.QuaternionVal, easedInterpolationFactor);

        float calcEasedT(float t)
        {
            var isForwardCycle = cyclesDone % 2 == 0;
            if (isForwardCycle)
            {
                return evaluate(t);
            }

            switch (settings.cycleMode)
            {
                case CycleMode.Restart:
                case CycleMode.Incremental:
                    return evaluate(t);
                case CycleMode.Yoyo:
                    return 1 - evaluate(t);
                case CycleMode.Rewind:
                    return evaluate(1 - t);
                default:
                    throw new Exception();
            }
        }

        float evaluate(float t)
        {
            if (settings.ease == Ease.Custom)
            {
                if (settings.parametricEase != ParametricEase.None)
                {
                    return Easing.Evaluate(t, this);
                }

                return settings.customEase.Evaluate(t);
            }

            return StandardEasing.Evaluate(t, settings.ease);
        }

        internal void cacheDiff()
        {
            Assert.IsFalse(startFromCurrent);
            Assert.AreNotEqual(PropType.None, propType);
            if (propType == PropType.Quaternion)
            {
                startValue.QuaternionVal.Normalize();
                endValue.QuaternionVal.Normalize();
                diff.QuaternionVal = Quaternion.Inverse(startValue.QuaternionVal) * endValue.QuaternionVal;
            }
            else
            {
                diff.x = endValue.x - startValue.x;
                diff.y = endValue.y - startValue.y;
                diff.z = endValue.z - startValue.z;
                diff.w = endValue.w - startValue.w;
            }
        }

        /// This method can be called at any time (for example, immediately after destroying the unityTarget), so before calling it, we should guarantee that <see cref="isUnityTargetDestroyed()"/> == false.
        internal void ForceComplete()
        {
            Assert.IsFalse(isUnityTargetDestroyed());
            kill(); // protects from recursive call

            if (settings.cycleMode == CycleMode.Incremental)
            {
                var incrementsLeft = settings.cycles - cyclesDone - 1;
                for (int i = 0; i < incrementsLeft; i++)
                {
                    increment();
                }
            }

            cyclesDone = settings.cycles - 1; // simulate the last cycle so calcEasedT() calculates the correct value depending on cycleMode
            ReportOnValueChangeIfAnimation(1);
            if (stoppedEmergently)
            {
                return;
            }

            isInterpolationCompleted = true;
            cyclesDone = settings.cycles;
            ReportOnComplete();
            Assert.IsTrue(sequence.IsCreated || !_isAlive);
        }

        internal void warnOnCompleteIgnored(bool isTargetDestroyed)
        {
            if (HasOnComplete && warnIgnoredOnCompleteIfTargetDestroyed)
            {
                onComplete = null;
                var msg = $"{Constants.onCompleteCallbackIgnored} Tween: {GetDescription()}.\n";
                if (isTargetDestroyed)
                {
                    msg +=
                        "\nIf you use tween.OnComplete(), Tween.Delay(), or sequence.ChainDelay() only for cosmetic purposes, you can turn off this error by passing 'warnIfTargetDestroyed: false' to the method.\n" +
                        "More info: https://github.com/KyryloKuzyk/PrimeTween/discussions/4\n";
                }

                Debug.LogError(msg, unityTarget);
            }
        }

        internal void EmergencyStop(bool isTargetDestroyed = false)
        {
            if (sequence.IsCreated)
            {
                sequence.emergencyStop();
                Assert.IsFalse(_isAlive);
            }
            else if (_isAlive)
            {
                // EmergencyStop() can be called after ForceComplete() and a caught exception in Tween.Custom()
                kill();
            }

            stoppedEmergently = true;
            warnOnCompleteIgnored(isTargetDestroyed);
            Assert.IsFalse(_isAlive);
            Assert.IsFalse(sequence.isAlive);
        }

        internal void kill()
        {
            // Debug.Log($"{Time.frameCount} kill {GetDescription()}");
            Assert.IsTrue(_isAlive);
            _isAlive = false;
        }

        internal void revive()
        {
            // Debug.Log($"{Time.frameCount} revive {GetDescription()}");
            Assert.IsFalse(_isAlive);
            _isAlive = true;
        }

        internal bool IsInSequence()
        {
            return sequence.IsCreated || nextInSequence.IsCreated;
        }

        internal void setNextInSequence(Tween? tween)
        {
            if (!tween.HasValue)
            {
                Assert.IsTrue(sequence.IsCreated);
                nextInSequence = default;
                return;
            }

            Assert.IsTrue(sequence.isAlive);
            Assert.IsFalse(nextInSequence.IsCreated);
            nextInSequence = tween.Value;
            sequence.first.tween.addAliveTweensInSequence(1, tween.Value.id);
        }

        internal void setWaitFor(Tween tween)
        {
            Assert.IsFalse(waitFor.IsCreated);
            Assert.IsTrue(tween.IsCreated);
            Assert.AreNotEqual(id, tween.id);
            waitFor = tween;
        }

        internal bool trySetPause(bool _isPaused)
        {
            if (sequence.IsCreated)
            {
                Debug.LogError(Constants.setPauseOnTweenInsideSequenceError);
                return false;
            }

            if (this._isPaused == _isPaused)
            {
                return false;
            }

            this._isPaused = _isPaused;
            return true;
        }

        // ReSharper disable once UnusedParameter.Global
        internal void addAliveTweensInSequence(int _diff, int tweenId)
        {
            aliveTweensInSequence += _diff;
        }

        internal int aliveTweensInSequence;

        [CanBeNull] object onUpdateTarget;
        object onUpdateCallback;
        Action<ReusableTween> onUpdate;

        internal void SetOnUpdate<T>(T _target, [NotNull] Action<T, Tween> _onUpdate) where T : class
        {
            Assert.IsNull(onUpdate, "Only one OnUpdate() is allowed for one tween.");
            Assert.IsNotNull(_onUpdate, nameof(_onUpdate) + " is null!");
            onUpdateTarget = _target;
            onUpdateCallback = _onUpdate;
            onUpdate = reusableTween => reusableTween.invokeOnUpdate<T>();
        }

        void invokeOnUpdate<T>() where T : class
        {
            var callback = onUpdateCallback as Action<T, Tween>;
            Assert.IsNotNull(callback);
            var _onUpdateTarget = onUpdateTarget as T;
            if (isDestroyedUnityObject(_onUpdateTarget))
            {
                Debug.LogError($"OnUpdate() will not be called again because OnUpdate()'s target has been destroyed, tween: {GetDescription()}", unityTarget);
                clearOnUpdate();
                return;
            }

            try
            {
                callback(_onUpdateTarget, new Tween(this));
            }
            catch (Exception e)
            {
                Debug.LogError($"OnUpdate() will not be called again because it thrown exception, tween: {GetDescription()}, exception:\n{e}", unityTarget);
                clearOnUpdate();
            }
        }

        void clearOnUpdate()
        {
            onUpdateTarget = null;
            onUpdateCallback = null;
            onUpdate = null;
        }
    }
}