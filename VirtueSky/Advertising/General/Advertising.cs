using System;
using System.Collections;
using UnityEngine;
using VirtueSky.EditorUtils;
using VirtueSky.Events;

namespace VirtueSky.Ads
{
    public class Advertising : MonoBehaviour
    {
        [SerializeField] private AdSetting adSetting;
        private BooleanEvent changePreventDisplayAppOpenEvent;
        private IEnumerator autoLoadAdCoroutine;
        private float _lastTimeLoadInterstitialAdTimestamp = DEFAULT_TIMESTAMP;
        private float _lastTimeLoadRewardedTimestamp = DEFAULT_TIMESTAMP;
        private float _lastTimeLoadRewardedInterstitialTimestamp = DEFAULT_TIMESTAMP;
        private float _lastTimeLoadAppOpenTimestamp = DEFAULT_TIMESTAMP;
        private const float DEFAULT_TIMESTAMP = -1000;

        private AdClient currentAdClient;

        private void Start()
        {
            switch (adSetting.CurrentAdNetwork)
            {
                case AdNetwork.Applovin:
                    currentAdClient = adSetting.MaxAdClient;
                    break;
                case AdNetwork.Admob:
                    currentAdClient = adSetting.AdmobAdClient;
                    break;
            }

            currentAdClient.Initialize();
            if (autoLoadAdCoroutine != null) StopCoroutine(autoLoadAdCoroutine);
            autoLoadAdCoroutine = IeAutoLoadAll();
            StartCoroutine(autoLoadAdCoroutine);
            Debug.Log("currentAdClient: " + currentAdClient.name);
        }

        public void OnChangePreventDisplayOpenAd(bool state)
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
            if (Time.realtimeSinceStartup - _lastTimeLoadInterstitialAdTimestamp < adSetting.AdLoadingInterval) return;
            currentAdClient.LoadInterstitial();
            _lastTimeLoadInterstitialAdTimestamp = Time.realtimeSinceStartup;
        }

        void AutoLoadRewardAds()
        {
            if (Time.realtimeSinceStartup - _lastTimeLoadRewardedTimestamp < adSetting.AdLoadingInterval) return;
            currentAdClient.LoadRewarded();
            _lastTimeLoadRewardedTimestamp = Time.realtimeSinceStartup;
        }

        void AutoLoadRewardInterAds()
        {
            if (Time.realtimeSinceStartup - _lastTimeLoadRewardedInterstitialTimestamp < adSetting.AdLoadingInterval) return;
            currentAdClient.LoadRewardedInterstitial();
            _lastTimeLoadRewardedInterstitialTimestamp = Time.realtimeSinceStartup;
        }

        void AutoLoadAppOpenAds()
        {
            if (Time.realtimeSinceStartup - _lastTimeLoadAppOpenTimestamp < adSetting.AdLoadingInterval) return;
            currentAdClient.LoadAppOpen();
            _lastTimeLoadAppOpenTimestamp = Time.realtimeSinceStartup;
        }

        #endregion

#if ADS_APPLOVIN
        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus) (currentAdClient as MaxAdClient)?.ShowAppOpen();
        }
#endif

#if UNITY_EDITOR
        private void Reset()
        {
            adSetting = ScriptableSetting.CreateAndGetScriptableAsset<VirtueSky.Ads.AdSetting>("/Ads");
        }
#endif
    }
}