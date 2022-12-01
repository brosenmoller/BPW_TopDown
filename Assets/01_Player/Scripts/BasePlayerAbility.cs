using UnityEngine;

public abstract class BasePlayerAbility : MonoBehaviour
{
    protected Rigidbody2D rb;
    //protected Animator anim;
    protected SpriteRenderer spriteHolder;
    protected PlayerAbilityManager abilityManager;

    private void Start()
    {
        abilityManager = GetComponent<PlayerAbilityManager>();
        if (!abilityManager.accesableAbilitys.Contains(GetType()) || !IsAllowedToSetup())
        {
            enabled = false;
            return;
        }
        
        Setup();

        rb = abilityManager.rb;
        //anim = GetComponent<Animator>();
        spriteHolder = abilityManager.spriteHolder;
    }

    protected virtual bool IsAllowedToSetup() { return true; }

    public abstract void Setup();
    public abstract void Unset();
}
