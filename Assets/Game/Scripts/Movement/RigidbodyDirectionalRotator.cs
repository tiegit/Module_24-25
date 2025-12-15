using UnityEngine;

public class RigidbodyDirectionalRotator : DirectionalRotator
{
    private Rigidbody _rigidbody;

    public RigidbodyDirectionalRotator(Rigidbody rigidbody, float rotationSpeed) : base(rotationSpeed) =>
        _rigidbody = rigidbody;

    public override Quaternion CurrentRotation => _rigidbody.rotation;

    public override void ApplyRotation(Quaternion rotation) => _rigidbody.MoveRotation(rotation);
}