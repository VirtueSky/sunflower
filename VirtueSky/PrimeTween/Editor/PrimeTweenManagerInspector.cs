using System.Collections.Generic;
using PrimeTween;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PrimeTweenManager))]
internal class PrimeTweenManagerInspector : Editor {
    SerializedProperty tweensProp;
    SerializedProperty fixedUpdateTweensProp;
    GUIContent aliveTweenGuiContent;
    GUIContent fixedUpdateTweenGuiContent;

    void OnEnable() {
        tweensProp = serializedObject.FindProperty(nameof(PrimeTweenManager.tweens));
        fixedUpdateTweensProp = serializedObject.FindProperty(nameof(PrimeTweenManager.fixedUpdateTweens));
        Assert.IsNotNull(tweensProp);
        aliveTweenGuiContent = new GUIContent("Tweens");
        fixedUpdateTweenGuiContent = new GUIContent("Fixed update tweens");
    }

    public override void OnInspectorGUI() {
        using (new EditorGUI.DisabledScope(true)) {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(MonoBehaviour), false);
        }
        
        var manager = target as PrimeTweenManager;
        Assert.IsNotNull(manager);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Alive tweens", EditorStyles.label);
        GUILayout.Label(manager.tweensCount.ToString(), EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label( Constants.maxAliveTweens + "", EditorStyles.label);
        GUILayout.Label(manager.maxSimultaneousTweensCount.ToString(), EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Tweens capacity", EditorStyles.label);
        GUILayout.Label(manager.currentPoolCapacity.ToString(), EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.HelpBox("Use " + Constants.setTweensCapacityMethod + " to set tweens capacity.\n" +
                                "To prevent memory allocations during runtime, choose the value that is greater than the maximum number of simultaneous tweens in your game.", MessageType.None);

        drawList(tweensProp, manager.tweens, aliveTweenGuiContent);
        drawList(fixedUpdateTweensProp, manager.fixedUpdateTweens, fixedUpdateTweenGuiContent);
        void drawList(SerializedProperty tweensProp, List<ReusableTween> list, GUIContent guiContent) {
            if (tweensProp.isExpanded) {
                foreach (var tween in list) {
                    if (tween != null && string.IsNullOrEmpty(tween.debugDescription)) {
                        tween.debugDescription = tween.GetDescription();
                    }
                }
            }
            using (new EditorGUI.DisabledScope(true)) {
                EditorGUILayout.PropertyField(tweensProp, guiContent);
            }
        }
    }
}