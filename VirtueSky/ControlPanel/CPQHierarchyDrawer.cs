using qtools.qhierarchy.phierarchy;
using UnityEditor;
using UnityEngine;


namespace VirtueSky.ControlPanel.Editor
{
    public class CPQHierarchyDrawer
    {
        public static void OnDrawQHierarchyEvent()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("Q-HIERARCHY", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Open QHierarchy Settings"))
            {
                QHierarchySettingsWindow.ShowWindow();
            }

            GUILayout.EndVertical();
        }
    }
}