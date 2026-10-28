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
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Tracking;
using VirtueSky.Utils;
using VirtueSky.Variables;

namespace VirtueSky.Ads
{
    [EditorIcon("icon_controller"), HideMonoScript]
    public class Advertising : MonoBehaviour
    {
        public enum InitType
        {
            InitOnAwake,
            InitOnEnable,
            InitOnStart,
            Manual
        }

        [Space] [SerializeField] private bool dontDestroyOnLoad = false;
        [SerializeField] private InitType initType = InitType.InitOnAwake;
        [Tooltip("Require"), SerializeField] private AdSetting adSetting;

        [Tooltip("Allows nulls"), SerializeField]
        private BooleanEvent changePreventDisplayAppOpenEvent;

        [SerializeField] private EventNoParam initializeAdEvent;
        [SerializeField] private BooleanVariable isInitAdClientVariable;
        [SerializeField] private StringEvent showAdDebugerEvent;

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

        // private AdClient currentAdClient;
        private AdClient maxAdClient;
        private AdClient admobAdClient;
        private AdClient levelplayAdClient;
        private bool isInititalization = false;
        private int pendingAdClientInitializations;
        private int mainThreadId;

        private void Awake()
        {
            mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
#if VIRTUESKY_ADMOB
            if (callShowAgainGDPREvent != null)
            {
                callShowAgainGDPREvent.AddListener(ShowPrivacyOptionsForm);
            }
#endif
            if (initializeAdEvent != null)
            {
                initializeAdEvent.AddListener(Initialization);
            }

            if (showAdDebugerEvent != null)
            {
                showAdDebugerEvent.AddListener(ShowAdDebuger);
            }

            isInititalization = false;
            if (initType == InitType.InitOnAwake)
            {
                Initialization();
            }
        }

        private void OnEnable()
        {
            if (initType == InitType.InitOnEnable)
            {
                Initialization();
            }
        }

        private void Start()
        {
            if (initType == InitType.InitOnStart)
            {
                Initialization();
            }
        }

        private void OnDestroy()
        {
#if VIRTUESKY_ADMOB
            if (callShowAgainGDPREvent != null)
            {
                callShowAgainGDPREvent.RemoveListener(ShowPrivacyOptionsForm);
            }
#endif
            if (initializeAdEvent != null)
            {
                initializeAdEvent.RemoveListener(Initialization);
            }

            if (showAdDebugerEvent != null)
            {
                showAdDebugerEvent.RemoveListener(ShowAdDebuger);
            }
        }

        private void Initialization()
        {
            if (isInititalization) return;
            isInititalization = true;
            if (isInitAdClientVariable != null) isInitAdClientVariable.Value = false;
            if (changePreventDisplayAppOpenEvent != null) changePreventDisplayAppOpenEvent.AddListener(OnChangePreventDisplayOpenAd);
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
            // Callers like UMP consent callbacks run on a native (Java) thread, but everything below touches Unity API
            // (App.Delay -> Time.realtimeSinceStartup, StartCoroutine, SDK init), so always continue on main thread.
            if (System.Threading.Thread.CurrentThread.ManagedThreadId != mainThreadId)
            {
                App.RunOnMainThread(InitAdClient);
                return;
            }

            bool initializeAppLovin = adSetting.IsApplovin();
            bool initializeAdmob = adSetting.IsAdmob();
            bool initializeLevelPlay = adSetting.IsLevelPlay();

            pendingAdClientInitializations = 0;
            if (initializeAppLovin) pendingAdClientInitializations++;
            if (initializeAdmob) pendingAdClientInitializations++;
            if (initializeLevelPlay) pendingAdClientInitializations++;

            if (pendingAdClientInitializations == 0)
            {
                CompleteAdClientInitialization();
                return;
            }

            if (initializeAppLovin)
                ScheduleAdClientInitialization(adSetting.AppLovinInitDelay, InitApplovinClient, "AppLovin");
            if (initializeAdmob)
                ScheduleAdClientInitialization(adSetting.AdmobInitDelay, InitAdmobClient, "AdMob");
            if (initializeLevelPlay)
                ScheduleAdClientInitialization(adSetting.LevelPlayInitDelay, InitLevelPlayClient, "LevelPlay");
        }

        private void ScheduleAdClientInitialization(float delay, Action initialize, string mediationName)
        {
            if (delay <= 0f)
            {
                InitializeAdClient(initialize);
                return;
            }

            Debug.Log($"Initialize {mediationName} AdClient after {delay:0.##} seconds.".SetColor(CustomColor.Cyan));
            App.Delay(this, delay, () => InitializeAdClient(initialize), useRealTime: true);
        }

        private void InitializeAdClient(Action initialize)
        {
            initialize();

            // Start retries as soon as the first mediation begins initializing; delayed mediations load independently
            // from their own SDK initialization callbacks.
            if (autoLoadAdCoroutine == null) InitAutoLoadAds();

            pendingAdClientInitializations--;
            if (pendingAdClientInitializations == 0) CompleteAdClientInitialization();
        }

        /// <summary>
        /// isInitAdClientVariable is set true after all enabled ad clients have started initialization, including their configured delays.
        /// </summary>
        private void CompleteAdClientInitialization()
        {
            if (isInitAdClientVariable != null) isInitAdClientVariable.Value = true;
        }

        void InitApplovinClient()
        {
            maxAdClient = new MaxAdClient(adSetting);
            maxAdClient.Initialize();
            Debug.Log($"Use: {maxAdClient}".SetColor(CustomColor.Cyan));
        }

        void InitAdmobClient()
        {
            admobAdClient = new AdmobAdClient(adSetting);
            admobAdClient.Initialize();
            Debug.Log($"Use: {admobAdClient}".SetColor(CustomColor.Cyan));
        }

        void InitLevelPlayClient()
        {
            levelplayAdClient = new LevelPlayAdClient(adSetting);
            levelplayAdClient.Initialize();
            Debug.Log($"Use: {levelplayAdClient}".SetColor(CustomColor.Cyan));
        }

        private void InitAutoLoadAds()
        {
            if (autoLoadAdCoroutine != null) StopCoroutine(autoLoadAdCoroutine);
            autoLoadAdCoroutine = IeAutoLoadAll();
            StartCoroutine(autoLoadAdCoroutine);
        }

        private void OnChangePreventDisplayOpenAd(bool state)
        {
            AdStatic.IsShowingAd = state;
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
            if (IsAdClientReady(adSetting.IsApplovin(), maxAdClient)) maxAdClient.LoadInterstitial();
            if (IsAdClientReady(adSetting.IsAdmob(), admobAdClient)) admobAdClient.LoadInterstitial();
            if (IsAdClientReady(adSetting.IsLevelPlay(), levelplayAdClient)) levelplayAdClient.LoadInterstitial();
            _lastTimeLoadInterstitialAdTimestamp = Time.realtimeSinceStartup;
        }

        void AutoLoadRewardAds()
        {
            if (Time.realtimeSinceStartup - _lastTimeLoadRewardedTimestamp <
                adSetting.AdLoadingInterval) return;
            if (IsAdClientReady(adSetting.IsApplovin(), maxAdClient)) maxAdClient.LoadRewarded();
            if (IsAdClientReady(adSetting.IsAdmob(), admobAdClient)) admobAdClient.LoadRewarded();
            if (IsAdClientReady(adSetting.IsLevelPlay(), levelplayAdClient)) levelplayAdClient.LoadRewarded();
            _lastTimeLoadRewardedTimestamp = Time.realtimeSinceStartup;
        }

        void AutoLoadRewardInterAds()
        {
            if (Time.realtimeSinceStartup - _lastTimeLoadRewardedInterstitialTimestamp <
                adSetting.AdLoadingInterval) return;
            if (IsAdClientReady(adSetting.IsApplovin(), maxAdClient)) maxAdClient.LoadRewardedInterstitial();
            if (IsAdClientReady(adSetting.IsAdmob(), admobAdClient)) admobAdClient.LoadRewardedInterstitial();
            if (IsAdClientReady(adSetting.IsLevelPlay(), levelplayAdClient)) levelplayAdClient.LoadRewardedInterstitial();
            _lastTimeLoadRewardedInterstitialTimestamp = Time.realtimeSinceStartup;
        }

        void AutoLoadAppOpenAds()
        {
            if (Time.realtimeSinceStartup - _lastTimeLoadAppOpenTimestamp <
                adSetting.AdLoadingInterval) return;
            if (IsAdClientReady(adSetting.IsApplovin(), maxAdClient)) maxAdClient.LoadAppOpen();
            if (IsAdClientReady(adSetting.IsAdmob(), admobAdClient)) admobAdClient.LoadAppOpen();
            if (IsAdClientReady(adSetting.IsLevelPlay(), levelplayAdClient)) levelplayAdClient.LoadAppOpen();
            _lastTimeLoadAppOpenTimestamp = Time.realtimeSinceStartup;
        }

        private static bool IsAdClientReady(bool isEnabled, AdClient adClient)
        {
            return isEnabled && adClient != null && adClient.SdkInitializationCompleted;
        }

        #endregion

        private void ShowAdDebuger(string adMediation)
        {
            if (adMediation.Equals(AdMediation.AppLovin.ToString()))
            {
                maxAdClient.ShowAdMediationDebugger();
            }
            else if (adMediation.Equals(AdMediation.Admob.ToString()))
            {
                admobAdClient.ShowAdMediationDebugger();
            }
            else if (adMediation.Equals(AdMediation.LevelPlay.ToString()))
            {
                levelplayAdClient.ShowAdMediationDebugger();
            }
        }

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

            // UMP callbacks are invoked on a Java thread, marshal them back to Unity main thread
            ConsentInformation.Update(request, App.ToMainThread<FormError>(OnConsentInfoUpdated));
#endif
        }

        private void OnConsentInfoUpdated(FormError consentError)
        {
            if (consentError != null)
            {
                Debug.Log("error consentError = " + consentError);
                InitAdClient();
                return;
            }

            if (ConsentInformation.CanRequestAds())
            {
                InitAdClient();
                return;
            }

            ConsentForm.LoadAndShowConsentFormIfRequired(App.ToMainThread<FormError>(formError =>
                {
                    if (formError != null)
                    {
                        Debug.Log("error formError = " + formError);
                        InitAdClient();
                        return;
                    }

                    Debug.Log("ConsentStatus = " + ConsentInformation.ConsentStatus.ToString());
                    Debug.Log("CanRequestAds = " + ConsentInformation.CanRequestAds());

                    InitAdClient();
                })
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