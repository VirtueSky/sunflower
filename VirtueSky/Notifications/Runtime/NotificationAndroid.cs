#if UNITY_ANDROID && VIRTUESKY_NOTIFICATION
using System;
using System.Collections.Generic;
using Unity.Notifications.Android;

namespace VirtueSky.Notifications
{
    internal static class NotificationAndroid
    {
        private static readonly Dictionary<string, bool> ChannelRegistered = new Dictionary<string, bool>();

        private static void RegisterNotificationChannel(string identifier, string name, string description)
        {
            ChannelRegistered.TryGetValue(identifier, out bool registered);
            if (registered) return;

            AndroidNotificationCenter.RegisterNotificationChannel(new AndroidNotificationChannel
            {
                Id = identifier, Name = name, Importance = Importance.High, Description = description
            });
            ChannelRegistered.Add(identifier, true);
        }

        internal static void Schedule(
            string identifier,
            string title,
            string text,
            TimeSpan timeOffset,
            string largeIcon = null,
            string channelName = "Nova",
            string channelDescription = "Newsletter Announcement",
            string smallIcon = "icon_0",
            BigPictureStyle? bigPictureStyle = null,
            bool repeat = false)
        {
            RegisterNotificationChannel(identifier, channelName, channelDescription);

            var notification = new AndroidNotification()
            {
                Title = title,
                Text = text,
                FireTime = DateTime.Now + timeOffset,
                Group = identifier,
                GroupSummary = true,
                ShouldAutoCancel = true,
                BigPicture = bigPictureStyle,
            };

            if (repeat) notification.RepeatInterval = timeOffset;

            if (largeIcon != null) notification.LargeIcon = largeIcon;
            if (smallIcon != null) notification.SmallIcon = smallIcon;

            AndroidNotificationCenter.SendNotification(notification, identifier);
        }

        internal static void ScheduleAtSpecificTime(
            string identifier,
            string title,
            string text,
            DateTime fireTime,
            string largeIcon = null,
            string channelName = "Nova",
            string channelDescription = "Newsletter Announcement",
            string smallIcon = "icon_0",
            BigPictureStyle? bigPictureStyle = null,
            bool repeat = false)
        {
            RegisterNotificationChannel(identifier, channelName, channelDescription);
            var now = DateTime.Now;
            var todayFireTime = new DateTime(now.Year, now.Month, now.Day, 
                fireTime.Hour, fireTime.Minute, fireTime.Second);
            var adjustedFireTime = todayFireTime;
            if (adjustedFireTime <= now)
            {
                adjustedFireTime = adjustedFireTime.AddDays(1);
            }
            
            var timeOffset = adjustedFireTime - now;

            var notification = new AndroidNotification()
            {
                Title = title,
                Text = text,
                FireTime = DateTime.Now + timeOffset,
                Group = identifier,
                GroupSummary = true,
                ShouldAutoCancel = true,
                BigPicture = bigPictureStyle,
            };
            
            if (repeat)
            {
                notification.RepeatInterval = TimeSpan.FromDays(1);
            }

            if (largeIcon != null) notification.LargeIcon = largeIcon;
            if (smallIcon != null) notification.SmallIcon = smallIcon;

            AndroidNotificationCenter.SendNotification(notification, identifier);
        }

        internal static void CancelAllScheduled()
        {
            AndroidNotificationCenter.CancelAllScheduledNotifications();
        }
    }
}
#endif