using UnityEngine;

public interface ICharacter : IDamagable
{
    float MaxSpeed { get; }
    float InjuredMoveSpeed { get; }
    Vector3 CurrentHorizontalVelocity { get; }

    void SetMoveSpeed(float speed);
    void SetDeathState(bool isDead);
    void StopMove();
    void ResumeMove();
}