using UnityEngine;

public class TransformDirectionalRotator : DirectionalRotator
{
    private Transform _transform;

    public TransformDirectionalRotator(Transform transform, float rotationSpeed) : base(rotationSpeed) =>
        _transform = transform;

    public override Quaternion CurrentRotation => _transform.rotation;

    public override void ApplyRotation(Quaternion rotation) => _transform.rotation = rotation;
}
