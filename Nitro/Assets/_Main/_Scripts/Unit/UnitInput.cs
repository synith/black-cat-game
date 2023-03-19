using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Synith
{
    public class UnitInput : MonoBehaviour
    {
        PlayerControls playerControls;
        Vector2 movementInput;
        float zoomValue;

        public event Action OnJumpPressed;

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
                playerControls.PlayerMovement.Jump.performed += Jump_Performed;
            }
            playerControls.Enable();            
        }

        void Jump_Performed(InputAction.CallbackContext obj)
        {
            OnJumpPressed?.Invoke();
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
