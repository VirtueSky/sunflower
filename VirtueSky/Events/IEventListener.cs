using UnityEngine.Events;

namespace VirtueSky.Events
{
    public interface IEventListener
    {
        void OnEventRaised(BaseEvent eventRaise);
        void ToggleListenerEvent(bool isListenerEvent);
    }

    public interface IEventListener<TType>
    {
        void OnEventRaised(BaseEvent<TType> eventRaise, TType value);
        void ToggleListenerEvent(bool isListenerEvent);
    }
}