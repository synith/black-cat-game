using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synith
{
    public class CatToyTracker : MonoBehaviour
    {
        public event EventHandler<int> OnToyDestroyedCount;
        public event EventHandler OnGameWon;
        public int CatToyMax { get; private set; }

        int catToyCurrent;

        void Start()
        {
            CatToyMax = FindObjectsOfType<CatToy>().Length;

            CatToy.OnDestroyed += CatToy_OnDestroyed;


            OnToyDestroyedCount?.Invoke(this, catToyCurrent);
        }

        void CatToy_OnDestroyed(object sender, EventArgs e)
        {
            AddScore();
        }

        void AddScore()
        {
            catToyCurrent++;
            OnToyDestroyedCount?.Invoke(this, catToyCurrent);

            if (catToyCurrent >= CatToyMax)
            {
                // Game is Won
                Debug.Log("You Win!");
                OnGameWon?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
