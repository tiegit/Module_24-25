using UnityEngine;

public interface IMovable : ITransformPosition
{
    float MaxSpeed { get; }
    float InjuredMoveSpeed { get; }
    Vector3 CurrentHorizontalVelocity { get; }

    void SetMoveSpeed(float speed);
    void StopMove();
    void ResumeMove();
}