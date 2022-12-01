using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseAttackAbility : BasePlayerAbility
{
    [Header("Base Attack Settings")]
    [SerializeField] protected Transform weaponHolder;
    [SerializeField] private float cooldown;
    [SerializeField] protected int damage;
    [SerializeField] protected float force;

    [Header("Base Attack Looks")]
    [SerializeField] protected AnimatorController weaponAnimatorController;

    private float cooldownTimer = 0;

    private Camera mainCamera;
    private Animator weaponAnimator;

    private Vector2 previousVector2Input;
    private Quaternion targetRotation;

    protected Vector2 attackDirection;

    protected override bool IsAllowedToSetup()
    {
        if (abilityManager.activeAttackAbility != this) { return false; }
        else { return true; }
    }

    public override void Setup()
    {
        mainCamera = Camera.main;
        weaponAnimator = GetComponentInChildren<Animator>();
        weaponAnimator.runtimeAnimatorController = weaponAnimatorController;

        abilityManager.controls.Default.Attack.performed += CheckForAttack;
        abilityManager.controls.Default.Aiming.performed += SetAttackDirection;
    }

    public override void Unset()
    {
        abilityManager.controls.Default.Attack.performed -= CheckForAttack;
        abilityManager.controls.Default.Aiming.performed -= SetAttackDirection;
    }

    private void CheckForAttack(InputAction.CallbackContext context)
    {
        if (attackDirection == null || attackDirection == Vector2.zero) { return; }

        if (Time.time < cooldownTimer) { return; }
        cooldownTimer = Time.time + cooldown;

        weaponAnimator.SetTrigger("AttackTrigger");

        PerformAttack();
    }

    protected abstract void PerformAttack();

    private void FixedUpdate()
    {
        if (abilityManager.controls.Default.Movement.IsPressed())
        {
            if (!abilityManager.usingGamepad)
            {
                SetAttackDirectionByMousePosition(previousVector2Input);
                LookTowardsAttackDirection();
            }
        }

        if (weaponHolder.rotation != targetRotation)
        {
            weaponHolder.rotation = targetRotation;
        }
    }

    private void SetAttackDirection(InputAction.CallbackContext context)
    {
        Vector2 vector2Input = context.ReadValue<Vector2>();
        if (previousVector2Input == vector2Input) { return; }
        
        previousVector2Input = vector2Input;

        if (abilityManager.usingGamepad)
        {
            if (vector2Input != Vector2.zero) 
            {
                attackDirection = vector2Input.normalized; 
            }
        }
        else
        {
            SetAttackDirectionByMousePosition(vector2Input);
        }

        LookTowardsAttackDirection();
    }

    private void SetAttackDirectionByMousePosition(Vector2 mouseScreenPosition)
    {
        Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        attackDirection = (mouseWorldPosition - (Vector2)transform.position).normalized;
    }

    private void LookTowardsAttackDirection()
    {
        float angle = Vector2.SignedAngle(Vector2.right, attackDirection);
        targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
}

