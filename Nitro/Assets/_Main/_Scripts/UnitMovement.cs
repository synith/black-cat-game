using UnityEngine;

namespace Synith
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class UnitMovement : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed = 1f;
        [SerializeField] float rotationSpeed = 360f;
        [SerializeField] protected int maxHealth = 100;
        [SerializeField] protected Animator animator;


        Rigidbody rb;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            HandleMovement();

            if (animator != null)
                HandleAnimation();
        }

        void HandleAnimation()
        {
            if (CalculateMoveDirection() != Vector3.zero)
            {
                animator.SetBool("IsWalking", true);
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }
        }

        void HandleMovement()
        {
            // remove collision momentum
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Vector3 moveDirection = CalculateMoveDirection();
            Vector3 rotationDirection = CalculateRotationDirection();

            if (moveDirection != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(rotationDirection);
                Vector3 moveVector = transform.position + moveDirection * moveSpeed;
                Quaternion rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.fixedDeltaTime);

                rb.Move(Vector3.Lerp(transform.position, moveVector, Time.fixedDeltaTime), rotation);
            }
        }

        protected abstract Vector3 CalculateMoveDirection();
        protected abstract Vector3 CalculateRotationDirection();
    }
}