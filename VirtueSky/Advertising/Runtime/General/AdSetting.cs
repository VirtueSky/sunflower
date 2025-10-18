using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Inspector;
#if UNITY_EDITOR
using VirtueSky.UtilsEditor;
#endif


namespace VirtueSky.Ads
{
    [EditorIcon("icon_scriptable")]
    public class AdSetting : ScriptableObject
    {
        [Range(5, 100), SerializeField] private float adCheckingInterval = 8f;

        [Range(5, 100), SerializeField] private float adLoadingInterval = 15f;

        [SerializeField] private bool useMax = true;
        [SerializeField] private bool useAdmob;
        [SerializeField] private bool useLevelPlay;
        [SerializeField] private bool enableTrackAdRevenue = true;

        [Tooltip("Install google-mobile-ads sdk to use GDPR"), SerializeField]
        private bool enableGDPR;

        [SerializeField] private bool enableGDPRTestMode;
        public float AdCheckingInterval => adCheckingInterval;
        public float AdLoadingInterval => adLoadingInterval;
        public bool UseMax => useMax;
        public bool UseAdmob => useAdmob;
        public bool UseLevelPlay => useLevelPlay;
        public bool EnableTrackAdRevenue => enableTrackAdRevenue;
        public bool EnableGDPR => enableGDPR;
        public bool EnableGDPRTestMode => enableGDPRTestMode;

        #region AppLovin

        [TextArea, SerializeField] private string sdkKey;
        [SerializeField] private MaxBannerVariable maxBannerVariable;
        [SerializeField] private MaxInterVariable maxInterVariable;
        [SerializeField] private MaxRewardVariable maxRewardVariable;
        [SerializeField] private MaxAppOpenVariable maxAppOpenVariable;

        public string SdkKey => sdkKey;
        public MaxBannerVariable MaxBannerVariable => maxBannerVariable;
        public MaxInterVariable MaxInterVariable => maxInterVariable;
        public MaxRewardVariable MaxRewardVariable => maxRewardVariable;
        public MaxAppOpenVariable MaxAppOpenVariable => maxAppOpenVariable;

        #endregion

        #region Admob

        // [HeaderLine("Admob")] 
        [SerializeField] private AdmobBannerVariable admobBannerVariable;
        [SerializeField] private AdmobInterVariable admobInterVariable;
        [SerializeField] private AdmobRewardVariable admobRewardVariable;
        [SerializeField] private AdmobRewardInterVariable admobRewardInterVariable;
        [SerializeField] private AdmobAppOpenVariable admobAppOpenVariable;
        [SerializeField] private AdmobNativeOverlayVariable admobNativeOverlayVariable;

        [Tooltip(
             "If you enable and connect admob with firebase, ad_impression will be automatically tracked. If you disable and disconnect admob with firebase, ad_impression will be tracked manually."),
         SerializeField]
        private bool autoTrackingAdImpressionAdmob = true;

        [SerializeField] private bool admobEnableTestMode;
        [SerializeField] private List<string> admobDevicesTest;
        public AdmobBannerVariable AdmobBannerVariable => admobBannerVariable;
        public AdmobInterVariable AdmobInterVariable => admobInterVariable;
        public AdmobRewardVariable AdmobRewardVariable => admobRewardVariable;
        public AdmobRewardInterVariable AdmobRewardInterVariable => admobRewardInterVariable;
        public AdmobAppOpenVariable AdmobAppOpenVariable => admobAppOpenVariable;
        public AdmobNativeOverlayVariable AdmobNativeOverlayVariable => admobNativeOverlayVariable;
        public bool AdmobEnableTestMode => admobEnableTestMode;
        public bool AutoTrackingAdImpressionAdmob => autoTrackingAdImpressionAdmob;

        public List<string> AdmobDevicesTest => admobDevicesTest;

        #endregion

        #region LevelPlay

        [SerializeField] private string androidAppKey;
        [SerializeField] private string iOSAppKey;
        [SerializeField] private bool useTestAppKey;

        [SerializeField] private LevelPlayBannerVariable levelPlayBannerVariable;
        [SerializeField] private LevelPlayInterVariable levelPlayInterVariable;
        [SerializeField] private LevelPlayRewardVariable levelPlayRewardVariable;

        public string AndroidAppKey
        {
            get => androidAppKey;
            set => androidAppKey = value;
        }

        public string IosAppKey
        {
            get => iOSAppKey;
            set => iOSAppKey = value;
        }

        public string AppKey
        {
            get
            {
#if UNITY_ANDROID
                return androidAppKey;
#elif UNITY_IOS
                return iOSAppKey;
#else
                return string.Empty;
#endif
            }
            set
            {
#if UNITY_ANDROID
                androidAppKey = value;
#elif UNITY_IOS
                iOSAppKey = value;
#endif
            }
        }

        public bool UseTestAppKey => useTestAppKey;
        public LevelPlayBannerVariable LevelPlayBannerVariable => levelPlayBannerVariable;
        public LevelPlayInterVariable LevelPlayInterVariable => levelPlayInterVariable;
        public LevelPlayRewardVariable LevelPlayRewardVariable => levelPlayRewardVariable;

        #endregion
    }

    public enum AdNetwork
    {
        Max,
        Admob,
        LevelPlay
    }

    public enum AdsPosition
    {
        Top = 1,
        Bottom = 0,
        TopLeft = 2,
        TopRight = 3,
        BottomLeft = 4,
        BottomRight = 5,
        Center = 6,
    }

    public enum AdsSize
    {
        Banner = 0, // 320x50
        Adaptive = 5, // full width
        MediumRectangle = 1, // 300x250
        IABBanner = 2, // 468x60
        Leaderboard = 3, // 728x90
        // SmartBanner = 4,
    }
}