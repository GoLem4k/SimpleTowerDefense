using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    
    [SerializeField] private int _money;
    [SerializeField] private int _hp;
    [SerializeField] private TextMeshProUGUI _tmpMoney;
    [SerializeField] private TextMeshProUGUI _tmpHP;
    
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        UpdateMoneyView();
        UpdateHPView();
        EventManager.EnemyDied += AddMoney;
    }

    public int GetMoney()
    {
        return _money;
    }

    public bool TrySpend(int amount)
    {
        if (_money < amount) return false;
        _money -= amount;
        UpdateMoneyView();
        return true;
    }

    public bool Hit(int amount)
    {
        if (amount > _hp)
        {
            _hp = 0;
        }
        else _hp -= amount;
        UpdateHPView();
        SoundManager.Instance.PlaySFX(SoundManager.Instance.enemyReachBaseClip);
        return true;
    }

    public void AddMoney(int amount)
    {
        _money += amount;
        UpdateMoneyView();
    }

    public void UpdateMoneyView()
    {
        _tmpMoney.text = _money.ToString();
    }
    
    public void UpdateHPView()
    {
        _tmpHP.text = _hp.ToString();
    }
}