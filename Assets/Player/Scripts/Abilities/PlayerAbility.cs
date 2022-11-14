using UnityEngine;

public abstract class PlayerAbility : MonoBehaviour
{
    [HideInInspector] public PlayerAbilitys abilityType;

    protected PlayerController player;
    protected Rigidbody2D rb;
    protected Animator anim;

    // Binding references
    private void Awake()
    {
        player = GetComponent<PlayerController>();
        SetAbilityType();
    }

    // Check on load if the player has access to a certain ability and 
    // Then enable and initialize the component if necessary
    protected virtual void Start()
    {
        if (!player.abilityManager.accesableAbilitys.Contains(abilityType))
        {
            enabled = false;
        }

        rb = player.rb;
        anim = player.anim;

        if (enabled) Initialize();
    }
    protected abstract void SetAbilityType();
    public abstract void Initialize();
}

public enum PlayerAbilitys
{
    Move
}
