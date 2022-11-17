using UnityEngine;

public class SwordAttackAbility : BaseAttackAbility
{
    [Header("Movement Settings")]
    [SerializeField] private float cooldown;

    private float cooldownTimer = 0;

    protected override void SetAbilityType() => abilityType = PlayerAbilitys.ShootAttack;

    protected override void PerformAttack()
    {
        if (attackDirection == null || attackDirection == Vector2.zero) { return; }
        
        if (Time.time < cooldownTimer) { return; }
        cooldownTimer = Time.time + cooldown;

        Debug.Log("SwordAttack");
    }
}

