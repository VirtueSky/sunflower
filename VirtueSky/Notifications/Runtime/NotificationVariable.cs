using System;
using System.IO;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Variables;

namespace VirtueSky.Notifications
{
    [CreateAssetMenu(fileName = "notification_channel_data.asset",
        menuName = "Sunflower/Notification Channel")]
    [EditorIcon("scriptable_notification")]
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
        [SerializeField] private bool isRemoteConfigTimeSchedule;

        [ShowIf(nameof(isRemoteConfigTimeSchedule)), SerializeField]
        private IntegerVariable remoteConfigMinute;

        [HideIf(nameof(isRemoteConfigTimeSchedule))]
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

        int GetMinute()
        {
            if (isRemoteConfigTimeSchedule)
            {
                return remoteConfigMinute.Value;
            }

            return minute;
        }

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
                TimeSpan.FromMinutes(GetMinute()),
                smallIcon: smallIcon,
                largeIcon: largeIcon,
                bigPicture: bigPicture,
                namePicture: pathPicture,
                repeat: repeat);
        }

        public void CancelAllScheduled()
        {
            if (!Application.isMobilePlatform) return;
            NotificationConsole.CancelAllScheduled();
        }

        public void ClearBadgeCounterIOS()
        {
            if (!Application.isMobilePlatform) return;
            NotificationConsole.ClearBadgeCounterIOS();
        }
    }
}