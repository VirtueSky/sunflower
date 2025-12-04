using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderWindowAll
    {
        private AssetFinderDeleteButton deleteUnused;

        private void DrawTools()
        {
            if (isFocusingDuplicate)
            {
                Duplicated.DrawLayout();
                GUILayout.FlexibleSpace();
                return;
            }

            if (isFocusingUnused)
            {
                DrawUnusedAssetsPanel();
                return;
            }

            if (isFocusingUsedInBuild)
            {
                UsedInBuild.DrawLayout();
                return;
            }

            if (isFocusingOthers)
            {
                DrawOthersPanel();
                return;
            }

            if (isFocusingGUIDs)
            {
                DrawGUIDs();
            }
        }

        private void DrawUnusedAssetsPanel()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Recursive Search", AssetFinderTheme.Current.RecursiveSearchLabelWidth);
            bool oldRecursive = settings.recursiveUnusedScan;
            settings.recursiveUnusedScan = EditorGUILayout.Toggle(settings.recursiveUnusedScan, AssetFinderTheme.Current.ToggleWidth);
            if (oldRecursive != settings.recursiveUnusedScan)
            {
                RefUnUse.ResetUnusedAsset(settings.recursiveUnusedScan);
                EditorUtility.SetDirty(this);
            }
            EditorGUILayout.EndHorizontal();
            
            if ((RefUnUse.refs != null) && (RefUnUse.refs.Count == 0))
            {
                EditorGUILayout.HelpBox("Clean! Your project does not has have any unused assets!", MessageType.Info);
                GUILayout.FlexibleSpace();
                EditorGUILayout.HelpBox("Your deleted assets was backup at Library/FR2/ just in case you want your assets back!", MessageType.Info);
            } 
            else
            {
                RefUnUse.DrawLayout();

                if (deleteUnused == null)
                {
                    deleteUnused = new AssetFinderDeleteButton
                    {
                        warningMessage = "A backup (.unitypackage) will be created so you can reimport the deleted assets later!",
                        deleteLabel = AssetFinderGUIContent.From("DELETE ASSETS", AssetFinderIcon.Delete.image),
                        confirmMessage = "Create backup at Library/FR2/"
                    };
                }

                GUILayout.BeginHorizontal();
                {
                    deleteUnused.Draw(() => { AssetFinderUnity.BackupAndDeleteAssets(RefUnUse.source); });
                }
                GUILayout.EndHorizontal();
            }
        }

        private void DrawOthersPanel()
        {
            GUILayout.Space(4f);
            EditorGUILayout.BeginHorizontal();
            
            // Left: Vertical tab bar
            EditorGUILayout.BeginVertical(AssetFinderTheme.Current.TabPanelWidth);
            var tabStyle = new GUIStyle(EditorStyles.toolbarButton)
            {
                alignment = TextAnchor.MiddleLeft,
                fixedHeight = 32f,
                fontStyle = FontStyle.Normal
            };
            Color origColor = GUI.backgroundColor;
            for (var i = 0; i < 3; i++)
            {
                string label = i == 0 ? "Missing Scripts" : i == 1 ? "Organize Assets" : "Delete Empty Folders";
                GUI.backgroundColor = (settings.othersTabIndex == i) ? new Color(0.7f, 0.9f, 1f, 1f) : origColor;
                if (GUILayout.Toggle(settings.othersTabIndex == i, label, tabStyle))
                {
                    if (settings.othersTabIndex != i) 
                    { 
                        settings.othersTabIndex = i; 
                        WillRepaint = true; 
                    }
                }
            }
            GUILayout.FlexibleSpace();
            GUI.backgroundColor = origColor;
            EditorGUILayout.EndVertical();
            
            // Right: Tool content
            EditorGUILayout.BeginVertical();
            if (settings.othersTabIndex == 0) 
            { 
                MissingReference.DrawLayout(); 
            }
            else if (settings.othersTabIndex == 1) 
            { 
                AssetOrganizer.DrawLayout(); 
            }
            else 
            { 
                DeleteEmptyFolder.DrawLayout(); 
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
} 