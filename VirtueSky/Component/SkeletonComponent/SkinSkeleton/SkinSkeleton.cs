#if VIRTUESKY_SKELETON
using System.Collections.Generic;
using Spine;
using UnityEngine;
using AnimationState = Spine.AnimationState;

namespace VirtueSky.Component
{
    public abstract class SkinSkeleton : MonoBehaviour
    {
        protected Skeleton skeleton;
        protected Spine.AnimationState animationState;
        public Skeleton Skeleton => skeleton;
        public AnimationState AnimationState => animationState;

        public abstract void Init();

        public virtual void MixSkin(string mixSkinName)
        {
            if (skeleton == null) return;
            if (string.IsNullOrEmpty(mixSkinName) || mixSkinName.Equals("default")) return;
            skeleton.Skin.AddSkin(skeleton.Data.FindSkin(mixSkinName));
            skeleton.SetSlotsToSetupPose();
            animationState.Apply(skeleton);
        }

        public virtual void MixSkin(List<string> listMixSkinName)
        {
            if (skeleton == null) return;
            foreach (var skinMixName in listMixSkinName)
            {
                if (!string.IsNullOrEmpty(skinMixName) && !skinMixName.Equals("default"))
                {
                    skeleton.Skin.AddSkin(skeleton.Data.FindSkin(skinMixName));
                }
            }

            skeleton.SetSlotsToSetupPose();
            animationState.Apply(skeleton);
        }

        public virtual void MixNewSkin(string mixSkinName)
        {
            if (skeleton == null) return;
            if (string.IsNullOrEmpty(mixSkinName) || mixSkinName.Equals("default")) return;
            var mixAndMatchSkin = new Skin("temp");
            mixAndMatchSkin.AddSkin(skeleton.Data.FindSkin(mixSkinName));
            skeleton.SetSkin(mixAndMatchSkin);
            skeleton.SetSlotsToSetupPose();
            animationState.Apply(skeleton);
        }

        public virtual void MixNewSkin(List<string> listMixSkinName)
        {
            if (skeleton == null) return;
            var mixAndMatchSkin = new Skin("temp");
            foreach (var skinMixName in listMixSkinName)
            {
                if (!string.IsNullOrEmpty(skinMixName) && !skinMixName.Equals("default"))
                {
                    mixAndMatchSkin.AddSkin(skeleton.Data.FindSkin(skinMixName));
                }
            }

            skeleton.SetSkin(mixAndMatchSkin);
            skeleton.SetSlotsToSetupPose();
            animationState.Apply(skeleton);
        }

        public virtual void SetSkin(string skinName)
        {
            if (skeleton == null) return;
            Skin newSkin = new Skin("skin");
            newSkin.AddSkin(skeleton.Data.FindSkin(skinName));
            skeleton.SetSkin(newSkin);
            skeleton.SetSlotsToSetupPose();
            animationState.Apply(skeleton);
        }
    }
}

#endif