using System;
using VirtueSky.Inspector;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class IronSourceBannerVariable : AdUnitVariable
    {
        public override void Init()
        {
        }

        public override void Load()
        {
            throw new NotImplementedException();
        }

        public override bool IsReady()
        {
            throw new NotImplementedException();
        }

        protected override void ShowImpl()
        {
            throw new NotImplementedException();
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}