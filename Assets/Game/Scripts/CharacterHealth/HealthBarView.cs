using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private Character _chracter;
    [SerializeField] private Image _healthBarFilling;

    private Camera _camera;

    private float _lastHealthPercent;

    private void Awake()
    {
        _camera = Camera.main;

        _lastHealthPercent = _chracter.CurrentHealthPercent;

        OnHealthChanged();
    }

    private void Update()
    {
        if (_chracter == null)
            return;

        if (_chracter.CurrentHealthPercent != _lastHealthPercent)
        {
            OnHealthChanged();

            _lastHealthPercent = _chracter.CurrentHealthPercent;
        }
    }

    private void LateUpdate() => transform.rotation = _camera.transform.rotation;

    private void OnHealthChanged() => _healthBarFilling.fillAmount = _chracter.CurrentHealthPercent;
}
