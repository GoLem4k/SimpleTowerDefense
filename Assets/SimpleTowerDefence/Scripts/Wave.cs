using UnityEngine;

[System.Serializable]
public class Wave
{
    public string Name = "Wave";
    public float DelayBeforeStart = 1f;
    public float DelayBetweenEnemies = 0.3f;
    public WaveEnemy[] Enemies;
}