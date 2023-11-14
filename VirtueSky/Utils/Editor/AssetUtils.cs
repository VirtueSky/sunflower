using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace VirtueSky.UtilsEditor
{
    public static class AssetUtils
    {
#if UNITY_EDITOR
        public static void ChangeAssetName(Object asset, string name)
        {
            var assetPath = AssetDatabase.GetAssetPath(asset.GetInstanceID());
            AssetDatabase.RenameAsset(assetPath, name);
            AssetDatabase.SaveAssets();
        }

        public static T[] FindAssetAtFolder<T>(string[] paths) where T : Object
        {
            var list = new List<T>();
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", paths);
            foreach (var guid in guids)
            {
                var asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
                if (asset)
                {
                    list.Add(asset);
                }
            }

            return list.ToArray();
        }

        public static T FindAssetAtResource<T>(string path) where T : ScriptableObject
        {
            T config =
                Resources.Load<T>(path);
            if (config != null)
            {
                return config;
            }

            return null;
        }
#endif
    }
}