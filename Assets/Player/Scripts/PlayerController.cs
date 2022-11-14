using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Controls controls;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public PlayerAbilityManager abilityManager;

    private void OnEnable()
    {
        controls ??= new Controls();
        controls.Default.Enable();
    }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        abilityManager = GetComponent<PlayerAbilityManager>();

        //playerEffects.OnChangePlayerEffects(transform);
    }

    private void OnDisable()
    {
        controls.Default.Disable();
    }
}
