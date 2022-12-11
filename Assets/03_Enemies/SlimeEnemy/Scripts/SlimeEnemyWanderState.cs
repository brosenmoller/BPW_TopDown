using System.Collections;

public class SlimeEnemyWanderState : State<SlimeEnemyController>
{
    public override void OnEnter()
    {
        stateOwner.Controller.SetActiveAgent(true);
        stateOwner.Controller.StartCoroutine(Wandering());
    }

    public override void OnExit()
    {
        stateOwner.Controller.StopCoroutine(Wandering());
    }

    private IEnumerator Wandering()
    {
        yield return null; 
    }
}
