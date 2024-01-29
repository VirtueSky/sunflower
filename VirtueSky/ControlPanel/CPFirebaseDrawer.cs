using UnityEditor;
using UnityEngine;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPFirebaseDrawer
    {
        public static void OnDrawFirebase()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("FIREBASE", EditorStyles.boldLabel);
            GUILayout.Space(10);
            GUILayout.EndVertical();
        }
    }
}