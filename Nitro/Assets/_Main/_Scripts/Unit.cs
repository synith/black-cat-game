using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synith
{
    [RequireComponent(typeof(UnitInput), typeof(UnitMovement))]
    public class Unit : MonoBehaviour
    {
        public UnitInput UnitInput { get; private set; }
        public UnitMovement UnitMovement { get; private set; }

        void Awake()
        {
            UnitInput = GetComponent<UnitInput>();
            UnitMovement = GetComponent<UnitMovement>();
        }

        void Update()
        {
            UnitInput.HandleAllInput();            
        }

        void FixedUpdate()
        {
            UnitMovement.HandleAllMovement();            
        }
    }
}
