using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VirtueSky.Events;

#if UNITY_EDITOR
using VirtueSky.UtilsEditor;
#endif


namespace VirtueSky.UIButton
{
    public class ButtonCustom : Button
    {
        public ClickButtonEvent clickButtonEvent;
        [SerializeField] private bool isMotion = true;

        [Header("Motion")] [SerializeField] private Ease ease = Ease.OutQuint;

        [SerializeField] private float scale = 0.9f;

        Vector3 originScale = Vector3.one;

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


        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }

        void DoScale()
        {
            if (isMotion)
            {
                DOTween.Kill(this);
                transform.DOScale(originScale * scale, 0.15f).SetEase(ease).SetUpdate(true).SetTarget(this).Play();
            }
        }

        void ResetScale()
        {
            if (isMotion)
            {
                DOTween.Kill(this);
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