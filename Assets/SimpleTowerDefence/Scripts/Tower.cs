using UnityEngine;
using System.Linq;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerData _data;

    private Enemy _currentTarget;
    private float _cooldown;

    private void Update()
    {
        _cooldown -= Time.deltaTime;

        // 1. если нет цели — пытаемся найти
        if (_currentTarget == null)
        {
            _currentTarget = FindTarget();
        }
        else
        {
            // 2. проверяем: не умер и не вышел ли из радиуса
            if (_currentTarget.IsDead || 
                Vector3.Distance(transform.position, _currentTarget.transform.position) > _data.Range)
            {
                _currentTarget = FindTarget();
            }
        }

        // 3. если есть цель — стреляем
        if (_currentTarget != null && _cooldown <= 0f)
        {
            Shoot(_currentTarget);
            _cooldown = 1f / _data.AttackSpeed;
        }
    }

    private Enemy FindTarget()
    {
        // Собираем всех врагов (можно через EnemyManager)
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        // Фильтруем только тех, кто в радиусе
        var inRange = enemies
            .Where(e => !e.IsDead &&
                        Vector3.Distance(transform.position, e.transform.position) <= _data.Range)
            .ToList();

        if (inRange.Count == 0)
            return null;

        // Сортировка по прогрессу (кто дальше всех по пути → тот первый)
        inRange = inRange.OrderByDescending(e => e.Progress).ToList();

        // Берём первого в колонне
        return inRange[0];
    }

    public void Shoot(Enemy enemy)
    {
        var bullet = Instantiate(_data.BulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(enemy, _data.BulletSpeed, _data.Damage);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.towerShootClip);
    }

    public float GetRange()
    {
        return _data.Range;
    }

    public int GetCost()
    {
        return _data.Cost;
    }
}