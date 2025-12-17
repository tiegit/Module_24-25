using UnityEngine;

public abstract class DirectionalMover
{
    protected float MoveSpeed;

    private ObstacleChecker _groundChecker;
    private float _gravityForce;
    private Vector3 _gravityVelocity = Vector3.zero;

    private Vector3 _currentDirection;
    private bool _isGravityActive;
    private bool _isStopped;

    public DirectionalMover(float moveSpeed, ObstacleChecker groundChecker, float gravityForce)
    {
        MoveSpeed = moveSpeed;
        _groundChecker = groundChecker;
        _gravityForce = gravityForce;

        ToggleGravity(true);
    }

    //public Vector3 CurrentVelocity => _currentDirection.normalized * MoveSpeed;
    public Vector3 CurrentHorizontalVelocity { get; protected set; }

    public void Update(float deltaTime)
    {
        if (_groundChecker.IsTouches() || _isGravityActive == false)
            _gravityVelocity.y = 0f;
        else if (_groundChecker.IsTouches() == false)
            _gravityVelocity.y -= _gravityForce;

        if (_isStopped)
        {
            CurrentHorizontalVelocity = Vector3.zero;
        }
        else
        {
            _currentDirection = new Vector3(_currentDirection.x, 0, _currentDirection.z);
            CurrentHorizontalVelocity = _currentDirection.normalized * MoveSpeed;
        }

        ApplyVelocity(CurrentHorizontalVelocity + _gravityVelocity, deltaTime);
    }

    protected abstract void ApplyVelocity(Vector3 velocity, float deltaTime);

    public void SetInputDirection(Vector3 inputDirection) => _currentDirection = inputDirection;

    public void SetMoveSpeed(float speed) => MoveSpeed = speed;

    public void ToggleGravity(bool value)
    {
        if (_isGravityActive == value)
            return;

        _isGravityActive = value;
    }

    public void Stop() => _isStopped = true;

    public void Resume() => _isStopped = false;
}
