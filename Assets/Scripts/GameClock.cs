using UnityEngine;
using System;

public class GameClock : MonoBehaviour
{
    public static GameClock Instance;
    public event Action<int> OnTick;

    private int turn = 0;

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
        OnTick?.Invoke(turn);
        turn++;
        // need to ensure tick cant happen when game is processing
    }
}