using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synith
{
    public class CatToy : MonoBehaviour
    {
        public static event EventHandler OnDestroyed;

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
            OnDestroyed?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
}
