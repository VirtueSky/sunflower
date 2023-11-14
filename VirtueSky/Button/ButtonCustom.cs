using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VirtueSky.Attributes;
using VirtueSky.Events;
using VirtueSky.UtilsEditor;

namespace VirtueSky.UIButton
{
    public class ButtonCustom : Button
    {
        public ClickButtonEvent clickButtonEvent;
        [SerializeField] private bool isMotion = true;

        [ShowIf(nameof(isMotion), true)] [Header("Motion")]
        public Ease ease = Ease.OutQuint;

        [ShowIf(nameof(isMotion), true)] public float scale = 0.9f;

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

        [ContextMenu("GetClickButtonEvent")]
        void GetClickButtonEvent()
        {
            clickButtonEvent = CreateAsset.CreateAndGetScriptableAsset<ClickButtonEvent>("/Event");
            EditorUtility.SetDirty(this);
        }
#endif
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(ButtonCustom), true)]
    [CanEditMultipleObjects]
    public class ButtomCustomEditor : UnityEditor.UI.ButtonEditor
    {
        private SerializedProperty _isMotion;
        private SerializedProperty _ease;
        private SerializedProperty _scale;
        private SerializedProperty _clickButtonEvent;

        protected override void OnEnable()
        {
            base.OnEnable();
            _isMotion = serializedObject.FindProperty("isMotion");
            _ease = serializedObject.FindProperty("ease");
            _scale = serializedObject.FindProperty("scale");
            _clickButtonEvent = serializedObject.FindProperty("clickButtonEvent");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(_clickButtonEvent);
            EditorGUILayout.PropertyField(_isMotion);
            if (_isMotion.boolValue == true)
            {
                EditorGUILayout.PropertyField(_ease);
                EditorGUILayout.PropertyField(_scale);
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }
#endif
}