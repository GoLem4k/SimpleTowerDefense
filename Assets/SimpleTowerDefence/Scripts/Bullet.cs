using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Enemy _target;
    private float _speed;
    private float _damage;

    public void Init(Enemy target, float speed, float damage)
    {
        _target = target;
        _speed = speed;
        _damage = damage;
    }

    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        transform.position = Vector3.MoveTowards(
            transform.position,
            _target.transform.position,
            _speed * Time.deltaTime
        );
        
        if (Vector3.Distance(transform.position, _target.transform.position) < 0.1f)
        {
            _target.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}