using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.Notifications
{
    [EditorIcon("script_noti"), HideMonoScript]
    public class NotificationPrepare : BaseMono
    {
        [Space(20), SerializeField] private NotificationVariable[] notificationVariables;
        [SerializeField] private bool autoSchedule = true;
        private void Start()
        {
#if UNITY_ANDROID
            PermissionPostNotification();
            if (Application.isMobilePlatform)
            {
                var strs = new List<string>();

                foreach (var variable in notificationVariables)
                {
                    if (!variable.bigPicture) continue;
                    if (!strs.Contains(variable.namePicture)) strs.Add(variable.namePicture);
                }

                foreach (string s in strs)
                {
                    App.StartCoroutine(PrepareImage(Application.persistentDataPath, s));
                }
            }
#endif
            if (autoSchedule) AutoSchedule();
        }
#if UNITY_ANDROID
        private IEnumerator PrepareImage(string destDir, string filename)
        {
            string path = Path.Combine(destDir, filename);
            if (File.Exists(path)) yield break;
            using var uwr =
                UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, filename));
            yield return uwr.SendWebRequest();
            File.WriteAllBytes(path, uwr.downloadHandler.data);
        }

        void PermissionPostNotification()
        {
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
            {
                UnityEngine.Android.Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
            }
        }

#endif
        void AutoSchedule()
        {
            foreach (var notification in notificationVariables)
            {
                notification.Schedule();
            }
        }
    }
}