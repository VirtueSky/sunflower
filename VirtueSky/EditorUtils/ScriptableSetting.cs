using System.IO;
using System.Linq;
using UnityEngine;
using VirtueSky.Utils;

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

        public static T CreateAndGetScriptableAsset<T>() where T : ScriptableObject
        {
            var so = AssetUtils.FindAssetAtFolder<T>(new string[] { "Assets" }).FirstOrDefault();
            if (so == null)
            {
                ScriptableSetting.CreateSettingAssets<T>();
                so = AssetUtils.FindAssetAtFolder<T>(new string[] { "Assets" }).FirstOrDefault();
            }

            return so;
        }
    }
}