using UnityEngine;

public class Character : MonoBehaviour, IDirectionalMovable, IDirectionalRotatable, IMovable
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField, Range(0f, 100f)] private float _injuredLayerThreshold = 60f;

    [SerializeField, Space(15)] private float _maxMoveSpeed;
    [SerializeField] private float _injuredMoveSpeed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField, Space(15)] private ObstacleChecker _groundChecker;
    [SerializeField] private float _gravityForce;

    private DirectionalMover _mover;
    private DirectionalRotator _rotator;
    private Health _health;

    private DamagableManager _damagableManager;

    private Rigidbody _rigidbody;
    private bool _isInjured;
    private bool _isDead;

    public float MaxSpeed => _maxMoveSpeed;
    public float InjuredMoveSpeed => _injuredMoveSpeed;

    public Vector3 CurrentHorizontalVelocity => _mover.CurrentHorizontalVelocity;
    public Quaternion CurrentRotation => _rotator.CurrentRotation;

    public Vector3 Position => transform.position;
    public float CurrentHealthPercent => _health.CurrentHealthPercent;

    public bool IsDead => _isDead;
    public bool IsInjured => _isInjured;

    public void Initialize(DamagableManager manager, HealthMediator healthMediator)
    {
        _damagableManager = manager;
        _damagableManager.RegisterDamagable(this);
    
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
            Debug.Log($"Not found mover component");

        _health = new Health(healthMediator, _maxHealth);
    }

    private void Update()
    {
        _mover?.Update(Time.deltaTime);
        _rotator?.Update(Time.deltaTime);

        if(_isDead == false && _health.CurrentHealth <= 0)
            SetDeathState(true);

        if (_isDead == false && _isInjured == false && CurrentHealthPercent <= _injuredLayerThreshold / 100)
        {
            SetMoveSpeed(_injuredMoveSpeed);

            _isInjured = true;
        }

        if (_isDead && _rigidbody)
            return;
    }

    private void FixedUpdate()
    {
        if (_isDead && _rigidbody == null)
            return;

        _mover?.Update(Time.deltaTime);
        _rotator?.Update(Time.deltaTime);
    }

    public void SetMoveSpeed(float speed) => _mover.SetMoveSpeed(speed);

    public void SetDeathState(bool isDead)
    {
        StopMove();

        _isDead = isDead;
    }

    public void StopMove() => _mover.Stop();

    public void ResumeMove()
    {
        if (_isDead)
            return;

        _mover.Resume();
    }

    public void SetMoveDirection(Vector3 inputDirection) => _mover.SetInputDirection(inputDirection);

    public void SetRotationDirection(Vector3 inputDirection) => _rotator.SetInputDirection(inputDirection);

    public void TakeDamage(float value)
    {
        StopMove();

        _health.TakeDamage(value);
    }
}
