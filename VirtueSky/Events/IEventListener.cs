using UnityEngine.Events;

namespace VirtueSky.Events
{
    public interface IEventListener
    {
        void OnEventRaised(BaseEvent eventRaise);
    }

    public interface IEventListener<TType>
    {
        void OnEventRaised(BaseEvent<TType> eventRaise, TType value);
    }

    public interface IEventListener<TType, TResult>
    {
        void OnEventRaised(BaseEvent<TType, TResult> eventRaise, TType value);
    }
}