using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using VirtueSky.Attributes;
using VirtueSky.Events;
using VirtueSky.Tween;
using VirtueSky.Misc;
using Button = UnityEngine.UI.Button;

#if UNITY_EDITOR
using VirtueSky.UtilsEditor;
#endif


namespace VirtueSky.UIButton
{
    public class ButtonCustom : Button
    {
        public ClickButtonEvent clickButtonEvent;

        [Title("Motion", CustomColor.Yellow, CustomColor.Orange)] [SerializeField]
        private bool isMotion = true;

        [SerializeField] private EasingTypes easingTypes = EasingTypes.QuinticOut;

        [SerializeField] private float scale = 0.9f;
        [SerializeField] private bool isShrugOver;
        [SerializeField] private float timeShrug = .2f;
        [SerializeField] private float strength = .2f;

        Vector3 originScale = Vector3.one;
        private Coroutine coroutine;
        private Coroutine coroutine2;
        private bool canShrug = true;

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

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            Shrug();
        }

        void DoScale()
        {
            if (isMotion)
            {
                if (coroutine != null)
                {
                    TweenManager.StopTween(coroutine);
                }

                coroutine = transform.ScaleTo(originScale * scale, .15f, easingTypes);
            }
        }

        void Shrug()
        {
            if (isMotion && isShrugOver && canShrug)
            {
                canShrug = false;
                if (isMotion && isShrugOver)
                {
                    transform.Shrug(.2f, .2f, EasingTypes.QuadraticOut, () => { canShrug = true; });
                }
            }
        }

        void ResetScale()
        {
            if (isMotion)
            {
                if (coroutine != null)
                {
                    TweenManager.StopTween(coroutine);
                }

                // if (coroutine2 != null)
                // {
                //     TweenManager.StopTween(coroutine2);
                // }

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