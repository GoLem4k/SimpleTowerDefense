using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<int> EnemyDied;
    public static event Action EnemyPassed;

    public static void OnEnemyDied(int reward)
    {
        EnemyDied?.Invoke(reward);
    }

    public static void OnEnemyPassed()
    {
        EnemyPassed?.Invoke();
    }
}
