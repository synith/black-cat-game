using UnityEngine;

namespace Synith
{
    [RequireComponent(typeof(Rigidbody))]
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 4f;
        [SerializeField] float rotationSpeed = 5f;
        [SerializeField] float fixedSpeedRotateRatio = 60f;
        [SerializeField] float movementForce = 300f;

        [SerializeField] bool followCamera;
        [SerializeField, Tooltip("Rotate at a constant angular speed, uncheck for snappier rotations")]
        bool constantRotationRate;
        [SerializeField, Tooltip("Rotations are not effected by physics system")]
        bool kinematicRotation;

        [SerializeField, Tooltip("Camera for this unit")] Transform cameraTransform;


        Rigidbody rb;
        Unit unit;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            unit = GetComponent<Unit>();

            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
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

            Quaternion rotation = constantRotationRate ?
                Quaternion.RotateTowards(transform.rotation, rotationDirection, rotationSpeed * fixedSpeedRotateRatio * Time.fixedDeltaTime) :
                Quaternion.Slerp(transform.rotation, rotationDirection, rotationSpeed * Time.fixedDeltaTime);

            if (kinematicRotation)
            {
                transform.rotation = rotation;
            }
            else
            {
                rb.MoveRotation(rotation);
            }
        }

        Vector3 CalculateMoveDirection()
        {
            Vector3 moveDirection = Vector3.zero;

            float horizontal = unit.UnitInput.Horizontal;
            float vertical = unit.UnitInput.Vertical;

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