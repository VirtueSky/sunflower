using UnityEditor;
using UnityEngine;
using VirtueSky.AssetFinder.Editor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPAssetFinderDrawer
    {
        public static void OnDrawAssetUsageDetector()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("ASSET FINDER", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Open Asset Finder Window (Ctrl+Shift+K / Command+Shift+K)"))
            {
                AssetFinderWindowAll.ShowWindow();
            }

            if (GUILayout.Button("Delete Finder Cache"))
            {
                AssetFinderWindowAll.DeleteFinderCache();
            }

            GUILayout.EndVertical();
        }
    }
}