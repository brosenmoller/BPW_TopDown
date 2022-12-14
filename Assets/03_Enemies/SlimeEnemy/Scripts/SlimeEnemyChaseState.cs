using System.Collections;
using UnityEngine;

public class SlimeEnemyChaseState : State<SlimeEnemyController>
{
    public override void OnEnter()
    {
        stateOwner.Controller.SetAgentDisabled(false);
        stateOwner.Controller.StartCoroutine(UpdatePlayerTracker());
    }

    public override void OnExit()
    {
        stateOwner.Controller.StopCoroutine(UpdatePlayerTracker());
    }

    private IEnumerator UpdatePlayerTracker()
    {
        while (true)
        {
            stateOwner.Controller.SetAgentDestination(stateOwner.Controller.target.position);
            
            yield return new WaitForSeconds(stateOwner.Controller.updateDelay);
        }
    }
}
