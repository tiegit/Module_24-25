using UnityEngine;

public interface IMovable : IDamagable
{
    float MaxSpeed { get; }
    float InjuredMoveSpeed { get; }
    Vector3 CurrentHorizontalVelocity { get; }
    Vector3 Position { get; }

    void SetMoveSpeed(float speed);
    void StopMove();
    void ResumeMove();
}