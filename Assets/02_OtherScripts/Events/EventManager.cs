using System;
using System.Collections.Generic;

public enum EventType
{
    ON_PLAYER_HEALTH_CHANGE,
}

public static class EventManager
{
    private static readonly Dictionary<EventType, Action> eventDictionary = new();

    public static void AddListener(EventType eventType, Action action)
    {
        if (!eventDictionary.ContainsKey(eventType))
        {
            eventDictionary.Add(eventType, null);
        }

        eventDictionary[eventType] -= action;
        eventDictionary[eventType] += action;
    }

    public static void RemoveListener(EventType eventType, Action action) 
    { 
        if (eventDictionary.ContainsKey(eventType) && eventDictionary[eventType] != null) 
        {
            eventDictionary[eventType] -= action;
        }
    }

    public static void InvokeEvent(EventType eventType)
    {
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType]?.Invoke();
        }
    }
}
