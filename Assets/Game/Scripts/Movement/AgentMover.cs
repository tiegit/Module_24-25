using UnityEngine;
using UnityEngine.AI;

public class AgentMover
{
    private NavMeshAgent _agent;

    public AgentMover(NavMeshAgent agent, float movementSpeed)
    {
        _agent = agent;
        _agent.speed = movementSpeed;
        _agent.acceleration = 999;
    }

    public Vector3 CurrentVelocity => _agent.desiredVelocity;

    public void SetMoveSpeed(float speed) => _agent.speed = speed;

    public void SetDestination(Vector3 position)
    {
        if (_agent.isStopped)
            return;

        _agent.SetDestination(position);
    }

    public void Stop() => _agent.isStopped = true;

    public void Resume() => _agent.isStopped = false;
}
