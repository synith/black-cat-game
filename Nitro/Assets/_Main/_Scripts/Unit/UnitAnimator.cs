using UnityEngine;

namespace Synith
{
    /// <summary>
    /// UnitAnimator
    /// </summary>
    public class UnitAnimator : MonoBehaviour
    {
        [SerializeField] Unit unit;

        Animator animator;

        const string IS_MOVING = "IsMoving";

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            animator.SetBool(IS_MOVING, unit.UnitMovement.IsMoving());
        }
    }
}
