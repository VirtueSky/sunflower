using UnityEditor;
using VirtueSky.Tracking;

namespace VirtueSky.TrackingEditor
{
    [CustomEditor(typeof(AdjustSettings))]
    public class AdjustSettingsEditor : Editor
    {
        private SerializedProperty appToken;
#if VIRTUESKY_ADJUST
        private SerializedProperty adjustEnvironment;
        private SerializedProperty logLevel;
#endif


        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            appToken = serializedObject.FindProperty("appToken");
#if VIRTUESKY_ADJUST
            adjustEnvironment = serializedObject.FindProperty("adjustEnvironment");
            logLevel = serializedObject.FindProperty("logLevel");
#endif
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(appToken);
#if VIRTUESKY_ADJUST
            EditorGUILayout.PropertyField(adjustEnvironment);
            EditorGUILayout.PropertyField(logLevel);
#endif
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }
    }
}