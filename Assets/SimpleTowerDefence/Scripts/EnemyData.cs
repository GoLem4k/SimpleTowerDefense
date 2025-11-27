using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private int reward;
    [SerializeField] private int damage;
    [SerializeField] private float hp;
    [SerializeField] private float speed;
    [SerializeField] private float armor;

    public int Damage => damage;
    public int Reward => reward;
    public float HP => hp;
    public float Speed => speed;
    public float Armor => armor;
}