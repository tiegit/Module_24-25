using UnityEngine;

public interface IDirectionalMovable : IMovable
{
    void SetMoveDirection(Vector3 inputDirection);
}
