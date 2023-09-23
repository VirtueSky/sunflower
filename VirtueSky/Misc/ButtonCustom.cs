using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VirtueSky.EditorUtils;
using VirtueSky.Events;
using VirtueSky.Utils;

namespace VirtueSky.Misc
{
    public class ButtonCustom : Button
    {
        [Header("Motion")] public Ease ease = Ease.OutQuint;

        public float scale = 0.9f;
        public ClickButtonEvent clickButtonEvent;
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
            // m_OnClick.Invoke();
        }

        void DoScale()
        {
            DOTween.Kill(this);
            transform.DOScale(originScale * scale, 0.15f).SetEase(ease).SetUpdate(true).SetTarget(this).Play();
        }

        void ResetScale()
        {
            DOTween.Kill(this);
            transform.localScale = originScale;
        }
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            clickButtonEvent = ScriptableSetting.CreateAndGetScriptableAsset<ClickButtonEvent>("/Event");
        }
#endif
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(ButtonCustom), true)]
    [CanEditMultipleObjects]
    public class ButtomCustomEditor : UnityEditor.UI.ButtonEditor
    {
        private SerializedProperty _ease;
        private SerializedProperty _scale;
        private SerializedProperty _clickButtonEvent;

        protected override void OnEnable()
        {
            base.OnEnable();
            _ease = serializedObject.FindProperty("ease");
            _scale = serializedObject.FindProperty("scale");
            _clickButtonEvent = serializedObject.FindProperty("clickButtonEvent");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(_ease);
            EditorGUILayout.PropertyField(_scale);
            EditorGUILayout.PropertyField(_clickButtonEvent);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }
#endif
}