using VirtueSky.Inspector;

namespace VirtueSky.Rating
{
    using UnityEngine;
    using System.Collections;
    using VirtueSky.Core;

#if UNITY_IOS
     using UnityEngine.iOS;
#elif UNITY_ANDROID && VIRTUESKY_RATING
    using Google.Play.Review;
#endif


    [CreateAssetMenu(menuName = "Sunflower/InAppReview", fileName = "in_app_review")]
    [EditorIcon("icon_scriptable")]
    public class InAppReview : ScriptableObject
    {
#if UNITY_ANDROID && VIRTUESKY_RATING
        private ReviewManager _reviewManager;
        private PlayReviewInfo _playReviewInfo;
        private Coroutine _coroutine;
#endif

        public void InitInAppReview()
        {
            if (!Application.isMobilePlatform) return;
#if UNITY_ANDROID && VIRTUESKY_RATING
            _coroutine = App.StartCoroutine(InitReview());
#endif
        }

        public void RateAndReview()
        {
            if (!Application.isMobilePlatform) return;

#if UNITY_ANDROID && VIRTUESKY_RATING
            App.StartCoroutine(LaunchReview());
#elif UNITY_IOS
            Device.RequestStoreReview();
#endif
        }

#if UNITY_ANDROID && VIRTUESKY_RATING
        private IEnumerator InitReview(bool force = false)
        {
            if (_reviewManager == null) _reviewManager = new ReviewManager();

            var requestFlowOperation = _reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                if (force) DirectlyOpen();
                yield break;
            }

            _playReviewInfo = requestFlowOperation.GetResult();
        }

        public IEnumerator LaunchReview()
        {
            if (_playReviewInfo == null)
            {
                if (_coroutine != null) App.StopCoroutine(_coroutine);
                yield return App.StartCoroutine(InitReview(true));
            }

            var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
            yield return launchFlowOperation;
            _playReviewInfo = null;
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                DirectlyOpen();
                yield break;
            }
        }
#endif
        private void DirectlyOpen()
        {
            Application.OpenURL($"https://play.google.com/store/apps/details?id={Application.identifier}");
        }
    }
}