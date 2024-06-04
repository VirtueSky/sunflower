using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Newtonsoft.Json.Linq;
using Debug = UnityEngine.Debug;

namespace VirtueSky.UtilsEditor
{
#if UNITY_EDITOR
    public static class FileExtension
    {
        public static List<T> FindAll<T>(string path) where T : Object
        {
            var results = new List<T>();
            var filter = $"t:{typeof(T).Name}";
            var assetNames = AssetDatabase.FindAssets(filter, new[] { path });

            foreach (string assetName in assetNames)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(assetName);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset == null) continue;

                results.Add(asset);
            }

            return results;
        }

        /// <summary>
        /// Find all asset with type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> FindAll<T>() where T : Object
        {
            var results = new List<T>();
            var filter = $"t:{typeof(T).Name}";
            var assetNames = AssetDatabase.FindAssets(filter);

            foreach (string assetName in assetNames)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(assetName);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset == null) continue;

                results.Add(asset);
            }

            return results;
        }

        public static T FindAssetWithPath<T>(string fullPath) where T : Object
        {
            string path = GetPathInCurrentEnvironent(fullPath);
            var t = AssetDatabase.LoadAssetAtPath(path, typeof(T));
            if (t == null) Debug.LogError($"Couldn't load the {nameof(T)} at path :{path}");
            return t as T;
        }

        public static T FindAssetWithPath<T>(string nameAsset, string relativePath) where T : Object
        {
            string path = AssetInPackagePath(relativePath, nameAsset);
            var t = AssetDatabase.LoadAssetAtPath(path, typeof(T));
            if (t == null) Debug.LogError($"Couldn't load the {nameof(T)} at path :{path}");
            return t as T;
        }

        public static T[] FindAssetsWithPath<T>(string nameAsset, string relativePath)
            where T : Object
        {
            string path = AssetInPackagePath(relativePath, nameAsset);
            var t = AssetDatabase.LoadAllAssetsAtPath(path).OfType<T>().ToArray();
            if (t.Length == 0) Debug.LogError($"Couldn't load the {nameof(T)} at path :{path}");
            return t;
        }

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

        public static string AssetInPackagePath(string relativePath, string nameAsset)
        {
            return GetPathInCurrentEnvironent($"{relativePath}/{nameAsset}");
        }

        public static string GetPathInCurrentEnvironent(string fullRelativePath)
        {
            var upmPath = $"Packages/com.virtuesky.sunflower/{fullRelativePath}";
            var normalPath = $"Assets/Sunflower/{fullRelativePath}";
            return !File.Exists(Path.GetFullPath(upmPath)) ? normalPath : upmPath;
        }

        public static string FormatJson(string json)
        {
            try
            {
                JToken parsedJson = JToken.Parse(json);
                return parsedJson.ToString(Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception)
            {
                return json;
            }
        }

        public static string ManifestPath => Application.dataPath + "/../Packages/manifest.json";

        public static void OpenFolderInExplorer(string path)
        {
            if (Directory.Exists(path))
            {
                Process.Start(path);
            }
            else
            {
                Debug.LogError("The directory does not exist: " + path);
            }
        }

        public static void OpenFolderInFinder(string path)
        {
            if (Directory.Exists(path))
            {
                Process.Start("open", path);
            }
            else
            {
                Debug.LogError("The directory does not exist: " + path);
            }
        }
    }
#endif
}