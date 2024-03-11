#if VIRTUESKY_SKELETON
using Spine.Unity;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Component
{
    [RequireComponent(typeof(SkeletonGraphic))]
    [EditorIcon("icon_csharp")]
    public class AnimationSkeletonUIComponent : AnimationSkeleton
    {
        [SerializeField] SkeletonGraphic skeletonGraphic;
        public SkeletonGraphic SkeletonGraphic => skeletonGraphic;

        public override void Init()
        {
            skeleton = skeletonGraphic.Skeleton;
            animationState = skeletonGraphic.AnimationState;
            base.Init();
        }

        public override void Initialize(bool reload = false)
        {
            skeletonGraphic.Initialize(reload);
        }

        public override void ChangeAnimationName(string animationName)
        {
            skeletonGraphic.startingAnimation = animationName;
        }

        public override void FlipX(bool isFlipX = false)
        {
            skeletonGraphic.initialFlipX = isFlipX;
        }

        public override void FlipY(bool isFlipY = false)
        {
            skeletonGraphic.initialFlipY = isFlipY;
        }

        public override void ChangeDataAsset(SkeletonDataAsset dataAsset)
        {
            skeletonGraphic.skeletonDataAsset = dataAsset;
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