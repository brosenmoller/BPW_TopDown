using UnityEngine;
using UnityEngine.InputSystem;

public class MoveAbility : BasePlayerAbility
{
    [Header("Movement Settings")]
    [SerializeField] private float speed;

    private Vector2 movement;

    public override void Setup()
    {
        abilityManager.controls.Default.Movement.performed += SetMovement;
        abilityManager.controls.Default.Movement.canceled += ResetMovement;
    }

    public override void Unset()
    {
        abilityManager.controls.Default.Movement.performed -= SetMovement;
        abilityManager.controls.Default.Movement.canceled -= ResetMovement;
    }

    public void SetMovement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
        animator.SetFloat("XMove", movement.x);
        animator.SetFloat("YMove", movement.y);
        animator.SetBool("IsWalking", true);
    }

    public void ResetMovement(InputAction.CallbackContext context)
    {
        animator.SetBool("IsWalking", false);
        movement = Vector2.zero;
    }

    private void FixedUpdate()
    {
        rigidBody2D.velocity = speed * Time.deltaTime * movement.normalized;
    }
}

