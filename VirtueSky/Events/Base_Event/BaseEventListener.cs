using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VirtueSky.Core;


namespace VirtueSky.Events
{
    public class BaseEventListener<TEvent, TResponse> : EventListenerMono, IEventListener
        where TEvent : BaseEvent
        where TResponse : UnityEvent
    {
        [SerializeField] private List<EventResponseData> listEventResponseDatas = new List<EventResponseData>();
        private readonly Dictionary<BaseEvent, UnityEvent> _dictionary = new Dictionary<BaseEvent, UnityEvent>();

        [Serializable]
        public class EventResponseData
        {
            public TEvent @event;
            public TResponse response;
        }

        protected override void ToggleListenerEvent(bool isListenerEvent)
        {
            if (isListenerEvent)
            {
                foreach (var t in listEventResponseDatas)
                {
                    t.@event.AddListener(this);
                    _dictionary.TryAdd(t.@event, t.response);
                }
            }
            else
            {
                foreach (var t in listEventResponseDatas)
                {
                    t.@event.RemoveListener(this);
                    if (_dictionary.ContainsKey(t.@event)) _dictionary.Remove(t.@event);
                }
            }
        }

        public virtual void OnEventRaised(BaseEvent eventRaise)
        {
            _dictionary[eventRaise]?.Invoke();
        }
    }

    public class BaseEventListener<TType, TEvent, TResponse> : EventListenerMono, IEventListener<TType>
        where TEvent : BaseEvent<TType>
        where TResponse : UnityEvent<TType>
    {
        [SerializeField] protected List<EventResponseData> listEventResponseDatas = new List<EventResponseData>();

        protected readonly Dictionary<BaseEvent<TType>, UnityEvent<TType>> _dictionary =
            new Dictionary<BaseEvent<TType>, UnityEvent<TType>>();

        [Serializable]
        public class EventResponseData
        {
            public TEvent @event;
            public TResponse response;
        }

        protected override void ToggleListenerEvent(bool isListenerEvent)
        {
            if (isListenerEvent)
            {
                foreach (var t in listEventResponseDatas)
                {
                    t.@event.AddListener(this);
                    _dictionary.TryAdd(t.@event, t.response);
                }
            }
            else
            {
                foreach (var t in listEventResponseDatas)
                {
                    t.@event.RemoveListener(this);
                    if (_dictionary.ContainsKey(t.@event)) _dictionary.Remove(t.@event);
                }
            }
        }

        public virtual void OnEventRaised(BaseEvent<TType> eventRaise, TType value)
        {
            _dictionary[eventRaise]?.Invoke(value);
        }
    }

    public class BaseEventListener<TType, TResult, TEvent, TResponse> : EventListenerMono, IEventListener<TType, TResult>
        where TEvent : BaseEvent<TType, TResult>
        where TResponse : UnityEvent<TType>
    {
        [SerializeField] protected List<EventResponseData> listEventResponseDatas = new List<EventResponseData>();

        protected readonly Dictionary<BaseEvent<TType, TResult>, UnityEvent<TType>> _dictionary =
            new Dictionary<BaseEvent<TType, TResult>, UnityEvent<TType>>();

        [Serializable]
        public class EventResponseData
        {
            public TEvent @event;
            public TResponse response;
        }

        public void OnEventRaised(BaseEvent<TType, TResult> eventRaise, TType value)
        {
            _dictionary[eventRaise]?.Invoke(value);
        }

        protected override void ToggleListenerEvent(bool isListenerEvent)
        {
            if (isListenerEvent)
            {
                foreach (var t in listEventResponseDatas)
                {
                    t.@event.AddListener(this);
                    _dictionary.TryAdd(t.@event, t.response);
                }
            }
            else
            {
                foreach (var t in listEventResponseDatas)
                {
                    t.@event.RemoveListener(this);
                    if (_dictionary.ContainsKey(t.@event)) _dictionary.Remove(t.@event);
                }
            }
        }
    }
}