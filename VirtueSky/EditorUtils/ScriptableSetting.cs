using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VirtueSky.Utils;

namespace VirtueSky.EditorUtils
{
    public static class ScriptableSetting
    {
#if UNITY_EDITOR
        public static void CreateScriptableAssets<T>(string path = "")
            where T : ScriptableObject
        {
            var setting = UnityEngine.ScriptableObject.CreateInstance<T>();
            UnityEditor.AssetDatabase.CreateAsset(setting, $"{DefaultResourcesPath(path)}/{typeof(T).Name}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            Debug.Log(
                $"<color=Green>{typeof(T).Name} was created ad {DefaultResourcesPath(path)}/{typeof(T).Name}.asset</color>");
        }

        public static void CreateScriptableAssets<T>(string path = "", string name = "")
            where T : ScriptableObject
        {
            string newName = name == "" ? typeof(T).Name : name;
            var setting = UnityEngine.ScriptableObject.CreateInstance<T>();
            UnityEditor.AssetDatabase.CreateAsset(setting, $"{DefaultResourcesPath(path)}/{newName}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            Debug.Log(
                $"<color=Green>{newName} was created ad {DefaultResourcesPath(path)}/{newName}.asset</color>");
        }

        public static void CreateScriptableAssetsOnlyName<T>(string path = "") where T : ScriptableObject
        {
            int assetCounter = 0;
            string assetName = $"{typeof(T).Name}";
            string assetPath = $"{DefaultResourcesPath(path)}/{assetName}.asset";

            while (AssetDatabase.LoadAssetAtPath<T>(assetPath) != null)
            {
                assetCounter++;
                assetPath = $"{DefaultResourcesPath(path)}/{assetName} {assetCounter}.asset";
            }

            var setting = ScriptableObject.CreateInstance<T>();

            UnityEditor.AssetDatabase.CreateAsset(setting, assetPath);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            Debug.Log(
                $"<color=Green>{typeof(T).Name} was created at {assetPath}</color>");
        }


        public static T CreateAndGetScriptableAsset<T>(string path = "") where T : ScriptableObject
        {
            var so = AssetUtils.FindAssetAtFolder<T>(new string[] { "Assets" }).FirstOrDefault();
            if (so == null)
            {
                ScriptableSetting.CreateScriptableAssets<T>(path);
                so = AssetUtils.FindAssetAtFolder<T>(new string[] { "Assets" }).FirstOrDefault();
            }

            return so;
        }
#endif


        public static string DefaultResourcesPath(string path = "")
        {
            const string defaultResourcePath = "Assets/_Sunflower/Resources";
            if (!Directory.Exists(defaultResourcePath + path))
            {
                Directory.CreateDirectory(defaultResourcePath + path);
            }

            return defaultResourcePath + path;
        }
    }
}