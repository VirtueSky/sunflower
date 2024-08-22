using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPNotificationChanelDrawer
    {
        public static void OnDrawNotificationChanel(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.NotificationsChanel, "NOTIFICATION");
            GUILayout.Space(10);
            if (GUILayout.Button("Create Notification Chanel"))
            {
                NotificationWindowEditor.CreateNotificationChannel();
            }

            GUILayout.Space(10);
            CPUtility.DrawLineLastRectY(3, ConstantControlPanel.POSITION_X_START_CONTENT, position.width);
            GUILayout.Space(10);
            GUILayout.Label("INSTALL MOBILE NOTIFICATIONS", EditorStyles.boldLabel);
            GUILayout.Space(10);
            CPUtility.DrawButtonInstallPackage("Install Mobile Notifications", "Remove Mobile Notifications",
                ConstantPackage.PackageNameMobileNotification, ConstantPackage.MaxVersionMobileNotification);
            GUILayout.Space(10);
            CPUtility.DrawLineLastRectY(3, ConstantControlPanel.POSITION_X_START_CONTENT, position.width);
            GUILayout.Space(10);
            GUILayout.Label("ADD DEFINE SYMBOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);
#if !VIRTUESKY_NOTIFICATION
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols \"{ConstantDefineSymbols.VIRTUESKY_NOTIFICATION}\" to use IAP",
                MessageType.Info);
#endif
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_NOTIFICATION);
            GUILayout.EndVertical();
        }
    }
}