using System;
using System.Collections.Generic;
using UnityEngine;

namespace Synith
{
    public class CatToyTracker : MonoBehaviour, IObserver<CatToy>
    {
        public event EventHandler<int> OnToyDestroyedCount;
        public event EventHandler OnGameWon;
        public int CatToyMax { get; private set; }

        public int CatToyCurrent { get; private set; }

        List<IObserver<ToySpawn>> observers = new List<IObserver<ToySpawn>>();

        public void Subscribe(IObserver<ToySpawn> observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver<ToySpawn> observer)
        {
            observers.Remove(observer);
        }

        void Notify(ToySpawn value)
        {
            foreach (var observer in observers)
            {
                observer.OnNotify(value);
            }
        }

        void Start()
        {
            CatToy[] catToys = FindObjectsOfType<CatToy>();
            CatToyMax = catToys.Length;

            foreach (CatToy catToy in catToys)
            {
                catToy.Subscribe(this);
            }

            OnToyDestroyedCount?.Invoke(this, CatToyCurrent);
        }

        public void OnNotify(CatToy value)
        {
            AddScore();

            // Notify observers of the toy count update
            Notify(null);
        }

        void AddScore()
        {
            CatToyCurrent++;
            OnToyDestroyedCount?.Invoke(this, CatToyCurrent);

            if (CatToyCurrent >= CatToyMax)
            {
                // Game is Won
                Debug.Log("You Win!");
                OnGameWon?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
