using System.Collections;
using UnityEngine;

public class SlimeEnemyWanderState : State<SlimeEnemyController>
{
    private Vector2 newPosition;
    private float timer = 0f;

    public override void OnEnter()
    {
        stateOwner.Controller.SetAgentDisabled(false);
    }

    public override void OnUpdate()
    {
        if (Time.time < timer) { return; }
        
        timer = Time.time + Random.Range(stateOwner.Controller.wanderDelay.x, stateOwner.Controller.wanderDelay.y);
        
        do
        {
            newPosition = GetRandomPosition();
        }
        while (!stateOwner.Controller.SetAgentDestination(newPosition));
    }

    private Vector2 GetRandomPosition()
    {
        return new()
        {
            x = Random.Range(stateOwner.Controller.transform.position.x - stateOwner.Controller.wanderDistance,
                     stateOwner.Controller.transform.position.x + stateOwner.Controller.wanderDistance),
            y = Random.Range(stateOwner.Controller.transform.position.y - stateOwner.Controller.wanderDistance,
                     stateOwner.Controller.transform.position.y + stateOwner.Controller.wanderDistance)
        };
    }
}
