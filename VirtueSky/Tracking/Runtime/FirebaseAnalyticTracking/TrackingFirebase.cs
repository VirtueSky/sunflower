using System;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Tracking
{
    public abstract class TrackingFirebase : ScriptableObject
    {
        [Space] [HeaderLine("Event Name")] [SerializeField]
        protected string eventName;

        protected Action onTracked;

        public event Action OnTracked
        {
            add => onTracked += value;
            remove => onTracked -= value;
        }
    }
}