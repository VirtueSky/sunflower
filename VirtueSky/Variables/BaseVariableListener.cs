using System;
using UnityEngine;
using UnityEngine.Events;
using VirtueSky.Events;

namespace VirtueSky.Variables
{
    public class BaseVariableListener<TType, TEvent, TResponse> : BaseEventListener<TType, TEvent, TResponse>
        where TEvent : BaseVariable<TType>
        where TResponse : UnityEvent<TType>
    {
        [SerializeField] private bool isRaisedOnStart;
        [SerializeField] private bool isRaisedOnEnable;

        private void Start()
        {
            if (isRaisedOnStart)
            {
                foreach (var t in listEventResponseDatas)
                {
                    OnEventRaised(t.@event, t.@event.Value);
                }
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (isRaisedOnEnable)
            {
                foreach (var t in listEventResponseDatas)
                {
                    OnEventRaised(t.@event, t.@event.Value);
                }
            }
        }
    }
}