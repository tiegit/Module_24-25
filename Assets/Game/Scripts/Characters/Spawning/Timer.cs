using System.Collections;
using UnityEngine;

public class Timer
{
    private float _timeLimit;
    private float _elapsedTime;

    private MonoBehaviour _coroutineRunner;

    private Coroutine _process;

    public Timer(MonoBehaviour coroutineRunner) => _coroutineRunner = coroutineRunner;

    public float TimeLimit=> _timeLimit;

    public bool InProcess(out float elapsedTime)
    {
        if(_process == null)
        {
            elapsedTime = TimeLimit;

            return false;
        }

        elapsedTime = _elapsedTime;

        return true;
    }

    public void StartProcess(float time)
    {
        _timeLimit = time;

        if (_process != null)
            _coroutineRunner.StopCoroutine(_process);

        _process = _coroutineRunner.StartCoroutine(Process());
    }

    private IEnumerator Process()
    {
        _elapsedTime = 0;

        while (_elapsedTime < _timeLimit)
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _timeLimit)
                _elapsedTime = _timeLimit;

            yield return null;
        }

        _process = null;
    }
}
