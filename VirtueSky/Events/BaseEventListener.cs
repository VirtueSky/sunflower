using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VirtueSky.Core;


namespace VirtueSky.Events
{
    public class BaseEventListener<TEvent, TResponse> : MonoBehaviour, IEventListener
        where TEvent : BaseEvent
        where TResponse : UnityEvent
    {
        [SerializeField] private BindingListener bindingListener;

        [SerializeField] private List<EventResponseData> listEventResponseDatas = new List<EventResponseData>();
        private readonly Dictionary<BaseEvent, UnityEvent> _dictionary = new Dictionary<BaseEvent, UnityEvent>();

        [Serializable]
        public class EventResponseData
        {
            public TEvent @event;
            public TResponse response;
        }

        public void ToggleListenerEvent(bool isListenerEvent)
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
            _dictionary[eventRaise].Invoke();
        }

        #region Binding Listener

        private void Awake()
        {
            if (bindingListener == BindingListener.UNTIL_DESTROY)
            {
                ToggleListenerEvent(true);
            }
        }

        private void OnEnable()
        {
            if (bindingListener == BindingListener.UNTIL_DISABLE)
            {
                ToggleListenerEvent(true);
            }
        }

        private void OnDisable()
        {
            if (bindingListener == BindingListener.UNTIL_DISABLE)
            {
                ToggleListenerEvent(false);
            }
        }

        private void OnDestroy()
        {
            if (bindingListener == BindingListener.UNTIL_DESTROY)
            {
                ToggleListenerEvent(false);
            }
        }

        #endregion
    }

    public class BaseEventListener<TType, TEvent, TResponse> : BaseMono, IEventListener<TType>
        where TEvent : BaseEvent<TType>
        where TResponse : UnityEvent<TType>
    {
        [SerializeField] private BindingListener bindingListener;

        [SerializeField] protected List<EventResponseData> listEventResponseDatas = new List<EventResponseData>();

        protected readonly Dictionary<BaseEvent<TType>, UnityEvent<TType>> _dictionary =
            new Dictionary<BaseEvent<TType>, UnityEvent<TType>>();

        [Serializable]
        public class EventResponseData
        {
            public TEvent @event;
            public TResponse response;
        }

        public void ToggleListenerEvent(bool isListenerEvent)
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
            _dictionary[eventRaise].Invoke(value);
        }

        #region Binding Listener

        private void Awake()
        {
            if (bindingListener == BindingListener.UNTIL_DESTROY)
            {
                ToggleListenerEvent(true);
            }
        }

        private void OnEnable()
        {
            if (bindingListener == BindingListener.UNTIL_DISABLE)
            {
                ToggleListenerEvent(true);
            }
        }

        private void OnDisable()
        {
            if (bindingListener == BindingListener.UNTIL_DISABLE)
            {
                ToggleListenerEvent(false);
            }
        }

        private void OnDestroy()
        {
            if (bindingListener == BindingListener.UNTIL_DESTROY)
            {
                ToggleListenerEvent(false);
            }
        }

        #endregion
    }
}

public enum BindingListener
{
    UNTIL_DISABLE,
    UNTIL_DESTROY
}