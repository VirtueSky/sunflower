using System;
using System.IO;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Notifications
{
    [CreateAssetMenu(fileName = "notification_channel_data.asset", menuName = "Notification Channel")]
    public class NotificationVariable : ScriptableObject
    {
        [Serializable]
        public class NotificationData
        {
            public string title;
            public string message;

            public NotificationData(string title, string message)
            {
                this.title = title;
                this.message = message;
            }
        }

        [SerializeField] private string identifier;
        public int minute;
        [SerializeField] private bool repeat;
        [SerializeField] internal bool bigPicture;

        [ShowIf(nameof(bigPicture))]
#if UNITY_EDITOR
        [HelpBox(
            "File big picture must be place in folder StreamingAsset, Name Picture must contains file extension ex .jpg")]
#endif

        [SerializeField]
        internal string namePicture;

        [SerializeField] internal bool overrideIcon;

        [SerializeField, ShowIf(nameof(overrideIcon))]
        internal string smallIcon = "icon_0";

        [SerializeField, ShowIf(nameof(overrideIcon))]
        internal string largeIcon = "icon_1";


        [SerializeField] private NotificationData[] datas;


        public void Send()
        {
            if (!Application.isMobilePlatform) return;
            var data = datas.PickRandom();
            string pathPicture = Path.Combine(Application.persistentDataPath, namePicture);
            NotificationConsole.Send(identifier,
                data.title,
                data.message,
                smallIcon: smallIcon,
                largeIcon: largeIcon,
                bigPicture: bigPicture,
                namePicture: pathPicture);
        }

        public void Schedule()
        {
            if (!Application.isMobilePlatform) return;
            var data = datas.PickRandom();

            string pathPicture = Path.Combine(Application.persistentDataPath, namePicture);

            NotificationConsole.Schedule(identifier,
                data.title,
                data.message,
                TimeSpan.FromMinutes(minute),
                smallIcon: smallIcon,
                largeIcon: largeIcon,
                bigPicture: bigPicture,
                namePicture: pathPicture,
                repeat: repeat);
        }
    }
}