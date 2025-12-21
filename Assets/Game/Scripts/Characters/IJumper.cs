using UnityEngine.AI;

public interface IJumper
{
    bool InJumpProcess { get; }
    float JumpDuration { get; }

    bool IsOnMeshLink(out OffMeshLinkData offMeshLinkData);
    void Jump(OffMeshLinkData offMeshLinkData);
}