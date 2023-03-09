using UnityEngine;

namespace Synith
{
    [RequireComponent(typeof(Rigidbody))]
    public class UnitMovement : MonoBehaviour
    {
        #region Inspector Fields
        [SerializeField] float moveSpeed = 4f;
        [SerializeField] float rotationSpeed = 5f;
        [SerializeField] float fixedSpeedRotateRatio = 60f;
        [SerializeField] float movementForce = 10f;

        [SerializeField, Tooltip("Rotation follows camera instead of movement")]
        bool followCamera;
        [SerializeField, Tooltip("Rotate at a constant angular speed, uncheck for snappier rotations")]
        bool constantRotationRate;
        [SerializeField, Tooltip("Rotations are not effected by physics system")]
        bool kinematicRotation;
        [SerializeField, Tooltip("Movements are not effected by physics system")]
        bool kinematicMovement;

        [SerializeField, Tooltip("Camera for this unit")]
        Transform cameraTransform;
        #endregion

        Rigidbody rb;
        Unit unit;

        Vector3 moveDirection;



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

        public bool IsMoving() => moveDirection != Vector3.zero;

        void HandleMovement()
        {            
            Vector3 moveDirection = CalculateMoveDirection();
            this.moveDirection = moveDirection;

            if (IsMoving())
            {
                Vector3 velocity = moveDirection * moveSpeed * Time.fixedDeltaTime;

                if (kinematicMovement)
                    rb.MovePosition(transform.position + velocity);
                else
                    rb.AddForce(velocity * movementForce, ForceMode.VelocityChange);
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
            => IsMoving() ? Quaternion.LookRotation(CalculateMoveDirection()) : transform.rotation;

        Quaternion CalculateRotationDirectionFromCamera()
            => Quaternion.Euler(new(0, cameraTransform.eulerAngles.y, 0));
    }
}