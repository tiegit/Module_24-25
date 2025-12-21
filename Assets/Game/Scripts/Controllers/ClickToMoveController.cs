using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class ClickToMoveController : Controller, IPointerTargetOwner
{
    private const float DeltaDistance = 0.5f;

    private readonly PlayerClickInputHandler _clickHandler;
    private IDirectionalMovable _movable;
    private readonly IDirectionalRotatable _rotatable;
    private readonly IJumper _jumper;
    private NavMeshQueryFilter _queryFilter;

    private NavMeshPath _pathToTarget = new NavMeshPath();

    private Vector3 _targetPosition;
    private bool _hasTarget;
    private int _currentCornerIndex;

    public ClickToMoveController(PlayerClickInputHandler clickHandler,
                                IDirectionalMovable movable,
                                IDirectionalRotatable rotatable,
                                IJumper jumper,
                                NavMeshQueryFilter queryFilter)
    {
        _clickHandler = clickHandler;
        _movable = movable;
        _rotatable = rotatable;
        _jumper = jumper;
        _queryFilter = queryFilter;
    }

    public Vector3 TargetPosition => _targetPosition;
    public bool HasTarget => _hasTarget;

    protected override void UpdateLogic(float deltaTime)
    {
        if (_clickHandler.HasHit)
            SetTargetPoint(_clickHandler.HitPoint);

        if (_hasTarget)
            MoveTowardsTarget();
        else
            _movable.SetMoveDirection(Vector3.zero);


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

    private bool SetTargetPoint(Vector3 position)
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

            _movable.SetMoveDirection(nextCorner);
        }
        else
        {
            CheckFinalTargetReached();
        }
    }

    private void CheckFinalTargetReached()
    {
        float distanceToTarget = Vector3.Distance(
            new Vector3(_movable.Position.x, 0f, _movable.Position.z),
            new Vector3(_targetPosition.x, 0f, _targetPosition.z)
        );

        if (distanceToTarget < DeltaDistance)
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
}
