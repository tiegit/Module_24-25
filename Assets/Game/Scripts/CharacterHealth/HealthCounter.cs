public class HealthCounter
{
    private HealthMediator _mediator;
    private float _maxHealth = 100f;
    private float _injuredLayerThreshold = 30f;

    private float _currentHealth;
    public float CurrentHealthPercent => _currentHealth / _maxHealth;

    public HealthCounter(HealthMediator mediator, float maxHealth, float injuredLayerThreshold)
    {
        _mediator = mediator;
        _currentHealth = _maxHealth = maxHealth;
        _injuredLayerThreshold = injuredLayerThreshold;
    }

    public void TakeDamage(float value)
    {
        ChangeHealth(-value);

        _mediator.TakeDamage();

        if (CurrentHealthPercent <= _injuredLayerThreshold / 100)
            _mediator.SetInjuredState();
    }

    private void ChangeHealth(float value)
    {
        _currentHealth += value;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;

            _mediator.SetDeathState();
        }

        if (_currentHealth >= _maxHealth)
            _currentHealth = _maxHealth;
    }
}
