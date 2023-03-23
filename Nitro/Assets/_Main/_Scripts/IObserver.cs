namespace Synith
{
    /// <summary>
    /// IObserver
    public interface IObserver<T>
    {
        void OnNotify(T value);
    }
}
