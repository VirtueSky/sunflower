using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

using UnityObject = UnityEngine.Object;
using AddUsageCB = System.Action<string, long>;


namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderParser // BinaryAsset
    {
        private static HashSet<string> BINARY_ASSET = new HashSet<string>()
        {
            ".asset", ".spriteatlas", ".unity"
        };
        
        private static bool Read_VerifyBinaryAsset(string assetPath)
        {
            try
            {
                foreach (string line in File.ReadLines(assetPath))
                {
                    return !line.StartsWith("%YAML", StringComparison.Ordinal);
                }
            } catch (Exception e)
            {
                LogWarning($"Read_VerifyBinaryAsset error: {assetPath}\n{e}");
            }

            // Should never be here!
            return false;
        }
        
        private static void ReadContent_BinaryAsset(string filePath, AddUsageCB callback)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            if (!BINARY_ASSET.Contains(ext)) return;
            
            var allAssets = AssetDatabase.LoadAllAssetsAtPath(filePath);
            foreach (UnityObject assetData in allAssets)
            {
                LogWarning($"Asset: {assetData} : {assetData.GetType()}");
                
                if (assetData is GameObject go)
                {
                    Component[] compList = go.GetComponentsInChildren<Component>(true);
                    for (var i = 0; i < compList.Length; i++)
                    {
                        LoadSerialized(compList[i], callback);
                    }
                }
                else if (assetData is TerrainData terrainData)
                {
                    Read_TerrainData(terrainData, callback);
                }
                else if (assetData is LightingDataAsset lightingDataAsset)
                {
                    Read_LightMap(lightingDataAsset, callback);
                }
                else
                {
                    LoadSerialized(assetData, callback);
                }
            }
        }
    }
}
