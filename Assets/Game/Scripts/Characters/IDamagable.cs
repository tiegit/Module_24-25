public interface IDamagable
{    
    float CurrentHealthPercent { get; }
    bool IsDead { get; }
    bool IsInjured { get; }

    void TakeDamage(float value);
    void SetDeathState(bool isDead);
}