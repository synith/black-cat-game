using UnityEngine;

namespace Synith
{
    [RequireComponent(typeof(Rigidbody))]
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float rotationSpeed = 360f;

        Rigidbody rb;

        InputManager inputManager;

        Vector2 inputDirection;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputManager = GetComponent<InputManager>();
        }

        private void Update()
        {
            HandleInput();  //TODO: Handle Input in separate class
        }
        void HandleInput()
        {
            inputDirection = inputManager.GetMovementInput();
        }

        void FixedUpdate()
        {
            HandleMovement();
        }


        void HandleMovement()
        {
            Vector3 moveDirection = CalculateMoveDirection();
            Quaternion rotationDirection = CalculateRotationDirection();

            if (moveDirection != Vector3.zero)
            {
                Vector3 position = transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
                Quaternion rotation = Quaternion.RotateTowards(transform.rotation, rotationDirection, rotationSpeed * Time.fixedDeltaTime);

                rb.Move(position, rotation);
            }
        }

        Vector3 CalculateMoveDirection()
        {
            Vector2 input = inputDirection;
            Vector3 moveDirection = Vector3.zero;
            moveDirection += Vector3.ProjectOnPlane(Camera.main.transform.right, transform.up).normalized * input.x;
            moveDirection += Vector3.ProjectOnPlane(Camera.main.transform.forward, transform.up).normalized * input.y;
            return moveDirection;
        }
        Quaternion CalculateRotationDirection()
        {
            float targetAngle = Camera.main.transform.eulerAngles.y; //TODO: Get camera angle somewhere else
            Vector3 rotationDirection = new (0, targetAngle, 0);
            return Quaternion.Euler(rotationDirection);
        }


        Vector3 CalculateRotationDirection2()
        {
            if (CalculateMoveDirection() != Vector3.zero)
            {
                return CalculateMoveDirection();
            }
            else
            {
                return transform.forward;
            }
        }

    }
}