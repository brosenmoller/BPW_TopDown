public abstract class State
{
    protected StateMachine stateOwner;

    public void Setup(StateMachine stateMachine)
    {
        stateOwner = stateMachine;
    }

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
    public virtual void OnExit() { }
}
