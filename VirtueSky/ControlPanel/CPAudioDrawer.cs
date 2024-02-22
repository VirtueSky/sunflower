using UnityEditor;
using UnityEngine;
using VirtueSky.Audio;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPAudioDrawer
    {
        public static void OnDrawAudio()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("AUDIO", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create SoundComponentPool"))
            {
                AudioWindowEditor.CreateSoundComponentPool();
            }

            if (GUILayout.Button("Create Event Audio Handle"))
            {
                AudioWindowEditor.CreateEventAudioHandle();
            }

            if (GUILayout.Button("Create Sound Data"))
            {
                AudioWindowEditor.CreateSoundData();
            }

            GUILayout.EndVertical();
        }
    }
}