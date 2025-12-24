using UnityEngine;
using UnityEngine.AI;

public class NavMeshUtils
{
    public static float GetPathLength(NavMeshPath path)
    {
        float pathLength = 0;

        if (path.corners.Length > 1)
            for (int i = 1; i < path.corners.Length; i++)
                pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);

        return pathLength;
    }

    public static bool TryGetPath(Vector3 sourcePosition, Vector3 targetPosition, NavMeshQueryFilter queryFilter, NavMeshPath pathToTarget)
    {
        if (NavMesh.CalculatePath(sourcePosition, targetPosition, queryFilter, pathToTarget) &&
            pathToTarget.status != NavMeshPathStatus.PathInvalid)
            return true;

        return false;
    }

    public static bool TryGetPath(NavMeshAgent agent, Vector3 targetPosition, NavMeshPath pathToTarget)
    {
        if (agent.CalculatePath(targetPosition, pathToTarget) &&
            pathToTarget.status != NavMeshPathStatus.PathInvalid)
            return true;

        return false;
    }

    public static bool TryFindRandomNavMeshPoint(Vector3 startPosition,
                                                 float patrolRadius,
                                                 float minEdgeDistance,
                                                 int maxAttempts,
                                                 NavMeshQueryFilter queryFilter,
                                                 out Vector3 foundPoint,
                                                 out NavMeshPath foundPath)
    {
        foundPoint = Vector3.zero;
        foundPath = new NavMeshPath();

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
            Vector3 randomOffset = new Vector3(randomCircle.x, 0, randomCircle.y);
            Vector3 potentialPoint = startPosition + randomOffset;

            if (NavMesh.SamplePosition(potentialPoint, out NavMeshHit hit, patrolRadius, queryFilter.areaMask))
            {
                if (NavMesh.FindClosestEdge(hit.position, out NavMeshHit edgeHit, queryFilter.areaMask))
                {
                    if (edgeHit.distance >= minEdgeDistance)
                    {
                        if (TryGetPath(startPosition, hit.position, queryFilter, foundPath) && foundPath.status == NavMeshPathStatus.PathComplete)
                        {
                            foundPoint = hit.position;
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
}
