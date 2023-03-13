using UnityEngine;

namespace Synith
{
    [RequireComponent(typeof(Rigidbody))]
    public class UnitMovement : MonoBehaviour
    {
        #region Inspector Fields
        [SerializeField] float moveSpeed = 3f;
        [SerializeField] float rotationSpeed = 3f;
        [SerializeField] float movementForce = 30f;
        [SerializeField] float dragFactor = 1f;

        [SerializeField, Tooltip("Rotation follows camera instead of movement")]
        bool followCamera;
        [SerializeField, Tooltip("Rotate at a constant angular speed, uncheck for snappier rotations")]
        bool constantRotationRate;

        [SerializeField] LayerMask groundLayer;

        [SerializeField]
        bool isGrounded;
        [SerializeField, Range(0.05f, 1f)]
        float groundCheckRadius = 0.2f;


        [SerializeField, Tooltip("Camera for this unit")]
        Transform cameraTransform;
        #endregion

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

        private void OnDrawGizmos()
        {
            Gizmos.color = new(1, 1, 1, 0.2f);
            Gizmos.DrawSphere(transform.position, groundCheckRadius);
        }

        public void HandleAllMovement()
        {
            HandleGroundCheck();
            HandleMovement();
            HandleRotation();
            HandleDrag();
        }

        void HandleGroundCheck()
        {
            float groundCheckRadius = this.groundCheckRadius;
            isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayer);
        }

        void HandleDrag()
        {
            Vector3 dragVelocity = -rb.velocity;
            float currentDragFactor = isGrounded ? dragFactor : 0f;

            rb.AddForce(dragVelocity * currentDragFactor);
        }

        public bool IsMoving() => CalculateMoveDirection() != Vector3.zero;

        void HandleMovement()
        {
            Vector3 moveDirection = CalculateMoveDirection();

            if (moveDirection != Vector3.zero)
            {
                if (!isGrounded) return;

                Vector3 velocity = moveDirection * moveSpeed * Time.fixedDeltaTime;
                rb.AddForce(velocity * movementForce, ForceMode.VelocityChange);
            }
        }

        void HandleRotation()
        {
            Quaternion rotationDirection = CalculateRotationDirection();

            float constantRotationRateRatio = 72f;

            Quaternion rotation = constantRotationRate ?
                Quaternion.RotateTowards(transform.rotation, rotationDirection, rotationSpeed * constantRotationRateRatio * Time.fixedDeltaTime) :
                Quaternion.Slerp(transform.rotation, rotationDirection, rotationSpeed * Time.fixedDeltaTime);


            rb.MoveRotation(rotation);
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
            => IsMoving() ? Quaternion.LookRotation(CalculateMoveDirection()) : transform.rotation;

        Quaternion CalculateRotationDirectionFromCamera()
            => Quaternion.Euler(new(0, cameraTransform.eulerAngles.y, 0));
    }
}