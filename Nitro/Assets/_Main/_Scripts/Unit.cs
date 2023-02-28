using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synith
{
    public class Unit : MonoBehaviour
    {
        public InputManager InputManager { get; private set; }
        public UnitMovement UnitMovement { get; private set; }

        void Awake()
        {
            InputManager = GetComponent<InputManager>();
            UnitMovement = GetComponent<UnitMovement>();
        }

        void Update()
        {
            InputManager.HandleAllInput();            
        }

        void FixedUpdate()
        {
            UnitMovement.HandleAllMovement();            
        }
    }
}
