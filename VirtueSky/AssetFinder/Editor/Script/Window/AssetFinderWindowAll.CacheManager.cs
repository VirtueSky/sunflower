using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderWindowAll
    {
        protected void DrawScanProject()
        {
            bool writeImportLog = settings.writeImportLog;
            settings.writeImportLog = EditorGUILayout.Toggle("Write Import Log", settings.writeImportLog);
            if (writeImportLog != settings.writeImportLog)
            {
                EditorUtility.SetDirty(this);
            }

            if (GUILayout.Button("Scan project"))
            {
                AssetFinderAsset.shouldWriteImportLog = writeImportLog;
                AssetFinderCache.DeleteCache();
                AssetFinderCache.CreateCache();
            }
        }
        
        protected bool CheckDrawImport()
        {
            AssetFinderUnity.RefreshEditorStatus();
            
            if (AssetFinderUnity.isEditorCompiling)
            {
                EditorGUILayout.HelpBox("Compiling scripts, please wait!", MessageType.Warning);
                Repaint();
                return false;
            }

            if (AssetFinderUnity.isEditorUpdating)
            {
                EditorGUILayout.HelpBox("Importing assets, please wait!", MessageType.Warning);
                Repaint();
                return false;
            }

            InitIfNeeded();

            if (EditorSettings.serializationMode != SerializationMode.ForceText)
            {
                EditorGUILayout.HelpBox("FR2 requires serialization mode set to FORCE TEXT!", MessageType.Warning);
                if (GUILayout.Button("FORCE TEXT")) EditorSettings.serializationMode = SerializationMode.ForceText;

                return false;
            }

            if (AssetFinderCache.hasCache && !AssetFinderCache.CheckSameVersion())
            {
                EditorGUILayout.HelpBox("Incompatible cache version found!!!\nFR2 will need a full refresh and according to your project's size this process may take several minutes to complete finish!",
                    MessageType.Warning);

                DrawScanProject();
                return false;
            }

            if (AssetFinderCache.isReady) return DrawEnable();

            if (!AssetFinderCache.hasCache)
            {
                EditorGUILayout.HelpBox(
                    "FR2 cache not found!\nA first scan is needed to build the cache for all asset references.\nDepending on the size of your project, this process may take a few minutes to complete but once finished, searching for asset references will be incredibly fast!",
                    MessageType.Warning);

                DrawScanProject();
                return false;
            }

            if (!DrawEnable()) return false;

            AssetFinderCache api = AssetFinderCache.Api;
            if (api.workCount > 0)
            {
                string text = "Refreshing ... " + (int)(api.progress * api.workCount) + " / " + api.workCount;

                // Show current asset being processed
                if (!string.IsNullOrEmpty(api.currentAssetName))
                {
                    EditorGUILayout.LabelField(api.currentAssetName, EditorStyles.miniLabel);
                }

                Rect rect = GUILayoutUtility.GetRect(1f, Screen.width, 18f, 18f);
                EditorGUI.ProgressBar(rect, api.progress, text);
                Repaint();
            } 
            else
            {
                api.workCount = 0;
                api.ready = true;
            }

            return false;
        }
    }
} 