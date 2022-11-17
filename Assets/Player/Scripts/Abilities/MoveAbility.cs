using UnityEngine;

public class MoveAbility : BasePlayerAbility
{
    [Header("Movement Settings")]
    [SerializeField] private float speed;

    private Vector2 movement;

    protected override void SetAbilityType() => abilityType = PlayerAbilitys.Move;

    public override void Setup()
    {
        abilityManager.controls.Default.Movement.performed += movemt_ctx => SetMovement(movemt_ctx.ReadValue<Vector2>());
        abilityManager.controls.Default.Movement.canceled += _ => SetMovement(Vector2.zero);
    }

    public override void Unset()
    {
        abilityManager.controls.Default.Movement.performed -= movemt_ctx => SetMovement(movemt_ctx.ReadValue<Vector2>());
        abilityManager.controls.Default.Movement.canceled -= _ => SetMovement(Vector2.zero);
    }

    public void SetMovement(Vector2 newMovement)
    {
        movement = newMovement;
        //anim.SetBool("isMoving", movement != 0);
        //effects.FlipSprite(movement);
    }

    private void FixedUpdate()
    {
        rb.velocity = speed * Time.deltaTime * movement.normalized;
    }
}

