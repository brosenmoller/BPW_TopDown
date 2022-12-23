using UnityEngine;

public abstract class BaseAttackItem : ScriptableObject
{
    [Header("Base Attack Data")]
    [SerializeField] protected int damage;
    [SerializeField] protected float force;
    [SerializeField] protected AudioObject attackSound;
    public RuntimeAnimatorController weaponAnimatorController;

    protected PlayerAttackManager attackManager;

    public void Setup(PlayerAttackManager attackManager)
    {
        this.attackManager = attackManager;
        SetupSpecificItem();
    }
    protected virtual void SetupSpecificItem() { }
    public virtual void OnUpdate() { }
    public virtual void OnDrawGizmosSelected() { }
    public virtual void OnAttackEnd() { }
    public abstract void PerformAttack();

}

