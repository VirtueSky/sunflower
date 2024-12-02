using System;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Utils;
using System.Collections;
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
using GoogleMobileAds.Api;
#endif
using VirtueSky.Core;
using VirtueSky.Misc;
using VirtueSky.Tracking;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class AdmobNativeOverlayVariable : AdmobAdUnitVariable
    {
        public enum NativeTemplate
        {
            Small,
            Medium
        }

        [SerializeField] private bool useTestId;
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        [HeaderLine("NativeAd Options", false)] [SerializeField]
        private AdChoicesPlacement adChoicesPlacement;

        [SerializeField] private MediaAspectRatio mediaAspectRatio;
        [SerializeField] private VideoOptions videoOptions;


        [HeaderLine("NativeAd Style", false)] public NativeTemplate nativeTemplate;
        public AdsSize adsSize = AdsSize.MediumRectangle;
        public AdsPosition adsPosition = AdsPosition.Bottom;

        //  public NativeTemplateFontStyle nativeTemplateFontStyle;
        private NativeOverlayAd _nativeOverlayAd;
#endif
        private readonly WaitForSeconds _waitReload = new WaitForSeconds(5f);
        private IEnumerator _reload;

        /// <summary>
        /// Init ads and register callback tracking
        /// </summary>
        public override void Init()
        {
            if (useTestId)
            {
                GetUnitTest();
            }
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            paidedCallback += AppTracking.TrackRevenue;
#endif
        }

        /// <summary>
        /// Load ads
        /// </summary>
        public override void Load()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            if (_nativeOverlayAd != null)
            {
                Destroy();
            }

            var adRequest = new AdRequest();
            var option = new NativeAdOptions
            {
                AdChoicesPlacement = adChoicesPlacement,
                MediaAspectRatio = mediaAspectRatio,
                VideoOptions = videoOptions
            };
            NativeOverlayAd.Load(Id, adRequest, option, AdLoadCallback);
#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            return _nativeOverlayAd != null;
#else
            return false;
#endif
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (_nativeOverlayAd != null)
                _nativeOverlayAd.Show();
#endif
        }

        /// <summary>
        /// destroy native overlay ads
        /// </summary>
        public override void Destroy()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (_nativeOverlayAd != null)
            {
                _nativeOverlayAd.Destroy();
                _nativeOverlayAd = null;
            }
#endif
        }

        /// <summary>
        /// Hide native overlay ads
        /// </summary>
        public void Hide()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (_nativeOverlayAd != null) _nativeOverlayAd.Hide();
#endif
        }


        /// <summary>
        /// Render native overlay ads default
        /// </summary>
        public void RenderAd()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADS
            if (_nativeOverlayAd == null) return;
            _nativeOverlayAd.RenderTemplate(Style(), ConvertSize(), ConvertPosition());
#endif
        }

        /// <summary>
        /// Render native ads according to uiElement, use canvas overlay
        /// </summary>
        /// <param name="uiElement"></param>
        public void RenderAd(RectTransform uiElement)
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADS
            if (_nativeOverlayAd == null) return;
            var screenPosition = uiElement.ToWorldPosition();

            float dpi = Screen.dpi / 160f;
            var admobX = (int)(screenPosition.x / dpi);
            var admobY = (int)((Screen.height - (int)screenPosition.y) / dpi);
            _nativeOverlayAd.RenderTemplate(Style(), admobX, admobY);
#endif
        }

        /// <summary>
        /// Render native ads according to uiElement, use canvas screen-space camera
        /// </summary>
        /// <param name="uiElement"></param>
        /// <param name="canvas"></param>
        public void RenderAd(RectTransform uiElement, Canvas canvas)
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (_nativeOverlayAd == null) return;
            var worldPosition = uiElement.TransformPoint(uiElement.position);
            Vector2 screenPosition = canvas.worldCamera.WorldToScreenPoint(worldPosition);

            float dpi = Screen.dpi / 160f;
            var admobX = (int)(screenPosition.x / dpi);
            var admobY = (int)((Screen.height - (int)screenPosition.y) / dpi);
            _nativeOverlayAd?.RenderTemplate(Style(), admobX, admobY);
#endif
        }


#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        public NativeTemplateStyle Style()
        {
            return new NativeTemplateStyle
            {
                TemplateId = nativeTemplate.ToString().ToLower(),
                // MainBackgroundColor = Color.red,
                // CallToActionText = new NativeTemplateTextStyle
                // {
                //     BackgroundColor = Color.green,
                //     TextColor = Color.white,
                //     FontSize = 9,
                //     Style = nativeTemplateFontStyle
                // }
            };
        }

        AdPosition ConvertPosition()
        {
            return adsPosition switch
            {
                AdsPosition.Top => AdPosition.Top,
                AdsPosition.Bottom => AdPosition.Bottom,
                AdsPosition.TopLeft => AdPosition.TopLeft,
                AdsPosition.TopRight => AdPosition.TopRight,
                AdsPosition.BottomLeft => AdPosition.BottomLeft,
                AdsPosition.BottomRight => AdPosition.BottomRight,
                _ => AdPosition.Center
            };
        }

        AdSize ConvertSize()
        {
            return adsSize switch
            {
                AdsSize.Banner => AdSize.Banner,
                AdsSize.MediumRectangle => AdSize.MediumRectangle,
                AdsSize.IABBanner => AdSize.IABBanner,
                AdsSize.Leaderboard => AdSize.Leaderboard,
                _ => AdSize.MediumRectangle,
            };
        }

        private void AdLoadCallback(NativeOverlayAd ad, LoadAdError error)
        {
            if (error != null || ad == null)
            {
                OnAdFailedToLoad(error);
                return;
            }

            _nativeOverlayAd = ad;
            _nativeOverlayAd.OnAdPaid += OnAdPaided;
            _nativeOverlayAd.OnAdClicked += OnAdClicked;
            _nativeOverlayAd.OnAdFullScreenContentOpened += OnAdOpening;
            _nativeOverlayAd.OnAdFullScreenContentClosed += OnAdClosed;
            OnAdLoaded();
        }

        private void OnAdLoaded()
        {
            Common.CallActionAndClean(ref loadedCallback);
            OnLoadAdEvent?.Invoke();
        }

        private void OnAdClosed()
        {
            Common.CallActionAndClean(ref closedCallback);
            OnClosedAdEvent?.Invoke();
        }

        private void OnAdOpening()
        {
            Common.CallActionAndClean(ref displayedCallback);
            OnDisplayedAdEvent?.Invoke();
        }

        private void OnAdClicked()
        {
            Common.CallActionAndClean(ref clickedCallback);
            OnClickedAdEvent?.Invoke();
        }

        private void OnAdPaided(AdValue value)
        {
            paidedCallback?.Invoke(value.Value / 1000000f,
                "Admob",
                Id,
                "NativeOverlayAd", AdNetwork.Admob.ToString());
        }

        private void OnAdFailedToLoad(LoadAdError error)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
            OnFailedToLoadAdEvent?.Invoke(error.GetMessage());
            if (_reload != null) App.StopCoroutine(_reload);
            _reload = DelayReload();
            App.StartCoroutine(_reload);
        }

        private IEnumerator DelayReload()
        {
            yield return _waitReload;
            Load();
        }
#endif
        [ContextMenu("Get Id test")]
        void GetUnitTest()
        {
#if UNITY_ANDROID
            androidId = "ca-app-pub-3940256099942544/2247696110";
#elif UNITY_IOS
            iOSId = "ca-app-pub-3940256099942544/3986624511";
#endif
        }
    }
}