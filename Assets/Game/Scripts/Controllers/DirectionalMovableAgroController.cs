using UnityEngine;
using UnityEngine.AI;

public class DirectionalMovableAgroController : Controller
{
    private const int MinCornersCountInPathToMove = 2;
    private const int StartCornerIndex = 0;
    private const int TargetCornerIndex = 1;

    private IDirectionalMovable _movable;

    private Transform _target;

    private float _agroRange;
    private float _minDistanceToTarget;

    private NavMeshQueryFilter _queryFilter;

    private float _idleTimer;
    private float _timeForIdle;

    private NavMeshPath _pathToTarget = new NavMeshPath();

    public DirectionalMovableAgroController(IDirectionalMovable movable,
                                            Transform target,
                                            float agroRange,
                                            float minDistanceToTarget,
                                            NavMeshQueryFilter queryFilter,
                                            float timeForIdle)
    {
        _movable = movable;
        _target = target;
        _agroRange = agroRange;
        _minDistanceToTarget = minDistanceToTarget;
        _queryFilter = queryFilter;
        _timeForIdle = timeForIdle;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        _idleTimer -= Time.deltaTime;

        if (NavMeshUtils.TryGetPath(_movable.Position, _target.position, _queryFilter, _pathToTarget))
        {
            float distanceToTarget = NavMeshUtils.GetPathLength(_pathToTarget);

            if (IsTargetReached(distanceToTarget))
                _idleTimer = _timeForIdle;

            if (InAgroRange(distanceToTarget)
                && EnoughCornersInPath(_pathToTarget)
                && IdleTimerIsUp())
            {
                _movable.ResumeMove();
                _movable.SetMoveDirection(_pathToTarget.corners[TargetCornerIndex] - _pathToTarget.corners[StartCornerIndex]);
                return;
            }

            _movable.StopMove();
        }
    }

    private bool IsTargetReached(float distanceToTarget) => distanceToTarget < _minDistanceToTarget;

    private bool InAgroRange(float distanceToTarget) => distanceToTarget < _agroRange;

    private bool EnoughCornersInPath(NavMeshPath pathToTarget) => _pathToTarget.corners.Length >= MinCornersCountInPathToMove;

    private bool IdleTimerIsUp() => _idleTimer <= 0;
}
