using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
public class TowerData : ScriptableObject
{
    [SerializeField] private int cost;
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private float attack_speed;
    [SerializeField] private GameObject bullet_prefab;
    [SerializeField] private float bullet_speed;

    public int Cost => cost;
    public float Range => range;
    public float Damage => damage;
    public float AttackSpeed => attack_speed;
    public GameObject BulletPrefab => bullet_prefab;
    public float BulletSpeed => bullet_speed;
}
