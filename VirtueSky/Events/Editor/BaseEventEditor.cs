using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.Events
{
    // [CustomEditor(typeof(BaseEvent), true)]
    // public class BaseEventNoParamEditor : Editor
    // {
    //     private MethodInfo _methodInfo;
    //
    //     void OnEnable()
    //     {
    //         _methodInfo = target.GetType().BaseType.GetMethod("DebugRaiseEvent",
    //             BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    //     }
    //
    //     public override void OnInspectorGUI()
    //     {
    //         base.OnInspectorGUI();
    //         GUILayout.Space(10);
    //         GUI.enabled = EditorApplication.isPlaying;
    //         if (GUILayout.Button("Raise"))
    //         {
    //             _methodInfo.Invoke(target, null);
    //         }
    //
    //         GUI.enabled = true;
    //     }
    // }
    //
    // [CustomEditor(typeof(BaseEvent<>), true)]
    // public class BaseEventParamEditor : UnityEditor.Editor
    // {
    //     private MethodInfo _methodInfo;
    //
    //     void OnEnable()
    //     {
    //         _methodInfo = target.GetType().BaseType.GetMethod("DebugRaiseEvent",
    //             BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    //     }
    //
    //     public override void OnInspectorGUI()
    //     {
    //         base.OnInspectorGUI();
    //         GUILayout.Space(10);
    //         GUI.enabled = EditorApplication.isPlaying;
    //         if (GUILayout.Button("Raise"))
    //         {
    //             _methodInfo.Invoke(target, null);
    //         }
    //
    //         GUI.enabled = true;
    //     }
    // }
    //
    // [CustomEditor(typeof(BaseEvent<,>), true)]
    // public class BaseEventResultEditor : UnityEditor.Editor
    // {
    //     private MethodInfo _methodInfo;
    //     private SerializedProperty _valueResult;
    //
    //     void OnEnable()
    //     {
    //         _methodInfo = target.GetType().BaseType.GetMethod("DebugRaiseEvent",
    //             BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    //     }
    //
    //     public override void OnInspectorGUI()
    //     {
    //         base.OnInspectorGUI();
    //         GUILayout.Space(10);
    //         _valueResult = serializedObject.FindProperty("valueResult");
    //         GUI.enabled = EditorApplication.isPlaying;
    //
    //         if (GUILayout.Button("Raise"))
    //         {
    //             _methodInfo.Invoke(target, null);
    //         }
    //
    //         EditorGUILayout.PropertyField(_valueResult);
    //         GUI.enabled = true;
    //     }
    // }
}