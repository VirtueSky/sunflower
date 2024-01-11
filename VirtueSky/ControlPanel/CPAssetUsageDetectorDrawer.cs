using UnityEditor;
using UnityEngine;
using VirtueSky.AssetFinder.Editor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPAssetUsageDetectorDrawer
    {
        public static void OnDrawAssetUsageDetector()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("ASSET USAGE DETECTOR", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Active Window"))
            {
                AssetUsageDetectorWindow.OpenActiveWindow();
            }

            if (GUILayout.Button("New Window"))
            {
                AssetUsageDetectorWindow.OpenNewWindow();
            }

            GUILayout.EndVertical();
        }
    }
}