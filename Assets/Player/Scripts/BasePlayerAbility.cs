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
        if (!abilityManager.accesableAbilitys.Contains(GetType()))
        {
            enabled = false;
            return;
        }

        Setup();

        rb = abilityManager.rb;
        //anim = GetComponent<Animator>();
        spriteHolder = abilityManager.spriteHolder;

        Initialize();
    }

    protected virtual void Initialize() { }

    public abstract void Setup();
    public abstract void Unset();
}
