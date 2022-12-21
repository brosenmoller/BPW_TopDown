using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerAttackManager : MonoBehaviour
{
    [Header("General Attack Settings")]
    public Transform weaponHolder;
    public Animator weaponAnimator;

    [Header("Attack Items")]
    [SerializeField] private List<BaseAttackItem> accesableAttackItems = new();

    private BaseAttackItem currentAttackItem;

    private PlayerAbilityManager abilityManager;
    private Camera mainCamera;

    [HideInInspector] public Vector2 attackDirection = Vector2.zero;
    private Vector2 previousVector2Input;
    private Quaternion targetRotation;

    private void Awake()
    {
        abilityManager = GetComponent<PlayerAbilityManager>();
        mainCamera = Camera.main;

        foreach (BaseAttackItem item in accesableAttackItems)
        {
            item.Setup(this);
        }

        currentAttackItem = accesableAttackItems[0];
        weaponAnimator.runtimeAnimatorController = currentAttackItem.weaponAnimatorController;
    }

    private void Start()
    {
        abilityManager.controls.Default.SwitchAttack.performed += SwitchAttack;

        abilityManager.controls.Default.Attack.performed += CheckForAttack;
        abilityManager.controls.Default.Aiming.performed += SetAttackDirection;
    }

    private void OnDisable()
    {
        abilityManager.controls.Default.SwitchAttack.performed -= SwitchAttack;

        abilityManager.controls.Default.Attack.performed -= CheckForAttack;
        abilityManager.controls.Default.Aiming.performed -= SetAttackDirection;
    }

    private void SwitchAttack(InputAction.CallbackContext context)
    {
        currentAttackItem = accesableAttackItems[(int)context.ReadValue<float>()];
        weaponAnimator.runtimeAnimatorController = currentAttackItem.weaponAnimatorController;
    }

    private void CheckForAttack(InputAction.CallbackContext context)
    {
        if (attackDirection == Vector2.zero || currentAttackItem == null) { return; }

        currentAttackItem.PerformAttack();
    }

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

        currentAttackItem.OnUpdate();

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

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) { return; }

        currentAttackItem.OnDrawGizmosSelected();
    }
}
