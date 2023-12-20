using PrimeTween;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using VirtueSky.Inspector;
using VirtueSky.Events;
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

        [TitleColor("Motion", CustomColor.Yellow, CustomColor.Orange)] [SerializeField]
        private bool isMotion = true;

        [SerializeField] private Ease easingTypes = Ease.OutQuint;

        [SerializeField] private float scale = 0.9f;
        [SerializeField] private bool isShrugOver;
        [SerializeField] private float timeShrug = .2f;
        [SerializeField] private float strength = .2f;

        Vector3 originScale = Vector3.one;
        private bool canShrug = true;
        private Tween _tween;

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
            DoScale();
            if (clickButtonEvent != null)
            {
                clickButtonEvent.Raise();
            }
            else
            {
                Debug.Log($"Click button event ({gameObject.name}) null");
            }
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
                _tween = Tween.Scale(transform, originScale * scale, .15f, easingTypes);
            }
        }

        void Shrug()
        {
            if (isMotion && isShrugOver && canShrug)
            {
                canShrug = false;
                if (isMotion && isShrugOver)
                {
                    transform.Shrug(timeShrug, strength, Ease.OutQuad, () => { canShrug = true; });
                }
            }
        }

        void ResetScale()
        {
            if (isMotion)
            {
                _tween.Stop();
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