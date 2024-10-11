using UnityEngine;

namespace VirtueSky.Ads
{
    public class IronSourceAdUnitVariable : AdUnitVariable
    {
        public override AdUnitVariable Show()
        {
            ResetChainCallback();
            if (!Application.isMobilePlatform || AdStatic.IsRemoveAd) return this;
            ShowImpl();
            return this;
        }
    }
}