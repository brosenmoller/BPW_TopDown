using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine<T> where T : MonoBehaviour
{
    public Dictionary<Type, State<T>> stateDictionary = new();
    public State<T> currentState;
    
    public T Controller { get; private set; }

    public StateMachine(T owner, State<T> initialState, State<T>[] states)
    {
        Controller = owner;

        foreach (State<T> state in states)
        {
            stateDictionary.Add(state.GetType(), state);
            state.Setup(this);
        }

        if (stateDictionary.ContainsValue(initialState)) { currentState = initialState; }
        else { currentState = stateDictionary.Values.First(); }
    }

    public void ChangeState(Type newStateType)
    {
        if (!stateDictionary.ContainsKey(newStateType))
        {
            Debug.LogWarning($"{newStateType.Name} is not a state in the current statemachine ({nameof(T)})");
            return;
        }

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

