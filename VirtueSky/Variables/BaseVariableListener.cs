using UnityEngine;
using UnityEngine.Events;
using VirtueSky.Events;

namespace VirtueSky.Variables
{
    public class BaseVariableListener<TType, TEvent, TResponse> : BaseEventListener<TType, TEvent, TResponse>
        where TEvent : BaseVariable<TType>
        where TResponse : UnityEvent<TType>
    {
        [SerializeField] bool setOnEnable;

        public override void Initialize()
        {
            base.Initialize();
            if (setOnEnable)
            {
                foreach (var t in listEventResponseDatas)
                {
                    OnEventRaised(t.@event, t.@event.Value);
                }
            }
        }

        public override void DoEnable()
        {
            base.DoEnable();
            if (setOnEnable)
            {
                foreach (var t in listEventResponseDatas)
                {
                    OnEventRaised(t.@event, t.@event.Value);
                }
            }
        }
    }
}