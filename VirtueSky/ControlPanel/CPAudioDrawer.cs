// Updated CPAudioDrawer to use a vertical scrollview for SoundData assets in the left panel
// and Unity's built-in SoundDataEditor for the right panel inspector view.
// This provides a seamless integration with Unity's native inspector experience.
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VirtueSky.Audio;
using VirtueSky.AudioEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPAudioDrawer
    {
        public enum AudioTab
        {
            Explore,
            Settings
        }

        // Helper function to create a colored texture for button backgrounds
        private static Texture2D MakeBackgroundTexture(int width, Color color)
        {
            Color[] pixels = new Color[width * width];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Texture2D texture = new Texture2D(width, width);
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        private static AudioTab audioTab = AudioTab.Explore;
        // Cache textures to avoid creating them every frame
        private static Texture2D selectedButtonTexture;
        private static Texture2D normalButtonTexture;

        // Clean up textures when the editor window is closed or recompiled
        [UnityEditor.InitializeOnLoadMethod]
        private static void Cleanup()
        {
            if (selectedButtonTexture != null)
            {
                Object.DestroyImmediate(selectedButtonTexture);
                selectedButtonTexture = null;
            }
            
            if (normalButtonTexture != null)
            {
                Object.DestroyImmediate(normalButtonTexture);
                normalButtonTexture = null;
            }
        }

        public static void OnDrawAudio(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.Audio, "Audio");
            GUILayout.Space(10);
            DrawTab();
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
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

        private static void DrawExplore(Rect position)
        {
            CPUtility.DrawLineLastRectX(3, GUILayoutUtility.GetLastRect().y, position.width,
                (position.width - ConstantControlPanel.POSITION_X_START_CONTENT) / 2 - 5);
            
            // Calculate dimensions for the layout
            float leftPanelWidth = (position.width - ConstantControlPanel.POSITION_X_START_CONTENT) / 2 - 15;
            float rightPanelWidth = position.width - ConstantControlPanel.POSITION_X_START_CONTENT - leftPanelWidth - 30;
            
            GUILayout.BeginHorizontal();
            
            // Left panel - SoundData list
            GUILayout.BeginVertical(GUILayout.Width(leftPanelWidth));
            DrawLeftExplore(position);
            GUILayout.EndVertical();
            
            GUILayout.Space(20);
            
            // Right panel - Selected SoundData inspector
            GUILayout.BeginVertical(GUILayout.Width(rightPanelWidth));
            DrawRightExplore(position);
            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();
        }

        // Reference to the currently selected SoundData asset for inspection
        private static SoundData selectedSoundData;
        // Editor instance that renders the inspector for the selected SoundData
        private static UnityEditor.Editor soundDataEditor = null;
        // Scroll positions for maintaining scroll state in both panels
        private static Vector2 leftPanelScrollPosition = Vector2.zero;
        private static Vector2 rightPanelScrollPosition = Vector2.zero;
        
        // Draws the left panel with a vertical scrollview of all SoundData assets in the project
        private static void DrawLeftExplore(Rect position)
        {
            // Find all SoundData assets in the project using AssetDatabase
            var soundDataAssets = AssetDatabase.FindAssets("t:SoundData");
            var soundDataObjects = new SoundData[soundDataAssets.Length];

            for (int i = 0; i < soundDataAssets.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(soundDataAssets[i]);
                soundDataObjects[i] = AssetDatabase.LoadAssetAtPath<SoundData>(assetPath);
            }

            // Create a scrollable view for the SoundData assets using the left panel scroll position
            // This allows users to scroll through long lists of SoundData assets
            leftPanelScrollPosition = GUILayout.BeginScrollView(leftPanelScrollPosition, GUILayout.ExpandHeight(true));

            for (int i = 0; i < soundDataObjects.Length; i++)
            {
                if (soundDataObjects[i] != null)
                {
                    // Tạo style mặc định cho nút
                    GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
                    
                    // Nếu nút này là nút được chọn, áp dụng màu nền đặc biệt
                    if (selectedSoundData == soundDataObjects[i])
                    {
                        // Create selected button texture if it doesn't exist
                        // Using a color that matches Unity's default selected state (Unity blue)
                        if (selectedButtonTexture == null)
                        {
                            // Using Unity's default selected button color (a blueish tone)
                            selectedButtonTexture = MakeBackgroundTexture(2, new Color(0.3f, 0.5f, 0.85f, 1f)); // Unity selected blue color
                        }
                        
                        // Apply the selected background color
                        buttonStyle.normal.background = selectedButtonTexture;
                        buttonStyle.onNormal.background = selectedButtonTexture;
                        buttonStyle.normal.textColor = Color.white; // Ensure text is visible on colored background
                        buttonStyle.hover.background = selectedButtonTexture;
                    }

                    if (GUILayout.Button(soundDataObjects[i].name, buttonStyle, GUILayout.ExpandWidth(true)))
                    {
                        selectedSoundData = soundDataObjects[i];
                    }
                }
            }

            GUILayout.EndScrollView();
        }

        // Draws the right panel using the existing SoundDataEditor to display the selected SoundData
        // This provides the same inspector experience as viewing the asset in Unity's native inspector
        private static void DrawRightExplore(Rect position)
        {
            // Create a scrollable view for the right panel to handle tall inspector content
            rightPanelScrollPosition = GUILayout.BeginScrollView(rightPanelScrollPosition, GUILayout.ExpandHeight(true));

            if (selectedSoundData != null)
            {
                // Add Ping button at the top
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Ping Asset", GUILayout.Width(100)))
                {
                    EditorGUIUtility.PingObject(selectedSoundData);
                    Selection.activeObject = selectedSoundData;
                }
                GUILayout.EndHorizontal();
                
                GUILayout.Space(5);

                // Create or update the editor for the selected SoundData
                if (soundDataEditor == null || (soundDataEditor.target as SoundData) != selectedSoundData)
                {
                    if (soundDataEditor != null)
                    {
                        Object.DestroyImmediate(soundDataEditor);
                    }

                    soundDataEditor = UnityEditor.Editor.CreateEditor(selectedSoundData);
                }

                // Draw the editor GUI
                if (soundDataEditor != null)
                {
                    // Use BeginChangeCheck/EndChangeCheck to detect changes and repaint if needed
                    EditorGUI.BeginChangeCheck();
                    soundDataEditor.OnInspectorGUI();
                    if (EditorGUI.EndChangeCheck())
                    {
                        // Force repaint if changes were detected
                        UnityEditor.EditorUtility.SetDirty(selectedSoundData);
                    }
                }
            }
            else
            {
                GUILayout.Label("No SoundData selected", EditorStyles.centeredGreyMiniLabel);
            }

            GUILayout.EndScrollView();
        }


        private static void DrawSetting(Rect position)
        {
            if (GUILayout.Button("Create Sound Data"))
            {
                AudioWindowEditor.CreateSoundData();
            }

            GUILayout.Space(10);
            CPUtility.DrawLineLastRectY(3, ConstantControlPanel.POSITION_X_START_CONTENT, position.width);
            GUILayout.Space(10);
            CPUtility.DrawHeader("Music Event");
            GUILayout.Space(10);
            if (GUILayout.Button("Create Play Music Event"))
            {
                AudioWindowEditor.CreatePlayMusicEvent();
            }

            if (GUILayout.Button("Create Pause Music Event"))
            {
                AudioWindowEditor.CreatePauseMusicEvent();
            }

            if (GUILayout.Button("Create Resume Music Event"))
            {
                AudioWindowEditor.CreateResumeMusicEvent();
            }

            if (GUILayout.Button("Create Stop Music Event"))
            {
                AudioWindowEditor.CreateStopMusicEvent();
            }

            GUILayout.Space(10);
            CPUtility.DrawLineLastRectY(3, ConstantControlPanel.POSITION_X_START_CONTENT, position.width);
            GUILayout.Space(10);
            CPUtility.DrawHeader("Sfx Event");
            GUILayout.Space(10);
            if (GUILayout.Button("Create Play Sfx Event"))
            {
                AudioWindowEditor.CreatePlaySfxEvent();
            }

            if (GUILayout.Button("Create Pause Sfx Event"))
            {
                AudioWindowEditor.CreatePauseSfxEvent();
            }

            if (GUILayout.Button("Create Resume Sfx Event"))
            {
                AudioWindowEditor.CreateResumeSfxEvent();
            }

            if (GUILayout.Button("Create Finish Sfx Event"))
            {
                AudioWindowEditor.CreateFinishSfxEvent();
            }

            if (GUILayout.Button("Create Stop Sfx Event"))
            {
                AudioWindowEditor.CreateStopSfxEvent();
            }

            if (GUILayout.Button("Create Stop All Sfx Event"))
            {
                AudioWindowEditor.CreateStopAllSfxEvent();
            }

            GUILayout.Space(10);
            CPUtility.DrawLineLastRectY(3, ConstantControlPanel.POSITION_X_START_CONTENT, position.width);
            GUILayout.Space(10);
            CPUtility.DrawHeader("Volume Changed Variable");
            GUILayout.Space(10);
            if (GUILayout.Button("Create Music Volume Variable"))
            {
                AudioWindowEditor.CreateMusicVolume();
            }

            if (GUILayout.Button("Create Sfx Volume Variable"))
            {
                AudioWindowEditor.CreateSfxVolume();
            }
        }

        static void DrawTab()
        {
            EditorGUILayout.BeginHorizontal();
            bool clickSetting = GUILayout.Toggle(audioTab == AudioTab.Explore, "Explore",
                GUI.skin.button, GUILayout.ExpandWidth(true), GUILayout.Height(25));
            if (clickSetting && audioTab != AudioTab.Explore)
            {
                audioTab = AudioTab.Explore;
            }

            bool clickPickup = GUILayout.Toggle(audioTab == AudioTab.Settings, "Setting",
                GUI.skin.button, GUILayout.ExpandWidth(true), GUILayout.Height(25));
            if (clickPickup && audioTab != AudioTab.Settings)
            {
                audioTab = AudioTab.Settings;
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}