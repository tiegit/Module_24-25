using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private Image _healthBarFilling;

    private Health _health;
    private Camera _camera;

    private float _lastHealthPercent;

    public void Initialize(Health health = null)
    {
        _health = health;
        _camera = Camera.main;

        _lastHealthPercent = _health.CurrentHealthPercent;

        OnHealthChanged();
    }

    private void Update()
    {
        if (_health == null)
            return;

        if (_health.CurrentHealthPercent != _lastHealthPercent)
        {
            OnHealthChanged();
            _lastHealthPercent = _health.CurrentHealthPercent;
        }
    }

    private void LateUpdate() => transform.rotation = _camera.transform.rotation;

    private void OnHealthChanged() => _healthBarFilling.fillAmount = _health.CurrentHealthPercent;
}
