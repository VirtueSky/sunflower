using PrimeTween;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PrimeTweenManager))]
internal class PrimeTweenManagerInspector : Editor {
    SerializedProperty tweensProp;
    GUIContent aliveTweenGuiContent;
    const string aliveTweensLabel = "Alive tweens";

    void OnEnable() {
        tweensProp = serializedObject.FindProperty(nameof(PrimeTweenManager.tweens));
        Assert.IsNotNull(tweensProp);
        aliveTweenGuiContent = new GUIContent(aliveTweensLabel);
    }

    public override void OnInspectorGUI() {
        using (new EditorGUI.DisabledScope(true)) {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(MonoBehaviour), false);
        }
        
        var manager = target as PrimeTweenManager;
        Assert.IsNotNull(manager);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label(aliveTweensLabel, EditorStyles.label);
        GUILayout.Label(manager.tweens.Count.ToString(), EditorStyles.boldLabel);
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

        if (tweensProp.isExpanded) {
            foreach (var tween in manager.tweens) {
                if (tween != null && string.IsNullOrEmpty(tween.debugDescription)) {
                    tween.debugDescription = tween.GetDescription();
                }
            }
        }
        using (new EditorGUI.DisabledScope(true)) {
            EditorGUILayout.PropertyField(tweensProp, aliveTweenGuiContent);
        }
    }
}