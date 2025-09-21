using UnityEditor;
using UnityEngine;
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

        private static AudioTab audioTab = AudioTab.Explore;

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
            GUILayout.BeginHorizontal();
            DrawLeftExplore(position);
            GUILayout.Space(20);
            DrawRightExplore(position);
            GUILayout.EndHorizontal();
        }

        private static void DrawLeftExplore(Rect position)
        {
            GUILayout.Label($"Coming soon");
        }

        private static void DrawRightExplore(Rect position)
        {
            GUILayout.Label($"Coming soon");
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