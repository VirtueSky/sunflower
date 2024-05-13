using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.UtilsEditor
{
    public static class CreateAsset
    {
#if UNITY_EDITOR
        public static T CreateScriptableAssets<T>(string path = "", bool isPingAsset = true)
            where T : ScriptableObject
        {
            var setting = UnityEngine.ScriptableObject.CreateInstance<T>();
            UnityEditor.AssetDatabase.CreateAsset(setting, $"{DefaultResourcesPath(path)}/{typeof(T).Name}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            Selection.activeObject = setting;
            if (isPingAsset)
            {
                EditorGUIUtility.PingObject(setting);
            }

            Debug.Log(
                $"<color=Green>{typeof(T).Name} was created ad {DefaultResourcesPath(path)}/{typeof(T).Name}.asset</color>");
            return setting;
        }

        public static T CreateScriptableAssets<T>(string path = "", string name = "", bool isPingAsset = true)
            where T : ScriptableObject
        {
            string newName = name == "" ? typeof(T).Name : name;
            var setting = UnityEngine.ScriptableObject.CreateInstance<T>();
            UnityEditor.AssetDatabase.CreateAsset(setting, $"{DefaultResourcesPath(path)}/{newName}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            Selection.activeObject = setting;
            if (isPingAsset)
            {
                EditorGUIUtility.PingObject(setting);
            }

            Debug.Log(
                $"<color=Green>{newName} was created ad {DefaultResourcesPath(path)}/{newName}.asset</color>");
            return setting;
        }

        public static T CreateScriptableAssetsOnlyName<T>(string path = "", string name = "",
            bool isPingAsset = true)
            where T : ScriptableObject
        {
            int assetCounter = 0;
            string assetName = name == "" ? $"{typeof(T).Name}" : name;
            string assetPath = $"{DefaultResourcesPath(path)}/{assetName}.asset";

            while (AssetDatabase.LoadAssetAtPath<T>(assetPath) != null)
            {
                assetCounter++;
                assetPath =
                    $"{DefaultResourcesPath(path)}/{CreateNameBasedOnGameObjectNamingScheme(assetName, assetCounter)}.asset";
            }

            var setting = ScriptableObject.CreateInstance<T>();

            UnityEditor.AssetDatabase.CreateAsset(setting, assetPath);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            Selection.activeObject = setting;
            if (isPingAsset)
            {
                EditorGUIUtility.PingObject(setting);
            }

            Debug.Log(
                $"<color=Green>{typeof(T).Name} was created at {assetPath}</color>");
            return setting;
        }


        public static T CreateAndGetScriptableAsset<T>(string path = "", string assetName = "", bool isPingAsset = true)
            where T : ScriptableObject
        {
            var so = AssetUtils.FindAssetAtFolder<T>(new string[] { "Assets" }).FirstOrDefault();
            if (so == null)
            {
                CreateScriptableAssets<T>(path, assetName, isPingAsset);
                so = AssetUtils.FindAssetAtFolder<T>(new string[] { "Assets" }).FirstOrDefault();
            }

            return so;
        }

        public static T GetScriptableAsset<T>() where T : ScriptableObject
        {
            return AssetUtils.FindAssetAtFolder<T>(new string[] { "Assets" }).FirstOrDefault();
        }

        // public enum NamingScheme
        // {
        //     /// <summary>
        //     ///   <para>Adds a space and a number in parenthesis to the name of an instantiated or duplicated GameObject ("Prefab (1)").</para>
        //     /// </summary>
        //     SpaceParenthesis,
        //     /// <summary>
        //     ///   <para>Adds a dot followed by a number to the name of an instantiated or duplicated GameObject ("Prefab.1").</para>
        //     /// </summary>
        //     Dot,
        //     /// <summary>
        //     ///   <para>Adds an underscore and a number to the name of an instantiated or duplicated GameObject ("Prefab_1").</para>
        //     /// </summary>
        //     Underscore,
        // }
        private static string CreateNameBasedOnGameObjectNamingScheme(string baseName, int counter)
        {
            EditorSettings.NamingScheme currentNamingScheme = EditorSettings.gameObjectNamingScheme;
            return currentNamingScheme switch
            {
                EditorSettings.NamingScheme.SpaceParenthesis => $"{baseName} ({counter})",
                EditorSettings.NamingScheme.Dot => $"{baseName}.{counter}",
                EditorSettings.NamingScheme.Underscore => $"{baseName}_{counter}",
                _ => $"{baseName} ({counter})"
            };
        }
#endif


        public static string DefaultResourcesPath(string path = "")
        {
            const string defaultResourcePath = "Assets/_Sunflower/Scriptable";
            if (!Directory.Exists(defaultResourcePath + path))
            {
                Directory.CreateDirectory(defaultResourcePath + path);
            }

            return defaultResourcePath + path;
        }
    }
}