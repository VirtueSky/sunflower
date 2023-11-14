#if UNITY_EDITOR
using UnityEditor;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Ads
{
    public class AdsWindowEditor : EditorWindow
    {
        [MenuItem("Sunflower/Ads/AdSetting %E", false)]
        public static void MenuOpenAdSettings()
        {
            var adSetting = CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Ads.AdSetting>("/Ads");
            Selection.activeObject = adSetting;
            EditorUtility.FocusProjectWindow();
        }

        #region Applovin

        private const string pathMax = "/Ads/Applovin";

        [MenuItem("Sunflower/Ads/Applovin/Max Client")]
        public static void CreateMaxClient()
        {
            CreateAsset.CreateScriptableAssets<MaxAdClient>(pathMax);
        }

        [MenuItem("Sunflower/Ads/Applovin/Max Banner")]
        public static void CreateMaxBanner()
        {
            CreateAsset.CreateScriptableAssets<MaxBannerVariable>(pathMax);
        }

        [MenuItem("Sunflower/Ads/Applovin/Max Inter")]
        public static void CreateMaxInter()
        {
            CreateAsset.CreateScriptableAssets<MaxInterVariable>(pathMax);
        }

        [MenuItem("Sunflower/Ads/Applovin/Max Reward")]
        public static void CreateMaxReward()
        {
            CreateAsset.CreateScriptableAssets<MaxRewardVariable>(pathMax);
        }

        [MenuItem("Sunflower/Ads/Applovin/Max App Open")]
        public static void CreateMaxAppOpen()
        {
            CreateAsset.CreateScriptableAssets<MaxAppOpenVariable>(pathMax);
        }

        [MenuItem("Sunflower/Ads/Applovin/Max Reward Inter")]
        public static void CreateMaxRewardInter()
        {
            CreateAsset.CreateScriptableAssets<MaxRewardInterVariable>(pathMax);
        }

        #endregion

        #region Admob

        private const string pathAdmob = "/Ads/Admob";

        [MenuItem("Sunflower/Ads/Admob/Admob Client")]
        public static void CreateAdmobClient()
        {
            CreateAsset.CreateScriptableAssets<AdmobAdClient>(pathAdmob);
        }

        [MenuItem("Sunflower/Ads/Admob/Admob Banner")]
        public static void CreateAdmobBanner()
        {
            CreateAsset.CreateScriptableAssets<AdmobBannerVariable>(pathAdmob);
        }

        [MenuItem("Sunflower/Ads/Admob/Admob Inter")]
        public static void CreateAdmobInter()
        {
            CreateAsset.CreateScriptableAssets<AdmobInterVariable>(pathAdmob);
        }

        [MenuItem("Sunflower/Ads/Admob/Admob Reward")]
        public static void CreateAdmobReward()
        {
            CreateAsset.CreateScriptableAssets<AdmobRewardVariable>(pathAdmob);
        }

        [MenuItem("Sunflower/Ads/Admob/Admob App Open")]
        public static void CreateAdmobAppOpen()
        {
            CreateAsset.CreateScriptableAssets<AdmobAppOpenVariable>(pathAdmob);
        }

        [MenuItem("Sunflower/Ads/Admob/Admob Reward Inter")]
        public static void CreateAdmobRewardInter()
        {
            CreateAsset.CreateScriptableAssets<AdmobRewardInterVariable>(pathAdmob);
        }

        #endregion
    }
}
#endif