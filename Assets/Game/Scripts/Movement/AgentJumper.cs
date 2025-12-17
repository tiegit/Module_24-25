using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AgentJumper
{
    private MonoBehaviour _coroutineRunner;
    private NavMeshAgent _agent;
    private float _speed;
    private AnimationCurve _yOffsetCurve;

    private Coroutine _jumpProcess;

    public AgentJumper(MonoBehaviour coroutineRunner,
                       NavMeshAgent agent,
                       float speed,
                       AnimationCurve yOffsetCurve)
    {
        _coroutineRunner = coroutineRunner;
        _agent = agent;
        _speed = speed;
        _yOffsetCurve = yOffsetCurve;
    }

    public bool InProcess => _jumpProcess != null;
    public float Duration { get; private set; }

    public void Jump(OffMeshLinkData offMeshLinkData)
    {
        if (InProcess)
            return;

        _jumpProcess = _coroutineRunner.StartCoroutine(JumpProcess(offMeshLinkData));
    }

    private IEnumerator JumpProcess(OffMeshLinkData offMeshLinkData)
    {
        Vector3 startPosition = offMeshLinkData.startPos;
        Vector3 endPosition = offMeshLinkData.endPos;

        Duration = Vector3.Distance(startPosition, endPosition) / _speed;

        float progress = 0f;

        while (progress < Duration)
        {
            float yOffset = _yOffsetCurve.Evaluate(progress / Duration);

            _agent.transform.position = Vector3.Lerp(startPosition, endPosition, progress / Duration) + Vector3.up * yOffset;
            progress += Time.deltaTime;

            yield return null;
        }

        _agent.CompleteOffMeshLink();
        _jumpProcess = null;
    }
}