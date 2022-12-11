using UnityEngine;

public abstract class BasePlayerAbility : MonoBehaviour
{
    protected Rigidbody2D rigidBody2D;
    protected Animator animator;
    protected SpriteRenderer spriteHolder;
    protected PlayerAbilityManager abilityManager;

    public bool isMovementAbility;

    private void Start()
    {
        abilityManager = GetComponent<PlayerAbilityManager>();
        if (!abilityManager.accesableAbilitys.Contains(GetType()) || !IsAllowedToSetup())
        {
            enabled = false;
            return;
        }
        
        Setup();

        rigidBody2D = abilityManager.rigidBody2D;
        animator = abilityManager.animator;
        spriteHolder = abilityManager.spriteHolder;
    }

    protected virtual bool IsAllowedToSetup() { return true; }

    public abstract void Setup();
    public abstract void Unset();
}
