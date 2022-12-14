using System.Collections;
using UnityEngine;

public class SlimeEnemyWanderState : State<SlimeEnemyController>
{
    public override void OnEnter()
    {
        stateOwner.Controller.SetAgentDisabled(false);
        stateOwner.Controller.StartCoroutine(Wandering());
    }

    public override void OnExit()
    {
        stateOwner.Controller.StopCoroutine(Wandering());
    }

    private IEnumerator Wandering()
    {
        Vector2 newPosition = GetRandomPosition();

        while (true)
        {

            yield return new WaitForSeconds(
                Random.Range(stateOwner.Controller.wanderDelay.x, stateOwner.Controller.wanderDelay.y)
            );

            do
            {
                newPosition = GetRandomPosition();
                yield return null;
            }
            while (!stateOwner.Controller.SetAgentDestination(newPosition));
        }
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
