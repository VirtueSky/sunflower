#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

namespace VirtueSky.EditorUtils
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

        #endregion

        #region Firebase Analytics

        private const string menuPathAnalytic = "Sunflower/ScriptDefineSymbols/VIRTUESKY_FIREBASE_ANALYTIC";

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

        #endregion

        #region Firebase Remote Config

        private const string menuPathRemoteConfig = "Sunflower/ScriptDefineSymbols/VIRTUESKY_FIREBASE_REMOTECONFIG";

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

        #endregion

        #region Firebase App

        private const string menuPathFirebaseApp = "Sunflower/ScriptDefineSymbols/VIRTUESKY_FIREBASE";

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

        #endregion

        #region Base Functions

        static void SwitchFlag(string flag)
        {
            PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                out var defines);
            var enabled = defines.Contains(flag);
            defines = enabled ? defines.Where(value => value != flag).ToArray() : defines.Append(flag).ToArray();
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
        }

        static bool IsFlagEnabled(string flag)
        {
            PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                out var defines);
            return defines.Contains(flag);
        }

        #endregion
    }
}
#endif