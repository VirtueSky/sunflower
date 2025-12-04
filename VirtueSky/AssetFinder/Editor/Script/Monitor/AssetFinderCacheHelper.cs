using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    [InitializeOnLoad]
    internal class AssetFinderCacheHelper : AssetPostprocessor
    {
        [NonSerialized] private static HashSet<string> scenes;
        [NonSerialized] private static HashSet<string> guidsIgnore;
        [NonSerialized] internal static bool inited = false;
        
        static AssetFinderCacheHelper()
        {
            try
            {
                EditorApplication.update -= InitHelper;
                EditorApplication.update += InitHelper;
            }
            catch (Exception e)
            {
                AssetFinderLOG.LogWarning(e);
            }
        }

        private static void OnPostprocessAllAssets(
            string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {

            if (AssetFinderSettingExt.disable) return;
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (!AssetFinderSettingExt.isAutoRefreshEnabled) return; // OFF mode: do nothing

            //Debug.Log("OnPostProcessAllAssets : " + ":" + importedAssets.Length + ":" + deletedAssets.Length + ":" + movedAssets.Length + ":" + movedFromAssetPaths.Length);
            if (!AssetFinderCache.isReady)
            {
                AssetFinderLOG.Log($"Not ready, will refresh anyway {importedAssets.Length} !\n{string.Join("\n", importedAssets)}");
                return;
            }

            // FR2 not yet ready
            if (AssetFinderCache.Api.AssetMap == null) return;
            AssetFinderCache.DelayCheck4Changes();

            // Handle new/changed assets based on current processing state
            bool shouldInterruptUsedByBuild = false;
            
            for (var i = 0; i < importedAssets.Length; i++)
            {
                if (importedAssets[i] == AssetFinderCache.CachePath) continue;

                string guid = AssetDatabase.AssetPathToGUID(importedAssets[i]);
                if (!AssetFinderAsset.IsValidGUID(guid)) continue;

                if (AssetFinderCache.Api.AssetMap.ContainsKey(guid))
                {
                    // Get the asset to check if it's critical
                    var asset = AssetFinderCache.Api.Get(guid);
                    if (asset != null && !asset.IsCriticalAsset())
                    {
                        // Skip marking non-critical assets as dirty
                        AssetFinderLOG.Log($"Skipping non-critical asset change: {importedAssets[i]}");
                        continue;
                    }
                    
                    // Asset already exists - mark as dirty for re-processing
                    AssetFinderCache.Api.RefreshAsset(guid, true);
                    
                    AssetFinderLOG.Log("Changed : " + importedAssets[i]);
                    // If we're building usedBy map, we need to interrupt and restart from content reading
                    if (AssetFinderCache.Api.currentState == ProcessingState.BuildingUsedBy)
                    {
                        shouldInterruptUsedByBuild = true;
                    }
                    continue;
                }

                // New asset - add to AssetMap and queue for processing
                AssetFinderCache.Api.AddAsset(guid);
                AssetFinderLOG.Log("New : " + importedAssets[i]);

                // If we're building usedBy map, we need to interrupt and restart from content reading
                if (AssetFinderCache.Api.currentState == ProcessingState.BuildingUsedBy)
                {
                    shouldInterruptUsedByBuild = true;
                }
            }

            for (var i = 0; i < deletedAssets.Length; i++)
            {
                string guid = AssetDatabase.AssetPathToGUID(deletedAssets[i]);
                if (AssetFinderSettingExt.isAutoRefreshEnabled)
                {
                    AssetFinderCache.Api.RemoveAsset(guid);
                }
                AssetFinderLOG.Log("Deleted : " + deletedAssets[i]);
            }

            for (var i = 0; i < movedAssets.Length; i++)
            {
                string guid = AssetDatabase.AssetPathToGUID(movedAssets[i]);
                AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
                if (asset != null && asset.IsCriticalAsset()) 
                {
                    // Only mark critical assets as dirty when moved
                    asset.MarkAsDirty();
                }
            }

            // Handle interruption if we were building usedBy and new assets were added
            if (AssetFinderSettingExt.isAutoRefreshEnabled && shouldInterruptUsedByBuild)
            {
                AssetFinderLOG.Log("FR2: Interrupting usedBy build due to new assets, restarting from content reading");
                // Reset state to allow restart from content reading phase
                AssetFinderCache.Api.currentState = ProcessingState.Idle;
                
                // Force a complete refresh to restart from content reading
                AssetFinderCache.Api.IncrementalRefresh();
                return;
            }

            if (AssetFinderSettingExt.isAutoRefreshEnabled)
            {
                AssetFinderLOG.Log("Changes :: " + importedAssets.Length + "/" + AssetFinderCache.Api.workCount);
                AssetFinderCache.Api.Check4Work();
            }
        }
        
        internal static void InitHelper()
        {
            if (AssetFinderUnity.isEditorCompiling || AssetFinderUnity.isEditorUpdating) return;
            if (!AssetFinderCache.isReady) return;
            EditorApplication.update -= InitHelper;
            
            inited = true;
            InitListScene();
            InitIgnore();
            CheckGitStatus(false);
            
#if UNITY_2018_1_OR_NEWER
            EditorBuildSettings.sceneListChanged -= InitListScene;
            EditorBuildSettings.sceneListChanged += InitListScene;
#endif

            #if UNITY_2022_1_OR_NEWER
            EditorApplication.projectWindowItemInstanceOnGUI -= OnGUIProjectInstance;
            EditorApplication.projectWindowItemInstanceOnGUI += OnGUIProjectInstance;
            #else
            EditorApplication.projectWindowItemOnGUI -= OnGUIProjectItem;
            EditorApplication.projectWindowItemOnGUI += OnGUIProjectItem;
            #endif

            InitIgnore();
            // force repaint all project panels
            EditorApplication.RepaintProjectWindow();
        }
        
        private static void CheckGitStatus(bool force)
        {
            if (AssetFinderSettingExt.gitIgnoreAdded && !force) return;
            AssetFinderSettingExt.isGitProject = AssetFinderGitUtil.IsGitProject();
            if (!AssetFinderSettingExt.isGitProject) return;
            AssetFinderSettingExt.gitIgnoreAdded = AssetFinderGitUtil.CheckGitIgnoreContainsFR2Cache();
        }
        
        public static void InitIgnore()
        {
            guidsIgnore = new HashSet<string>();
            foreach (string item in AssetFinderSetting.IgnoreAsset)
            {
                string guid = AssetDatabase.AssetPathToGUID(item);
                guidsIgnore.Add(guid);
            }
            
            // Debug.Log($"Init Ignore: {guidsIgnore.Count} items");
        }

        private static void InitListScene()
        {
            scenes = new HashSet<string>();

            // string[] scenes = new string[sceneCount];
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                string sce = AssetDatabase.AssetPathToGUID(scene.path);
                scenes.Add(sce);
            }
        }

        private static string lastGUID;
        private static readonly Dictionary<int, GUIContent> _countContentCache = new Dictionary<int, GUIContent>();
        private static GUIContent _plusContent;

        private static void OnGUIProjectInstance(int instanceID, Rect selectionRect)
        {
            if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(instanceID, out string guid, out long localId)) return;

            bool isMainAsset = guid != lastGUID;
            lastGUID = guid;

            if (isMainAsset)
            {
                DrawProjectItem(guid, selectionRect);
                return;
            }
            
            if (!AssetFinderCache.Api.setting.showSubAssetFileId) return;
            var rect2 = selectionRect;
            
            // Cache the localId GUIContent to avoid repeated creation
            if (!_countContentCache.TryGetValue((int)localId, out GUIContent label))
            {
                label = new GUIContent(localId.ToString());
                _countContentCache[(int)localId] = label;
            }
            
            rect2.xMin = rect2.xMax - EditorStyles.miniLabel.CalcSize(label).x;

            var c = GUI.color;
            GUI.color = new Color(.5f, .5f, .5f, 0.5f);
            GUI.Label(rect2, label, EditorStyles.miniLabel);
            GUI.color = c;
        }

        private static void OnGUIProjectItem(string guid, Rect rect)
        {
            bool isMainAsset = guid != lastGUID;
            lastGUID = guid;
            if (isMainAsset) DrawProjectItem(guid, rect);
        }

        private static void DrawProjectItem(string guid, Rect rect)
        {
            var r = new Rect(rect.x, rect.y, 1f, 16f);
            if (scenes.Contains(guid))
                EditorGUI.DrawRect(r, GUI2.Theme(new Color32(72, 150, 191, 255), Color.blue));
            else if (guidsIgnore.Contains(guid))
            {
                var ignoreRect = new Rect(rect.x + 3f, rect.y + 6f, 2f, 2f);
                EditorGUI.DrawRect(ignoreRect, GUI2.darkRed);
            }

            if (!AssetFinderCache.isReady) return; // not ready
            if (!AssetFinderSetting.ShowReferenceCount) return;

            AssetFinderCache api = AssetFinderCache.Api;
            if (AssetFinderCache.Api.AssetMap == null) AssetFinderCache.Api.Check4Changes(false);
            if (!api.AssetMap.TryGetValue(guid, out AssetFinderAsset item)) return;

            if (item == null || item.UsedByMap == null) return;

            if (item.UsedByMap.Count > 0)
            {
                // Cache GUIContent to avoid allocation
                int count = item.UsedByMap.Count;
                if (!_countContentCache.TryGetValue(count, out GUIContent content))
                {
                    content = AssetFinderGUIContent.FromString(count.ToString());
                    _countContentCache[count] = content;
                }
                
                r.width = 0f;
                r.xMin -= 100f;
                GUI.Label(r, content, GUI2.miniLabelAlignRight);
            } else if (item.forcedIncludedInBuild)
            {
                var c = GUI.color;
                GUI.color = c.Alpha(0.2f);
                
                // Cache plus content
                if (_plusContent == null)
                    _plusContent = AssetFinderGUIContent.FromString("+");
                
                r.width = 0f;
                r.xMin -= 100f;
                GUI.Label(r, _plusContent, GUI2.miniLabelAlignRight);
                GUI.color = c;
            }
            
            // CRITICAL FIX: Show warning indicator when auto refresh is off and cache might be stale
            // Only show for assets that might have reference count issues
            if (!AssetFinderSettingExt.isAutoRefreshEnabled && api.workCount > 0)
            {
                var warningRect = new Rect(rect.xMax - 8f, rect.y + 1f, 6f, 6f);
                Color oldColor = GUI.color;
                GUI.color = Color.yellow;
                EditorGUI.DrawRect(warningRect, Color.yellow);
                GUI.color = oldColor;
                
                // Tooltip to explain the warning
                if (warningRect.Contains(Event.current.mousePosition))
                {
                    GUI.Label(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 200f, 40f), 
                             new GUIContent("Auto refresh is disabled. Reference counts may be outdated. Use Window > Find Reference 2 to force refresh."));
                }
            }
        }
    }
}
