using System;
using UnityEngine;

namespace VirtueSky.Tracking
{
    public abstract class TrackingAppsFlyer : ScriptableObject
    {
        protected Action onTracked;

        public event Action OnTracked
        {
            add => onTracked += value;
            remove => onTracked -= value;
        }
    }
}