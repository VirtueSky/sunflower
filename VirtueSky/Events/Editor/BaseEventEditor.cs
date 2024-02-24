using UnityEditor;
using UnityEngine;

namespace VirtueSky.Events
{
    // [CustomEditor(typeof(BaseEvent), true)]
    // public class BaseEventEditor : UnityEditor.Editor
    // {
    //     BaseEvent baseEvent;
    //     private bool enableDebugField;
    //
    //     void OnEnable()
    //     {
    //         baseEvent = target as BaseEvent;
    //     }
    //
    //     public override void OnInspectorGUI()
    //     {
    //         base.OnInspectorGUI();
    //         GUILayout.Space(10);
    //         enableDebugField = GUILayout.Toggle(enableDebugField, "Enable Debug Field");
    //         GUILayout.Space(10);
    //         if (enableDebugField)
    //         {
    //             if (GUILayout.Button("Raise"))
    //             {
    //                 baseEvent.Raise();
    //             }
    //         }
    //     }
    // }
}