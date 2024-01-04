using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;
using VirtueSky.Utils;

namespace VirtueSky.LevelEditor
{
    [Serializable]
    public class LevelSystemEditorSetting : ScriptableSettings<LevelSystemEditorSetting>
    {
        public List<string> whitelistPaths = new List<string>();
        public List<string> blacklistPaths = new List<string>();
    }

    public static class UtilitiesLevelSystemDrawer
    {
        [MenuItem("Sunflower/LevelEditor &3")]
        public static void OpenLevelEditor()
        {
            OnInspectorGUI();
        }

        public static void OnInspectorGUI()
        {
            var scriptableSetting = Resources.Load<LevelSystemEditorSetting>(nameof(LevelSystemEditorSetting));
            if (scriptableSetting == null)
            {
                GUI.enabled = !EditorApplication.isCompiling;
                GUI.backgroundColor = Uniform.Pink;
                var setting = ScriptableObject.CreateInstance<LevelSystemEditorSetting>();
                const string path = "Assets/_Sunflower/Editor/Resources";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                AssetDatabase.CreateAsset(setting, $"{path}/{nameof(LevelSystemEditorSetting)}.asset");
                RegistryManager.Add("com.unity.addressables", "1.21.19");
                RegistryManager.Resolve();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log($"{nameof(LevelSystemEditorSetting)} was created ad {path}/{nameof(LevelSystemEditorSetting)}.asset");
                GUI.backgroundColor = Color.white;
                GUI.enabled = true;

                OpenWindowLevelEditor();
            }
            else
            {
                OpenWindowLevelEditor();
            }
        }

        static void OpenWindowLevelEditor()
        {
            var window = EditorWindow.GetWindow<LevelEditor>("Level Editor", true);
            if (window)
            {
                window.minSize = new Vector2(275, 0);
                window.Show(true);
            }
        }
    }
}