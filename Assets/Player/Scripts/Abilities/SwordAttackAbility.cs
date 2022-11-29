using UnityEngine;

public class SwordAttackAbility : BaseAttackAbility
{
    [Header("Movement Settings")]
    [SerializeField] private Vector2 hitSize;

    protected override void PerformAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(weaponHolder.transform.GetChild(0).position, hitSize, 0);
        foreach (Collider2D collider in colliders)
        {
            collider.TryGetComponent(out IAttackInteractable attackInteractable);
            attackInteractable?.Interaction(attackDirection, damage, force);

            // Own Knockback
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(weaponHolder.transform.GetChild(0).position, hitSize);
    }
}

