using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.TouchInput
{
    [CustomEditor(typeof(TouchInputManager))]
    public class TouchInputManagerEditor : Editor
    {
        private SerializedProperty _inputEventTouchBegin;
        private SerializedProperty _inputEventTouchMove;
        private SerializedProperty _inputEventTouchStationary;
        private SerializedProperty _inputEventTouchEnd;
        private SerializedProperty _inputEventTouchCancel;
        private SerializedProperty _inputPreventTouchVariable;
        private SerializedProperty _preventTouch;
        private SerializedProperty _touchPosition;
        private const string path = "/InputEvent";

        void Init()
        {
            _inputEventTouchBegin = serializedObject.FindProperty("inputEventTouchBegin");
            _inputEventTouchMove = serializedObject.FindProperty("inputEventTouchMove");
            _inputEventTouchStationary = serializedObject.FindProperty("inputEventTouchStationary");
            _inputEventTouchEnd = serializedObject.FindProperty("inputEventTouchEnd");
            _inputEventTouchCancel = serializedObject.FindProperty("inputEventTouchCancel");
            _inputPreventTouchVariable = serializedObject.FindProperty("inputPreventTouchVariable");
            _preventTouch = serializedObject.FindProperty("preventTouch");
            _touchPosition = serializedObject.FindProperty("touchPosition");
        }

        private void OnEnable()
        {
            Init();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawEvent<InputEventTouchBegin>(ref _inputEventTouchBegin, "input_event_touch_begin");
            DrawEvent<InputEventTouchMove>(ref _inputEventTouchMove, "input_event_touch_move");
            DrawEvent<InputEventTouchStationary>(ref _inputEventTouchStationary, "input_event_touch_stationary");
            DrawEvent<InputEventTouchEnd>(ref _inputEventTouchEnd, "input_event_touch_end");
            DrawEvent<InputEventTouchCancel>(ref _inputEventTouchCancel, "input_event_touch_cancel");
            GUILayout.Space(5);
            DrawEvent<InputPreventTouchVariable>(ref _inputPreventTouchVariable, "input_prevent_touch_variable");
            GUILayout.Space(5);
            if (Application.isPlaying)
            {
                EditorGUILayout.PropertyField(_preventTouch);
            }

            GUILayout.Space(5);
            if (Application.isPlaying)
            {
                EditorGUILayout.PropertyField(_touchPosition);
            }

            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
            //serializedObject.Update();
        }

        void DrawEvent<T>(ref SerializedProperty inputEvent, string eventName)
            where T : ScriptableObject
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(inputEvent);
            if (inputEvent.objectReferenceValue == null)
            {
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    inputEvent.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<T>(path, eventName, false);
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}