using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    public class BaseEvent : BaseSO, IEvent
    {
        readonly List<IEventListener> listeners = new List<IEventListener>();

        private Action onRaised = null;

#if UNITY_EDITOR
        private void DebugRaiseEvent()
        {
            Raise();
        }
#endif
        public void Raise()
        {
#if UNITY_EDITOR
            // Debug.Log($"===> {name}");
#endif
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(this);
            }

            onRaised?.Invoke();
        }


        public event Action OnRaised
        {
            add => onRaised += value;
            remove => onRaised -= value;
        }

        public void AddListener(Action action)
        {
            onRaised += action;
        }

        public void RemoveListener(Action action)
        {
            onRaised -= action;
        }

        public void AddListener(IEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void RemoveListener(IEventListener listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        public void RemoveAll()
        {
            listeners.Clear();
            onRaised = null;
        }
    }

    public class BaseEvent<TType> : BaseSO, IEvent<TType>
    {
        readonly List<IEventListener<TType>> listeners = new List<IEventListener<TType>>();
        private Action<TType> onRaised = null;
#if UNITY_EDITOR

        [SerializeField] private TType valueDebug = default(TType);
        private void DebugRaiseEvent()
        {
            Raise(valueDebug);
        }
#endif

        public virtual void Raise(TType value)
        {
#if UNITY_EDITOR
            //Debug.Log($"===> {name}");
#endif
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(this, value);
            }

            onRaised?.Invoke(value);
        }

        public event Action<TType> OnRaised
        {
            add => onRaised += value;
            remove => onRaised -= value;
        }

        public void AddListener(Action<TType> action)
        {
            onRaised += action;
        }

        public void RemoveListener(Action<TType> action)
        {
            onRaised -= action;
        }

        public void AddListener(IEventListener<TType> listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void RemoveListener(IEventListener<TType> listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        public void RemoveAll()
        {
            listeners.Clear();
            onRaised = null;
        }
    }

    public class BaseEvent<TType, TResult> : BaseSO, IEvent<TType, TResult>
    {
        readonly List<IEventListener<TType, TResult>> listeners = new List<IEventListener<TType, TResult>>();
        private Func<TType, TResult> onRaised = null;

#if UNITY_EDITOR
        [SerializeField] private TType valueDebug = default(TType);
        [ReadOnly] [SerializeField] private TResult valueResult = default(TResult);

        private void DebugRaiseEvent()
        {
            valueResult = Raise(valueDebug);
        }
#endif
        public TResult Raise(TType value)
        {
            TResult result = default;
            if (!Application.isPlaying) return result;
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(this, value);
            }

            if (onRaised != null) result = onRaised.Invoke(value);
            return result;
        }

        public event Func<TType, TResult> OnRaised
        {
            add { onRaised += value; }
            remove { onRaised -= value; }
        }


        public void AddListener(Func<TType, TResult> func)
        {
            onRaised += func;
        }

        public void RemoveListener(Func<TType, TResult> func)
        {
            onRaised -= func;
        }

        public void AddListener(IEventListener<TType, TResult> listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void RemoveListener(IEventListener<TType, TResult> listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        public void RemoveAll()
        {
            listeners.Clear();
            onRaised = null;
        }
    }

#if UNITY_EDITOR


#endif
}