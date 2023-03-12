using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synith
{
    [RequireComponent(typeof(UnitInput), typeof(UnitMovement), typeof(UnitCamera))]
    public class Unit : MonoBehaviour
    {
        public UnitInput UnitInput { get; private set; }
        public UnitMovement UnitMovement { get; private set; }
        public UnitCamera UnitCamera { get; private set; }

        void Awake()
        {
            UnitInput = GetComponent<UnitInput>();
            UnitMovement = GetComponent<UnitMovement>();
            UnitCamera = GetComponent<UnitCamera>();
        }

        void Update()
        {
            UnitInput.HandleAllInput();
            UnitCamera.HandleAllCamera();
        }

        void FixedUpdate()
        {
            UnitMovement.HandleAllMovement();
        }
    }
}
