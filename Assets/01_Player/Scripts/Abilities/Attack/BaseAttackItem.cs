using UnityEngine;
using UnityEditor.Animations;

public class BaseAttackItem : ScriptableObject
{
    [Header("General Attack Settings")]
    public int damage;
    public float force;
    public float cooldown;
    public float hitRadius;
    public LayerMask attackLayerMask;
    public AnimatorController weaponAnimatorController;
}
