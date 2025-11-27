using UnityEngine;
using UnityEngine.InputSystem;

public class TowerPlacementManager : MonoBehaviour
{
    public LayerMask buildZoneMask;
    public LayerMask towerMask;
    public Color successfulPlacement;
    public Color failedPlacement;

    private GameObject _previewTower;
    private Tower _tower;
    private GameObject _radiusIndicator;
    private bool _isPlacing;

    void Update()
    {
        if (!_isPlacing) return;

        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(mouseScreen);
        world.z = 0;

        // позиционирование башни
        _previewTower.transform.position = world;

        // --- проверки ---

        bool canPlace = IsInsideBuildZone() && !HasTowerCollision() && PlayerStats.Instance.GetMoney() >= _tower.GetCost();

        SetColor(canPlace ? successfulPlacement : failedPlacement);

        if (Mouse.current.leftButton.wasPressedThisFrame && canPlace)
            FinishPlacement();

        if (Mouse.current.rightButton.wasPressedThisFrame)
            CancelPlacement();
    }

    private bool HasTowerCollision()
    {
        Collider2D myCol = _previewTower.GetComponent<Collider2D>();

        if (!myCol) return false;

        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = towerMask;
        filter.useLayerMask = true;

        Collider2D[] results = new Collider2D[10];
        int count = myCol.Overlap(filter, results);

        for (int i = 0; i < count; i++)
        {
            if (results[i] && results[i].gameObject != _previewTower)
                return true;
        }

        return false;
    }


    private bool IsInsideBuildZone()
    {
        return Physics2D.OverlapPoint(_previewTower.transform.position, buildZoneMask) != null;
    }



    public void StartPlacement(GameObject towerPrefab)
    {
        if (_isPlacing) return;
        _previewTower = Instantiate(towerPrefab);
        _tower = _previewTower.GetComponent<Tower>();
        _isPlacing = true;

        if (_tower.GetCost() > PlayerStats.Instance.GetMoney())
        {
            CancelPlacement();
            return;
        }

        DisableTowerLogic(_previewTower, true);

        SetColor(failedPlacement);
        
        _radiusIndicator = _previewTower.transform.Find("RadiusIndicator").gameObject;
        _radiusIndicator.SetActive(true);

        float range = _tower.GetRange();

// Масштабируем круг
        _radiusIndicator.transform.localScale = Vector3.one * (range * 2f);

    }

    private void FinishPlacement()
    {
        if (!PlayerStats.Instance.TrySpend(_tower.GetCost())) return;
        DisableTowerLogic(_previewTower, false);
        SetColor(Color.white);

        _radiusIndicator.SetActive(false);
        _previewTower = null;
        _radiusIndicator = null;
        _isPlacing = false;
        SoundManager.Instance.PlaySFX(SoundManager.Instance.towerBuyClip);
    }

    private void CancelPlacement()
    {
        Destroy(_previewTower);
        _previewTower = null;
        _isPlacing = false;
    }

    private void SetColor(Color c)
    {
        SpriteRenderer sr = _previewTower.transform.Find("TowerSprite").GetComponent<SpriteRenderer>();
        if (sr) sr.color = c;
    }

    private void DisableTowerLogic(GameObject obj, bool disable)
    {
        var s = obj.GetComponent<Tower>();
        if (s) s.enabled = !disable;
    }
}
