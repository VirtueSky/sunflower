#if PRIME_TWEEN_INSPECTOR_DEBUGGING && UNITY_EDITOR
#define ENABLE_SERIALIZATION
#endif
using System;
using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween
{
    /// <summary>The main API of the PrimeTween library.<br/><br/>
    /// Use static Tween methods to start animations (tweens).<br/>
    /// Use the returned Tween struct to control the running tween and access its properties.<br/><br/>
    /// Tweens are non-reusable. That is, when a tween completes (or is stopped manually), it becomes 'dead' (<see cref="isAlive"/> == false) and can no longer be used to control the tween or access its properties.<br/>
    /// To restart the animation from the beginning (or play in the opposite direction), simply start a new Tween. Starting tweens is very fast and doesn't allocate garbage,
    /// so you can start hundreds of tweens per seconds with no performance overhead.</summary>
    /// <example><code>
    /// var tween = Tween.LocalPositionX(transform, endValue: 1.5f, duration: 1f);
    /// // Let the tween run for some time...
    /// if (tween.isAlive) {
    ///     Debug.Log($"Animation is still running, elapsed time: {tween.elapsedTime}.");
    /// } else {
    ///     Debug.Log("Animation is already completed.");
    /// }
    /// </code></example>
#if ENABLE_SERIALIZATION
    [Serializable]
#endif
    public
#if !ENABLE_SERIALIZATION
        readonly
#endif
        partial struct Tween : IEquatable<Tween>
    {
        /// Uniquely identifies the tween.
        /// Can be observed from the Debug Inspector if PRIME_TWEEN_INSPECTOR_DEBUGGING is defined. Use only for debugging purposes.
        internal
#if !ENABLE_SERIALIZATION
            readonly
#endif
            int id;

        internal readonly ReusableTween tween;

        /// This should not be part of public API. When tween IsCreated and !isAlive, it's not guaranteed to be IsCompleted, it can also be stopped.
        internal bool IsCreated => id != 0;

        internal Tween([NotNull] ReusableTween tween)
        {
            Assert.IsNotNull(tween);
            id = tween.id;
            this.tween = tween;
        }

        /// A tween is 'alive' when it has been created and is not stopped or completed yet. Paused tween is also considered 'alive'.
        public bool isAlive => id != 0 && tween.id == id && tween._isAlive;

        /// Elapsed time of the current cycle.
        public float elapsedTime => validateIsAlive() ? tween.elapsedTimeInCurrentCycle : 0;

        /// The total number of cycles. Returns -1 to indicate infinite number cycles.
        public int cyclesTotal => validateIsAlive() ? tween.settings.cycles : 0;

        public int cyclesDone => validateIsAlive() ? tween.cyclesDone : 0;

        /// The duration of one cycle.
        public float duration
        {
            get
            {
                if (!validateIsAlive())
                {
                    return 0;
                }

                var result = tween.totalDuration;
                TweenSettings.validateFiniteDuration(result);
                return result;
            }
        }

        [NotNull]
        public override string ToString()
        {
            return isAlive ? tween.GetDescription() : $"DEAD / id {id}";
        }

        SharedProps sharedProps => isAlive ? new SharedProps(true, elapsedTime, cyclesTotal, cyclesDone, duration) : new SharedProps();

        /// Elapsed time of all cycles.
        public float elapsedTimeTotal => sharedProps.elapsedTimeTotal;

        /// <summary>The duration of all cycles. If cycles == -1, returns <see cref="float.PositiveInfinity"/>.</summary>
        public float durationTotal => sharedProps.durationTotal;

        /// Normalized progress of the current cycle expressed in 0..1 range.
        public float progress => sharedProps.progress;

        /// Normalized progress of all cycles expressed in 0..1 range.
        public float progressTotal => sharedProps.progressTotal;

        /// <summary>The current percentage of change between 'startValue' and 'endValue' values in 0..1 range.</summary>
        public float interpolationFactor => validateIsAlive() ? tween.easedInterpolationFactor : 0;

        public bool isPaused
        {
            get => validateIsAlive() && tween._isPaused;
            set
            {
                if (tryManipulate())
                {
                    tween.trySetPause(value);
                }
            }
        }

        /// Interrupts the tween, ignoring onComplete callback. 
        public void Stop()
        {
            if (isAlive)
            {
                tween.kill();
                tween.updateSequenceAfterKill();
            }
        }

        /// Immediately sets the tween to the endValue and calls onComplete.
        public void Complete()
        {
            // don't warn that tween is dead because dead tween means that it's already 'completed'
            if (isAlive && tween.tryManipulate())
            {
                tween.ForceComplete();
                tween.updateSequenceAfterKill();
            }
        }

        bool tryManipulate() => validateIsAlive() && tween.tryManipulate();

        /// <summary>Stops the tween when it reaches 'startValue' or 'endValue' for the next time.<br/>
        /// For example, if you have an infinite tween (cycles == -1) with CycleMode.Yoyo/Rewind, and you wish to stop it when it reaches the 'endValue' (odd cycle), then set <see cref="stopAtEndValue"/> to true.
        /// To stop the animation at the 'startValue' (even cycle), set <see cref="stopAtEndValue"/> to false.</summary>
        public void SetCycles(bool stopAtEndValue)
        {
            if (isAlive && (tween.settings.cycleMode == CycleMode.Restart || tween.settings.cycleMode == CycleMode.Incremental))
            {
                Debug.LogWarning(nameof(SetCycles) + "(bool " + nameof(stopAtEndValue) +
                                 ") is meant to be used with CycleMode.Yoyo or Rewind. Please consider using the overload that accepts int instead.");
            }

            SetCycles(tween.cyclesDone % 2 == 0 == stopAtEndValue ? 1 : 2);
        }

        /// <summary>Sets the number of remaining cycles.
        /// This method modifies the <see cref="cyclesTotal"/> so that the tween will complete after the number of <see cref="cycles"/>.
        /// Setting cycles to -1 will repeat the tween indefinitely.</summary>
        public void SetCycles(int cycles)
        {
            Assert.IsTrue(cycles >= -1);
            if (!tryManipulate())
            {
                return;
            }

            if (cycles == -1)
            {
                if (tween.sequence.IsCreated)
                {
                    Debug.LogError(Constants.infiniteTweenInSequenceError);
                    return;
                }

                tween.settings.cycles = -1;
            }
            else
            {
                TweenSettings.setCyclesTo1If0(ref cycles);
                tween.settings.cycles = tween.cyclesDone + cycles;
            }
        }

        /// <summary>Adds completion callback. Please consider using <see cref="OnComplete{T}"/> to prevent a possible capture of variable into a closure.</summary>
        /// <param name="warnIfTargetDestroyed">Set to 'false' to disable the error about target's destruction. Please note that the the <see cref="onComplete"/> callback will be silently ignored in the case of target's destruction. More info: https://github.com/KyryloKuzyk/PrimeTween/discussions/4</param>
        public Tween OnComplete([NotNull] Action onComplete, bool warnIfTargetDestroyed = true)
        {
            if (canAddOnComplete())
            {
                tween.OnComplete(onComplete, warnIfTargetDestroyed);
            }

            return this;
        }

        /// <summary>Adds completion callback.</summary>
        /// <param name="warnIfTargetDestroyed">Set to 'false' to disable the error about target's destruction. Please note that the the <see cref="onComplete"/> callback will be silently ignored in the case of target's destruction. More info: https://github.com/KyryloKuzyk/PrimeTween/discussions/4</param>
        /// <example>The example shows how to destroy the object after the completion of a tween.
        /// Please note: we're using the '_transform' variable from the onComplete callback to prevent garbage allocation. Using the 'transform' variable directly will capture it into a closure and generate garbage.
        /// <code>
        /// Tween.PositionX(transform, endValue: 1.5f, duration: 1f)
        ///     .OnComplete(transform, _transform =&gt; Destroy(_transform.gameObject));
        /// </code></example>
        public Tween OnComplete<T>([NotNull] T target, [NotNull] Action<T> onComplete, bool warnIfTargetDestroyed = true) where T : class
        {
            if (canAddOnComplete())
            {
                tween.OnComplete(target, onComplete, warnIfTargetDestroyed);
            }

            return this;
        }

        bool canAddOnComplete()
        {
            if (!validateIsAlive())
            {
                return false;
            }

            if (tween.warnIfTargetDestroyed())
            {
                return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Tween other && Equals(other);
        }

        public bool Equals(Tween other)
        {
            return id == other.id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public Sequence Group(Tween _tween) => Sequence.Create(this).Group(_tween);
        public Sequence Chain(Tween _tween) => Sequence.Create(this).Chain(_tween);
        public Sequence Group(Sequence sequence) => Sequence.Create(this).Group(sequence);
        public Sequence Chain(Sequence sequence) => Sequence.Create(this).Chain(sequence);

        bool validateIsAlive() => Constants.validateIsAlive(isAlive);

        /// <summary>Custom timeScale. To smoothly animate timeScale over time, use <see cref="Tween.TweenTimeScale"/> method.</summary>
        public float timeScale
        {
            get => validateIsAlive() ? tween.timeScale : 1;
            set
            {
                if (!tryManipulate())
                {
                    return;
                }

                TweenSettings.clampTimescale(ref value);
                if (tween.IsInSequence())
                {
                    Debug.LogError("Setting timeScale is not allowed because this tween in a Sequence. Please use Sequence.timeScale instead.");
                    return;
                }

                tween.timeScale = value;
            }
        }

        public Tween OnUpdate<T>(T target, Action<T, Tween> onUpdate) where T : class
        {
            if (validateIsAlive())
            {
                tween.SetOnUpdate(target, onUpdate);
            }

            return this;
        }
    }
}