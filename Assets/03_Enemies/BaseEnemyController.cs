using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemyController : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] protected int startHealth;
    [SerializeField] protected float getHitKnockbackMultiplier;
    [SerializeField] protected float stunTime;
    [SerializeField] protected int damage;
    [SerializeField] protected float force;

    [Header("Effects")]
    [SerializeField] protected Material hitFlashMat;
    [SerializeField] protected AudioObject hitAudio;

    protected Material normalMat;

    protected NavMeshAgent agent;
    protected Rigidbody2D rigidBody2D;
    protected SpriteRenderer spriteHolder;
    [HideInInspector] public Animator animator;

    protected int health;

    protected StateMachine<SlimeEnemyController> stateMachine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        spriteHolder = GetComponent<SpriteRenderer>();
        normalMat = spriteHolder.material;

        health = startHealth;
    }

    private void Start()
    {
        SetupStateMachine();
    }

    protected abstract void SetupStateMachine();

    private void FixedUpdate()
    {
        stateMachine.OnFixedUpdate();
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

    public virtual void OnAttackInteract(Vector2 direction, int damage, float force)
    {
        health -= damage;

        agent.isStopped = true;

        rigidBody2D.AddForce(force * getHitKnockbackMultiplier * direction, ForceMode2D.Impulse);

        hitAudio.Play();

        spriteHolder.material = hitFlashMat;
        new Timer(.1f, () => spriteHolder.material = normalMat);

        Invoke(nameof(StunReset), stunTime);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.gameObject.TryGetComponent(out PlayerHealthManager player);
        if (player != null) player.TakeDamage((collision.transform.position - transform.position).normalized, damage, force);
    }

    private void StunReset()
    {
        agent.isStopped = false;
        rigidBody2D.velocity = Vector2.zero;

        if (health <= 0) Death();
    }

    private void Death()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
