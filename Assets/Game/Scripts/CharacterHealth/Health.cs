using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField, Range(0f, 100f)] private float _injuredLayerThreshold = 30f;

    private ICharacter _charecter;
    private DamagableManager _damagableManager;
    private IDamageAnimator _damageAnimator;

    private float _currentHealth;
    public Vector3 Position => transform.position;
    public float CurrentHealthPercent => _currentHealth / _maxHealth;

    public void Initialize(ICharacter character, DamagableManager manager, IDamageAnimator damageAnimator)
    {
        _charecter = character;
        _damagableManager = manager;
        _damagableManager.RegisterDamagable(this);

        _damageAnimator = damageAnimator;

        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float value)
    {        
        ChangeHealth(-value);

        _damageAnimator.TakeDamage();

        if (CurrentHealthPercent <= _injuredLayerThreshold / 100)
        {
            _damageAnimator.SetInjuredLayer();
            _charecter.SetMoveSpeed(_charecter.InjuredMoveSpeed);
        }

        _charecter.StopMove();
    }

    private void ChangeHealth(float value)
    {
        _currentHealth += value;

        if (_currentHealth <= 0)
        {
            _maxHealth = 0;

            _charecter.StopMove();
            _charecter.SetDeathState(true);

            _damageAnimator.DyingAnimation();
        }

        if (_currentHealth >= _maxHealth)
            _currentHealth = _maxHealth;
    }
}
