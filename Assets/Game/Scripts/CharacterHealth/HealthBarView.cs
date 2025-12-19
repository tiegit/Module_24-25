using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private Image _healthBarFilling;

    private IDamagable _damagable;
    private Camera _camera;

    private float _lastHealthPercent;

    public void Initialize(IDamagable damagable = null)
    {
        if (_damagable == null)
            return;

        _camera = Camera.main;

        _lastHealthPercent = _damagable.CurrentHealthPercent;

        OnHealthChanged();
    }

    private void Update()
    {
        if (_damagable == null)
            return;

        if (_lastHealthPercent != _damagable.CurrentHealthPercent)
        {
            _lastHealthPercent = _damagable.CurrentHealthPercent;

            OnHealthChanged();
        }
    }

    private void LateUpdate()
    {
        if (_damagable == null)
            return;

        transform.rotation = _camera.transform.rotation;
    }

    private void OnHealthChanged() => _healthBarFilling.fillAmount = _damagable.CurrentHealthPercent;
}
