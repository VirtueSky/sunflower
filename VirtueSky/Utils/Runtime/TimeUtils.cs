using System;

namespace VirtueSky.Utils
{
    public static class TimeUtils
    {
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0);
        public static readonly DateTime EpochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime Now => DateTime.Now;
        public static long CurrentTicks => DateTime.Now.Ticks - Epoch.Ticks;

        public static long CurrentTicksUtc => DateTime.UtcNow.Ticks - EpochUtc.Ticks;

        public static long CurrentDays => CurrentTicks / TimeSpan.TicksPerDay;

        public static double CurrentSeconds => TicksToSeconds(CurrentTicks);

        public static double CurrentSecondsUtc => TicksToSeconds(CurrentTicksUtc);

        public static DateTime TicksToDateTime(long ticks)
        {
            var date = Epoch + TimeSpan.FromTicks(ticks);
            return date.ToLocalTime();
        }

        public static DateTime SecondsToDateTime(double seconds)
        {
            var date = Epoch + TimeSpan.FromSeconds(seconds);
            return date;
        }

        public static double TimespanSeconds(long ticks, long lastTicks)
        {
            return TimeSpan.FromTicks(ticks - lastTicks).TotalSeconds;
        }

        public static double TimespanHours(long ticks, long lastTicks)
        {
            return TimeSpan.FromTicks(ticks - lastTicks).TotalHours;
        }

        public static long SecondsToTicks(double seconds)
        {
            return (long)(seconds * TimeSpan.TicksPerSecond);
        }

        public static int SecondsToMiniseconds(double seconds)
        {
            return (int)(seconds * 1000);
        }

        public static double MinisecondsToSeconds(int miniseconds)
        {
            return (double)(miniseconds / 1000f);
        }

        public static double TicksToSeconds(long ticks)
        {
            return (double)ticks / TimeSpan.TicksPerSecond;
        }

        public static long SecondsToDays(double seconds)
        {
            return SecondsToTicks(seconds) / TimeSpan.TicksPerDay;
        }

        public static double DaysToSeconds(long days)
        {
            return TimeSpan.FromDays(days).TotalSeconds;
        }

        public static string FormatTimeSpan(double seconds)
        {
            var span = new TimeSpan(SecondsToTicks(seconds));
            return span.Days > 0 ? $"{span.Days}:{span.Hours:00}:{span.Minutes:00}:{span.Seconds:00}" :
                span.Hours > 0 ? $"{span.Hours:00}:{span.Minutes:00}:{span.Seconds:00}" :
                $"{span.Minutes:00}:{span.Seconds:00}";
        }

        public static string FormatTimeSpanExcludeSecond(double seconds)
        {
            var span = new TimeSpan(SecondsToTicks(seconds));
            return span.Hours > 0 ? $"{span.Hours:00}h:{span.Minutes:00}min" : $"{span.Minutes:00}min";
        }

        public static float TargetTimeScale { get; set; } = 1;
    }
}