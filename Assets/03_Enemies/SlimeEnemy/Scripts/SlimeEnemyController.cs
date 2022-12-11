using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SlimeEnemyController : MonoBehaviour, IAttackInteractable
{
    [Header("Movement Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private float updateDelay;

    [Header("Combat")]
    [SerializeField] private int startHealth;
    [SerializeField] private float getHitKnockbackMultiplier;
    [SerializeField] private float stunTime;
    [SerializeField] private int damage;
    [SerializeField] private float force;

    [Header("Effects")]
    [SerializeField] private Material hitFlashMat;

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
        State<SlimeEnemyController>[] states = new[] { new SlimeEnemyWanderState() };
        stateMachine = new StateMachine<SlimeEnemyController>(this, states[0], states);
    }

    private IEnumerator UpdatePlayerTracker()
    {
        while (true)
        {
            if (!agent.isStopped)
            {
                agent.SetDestination(target.position);
            }
            yield return new WaitForSeconds(updateDelay);
        }
    }

    public void SetActiveAgent(bool activation)
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

        //AudioManager.Instance.Play("FungusHit");

        agent.isStopped = true;

        rigidBody.AddForce(force * getHitKnockbackMultiplier * direction, ForceMode2D.Impulse);
        spriteHolder.material = hitFlashMat;

        Invoke(nameof(SetNormalMaterial), .1f);
        Invoke(nameof(StunReset), stunTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.TryGetComponent(out PlayerHealthManager player);
        if (player != null) player.TakeDamage((collision.transform.position - transform.position).normalized, damage, force);
    }

    private void SetNormalMaterial()
    {
        spriteHolder.material = normalMat;
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
