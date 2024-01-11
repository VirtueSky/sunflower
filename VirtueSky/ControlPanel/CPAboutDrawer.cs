using System;
using UnityEditor;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPAboutDrawer
    {
        public static void OnDrawAbout(Rect position, Action drawSetting)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("ABOUT", EditorStyles.boldLabel);
            GUILayout.Space(10);
            GUILayout.TextArea("Name: Sunflower", EditorStyles.boldLabel);
            GUILayout.TextArea(
                "Description: Core ScriptableObject architecture for building Unity games",
                EditorStyles.boldLabel);
            GUILayout.TextArea("Version: 2.3.3", EditorStyles.boldLabel);
            GUILayout.TextArea("Author: VirtueSky", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Open GitHub Repository"))
            {
                Application.OpenURL("https://github.com/VirtueSky/sunflower");
            }

            if (GUILayout.Button("Document"))
            {
                Application.OpenURL("https://github.com/VirtueSky/sunflower/wiki");
            }

            Handles.DrawAAPolyLine(3, new Vector3(210, 195), new Vector3(position.width, 195));
            GUILayout.Space(20);
            GUILayout.Label("SETUP THEME", EditorStyles.boldLabel);
            GUILayout.Space(10);
            drawSetting?.Invoke();
            GUILayout.EndVertical();
        }
    }
}