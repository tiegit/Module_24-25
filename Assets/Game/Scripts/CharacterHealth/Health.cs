public class Health
{
    private HealthMediator _mediator;
    private float _maxHealth;

    private float _currentHealth;

    public Health(HealthMediator mediator, float maxHealth)
    {
        _mediator = mediator;
        _currentHealth = _maxHealth = maxHealth;
    }

    public float CurrentHealth => _currentHealth;
    public float CurrentHealthPercent => _currentHealth / _maxHealth;

    public void TakeDamage(float value)
    {
        ChangeHealth(-value);

        _mediator.TakeDamage();
    }

    private void ChangeHealth(float value)
    {
        _currentHealth += value;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
        }

        if (_currentHealth >= _maxHealth)
            _currentHealth = _maxHealth;
    }
}
