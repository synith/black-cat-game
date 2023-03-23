using Synith;
using System.Collections.Generic;
using UnityEngine;

public class CatToy : MonoBehaviour, IObservable<CatToy>
{
    private List<IObserver<CatToy>> observers = new List<IObserver<CatToy>>();

    public void Subscribe(IObserver<CatToy> observer)
    {
        observers.Add(observer);
    }

    public void Unsubscribe(IObserver<CatToy> observer)
    {
        observers.Remove(observer);
    }

    public void Notify(CatToy value)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(value);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Ground ground))
        {
            float delay = 1f;
            Invoke(nameof(DestroySelf), delay);
        }
    }

    void DestroySelf()
    {
        Notify(this);
        Destroy(gameObject);
    }
}