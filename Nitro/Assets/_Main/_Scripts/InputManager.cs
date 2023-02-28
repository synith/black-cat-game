using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synith
{
    public class InputManager : MonoBehaviour
    {
        PlayerControls playerControls;
        Vector2 movementInput;

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

        public Vector2 GetMovementInput() => movementInput;
    }
}
