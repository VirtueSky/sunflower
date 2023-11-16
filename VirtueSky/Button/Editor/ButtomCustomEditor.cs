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
    private SerializedProperty _clickButtonEvent;

    protected override void OnEnable()
    {
        base.OnEnable();
        _buttonCustom = target as ButtonCustom;
        _isMotion = serializedObject.FindProperty("isMotion");
        _ease = serializedObject.FindProperty("ease");
        _scale = serializedObject.FindProperty("scale");
        _clickButtonEvent = serializedObject.FindProperty("clickButtonEvent");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_clickButtonEvent);

        GUILayout.Space(2);
        if (GUILayout.Button("Create", GUILayout.Width(55)))
        {
            _buttonCustom.GetClickButtonEvent();
        }

        EditorGUILayout.EndHorizontal();

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