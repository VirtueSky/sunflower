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

        public enum ScheduleMode
        {
            RelativeTime,    // Schedule sau X phút từ thời điểm hiện tại
            SpecificTime     // Schedule vào giờ cụ thể (ví dụ: 6h tối ngày mai)
        }

        [SerializeField] private string identifier;
        [SerializeField] private ScheduleMode scheduleMode = ScheduleMode.RelativeTime;
        
        // === Relative Time Settings ===
        [ShowIf(nameof(scheduleMode), ScheduleMode.RelativeTime)]
        [SerializeField] private bool isRemoteConfigTimeSchedule;

        [ShowIf(nameof(isRemoteConfigTimeSchedule)), SerializeField]
        private IntegerVariable remoteConfigMinute;

        [HideIf(nameof(isRemoteConfigTimeSchedule))]
        [ShowIf(nameof(scheduleMode), ScheduleMode.RelativeTime)]
        public int minute;

        // === Specific Time Settings ===
        [ShowIf(nameof(scheduleMode), ScheduleMode.SpecificTime)]
        [SerializeField, Range(0, 23)] private int targetHour = 18; // 6h tối
        
        [ShowIf(nameof(scheduleMode), ScheduleMode.SpecificTime)]
        [SerializeField, Range(0, 59)] private int targetMinute = 0;

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

        DateTime? GetFireTime()
        {
            if (scheduleMode == ScheduleMode.SpecificTime)
            {
                var now = DateTime.Now;
                return new DateTime(now.Year, now.Month, now.Day, 
                    targetHour, targetMinute, 0);
            }
            
            return null; // RelativeTime mode không cần DateTime cụ thể
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

            if (scheduleMode == ScheduleMode.SpecificTime)
            {
                // Schedule vào giờ cụ thể
                var fireTime = GetFireTime();
                if (fireTime.HasValue)
                {
                    NotificationConsole.ScheduleAtSpecificTime(identifier,
                        data.title,
                        data.message,
                        fireTime.Value,
                        smallIcon: smallIcon,
                        largeIcon: largeIcon,
                        bigPicture: bigPicture,
                        namePicture: pathPicture,
                        repeat: repeat); // Repeat interval cố định 24h
                }
            }
            else
            {
                // Schedule theo khoảng thời gian tương đối (logic cũ)
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
        }

        /// <summary>
        /// Schedule notification with a custom delay time.
        /// Used for scheduling notifications after app quit.
        /// </summary>
        public void ScheduleWithDelay(TimeSpan delay)
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
                repeat: false); // Không repeat khi schedule từ quit app
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