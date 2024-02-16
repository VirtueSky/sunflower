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

        public static float Duration(this SkeletonGraphic skeletonGraphic, string animationName)
        {
            var animation =
                skeletonGraphic.AnimationState.Data.SkeletonData.Animations.Items.FirstOrDefault(animation =>
                    animation.Name.Equals(animationName));
            if (animation == null) return 0;
            return animation.Duration;
        }

        public static SkeletonGraphic OnComplete(this SkeletonGraphic skeletonGraphic, Action onComplete)
        {
            Tween.Delay(skeletonGraphic.Duration(skeletonGraphic.startingAnimation), onComplete);
            return skeletonGraphic;
        }


        public static SkeletonGraphic Play(this SkeletonGraphic skeletonGraphic, string animationName,
            bool loop = false)
        {
            skeletonGraphic.Clear();
            skeletonGraphic.startingAnimation = animationName;
            skeletonGraphic.startingLoop = loop;
            skeletonGraphic.AnimationState.SetAnimation(0, animationName, loop);
            skeletonGraphic.LateUpdate();
            skeletonGraphic.Initialize(true);
            return skeletonGraphic;
        }

        public static SkeletonGraphic PlayOnly(this SkeletonGraphic skeletonGraphic, string animationName,
            bool loop = false)
        {
            skeletonGraphic.AnimationState.SetAnimation(0, animationName, loop);
            return skeletonGraphic;
        }

        public static SkeletonGraphic SetSkin(this SkeletonGraphic skeletonGraphic, string skinName)
        {
            var skin = new Skin("temp");
            skin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin(skinName));
            skeletonGraphic.Skeleton.SetSkin(skin);
            skeletonGraphic.Skeleton.SetSlotsToSetupPose();
            skeletonGraphic.LateUpdate();
            skeletonGraphic.AnimationState.Apply(skeletonGraphic.Skeleton);
            return skeletonGraphic;
        }

        public static SkeletonGraphic SetSkin(this SkeletonGraphic skeletonGraphic, List<string> skinNames)
        {
            var skin = new Skin("temp");
            var skeletonData = skeletonGraphic.Skeleton.Data;
            foreach (string skinName in skinNames)
            {
                skin.AddSkin(skeletonData.FindSkin(skinName));
            }

            skeletonGraphic.initialSkinName = "temp";
            skeletonGraphic.Skeleton.SetSkin(skin);
            skeletonGraphic.Skeleton.SetSlotsToSetupPose();
            skeletonGraphic.LateUpdate();
            skeletonGraphic.AnimationState.Apply(skeletonGraphic.Skeleton);

            return skeletonGraphic;
        }

        public static SkeletonGraphic ChangeAttachment(this SkeletonGraphic skeletonGraphic, string slotName,
            string attachmentName)
        {
            var slotIndex = skeletonGraphic.Skeleton.Data.FindSlot(slotName).Index;
            var attachment = skeletonGraphic.Skeleton.GetAttachment(slotIndex, attachmentName);
            var skin = new Skin("temp");
            skin.SetAttachment(slotIndex, slotName, attachment);
            skeletonGraphic.Skeleton.SetSkin(skin);
            skeletonGraphic.Skeleton.SetSlotsToSetupPose();
            skeletonGraphic.LateUpdate();
            return skeletonGraphic;
        }

        public static SkeletonGraphic ChangeAttachment(this SkeletonGraphic skeletonGraphic, string slotName,
            List<string> attachmentNames)
        {
            var slotIndex = skeletonGraphic.Skeleton.Data.FindSlot(slotName).Index;
            var skin = new Skin("temp");
            foreach (var attachmentName in attachmentNames)
            {
                var attachment = skeletonGraphic.Skeleton.GetAttachment(slotIndex, attachmentName);
                skin.SetAttachment(slotIndex, slotName, attachment);
            }

            skeletonGraphic.Skeleton.SetSkin(skin);
            skeletonGraphic.Skeleton.SetSlotsToSetupPose();
            skeletonGraphic.LateUpdate();
            return skeletonGraphic;
        }

        public static SkeletonGraphic MixSkin(this SkeletonGraphic skeletonGraphic, string mixSkinName)
        {
            Skin skin = new Skin("temp");
            skin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin(mixSkinName));
            skeletonGraphic.Skeleton.SetSkin(skin);
            skeletonGraphic.Skeleton.SetSlotsToSetupPose();
            skeletonGraphic.AnimationState.Apply(skeletonGraphic.Skeleton);
            return skeletonGraphic;
        }

        public static SkeletonGraphic MixSkin(this SkeletonGraphic skeletonGraphic, List<string> mixSkinNames)
        {
            Skin skin = new Skin("temp");
            foreach (var mixSkinName in mixSkinNames)
            {
                skin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin(mixSkinName));
            }

            skeletonGraphic.Skeleton.SetSkin(skin);
            skeletonGraphic.Skeleton.SetSlotsToSetupPose();
            skeletonGraphic.AnimationState.Apply(skeletonGraphic.Skeleton);
            return skeletonGraphic;
        }

    }
}
#endif