using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerAbility : MonoBehaviour
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
        
        Initialize();

        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        spriteHolder = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }



    protected abstract void SetAbilityType();
    public abstract void Initialize();
}

public enum PlayerAbilitys
{
    Move,
    Shoot
}
