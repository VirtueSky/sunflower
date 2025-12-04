using UnityEditor;
using UnityEngine;
using VirtueSky.AssetFinder.Editor;
using VirtueSky.UtilsEditor;

//using VirtueSky.AssetFinder.Editor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPAssetFinderDrawer
    {
        public static void OnDrawAssetUsageDetector()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.AssetsFinder, "Asset Finder");
            GUILayout.Space(10);
            if (GUILayout.Button("Open Asset Finder Window (Ctrl+Shift+K / Command+Shift+K)"))
            {
                AssetFinderWindowExtension.ShowWindow();
            }

            if (GUILayout.Button("Delete Finder Cache"))
            {
                AssetFinderWindowExtension.DeleteCache();
            }
            GUILayout.Space(10);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.ASSET_FINDER_ADDRESSABLE);

            GUILayout.EndVertical();
        }
    }
}