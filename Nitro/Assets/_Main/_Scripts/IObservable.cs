namespace Synith
{
    /// <summary>
    /// IObserver
    public interface IObservable<T>
    {
        void Subscribe(IObserver<T> observer);
        void Unsubscribe(IObserver<T> observer);
        void Notify(T value);
    }
}
