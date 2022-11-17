using UnityEngine;
using UnityEngine.InputSystem;

public class MoveAbility : BasePlayerAbility
{
    [Header("Movement Settings")]
    [SerializeField] private float speed;

    private Vector2 movement;

    protected override void SetAbilityType() => abilityType = PlayerAbilitys.Move;

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
    }

    public void ResetMovement(InputAction.CallbackContext context)
    {
        movement = Vector2.zero;
    }

    private void FixedUpdate()
    {
        rb.velocity = speed * Time.deltaTime * movement.normalized;
    }
}

