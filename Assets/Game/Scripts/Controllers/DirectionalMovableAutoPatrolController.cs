using UnityEngine;
using UnityEngine.AI;

public class DirectionalMovableAutoPatrolController : Controller
{
    private const float MinEdgeDistance = 0.5f;
    private const int MaxAttempts = 10;
    private const int MinCornersCountInPathToMove = 2;
    private const int StartCornerIndex = 0;
    private const int TargetCornerIndex = 1;
    private const float MoveDistanceTrashold = 0.2f;
    private const float TimeToReachTarget = 3f;

    private IDirectionalMovable _movable;
    private NavMeshQueryFilter _queryFilter;
    private float _patrolRadius;
    private float _minDistanceToTarget;
    private float _timeForIdle;
    private GameObject _patrolPointPrefabInstance;

    private Vector3 _currentPatrolPoint = Vector3.zero;
    private NavMeshPath _pathToTarget = new NavMeshPath();
    private float _idleTimer;
    private Vector3 _previousPosition;
    private float _stuckTimer;

    public DirectionalMovableAutoPatrolController(IDirectionalMovable movable,
                                                  NavMeshQueryFilter queryFilter,
                                                  float patrolRadius,
                                                  float minDistanceToTarget,
                                                  float timeForIdle,
                                                  GameObject patrolPointPrefabInstance = null)
    {
        _movable = movable;
        _queryFilter = queryFilter;
        _patrolRadius = patrolRadius;
        _minDistanceToTarget = minDistanceToTarget;
        _timeForIdle = timeForIdle;
        _patrolPointPrefabInstance = patrolPointPrefabInstance;
        _previousPosition = movable.Position;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        _idleTimer -= Time.deltaTime;

        CheckIfStuck();

        if (NavMeshUtils.TryGetPath(_movable.Position, _currentPatrolPoint, _queryFilter, _pathToTarget))
        {
            float distanceToTarget = NavMeshUtils.GetPathLength(_pathToTarget);

            if (IsTargetReached(distanceToTarget))
            {
                _idleTimer = _timeForIdle;
                GenerateNewPatrolPoint();
                return;
            }

            if (EnoughCornersInPath(_pathToTarget) && IdleTimerIsUp())
            {
                _movable.SetMoveDirection(_pathToTarget.corners[TargetCornerIndex] - _pathToTarget.corners[StartCornerIndex]);
                UpdatePatrolPointVisualization();
                return;
            }

            _movable.SetMoveDirection(Vector3.zero);
        }
        else
        {
            GenerateNewPatrolPoint();
        }
    }

    public override void Enable()
    {
        base.Enable();

        GenerateNewPatrolPoint();

        _patrolPointPrefabInstance.gameObject.SetActive(true);
    }

    public override void Disable()
    {
        base.Disable();

        _patrolPointPrefabInstance.gameObject.SetActive(false);
    }

    private void CheckIfStuck()
    {
        float distanceMoved = Vector3.Distance(_movable.Position, _previousPosition);

        if (distanceMoved < MoveDistanceTrashold)
        {
            _stuckTimer += Time.deltaTime;

            if (_stuckTimer > TimeToReachTarget)
            {
                GenerateNewPatrolPoint();

                _stuckTimer = 0f;
            }
        }
        else
        {
            _stuckTimer = 0f;
        }

        _previousPosition = _movable.Position;
    }
    private void GenerateNewPatrolPoint()
    {
        for (int i = 0; i < MaxAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * _patrolRadius;
            Vector3 randomOffset = new Vector3(randomCircle.x, 0, randomCircle.y);
            Vector3 potentialPoint = _movable.Position + randomOffset;

            if (NavMesh.SamplePosition(potentialPoint, out NavMeshHit hit, _patrolRadius, _queryFilter.areaMask))
            {
                if (NavMesh.FindClosestEdge(hit.position, out NavMeshHit edgeHit, _queryFilter.areaMask))
                {
                    if (edgeHit.distance >= MinEdgeDistance)
                    {
                        _currentPatrolPoint = hit.position;
                        _stuckTimer = 0f;
                        UpdatePatrolPointVisualization();

                        return;
                    }
                }
            }
        }
    }

    private void UpdatePatrolPointVisualization()
    {
        if (_patrolPointPrefabInstance != null)
            _patrolPointPrefabInstance.transform.position = _currentPatrolPoint;
    }

    private bool IsTargetReached(float distanceToTarget) => distanceToTarget < _minDistanceToTarget;

    private bool EnoughCornersInPath(NavMeshPath pathToTarget) => _pathToTarget.corners.Length >= MinCornersCountInPathToMove;

    private bool IdleTimerIsUp() => _idleTimer <= 0;
}
