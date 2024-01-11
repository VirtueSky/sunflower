using UnityEditor;
using UnityEngine;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPNotificationChanelDrawer
    {
        public static void OnDrawNotificationChanel()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("NOTIFICATION CHANEL", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create Notification Chanel"))
            {
                NotificationWindowEditor.CreateNotificationChannel();
            }

            GUILayout.EndVertical();
        }
    }
}