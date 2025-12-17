using UnityEngine;

public class Character : MonoBehaviour, IDirectionalMovable, IDirectionalRotatable, ICharacter
{
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _injuredMoveSpeed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private ObstacleChecker _groundChecker;
    [SerializeField] private float _gravityForce;

    [SerializeField] private float _jumpSpeed;
    [SerializeField] private AnimationCurve _jumpCurve;

    private DirectionalMover _mover;
    private DirectionalRotator _rotator;
    private AgentJumper _jumper;

    private bool _isDead;
    private Rigidbody _rigidbody;

    public float MaxSpeed => _maxMoveSpeed;
    public float InjuredMoveSpeed => _injuredMoveSpeed;

    public Vector3 CurrentHorizontalVelocity => _mover.CurrentHorizontalVelocity;
    public Quaternion CurrentRotation => _rotator.CurrentRotation;
    public Vector3 Position => transform.position;
    public bool InJumpProcess => _jumper.InProcess;
    public float JumpDuration => _jumper.Duration;

    private void Awake()
    {
        if (TryGetComponent(out CharacterController characterController))
        {
            _mover = new CharacterControllerDirectionalMover(characterController, _maxMoveSpeed, _groundChecker, _gravityForce);
            _rotator = new TransformDirectionalRotator(transform, _rotationSpeed);
        }
        else if (TryGetComponent(out Rigidbody rigidbody))
        {
            _mover = new RigidbodyDirectionalMover(rigidbody, _maxMoveSpeed, _groundChecker, _gravityForce);
            _rotator = new RigidbodyDirectionalRotator(rigidbody, _rotationSpeed);

            _rigidbody = rigidbody;
        }
        else
            Debug.Log($"Not found mover comkdponent");
    }

    private void Update()
    {
        if (_isDead && _rigidbody)
            return;

        _mover?.Update(Time.deltaTime);
        _rotator?.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (_isDead && _rigidbody == null)
            return;

        _mover?.Update(Time.deltaTime);
        _rotator?.Update(Time.deltaTime);
    }

    public void SetMoveSpeed(float speed) => _mover.SetMoveSpeed(speed);

    public void SetDeathState(bool isDead) => _isDead = isDead;

    public void StopMove() => _mover.Stop();

    public void ResumeMove() => _mover.Resume();

    public void SetMoveDirection(Vector3 inputDirection) => _mover.SetInputDirection(inputDirection);

    public void ToggleGravity(bool value) => _mover.ToggleGravity(value);

    public void ToggleRigidbodyKinematic(bool value)
    {
        if (_rigidbody)
            return;

        _rigidbody.isKinematic = value;

        Debug.Log($"{value}");
    }

    public void SetRotationDirection(Vector3 inputDirection) => _rotator.SetInputDirection(inputDirection);

    public void Jump(Vector3 startPosition, Vector3 endPosition)
    {
        if (_isDead)
            return;
    }
}
