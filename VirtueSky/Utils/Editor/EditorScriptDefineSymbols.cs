#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

namespace VirtueSky.UtilsEditor
{
    public class EditorScriptDefineSymbols : EditorWindow
    {
        private const string defaultMenuPath = "Sunflower/ScriptDefineSymbols/";

        // #region Ads
        //
        // private const string menuPathAds = defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_ADS;
        //
        // [MenuItem(menuPathAds)]
        // public static void AdsConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_ADS);
        // }
        //
        // [MenuItem(menuPathAds, true)]
        // public static bool IsAdsConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathAds, IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_ADS));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region Applovin
        //
        // private const string menuPathApplovin = defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_APPLOVIN;
        //
        // [MenuItem(menuPathApplovin)]
        // public static void ApplovinConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_APPLOVIN);
        // }
        //
        // [MenuItem(menuPathApplovin, true)]
        // public static bool IsApplovinConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathApplovin, IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_APPLOVIN));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region Admob
        //
        // private const string menuPathAdmob = defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_ADMOB;
        //
        // [MenuItem(menuPathAdmob)]
        // public static void AdmobConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_ADMOB);
        // }
        //
        // [MenuItem(menuPathAdmob, true)]
        // public static bool IsAdmobConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathAdmob, IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_ADMOB));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region Adjust
        //
        // private const string menuPathAdjust = defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_ADJUST;
        //
        // [MenuItem(menuPathAdjust)]
        // public static void AdjustConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_ADJUST);
        // }
        //
        // [MenuItem(menuPathAdjust, true)]
        // public static bool IsAdjustConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathAdjust, IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_ADJUST));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region Firebase Analytics
        //
        // private const string menuPathAnalytic = defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_FIREBASE_ANALYTIC;
        //
        // [MenuItem(menuPathAnalytic)]
        // public static void AnalyticConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_FIREBASE_ANALYTIC);
        // }
        //
        // [MenuItem(menuPathAnalytic, true)]
        // public static bool IsAnalyticConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathAnalytic, IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_FIREBASE_ANALYTIC));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region Firebase Remote Config
        //
        // private const string menuPathRemoteConfig =
        //     defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_FIREBASE_REMOTECONFIG;
        //
        // [MenuItem(menuPathRemoteConfig)]
        // public static void RemoteConfigConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_FIREBASE_REMOTECONFIG);
        // }
        //
        // [MenuItem(menuPathRemoteConfig, true)]
        // public static bool IsRemoteConfigConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathRemoteConfig, IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_FIREBASE_REMOTECONFIG));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region Firebase App
        //
        // private const string menuPathFirebaseApp = defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_FIREBASE;
        //
        // [MenuItem(menuPathFirebaseApp)]
        // public static void FirebaseAppConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_FIREBASE);
        // }
        //
        // [MenuItem(menuPathFirebaseApp, true)]
        // public static bool IsFirebaseAppConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathFirebaseApp, IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_FIREBASE));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region Iap
        //
        // private const string menuPathIAP = defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_IAP;
        //
        // [MenuItem(menuPathIAP)]
        // public static void IapConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_IAP);
        // }
        //
        // [MenuItem(menuPathIAP, true)]
        // public static bool IsIapFlagEnable()
        // {
        //     Menu.SetChecked(menuPathIAP, IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_IAP));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region Ratting
        //
        // private const string menuPathRatting = defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_RATING;
        //
        // [MenuItem(menuPathRatting)]
        // public static void RattingConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_RATING);
        // }
        //
        // [MenuItem(menuPathRatting, true)]
        // public static bool IsRattingConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathRatting, IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_RATING));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region Notification
        //
        // private const string menuPathNotification = defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_NOTIFICATION;
        //
        // [MenuItem(menuPathNotification)]
        // public static void NotificationConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_NOTIFICATION);
        // }
        //
        // [MenuItem(menuPathNotification, true)]
        // public static bool IsNotificationConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathNotification, IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_NOTIFICATION));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region AppsFlyer
        //
        // private const string menuPathAppsFlyer = defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_APPSFLYER;
        //
        // [MenuItem(menuPathAppsFlyer)]
        // public static void AppsFlyerConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_APPSFLYER);
        // }
        //
        // [MenuItem(menuPathAppsFlyer, true)]
        // public static bool IsAppsFlyerConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathAppsFlyer, IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_APPSFLYER));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region PRIME_TWEEN_DOTWEEN_ADAPTER
        //
        // private const string menuPathPrimeTweenDotweenAdapter =
        //     defaultMenuPath + ConstantDefineSymbols.PRIME_TWEEN_DOTWEEN_ADAPTER;
        //
        // [MenuItem(menuPathPrimeTweenDotweenAdapter)]
        // public static void PrimeTweenDoTweenAdapterConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.PRIME_TWEEN_DOTWEEN_ADAPTER);
        // }
        //
        // [MenuItem(menuPathPrimeTweenDotweenAdapter, true)]
        // public static bool IsPrimeTweenDoTweenAdapterConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathPrimeTweenDotweenAdapter,
        //         IsFlagEnabled(ConstantDefineSymbols.PRIME_TWEEN_DOTWEEN_ADAPTER));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region GPGS
        //
        // private const string menuPathGPGS =
        //     defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_GPGS;
        //
        // [MenuItem(menuPathGPGS)]
        // public static void GPGSConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_GPGS);
        // }
        //
        // [MenuItem(menuPathGPGS, true)]
        // public static bool IsGPGSConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathGPGS,
        //         IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_GPGS));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region Apple Auth
        //
        // private const string menuPathAppleAuth =
        //     defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_APPLE_AUTH;
        //
        // [MenuItem(menuPathAppleAuth)]
        // public static void AppleAuthConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_APPLE_AUTH);
        // }
        //
        // [MenuItem(menuPathAppleAuth, true)]
        // public static bool IsAppleAuthConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathAppleAuth,
        //         IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_APPLE_AUTH));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region Skeleton
        //
        // private const string menuPathSkeleton =
        //     defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_SKELETON;
        //
        // [MenuItem(menuPathSkeleton)]
        // public static void SkeletonConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_SKELETON);
        // }
        //
        // [MenuItem(menuPathSkeleton, true)]
        // public static bool IsSkeletonConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathAppleAuth,
        //         IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_SKELETON));
        //     return true;
        // }
        //
        // #endregion
        //
        // #region Animancer
        //
        // private const string menuPathAnimancer =
        //     defaultMenuPath + ConstantDefineSymbols.VIRTUESKY_ANIMANCER;
        //
        // [MenuItem(menuPathAnimancer)]
        // public static void AnimancerConfigFlag()
        // {
        //     SwitchFlag(ConstantDefineSymbols.VIRTUESKY_ANIMANCER);
        // }
        //
        // [MenuItem(menuPathAnimancer, true)]
        // public static bool IsAnimancerConfigFlagEnable()
        // {
        //     Menu.SetChecked(menuPathAppleAuth,
        //         IsFlagEnabled(ConstantDefineSymbols.VIRTUESKY_ANIMANCER));
        //     return true;
        // }
        //
        // #endregion

        #region Base Functions

        public static void SwitchFlag(string flag)
        {
            PlayerSettings.GetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                out var defines);
            var enabled = defines.Contains(flag);
            defines = enabled
                ? defines.Where(value => value != flag).ToArray()
                : defines.Append(flag).ToArray();
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup, defines);
        }

        public static bool IsFlagEnabled(string flag)
        {
            PlayerSettings.GetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                out var defines);
            return defines.Contains(flag);
        }

        #endregion
    }
}
#endif