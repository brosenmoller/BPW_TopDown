using UnityEngine;
using UnityEngine.InputSystem;

public class SwordAttackAbility : BaseAttackAbility
{
    [Header("Movement Settings")]
    [SerializeField] private float cooldown;

    private float cooldownTimer = 0;

    protected override void PerformAttack(InputAction.CallbackContext context)
    {
        if (attackDirection == null || attackDirection == Vector2.zero) { return; }
        
        if (Time.time < cooldownTimer) { return; }
        cooldownTimer = Time.time + cooldown;

        Debug.Log("PerformSwordAttack");
    }
}

