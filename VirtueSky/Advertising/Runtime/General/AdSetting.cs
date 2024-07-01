using System.Collections.Generic;
using UnityEngine;
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
        [SerializeField] private AdNetwork adNetwork = AdNetwork.Max;

        public float AdCheckingInterval => adCheckingInterval;
        public float AdLoadingInterval => adLoadingInterval;

        public AdNetwork CurrentAdNetwork
        {
            get => adNetwork;
            set => adNetwork = value;
        }

        #region AppLovin

        [TextArea, SerializeField] private string sdkKey;
        [SerializeField] private bool applovinEnableAgeRestrictedUser;
        [SerializeField] private MaxBannerVariable maxBannerVariable;
        [SerializeField] private MaxInterVariable maxInterVariable;
        [SerializeField] private MaxRewardVariable maxRewardVariable;
        [SerializeField] private MaxRewardInterVariable maxRewardInterVariable;
        [SerializeField] private MaxAppOpenVariable maxAppOpenVariable;

        public string SdkKey => sdkKey;
        public bool ApplovinEnableAgeRestrictedUser => applovinEnableAgeRestrictedUser;
        public MaxBannerVariable MaxBannerVariable => maxBannerVariable;
        public MaxInterVariable MaxInterVariable => maxInterVariable;
        public MaxRewardVariable MaxRewardVariable => maxRewardVariable;
        public MaxRewardInterVariable MaxRewardInterVariable => maxRewardInterVariable;
        public MaxAppOpenVariable MaxAppOpenVariable => maxAppOpenVariable;

        #endregion

        #region Admob

        // [HeaderLine("Admob")] 
        [SerializeField] private AdmobBannerVariable admobBannerVariable;
        [SerializeField] private AdmobInterVariable admobInterVariable;
        [SerializeField] private AdmobRewardVariable admobRewardVariable;
        [SerializeField] private AdmobRewardInterVariable admobRewardInterVariable;
        [SerializeField] private AdmobAppOpenVariable admobAppOpenVariable;
        [SerializeField] private bool admobEnableTestMode;
        [SerializeField] private bool enableGDPR;
        [SerializeField] private bool enableGDPRTestMode;
        [SerializeField] private List<string> admobDevicesTest;
        public AdmobBannerVariable AdmobBannerVariable => admobBannerVariable;
        public AdmobInterVariable AdmobInterVariable => admobInterVariable;
        public AdmobRewardVariable AdmobRewardVariable => admobRewardVariable;
        public AdmobRewardInterVariable AdmobRewardInterVariable => admobRewardInterVariable;
        public AdmobAppOpenVariable AdmobAppOpenVariable => admobAppOpenVariable;
        public bool AdmobEnableTestMode => admobEnableTestMode;
        public bool EnableGDPR => enableGDPR;
        public bool EnableGDPRTestMode => enableGDPRTestMode;
        public List<string> AdmobDevicesTest => admobDevicesTest;

        #endregion

        #region IronSource

        [SerializeField] private string androidAppKey;
        [SerializeField] private string iOSAppKey;
        [SerializeField] private bool useTestAppKey;

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

        #endregion
    }

    public enum AdNetwork
    {
        Max,
        Admob,
        IronSource_UnityLevelPlay
    }

    public enum BannerPosition
    {
        Top = 1,
        Bottom = 0,
        TopLeft = 2,
        TopRight = 3,
        BottomLeft = 4,
        BottomRight = 5,
    }

    public enum BannerSize
    {
        Banner = 0, // 320x50
        Adaptive = 5, // full width
        MediumRectangle = 1, // 300x250
        IABBanner = 2, // 468x60
        Leaderboard = 3, // 728x90
        //    SmartBanner = 4,
    }
}