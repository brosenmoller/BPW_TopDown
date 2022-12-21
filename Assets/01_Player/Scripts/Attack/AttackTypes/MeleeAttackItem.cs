using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeAttackItem", menuName = "AttackItems/MeleeAttack", order = 0)]
public class MeleeAttackItem : BaseAttackItem
{
    [Header("Melee Data")]
    [SerializeField] private float cooldownTime;
    [SerializeField] private float hitRadius;
    [SerializeField] private float hitOffset;
    [SerializeField] private float attackDuration;
    [SerializeField] protected LayerMask attackLayerMask;

    private Timer cooldownTimer;
    private bool canAttack;

    protected override void SetupSpecificItem()
    {
        cooldownTimer = new Timer(cooldownTime, () => { canAttack = true; });
    }

    public override void PerformAttack()
    {
        if (!canAttack) { return; }

        attackManager.weaponAnimator.SetTrigger("AttackTrigger");
        attackSound.Play();

        attackManager.StartCoroutine(CheckForAttackInteractableInRange());

        canAttack = false;
        cooldownTimer.Reset();
    }

    private Vector3 GetCircleOrigin()
    {
        return (Vector2)attackManager.transform.position + hitOffset * attackManager.attackDirection;
    }

    public IEnumerator CheckForAttackInteractableInRange()
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
                    attackInteractable?.OnAttackInteract(attackManager.attackDirection, damage, force);

                    // Own Knockback
                }
            }
            

            yield return null;
        }
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetCircleOrigin(), hitRadius);
    }
}

