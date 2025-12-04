using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderWindowAll
    {
        private void DrawScenePanel(Rect rect)
        {
            AssetFinderRefDrawer drawer = isFocusingUses
                ? IsSelectingAssets ? null : SceneUsesDrawer
                : IsSelectingAssets ? RefInScene : RefSceneInScene;
            
            if (drawer == null) return;

            if (!AssetFinderSceneCache.isReady && AssetFinderSceneCache.Api.Status == SceneCacheStatus.Scanning)
            {
                DrawSceneCacheProgress(rect);
                rect.yMin += 18f;
            }
            
            if (AssetFinderSceneCache.hasCache) drawer.Draw(rect);
        }

        private void DrawSceneCacheProgress(Rect rect)
        {
            Rect rr = rect;
            rr.height = 16f;

            if (AssetFinderSceneCache.Api.Status == SceneCacheStatus.Scanning)
            {
                int cur = AssetFinderSceneCache.Api.current, total = AssetFinderSceneCache.Api.total;
                var progress = Mathf.Clamp01(cur * 1f / total);
                var progressText = AssetFinderSceneCache.Api.Status == SceneCacheStatus.Scanning
                    ? $"Scanning objects: {cur} / {total}"
                    : $"{cur} / {total}";
                EditorGUI.ProgressBar(rr, progress, progressText);

                if (cur >= total)
                {
                    AssetFinderLOG.LogWarning($"Stuck at scanning? {cur}/{total}");
                }
                WillRepaint = true;
                return;
            }
            
            string statusText;
            switch (AssetFinderSceneCache.Api.Status)
            {
                case SceneCacheStatus.None:
                    statusText = "Scene cache is not ready!";
                    break;
                case SceneCacheStatus.Changed:
                    statusText = "Scene changed - results might be incompleted";
                    break;
                case SceneCacheStatus.Scanning:
                    statusText = "Preparing to scan scene objects...";
                    break;
                case SceneCacheStatus.Ready:
                    statusText = "Scene cache ready";
                    break;
                default:
                    statusText = "Unknown status";
                    break;
            }
            
            EditorGUI.ProgressBar(rr, 0f, statusText);
        }



        private void DrawAssetPanel(Rect rect)
        {
            AssetFinderRefDrawer drawer = GetAssetDrawer();
            if (drawer == null) return;
            drawer.Draw(rect);

            if (!drawer.showDetail) return;

            settings.details = true;
            drawer.showDetail = false;
            sp1.splits[2].visible = settings.details;
            sp1.CalculateWeight();
            Repaint();
        }

        private void DrawGitWarningPanel()
        {
            if (!AssetFinderSettingExt.isGitProject || AssetFinderSettingExt.gitIgnoreAdded || AssetFinderSettingExt.hideGitIgnoreWarning) return;
            
            EditorGUILayout.BeginHorizontal();
            
            // Left side: Warning message
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            EditorGUILayout.HelpBox("You should add **/AssetFinderCache.asset* to your .gitignore file to avoid committing cache files.", MessageType.Warning);
            EditorGUILayout.EndVertical();
            
            // Right side: Buttons stacked vertically
            EditorGUILayout.BeginVertical(AssetFinderTheme.Current.ApplyButtonWidth);

            if (GUILayout.Button("Apply", AssetFinderTheme.Current.CompactButtonHeight))
            {
                AssetFinderGitUtil.AddFR2CacheToGitIgnore();
                AssetFinderSettingExt.gitIgnoreAdded = true;
            }

            if (GUILayout.Button("Ignore", AssetFinderTheme.Current.CompactButtonHeight))
            {
                AssetFinderSettingExt.hideGitIgnoreWarning = true;
            }
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndHorizontal();
        }

        private void DrawToolsWarningPanel()
        {
            if (AssetFinderSettingExt.hideToolsWarning) return;
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox(AssetFinderGUIContent.From("Tools are POWERFUL & DANGEROUS! Only use if you know what you are doing!!!", AssetFinderIcon.Warning.image));
            if (GUILayout.Button("  x", EditorStyles.label, AssetFinderTheme.Current.CloseButtonWidth, AssetFinderTheme.Current.WarningCloseButtonHeight))
            {
                AssetFinderSettingExt.hideToolsWarning = true;
            }
            EditorGUILayout.EndHorizontal();
        }


        internal bool DrawButton(Rect rect, ref bool show, GUIContent icon)
        {
            var changed = false;
            Color oColor = GUI.color;
            Color originalContentColor = GUI.contentColor;
            
            // For light theme, make icons more visible by adjusting content color
            if (!EditorGUIUtility.isProSkin)
            {
                GUI.contentColor = new Color(0.3f, 0.3f, 0.3f, 1f); // Darker color for better visibility in light theme
            }
            
            if (show) GUI.color = new Color(0.7f, 1f, 0.7f, 1f);
            {
                if (GUI.Button(rect, icon, EditorStyles.toolbarButton))
                {
                    show = !show;
                    EditorUtility.SetDirty(this);
                    WillRepaint = true;
                    changed = true;
                }
            }
            GUI.color = oColor;
            GUI.contentColor = originalContentColor;
            return changed;
        }

        internal void DrawAssetViewSettings()
        {
            bool isDisable = !sp2.splits[1].visible;
            EditorGUI.BeginDisabledGroup(isDisable);
            {
                GUI2.ToolbarToggle(ref AssetFinderSetting.s.displayAssetBundleName, AssetFinderIcon.AssetBundle.image, Vector2.zero, "Show / Hide Assetbundle Names");
#if UNITY_2017_1_OR_NEWER
                GUI2.ToolbarToggle(ref AssetFinderSetting.s.displayAtlasName, AssetFinderIcon.Atlas.image, Vector2.zero, "Show / Hide Atlas packing tags");
#endif
                GUI2.ToolbarToggle(ref AssetFinderSetting.s.showUsedByClassed, AssetFinderIcon.Material.image, Vector2.zero, "Show / Hide usage icons");

                if (GUILayout.Button("CSV", EditorStyles.toolbarButton)) OnCSVClickExtension();
            }
            EditorGUI.EndDisabledGroup();
        }

        private AssetFinderEnumDrawer groupModeED;
        private AssetFinderEnumDrawer toolModeED;
        private AssetFinderEnumDrawer sortModeED;

        internal void DrawViewModes(Rect rect)
        {
            var (rect1, rect2) = rect.HzSplit(0f, 0.5f);

            if (toolModeED == null)
            {
                toolModeED = new AssetFinderEnumDrawer
                {
                    AssetFinderenum = new AssetFinderEnumDrawer.EnumInfo(
                        AssetFinderRefDrawer.Mode.Type,
                        AssetFinderRefDrawer.Mode.Folder,
                        AssetFinderRefDrawer.Mode.Extension
                    )
                };
            }
            if (groupModeED == null) groupModeED = new AssetFinderEnumDrawer { tooltip = "Group By" };
            if (sortModeED == null) sortModeED = new AssetFinderEnumDrawer { tooltip = "Sort By" };

            if (settings.toolMode)
            {
                AssetFinderRefDrawer.Mode tMode = settings.toolGroupMode;
                if (toolModeED.Draw(rect1, ref tMode))
                {
                    settings.toolGroupMode = tMode;
                    MarkDirty();
                    RefreshSort();
                }
            } else
            {
                AssetFinderRefDrawer.Mode gMode = settings.groupMode;
                if (groupModeED.Draw(rect1, ref gMode))
                {
                    settings.groupMode = gMode;
                    MarkDirty();
                    RefreshSort();
                }
            }

            AssetFinderRefDrawer.Sort sMode = settings.sortMode;
            if (sortModeED.Draw(rect2, ref sMode))
            {
                settings.sortMode = sMode;
                RefreshSort();
            }
        }
    }
} 