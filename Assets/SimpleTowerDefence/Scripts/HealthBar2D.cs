using UnityEngine;

public class HealthBar2D : MonoBehaviour
{
    private Vector3 _initialScale;

    private void Awake()
    {
        _initialScale = transform.localScale;
    }
    
    public void SetValue(float normalizedValue)
    {
        transform.localScale = new Vector3(
            _initialScale.x * normalizedValue,
            _initialScale.y,
            _initialScale.z
        );
    }

    private void LateUpdate()
    {
        // Чтобы полоска всегда была повернута к камере
        transform.forward = Camera.main.transform.forward;
    }
}