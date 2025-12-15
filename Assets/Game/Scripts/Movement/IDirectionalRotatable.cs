using UnityEngine;

public interface IDirectionalRotatable : ITransformPosition
{
    Quaternion CurrentRotation { get; }

    void SetRotationDirection(Vector3 inputDirection);
}
