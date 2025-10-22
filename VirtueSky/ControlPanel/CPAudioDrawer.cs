using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VirtueSky.Audio;
using VirtueSky.AudioEditor;
using Object = UnityEngine.Object;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPAudioDrawer
    {
        #region Constants
        private static class LayoutConstants
        {
            public const float PANEL_SPACING = 20f;
            public const float LEFT_PANEL_MARGIN = 15f;
            public const float RIGHT_PANEL_MARGIN = 30f;
            public const float BUTTON_HEIGHT = 25f;
            public const float HEADER_SPACING = 10f;
            public const float LINE_THICKNESS = 3f;
            public const float PING_BUTTON_WIDTH = 100f;
            public const float QUICK_ACTION_BUTTON_WIDTH = 80f;
            public const int TEXTURE_SIZE = 2;
        }
        
        private static readonly Color SELECTED_BUTTON_COLOR = new Color(0.3f, 0.5f, 0.85f, 1f);
        #endregion

        #region Fields
        public enum AudioTab { Explore, Settings }
        
        private static AudioTab audioTab = AudioTab.Explore;
        private static SoundData selectedSoundData;
        private static UnityEditor.Editor soundDataEditor;
        private static EditorWindow hostWindow;
        
        private static Vector2 leftPanelScrollPosition = Vector2.zero;
        private static Vector2 rightPanelScrollPosition = Vector2.zero;
        
        private static Texture2D selectedButtonTexture;
        private static Texture2D normalButtonTexture;
        private static GUIStyle normalButtonStyle;
        private static GUIStyle selectedButtonStyle;
        
        private static List<SoundData> cachedSoundDataAssets;
        private static bool needsRefresh = true;
        private static string searchFilter = "";
        
        private static SoundData renamingSoundData;
        private static string renamingText = "";
        private static bool isRenaming = false;
        #endregion

        #region Initialization & Cleanup
        [UnityEditor.InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.quitting += Cleanup;
        }
        
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            needsRefresh = true;
        }

        private static void Cleanup()
        {
            DestroyTexture(ref selectedButtonTexture);
            DestroyTexture(ref normalButtonTexture);
            DestroyEditor(ref soundDataEditor);
            
            cachedSoundDataAssets?.Clear();
            cachedSoundDataAssets = null;
            
            normalButtonStyle = null;
            selectedButtonStyle = null;
        }

        private static void DestroyTexture(ref Texture2D texture)
        {
            if (texture != null)
            {
                Object.DestroyImmediate(texture);
                texture = null;
            }
        }

        private static void DestroyEditor(ref UnityEditor.Editor editor)
        {
            if (editor != null)
            {
                Object.DestroyImmediate(editor);
                editor = null;
            }
        }
        #endregion

        #region Asset Management
        private static List<SoundData> GetSoundDataAssets()
        {
            if (cachedSoundDataAssets == null || needsRefresh)
            {
                RefreshSoundDataAssets();
            }
            return cachedSoundDataAssets;
        }

        private static void RefreshSoundDataAssets()
        {
            var soundDataGuids = AssetDatabase.FindAssets("t:SoundData");
            cachedSoundDataAssets = new List<SoundData>(soundDataGuids.Length);
            
            foreach (var guid in soundDataGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var soundData = AssetDatabase.LoadAssetAtPath<SoundData>(assetPath);
                if (soundData != null)
                {
                    cachedSoundDataAssets.Add(soundData);
                }
            }
            
            cachedSoundDataAssets.Sort((a, b) => 
                string.Compare(a.name, b.name, System.StringComparison.Ordinal));
            
            needsRefresh = false;
        }

        private static List<SoundData> GetFilteredSoundDataAssets()
        {
            var allAssets = GetSoundDataAssets();
            
            if (string.IsNullOrWhiteSpace(searchFilter))
                return allAssets;
            
            return allAssets.FindAll(sd => 
                sd.name.IndexOf(searchFilter, System.StringComparison.OrdinalIgnoreCase) >= 0);
        }
        #endregion

        #region Style Management
        private static void InitializeStyles()
        {
            if (normalButtonStyle == null)
            {
                normalButtonStyle = new GUIStyle(GUI.skin.button);
            }
            
            if (selectedButtonStyle == null)
            {
                selectedButtonStyle = new GUIStyle(GUI.skin.button);
                
                if (selectedButtonTexture == null)
                {
                    selectedButtonTexture = MakeBackgroundTexture(
                        LayoutConstants.TEXTURE_SIZE, 
                        SELECTED_BUTTON_COLOR);
                }
                
                selectedButtonStyle.normal.background = selectedButtonTexture;
                selectedButtonStyle.onNormal.background = selectedButtonTexture;
                selectedButtonStyle.normal.textColor = Color.white;
                selectedButtonStyle.hover.background = selectedButtonTexture;
            }
        }

        private static Texture2D MakeBackgroundTexture(int size, Color color)
        {
            int pixelCount = size * size;
            Color[] pixels = new Color[pixelCount];
            for (int i = 0; i < pixelCount; i++)
            {
                pixels[i] = color;
            }

            Texture2D texture = new Texture2D(size, size);
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
        #endregion

        #region Main Drawing
        public static void OnDrawAudio(Rect position, EditorWindow ownerWindow)
        {
            hostWindow = ownerWindow;
            GUILayout.Space(LayoutConstants.HEADER_SPACING);
            GUILayout.BeginVertical();
            
            CPUtility.DrawHeaderIcon(StatePanelControl.Audio, "Audio");
            GUILayout.Space(LayoutConstants.HEADER_SPACING);
            
            DrawTab();
            
            GUILayout.Space(LayoutConstants.HEADER_SPACING);
            CPUtility.GuiLine(2);
            GUILayout.Space(LayoutConstants.HEADER_SPACING);
            
            switch (audioTab)
            {
                case AudioTab.Explore:
                    DrawExplore(position);
                    break;
                case AudioTab.Settings:
                    DrawSetting(position);
                    break;
            }

            GUILayout.EndVertical();
        }

        private static void DrawTab()
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Toggle(audioTab == AudioTab.Explore, "Explore",
                GUI.skin.button, GUILayout.ExpandWidth(true), 
                GUILayout.Height(LayoutConstants.BUTTON_HEIGHT)))
            {
                audioTab = AudioTab.Explore;
            }

            if (GUILayout.Toggle(audioTab == AudioTab.Settings, "Setting",
                GUI.skin.button, GUILayout.ExpandWidth(true), 
                GUILayout.Height(LayoutConstants.BUTTON_HEIGHT)))
            {
                audioTab = AudioTab.Settings;
            }

            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region Explore Tab
        private static void DrawExplore(Rect position)
        {
            CPUtility.DrawLineLastRectX(
                LayoutConstants.LINE_THICKNESS, 
                GUILayoutUtility.GetLastRect().y, 
                position.width,
                (position.width - ConstantControlPanel.POSITION_X_START_CONTENT) / 2 - 5);
            
            float leftPanelWidth = (position.width - ConstantControlPanel.POSITION_X_START_CONTENT) / 2 
                - LayoutConstants.LEFT_PANEL_MARGIN;
            float rightPanelWidth = position.width - ConstantControlPanel.POSITION_X_START_CONTENT 
                - leftPanelWidth - LayoutConstants.RIGHT_PANEL_MARGIN;
            
            GUILayout.BeginHorizontal();
            
            GUILayout.BeginVertical(GUILayout.Width(leftPanelWidth));
            DrawLeftExplore();
            GUILayout.EndVertical();
            
            GUILayout.Space(LayoutConstants.PANEL_SPACING);
            
            GUILayout.BeginVertical(GUILayout.Width(rightPanelWidth));
            DrawRightExplore();
            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();
        }

        private static void DrawLeftExplore()
        {
            DrawSearchBar();
            GUILayout.Space(5);
            DrawQuickActions();
            GUILayout.Space(5);
            
            // Handle keyboard input before scrollview to capture events
            HandleKeyboardInput();
            
            leftPanelScrollPosition = GUILayout.BeginScrollView(
                leftPanelScrollPosition, 
                GUILayout.ExpandHeight(true));

            InitializeStyles();
            var filteredAssets = GetFilteredSoundDataAssets();
            
            foreach (var soundData in filteredAssets)
            {
                if (soundData != null)
                {
                    DrawSoundDataButton(soundData);
                }
            }

            GUILayout.EndScrollView();
        }

        private static void DrawSearchBar()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Search:", GUILayout.Width(60));
            
            EditorGUI.BeginChangeCheck();
            var searchFieldStyle = GUI.skin.FindStyle("ToolbarSearchTextField") ?? GUI.skin.textField;
            searchFilter = GUILayout.TextField(searchFilter, searchFieldStyle);
            
            if (EditorGUI.EndChangeCheck())
            {
                var filtered = GetFilteredSoundDataAssets();
                if (filtered.Count > 0 && !filtered.Contains(selectedSoundData))
                {
                    SelectSoundData(filtered[0]);
                }
            }
            
            if (GUILayout.Button("×", GUILayout.Width(20)))
            {
                searchFilter = "";
                GUI.FocusControl(null);
            }
            
            GUILayout.EndHorizontal();
        }

        private static void DrawQuickActions()
        {
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Refresh", 
                GUILayout.Width(LayoutConstants.QUICK_ACTION_BUTTON_WIDTH)))
            {
                RefreshSoundDataAssets();
            }
            
            if (GUILayout.Button("Create New", 
                GUILayout.Width(LayoutConstants.QUICK_ACTION_BUTTON_WIDTH)))
            {
                AudioWindowEditor.CreateSoundData();
                needsRefresh = true;
            }
            
            GUILayout.FlexibleSpace();
            
            var filteredCount = GetFilteredSoundDataAssets().Count;
            var totalCount = GetSoundDataAssets().Count;
            string countText = filteredCount == totalCount 
                ? $"Total: {totalCount}" 
                : $"Showing: {filteredCount}/{totalCount}";
            GUILayout.Label(countText, EditorStyles.miniLabel);
            
            GUILayout.EndHorizontal();
        }

        private static void DrawSoundDataButton(SoundData soundData)
        {
            if (isRenaming && renamingSoundData == soundData)
            {
                DrawRenamingField(soundData);
            }
            else
            {
                DrawNormalButton(soundData);
            }
        }

        private static void DrawNormalButton(SoundData soundData)
        {
            var style = (selectedSoundData == soundData) 
                ? selectedButtonStyle 
                : normalButtonStyle;
            
            var rect = GUILayoutUtility.GetRect(new GUIContent(soundData.name), style, GUILayout.ExpandWidth(true));
            
            // Handle events BEFORE button to capture them
            Event evt = Event.current;
            
            // Handle double-click
            if (evt.type == EventType.MouseDown && evt.clickCount == 2 && evt.button == 0 && rect.Contains(evt.mousePosition))
            {
                StartRename(soundData);
                evt.Use();
                return;
            }
            
            // Handle right-click
            if (evt.type == EventType.ContextClick && rect.Contains(evt.mousePosition))
            {
                ShowSoundDataContextMenu(soundData);
                evt.Use();
                return;
            }
            
            // Normal button click
            if (GUI.Button(rect, soundData.name, style))
            {
                SelectSoundData(soundData);
            }
        }

        private static void DrawRenamingField(SoundData soundData)
        {
            GUILayout.BeginHorizontal();
            
            GUI.SetNextControlName("RenameField");
            renamingText = GUILayout.TextField(renamingText, GUILayout.ExpandWidth(true));
            
            if (GUILayout.Button("✓", GUILayout.Width(25)))
            {
                ConfirmRename(soundData);
            }
            
            if (GUILayout.Button("✕", GUILayout.Width(25)))
            {
                CancelRename();
            }
            
            GUILayout.EndHorizontal();
            
            HandleRenameFieldEvents();
        }

        private static void HandleRenameFieldEvents()
        {
            Event evt = Event.current;
            
            if (evt.type == EventType.KeyDown)
            {
                if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
                {
                    ConfirmRename(renamingSoundData);
                    evt.Use();
                }
                else if (evt.keyCode == KeyCode.Escape)
                {
                    CancelRename();
                    evt.Use();
                }
            }
            
            if (isRenaming && GUI.GetNameOfFocusedControl() != "RenameField")
            {
                GUI.FocusControl("RenameField");
            }
        }

        private static void StartRename(SoundData soundData)
        {
            renamingSoundData = soundData;
            renamingText = soundData.name;
            isRenaming = true;
            GUI.FocusControl("RenameField");
        }

        private static void ConfirmRename(SoundData soundData)
        {
            if (string.IsNullOrWhiteSpace(renamingText))
            {
                EditorUtility.DisplayDialog("Invalid Name", "Asset name cannot be empty!", "OK");
                return;
            }
            
            if (renamingText == soundData.name)
            {
                CancelRename();
                return;
            }
            
            string assetPath = AssetDatabase.GetAssetPath(soundData);
            string errorMessage = AssetDatabase.RenameAsset(assetPath, renamingText);
            
            if (!string.IsNullOrEmpty(errorMessage))
            {
                EditorUtility.DisplayDialog("Rename Failed", errorMessage, "OK");
            }
            else
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                needsRefresh = true;
            }
            
            CancelRename();
        }

        private static void CancelRename()
        {
            isRenaming = false;
            renamingSoundData = null;
            renamingText = "";
            GUI.FocusControl(null);
        }

        private static void ShowSoundDataContextMenu(SoundData soundData)
        {
            GenericMenu menu = new GenericMenu();
            
            menu.AddItem(new GUIContent("Rename"), false, () => 
            {
                StartRename(soundData);
            });
            
            menu.AddSeparator("");
            
            menu.AddItem(new GUIContent("Ping in Project"), false, () => 
            {
                Selection.activeObject = soundData;
                EditorGUIUtility.PingObject(soundData);
            });
            
            menu.AddItem(new GUIContent("Duplicate"), false, () => 
            {
                var path = AssetDatabase.GetAssetPath(soundData);
                var newPath = AssetDatabase.GenerateUniqueAssetPath(path);
                AssetDatabase.CopyAsset(path, newPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                needsRefresh = true;
            });
            
            menu.AddSeparator("");
            
            menu.AddItem(new GUIContent("Delete"), false, () => 
            {
                if (EditorUtility.DisplayDialog("Delete SoundData", 
                    $"Are you sure you want to delete '{soundData.name}'?", "Delete", "Cancel"))
                {
                    var path = AssetDatabase.GetAssetPath(soundData);
                    AssetDatabase.DeleteAsset(path);
                    if (selectedSoundData == soundData)
                    {
                        selectedSoundData = null;
                        DestroyEditor(ref soundDataEditor);
                    }
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    needsRefresh = true;
                }
            });
            
            menu.ShowAsContext();
        }

        private static void SelectSoundData(SoundData soundData)
        {
            if (selectedSoundData != soundData)
            {
                selectedSoundData = soundData;
                DestroyEditor(ref soundDataEditor);
            }
        }

        private static void DrawRightExplore()
        {
            rightPanelScrollPosition = GUILayout.BeginScrollView(
                rightPanelScrollPosition, 
                GUILayout.ExpandHeight(true));

            if (selectedSoundData != null)
            {
                DrawPingButton();
                GUILayout.Space(5);
                DrawSoundDataEditor();
            }
            else
            {
                GUILayout.Label("No SoundData selected", EditorStyles.centeredGreyMiniLabel);
            }

            GUILayout.EndScrollView();
        }

        private static void DrawPingButton()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Ping Asset", 
                GUILayout.Width(LayoutConstants.PING_BUTTON_WIDTH)))
            {
                EditorGUIUtility.PingObject(selectedSoundData);
                Selection.activeObject = selectedSoundData;
            }
            GUILayout.EndHorizontal();
        }

        private static void DrawSoundDataEditor()
        {
            if (soundDataEditor == null || 
                (soundDataEditor.target as SoundData) != selectedSoundData)
            {
                DestroyEditor(ref soundDataEditor);
                soundDataEditor = UnityEditor.Editor.CreateEditor(selectedSoundData);
            }

            if (soundDataEditor != null)
            {
                if (soundDataEditor is SoundDataEditor sdEditor)
                {
                    sdEditor.SetExternalRepaintCallback(hostWindow != null
                        ? new Action(hostWindow.Repaint)
                        : null);
                }

                EditorGUI.BeginChangeCheck();
                soundDataEditor.OnInspectorGUI();
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(selectedSoundData);
                }
            }
        }

        private static void HandleKeyboardInput()
        {
            if (isRenaming)
                return;
            
            Event evt = Event.current;
            
            // Accept both KeyDown and KeyUp for better compatibility
            if (evt.type != EventType.KeyDown && evt.type != EventType.KeyUp)
                return;
            
            // Only process on KeyDown to avoid double-processing
            if (evt.type != EventType.KeyDown)
                return;
            
            var assets = GetFilteredSoundDataAssets();
            int currentIndex = assets.IndexOf(selectedSoundData);
            
            switch (evt.keyCode)
            {
                case KeyCode.F2:
                    if (selectedSoundData != null)
                    {
                        StartRename(selectedSoundData);
                        evt.Use();
                        GUI.changed = true;
                    }
                    break;
                
                case KeyCode.UpArrow:
                    if (currentIndex > 0)
                    {
                        SelectSoundData(assets[currentIndex - 1]);
                        evt.Use();
                    }
                    break;
                    
                case KeyCode.DownArrow:
                    if (currentIndex >= 0 && currentIndex < assets.Count - 1)
                    {
                        SelectSoundData(assets[currentIndex + 1]);
                        evt.Use();
                    }
                    break;
                    
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    if (selectedSoundData != null)
                    {
                        EditorGUIUtility.PingObject(selectedSoundData);
                        Selection.activeObject = selectedSoundData;
                        evt.Use();
                    }
                    break;
                    
                case KeyCode.Delete:
                case KeyCode.Backspace:
                    if (selectedSoundData != null && evt.command == false && evt.control == false)
                    {
                        if (EditorUtility.DisplayDialog("Delete SoundData", 
                            $"Are you sure you want to delete '{selectedSoundData.name}'?", "Delete", "Cancel"))
                        {
                            var path = AssetDatabase.GetAssetPath(selectedSoundData);
                            AssetDatabase.DeleteAsset(path);
                            selectedSoundData = null;
                            DestroyEditor(ref soundDataEditor);
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                            needsRefresh = true;
                        }
                        evt.Use();
                    }
                    break;
            }
        }
        #endregion
        #region Settings Tab
        private static void DrawSetting(Rect position)
        {
            DrawCreateSoundDataSection();
            DrawSectionSeparator(position);
            DrawMusicEventSection();
            DrawSectionSeparator(position);
            DrawSfxEventSection();
            DrawSectionSeparator(position);
            DrawVolumeVariableSection();
        }

        private static void DrawSectionSeparator(Rect position)
        {
            GUILayout.Space(LayoutConstants.HEADER_SPACING);
            CPUtility.DrawLineLastRectY(
                LayoutConstants.LINE_THICKNESS, 
                ConstantControlPanel.POSITION_X_START_CONTENT, 
                position.width);
            GUILayout.Space(LayoutConstants.HEADER_SPACING);
        }

        private static void DrawCreateSoundDataSection()
        {
            if (GUILayout.Button("Create Sound Data"))
            {
                AudioWindowEditor.CreateSoundData();
                needsRefresh = true;
            }
        }

        private static void DrawMusicEventSection()
        {
            CPUtility.DrawHeader("Music Event");
            GUILayout.Space(LayoutConstants.HEADER_SPACING);
            
            DrawButton("Create Play Music Event", AudioWindowEditor.CreatePlayMusicEvent);
            DrawButton("Create Pause Music Event", AudioWindowEditor.CreatePauseMusicEvent);
            DrawButton("Create Resume Music Event", AudioWindowEditor.CreateResumeMusicEvent);
            DrawButton("Create Stop Music Event", AudioWindowEditor.CreateStopMusicEvent);
        }

        private static void DrawSfxEventSection()
        {
            CPUtility.DrawHeader("Sfx Event");
            GUILayout.Space(LayoutConstants.HEADER_SPACING);
            
            DrawButton("Create Play Sfx Event", AudioWindowEditor.CreatePlaySfxEvent);
            DrawButton("Create Pause Sfx Event", AudioWindowEditor.CreatePauseSfxEvent);
            DrawButton("Create Resume Sfx Event", AudioWindowEditor.CreateResumeSfxEvent);
            DrawButton("Create Finish Sfx Event", AudioWindowEditor.CreateFinishSfxEvent);
            DrawButton("Create Stop Sfx Event", AudioWindowEditor.CreateStopSfxEvent);
            DrawButton("Create Stop All Sfx Event", AudioWindowEditor.CreateStopAllSfxEvent);
        }

        private static void DrawVolumeVariableSection()
        {
            CPUtility.DrawHeader("Volume Changed Variable");
            GUILayout.Space(LayoutConstants.HEADER_SPACING);
            
            DrawButton("Create Music Volume Variable", AudioWindowEditor.CreateMusicVolume);
            DrawButton("Create Sfx Volume Variable", AudioWindowEditor.CreateSfxVolume);
        }

        private static void DrawButton(string label, System.Action onClick)
        {
            if (GUILayout.Button(label))
            {
                onClick?.Invoke();
            }
        }
        #endregion
    }
}





