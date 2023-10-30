using System;

namespace VirtueSky.Notifications
{
    internal static class NotificationConsole
    {
        internal static void Send(
            string identifier,
            string title,
            string text,
            string largeIcon = null,
            string channelName = "Nova",
            string channelDescription = "Newsletter Announcement",
            string smallIcon = null,
            bool bigPicture = false,
            string namePicture = "")
        {
            Schedule(identifier,
                title,
                text,
                TimeSpan.FromMilliseconds(250),
                largeIcon,
                channelName,
                channelDescription,
                smallIcon,
                bigPicture,
                namePicture);
        }

        internal static void Schedule(
            string identifier,
            string title,
            string text,
            TimeSpan timeOffset,
            string largeIcon = null,
            string channelName = "Nova",
            string channelDescription = "Newsletter Announcement",
            string smallIcon = null,
            bool bigPicture = false,
            string namePicture = "",
            bool repeat = false)
        {
            if (string.IsNullOrEmpty(smallIcon)) smallIcon = "icon_0";
            if (string.IsNullOrEmpty(largeIcon)) largeIcon = "icon_1";

#if UNITY_ANDROID
            Unity.Notifications.Android.BigPictureStyle? bigPictureStyle = null;
            if (bigPicture)
            {
                bigPictureStyle = new Unity.Notifications.Android.BigPictureStyle { Picture = namePicture, ContentTitle = title, ContentDescription = text };
            }

            NotificationAndroid.Schedule(identifier,
                title,
                text,
                timeOffset,
                largeIcon,
                channelName,
                channelDescription,
                smallIcon,
                bigPictureStyle,
                repeat);
#elif UNITY_IOS && VIRTUESKY_NOTIFICATION
			NotificationIOS.Schedule(identifier, title, "", text, timeOffset, repeat);
#endif
        }

        internal static void CancelAllScheduled()
        {
#if UNITY_ANDROID && VIRTUESKY_NOTIFICATION
            NotificationAndroid.CancelAllScheduled();
#elif UNITY_IOS && VIRTUESKY_NOTIFICATION
            NotificationIOS.CancelAllScheduled();
#endif
        }

        internal static void ClearBadgeCounteriOS()
        {
#if UNITY_IOS && VIRTUESKY_NOTIFICATION
            NotificationIOS.ClearBadgeCounter();
#endif
        }
    }
}