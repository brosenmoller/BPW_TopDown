using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TestEnemy : MonoBehaviour, IAttackInteractable
{
    [Header("Movement Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private float updateDelay;

    [Header("Combat")]
    [SerializeField] private int startHealth;
    [SerializeField] private float getHitKnockbackMultiplier;
    [SerializeField] private float stunTime;

    [Header("Effects")]
    [SerializeField] private Material hitFlashMat;

    private Material normalMat;

    private NavMeshAgent agent;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    private int health;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        rigidBody = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        normalMat = spriteRenderer.material;

        health = startHealth;
        
    }

    private void Start()
    {
        StartCoroutine(UpdatePlayerTracker());
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
    public void OnAttackInteract(Vector2 direction, int damage, float force)
    {
        health -= damage;

        //AudioManager.Instance.Play("FungusHit");

        agent.isStopped = true;

        rigidBody.AddForce(force * getHitKnockbackMultiplier * direction, ForceMode2D.Impulse);
        spriteRenderer.material = hitFlashMat;
        
        Invoke(nameof(SetNormalMaterial), .1f);
        Invoke(nameof(StunReset), stunTime);
    }

    private void SetNormalMaterial()
    {
        spriteRenderer.material = normalMat;
    }

    private void StunReset()
    {
        agent.isStopped = false;
        rigidBody.velocity = Vector2.zero;

        if (health <= 0) Death();
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
