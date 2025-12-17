using UnityEngine;

public interface IDirectionalMovable : ITransformPosition
{
    float MaxSpeed { get; }
    Vector3 CurrentHorizontalVelocity { get; }

    void SetMoveDirection(Vector3 inputDirection);

    void ToggleGravity(bool value);
}
