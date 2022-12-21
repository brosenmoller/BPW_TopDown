using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static event Action<float> OnTimerUpdate;

    public void FixedUpdate()
    {
        OnTimerUpdate?.Invoke(Time.fixedDeltaTime);
    }
}
