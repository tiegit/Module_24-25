using UnityEngine;

public abstract class DirectionalRotator
{
    private float _rotationSpeed;

    private Vector3 _currentDirection;
    public DirectionalRotator(float rotationSpeed) => _rotationSpeed = rotationSpeed;

    public abstract Quaternion CurrentRotation { get; }

    public void SetInputDirection(Vector3 inputDirection) => _currentDirection = inputDirection;

    public void Update(float deltaTime)
    {
        if (_currentDirection.magnitude < 0.05f)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(_currentDirection.normalized);

        float speed = _rotationSpeed * deltaTime;

        ApplyRotation(Quaternion.RotateTowards(CurrentRotation, lookRotation, speed));
    }

    public abstract void ApplyRotation(Quaternion rotation);
}