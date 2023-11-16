using System;

namespace VirtueSky.Events
{
    public interface IEvent
    {
        void Raise();
        event Action OnRaised;
        void AddListener(Action action);
        void RemoveListener(Action action);
        void AddListener(IEventListener listener);
        void RemoveListener(IEventListener listener);
        void RemoveAll();
    }

    public interface IEvent<T>
    {
        void Raise(T value);
        event Action<T> OnRaised;
        void AddListener(Action<T> action);
        void RemoveListener(Action<T> action);
        void AddListener(IEventListener<T> listener);
        void RemoveListener(IEventListener<T> listener);
        void RemoveAll();
    }

    public interface IEvent<T, TResult>
    {
        TResult Raise(T value);
        event Func<T, TResult> OnRaised;
        void AddListener(Func<T, TResult> func);
        void RemoveListener(Func<T, TResult> func);
        void AddListener(IEventListener<T, TResult> listener);
        void RemoveListener(IEventListener<T, TResult> listener);
        void RemoveAll();
    }
}