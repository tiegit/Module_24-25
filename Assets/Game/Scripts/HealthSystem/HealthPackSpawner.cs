using UnityEngine;
using UnityEngine.AI;

public class HealthPackSpawner
{
    private Transform _playerTransform;
    private NavMeshQueryFilter _queryFilter;
    private HealthPack _healthPackPrefab;

    public HealthPackSpawner(Transform playerTransform, NavMeshQueryFilter queryFilter, HealthPack healthPackPrefab)
    {
        _playerTransform = playerTransform;
        _queryFilter = queryFilter;
        _healthPackPrefab = healthPackPrefab;
    }

    public HealthPack TrySpawnHealthPack()
    {
        if (NavMeshUtils.TryFindRandomNavMeshPoint(_playerTransform.position, 3f, 0.5f, 10, _queryFilter, out Vector3 spawnPoint, out NavMeshPath path))
            return Object.Instantiate(_healthPackPrefab, spawnPoint, Quaternion.identity);

        return null;
    }
}
