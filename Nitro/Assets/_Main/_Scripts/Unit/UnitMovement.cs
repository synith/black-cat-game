using System;
using UnityEngine;

namespace Synith
{
    [RequireComponent(typeof(Rigidbody))]
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] float currentDrag;

        #region Inspector Fields
        [SerializeField] float moveSpeed = 3f;
        [SerializeField] float rotationSpeed = 3f;
        [SerializeField] float movementForce = 30f;
        [SerializeField] float jumpForce = 30f;

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

        public event Action OnUnitAboutToJump;

        public bool IsMoving() => CalculateMoveDirection() != Vector3.zero;

        public void HandleAllMovement()
        {
            HandleGroundCheck();
            HandleMovement();
            HandleGravity();
            HandleRotation();
            HandleDrag();
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            unit = GetComponent<Unit>();

            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        void Start()
        {
            unit.UnitInput.OnJumpPressed += () => Jump();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new(1, 1, 1, 0.2f);
            Gizmos.DrawSphere(transform.position, groundCheckRadius);
        }

        void HandleGroundCheck()
        {
            float groundCheckRadius = this.groundCheckRadius;
            isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayer);
        }

        void HandleDrag()
        {
            Vector3 dragVelocity = -rb.velocity;

            float groundDrag = 10f;
            float airDrag = 1f;
            float currentDragFactor = isGrounded ? groundDrag : airDrag;

            currentDrag = currentDragFactor; // Just to show in inspector

            rb.AddForce(dragVelocity * currentDragFactor);
        }

        void HandleMovement()
        {
            Vector3 moveDirection = CalculateMoveDirection();

            if (moveDirection != Vector3.zero)
            {
                float walkingSpeed = 1f;
                float airSpeed = 0.25f;

                float speed = isGrounded ? walkingSpeed : airSpeed;
                speed *= moveSpeed;

                Vector3 velocity = moveDirection * speed * Time.fixedDeltaTime;
                rb.AddForce(velocity * movementForce, ForceMode.VelocityChange);
            }
        }

        void HandleGravity()
        {
            Vector3 gravityVelocity = Physics.gravity;
            float groundGravityModifier = 1f;
            float airGravityModifier = 2f;

            float gravityModifier = isGrounded ? groundGravityModifier : airGravityModifier;
            rb.AddForce(gravityVelocity * gravityModifier);
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

        void Jump()
        {
            if (!isGrounded) return;

            OnUnitAboutToJump?.Invoke();

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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