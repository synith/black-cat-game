using System.Collections;
using UnityEngine;

namespace Synith
{
    /// <summary>
    /// UnitFloatingCapsule
    /// </summary>
    /// 
    [RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody), typeof(Unit))]
    public class UnitFloatingCapsule : MonoBehaviour
    {
        CapsuleCollider capsuleCollider;
        Rigidbody rb;
        Unit unit;

        [SerializeField] float rayLength = 2f;
        [SerializeField] float rideHeight = 1.5f;
        [SerializeField] float rideSpringStrength = 10f;
        [SerializeField] float rideSpringDamper = 5f;
        [SerializeField] float disabledTime = 0.5f;

        bool isDisabled;

        IEnumerator DisableTemporarily(float seconds)
        {
            isDisabled = true;
            yield return new WaitForSeconds(seconds);
            isDisabled = false;
        }

        void Awake()
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();
            unit = GetComponent<Unit>();
        }

        void Start()
        {
            unit.UnitMovement.OnUnitAboutToJump += UnitMovement_OnUnitAboutToJump;
        }

        void UnitMovement_OnUnitAboutToJump()
        {
            if (isDisabled) return;

            StartCoroutine(DisableTemporarily(disabledTime));
        }

        void FixedUpdate()
        {
            if (isDisabled) return;

            Vector3 bottomOfCollider = transform.position + Vector3.up * (capsuleCollider.center.y - capsuleCollider.radius);
            Vector3 downDirection = Vector3.down;


            Ray ray = new(bottomOfCollider, downDirection);

            bool rayDidHit = Physics.Raycast(ray, out RaycastHit rayHit, rayLength);

            if (rayDidHit)
            {
                Vector3 velocity = rb.velocity;
                Vector3 rayDirection = transform.TransformDirection(downDirection);

                Vector3 otherVelocity = Vector3.zero;
                Rigidbody hitBody = rayHit.rigidbody;

                if (hitBody != null)
                {
                    otherVelocity = hitBody.velocity;
                }

                float rayDirectionVelocity = Vector3.Dot(rayDirection, velocity);
                float otherDirectionVelocity = Vector3.Dot(rayDirection, otherVelocity);

                float relativeVelocity = rayDirectionVelocity - otherDirectionVelocity;
                float heightOffset = rayHit.distance - rideHeight;

                float springForce = (heightOffset * rideSpringStrength) - (relativeVelocity * rideSpringDamper);

                rb.AddForce(rayDirection * springForce);
            }
        }
    }
}
