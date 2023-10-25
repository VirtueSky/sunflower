#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Notifications.iOS;
using UnityEngine;

namespace VirtueSky.Notifications
{
    internal static class NotificationIOS
    {
        private static Dictionary<string, bool> channelRegistered = new Dictionary<string, bool>();

        internal static async Task RequestAuthorization()
        {
            var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound;
            using (var req = new AuthorizationRequest(authorizationOption, true))
            {
                while (!req.IsFinished)
                {
                    await Task.Yield();
                }
            }
        }

        private static async Task RegisterNotificationChannel(string identifier, string title, string subtitle, string body, iOSNotificationTrigger trigger)
        {
            await RequestAuthorization();

            if (new iOSNotificationSettings().AuthorizationStatus != AuthorizationStatus.Authorized)
            {
                Debug.LogError("IOSNotification non authorized for schedule notification");
            }

            var unregistered = !channelRegistered.ContainsKey(identifier) || !channelRegistered[identifier];
            if (unregistered)
            {
                var notification = new iOSNotification()
                {
                    Identifier = identifier,
                    Title = title,
                    Body = body,
                    Subtitle = subtitle,
                    ShowInForeground = true,
                    ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Badge | PresentationOption.Sound),
                    CategoryIdentifier = "category_a",
                    ThreadIdentifier = "thread1",
                    Trigger = trigger,
                };

                try
                {
                    iOSNotificationCenter.ScheduleNotification(notification);
                    channelRegistered[identifier] = true;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        internal static void Schedule(string identifier, string title, string subtitle, string text, TimeSpan fireTime, bool repeat)
        {
            var interval = fireTime;
            if (interval <= TimeSpan.Zero) interval = TimeSpan.FromSeconds(1);

            var timeTrigger = new iOSNotificationTimeIntervalTrigger { TimeInterval = interval, Repeats = repeat };

            RegisterNotificationChannel(identifier,
                title,
                subtitle,
                text,
                timeTrigger);
        }

        internal static void CancelAllScheduled()
        {
            iOSNotificationCenter.RemoveAllScheduledNotifications();
        }

        internal static void ClearBadgeCounter()
        {
            iOSNotificationCenter.ApplicationBadge = 0;
        }
    }
}
#endif