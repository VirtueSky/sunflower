using UnityEngine;
using UnityEngine.UI;
using System;

namespace VirtueSky.Tween
{
    public static class ExtensionMethods
    {
        #region Transform Easing

        /// <summary>
        /// Eases the  transform position to the target position. If no start position value is provided, the current position
        /// will serve as the start value
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="trans">Trans.</param>
        /// <param name="start">Start.</param>
        /// <param name="target">Target.</param>
        /// <param name="length">Length.</param>
        /// <param name="easingChoice">Easing choice.</param>
        /// <param name="unscaled">If set to <c>true</c> unscaled.</param>
        /// <param name="repeat">Repeat.</param>
        /// <param name="onComplete">On complete.</param>
        public static Coroutine MoveTo(this Transform trans, Vector3 start, Vector3 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            return TweenManager.instance.PlayTween(new Tween((Vector3 pos) => { trans.position = pos; }, start, target, length, easingChoice, unscaled, repeat, onComplete));
        }

        public static Coroutine MoveTo(this Transform trans, Vector3 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            Vector3 start = trans.position;
            return VirtueSky.Tween.ExtensionMethods.MoveTo(trans, start, target, length,
                easingChoice, unscaled, repeat, onComplete);
        }

        /// <summary>
        /// Eases the local transform position to the target local position. If no start position value is provided, the current position
        /// will serve as the start value
        /// </summary>
        /// <returns>The move to.</returns>
        /// <param name="trans">Trans.</param>
        /// <param name="start">Start.</param>
        /// <param name="target">Target.</param>
        /// <param name="length">Length.</param>
        /// <param name="easingChoice">Easing choice.</param>
        /// <param name="unscaled">If set to <c>true</c> unscaled.</param>
        /// <param name="repeat">Repeat.</param>
        /// <param name="onComplete">On complete.</param>
        public static Coroutine LocalMoveTo(this Transform trans, Vector3 start, Vector3 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            return TweenManager.instance.PlayTween(new Tween((Vector3 pos) => { trans.localPosition = pos; }, start, target, length, easingChoice, unscaled, repeat, onComplete));
        }

        public static Coroutine LocalMoveTo(this Transform trans, Vector3 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            Vector3 start = trans.localPosition;
            return VirtueSky.Tween.ExtensionMethods.LocalMoveTo(trans, start, target, length,
                easingChoice, unscaled, repeat, onComplete);
        }

        /// <summary>
        /// Eases the transform scale to the target scale. If no start scale value is provided, the current transform
        /// scale will serve as the start value
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="trans">Trans.</param>
        /// <param name="start">Start.</param>
        /// <param name="target">Target.</param>
        /// <param name="length">Length.</param>
        /// <param name="easingChoice">Easing choice.</param>
        /// <param name="unscaled">If set to <c>true</c> unscaled.</param>
        /// <param name="repeat">Repeat.</param>
        /// <param name="onComplete">On complete.</param>
        public static Coroutine ScaleTo(this Transform trans, Vector3 start, Vector3 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            return TweenManager.instance.PlayTween(new Tween((Vector3 scale) => { trans.localScale = scale; }, start, target, length, easingChoice, unscaled, repeat, onComplete));
        }

        public static Coroutine ScaleTo(this Transform trans, Vector3 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            Vector3 start = trans.localScale;
            return ExtensionMethods.ScaleTo(trans, start, target, length,
                easingChoice, unscaled, repeat, onComplete);
        }

        public static Coroutine RotateTo(this Transform trans, Vector3 start, Vector3 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            return VirtueSky.Tween.ExtensionMethods.RotateTo(trans, Quaternion.Euler(start), Quaternion.Euler(target), length,
                easingChoice, unscaled, repeat, onComplete);
        }

        public static Coroutine RotateTo(this Transform trans, Vector3 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            return VirtueSky.Tween.ExtensionMethods.RotateTo(trans, Quaternion.Euler(target), length,
                easingChoice, unscaled, repeat, onComplete);
        }

        public static Coroutine RotateTo(this Transform trans, Quaternion target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            Quaternion start = trans.localRotation;
            return ExtensionMethods.RotateTo(trans, start, target, length,
                easingChoice, unscaled, repeat, onComplete);
        }

        /// <summary>
        /// Eases the transform rotation to the target rotation. If no start rotation value is provided, the current transform
        /// rotation will serve as the start value
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="trans">Trans.</param>
        /// <param name="start">Start.</param>
        /// <param name="target">Target.</param>
        /// <param name="length">Length.</param>
        /// <param name="easingChoice">Easing choice.</param>
        /// <param name="unscaled">If set to <c>true</c> unscaled.</param>
        /// <param name="repeat">Repeat.</param>
        /// <param name="onComplete">On complete.</param>
        public static Coroutine RotateTo(this Transform trans, Quaternion start, Quaternion target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            return TweenManager.instance.PlayTween(new Tween((Quaternion rot) => { trans.localRotation = rot; }, start, target, length, easingChoice, unscaled, repeat, onComplete));
        }

        #endregion

        public static Coroutine FadeTo(this CanvasGroup cgroup, float target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            float start = cgroup.alpha;
            return TweenManager.instance.PlayTween(new Tween((float newAlpha) => { cgroup.alpha = newAlpha; }, start, target, length, easingChoice, unscaled, repeat, onComplete));
        }

        public static Coroutine EaseFill(this Image image, float target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            float start = image.fillAmount;
            return TweenManager.instance.PlayTween(new Tween((float newFill) => { image.fillAmount = newFill; }, start, target, length, easingChoice, unscaled, repeat, onComplete));
        }

        public static Coroutine EaseLayoutMinValues(this LayoutElement layout, Vector2 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            Vector2 start = new Vector2(layout.minWidth, layout.minHeight);
            return TweenManager.instance.PlayTween(new Tween((Vector2 newDimensions) =>
            {
                layout.minWidth = newDimensions.x;
                layout.minHeight = newDimensions.y;
            }, start, target, length, easingChoice, unscaled, repeat, onComplete));
        }

        public static Coroutine Jump(this ScrollRect scrollRect, Vector2 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            Vector2 start = scrollRect.normalizedPosition;
            return TweenManager.instance.PlayTween(new Tween((Vector2 newPosition) => { scrollRect.normalizedPosition = newPosition; }, start, target, length, easingChoice, unscaled,
                repeat, onComplete));
        }

        public static Coroutine JumpVertical(this ScrollRect scrollRect, float target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            float start = scrollRect.verticalNormalizedPosition;
            return TweenManager.instance.PlayTween(new Tween((float newPosition) => { scrollRect.verticalNormalizedPosition = newPosition; }, start, target, length, easingChoice,
                unscaled, repeat, onComplete));
        }

        public static Coroutine JumpHorizontal(this ScrollRect scrollRect, float target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            float start = scrollRect.horizontalNormalizedPosition;
            return TweenManager.instance.PlayTween(new Tween((float newPosition) => { scrollRect.horizontalNormalizedPosition = newPosition; }, start, target, length, easingChoice,
                unscaled, repeat, onComplete));
        }

        public static Coroutine AnchoredPositionEase(this RectTransform rect, Vector3 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            Vector3 start = rect.anchoredPosition3D;
            return TweenManager.instance.PlayTween(new Tween((Vector3 newPosition) => { rect.anchoredPosition3D = newPosition; }, start, target, length, easingChoice, unscaled, repeat,
                onComplete));
        }

        public static Coroutine PivotEase(this RectTransform rect, Vector2 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            Vector2 start = rect.pivot;
            return TweenManager.instance.PlayTween(new Tween((Vector2 newPivot) => { rect.pivot = newPivot; }, start, target, length, easingChoice, unscaled, repeat, onComplete));
        }

        public static Coroutine AnchorMaxEase(this RectTransform rect, Vector2 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            Vector2 start = rect.anchorMax;
            return TweenManager.instance.PlayTween(new Tween((Vector2 newMax) => { rect.anchorMax = newMax; }, start, target, length, easingChoice, unscaled, repeat, onComplete));
        }

        public static Coroutine AnchorMinEase(this RectTransform rect, Vector2 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            Vector2 start = rect.anchorMin;
            return TweenManager.instance.PlayTween(new Tween((Vector2 newMin) => { rect.anchorMin = newMin; }, start, target, length, easingChoice, unscaled, repeat, onComplete));
        }

        public static Coroutine OffsetMaxEase(this RectTransform rect, Vector2 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            Vector2 start = rect.offsetMax;
            return TweenManager.instance.PlayTween(new Tween((Vector2 newMax) => { rect.offsetMax = newMax; }, start, target, length, easingChoice, unscaled, repeat, onComplete));
        }

        public static Coroutine OffsetMinEase(this RectTransform rect, Vector2 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            Vector2 start = rect.offsetMin;
            return TweenManager.instance.PlayTween(new Tween((Vector2 newMin) => { rect.anchorMin = newMin; }, start, target, length, easingChoice, unscaled, repeat, onComplete));
        }

        public static Coroutine SizeDeltaEase(this RectTransform rect, Vector2 target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            Vector2 start = rect.sizeDelta;
            return TweenManager.instance.PlayTween(new Tween((Vector2 newDelta) => { rect.sizeDelta = newDelta; }, start, target, length, easingChoice, unscaled, repeat, onComplete));
        }

        public static Coroutine EaseMainColor(this Material material, Color target, float length, EasingTypes easingChoice, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once,
            Action onComplete = null)
        {
            Color start = material.color;
            return TweenManager.instance.PlayTween(new Tween((Color newColor) => { material.color = newColor; }, start, target, length, easingChoice, unscaled, repeat, onComplete));
        }
    }
}