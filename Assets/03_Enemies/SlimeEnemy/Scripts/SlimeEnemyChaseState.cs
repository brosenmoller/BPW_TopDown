using System.Collections;
using UnityEngine;

public class SlimeEnemyChaseState : State<SlimeEnemyController>
{
    private float timer = 0f;

    public override void OnEnter()
    {
        stateOwner.Controller.SetAgentDisabled(false);
    }

    public override void OnUpdate()
    {
        if (Time.time < timer) { return; }

        timer = Time.time + stateOwner.Controller.updateDelay;

        stateOwner.Controller.SetAgentDestination(stateOwner.Controller.target.position);
    }
}
