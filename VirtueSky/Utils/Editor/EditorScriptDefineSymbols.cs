#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

namespace VirtueSky.UtilsEditor
{
    public class EditorScriptDefineSymbols : EditorWindow
    {
        #region Ads

        private const string menuPathAds = "Sunflower/ScriptDefineSymbols/VIRTUESKY_ADS";

        [MenuItem(menuPathAds)]
        public static void AdsConfigFlag()
        {
            SwitchFlag("VIRTUESKY_ADS");
        }

        [MenuItem(menuPathAds, true)]
        public static bool IsAdsConfigFlagEnable()
        {
            Menu.SetChecked(menuPathAds, IsFlagEnabled("VIRTUESKY_ADS"));
            return true;
        }

        public static bool IsAdsFlag()
        {
            return IsFlagEnabled("VIRTUESKY_ADS");
        }

        #endregion

        #region Applovin

        private const string menuPathApplovin = "Sunflower/ScriptDefineSymbols/ADS_APPLOVIN";

        [MenuItem(menuPathApplovin)]
        public static void ApplovinConfigFlag()
        {
            SwitchFlag("ADS_APPLOVIN");
        }

        [MenuItem(menuPathApplovin, true)]
        public static bool IsApplovinConfigFlagEnable()
        {
            Menu.SetChecked(menuPathApplovin, IsFlagEnabled("ADS_APPLOVIN"));
            return true;
        }

        public static bool IsApplovinFlag()
        {
            return IsFlagEnabled("ADS_APPLOVIN");
        }

        #endregion

        #region Admob

        private const string menuPathAdmob = "Sunflower/ScriptDefineSymbols/ADS_ADMOB";

        [MenuItem(menuPathAdmob)]
        public static void AdmobConfigFlag()
        {
            SwitchFlag("ADS_ADMOB");
        }

        [MenuItem(menuPathAdmob, true)]
        public static bool IsAdmobConfigFlagEnable()
        {
            Menu.SetChecked(menuPathAdmob, IsFlagEnabled("ADS_ADMOB"));
            return true;
        }

        public static bool IsAdmobFlag()
        {
            return IsFlagEnabled("ADS_ADMOB");
        }

        #endregion

        #region Adjust

        private const string menuPathAdjust = "Sunflower/ScriptDefineSymbols/VIRTUESKY_ADJUST";

        [MenuItem(menuPathAdjust)]
        public static void AdjustConfigFlag()
        {
            SwitchFlag("VIRTUESKY_ADJUST");
        }

        [MenuItem(menuPathAdjust, true)]
        public static bool IsAdjustConfigFlagEnable()
        {
            Menu.SetChecked(menuPathAdjust, IsFlagEnabled("VIRTUESKY_ADJUST"));
            return true;
        }

        public static bool IsAdjustFlag()
        {
            return IsFlagEnabled("VIRTUESKY_ADJUST");
        }

        #endregion

        #region Firebase Analytics

        private const string menuPathAnalytic =
            "Sunflower/ScriptDefineSymbols/VIRTUESKY_FIREBASE_ANALYTIC";

        [MenuItem(menuPathAnalytic)]
        public static void AnalyticConfigFlag()
        {
            SwitchFlag("VIRTUESKY_FIREBASE_ANALYTIC");
        }

        [MenuItem(menuPathAnalytic, true)]
        public static bool IsAnalyticConfigFlagEnable()
        {
            Menu.SetChecked(menuPathAnalytic, IsFlagEnabled("VIRTUESKY_FIREBASE_ANALYTIC"));
            return true;
        }

        public static bool IsAnalyticFlag()
        {
            return IsFlagEnabled("VIRTUESKY_FIREBASE_ANALYTIC");
        }

        #endregion

        #region Firebase Remote Config

        private const string menuPathRemoteConfig =
            "Sunflower/ScriptDefineSymbols/VIRTUESKY_FIREBASE_REMOTECONFIG";

        [MenuItem(menuPathRemoteConfig)]
        public static void RemoteConfigConfigFlag()
        {
            SwitchFlag("VIRTUESKY_FIREBASE_REMOTECONFIG");
        }

        [MenuItem(menuPathRemoteConfig, true)]
        public static bool IsRemoteConfigConfigFlagEnable()
        {
            Menu.SetChecked(menuPathRemoteConfig, IsFlagEnabled("VIRTUESKY_FIREBASE_REMOTECONFIG"));
            return true;
        }

        public static bool IsRemoteConfigConfigFlag()
        {
            return IsFlagEnabled("VIRTUESKY_FIREBASE_REMOTECONFIG");
        }

        #endregion

        #region Firebase App

        private const string menuPathFirebaseApp =
            "Sunflower/ScriptDefineSymbols/VIRTUESKY_FIREBASE";

        [MenuItem(menuPathFirebaseApp)]
        public static void FirebaseAppConfigFlag()
        {
            SwitchFlag("VIRTUESKY_FIREBASE");
        }

        [MenuItem(menuPathFirebaseApp, true)]
        public static bool IsFirebaseAppConfigFlagEnable()
        {
            Menu.SetChecked(menuPathFirebaseApp, IsFlagEnabled("VIRTUESKY_FIREBASE"));
            return true;
        }

        public static bool IsFirebaseAppFlag()
        {
            return IsFlagEnabled("VIRTUESKY_FIREBASE");
        }

        #endregion

        #region Iap

        private const string menuPathIAP = "Sunflower/ScriptDefineSymbols/VIRTUESKY_IAP";

        [MenuItem(menuPathIAP)]
        public static void IapConfigFlag()
        {
            SwitchFlag("VIRTUESKY_IAP");
        }

        [MenuItem(menuPathIAP, true)]
        public static bool IsIapFlagEnable()
        {
            Menu.SetChecked(menuPathIAP, IsFlagEnabled("VIRTUESKY_IAP"));
            return true;
        }

        public static bool IsIapFlag()
        {
            return IsFlagEnabled("VIRTUESKY_IAP");
        }

        #endregion

        #region Ratting

        private const string menuPathRatting = "Sunflower/ScriptDefineSymbols/VIRTUESKY_RATING";

        [MenuItem(menuPathRatting)]
        public static void RattingConfigFlag()
        {
            SwitchFlag("VIRTUESKY_RATING");
        }

        [MenuItem(menuPathRatting, true)]
        public static bool IsRattingConfigFlagEnable()
        {
            Menu.SetChecked(menuPathRatting, IsFlagEnabled("VIRTUESKY_RATING"));
            return true;
        }

        public static bool IsRattingFlag()
        {
            return IsFlagEnabled("VIRTUESKY_RATING");
        }

        #endregion

        #region Notification

        private const string menuPathNotification =
            "Sunflower/ScriptDefineSymbols/VIRTUESKY_NOTIFICATION";

        [MenuItem(menuPathNotification)]
        public static void NotificationConfigFlag()
        {
            SwitchFlag("VIRTUESKY_NOTIFICATION");
        }

        [MenuItem(menuPathNotification, true)]
        public static bool IsNotificationConfigFlagEnable()
        {
            Menu.SetChecked(menuPathNotification, IsFlagEnabled("VIRTUESKY_NOTIFICATION"));
            return true;
        }

        public static bool IsNotificationFlag()
        {
            return IsFlagEnabled("VIRTUESKY_NOTIFICATION");
        }

        #endregion

        #region AppsFlyer

        private const string menuPathAppsFlyer =
            "Sunflower/ScriptDefineSymbols/VIRTUESKY_APPSFLYER";

        [MenuItem(menuPathAppsFlyer)]
        public static void AppsFlyerConfigFlag()
        {
            SwitchFlag("VIRTUESKY_APPSFLYER");
        }

        [MenuItem(menuPathAppsFlyer, true)]
        public static bool IsAppsFlyerConfigFlagEnable()
        {
            Menu.SetChecked(menuPathAppsFlyer, IsFlagEnabled("VIRTUESKY_APPSFLYER"));
            return true;
        }

        public static bool IsAppsFlyerFlag()
        {
            return IsFlagEnabled("VIRTUESKY_APPSFLYER");
        }

        #endregion

        #region PRIME_TWEEN_DOTWEEN_ADAPTER

        private const string menuPathPrimeTweenDotweenAdapter =
            "Sunflower/ScriptDefineSymbols/PRIME_TWEEN_DOTWEEN_ADAPTER";

        [MenuItem(menuPathPrimeTweenDotweenAdapter)]
        public static void PrimeTweenDoTweenAdapterConfigFlag()
        {
            SwitchFlag("PRIME_TWEEN_DOTWEEN_ADAPTER");
        }

        [MenuItem(menuPathPrimeTweenDotweenAdapter, true)]
        public static bool IsPrimeTweenDoTweenAdapterConfigFlagEnable()
        {
            Menu.SetChecked(menuPathPrimeTweenDotweenAdapter,
                IsFlagEnabled("PRIME_TWEEN_DOTWEEN_ADAPTER"));
            return true;
        }

        public static bool IsPrimeTweenDoTweenAdapterFlag()
        {
            return IsFlagEnabled("PRIME_TWEEN_DOTWEEN_ADAPTER");
        }

        #endregion

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