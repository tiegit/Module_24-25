using UnityEngine;

public class RigidbodyDirectionalMover : DirectionalMover
{
    private const float SmoothFactor = 10f;
    private Rigidbody _rigidbody;

    public RigidbodyDirectionalMover(Rigidbody rigidbody,
                                     float moveSpeed,
                                     ObstacleChecker groundChecker,
                                     float gravityForce) : base(moveSpeed, groundChecker, gravityForce)
    {
        _rigidbody = rigidbody;

        SetMoveSpeed(moveSpeed);
    }

    protected override void ApplyVelocity(Vector3 velocity, float deltaTime) =>
         //_rigidbody.velocity = velocity;
         _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, velocity, deltaTime * SmoothFactor);
}