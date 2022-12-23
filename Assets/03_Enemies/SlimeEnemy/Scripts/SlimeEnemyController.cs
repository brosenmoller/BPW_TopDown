using UnityEngine;

public class SlimeEnemyController : BaseEnemyController, IAttackInteractable
{
    [Header("Slime Settings")]
    public Transform target;
    [SerializeField] private float speed;
    public float updateDelay;
    public float wanderDistance;
    public Vector2 wanderDelay;
    public float detectionRange;

    protected override void SetupStateMachine()
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
}
