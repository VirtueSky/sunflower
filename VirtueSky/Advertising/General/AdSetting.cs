using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VirtueSky.EditorUtils;

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

        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)]
#if UNITY_EDITOR
        [InlineButton(nameof(CreateMaxAds), "Create")]
#endif
        [SerializeField]
        private MaxAdClient maxAdClient;

        public MaxAdClient MaxAdClient => maxAdClient;

        #endregion

        #region Max Banner

        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)]
#if UNITY_EDITOR
        [InlineButton(nameof(CreateMaxBanner), "Create")]
#endif

        [SerializeField]
        private MaxBannerVariable maxBannerVariable;

        public MaxBannerVariable MaxBannerVariable => maxBannerVariable;

        #endregion

        #region Max Inter

        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)]
#if UNITY_EDITOR
        [InlineButton(nameof(CreateMaxInter), "Create")]
#endif

        [SerializeField]
        private MaxInterVariable maxInterVariable;

        public MaxInterVariable MaxInterVariable => maxInterVariable;

        #endregion

        #region Max Reward

        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)]
#if UNITY_EDITOR
        [InlineButton(nameof(CreateMaxReward), "Create")]
#endif

        [SerializeField]
        private MaxRewardVariable maxRewardVariable;

        public MaxRewardVariable MaxRewardVariable => maxRewardVariable;

        #endregion

        #region Max RewardInter

        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)]
#if UNITY_EDITOR
        [InlineButton(nameof(CreateMaxRewardInter), "Create")]
#endif

        [SerializeField]
        private MaxRewardInterVariable maxRewardInterVariable;

        public MaxRewardInterVariable MaxRewardInterVariable => maxRewardInterVariable;

        #endregion

        #region Max AppOpen

        [ShowIf(nameof(adNetwork), AdNetwork.Applovin)]
#if UNITY_EDITOR
        [InlineButton(nameof(CreateMaxAppOpen), "Create")]
#endif

        [SerializeField]
        private MaxAppOpenVariable maxAppOpenVariable;

        public MaxAppOpenVariable MaxAppOpenVariable => maxAppOpenVariable;

        #endregion

        #region Func Create Max Variable

#if UNITY_EDITOR
        void CreateMaxAds()
        {
            maxAdClient = ScriptableSetting.CreateAndGetScriptableAsset<MaxAdClient>(pathMax);
        }

        void CreateMaxBanner()
        {
            maxBannerVariable = ScriptableSetting.CreateAndGetScriptableAsset<MaxBannerVariable>(pathMax);
        }

        void CreateMaxInter()
        {
            maxInterVariable = ScriptableSetting.CreateAndGetScriptableAsset<MaxInterVariable>(pathMax);
        }

        void CreateMaxReward()
        {
            maxRewardVariable = ScriptableSetting.CreateAndGetScriptableAsset<MaxRewardVariable>(pathMax);
        }

        void CreateMaxRewardInter()
        {
            maxRewardInterVariable = ScriptableSetting.CreateAndGetScriptableAsset<MaxRewardInterVariable>(pathMax);
        }

        void CreateMaxAppOpen()
        {
            maxAppOpenVariable = ScriptableSetting.CreateAndGetScriptableAsset<MaxAppOpenVariable>(pathMax);
        }

#endif

        #endregion

        #endregion

        #region Admob

        private const string pathAdmob = "/Ads/Admob";


        #region Admob Ads

        [ShowIf(nameof(adNetwork), AdNetwork.Admob)]
        [Header("Admob")]
#if UNITY_EDITOR
        [InlineButton(nameof(CreateAdmodAds), "Create")]
#endif

        [SerializeField]
        private AdmobAdClient admobAdClient;

        public AdmobAdClient AdmobAdClient => admobAdClient;

        #endregion

        #region Admod Banner

        [ShowIf(nameof(adNetwork), AdNetwork.Admob)]
#if UNITY_EDITOR
        [InlineButton(nameof(CreateAdmobBanner), "Create")]
#endif

        [SerializeField]
        private AdmobBannerVariable admobBannerVariable;

        public AdmobBannerVariable AdmobBannerVariable => admobBannerVariable;

        #endregion

        #region Admod Inter

        [ShowIf(nameof(adNetwork), AdNetwork.Admob)]
#if UNITY_EDITOR
        [InlineButton(nameof(CreateAdmobInter), "Create")]
#endif

        [SerializeField]
        private AdmobInterVariable admobInterVariable;

        public AdmobInterVariable AdmobInterVariable => admobInterVariable;

        #endregion

        #region Admod Reward

        [ShowIf(nameof(adNetwork), AdNetwork.Admob)]
#if UNITY_EDITOR
        [InlineButton(nameof(CreateAdmobReward), "Create")]
#endif

        [SerializeField]
        private AdmobRewardVariable admobRewardVariable;

        public AdmobRewardVariable AdmobRewardVariable => admobRewardVariable;

        #endregion

        #region Admod RewardInter

        [ShowIf(nameof(adNetwork), AdNetwork.Admob)]
#if UNITY_EDITOR
        [InlineButton(nameof(CreateAdmobRewardInter), "Create")]
#endif

        [SerializeField]
        private AdmobRewardInterVariable admobRewardInterVariable;

        public AdmobRewardInterVariable AdmobRewardInterVariable => admobRewardInterVariable;

        #endregion

        #region Admod AppOpen

        [ShowIf(nameof(adNetwork), AdNetwork.Admob)]
#if UNITY_EDITOR
        [InlineButton(nameof(CreateAdmobAppOpen), "Create")]
#endif

        [SerializeField]
        private AdmobAppOpenVariable admobAppOpenVariable;

        public AdmobAppOpenVariable AdmobAppOpenVariable => admobAppOpenVariable;

        #endregion

        #region Func Create Admob Variable

#if UNITY_EDITOR
        void CreateAdmodAds()
        {
            admobAdClient = ScriptableSetting.CreateAndGetScriptableAsset<AdmobAdClient>(pathAdmob);
        }

        void CreateAdmobBanner()
        {
            admobBannerVariable = ScriptableSetting.CreateAndGetScriptableAsset<AdmobBannerVariable>(pathAdmob);
        }

        void CreateAdmobInter()
        {
            admobInterVariable = ScriptableSetting.CreateAndGetScriptableAsset<AdmobInterVariable>(pathAdmob);
        }

        void CreateAdmobAppOpen()
        {
            admobAppOpenVariable = ScriptableSetting.CreateAndGetScriptableAsset<AdmobAppOpenVariable>(pathAdmob);
        }

        void CreateAdmobRewardInter()
        {
            admobRewardInterVariable =
                ScriptableSetting.CreateAndGetScriptableAsset<AdmobRewardInterVariable>(pathAdmob);
        }

        void CreateAdmobReward()
        {
            admobRewardVariable = ScriptableSetting.CreateAndGetScriptableAsset<AdmobRewardVariable>(pathAdmob);
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
        Adaptive = 5 // full width
    }
}