using UnityEngine;
using UnityEngine.AI;

public class DirectionalMovableAutoPatrolController : Controller
{
    private const float MinEdgeDistance = 0.5f;
    private const int MaxAttempts = 10;
    private const float StuckCheckInterval = 2.0f;
    private const float MinStuckDistanceProgress = 0.5f;
    private const float DeltaDistance = 0.5f;

    private NavMeshQueryFilter _queryFilter;
    private float _patrolRadius;
    private float _minDistanceToTarget;
    private float _timeForIdle;
    private IDirectionalMovable _movable;
    private readonly IDirectionalRotatable _rotatable;
    private readonly IJumper _jumper;
    private GameObject _patrolPointInstance;

    private Vector3 _currentPatrolPoint = Vector3.zero;
    private NavMeshPath _patrolPath = new NavMeshPath();
    private int _currentCornerIndex;
    private float _idleTimer;
    private float _stuckTimer;
    private float _lastTargetDistance;

    private bool _isResetting;

    public DirectionalMovableAutoPatrolController(NavMeshQueryFilter queryFilter,
                                                  float patrolRadius,
                                                  float minDistanceToTarget,
                                                  float timeForIdle,
                                                  IDirectionalMovable movable,
                                                  IDirectionalRotatable rotatable,
                                                  IJumper jumper = null,
                                                  GameObject patrolPointInstance = null)
    {
        _movable = movable;
        _rotatable = rotatable;
        _jumper = jumper;
        _queryFilter = queryFilter;
        _patrolRadius = patrolRadius;
        _minDistanceToTarget = minDistanceToTarget;
        _timeForIdle = timeForIdle;
        _patrolPointInstance = patrolPointInstance;

        _stuckTimer = StuckCheckInterval;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        _idleTimer -= deltaTime;
        _stuckTimer -= deltaTime;

        if (_stuckTimer <= 0)
            CheckForStuckState();

        if (_jumper == null || !_jumper.InJumpProcess)
            HandlePatrolling();

        if (_jumper != null && !_jumper.InJumpProcess)
            HandleOffMeshLink();
    }

    public override void Enable()
    {
        base.Enable();

        GenerateNewPatrolPoint();

        _patrolPointInstance.gameObject.SetActive(true);
    }

    public override void Disable()
    {
        base.Disable();

        _patrolPointInstance.gameObject.SetActive(false);
        _patrolPath.ClearCorners();
        _currentCornerIndex = 0;
    }

    private void CheckForStuckState()
    {
        float currentTargetDistance = Vector3.Distance(_movable.Position, _currentPatrolPoint);
        float distanceProgress = _lastTargetDistance - currentTargetDistance;

        if (distanceProgress < MinStuckDistanceProgress && currentTargetDistance > _minDistanceToTarget)
            GenerateNewPatrolPoint();

        _lastTargetDistance = currentTargetDistance;
        _stuckTimer = StuckCheckInterval;
    }

    private void HandlePatrolling()
    {
        if (!IdleTimerIsUp())
        {
            _movable.StopMove();

            return;
        }

        if (_patrolPath.corners.Length == 0)
        {
            ResetPatrol();

            return;
        }

        if (_currentCornerIndex < _patrolPath.corners.Length)
        {
            Vector3 currentCorner = _patrolPath.corners[_currentCornerIndex];

            float distanceToCorner = Vector3.Distance(
                new Vector3(_movable.Position.x, 0f, _movable.Position.z),
                new Vector3(currentCorner.x, 0f, currentCorner.z)
            );

            if (distanceToCorner < DeltaDistance)
            {
                _currentCornerIndex++;

                if (_currentCornerIndex >= _patrolPath.corners.Length)
                {
                    _idleTimer = _timeForIdle;

                    GenerateNewPatrolPoint();

                    return;
                }
            }

            Vector3 nextCorner = _patrolPath.corners[_currentCornerIndex];

            _movable.ResumeMove();
            _movable.SetMoveDirection(nextCorner);

            UpdatePatrolPointVisualization();
        }
        else
        {
            CheckTargetReached();
        }
    }

    private void HandleOffMeshLink()
    {
        if (_jumper.IsOnMeshLink(out OffMeshLinkData offMeshLinkData))
        {
            if (_jumper.InJumpProcess == false)
            {
                Vector3 rotation = new Vector3((offMeshLinkData.endPos - offMeshLinkData.startPos).x, _rotatable.CurrentRotation.y, (offMeshLinkData.endPos - offMeshLinkData.startPos).z);

                _rotatable.SetRotationDirection(rotation);

                _jumper.Jump(offMeshLinkData);
            }

            return;
        }
    }

    private void GenerateNewPatrolPoint()
    {
        _movable.StopMove();

        Vector3 startPosition = _movable.Position;

        if (NavMeshUtils.TryFindRandomNavMeshPoint(startPosition, _patrolRadius, MinEdgeDistance, MaxAttempts, _queryFilter, out _currentPatrolPoint, out _patrolPath))
        {
            _currentCornerIndex = 0;
            _stuckTimer = StuckCheckInterval;

            _lastTargetDistance = Vector3.Distance(startPosition, _currentPatrolPoint);

            UpdatePatrolPointVisualization();
        }
        else
        {
            ResetPatrol();
        }
    }

    private void UpdatePatrolPointVisualization()
    {
        if (_patrolPointInstance != null)
            _patrolPointInstance.transform.position = _currentPatrolPoint;
    }

    private void CheckTargetReached()
    {
        float distanceToTarget = Vector3.Distance(
            new Vector3(_movable.Position.x, 0f, _movable.Position.z),
            new Vector3(_currentPatrolPoint.x, 0f, _currentPatrolPoint.z)
        );

        if (IsTargetReached(distanceToTarget))
        {
            _idleTimer = _timeForIdle;
            GenerateNewPatrolPoint();
            _movable.StopMove();

            return;
        }
    }

    private void ResetPatrol()
    {
        if (_isResetting)
            return;

        _isResetting = true;

        _currentPatrolPoint = _movable.Position;
        _patrolPath.ClearCorners();
        _currentCornerIndex = 0;
        _lastTargetDistance = 0f;
        _stuckTimer = StuckCheckInterval;

        GenerateNewPatrolPoint();

        _isResetting = false;

        UpdatePatrolPointVisualization();
    }

    private bool IsTargetReached(float distanceToTarget) => distanceToTarget < _minDistanceToTarget;

    private bool IdleTimerIsUp() => _idleTimer <= 0;
}