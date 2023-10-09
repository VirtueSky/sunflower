using System;
using UnityEngine;
using VirtueSky.DataStorage;

namespace VirtueSky.Ads
{
    public static class AdStatic
    {
        public static bool IsRemoveAd
        {
            get => GameData.Get($"{Application.identifier}_removeads", false);
            set => GameData.Set($"{Application.identifier}_removeads", value);
        }

        internal static bool isShowingAd;

        public static AdUnitVariable OnDisplayed(this AdUnitVariable unit, Action onDisplayed)
        {
            unit.displayedCallback = onDisplayed;
            return unit;
        }

        public static AdUnitVariable OnClosed(this AdUnitVariable unit, Action onClosed)
        {
            unit.closedCallback = onClosed;
            return unit;
        }

        public static AdUnitVariable OnLoaded(this AdUnitVariable unit, Action onLoaded)
        {
            unit.loadedCallback = onLoaded;
            return unit;
        }

        public static AdUnitVariable OnFailedToLoad(this AdUnitVariable unit, Action onFailedToLoad)
        {
            unit.failedToLoadCallback = onFailedToLoad;
            return unit;
        }

        public static AdUnitVariable OnFailedToDisplay(this AdUnitVariable unit, Action onFailedToDisplay)
        {
            unit.failedToDisplayCallback = onFailedToDisplay;
            return unit;
        }

        public static AdUnitVariable OnCompleted(this AdUnitVariable unit, Action onCompleted)
        {
            switch (unit)
            {
                case AdmobInterVariable inter:

                    return unit;
                case AdmobRewardVariable reward:

                    return unit;
                case AdmobRewardInterVariable rewardInter:

                    return unit;
                case MaxInterVariable maxInter:
                    maxInter.completedCallback = onCompleted;
                    return unit;
                case MaxRewardVariable maxReward:
                    maxReward.completedCallback = onCompleted;
                    return unit;
                case MaxRewardInterVariable maxRewardInter:
                    maxRewardInter.completedCallback = onCompleted;
                    return unit;
            }

            return unit;
        }

        public static AdUnitVariable OnSkipped(this AdUnitVariable unit, Action onSkipped)
        {
            switch (unit)
            {
                case AdmobRewardVariable reward:

                    return unit;
                case AdmobRewardInterVariable rewardInter:

                    return unit;
                case MaxRewardVariable maxReward:
                    maxReward.skippedCallback = onSkipped;
                    return unit;
                case MaxRewardInterVariable maxRewardInter:
                    maxRewardInter.skippedCallback = onSkipped;
                    return unit;
            }

            return unit;
        }
    }
}