using UnityEngine.Events;

namespace VirtueSky.Events
{
    public class BaseEventResponse : UnityEvent, IEventResponse
    {
    }

    public class BaseEventResponse<TType> : UnityEvent<TType>, IEventResponse
    {
    }
}