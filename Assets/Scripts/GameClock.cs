using UnityEngine;
using System;

public class GameClock : MonoBehaviour
{
    public static GameClock Instance;
    public event Action OnTick;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Tick()
    {
        OnTick?.Invoke();
        // need to ensure tick cant happen when game is processing
    }
}