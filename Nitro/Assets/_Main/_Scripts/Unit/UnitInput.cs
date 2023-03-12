using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synith
{
    public class UnitInput : MonoBehaviour
    {
        PlayerControls playerControls;
        Vector2 movementInput;
        float zoomValue;

        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }
        public float Zoom { get; private set; }

        

        void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += _ => movementInput = _.ReadValue<Vector2>();
                playerControls.PlayerCamera.Zoom.performed += _ => zoomValue = _.ReadValue<float>();
                playerControls.PlayerCamera.Zoom.canceled += _ => zoomValue = 0f;
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
            HandleCameraInput();
        }

        void HandleMovementInput()
        {
            Horizontal = movementInput.x;
            Vertical = movementInput.y;
        }

        void HandleCameraInput() 
        {
            Zoom = zoomValue;
        }
    }
}
