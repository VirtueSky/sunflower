using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Core;

namespace VirtueSky.Events
{
    public class EventDispatcher : BaseMono
    {
        [FormerlySerializedAs("eventNoFunc")] [FormerlySerializedAs("scriptableEvent")] [FormerlySerializedAs("event")] [SerializeField]
        EventNoParam eventNoParam;

        [SerializeField] bool dispatchOnEnable;

        public override void Initialize()
        {
            if (dispatchOnEnable)
            {
                Dispatch();
            }
        }

        public void Dispatch()
        {
            eventNoParam.Raise();
        }
    }
}