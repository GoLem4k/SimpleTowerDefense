using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _data;
    [SerializeField] private HealthBar2D _hpBar; // <-- добавили

    private Transform _target;
    private float _currentHP;
    private float _startDistance;

    public float Progress { get; private set; }
    public bool IsDead => _currentHP <= 0;

    private void Start()
    {
        _target = GameObject.Find("core").transform;
        _currentHP = _data.HP;

        _startDistance = Vector3.Distance(transform.position, _target.position);

        _hpBar.SetValue(1f);
    }

    private void OnEnable()
    {
        
        EnemyManager.Instance.Register(this);
    }

    private void OnDisable()
    {
        if (EnemyManager.Instance != null)
            EnemyManager.Instance.Unregister(this);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            _target.position,
            _data.Speed * Time.deltaTime
        );

        float currentDistance = Vector3.Distance(transform.position, _target.position);
        Progress = 1f - Mathf.Clamp01(currentDistance / _startDistance);

        if (currentDistance < 0.01f)
        {
            PlayerStats.Instance.Hit(_data.Damage);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHP -= damage;

        float normalizedHP = Mathf.Clamp01(_currentHP / _data.HP);
        _hpBar.SetValue(normalizedHP);

        if (_currentHP <= 0)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.enemyDeathClip);
            EventManager.OnEnemyDied(_data.Reward);
            Destroy(gameObject);
        }
    }
}