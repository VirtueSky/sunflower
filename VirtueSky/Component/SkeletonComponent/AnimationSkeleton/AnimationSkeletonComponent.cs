#if VIRTUESKY_SKELETON
using Spine.Unity;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Component
{
    [RequireComponent(typeof(SkeletonAnimation))]
    [EditorIcon("icon_csharp"), HideMonoScript]
    public class AnimationSkeletonComponent : AnimationSkeleton
    {
        [SerializeField] SkeletonAnimation skeletonAnimation;
        public SkeletonAnimation SkeletonAnimation => skeletonAnimation;

        public override void Init()
        {
            skeleton = skeletonAnimation.Skeleton;
            animationState = skeletonAnimation.AnimationState;
            base.Init();
        }

        public override void Initialize(bool reload = false)
        {
            skeletonAnimation.Initialize(reload);
        }

        public override void ChangeAnimationName(string animationName)
        {
            skeletonAnimation.AnimationName = animationName;
        }

        public override void FlipX(bool isFlipX = false)
        {
            skeletonAnimation.initialFlipX = isFlipX;
        }

        public override void FlipY(bool isFlipY = false)
        {
            skeletonAnimation.initialFlipY = isFlipY;
        }

        public override void ChangeDataAsset(SkeletonDataAsset dataAsset)
        {
            skeletonAnimation.skeletonDataAsset = dataAsset;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (skeletonAnimation == null)
            {
                skeletonAnimation = GetComponent<SkeletonAnimation>();
            }
        }
#endif
    }
}
#endif