#if VIRTUESKY_SKELETON
using System;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using AnimationState = Spine.AnimationState;

namespace VirtueSky.Component
{
    public abstract class AnimationSkeleton : MonoBehaviour
    {
        protected Skeleton skeleton;
        protected AnimationState animationState;
        protected string animationName;
        public Skeleton Skeleton => skeleton;
        public AnimationState AnimationState => animationState;
        public string AnimationName => animationName;
        protected Dictionary<string, Action> cacheEvent = new Dictionary<string, Action>();

        public virtual void Init()
        {
            animationState.Event += HandleAnimationStateEvent;
        }

        public abstract void Initialize(bool reload = false);
        public abstract void ChangeAnimationName(string animationName);
        public abstract void FlipX(bool isFlipX = false);
        public abstract void FlipY(bool isFlipY = false);
        public abstract void ChangeDataAsset(SkeletonDataAsset dataAsset);

        public void AddAnimation(int trackIndex, string animationName, bool loop, float timeDelay = 0)
        {
            animationState.AddAnimation(trackIndex, animationName, loop, timeDelay);
        }

        public TrackEntry PlayAnimation(int trackIndex, string animationName, bool loop = false,
            float speed = 1)
        {
            this.animationName = animationName;
            animationState.TimeScale = speed;
            var trackEntry = animationState.SetAnimation(trackIndex, animationName, loop);
            animationState.Apply(skeleton);
            return trackEntry;
        }

        public void RegisterEvent(string eventName, Action actionEvent = null)
        {
            if (cacheEvent.ContainsKey(eventName))
            {
                cacheEvent[eventName] = actionEvent;
            }
            else
            {
                cacheEvent.Add(eventName, actionEvent);
            }
        }

        public void StopAnimation()
        {
            animationState.TimeScale = 0;
        }

        protected void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
        {
            Action action = null;
            if (cacheEvent.TryGetValue(e.Data.Name, out action))
            {
                action?.Invoke();
            }
        }
    }
}
#endif