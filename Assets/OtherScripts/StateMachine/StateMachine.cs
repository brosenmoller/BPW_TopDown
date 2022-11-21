using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public Dictionary<Type, State> stateDictionary = new();
    public State currentState;
    
    public MonoBehaviour Controller { get; private set; }

    public StateMachine(State initialState, MonoBehaviour owner, params State[] states)
    {
        Controller = owner;

        foreach (State state in states)
        {
            stateDictionary.Add(state.GetType(), state);
            state.Setup(this);
        }

        currentState = initialState;
    }

    public void ChangeState(Type newStateType)
    {
        currentState?.OnExit();

        currentState = stateDictionary[newStateType];
        currentState.OnEnter();
    }

    public void OnUpdate()
    {
        currentState.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        currentState.OnFixedUpdate();
    }
}

