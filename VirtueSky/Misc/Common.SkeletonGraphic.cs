using System;
using System.Collections.Generic;
using System.Linq;
#if VIRTUESKY_SKELETON
using Spine;
using Spine.Unity;
using UnityEngine;
using VirtueSky.Core;

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

        public static float Duration(this SkeletonGraphic skeletonGraphic, int track = 0)
        {
            var animation = skeletonGraphic.AnimationState.GetCurrent(track);
            if (animation == null) return 0;
            return animation.Animation.Duration;
        }

        public static SkeletonGraphic OnComplete(this SkeletonGraphic skeletonGraphic, Action onComplete,
            int trackIndex = 0, MonoBehaviour target = null)
        {
            App.Delay(target, skeletonGraphic.Duration(trackIndex), () =>
            {
                if (skeletonGraphic != null)
                {
                    onComplete?.Invoke();
                }
            });
            return skeletonGraphic;
        }


        public static SkeletonGraphic OnUpdate(this SkeletonGraphic skeletonGraphic, Action<float> onUpdate,
            int trackIndex = 0, MonoBehaviour target = null)
        {
            App.Delay(target, skeletonGraphic.Duration(trackIndex), null, onUpdate);
            return skeletonGraphic;
        }


        public static SkeletonGraphic Play(this SkeletonGraphic skeletonGraphic, string animationName,
            bool loop = false, int trackIndex = 0)
        {
            skeletonGraphic.Clear();
            skeletonGraphic.startingAnimation = animationName;
            skeletonGraphic.startingLoop = loop;
            skeletonGraphic.AnimationState.SetAnimation(trackIndex, animationName, loop);
            skeletonGraphic.LateUpdate();
            skeletonGraphic.Initialize(true);
            return skeletonGraphic;
        }

        public static SkeletonGraphic PlayOnly(this SkeletonGraphic skeletonGraphic, string animationName,
            bool loop = false, int trackIndex = 0)
        {
            skeletonGraphic.startingAnimation = animationName;
            skeletonGraphic.AnimationState.SetAnimation(trackIndex, animationName, loop);
            return skeletonGraphic;
        }

        public static SkeletonGraphic AddAnimation(this SkeletonGraphic skeletonGraphic, int trackIndex,
            string animationName, bool loop, float timeDelay = 0f, float mixDuration = 0f)
        {
            var track = skeletonGraphic.AnimationState.AddAnimation(trackIndex, animationName, loop, timeDelay);
            track.MixDuration = mixDuration;
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