using System;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    [Serializable] internal class AssetFinderSettingExt
    {
        
        public static AssetFinderAutoRefreshMode autoRefreshMode
        {
            get => inst._autoRefresh;
            set
            {
                if (inst._autoRefresh == value) return;
                inst._autoRefresh = value;
                
                EditorUtility.SetDirty(AssetFinderCache.Api);
                EditorApplication.update -= DelaySave;
                EditorApplication.update += DelaySave;
            }
        }

        public static bool isAutoRefreshEnabled
        {
            get
            {
                if (EditorApplication.isPlayingOrWillChangePlaymode) return false;
                return autoRefreshMode == AssetFinderAutoRefreshMode.On;
            }
        }
        
        public static bool disable
        {
            get => inst.internalDisabled;
            set => inst.internalDisabled = value;
        }
        
        public static bool hideToolsWarning
        {
            get => inst._hideToolsWarning;
            set
            {
                if (inst._hideToolsWarning == value) return;
                inst._hideToolsWarning = value;
                
                EditorUtility.SetDirty(AssetFinderCache.Api);
                EditorApplication.update -= DelaySave;
                EditorApplication.update += DelaySave;
            }
        }

        public static bool isGitProject;
        public static bool gitIgnoreAdded
        {
            get => inst._gitIgnoreAdded;
            set
            {
                if (inst._gitIgnoreAdded == value) return;
                inst._gitIgnoreAdded = value;
                
                EditorUtility.SetDirty(AssetFinderCache.Api);
                EditorApplication.update -= DelaySave;
                EditorApplication.update += DelaySave;
            }
        }
        
        public static bool hideGitIgnoreWarning
        {
            get => inst._hideGitIgnoreWarning;
            set
            {
                if (inst._hideGitIgnoreWarning == value) return;
                inst._hideGitIgnoreWarning = value;
                
                EditorUtility.SetDirty(AssetFinderCache.Api);
                EditorApplication.update -= DelaySave;
                EditorApplication.update += DelaySave;
            }
        }
        
        private const string path = "Library/FR2/fr2.cfg";
        private static AssetFinderSettingExt inst;
        
        static AssetFinderSettingExt()
        {
            
            inst = new AssetFinderSettingExt();
            if (!File.Exists(path)) return;

            try
            {
                string content = File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(content, inst);
            }
            catch (Exception e)
            {
                AssetFinderLOG.LogWarning(e);
            }
        }

        static void DelaySave()
        {
            EditorApplication.update -= DelaySave;
            
            try
            {
                Directory.CreateDirectory("Library/FR2/");
                File.WriteAllText(path, JsonUtility.ToJson(inst));
            }
            catch (Exception e)
            {
                AssetFinderLOG.LogWarning(e);
            }
        }
        
        [SerializeField] private bool _disableInPlayMode = true;
        [SerializeField] private bool _disabled;
        [SerializeField] private AssetFinderAutoRefreshMode _autoRefresh;
        [SerializeField] private bool _hideToolsWarning;
        [SerializeField] private bool _isGitProject;
        [SerializeField] private bool _gitIgnoreAdded;
        [SerializeField] private bool _hideGitIgnoreWarning;
        
        private bool internalDisabled
        {
            get => _disabled || (_disableInPlayMode && EditorApplication.isPlayingOrWillChangePlaymode);
            set
            {
                ref bool disableRef = ref _disabled;
                if (EditorApplication.isPlayingOrWillChangePlaymode) disableRef = ref _disableInPlayMode;
                
                if (disableRef == value) return;
                disableRef = value;
                
                // disable at runtime: only disable `disableInPlayMode`
                // enable at runtime: enable all
                if (!value) _disabled = false;
                EditorUtility.SetDirty(AssetFinderCache.Api);
                EditorApplication.update -= DelaySave;
                EditorApplication.update += DelaySave;
            }
        }
    }
}
