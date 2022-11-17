using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseAttackAbility : BasePlayerAbility
{
    [Header("Base Attack Settings")]
    [SerializeField] private Transform weaponHolder;

    private Camera mainCamera;

    private Vector2 previousVector2Input;
    private Quaternion targetRotation;

    protected Vector2 attackDirection;

    protected override void Start()
    {
        base.Start();

        if (abilityManager.activeAttackAbility != abilityType) 
        {
            Unset();
            enabled = false;
        }
    }

    public override void Setup()
    {
        mainCamera = Camera.main;
        weaponHolder = transform.GetChild(1);

        abilityManager.controls.Default.Attack.performed += PerformAttack;
        abilityManager.controls.Default.Aiming.performed += SetAttackDirection;
    }

    public override void Unset()
    {
        abilityManager.controls.Default.Attack.performed -= PerformAttack;
        abilityManager.controls.Default.Aiming.performed -= SetAttackDirection;
    }

    protected abstract void PerformAttack(InputAction.CallbackContext context);

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
                attackDirection = vector2Input; 
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

