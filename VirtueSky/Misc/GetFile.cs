using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VirtueSky.Misc
{
#if UNITY_EDITOR
    public static class GetFile
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
    }
#endif
}