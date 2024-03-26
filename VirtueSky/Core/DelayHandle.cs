using System;
using UnityEngine;

namespace VirtueSky.Core
{
    public class DelayHandle
    {
        /// <summary>
        /// How long the timer takes to complete from start to finish.
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// Whether the timer will run again after completion.
        /// </summary>
        public bool IsLooped { get; set; }

        /// <summary>
        /// Whether or not the timer completed running. This is false if the timer was cancelled.
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Whether the timer uses real-time or game-time. Real time is unaffected by changes to the timescale
        /// of the game(e.g. pausing, slow-mo), while game time is affected.
        /// </summary>
        public bool UsesRealTime { get; private set; }

        /// <summary>
        /// Whether the timer is currently paused.
        /// </summary>
        public bool IsPaused => _timeElapsedBeforePause.HasValue;

        /// <summary>
        /// Whether or not the timer was cancelled.
        /// </summary>
        public bool IsCancelled => _timeElapsedBeforeCancel.HasValue;

        /// <summary>
        /// Get whether or not the timer has finished running for any reason.
        /// </summary>
        public bool IsDone => IsCompleted || IsCancelled || IsOwnerDestroyed;


        private bool IsOwnerDestroyed => _hasAutoDestroyOwner && _autoDestroyOwner == null;

        private readonly Action _onComplete;
        private readonly Action<float> _onUpdate;
        private float _startTime;
        private float _lastUpdateTime;

        // for pausing, we push the start time forward by the amount of time that has passed.
        // this will mess with the amount of time that elapsed when we're cancelled or paused if we just
        // check the start time versus the current world time, so we need to cache the time that was elapsed
        // before we paused/cancelled
        private float? _timeElapsedBeforeCancel;
        private float? _timeElapsedBeforePause;

        // after the auto destroy owner is destroyed, the timer will expire
        // this way you don't run into any annoying bugs with timers running and accessing objects
        // after they have been destroyed
        private readonly MonoBehaviour _autoDestroyOwner;
        private readonly bool _hasAutoDestroyOwner;


        /// <summary>
        /// Stop a timer that is in-progress or paused. The timer's on completion callback will not be called.
        /// </summary>
        public void Cancel()
        {
            if (IsDone)
            {
                return;
            }

            _timeElapsedBeforeCancel = GetTimeElapsed();
            _timeElapsedBeforePause = null;
        }

        /// <summary>
        /// Pause a running timer. A paused timer can be resumed from the same point it was paused.
        /// </summary>
        public void Pause()
        {
            if (IsPaused || IsDone)
            {
                return;
            }

            _timeElapsedBeforePause = GetTimeElapsed();
        }

        /// <summary>
        /// Continue a paused timer. Does nothing if the timer has not been paused.
        /// </summary>
        public void Resume()
        {
            if (!IsPaused || IsDone)
            {
                return;
            }

            _timeElapsedBeforePause = null;
        }

        /// <summary>
        /// Get how many seconds have elapsed since the start of this timer's current cycle.
        /// </summary>
        /// <returns>The number of seconds that have elapsed since the start of this timer's current cycle, i.e.
        /// the current loop if the timer is looped, or the start if it isn't.
        ///
        /// If the timer has finished running, this is equal to the duration.
        ///
        /// If the timer was cancelled/paused, this is equal to the number of seconds that passed between the timer
        /// starting and when it was cancelled/paused.</returns>
        public float GetTimeElapsed()
        {
            if (IsCompleted || GetWorldTime() >= GetFireTime())
            {
                return Duration;
            }

            return _timeElapsedBeforeCancel ?? _timeElapsedBeforePause ?? GetWorldTime() - _startTime;
        }

        /// <summary>
        /// Get how many seconds remain before the timer completes.
        /// </summary>
        /// <returns>The number of seconds that remain to be elapsed until the timer is completed. A timer
        /// is only elapsing time if it is not paused, cancelled, or completed. This will be equal to zero
        /// if the timer completed.</returns>
        public float GetTimeRemaining()
        {
            return Duration - GetTimeElapsed();
        }

        /// <summary>
        /// Get how much progress the timer has made from start to finish as a ratio.
        /// </summary>
        /// <returns>A value from 0 to 1 indicating how much of the timer's duration has been elapsed.</returns>
        public float GetRatioComplete()
        {
            return GetTimeElapsed() / Duration;
        }

        /// <summary>
        /// Get how much progress the timer has left to make as a ratio.
        /// </summary>
        /// <returns>A value from 0 to 1 indicating how much of the timer's duration remains to be elapsed.</returns>
        public float GetRatioRemaining()
        {
            return GetTimeRemaining() / Duration;
        }


        internal DelayHandle(float duration, Action onComplete, Action<float> onUpdate, bool isLooped,
            bool usesRealTime, MonoBehaviour autoDestroyOwner)
        {
            Duration = duration;
            _onComplete = onComplete;
            _onUpdate = onUpdate;

            IsLooped = isLooped;
            UsesRealTime = usesRealTime;

            _autoDestroyOwner = autoDestroyOwner;
            _hasAutoDestroyOwner = autoDestroyOwner != null;

            _startTime = GetWorldTime();
            _lastUpdateTime = _startTime;
        }

        private float GetWorldTime()
        {
            return UsesRealTime ? Time.realtimeSinceStartup : Time.time;
        }

        private float GetFireTime()
        {
            return _startTime + Duration;
        }

        private float GetTimeDelta()
        {
            return GetWorldTime() - _lastUpdateTime;
        }

        internal void Update()
        {
            if (IsDone) return;

            if (IsPaused)
            {
                _startTime += GetTimeDelta();
                _lastUpdateTime = GetWorldTime();
                return;
            }

            _lastUpdateTime = GetWorldTime();
            _onUpdate?.Invoke(GetTimeElapsed());

            if (GetWorldTime() >= GetFireTime())
            {
                _onComplete?.Invoke();

                if (IsLooped) _startTime = GetWorldTime();
                else IsCompleted = true;
            }
        }
    }
}