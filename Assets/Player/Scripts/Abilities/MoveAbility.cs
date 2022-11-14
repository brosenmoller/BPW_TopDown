using UnityEngine;

public class MoveAbility : PlayerAbility
{
    [Header("Movement Settings")]
    [SerializeField] float speed;

    private Vector2 movement;

    protected override void SetAbilityType() => abilityType = PlayerAbilitys.Move;

    public override void Initialize()
    {
        //Set Up Controls
        player.controls.Default.Horizontal.performed += movemt_ctx => SetMovementHorizontal((int)movemt_ctx.ReadValue<float>());
        player.controls.Default.Horizontal.canceled += _ => SetMovementHorizontal(0);
        player.controls.Default.Vertical.performed += movemt_ctx => SetMovementVertical((int)movemt_ctx.ReadValue<float>());
        player.controls.Default.Vertical.canceled += _ => SetMovementVertical(0);
    }

    public void SetMovementHorizontal(int newMovement)
    {
        movement.x = newMovement;
        //anim.SetBool("isMoving", movement != 0);
        //effects.FlipSprite(movement);
    }
    
    public void SetMovementVertical(int newMovement)
    {
        movement.y = newMovement;
        //anim.SetBool("isMoving", movement != 0);
        //effects.FlipSprite(movement);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        rb.velocity = speed * Time.deltaTime * movement.normalized;
    }
}

