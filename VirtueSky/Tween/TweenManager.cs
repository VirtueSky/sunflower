using UnityEngine;

namespace VirtueSky.Tween
{
    public struct TweenManager
    {
        private static GlobalTween _globalTween;

        public static GlobalTween GlobalTween => _globalTween;

        public static void InitGlobalTween(GlobalTween globalTween)
        {
            TweenManager._globalTween = globalTween;
        }

        public static Coroutine PlayTween(Tween t)
        {
            return _globalTween.PlayTween(t);
        }

        public static Coroutine ChainTweens(params Tween[] tweens)
        {
            return _globalTween.ChainTweens(tweens);
        }

        public static void StopAllTweens()
        {
            _globalTween.StopAllTweens();
        }

        public static void StopTween(Coroutine cor)
        {
            _globalTween.StopTween(cor);
        }
    }
}