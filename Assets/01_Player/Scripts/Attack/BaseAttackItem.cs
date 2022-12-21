using UnityEngine;
using UnityEditor.Animations;

public abstract class BaseAttackItem : ScriptableObject
{
    [Header("Base Attack Settings")]
    [SerializeField] protected int damage;
    [SerializeField] protected float force;

    [Header("Base Attack Looks")]
    public AnimatorController weaponAnimatorController;

    protected PlayerAttackManager attackManager;

    public void Setup(PlayerAttackManager attackManager)
    {
        this.attackManager = attackManager;
        SetupSpecificItem();
    }
    protected virtual void SetupSpecificItem() { }

    public virtual void OnUpdate() { }

    public virtual void OnDrawGizmosSelected() { }

    public abstract void PerformAttack();
}

