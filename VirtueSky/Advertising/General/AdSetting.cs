using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Attributes;
using VirtueSky.UtilsEditor;

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

        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)] [Header("Applovin")] [SerializeField, TextArea]
        private string sdkKey;

        public string SdkKey => sdkKey;

        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)] [SerializeField]
        private bool applovinEnableAgeRestrictedUser;

        public bool ApplovinEnableAgeRestrictedUser => applovinEnableAgeRestrictedUser;

        #region Max Ads

        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)] [SerializeField]
        private MaxAdClient maxAdClient;

        public MaxAdClient MaxAdClient => maxAdClient;

        #endregion

        #region Max Banner

        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)] [SerializeField]
        private MaxBannerVariable maxBannerVariable;

        public MaxBannerVariable MaxBannerVariable => maxBannerVariable;

        #endregion

        #region Max Inter

        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)] [SerializeField]
        private MaxInterVariable maxInterVariable;

        public MaxInterVariable MaxInterVariable => maxInterVariable;

        #endregion

        #region Max Reward

        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)] [SerializeField]
        private MaxRewardVariable maxRewardVariable;

        public MaxRewardVariable MaxRewardVariable => maxRewardVariable;

        #endregion

        #region Max RewardInter

        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)] [SerializeField]
        private MaxRewardInterVariable maxRewardInterVariable;

        public MaxRewardInterVariable MaxRewardInterVariable => maxRewardInterVariable;

        #endregion

        #region Max AppOpen

        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)] [SerializeField]
        private MaxAppOpenVariable maxAppOpenVariable;

        public MaxAppOpenVariable MaxAppOpenVariable => maxAppOpenVariable;

        #endregion

        #region Func Create Max Variable

#if UNITY_EDITOR
        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)]
        [Button("Create MaxClient And MaxVariable")]
        void CreateMax()
        {
            CreateMaxAds();
            CreateMaxBanner();
            CreateMaxInter();
            CreateMaxReward();
            CreateMaxRewardInter();
            CreateMaxAppOpen();
        }

        void CreateMaxAds()
        {
            maxAdClient = CreateAsset.CreateAndGetScriptableAsset<MaxAdClient>(pathMax);
        }

        void CreateMaxBanner()
        {
            maxBannerVariable = CreateAsset.CreateAndGetScriptableAsset<MaxBannerVariable>(pathMax);
        }

        void CreateMaxInter()
        {
            maxInterVariable = CreateAsset.CreateAndGetScriptableAsset<MaxInterVariable>(pathMax);
        }

        void CreateMaxReward()
        {
            maxRewardVariable = CreateAsset.CreateAndGetScriptableAsset<MaxRewardVariable>(pathMax);
        }

        void CreateMaxRewardInter()
        {
            maxRewardInterVariable = CreateAsset.CreateAndGetScriptableAsset<MaxRewardInterVariable>(pathMax);
        }

        void CreateMaxAppOpen()
        {
            maxAppOpenVariable = CreateAsset.CreateAndGetScriptableAsset<MaxAppOpenVariable>(pathMax);
        }

#endif

        #endregion

        #endregion

        #region Admob

        private const string pathAdmob = "/Ads/Admob";


        #region Admob Ads

        [ShowIf(nameof(adNetwork), AdNetwork.Admob)] [Header("Admob")] [SerializeField]
        private AdmobAdClient admobAdClient;

        public AdmobAdClient AdmobAdClient => admobAdClient;

        #endregion

        #region Admod Banner

        [ShowIf(nameof(adNetwork), AdNetwork.Admob)] [SerializeField]
        private AdmobBannerVariable admobBannerVariable;

        public AdmobBannerVariable AdmobBannerVariable => admobBannerVariable;

        #endregion

        #region Admod Inter

        [ShowIf(nameof(adNetwork), AdNetwork.Admob)] [SerializeField]
        private AdmobInterVariable admobInterVariable;

        public AdmobInterVariable AdmobInterVariable => admobInterVariable;

        #endregion

        #region Admod Reward

        [ShowIf(nameof(adNetwork), AdNetwork.Admob)] [SerializeField]
        private AdmobRewardVariable admobRewardVariable;

        public AdmobRewardVariable AdmobRewardVariable => admobRewardVariable;

        #endregion

        #region Admod RewardInter

        [ShowIf(nameof(adNetwork), AdNetwork.Admob)] [SerializeField]
        private AdmobRewardInterVariable admobRewardInterVariable;

        public AdmobRewardInterVariable AdmobRewardInterVariable => admobRewardInterVariable;

        #endregion

        #region Admod AppOpen

        [ShowIf(nameof(adNetwork), AdNetwork.Admob)] [SerializeField]
        private AdmobAppOpenVariable admobAppOpenVariable;

        public AdmobAppOpenVariable AdmobAppOpenVariable => admobAppOpenVariable;

        #endregion

        #region Func Create Admob Variable

#if UNITY_EDITOR
        [ShowIf(nameof(adNetwork), AdNetwork.Admob)]
        [Button("Create AdmobClient And AdmobVariable")]
        void CreateAdmob()
        {
            CreateAdmobAds();
            CreateAdmobBanner();
            CreateAdmobInter();
            CreateAdmobReward();
            CreateAdmobRewardInter();
            CreateAdmobAppOpen();
        }

        void CreateAdmobAds()
        {
            admobAdClient = CreateAsset.CreateAndGetScriptableAsset<AdmobAdClient>(pathAdmob);
        }

        void CreateAdmobBanner()
        {
            admobBannerVariable = CreateAsset.CreateAndGetScriptableAsset<AdmobBannerVariable>(pathAdmob);
        }

        void CreateAdmobInter()
        {
            admobInterVariable = CreateAsset.CreateAndGetScriptableAsset<AdmobInterVariable>(pathAdmob);
        }

        void CreateAdmobAppOpen()
        {
            admobAppOpenVariable = CreateAsset.CreateAndGetScriptableAsset<AdmobAppOpenVariable>(pathAdmob);
        }

        void CreateAdmobRewardInter()
        {
            admobRewardInterVariable =
                CreateAsset.CreateAndGetScriptableAsset<AdmobRewardInterVariable>(pathAdmob);
        }

        void CreateAdmobReward()
        {
            admobRewardVariable = CreateAsset.CreateAndGetScriptableAsset<AdmobRewardVariable>(pathAdmob);
        }
#endif

        #endregion

        [SerializeField] private bool admobEnableTestMode;
        public bool AdmobEnableTestMode => admobEnableTestMode;

        [SerializeField] private List<string> admobDevicesTest;
        public List<string> AdmobDevicesTest => admobDevicesTest;

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