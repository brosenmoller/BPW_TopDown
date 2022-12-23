using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private bool isPerformingAttack = false;

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

        abilityManager.controls.Default.Attack.started += StartAttack;
        abilityManager.controls.Default.Attack.canceled += EndAttack;

        abilityManager.controls.Default.MouseAiming.performed += SetAttackDirectionMouse;
        abilityManager.controls.Default.GamepadAiming.performed += SetAttackDirectionGamePad;
    }

    private void OnDisable()
    {
        abilityManager.controls.Default.SwitchAttack.performed -= SwitchAttack;

        abilityManager.controls.Default.Attack.started -= StartAttack;
        abilityManager.controls.Default.Attack.canceled -= EndAttack;

        abilityManager.controls.Default.MouseAiming.performed -= SetAttackDirectionMouse;
        abilityManager.controls.Default.GamepadAiming.performed -= SetAttackDirectionGamePad;
    }

    private void SwitchAttack(InputAction.CallbackContext context)
    {
        int currentAttackItemIndex = accesableAttackItems.IndexOf(currentAttackItem);
        currentAttackItemIndex += (int)context.ReadValue<float>();
        
        if (currentAttackItemIndex < 0) { currentAttackItemIndex = accesableAttackItems.Count - 1; }
        else if (currentAttackItemIndex > accesableAttackItems.Count - 1) { currentAttackItemIndex = 0; }

        currentAttackItem = accesableAttackItems[currentAttackItemIndex];
        weaponAnimator.runtimeAnimatorController = currentAttackItem.weaponAnimatorController;
    }

    private void StartAttack(InputAction.CallbackContext context) => isPerformingAttack = true;
    private void EndAttack(InputAction.CallbackContext context)
    {
        isPerformingAttack = false;
        currentAttackItem.OnAttackEnd();
    }

    private void PerformCurrentAttack()
    {
        if (attackDirection == Vector2.zero || currentAttackItem == null) { return; }

        currentAttackItem.PerformAttack();
    }

    private void SetAttackDirectionMouse(InputAction.CallbackContext context) => SetAttackDirection(false, context.ReadValue<Vector2>());

    private void SetAttackDirectionGamePad(InputAction.CallbackContext context) => SetAttackDirection(true, context.ReadValue<Vector2>());

    private void SetAttackDirection(bool isGameplad, Vector2 vector2Input)
    {
        if (isGameplad != abilityManager.usingGamepad) { return; }

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

        if (isPerformingAttack) { PerformCurrentAttack(); }
        currentAttackItem.OnUpdate();

        if (weaponHolder.rotation != targetRotation)
        {
            weaponHolder.rotation = targetRotation;
        }
    }

    public void GiveAttackItem(BaseAttackItem attackItem)
    {
        if (!accesableAttackItems.Contains(attackItem))
        {
            attackItem.Setup(this);
            accesableAttackItems.Add(attackItem);
            currentAttackItem.OnAttackEnd();
            currentAttackItem = attackItem;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) { return; }

        currentAttackItem.OnDrawGizmosSelected();
    }
}
