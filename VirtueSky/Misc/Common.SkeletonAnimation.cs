using System;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;

#if VIRTUESKY_SKELETON
using Spine;
using Spine.Unity;

namespace VirtueSky.Misc
{
    public static partial class Common
    {
        public static float Duration(this SkeletonAnimation skeletonAnimation, string animationName)
        {
            var animation =
                skeletonAnimation.AnimationState.Data.SkeletonData.Animations.Items.FirstOrDefault(animation =>
                    animation.Name.Equals(animationName));
            if (animation == null) return 0;
            return animation.Duration;
        }

        public static SkeletonAnimation OnComplete(this SkeletonAnimation skeletonAnimation, Action onComplete)
        {
            Tween.Delay(skeletonAnimation.Duration(skeletonAnimation.AnimationName), onComplete);
            return skeletonAnimation;
        }


        public static SkeletonAnimation Play(this SkeletonAnimation skeletonAnimation, string animationName,
            bool loop = false)
        {
            skeletonAnimation.ClearState();
            skeletonAnimation.AnimationName = animationName;
            skeletonAnimation.loop = loop;
            skeletonAnimation.LateUpdate();
            skeletonAnimation.Initialize(true);
            return skeletonAnimation;
        }

        public static SkeletonAnimation PlayOnly(this SkeletonAnimation skeletonAnimation, string animationName,
            bool loop = false)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);
            return skeletonAnimation;
        }

        public static SkeletonAnimation AddAnimation(this SkeletonAnimation skeletonAnimation, int trackIndex,
            string animationName, bool loop, float timeDelay = 0)
        {
            skeletonAnimation.AnimationState.AddAnimation(trackIndex, animationName, loop, timeDelay);
            return skeletonAnimation;
        }

        public static SkeletonAnimation SetSkin(this SkeletonAnimation skeletonAnimation, string skinName)
        {
            var skin = new Skin("temp");
            skin.AddSkin(skeletonAnimation.skeleton.Data.FindSkin(skinName));
            skeletonAnimation.initialSkinName = "temp";
            skeletonAnimation.skeleton.SetSkin(skin);
            skeletonAnimation.skeleton.SetSlotsToSetupPose();
            skeletonAnimation.LateUpdate();
            skeletonAnimation.AnimationState.Apply(skeletonAnimation.skeleton);

            return skeletonAnimation;
        }

        public static SkeletonAnimation SetSkin(this SkeletonAnimation skeletonAnimation, List<string> skinNames)
        {
            var skin = new Skin("temp");
            var skeletonData = skeletonAnimation.Skeleton.Data;
            foreach (string skinName in skinNames)
            {
                skin.AddSkin(skeletonData.FindSkin(skinName));
            }

            skeletonAnimation.initialSkinName = "temp";
            skeletonAnimation.skeleton.SetSkin(skin);
            skeletonAnimation.skeleton.SetSlotsToSetupPose();
            skeletonAnimation.LateUpdate();
            skeletonAnimation.AnimationState.Apply(skeletonAnimation.skeleton);

            return skeletonAnimation;
        }

        public static SkeletonAnimation ChangeAttachment(this SkeletonAnimation skeletonAnimation, string slotName,
            string attachmentName)
        {
            var slotIndex = skeletonAnimation.skeleton.Data.FindSlot(slotName).Index;
            var attachment = skeletonAnimation.skeleton.GetAttachment(slotIndex, attachmentName);
            var skin = new Skin("temp");
            skin.SetAttachment(slotIndex, slotName, attachment);
            skeletonAnimation.skeleton.SetSkin(skin);
            skeletonAnimation.skeleton.SetSlotsToSetupPose();
            skeletonAnimation.LateUpdate();
            return skeletonAnimation;
        }

        public static SkeletonAnimation ChangeAttachment(this SkeletonAnimation skeletonAnimation, string slotName,
            List<string> attachmentNames)
        {
            var slotIndex = skeletonAnimation.skeleton.Data.FindSlot(slotName).Index;
            var skin = new Skin("temp");
            foreach (var attachmentName in attachmentNames)
            {
                var attachment = skeletonAnimation.skeleton.GetAttachment(slotIndex, attachmentName);
                skin.SetAttachment(slotIndex, slotName, attachment);
            }

            skeletonAnimation.skeleton.SetSkin(skin);
            skeletonAnimation.skeleton.SetSlotsToSetupPose();
            skeletonAnimation.LateUpdate();
            return skeletonAnimation;
        }

        public static SkeletonAnimation MixSkin(this SkeletonAnimation skeletonAnimation, string mixSkinName)
        {
            Skin skin = new Skin("temp");
            skin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin(mixSkinName));
            skeletonAnimation.Skeleton.SetSkin(skin);
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton);
            return skeletonAnimation;
        }

        public static SkeletonAnimation MixSkin(this SkeletonAnimation skeletonAnimation, List<string> mixSkinNames)
        {
            Skin skin = new Skin("temp");
            foreach (var mixSkinName in mixSkinNames)
            {
                skin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin(mixSkinName));
            }

            skeletonAnimation.Skeleton.SetSkin(skin);
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton);
            return skeletonAnimation;
        }
    }
}
#endif