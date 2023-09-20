using System.IO;
using UnityEngine;

namespace VirtueSky.EditorUtils
{
    public static class ScriptableSetting
    {
#if UNITY_EDITOR
        public static void CreateSettingAssets<T>() where T : ScriptableObject
        {
            var setting = UnityEngine.ScriptableObject.CreateInstance<T>();
            UnityEditor.AssetDatabase.CreateAsset(setting, $"{DefaultResourcesPath()}/{typeof(T).Name}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            Debug.Log($"{typeof(T).Name} was created ad {DefaultResourcesPath()}/{typeof(T).Name}.asset");
        }
#endif


        public static string DefaultResourcesPath()
        {
            const string defaultResourcePath = "Assets/_Root/Resources";
            if (!Directory.Exists(defaultResourcePath))
            {
                Directory.CreateDirectory(defaultResourcePath);
            }

            return defaultResourcePath;
        }
    }
}