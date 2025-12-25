using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentCharacter : MonoBehaviour, IDirectionalMovable, IDirectionalRotatable, IJumper, IDamagable, IHealable
{
    [SerializeField] private float _maxHealth = 70f;
    [SerializeField, Range(0f, 100f)] private float _injuredLayerThreshold = 30f;

    [SerializeField, Space(15)] private float _maxMoveSpeed;
    [SerializeField] private float _injuredMoveSpeed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField, Space(15)] private float _jumpSpeed;
    [SerializeField] private AnimationCurve _jumpCurve;

    [SerializeField, Space(15)] private float _timeToSpawn = 2f;
    [SerializeField] private float _timeDeathDisappear = 4f;

    private NavMeshAgent _agent;

    private AgentMover _mover;
    private TransformDirectionalRotator _rotator;
    private AgentJumper _jumper;
    private Health _health;

    private Timer _spawnTimer;

    public float MaxSpeed => _maxMoveSpeed;
    public float InjuredMoveSpeed => _injuredMoveSpeed;

    public Vector3 CurrentHorizontalVelocity => new Vector3(_mover.CurrentVelocity.x, 0, _mover.CurrentVelocity.z);
    public Quaternion CurrentRotation => _rotator.CurrentRotation;

    public Vector3 Position => transform.position;
    public float CurrentHealthPercent => _health.CurrentHealthPercent;

    public bool InJumpProcess => _jumper.InProcess;
    public float JumpDuration => _jumper.Duration;

    public bool IsTakingDamage { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsInjured { get; private set; }

    public float TimeToSpawn => _spawnTimer.TimeLimit;
    public float TimeDeathDisappear => _timeDeathDisappear;

    public void Initialize(MonoBehaviour coroutineRunner, HealthMediator healthMediator)
    {
        _spawnTimer = new Timer(coroutineRunner);
        _spawnTimer.StartProcess(_timeToSpawn);

        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _mover = new AgentMover(_agent, _maxMoveSpeed);
        _rotator = new TransformDirectionalRotator(transform, _rotationSpeed);
        _jumper = new AgentJumper(this, _agent, _jumpSpeed, _jumpCurve);

        _health = new Health(healthMediator, _maxHealth);
    }

    private void Update()
    {
        _rotator.Update(Time.deltaTime);

        if (IsDead == false)
        {
            if (_health.CurrentHealth <= 0)
                SetDeathState(true);

            bool desiredInjuredState = CurrentHealthPercent <= _injuredLayerThreshold / 100f;

            if (desiredInjuredState != IsInjured)
            {
                IsInjured = desiredInjuredState;

                SetMoveSpeed(IsInjured ? _injuredMoveSpeed : _maxMoveSpeed);
            }
        }
    }

    public void SetMoveSpeed(float speed) => _mover.SetMoveSpeed(speed);

    public void SetDeathState(bool isDead)
    {
        StopMove();

        IsDead = isDead;
    }

    public void StopMove() => _mover.Stop();

    public void ResumeMove()
    {
        if (IsDead || IsTakingDamage)
            return;

        _mover.Resume();
    }

    public void SetMoveDirection(Vector3 inputDirection) => SetDestination(inputDirection); // тут inputDirection не совсем так должен называться, сделал чтобы и в Character это был сырой(не нормализованный) вектор

    public void SetRotationDirection(Vector3 inputDirection) => _rotator.SetInputDirection(inputDirection);

    public void TakeDamage(float value)
    {
        StopMove();

        _health.TakeDamage(value);
    }

    public void SetDamageStatus(bool isTakingDamage) => IsTakingDamage = isTakingDamage;

    public void Heal(int healingAmount) => _health.AddHealth(healingAmount);

    public bool TryGetPath(Vector3 targetPosition, NavMeshPath pathToTarget)
        => NavMeshUtils.TryGetPath(_agent, targetPosition, pathToTarget);

    public bool IsOnMeshLink(out OffMeshLinkData offMeshLinkData)
    {
        if (_agent.isOnOffMeshLink)
        {
            offMeshLinkData = _agent.currentOffMeshLinkData;

            return true;
        }

        offMeshLinkData = default;

        return false;
    }

    public void Jump(OffMeshLinkData offMeshLinkData)
    {
        if (IsDead)
            return;

        _jumper.Jump(offMeshLinkData);
    }

    private void SetDestination(Vector3 position) => _mover.SetDestination(position);

    public bool InSpawnProcess(out float elapsedTime) => _spawnTimer.InProcess(out elapsedTime);
}
