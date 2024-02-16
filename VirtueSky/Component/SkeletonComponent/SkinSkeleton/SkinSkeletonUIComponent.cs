#if VIRTUESKY_SKELETON
using System;
using Spine.Unity;
using UnityEngine;

namespace VirtueSky.Component
{
    [RequireComponent(typeof(SkeletonGraphic))]
    public class SkinSkeletonUIComponent : SkinSkeleton
    {
        [SerializeField] private SkeletonGraphic skeletonGraphic;

        public override void Init()
        {
            skeleton = skeletonGraphic.Skeleton;
            animationState = skeletonGraphic.AnimationState;
        }
#if UNITY_EDITOR
        private void Reset()
        {
            if (skeletonGraphic == null)
            {
                skeletonGraphic = GetComponent<SkeletonGraphic>();
            }
        }
#endif
    }
}
#endif