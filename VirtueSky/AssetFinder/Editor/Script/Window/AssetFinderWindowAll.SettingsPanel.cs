using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderWindowAll
    {
        private void DrawSettings()
        {
            if (bottomTabs == null || bottomTabs.current == -1) return;

            GUILayout.BeginVertical(AssetFinderTheme.Current.SettingsPanelHeight);
            {
                GUILayout.Space(2f);
                switch (bottomTabs.current)
                {
                case 0:
                    {
                        DrawMainSettings();
                        break;
                    }

                case 1:
                    {
                        DrawIgnoreSettings();
                        break;
                    }

                case 2:
                    {
                        DrawFilterSettings();
                        break;
                    }
                }
            }
            GUILayout.EndVertical();

            Rect rect = GUILayoutUtility.GetLastRect();
            rect.height = 1f;
            GUI2.Rect(rect, Color.black, 0.4f);
        }

        private void DrawMainSettings()
        {
            AssetFinderSetting.s.DrawSettings();

            // Add the Write Import Log toggle in the settings
            GUILayout.Space(5f);
            EditorGUILayout.LabelField("Advanced Settings", EditorStyles.boldLabel);

            bool writeLog = settings.writeImportLog;
            settings.writeImportLog = EditorGUILayout.Toggle("Write Import Log", settings.writeImportLog);
            if (writeLog != settings.writeImportLog)
            {
                EditorUtility.SetDirty(this);
            }
            
            // Add Git settings if applicable
            if (AssetFinderSettingExt.isGitProject)
            {
                DrawGitSettings();
            }
        }

        private void DrawGitSettings()
        {
            GUILayout.Space(5f);
            EditorGUILayout.LabelField("Git Settings", EditorStyles.boldLabel);
            
            if (AssetFinderSettingExt.gitIgnoreAdded)
            {
                EditorGUILayout.HelpBox("AssetFinderCache.asset* is already in your .gitignore file.", MessageType.Info);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Add AssetFinderCache.asset* to .gitignore");
                if (GUILayout.Button("Apply", AssetFinderTheme.Current.ApplyButtonWidth))
                {
                    AssetFinderGitUtil.AddFR2CacheToGitIgnore();
                    AssetFinderSettingExt.gitIgnoreAdded = true;
                    AssetFinderSettingExt.hideGitIgnoreWarning = true;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawIgnoreSettings()
        {
            if (AssetFinderAssetGroupDrawer.DrawIgnoreFolder()) 
            {
                MarkDirty();
            }
        }

        private void DrawFilterSettings()
        {
            if (AssetFinderAssetGroupDrawer.DrawSearchFilter()) 
            {
                MarkDirty();
            }
        }
    }
} 