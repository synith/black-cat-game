using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synith
{
    public class UnitInput : MonoBehaviour
    {
        PlayerControls playerControls;
        Vector2 movementInput;

        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }

        void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += (input) => movementInput = input.ReadValue<Vector2>();
            }
            playerControls.Enable();            
        }

        void OnDisable()
        {
            playerControls.Disable();
        }

        public void HandleAllInput()
        {
            HandleMovementInput();
        }

        void HandleMovementInput()
        {
            Horizontal = movementInput.x;
            Vertical = movementInput.y;
        }
    }
}
