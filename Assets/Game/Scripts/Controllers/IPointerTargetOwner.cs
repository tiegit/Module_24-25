using UnityEngine;

public interface IPointerTargetOwner
{
    bool HasTarget { get; }
    Vector3 TargetPosition { get; }
}