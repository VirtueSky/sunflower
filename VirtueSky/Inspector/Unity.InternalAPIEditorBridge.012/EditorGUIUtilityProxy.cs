using UnityEditor;
using UnityEngine;

namespace VirtueSky.InspectorUnityInternalBridge
{
    public static class EditorGUIUtilityProxy
    {
        public static Texture2D GetHelpIcon(MessageType type)
        {
            return EditorGUIUtility.GetHelpIcon(type);
        }
    }
}