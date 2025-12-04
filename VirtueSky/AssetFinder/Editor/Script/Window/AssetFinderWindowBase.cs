using UnityEditor;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{

    public abstract class AssetFinderWindowBase : EditorWindow, IWindow
    {
        public bool WillRepaint { get; set; }
        protected bool showFilter, showIgnore;

        //[NonSerialized] protected bool lockSelection;
        //[NonSerialized] internal List<AssetFinderAsset> Selected;

        public void AddItemsToMenu(GenericMenu menu)
        {
            var api = AssetFinderCache.Api;
            if (api == null) return;

            menu.AddDisabledItem(AssetFinderGUIContent.FromString("AssetFinder - ref2.6.4"));
            menu.AddSeparator(string.Empty);

            menu.AddItem(AssetFinderGUIContent.FromString("Enable"), !AssetFinderSettingExt.disable, () => { AssetFinderSettingExt.disable = !AssetFinderSettingExt.disable; });
            menu.AddItem(AssetFinderGUIContent.FromString($"Auto Refresh: {AssetFinderSettingExt.autoRefreshMode}"), AssetFinderSettingExt.isAutoRefreshEnabled, () =>
            {
                AssetFinderSettingExt.autoRefreshMode = AssetFinderSettingExt.isAutoRefreshEnabled ? AssetFinderAutoRefreshMode.Off : AssetFinderAutoRefreshMode.On;
                if (AssetFinderSettingExt.autoRefreshMode == AssetFinderAutoRefreshMode.On)
                {
                    AssetFinderCache.Api.IncrementalRefresh();
                }
            });
            
            menu.AddItem(AssetFinderGUIContent.FromString($"Refresh"), false, () =>
            {
                AssetFinderCache.Api.ClearCacheCompletely();
                AssetFinderCache.Api.Check4Changes(true);
            });

            menu.AddSeparator(string.Empty);
            
            // Developer mode toggle
            bool isDebugMode = AssetFinderDefine.IsDebugModeEnabled();
            menu.AddItem(AssetFinderGUIContent.FromString($"Developer Mode"), isDebugMode, () =>
            {
                AssetFinderDefine.ToggleDebugMode(!isDebugMode);
            });

            AddToCustomMenu(menu);
        }
        
        public abstract void AddToCustomMenu(GenericMenu menu);

        public abstract void OnSelectionChange();
        protected abstract void OnGUI();

#if UNITY_2018_OR_NEWER
        protected void OnSceneChanged(Scene arg0, Scene arg1)
        {
            if (IsFocusingFindInScene || IsFocusingSceneToAsset || IsFocusingSceneInScene)
            {
                OnSelectionChange();
            }
        }
#endif

        protected bool DrawEnable()
        {
            AssetFinderCache api = AssetFinderCache.Api;
            if (api == null) return false;
            if (!AssetFinderSettingExt.disable) return true;

            bool isPlayMode = EditorApplication.isPlayingOrWillChangePlaymode;
            string message = isPlayMode
                ? "Find References 2 is disabled in play mode!"
                : "Find References 2 is disabled!";

            EditorGUILayout.HelpBox(AssetFinderGUIContent.From(message, AssetFinderIcon.Warning.image));
            if (GUILayout.Button(AssetFinderGUIContent.FromString("Enable")))
            {
                AssetFinderSettingExt.disable = !AssetFinderSettingExt.disable;
                if (!AssetFinderSettingExt.disable && AssetFinderCache.Api != null)
                {
                    AssetFinderCache.Api.IncrementalRefresh();
                }
                Repaint();
            }

            return false;
        }

    }
}
