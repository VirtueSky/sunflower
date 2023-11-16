using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using VirtueSky.UtilsEditor;
#endif


namespace VirtueSky.Ads
{
    public class AdSetting : ScriptableObject
    {
        // [SerializeField] private bool autoInit = true;
        [Range(5, 100), SerializeField] private float adCheckingInterval = 8f;
        [Range(5, 100), SerializeField] private float adLoadingInterval = 15f;
        [SerializeField] private AdNetwork adNetwork = AdNetwork.Applovin;

        //  public bool AutoInit => autoInit;
        public float AdCheckingInterval => adCheckingInterval;
        public float AdLoadingInterval => adLoadingInterval;

        public AdNetwork CurrentAdNetwork
        {
            get => adNetwork;
            set => adNetwork = value;
        }

        #region AppLovin

        private const string pathMax = "/Ads/Applovin";

        [Header("Applovin")] [Space, TextArea, SerializeField]
        private string sdkKey;

        [SerializeField] private bool applovinEnableAgeRestrictedUser;
        [SerializeField] private MaxAdClient maxAdClient;
        [SerializeField] private MaxBannerVariable maxBannerVariable;
        [SerializeField] private MaxInterVariable maxInterVariable;
        [SerializeField] private MaxRewardVariable maxRewardVariable;
        [SerializeField] private MaxRewardInterVariable maxRewardInterVariable;
        [SerializeField] private MaxAppOpenVariable maxAppOpenVariable;

        public string SdkKey => sdkKey;
        public bool ApplovinEnableAgeRestrictedUser => applovinEnableAgeRestrictedUser;
        public MaxAdClient MaxAdClient => maxAdClient;
        public MaxBannerVariable MaxBannerVariable => maxBannerVariable;
        public MaxInterVariable MaxInterVariable => maxInterVariable;
        public MaxRewardVariable MaxRewardVariable => maxRewardVariable;
        public MaxRewardInterVariable MaxRewardInterVariable => maxRewardInterVariable;
        public MaxAppOpenVariable MaxAppOpenVariable => maxAppOpenVariable;


#if UNITY_EDITOR
        public void CreateMax()
        {
            maxAdClient = CreateAsset.CreateAndGetScriptableAsset<MaxAdClient>(pathMax);
            maxBannerVariable = CreateAsset.CreateAndGetScriptableAsset<MaxBannerVariable>(pathMax);
            maxInterVariable = CreateAsset.CreateAndGetScriptableAsset<MaxInterVariable>(pathMax);
            maxRewardVariable = CreateAsset.CreateAndGetScriptableAsset<MaxRewardVariable>(pathMax);
            maxRewardInterVariable = CreateAsset.CreateAndGetScriptableAsset<MaxRewardInterVariable>(pathMax);
            maxAppOpenVariable = CreateAsset.CreateAndGetScriptableAsset<MaxAppOpenVariable>(pathMax);
        }
#endif

        #endregion

        #region Admob

        private const string pathAdmob = "/Ads/Admob";

        [Header("Admob")] [Space, SerializeField]
        private AdmobAdClient admobAdClient;

        [SerializeField] private AdmobBannerVariable admobBannerVariable;
        [SerializeField] private AdmobInterVariable admobInterVariable;
        [SerializeField] private AdmobRewardVariable admobRewardVariable;
        [SerializeField] private AdmobRewardInterVariable admobRewardInterVariable;
        [SerializeField] private AdmobAppOpenVariable admobAppOpenVariable;
        [SerializeField] private bool admobEnableTestMode;
        [SerializeField] private List<string> admobDevicesTest;
        public AdmobAdClient AdmobAdClient => admobAdClient;
        public AdmobBannerVariable AdmobBannerVariable => admobBannerVariable;
        public AdmobInterVariable AdmobInterVariable => admobInterVariable;
        public AdmobRewardVariable AdmobRewardVariable => admobRewardVariable;
        public AdmobRewardInterVariable AdmobRewardInterVariable => admobRewardInterVariable;
        public AdmobAppOpenVariable AdmobAppOpenVariable => admobAppOpenVariable;
        public bool AdmobEnableTestMode => admobEnableTestMode;
        public List<string> AdmobDevicesTest => admobDevicesTest;
#if UNITY_EDITOR
        public void CreateAdmob()
        {
            admobAdClient = CreateAsset.CreateAndGetScriptableAsset<AdmobAdClient>(pathAdmob);
            admobBannerVariable = CreateAsset.CreateAndGetScriptableAsset<AdmobBannerVariable>(pathAdmob);
            admobInterVariable = CreateAsset.CreateAndGetScriptableAsset<AdmobInterVariable>(pathAdmob);
            admobRewardVariable = CreateAsset.CreateAndGetScriptableAsset<AdmobRewardVariable>(pathAdmob);
            admobRewardInterVariable =
                CreateAsset.CreateAndGetScriptableAsset<AdmobRewardInterVariable>(pathAdmob);
            admobAppOpenVariable = CreateAsset.CreateAndGetScriptableAsset<AdmobAppOpenVariable>(pathAdmob);
        }
#endif

        #endregion
    }

    public enum AdNetwork
    {
        Applovin,
        Admob
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