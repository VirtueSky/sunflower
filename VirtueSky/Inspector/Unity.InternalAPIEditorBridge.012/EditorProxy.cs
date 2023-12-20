using UnityEditor;

namespace VirtueSky.InspectorUnityInternalBridge
{
    public static class EditorProxy
    {
        public static void DoDrawDefaultInspector(SerializedObject obj)
        {
            Editor.DoDrawDefaultInspector(obj);
        }
    }
}