using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [Header("Waves settings")]
    public Wave[] Waves;

    [Header("Spawn points")]
    public Transform SpawnPoint;

    private List<Enemy> _activeEnemies = new List<Enemy>();
    public IReadOnlyList<Enemy> ActiveEnemies => _activeEnemies;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    public void Register(Enemy enemy)
    {
        _activeEnemies.Add(enemy);
    }

    public void Unregister(Enemy enemy)
    {
        _activeEnemies.Remove(enemy);
    }

    private IEnumerator SpawnWaves()
    {
        foreach (var wave in Waves)
        {
            // Пауза перед волной
            yield return new WaitForSeconds(wave.DelayBeforeStart);

            foreach (var enemyPack in wave.Enemies)
            {
                for (int i = 0; i < enemyPack.Count; i++)
                {
                    Instantiate(
                        enemyPack.Prefab,
                        SpawnPoint.position,
                        Quaternion.identity
                    );

                    yield return new WaitForSeconds(wave.DelayBetweenEnemies);
                }
            }

            // Ждём пока все враги умрут
            while (ActiveEnemies.Count > 0)
                yield return null;
        }
    }
}