using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.Component
{
    public class TimeRemainingComponent : BaseMono
    {
        [HorizontalGroup("Year-Month-Day"), LabelWidth(90)] [SerializeField]
        private int targetYear;

        [HorizontalGroup("Year-Month-Day"), LabelWidth(90)] [SerializeField]
        private int targetMonth;

        [HorizontalGroup("Year-Month-Day"), LabelWidth(90)] [SerializeField]
        private int targetDay;

        [HorizontalGroup("Hour-Minute-Second"), LabelWidth(90)] [SerializeField]
        private int targetHour;

        [HorizontalGroup("Hour-Minute-Second"), LabelWidth(90)] [SerializeField]
        private int targetMinute;

        [HorizontalGroup("Hour-Minute-Second"), LabelWidth(90)] [SerializeField]
        private int targetSecond;

        private DateTime targetTime;


        public void InitTargetTime()
        {
            targetTime = new DateTime(targetYear, targetMonth, targetDay, targetHour, targetMinute, targetSecond);
        }

        public TimeSpan GetTimeRemaining()
        {
            return (targetTime - DateTime.Now).TotalSeconds > 0 ? (targetTime - DateTime.Now) : TimeSpan.Zero;
        }
    }
}