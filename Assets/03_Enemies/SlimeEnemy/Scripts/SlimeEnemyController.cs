using UnityEngine;
using UnityEngine.AI;

public class SlimeEnemyController : MonoBehaviour, IAttackInteractable
{
    [Header("Movement Settings")]
    public Transform target;
    [SerializeField] private float speed;
    public float updateDelay;
    public float wanderDistance;
    public Vector2 wanderDelay;
    public float detectionRange;

    [Header("Combat")]
    [SerializeField] private int startHealth;
    [SerializeField] private float getHitKnockbackMultiplier;
    [SerializeField] private float stunTime;
    [SerializeField] private int damage;
    [SerializeField] private float force;

    [Header("Effects")]
    [SerializeField] private Material hitFlashMat;
    [SerializeField] private AudioObject hitAudio;

    private Material normalMat;

    private NavMeshAgent agent;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteHolder;

    private int health;

    private StateMachine<SlimeEnemyController> stateMachine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        rigidBody = GetComponent<Rigidbody2D>();

        spriteHolder = GetComponent<SpriteRenderer>();
        normalMat = spriteHolder.material;

        health = startHealth;
    }

    private void Start()
    {
        SetupStateMachine();
    }

    private void SetupStateMachine()
    {
        State<SlimeEnemyController>[] states = new State<SlimeEnemyController>[] {
            new SlimeEnemyWanderState(),
            new SlimeEnemyChaseState(),
        };

        stateMachine = new StateMachine<SlimeEnemyController>(this, states);
        stateMachine.AddTransition(new Transition(typeof(SlimeEnemyWanderState), typeof(SlimeEnemyChaseState), IsPlayerInRange));
        stateMachine.AddTransition(new Transition(typeof(SlimeEnemyChaseState), typeof(SlimeEnemyWanderState), () => !IsPlayerInRange()));
        stateMachine.ChangeState(typeof(SlimeEnemyWanderState));
    }

    private void FixedUpdate()
    {
        stateMachine.OnFixedUpdate();
    }

    private bool IsPlayerInRange()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(
            transform.position,
            (target.position - transform.position).normalized,
            detectionRange
        );

        if (hit2D.collider == null) { return false; }

        hit2D.collider.gameObject.TryGetComponent(out PlayerAbilityManager playerAbilityManager);
        if (playerAbilityManager == null) { return false; }

        return true;
    }

    public void SetAgentDisabled(bool activation)
    {
        agent.isStopped = activation;
    }

    public bool SetAgentDestination(Vector2 target)
    {
        NavMeshPath path = new();

        if (agent.CalculatePath(target, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetPath(path);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnAttackInteract(Vector2 direction, int damage, float force)
    {
        health -= damage;

        //AudioManager.Instance.Play("hit");

        agent.isStopped = true;

        rigidBody.AddForce(force * getHitKnockbackMultiplier * direction, ForceMode2D.Impulse);
        
        hitAudio.Play();

        spriteHolder.material = hitFlashMat;
        new Timer(.1f, () => spriteHolder.material = normalMat);

        Invoke(nameof(StunReset), stunTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.TryGetComponent(out PlayerHealthManager player);
        if (player != null) player.TakeDamage((collision.transform.position - transform.position).normalized, damage, force);
    }

    private void StunReset()
    {
        agent.isStopped = false;
        rigidBody.velocity = Vector2.zero;

        if (health <= 0) Death();
    }

    private void Death()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
