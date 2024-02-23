using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.Notifications
{
    [EditorIcon("script_noti")]
    public class NotificationPrepare : BaseMono
    {
#if UNITY_ANDROID
        [SerializeField] private NotificationVariable[] notificationVariables;

        private void Start()
        {
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
        }

        private IEnumerator PrepareImage(string destDir, string filename)
        {
            string path = Path.Combine(destDir, filename);
            if (File.Exists(path)) yield break;
            using var uwr =
                UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, filename));
            yield return uwr.SendWebRequest();
            File.WriteAllBytes(path, uwr.downloadHandler.data);
        }
#endif
    }
}