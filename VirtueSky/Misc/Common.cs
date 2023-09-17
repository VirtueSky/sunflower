using System.IO;
using UnityEngine;

namespace VirtueSky.Misc
{
    public static class Common
    {
        public static void CreateSettingAssets<T>() where T : ScriptableObject
        {
            var setting = UnityEngine.ScriptableObject.CreateInstance<T>();
            UnityEditor.AssetDatabase.CreateAsset(setting, $"{DefaultResourcesPath()}/{typeof(T).Name}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            Debug.Log($"{typeof(T).Name} was created ad {DefaultResourcesPath()}/{typeof(T).Name}.asset");
        }

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