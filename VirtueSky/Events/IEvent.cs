namespace VirtueSky.Events
{
    public interface IEvent
    {
        void Raise();
        void AddListener(IEventListener listener);
        void RemoveListener(IEventListener listener);
        void RemoveAll();
    }

    public interface IEvent<T>
    {
        void Raise(T value);
        void AddListener(IEventListener<T> listener);
        void RemoveListener(IEventListener<T> listener);
        void RemoveAll();
    }
}