#if VIRTUESKY_APPSFLYER
using AppsFlyerSDK;
#endif

using System;
using UnityEngine;

namespace VirtueSky.Tracking
{
    public class AppsFlyerObject : MonoBehaviour
    {
        private void Awake()
        {
#if !UNITY_EDITOR
            DontDestroyOnLoad(this);
#endif
        }

        private void Start()
        {
#if VIRTUESKY_APPSFLYER
            // These fields are set from the editor so do not modify!
            //******************************//
            AppsFlyer.setIsDebug(AppsFlyerSetting.IsDebug);
#if UNITY_WSA_10_0 && !UNITY_EDITOR
            AppsFlyer.initSDK(AppsFlyerSetting.DevKey, AppsFlyerSetting.UWPAppID,
                AppsFlyerSetting.GetConversionData ? this : null);
#elif UNITY_STANDALONE_OSX && !UNITY_EDITOR
            AppsFlyer.initSDK(AppsFlyerSetting.DevKey, AppsFlyerSetting.MacOSAppID,
                AppsFlyerSetting.GetConversionData ? this : null);
#else

            AppsFlyer.initSDK(AppsFlyerSetting.DevKey, AppsFlyerSetting.AppID,
                AppsFlyerSetting.GetConversionData ? this : null);
#endif
            //******************************/

            AppsFlyer.startSDK();
#endif
        }
    }
}