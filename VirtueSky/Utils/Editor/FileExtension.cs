using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using VirtueSky.SimpleJSON;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VirtueSky.UtilsEditor
{
#if UNITY_EDITOR
    public static class FileExtension
    {
        public static T GetConfigFromFolder<T>(string path) where T : ScriptableObject
        {
            var fileEntries = Directory.GetFiles(path, ".", SearchOption.AllDirectories);

            foreach (var fileEntry in fileEntries)
                if (fileEntry.EndsWith(".asset"))
                {
                    var item =
                        AssetDatabase.LoadAssetAtPath<T>(fileEntry.Replace("\\", "/"));
                    if (item)
                        return item;
                }

            return null;
        }

        public static List<T> GetConfigsFromFolder<T>(string path) where T : ScriptableObject
        {
            var fileEntries = Directory.GetFiles(path, ".", SearchOption.AllDirectories);
            var result = new List<T>();
            foreach (var fileEntry in fileEntries)
                if (fileEntry.EndsWith(".asset"))
                {
                    var item = AssetDatabase.LoadAssetAtPath<T>(fileEntry.Replace("\\", "/"));

                    if (item)
                        result.Add(item);
                }

            if (result.Count > 0)
                return result;

            return null;
        }

        public static T GetConfigFromResource<T>(string path) where T : ScriptableObject
        {
            T config =
                Resources.Load<T>(path);
            if (config != null)
            {
                return config;
            }

            return null;
        }

        public static T GetPrefabFromFolder<T>(string path) where T : MonoBehaviour
        {
            var fileEntries = Directory.GetFiles(path, ".", SearchOption.AllDirectories);

            foreach (var fileEntry in fileEntries)
                if (fileEntry.EndsWith(".prefab"))
                {
                    var item =
                        AssetDatabase.LoadAssetAtPath<T>(fileEntry.Replace("\\", "/"));
                    if (item)
                        return item;
                }

            return null;
        }

        public static List<T> GetPrefabsFromFolder<T>(string path) where T : MonoBehaviour
        {
            var fileEntries = Directory.GetFiles(path, ".", SearchOption.AllDirectories);
            var result = new List<T>();
            foreach (var fileEntry in fileEntries)
                if (fileEntry.EndsWith(".prefab"))
                {
                    var item = AssetDatabase.LoadAssetAtPath<T>(fileEntry.Replace("\\", "/"));

                    if (item)
                        result.Add(item);
                }

            if (result.Count > 0)
                return result;

            return null;
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

        public static (string, string) GetPackageInManifestByPackageName(string packageName)
        {
            string manifestPath = Application.dataPath + "/../Packages/manifest.json";
            if (File.Exists(manifestPath))
            {
                string manifestContent = System.IO.File.ReadAllText(manifestPath);
                JSONNode manifestJson = JSON.Parse(manifestContent);
                JSONNode dependencies = manifestJson["dependencies"];
                if (dependencies != null && dependencies.Count > 0)
                {
                    //  List<string> libraries = new List<string>();
                    foreach (KeyValuePair<string, JSONNode> dep in dependencies.AsObject)
                    {
                        if (packageName == $"\"{dep.Key}\"")
                        {
                            // packageName and packageVersion
                            return ($"\"{dep.Key}\"", $": {dep.Value},");
                        }
                        // libraries.Add($"\"{dep.Key}\": {dep.Value}");
                    }
                }
                else
                {
                    Debug.LogError("Could not find dependencies or dependencies null.");
                    return (null, null);
                }
            }
            else
            {
                Debug.LogError("Could not find fileManifest.json");
                return (null, null);
            }

            return (null, null);
        }

        public static void AddPackageInManifest(string packageFullName)
        {
            string manifestPath = Application.dataPath + "/../Packages/manifest.json";
            if (File.Exists(manifestPath))
            {
                string manifestContent = System.IO.File.ReadAllText(manifestPath);
                int dependenciesIndex = manifestContent.IndexOf("\"dependencies\": {") +
                                        "\"dependencies\": {".Length;

                manifestContent = manifestContent.Insert(dependenciesIndex,
                    packageFullName);
                System.IO.File.WriteAllText(manifestPath, manifestContent);
                Debug.Log($"<color=Green>Add {packageFullName} to manifest</color>");
            }
        }

        public static void RemovePackageInManifest(string packageFullName)
        {
            string manifestPath = Application.dataPath + "/../Packages/manifest.json";
            if (File.Exists(manifestPath))
            {
                string manifestContent = System.IO.File.ReadAllText(manifestPath);
                int dependenciesIndex = manifestContent.IndexOf("\"dependencies\": {") +
                                        "\"dependencies\": {".Length;

                manifestContent = manifestContent.Replace(packageFullName,
                    "");
                System.IO.File.WriteAllText(manifestPath, manifestContent);

                Debug.Log($"<color=Green>Remove {packageFullName} to manifest</color>");
            }
        }
    }
#endif
}