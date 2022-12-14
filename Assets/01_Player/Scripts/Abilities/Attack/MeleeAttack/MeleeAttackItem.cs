using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeAttackItem", menuName = "AttackItems")]
public class MeleeAttackItem : BaseAttackItem
{
    [Header("Melee Settings")]
    public float hitOffset;
    public float attackDuration;
}
