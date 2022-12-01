using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackAbility : BaseAttackAbility
{
    [Header("Movement Settings")]
    [SerializeField] private float hitRadius;
    [SerializeField] private float hitOffset;
    [SerializeField] private float attackDuration;
    [SerializeField] protected LayerMask attackLayerMask;

    protected override void PerformAttack()
    {
        StartCoroutine(CheckForAttackInteractableInRange());
    }

    private Vector3 GetCircleOrigin()
    {
        Vector3 directionVector = transform.position - weaponHolder.transform.GetChild(0).position;
        directionVector *= hitOffset * -1f;
        return transform.position + directionVector;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(GetCircleOrigin(), hitRadius);
    }

    private IEnumerator CheckForAttackInteractableInRange()
    {
        Collider2D[] colliders;
        HashSet<Collider2D> alreadyHit = new();

        float timer = Time.time + attackDuration;

        while (Time.time < timer)
        {
            colliders = Physics2D.OverlapCircleAll(GetCircleOrigin(), hitRadius, attackLayerMask);
            
            if (colliders.Length > 0)
            {
                foreach (Collider2D collider in colliders)
                {
                    if (alreadyHit.Contains(collider)) { break; }
                    alreadyHit.Add(collider);

                    collider.TryGetComponent(out IAttackInteractable attackInteractable);
                    attackInteractable?.OnAttackInteract(attackDirection, damage, force);

                    // Own Knockback
                }
            }
            

            yield return null;
        }
    }
}

