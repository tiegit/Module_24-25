using UnityEngine;

public interface IMovable : ITransformPosition
{
    float MaxSpeed { get; }
    float InjuredMoveSpeed { get; }
    Vector3 CurrentHorizontalVelocity { get; }
    float TimeToSpawn { get; }
    float TimeDeathDisappear { get; }

    void SetMoveSpeed(float speed);
    void StopMove();
    void SetDamageStatus(bool IsTakingDamage);
    bool InSpawnProcess(out float elapsedTime);
    void ResumeMove();
}