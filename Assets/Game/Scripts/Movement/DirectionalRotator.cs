using UnityEngine;

public abstract class DirectionalRotator
{
    private const float RotationTrashold = 0.05f;

    private float _rotationSpeed;

    private Vector3 _currentDirection;
    public DirectionalRotator(float rotationSpeed) => _rotationSpeed = rotationSpeed;

    public abstract Quaternion CurrentRotation { get; }

    public void SetInputDirection(Vector3 inputDirection) => _currentDirection = inputDirection;

    public void Update(float deltaTime)
    {
        if (_currentDirection.magnitude < RotationTrashold)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(_currentDirection.normalized);

        float speed = _rotationSpeed * deltaTime;

        ApplyRotation(Quaternion.RotateTowards(CurrentRotation, lookRotation, speed));
    }

    public abstract void ApplyRotation(Quaternion rotation);
}