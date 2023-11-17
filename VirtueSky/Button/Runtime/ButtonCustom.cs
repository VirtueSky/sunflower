using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VirtueSky.Events;
using VirtueSky.Tween;

#if UNITY_EDITOR
using VirtueSky.UtilsEditor;
#endif


namespace VirtueSky.UIButton
{
    public class ButtonCustom : Button
    {
        public ClickButtonEvent clickButtonEvent;
        [SerializeField] private bool isMotion = true;

        [Header("Motion")] [SerializeField] private EasingTypes easingTypes = EasingTypes.QuinticOut;

        [SerializeField] private float scale = 0.9f;

        Vector3 originScale = Vector3.one;
        private Coroutine coroutine;

        protected override void OnEnable()
        {
            base.OnEnable();
            originScale = transform.localScale;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ResetScale();
        }


        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            clickButtonEvent.Raise();
            DoScale();
        }


        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            ResetScale();
        }

        void DoScale()
        {
            if (isMotion)
            {
                if (coroutine != null)
                {
                    TweenManager.instance.StopTween(coroutine);
                }

                coroutine = transform.ScaleTo(originScale * scale, .15f, easingTypes);
            }
        }

        void ResetScale()
        {
            if (isMotion)
            {
                if (coroutine != null)
                {
                    TweenManager.instance.StopTween(coroutine);
                }

                transform.localScale = originScale;
            }
        }
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            GetClickButtonEvent();
        }

        public void GetClickButtonEvent()
        {
            clickButtonEvent = CreateAsset.CreateAndGetScriptableAsset<ClickButtonEvent>("/Event");
            EditorUtility.SetDirty(this);
        }
#endif
    }
}