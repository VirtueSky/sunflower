using UnityEditor;
using UnityEngine;
using VirtueSky.LevelEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPLevelEditorDrawer
    {
        public static void OnDrawLevelEditor()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("LEVEL EDITOR", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Open Level Editor (Alt+3 / Option+3)"))
            {
                UtilitiesLevelSystemDrawer.OpenLevelEditor();
            }

            GUILayout.EndVertical();
        }
    }
}