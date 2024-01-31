using UnityEditor;
using UnityEngine;
using VirtueSky.UIButton;

#if UNITY_EDITOR
[CustomEditor(typeof(ButtonCustom), true)]
[CanEditMultipleObjects]
public class ButtomCustomEditor : UnityEditor.UI.ButtonEditor
{
    private ButtonCustom _buttonCustom;
    private SerializedProperty _isMotion;
    private SerializedProperty _ease;
    private SerializedProperty _scale;
    private SerializedProperty _easingTypes;
    private SerializedProperty _clickButtonEvent;
    private SerializedProperty _isShrugOver;
    private SerializedProperty _timeShrug;
    private SerializedProperty _strength;
    private bool isShowCustom;

    protected override void OnEnable()
    {
        base.OnEnable();
        _buttonCustom = target as ButtonCustom;
        _isMotion = serializedObject.FindProperty("isMotion");
        _easingTypes = serializedObject.FindProperty("easingTypes");
        _scale = serializedObject.FindProperty("scale");
        _clickButtonEvent = serializedObject.FindProperty("clickButtonEvent");
        _isShrugOver = serializedObject.FindProperty("isShrugOver");
        _timeShrug = serializedObject.FindProperty("timeShrug");
        _strength = serializedObject.FindProperty("strength");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        GUILayout.Space(5);
        isShowCustom = GUILayout.Toggle(isShowCustom, "Custom");
        GUILayout.Space(5);
        if (isShowCustom)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_clickButtonEvent);

            GUILayout.Space(2);
            if (GUILayout.Button("Create", GUILayout.Width(55)))
            {
                _buttonCustom.GetClickButtonEvent();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(_isMotion);
            if (_isMotion.boolValue)
            {
                EditorGUILayout.PropertyField(_easingTypes);
                EditorGUILayout.PropertyField(_scale);
                EditorGUILayout.PropertyField(_isShrugOver);
                if (_isShrugOver.boolValue)
                {
                    EditorGUILayout.PropertyField(_timeShrug);
                    EditorGUILayout.PropertyField(_strength);
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}
#endif