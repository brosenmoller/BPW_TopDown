using UnityEngine;

public abstract class BasePlayerAbility : MonoBehaviour
{
    [HideInInspector] public PlayerAbilitys abilityType;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer spriteHolder;
    protected PlayerAbilityManager abilityManager;

    private void Awake()
    {
        SetAbilityType();
    }

    protected virtual void Start()
    {
        abilityManager = GetComponent<PlayerAbilityManager>();
        if (!abilityManager.accesableAbilitys.Contains(abilityType))
        {
            enabled = false;
            return;
        }
        
        Setup();

        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        spriteHolder = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    protected abstract void SetAbilityType();
    public abstract void Setup();
    public abstract void Unset();
}