using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentCharacter : MonoBehaviour, ICharacter
{
    [SerializeField] private float _maxHealth = 70f;
    [SerializeField, Range(0f, 100f)] private float _injuredLayerThreshold = 30f;

    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _injuredMoveSpeed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _jumpSpeed;
    [SerializeField] private AnimationCurve _jumpCurve;
    //[SerializeField] private Transform _target;

    private NavMeshAgent _agent;

    private AgentMover _mover;
    private TransformDirectionalRotator _rotator;
    private HealthCounter _health;
    private AgentJumper _jumper;

    private bool _isDead;

    public float MaxSpeed => _maxMoveSpeed;
    public float InjuredMoveSpeed => _injuredMoveSpeed;

    public Vector3 CurrentHorizontalVelocity => new Vector3(_mover.CurrentVelocity.x, 0, _mover.CurrentVelocity.z);
    public Quaternion CurrentRotation => _rotator.CurrentRotation;
    public bool InJumpProcess => _jumper.InProcess;
    public float JumpDuration => _jumper.Duration;

    public Vector3 Position => transform.position;
    public float CurrentHealthPercent => _health.CurrentHealthPercent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _mover = new AgentMover(_agent, _maxMoveSpeed);
        _rotator = new TransformDirectionalRotator(transform, _rotationSpeed);
        _jumper = new AgentJumper(this, _agent, _jumpSpeed, _jumpCurve);
    }

    private void Update() => _rotator.Update(Time.deltaTime);

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

    public void SetDestination(Vector3 position) => _mover.SetDestination(position);

    public void SetRotationDirection(Vector3 inputDirection) => _rotator.SetInputDirection(inputDirection);

    public bool TryGetPath(Vector3 targetPosition, NavMeshPath pathToTarget)
        => NavMeshUtils.TryGetPath(_agent, targetPosition, pathToTarget);

    public bool IsOnMeshLink(out OffMeshLinkData offMeshLinkData)
    {
        if (_agent.isOnOffMeshLink)
        {
            offMeshLinkData = _agent.currentOffMeshLinkData;

            return true;
        }

        offMeshLinkData = default(OffMeshLinkData);

        return false;
    }

    public void Jump(OffMeshLinkData offMeshLinkData)
    {
        if (_isDead)
            return;

        _jumper.Jump(offMeshLinkData);
    }

    public void TakeDamage(float value)
    {
        throw new System.NotImplementedException();
    }
}
