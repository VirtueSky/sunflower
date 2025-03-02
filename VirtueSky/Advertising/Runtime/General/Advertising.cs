using System;
using System.Collections;
using System.Collections.Generic;
#if VIRTUESKY_ADMOB
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
#endif

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using VirtueSky.UtilsEditor;
#endif
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Utils;

namespace VirtueSky.Ads
{
    [EditorIcon("icon_controller"), HideMonoScript]
    public class Advertising : MonoBehaviour
    {
        [Space] [SerializeField] private bool dontDestroyOnLoad = false;
        [Tooltip("Require"), SerializeField] private AdSetting adSetting;

        [Tooltip("Allows nulls"), SerializeField]
        private BooleanEvent changePreventDisplayAppOpenEvent;

#if VIRTUESKY_ADMOB
        [Space] [HeaderLine("Admob GDPR")] [Tooltip("Allows nulls"), SerializeField]
        private EventNoParam callShowAgainGDPREvent;
#endif


        private IEnumerator autoLoadAdCoroutine;
        private float _lastTimeLoadInterstitialAdTimestamp = DEFAULT_TIMESTAMP;
        private float _lastTimeLoadRewardedTimestamp = DEFAULT_TIMESTAMP;
        private float _lastTimeLoadRewardedInterstitialTimestamp = DEFAULT_TIMESTAMP;
        private float _lastTimeLoadAppOpenTimestamp = DEFAULT_TIMESTAMP;
        private const float DEFAULT_TIMESTAMP = -1000;

        private AdClient currentAdClient;

        private void Awake()
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }

        private void OnEnable()
        {
#if VIRTUESKY_ADMOB
            if (callShowAgainGDPREvent != null)
            {
                callShowAgainGDPREvent.AddListener(ShowPrivacyOptionsForm);
            }
#endif
        }

        private void OnDisable()
        {
#if VIRTUESKY_ADMOB
            if (callShowAgainGDPREvent != null)
            {
                callShowAgainGDPREvent.RemoveListener(ShowPrivacyOptionsForm);
            }
#endif
        }

        private void Start()
        {
            if (changePreventDisplayAppOpenEvent != null)
                changePreventDisplayAppOpenEvent.AddListener(OnChangePreventDisplayOpenAd);
            if (adSetting.EnableGDPR)
            {
#if VIRTUESKY_ADMOB
#if UNITY_IOS
                if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
                    ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED)
                {
                    InitGdpr();
                }
                else
                {
                    InitAdClient();
                }
#else
                InitGdpr();
#endif


#endif
            }
            else
            {
                InitAdClient();
            }
        }

        void InitAdClient()
        {
            switch (adSetting.CurrentAdNetwork)
            {
                case AdNetwork.Max:
                    currentAdClient = new MaxAdClient();
                    break;
                case AdNetwork.Admob:
                    currentAdClient = new AdmobAdClient();
                    break;
                case AdNetwork.IronSource:
                    currentAdClient = new IronSourceAdClient();
                    break;
            }

            currentAdClient.SetupAdSetting(adSetting);
            currentAdClient.Initialize();
            Debug.Log($"currentAdClient: {currentAdClient}".SetColor(CustomColor.Cyan));
            InitAutoLoadAds();
        }

        private void InitAutoLoadAds()
        {
            if (autoLoadAdCoroutine != null) StopCoroutine(autoLoadAdCoroutine);
            autoLoadAdCoroutine = IeAutoLoadAll();
            StartCoroutine(autoLoadAdCoroutine);
        }

        private void OnChangePreventDisplayOpenAd(bool state)
        {
            AdStatic.isShowingAd = state;
        }

        IEnumerator IeAutoLoadAll(float delay = 0)
        {
            if (delay > 0) yield return new WaitForSeconds(delay);
            while (true)
            {
                AutoLoadInterAds();
                AutoLoadRewardAds();
                AutoLoadRewardInterAds();
                AutoLoadAppOpenAds();
                yield return new WaitForSeconds(adSetting.AdCheckingInterval);
            }
        }

        #region Func Load Ads

        void AutoLoadInterAds()
        {
            if (Time.realtimeSinceStartup - _lastTimeLoadInterstitialAdTimestamp <
                adSetting.AdLoadingInterval) return;
            currentAdClient.LoadInterstitial();
            _lastTimeLoadInterstitialAdTimestamp = Time.realtimeSinceStartup;
        }

        void AutoLoadRewardAds()
        {
            if (Time.realtimeSinceStartup - _lastTimeLoadRewardedTimestamp <
                adSetting.AdLoadingInterval) return;
            currentAdClient.LoadRewarded();
            _lastTimeLoadRewardedTimestamp = Time.realtimeSinceStartup;
        }

        void AutoLoadRewardInterAds()
        {
            if (Time.realtimeSinceStartup - _lastTimeLoadRewardedInterstitialTimestamp <
                adSetting.AdLoadingInterval) return;
            currentAdClient.LoadRewardedInterstitial();
            _lastTimeLoadRewardedInterstitialTimestamp = Time.realtimeSinceStartup;
        }

        void AutoLoadAppOpenAds()
        {
            if (Time.realtimeSinceStartup - _lastTimeLoadAppOpenTimestamp <
                adSetting.AdLoadingInterval) return;
            currentAdClient.LoadAppOpen();
            _lastTimeLoadAppOpenTimestamp = Time.realtimeSinceStartup;
        }

        #endregion

        #region Admob GDPR

#if VIRTUESKY_ADMOB
        private void InitGdpr()
        {
#if UNITY_EDITOR
            InitAdClient();
#else
            string deviceID = SystemInfo.deviceUniqueIdentifier;
            string deviceIDUpperCase = deviceID.ToUpper();

            Debug.Log("TestDeviceHashedId = " + deviceIDUpperCase);
            ConsentRequestParameters request = new ConsentRequestParameters { TagForUnderAgeOfConsent = false };
            if (adSetting.EnableGDPRTestMode)
            {
                List<string> listDeviceIdTestMode = new List<string>();
                listDeviceIdTestMode.Add(deviceIDUpperCase);
                request.ConsentDebugSettings = new ConsentDebugSettings
                {
                    DebugGeography = DebugGeography.EEA,
                    TestDeviceHashedIds = listDeviceIdTestMode
                };
            }

            ConsentInformation.Update(request, OnConsentInfoUpdated);
#endif
        }

        private void OnConsentInfoUpdated(FormError consentError)
        {
            if (consentError != null)
            {
                Debug.Log("error consentError = " + consentError);
                return;
            }

            ConsentForm.LoadAndShowConsentFormIfRequired(
                (FormError formError) =>
                {
                    if (formError != null)
                    {
                        Debug.Log("error consentError = " + consentError);
                        return;
                    }

                    Debug.Log("ConsentStatus = " + ConsentInformation.ConsentStatus.ToString());
                    Debug.Log("CanRequestAds = " + ConsentInformation.CanRequestAds());

                    if (ConsentInformation.CanRequestAds())
                    {
                        MobileAds.RaiseAdEventsOnUnityMainThread = true;
                        InitAdClient();
                    }
                }
            );
        }

        public void LoadAndShowConsentForm()
        {
            Debug.Log("LoadAndShowConsentForm Start!");

            ConsentForm.Load((consentForm, loadError) =>
            {
                if (loadError != null)
                {
                    Debug.Log("error loadError = " + loadError);
                    return;
                }


                consentForm.Show(showError =>
                {
                    if (showError != null)
                    {
                        Debug.Log("error showError = " + showError);
                        return;
                    }
                });
            });
        }

        private void ShowPrivacyOptionsForm()
        {
            Debug.Log("Showing privacy options form.");

            ConsentForm.ShowPrivacyOptionsForm((FormError showError) =>
            {
                if (showError != null)
                {
                    Debug.LogError("Error showing privacy options form with error: " + showError.Message);
                }
            });
        }
#endif

        #endregion

#if UNITY_EDITOR
        private void Reset()
        {
            adSetting = CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Ads.AdSetting>("/Ads/Setting");
        }
#endif
    }
}