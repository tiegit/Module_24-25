using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMoveController : Controller, IPointerTargetOwner
{
    private const float DeltaDistance = 1f;

    private Character _movable;
    private NavMeshQueryFilter _queryFilter;
    private List<NavMeshLink> _cachedLinks;
    private PlayerMovementInputHandler _inputHandler;

    private Vector3 _targetPosition;
    private NavMeshPath _pathToTarget = new NavMeshPath();
    private bool _hasTarget;
    private int _currentCornerIndex;

    private bool _wasOnLink;
    private Vector3 _globalStart;
    private Vector3 _globalEnd;
    private float _currentLinkHalfWidth;
    private bool _isJumping;

    public ClickToMoveController(PlayerInput playerInput,
                                Character movable,
                                NavMeshQueryFilter queryFilter,
                                IEnumerable<NavMeshLink> navMeshLinks)
    {
        _movable = movable;
        _queryFilter = queryFilter;
        _inputHandler = new PlayerMovementInputHandler(playerInput, this);

        _cachedLinks = new List<NavMeshLink>(navMeshLinks);
    }

    public Vector3 TargetPosition => _targetPosition;
    public bool HasTarget => _hasTarget;

    protected override void UpdateLogic(float deltaTime)
    {
        _inputHandler.Update();

        if (_isJumping && Vector3.Distance(_movable.Position, _globalEnd) < DeltaDistance)
            StopJump();

        bool isOnLink = IsOnNavMeshLink(_movable.Position);

        if (isOnLink != _wasOnLink)
            _wasOnLink = isOnLink;

        if (isOnLink && !_isJumping && Vector3.Distance(_movable.Position, _globalStart) < DeltaDistance * _currentLinkHalfWidth
          && NavMesh.SamplePosition(_movable.Position, out _, DeltaDistance, NavMesh.AllAreas))
            StartJump();

        if (_hasTarget)
            MoveTowardsTarget();
        else
            _movable.SetMoveDirection(Vector3.zero);

        ValidatePositionOnNavMesh();
    }

    public bool SetTargetPoint(Vector3 position)
    {
        if (NavMesh.CalculatePath(_movable.Position, position, _queryFilter, _pathToTarget))
        {
            if (_pathToTarget.status == NavMeshPathStatus.PathComplete)
            {
                _targetPosition = position;
                _hasTarget = true;
                _currentCornerIndex = 1;
                return true;
            }
        }

        ResetTarget();
        return false;
    }

    private void MoveTowardsTarget()
    {
        if (_pathToTarget.corners.Length == 0)
        {
            ResetTarget();
            return;
        }

        if (_currentCornerIndex < _pathToTarget.corners.Length)
        {
            Vector3 currentCorner = _pathToTarget.corners[_currentCornerIndex];

            float distanceToCorner = Vector3.Distance(
                new Vector3(_movable.Position.x, 0f, _movable.Position.z),
                new Vector3(currentCorner.x, 0f, currentCorner.z)
            );

            if (distanceToCorner < DeltaDistance)
            {
                _currentCornerIndex++;

                if (_currentCornerIndex >= _pathToTarget.corners.Length)
                {
                    CheckFinalTargetReached();
                    return;
                }
            }

            Vector3 nextCorner = _pathToTarget.corners[_currentCornerIndex];

            Vector3 horizontalDirection = new Vector3(
                nextCorner.x - _movable.Position.x,
                0f,
                nextCorner.z - _movable.Position.z
            ).normalized;

            _movable.SetMoveDirection(horizontalDirection);
        }
        else
        {
            CheckFinalTargetReached();
        }
    }

    private void ValidatePositionOnNavMesh()
    {
        if (NavMesh.SamplePosition(_movable.Position, out NavMeshHit hit, DeltaDistance, NavMesh.AllAreas))
        {
            if (Vector3.Distance(_movable.Position, hit.position) > 0.01f)
                _movable.SetPosition(hit.position);
        }
        else
        {
            _movable.SetMoveDirection(Vector3.zero);
        }
    }

    private void StartJump()
    {
        _movable.StartJump(_movable.Position, _globalEnd);
        _isJumping = true;
    }

    private void StopJump()
    {
        _movable.StopJump();
        _isJumping = false;
    }

    private void CheckFinalTargetReached()
    {
        float distanceToTarget = Vector3.Distance(
            new Vector3(_movable.Position.x, 0f, _movable.Position.z),
            new Vector3(_targetPosition.x, 0f, _targetPosition.z)
        );

        if (distanceToTarget <= DeltaDistance)
        {
            ResetTarget();
        }
        else
        {
            if (NavMesh.CalculatePath(_movable.Position, _targetPosition, _queryFilter, _pathToTarget))
            {
                if (_pathToTarget.status == NavMeshPathStatus.PathComplete)
                    _currentCornerIndex = 1;
                else
                    ResetTarget();
            }
            else
            {
                ResetTarget();
            }
        }
    }

    private void ResetTarget()
    {
        _hasTarget = false;
        _currentCornerIndex = 0;
        _movable.SetMoveDirection(Vector3.zero);
    }

    private bool IsOnNavMeshLink(Vector3 currentPosition)
    {
        foreach (NavMeshLink link in _cachedLinks)
        {
            if (!link.gameObject.activeInHierarchy) continue;

            _globalStart = link.transform.TransformPoint(link.startPoint);
            _globalEnd = link.transform.TransformPoint(link.endPoint);

            _currentLinkHalfWidth = link.width / 2;

            float distanceToLink = DistanceToLineSegment(currentPosition, _globalStart, _globalEnd);

            if (distanceToLink < 1.0f)
                return true;
        }

        return false;
    }

    private float DistanceToLineSegment(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 lineDirection = lineEnd - lineStart;
        float lineLength = lineDirection.magnitude;

        if (lineLength == 0)
            return Vector3.Distance(point, lineStart);

        Vector3 normalizedLineDirection = lineDirection / lineLength;
        Vector3 pointToLineStart = point - lineStart;
        float projectionLength = Vector3.Dot(pointToLineStart, normalizedLineDirection);
        projectionLength = Mathf.Clamp(projectionLength, 0, lineLength);
        Vector3 closestPoint = lineStart + normalizedLineDirection * projectionLength;

        return Vector3.Distance(point, closestPoint);
    }
}
