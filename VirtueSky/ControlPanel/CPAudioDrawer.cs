using UnityEditor;
using UnityEngine;
using VirtueSky.Audio;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPAudioDrawer
    {
        public static void OnDrawAudio(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.Audio, "AUDIO");
            GUILayout.Space(10);
            if (GUILayout.Button("Create Sound Data"))
            {
                AudioWindowEditor.CreateSoundData();
            }

            GUILayout.Space(10);
            CPUtility.DrawLineLastRectY(3, ConstantControlPanel.POSITION_X_START_CONTENT, position.width);
            GUILayout.Space(10);
            GUILayout.Label("MUSIC EVENT", EditorStyles.boldLabel);
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
            GUILayout.Label("SFX EVENT", EditorStyles.boldLabel);
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
            GUILayout.Label("VOLUME CHANGE", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create Music Volume Variable"))
            {
                AudioWindowEditor.CreateMusicVolume();
            }

            if (GUILayout.Button("Create Sfx Volume Variable"))
            {
                AudioWindowEditor.CreateSfxVolume();
            }

            GUILayout.EndVertical();
        }
    }
}