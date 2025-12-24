using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HealthPackSpawner
{
    private Transform _playerTransform;
    private NavMeshQueryFilter _queryFilter;
    private HealthPack _healthPackPrefab;
    private readonly float _spawnInterval;
    private readonly MonoBehaviour _coroutineRunner;

    private bool _canHealthPackSpawn;
    private Coroutine _healthPackSpawnCoroutine;

    public HealthPackSpawner(Transform playerTransform, NavMeshQueryFilter queryFilter, HealthPack healthPackPrefab, float spawnInterval, MonoBehaviour coroutineRunner)
    {
        _playerTransform = playerTransform;
        _queryFilter = queryFilter;
        _healthPackPrefab = healthPackPrefab;
        _spawnInterval = spawnInterval;
        _coroutineRunner = coroutineRunner;
    }

    public void ToggleHealthPackSpawning()
    {
        _canHealthPackSpawn = !_canHealthPackSpawn;

        if (_canHealthPackSpawn)
            StartSpawning();
        else
            StopSpawning();
    }

    private void StartSpawning()
    {
        if (_healthPackSpawnCoroutine != null)
            _coroutineRunner.StopCoroutine(_healthPackSpawnCoroutine);

        _healthPackSpawnCoroutine = _coroutineRunner.StartCoroutine(HealthPackSpawnCoroutine());
    }

    private void StopSpawning()
    {
        if (_healthPackSpawnCoroutine != null)
        {
            _coroutineRunner.StopCoroutine(_healthPackSpawnCoroutine);
            _healthPackSpawnCoroutine = null;
        }
    }

    private IEnumerator HealthPackSpawnCoroutine()
    {
        while (_canHealthPackSpawn)
        {
            yield return new WaitForSeconds(_spawnInterval);
            TrySpawnHealthPack();
        }
    }

    private void TrySpawnHealthPack()
    {
        if (NavMeshUtils.TryFindRandomNavMeshPoint(_playerTransform.position, 3f, 0.5f, 10, _queryFilter, out Vector3 spawnPoint, out NavMeshPath path))
            Object.Instantiate(_healthPackPrefab, spawnPoint, Quaternion.identity);
    }
}
