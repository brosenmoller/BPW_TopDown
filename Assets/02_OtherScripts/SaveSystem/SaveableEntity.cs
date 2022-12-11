﻿// Orginal Code by Dapper Dino : https://www.youtube.com/watch?v=f5GvfZfy3yk&t=487s

using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveableEntity : MonoBehaviour 
{
    [SerializeField] private string id = string.Empty;

    public string Id => id;

    [ContextMenu("Generate Id")]
    private void GenerateID() => id = Guid.NewGuid().ToString();

    public object CaptureState()
    {
        var state = new Dictionary<string, object>();

        foreach (var saveable in GetComponents<ISaveable>())
        {
            state[saveable.GetType().ToString()] = saveable.CaptureState();
        }

        return state;
    }

    public void RestoreState(object state)
    {
        var stateDictionary = (Dictionary<string, object>)state;

        foreach (var saveable in GetComponents<ISaveable>())
        {
            string typeName = saveable.GetType().ToString();

            if (stateDictionary.TryGetValue(typeName, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }
}

