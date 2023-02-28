using UnityEngine;

namespace Synith
{
    [RequireComponent(typeof(Rigidbody))]
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 4f;
        [SerializeField] float rotationSpeed = 5f;
        [SerializeField] float fixedSpeedRotateRatio = 60f;
        [SerializeField] bool followCamera, fixedSpeedRotation;

        Rigidbody rb;

        Transform cameraTransform;

        Unit unit;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            unit = GetComponent<Unit>();

            cameraTransform = Camera.main.transform;
        }
        public void HandleAllMovement()
        {
            HandleMovement();
            HandleRotation();
        }

        void HandleMovement()
        {
            Vector3 moveDirection = CalculateMoveDirection();
            if (moveDirection != Vector3.zero)
            {
                Vector3 position = transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(position);
            }
        }
        void HandleRotation()
        {
            Quaternion rotationDirection = CalculateRotationDirection();

            Quaternion rotation = fixedSpeedRotation ?
                Quaternion.Slerp(transform.rotation, rotationDirection, rotationSpeed * Time.fixedDeltaTime) :
                Quaternion.RotateTowards(transform.rotation, rotationDirection, rotationSpeed * fixedSpeedRotateRatio * Time.fixedDeltaTime);

            rb.MoveRotation(rotation);
        }

        Vector3 CalculateMoveDirection()
        {
            Vector3 moveDirection = Vector3.zero;

            float horizontal = unit.InputManager.Horizontal;
            float vertical = unit.InputManager.Vertical;

            moveDirection += Vector3.ProjectOnPlane(cameraTransform.right, transform.up).normalized * horizontal;
            moveDirection += Vector3.ProjectOnPlane(cameraTransform.forward, transform.up).normalized * vertical;

            return moveDirection;
        }
        Quaternion CalculateRotationDirection()
            => followCamera ? CalculateRotationDirectionFromCamera() : CalculateRotationDirectionFromMovement();

        Quaternion CalculateRotationDirectionFromMovement()
            => CalculateMoveDirection() == Vector3.zero ? transform.rotation : Quaternion.LookRotation(CalculateMoveDirection());

        Quaternion CalculateRotationDirectionFromCamera()
            => Quaternion.Euler(new(0, cameraTransform.eulerAngles.y, 0));
    }
}