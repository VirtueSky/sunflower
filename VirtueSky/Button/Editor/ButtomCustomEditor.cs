using UnityEditor;
using UnityEngine;
using VirtueSky.UIButton;
using VirtueSky.UtilsEditor;

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
    private SerializedProperty _useSoundFx;
    private SerializedProperty _playSfxEvent;
    private SerializedProperty _soundDataClickButton;

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
        _useSoundFx = serializedObject.FindProperty("useSoundFx");
        _playSfxEvent = serializedObject.FindProperty("playSfxEvent");
        _soundDataClickButton = serializedObject.FindProperty("soundDataClickButton");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        GUILayout.Space(5);
        GUILayout.Space(5);
        Uniform.DrawGroupFoldout("button_custom_setting", "Setting", () => DrawSetting(), false);

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

    void DrawSetting()
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

        GUILayout.Space(5);
        EditorGUILayout.PropertyField(_useSoundFx);
        if (_useSoundFx.boolValue)
        {
            EditorGUILayout.PropertyField(_playSfxEvent);
            EditorGUILayout.PropertyField(_soundDataClickButton);
        }
    }
}
#endif