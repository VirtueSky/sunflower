#if PRIME_TWEEN_SAFETY_CHECKS && UNITY_ASSERTIONS
#define SAFETY_CHECKS
#endif
#if PRIME_TWEEN_INSPECTOR_DEBUGGING && UNITY_EDITOR
#define ENABLE_SERIALIZATION
#endif
using System;
using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween
{
    /// <summary>An ordered group of tweens and callbacks. Tweens in a sequence can run in parallel to one another with <see cref="Group"/> and sequentially with <see cref="Chain"/>.<br/>
    /// To make tweens in a Sequence overlap each other, use <see cref="TweenSettings.startDelay"/> and <see cref="TweenSettings.endDelay"/>.</summary>
    /// <example><code>
    /// Sequence.Create()
    ///     .Group(Tween.PositionX(transform, endValue: 10f, duration: 1.5f))
    ///     .Group(Tween.Scale(transform, endValue: 2f, duration: 0.5f)) // position and localScale tweens will run in parallel because they are 'grouped'
    ///     .Chain(Tween.Rotation(transform, endValue: new Vector3(0f, 0f, 45f), duration: 1f)) // rotation tween is 'chained' so it will start when both previous tweens are finished (after 1.5 seconds) 
    ///     .ChainCallback(() =&gt; Debug.Log("Sequence completed"));
    /// </code></example>
#if ENABLE_SERIALIZATION
    [Serializable]
#endif
    public
#if !ENABLE_SERIALIZATION
        readonly
#endif
        partial struct Sequence : IEquatable<Sequence>
    {
        internal
#if !ENABLE_SERIALIZATION
            readonly
#endif
            int id;

        internal
#if !ENABLE_SERIALIZATION
            readonly
#endif
            Tween first;

        internal bool IsCreated => id != 0;

        /// Sequence is 'alive' when any of its tweens is 'alive'.
        public bool isAlive => IsCreated && first.tween.sequence.id == id;

        /// Elapsed time of the current cycle.
        public float elapsedTime
        {
            get
            {
                if (!validateIsAlive())
                {
                    return 0;
                }

                var t = first.tween;
                return t.elapsedTimeInCurrentCycle + t.cyclesDone * t.totalDuration;
            }
        }

        /// The total number of cycles. Returns -1 to indicate infinite number cycles.
        public int cyclesTotal => validateIsAlive() ? first.tween.sequenceCycles : 0;

        public int cyclesDone => validateIsAlive() ? first.tween.sequenceCyclesDone : 0;

        /// The duration of one cycle.
        public float duration
        {
            get
            {
                if (!validateIsAlive())
                {
                    return 0;
                }

                getLongest(out var result);
                TweenSettings.validateFiniteDuration(result);
                return result;
            }
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

        internal bool validateIsAlive() => Constants.validateIsAlive(isAlive);

        public static Sequence Create()
        {
            return new Sequence(PrimeTweenManager.createEmpty());
        }

        public static Sequence Create(Tween firstTween)
        {
            return new Sequence(firstTween);
        }

        Sequence(Tween firstTween)
        {
#if UNITY_EDITOR
            if (Constants.noInstance)
            {
                first = default;
                id = 0;
                return;
            }
#endif
            var instance = PrimeTweenManager.Instance;
            instance.lastSequenceId++;
            id = instance.lastSequenceId;
            validate(firstTween);
            first = firstTween;
            Assert.AreEqual(0, firstTween.tween.aliveTweensInSequence);
            setSequence(firstTween);
            firstTween.tween.addAliveTweensInSequence(1, firstTween.id);
            firstTween.tween.sequenceCycles = 1;
            Assert.IsTrue(isAlive);
        }

        /// <summary>Groups <paramref name="tween"/> with the 'last' tween/sequence in this Sequence.
        /// The 'last' is the tween/sequence passed to the last Group/Chain() method.
        /// Grouped tweens/sequences start at the same time and run in parallel.
        /// Grouping begins with <see cref="Group"/> and ends with <see cref="Chain"/>.</summary>
        public Sequence Group(Tween tween)
        {
            requireFinite(tween);
            Assert.IsTrue(IsCreated, Constants.defaultSequenceCtorError);
            if (!validateIsAlive())
            {
                return this;
            }

            Assert.IsTrue(isAlive);
            validate(tween);

            var waitDep = getWaitDep();
            if (waitDep.HasValue)
            {
                tween.tween.setWaitFor(waitDep.Value);
            }

            getLastInSelf().tween.setNextInSequence(tween);
            setSequence(tween);
            return this;
        }

        Tween? getWaitDep()
        {
            var result = getLastChildSequenceOrSelf().getLastInSelf().tween.waitFor;
            // ReSharper disable once RedundantCast
            return result.IsCreated ? result : (Tween?)null;
        }

        /// <summary>Schedules <see cref="tween"/> after all tweens/sequences in this Sequence.</summary>
        public Sequence Chain(Tween tween)
        {
            Assert.IsTrue(IsCreated, Constants.defaultSequenceCtorError);
            if (!validateIsAlive())
            {
                return this;
            }

            return chain(tween, getLongest());
        }

        Sequence chain(Tween other, Tween after)
        {
            Assert.IsTrue(isAlive);
            validate(other);
            Assert.IsTrue(after.IsCreated);
            getLastInSelf().tween.setNextInSequence(other);
            other.tween.setWaitFor(after);
            setSequence(other);
            return this;
        }

        /// <summary>Schedules <see cref="callback"/> after all previously added tweens.</summary>
        /// <param name="warnIfTargetDestroyed">https://github.com/KyryloKuzyk/PrimeTween/discussions/4</param>
        public Sequence ChainCallback([NotNull] Action callback, bool warnIfTargetDestroyed = true)
        {
            var delay = PrimeTweenManager.createEmpty();
            delay.tween.OnComplete(callback, warnIfTargetDestroyed);
            return Chain(delay);
        }

        /// <summary>Schedules <see cref="callback"/> after all previously added tweens. Passing 'target' allows to write a non-allocating callback.</summary>
        /// <param name="warnIfTargetDestroyed">https://github.com/KyryloKuzyk/PrimeTween/discussions/4</param>
        public Sequence ChainCallback<T>([NotNull] T target, [NotNull] Action<T> callback, bool warnIfTargetDestroyed = true) where T : class
        {
            var maybeDelay = PrimeTweenManager.delayWithoutDurationCheck(target, 0, false);
            if (!maybeDelay.HasValue)
            {
                return this;
            }

            var delay = maybeDelay.Value;
            delay.tween.OnComplete(target, callback, warnIfTargetDestroyed);
            return Chain(delay);
        }

        /// <summary>Schedules delay after all previously added tweens.</summary>
        public Sequence ChainDelay(float _duration, bool useUnscaledTime = false)
        {
            return Chain(Tween.Delay(_duration, null, useUnscaledTime));
        }

        internal Tween GetLongestOrDefault() => isAlive ? getLongest() : default;

        Tween getLongest() => getLongest(out _);

        Tween getLongest(out float durationWithWaitDeps)
        {
            Assert.IsTrue(isAlive);
            Tween result = default;
            float maxDuration = -1;
            foreach (var current in getEnumerator(true))
            {
                Assert.AreNotEqual(-1, current.tween.settings.cycles);
                var _duration = current.tween.calcDurationWithWaitDependencies();
                if (_duration > maxDuration)
                {
                    maxDuration = _duration;
                    result = current;
                }
            }

            Assert.IsTrue(maxDuration >= 0);
            Assert.IsTrue(result.IsCreated);
            durationWithWaitDeps = maxDuration;
            return result;
        }

        Tween getLastInSelf()
        {
            Assert.IsTrue(isAlive);
            Tween result = default;
            foreach (var current in getEnumerator())
            {
                result = current;
            }

            Assert.IsTrue(result.IsCreated);
            Assert.IsFalse(result.tween.nextInSequence.IsCreated);
            return result;
        }

        internal void onTweenKilled(int tweenId)
        {
            Assert.IsTrue(isAlive);
            first.tween.addAliveTweensInSequence(-1, tweenId);
            Assert.IsTrue(first.tween.aliveTweensInSequence >= 0);
            if (first.tween.aliveTweensInSequence > 0)
            {
                return;
            }

            var firstTween = first.tween;
            firstTween.sequenceCyclesDone++;
            Assert.IsTrue(firstTween.sequenceCycles == -1 ||
                          firstTween.sequenceCyclesDone <=
                          firstTween.sequenceCycles); // $"firstTween.sequenceCyclesDone {firstTween.sequenceCyclesDone} <= firstTween.sequenceCycles {firstTween.sequenceCycles}"
            if (firstTween.sequenceCyclesDone == firstTween.sequenceCycles)
            {
                if (parentSequence.IsCreated)
                {
                    Assert.AreEqual(this, parentSequence.childSequence);
                    parentSequence.onTweenKilled(first.id);
                    return;
                }

                // release all tweens in a sequence only after all tweens are completed
                releaseTweens(null);
                Assert.IsFalse(IsCreated); // releaseTweens() sets all ReusableTween.sequence to default, including this one
            }
            else
            {
                restart();
            }
        }

        static void requireFinite(Tween other)
        {
            requireIsAlive(other);
            Assert.IsTrue(other.tween.settings.cycles >= 1, Constants.infiniteTweenInSequenceError);
        }

        void setSequence(Tween handle)
        {
            Assert.IsTrue(IsCreated);
            Assert.IsTrue(handle.isAlive);
            var tween = handle.tween;
            Assert.IsFalse(tween.sequence.IsCreated);
            tween.sequence = this;
            var sequenceIsPaused = isPaused;
            if (tween._isPaused != sequenceIsPaused)
            {
                Debug.LogError(
                    $"{nameof(Tween)}.{nameof(Tween.isPaused)} changed to '{sequenceIsPaused}' after adding to {nameof(Sequence)}. Please use sequence.isPaused to apply the paused state to all tweens in a sequence.");
                tween._isPaused = sequenceIsPaused;
            }

            var sequenceTimeScale = timeScale;
            if (tween.timeScale != sequenceTimeScale)
            {
                Debug.LogError(
                    $"{nameof(Tween)}.{nameof(Tween.timeScale)} changed to '{sequenceTimeScale}' after adding to {nameof(Sequence)}. Please use sequence.timeScale to apply the timeScale to all tweens in a sequence.");
                tween.timeScale = sequenceTimeScale;
            }
        }

        static void validate(Tween other)
        {
            requireIsAlive(other);
            requireFinite(other);
            if (other.tween.sequence.IsCreated)
            {
                throw new Exception($"A tween can be added to a sequence only once and can only belong to one sequence. Tween: {other.tween.GetDescription()}");
            }
        }

        static void requireIsAlive(Tween other)
        {
            Assert.IsTrue(other.isAlive, "It's not allowed to add 'dead' tweens to a sequence.");
        }

        /// Stops all tweens in the Sequence, ignoring callbacks. 
        public void Stop()
        {
            if (isAlive)
            {
                tryRemoveFromSequenceHierarchy();
                releaseTweens(t => t.kill());
                Assert.IsFalse(isAlive);
            }
        }

        /// Immediately completes the current sequence cycle: completes all 'alive' tweens in the Sequence and invokes all remaining callbacks. Remaining sequence cycles are ignored.
        public void Complete()
        {
            if (isAlive)
            {
                tryRemoveFromSequenceHierarchy();
                releaseTweens(t =>
                {
                    if (t.warnIfTargetDestroyed())
                    {
                        t.kill();
                    }
                    else
                    {
                        t.ForceComplete();
                    }
                });
                Assert.IsFalse(isAlive);
            }
        }

        void tryRemoveFromSequenceHierarchy()
        {
            ref var parent = ref parentSequence;
            if (!parent.IsCreated)
            {
                return;
            }

            ref var child = ref childSequence;
            if (!child.IsCreated)
            {
                Assert.AreEqual(this, parent.childSequence);
                parent.childSequence = default;
                parent.onTweenKilled(first.id);
                parent = default;
                Assert.AreEqual(default, parentSequence);
                return;
            }

            Assert.AreEqual(this, child.parentSequence);
            child.parentSequence = parent;
            Assert.AreEqual(this, parent.childSequence);
            parent.childSequence = child;

            child = default;
            Assert.AreEqual(default, childSequence);
            parent = default;
            Assert.AreEqual(default, parentSequence);
        }

        internal void emergencyStop()
        {
            Assert.IsTrue(isAlive);
            releaseTweens(t =>
            {
                t.warnOnCompleteIgnored(false);
                t.kill();
            });
        }

        void releaseTweens([CanBeNull] Action<ReusableTween> killAction)
        {
            Assert.IsTrue(isAlive);
            var cur = this;
            while (cur.IsCreated)
            {
                Assert.IsTrue(cur.isAlive);
                var child = cur.childSequence;
                cur.releaseTweens_internal(killAction);
                cur = child;
            }

            Assert.IsFalse(isAlive); // not IsCreated because this may be a local variable in the user's codebase
        }

        void releaseTweens_internal([CanBeNull] Action<ReusableTween> killAction)
        {
            Assert.IsTrue(isAlive);
            first.tween.aliveTweensInSequence =
                0; // set to 0 here because this method may be called from the OnComplete(). When this happens, ReusableTween is cleared, and onTweenKilled() is never called, leaving aliveTweensInSequence == 1 
            var copy = this; // calling tween.sequence = default will overwrite the sequence on the callsite, so we should copy the initial struct to use the copy in Assert
            var enumerator = getEnumerator();
            var movedNext = enumerator.MoveNext();
            Assert.IsTrue(movedNext);
            var current = enumerator.Current;
            Assert.IsTrue(current.IsCreated);
            while (current.IsCreated)
            {
                var tween = current.tween;
                Assert.AreEqual(copy, tween.sequence);
                if (tween._isAlive)
                {
                    killAction?.Invoke(tween);
                }

                Assert.IsFalse(current.isAlive);
                current = enumerator.MoveNext() ? enumerator.Current : default; // move next before releasing a tween because MoveNext() relies on the nextInSequence
                releaseTween(tween);
            }

            Assert.IsFalse(isAlive); // not IsCreated because this may be a local variable in the releaseTweens() method
        }

        static void releaseTween([NotNull] ReusableTween tween)
        {
            tween.sequenceCycles = 0;
            tween.sequenceCyclesDone = 0;
            Assert.AreNotEqual(0, tween.sequence.id);
            tween.setNextInSequence(null);
            tween.sequence = default;
            tween.parentSequence = default;
            tween.childSequence = default;
        }

        void restart()
        {
            restart_internal();
            var child = childSequence;
            while (child.IsCreated)
            {
                child.restart_internal();
                child.first.tween.sequenceCyclesDone = 0;
                child = child.childSequence;
            }
        }

        void restart_internal()
        {
            Assert.IsTrue(isAlive);
            var buffer = PrimeTweenManager.Instance.buffer;
            Assert.AreEqual(0, buffer.Count);
            first.tween.elapsedTime = 0;
            foreach (var current in getEnumerator())
            {
                var tween = current.tween;
                Assert.IsFalse(tween._isAlive);
                tween.revive();
                first.tween.addAliveTweensInSequence(1, current.id);
                tween.rewindIncrementalTween();
                tween.cyclesDone = 0;
                tween.onCycleComplete();
                buffer.Add(tween);
            }

            for (int i = buffer.Count - 1; i >= 0; i--)
            {
                buffer[i].ReportOnValueChangeIfAnimation(0);
            }

            buffer.Clear();
            var child = childSequence;
            if (child.IsCreated)
            {
                Assert.IsTrue(child.isAlive);
                first.tween.addAliveTweensInSequence(1, child.first.id);
            }

            Assert.IsTrue(isAlive);
        }

        /// <summary>Sets the number of remaining cycles.
        /// This method modifies the <see cref="cyclesTotal"/> so that the sequence will complete after the number of <see cref="cycles"/>.
        /// Setting cycles to -1 will repeat the sequence indefinitely.</summary>
        public Sequence SetCycles(int cycles)
        {
            Assert.IsTrue(IsCreated, Constants.defaultSequenceCtorError);
            Assert.IsTrue(cycles >= -1);
            if (!validateIsAlive())
            {
                return this;
            }

            Assert.IsTrue(isAlive);
            if (cycles == -1)
            {
                first.tween.sequenceCycles = -1;
            }
            else
            {
                TweenSettings.setCyclesTo1If0(ref cycles);
                first.tween.sequenceCycles = first.tween.sequenceCyclesDone + cycles;
            }

            return this;
        }

        public bool isPaused
        {
            get => validateIsAlive() && first.tween._isPaused;
            set
            {
                if (!validateIsAlive())
                {
                    return;
                }

                Assert.IsFalse(parentSequence.IsCreated, Constants.setPauseOnTweenInsideSequenceError);
                if (isPaused == value)
                {
                    return;
                }

                foreach (var tween in getEnumerator(true))
                {
                    tween.tween._isPaused = value;
                }

                Assert.AreEqual(value, isPaused);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Sequence other && Equals(other);
        }

        public bool Equals(Sequence other)
        {
            return id == other.id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        internal SequenceEnumerator getEnumerator(bool includeChildrenSequences = false) => new SequenceEnumerator(this, includeChildrenSequences);

        internal struct SequenceEnumerator
        {
            Sequence sequence;
            Tween current;
            bool isStarted;
            readonly bool includeChildrenSequences;

            internal SequenceEnumerator(Sequence s, bool includeChildrenSequences)
            {
                Assert.IsTrue(s.isAlive);
                sequence = s;
                current = default;
                isStarted = false;
                this.includeChildrenSequences = includeChildrenSequences;
            }

            public
#if UNITY_2020_2_OR_NEWER
                readonly
#endif
                SequenceEnumerator GetEnumerator()
            {
                Assert.IsTrue(sequence.isAlive);
                return this;
            }

            public
#if UNITY_2020_2_OR_NEWER
                readonly
#endif
                Tween Current
            {
                get
                {
#if SAFETY_CHECKS
                    // Assert.IsTrue(sequence.isAlive); // sequence can be already dead because we're currently inside the ReleaseTweens() with more than two tweens. Assertion fail is reproducible with SequenceCompleteWhenMoreThanTwo() test
                    Assert.IsTrue(current.IsCreated);
                    Assert.IsNotNull(current.tween);
                    Assert.AreEqual(current.id, current.tween.id);
                    Assert.AreEqual(current.tween.sequence, sequence);
#endif
                    return current;
                }
            }

            public bool MoveNext()
            {
                if (!isStarted)
                {
                    current = sequence.first;
                    isStarted = true;
                    return true;
                }

                if (!current.IsCreated)
                {
                    return false;
                }

                current = current.tween.nextInSequence;
                if (!current.IsCreated && includeChildrenSequences)
                {
                    var childSequence = sequence.childSequence;
                    if (childSequence.IsCreated)
                    {
                        Assert.IsTrue(childSequence.isAlive);
                        sequence = childSequence;
                        Assert.IsTrue(sequence.first.IsCreated);
                        current = sequence.first;
                        return true;
                    }
                }

                return current.IsCreated;
            }
        }

        ref Sequence parentSequence
        {
            get
            {
                Assert.IsTrue(isAlive);
                return ref first.tween.parentSequence;
            }
        }

        internal ref Sequence childSequence
        {
            get
            {
                Assert.IsTrue(isAlive);
                return ref first.tween.childSequence;
            }
        }

        public Sequence Chain(Sequence other) => nestSequence(other, true);
        public Sequence Group(Sequence other) => nestSequence(other, false);

        Sequence nestSequence(Sequence other, bool isChainOp)
        {
            Assert.IsTrue(IsCreated, Constants.defaultSequenceCtorError);
            if (!validateIsAlive())
            {
                return this;
            }

            Assert.IsTrue(other.isAlive);
            Assert.IsFalse(other.parentSequence.IsCreated, "Sequence can be nested in other sequence only once.");
            var lastChildOrSelf = getLastChildSequenceOrSelf();
            other.parentSequence = lastChildOrSelf;
            other.setWaitDepAndPausedState(isChainOp ? getLongest() : getWaitDep(), isPaused);
            Assert.IsFalse(lastChildOrSelf.childSequence.IsCreated);
            lastChildOrSelf.childSequence = other;
            lastChildOrSelf.first.tween.addAliveTweensInSequence(1, other.first.id);
            return this;
        }

        /// tests: SequenceNestingDepsChain/SequenceNestingDepsGroup
        void setWaitDepAndPausedState(Tween? waitDep, bool isPaused)
        {
            Assert.IsFalse(first.tween.waitFor.IsCreated);
            foreach (var t in getEnumerator(true))
            {
                var tween = t.tween;
                tween._isPaused = isPaused;
                if (waitDep.HasValue && !tween.waitFor.IsCreated)
                {
                    tween.setWaitFor(waitDep.Value);
                }
            }
        }

        Sequence getLastChildSequenceOrSelf()
        {
            var cur = this;
            while (true)
            {
                var child = cur.childSequence;
                if (!child.IsCreated)
                {
                    return cur;
                }

                cur = child;
            }
        }

        /// <summary>Custom timeScale. To smoothly animate timeScale over time, use <see cref="Tween.TweenTimeScale"/> method.</summary>
        public float timeScale
        {
            get => validateIsAlive() ? first.tween.timeScale : 1;
            set
            {
                if (!validateIsAlive())
                {
                    return;
                }

                TweenSettings.clampTimescale(ref value);
                foreach (var tween in getEnumerator(true))
                {
                    tween.tween.timeScale = value;
                }
            }
        }
    }
}