using System;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.Component
{
    [EditorIcon("icon_csharp")]
    public class TimeRemainingComponent : BaseMono
    {
        [SerializeField] private int targetYear;

        [SerializeField] private int targetMonth;

        [SerializeField] private int targetDay;

        [SerializeField] private int targetHour;

        [SerializeField] private int targetMinute;

        [SerializeField] private int targetSecond;

        private DateTime targetTime;


        public void InitTargetTime()
        {
            targetTime = new DateTime(targetYear, targetMonth, targetDay, targetHour, targetMinute, targetSecond);
        }

        public void InitTargetTime(int year, int month, int day, int hour, int minute, int second)
        {
            targetTime = new DateTime(year, month, day, hour, minute, second);
        }

        public TimeSpan GetTimeRemaining()
        {
            return (targetTime - DateTime.Now).TotalSeconds > 0 ? (targetTime - DateTime.Now) : TimeSpan.Zero;
        }
    }
}