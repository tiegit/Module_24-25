using UnityEngine;

public interface IDamagable
{
    Vector3 Position { get; }
    float CurrentHealthPercent { get; }
    void TakeDamage(float value);
}