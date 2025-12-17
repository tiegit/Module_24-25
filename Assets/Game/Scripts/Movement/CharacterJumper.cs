using System.Collections;
using UnityEngine;

public class CharacterJumper
{
    private MonoBehaviour _coroutineRunner;
    private Rigidbody _rigidbody;
    private float _speed;
    private AnimationCurve _yOffsetCurve;

    private Coroutine _jumpProcess;

    public CharacterJumper(MonoBehaviour coroutineRunner,
                       Rigidbody rigidbody,
                       float speed,
                       AnimationCurve yOffsetCurve)
    {
        _coroutineRunner = coroutineRunner;
        _rigidbody = rigidbody;
        _speed = speed;
        _yOffsetCurve = yOffsetCurve;
    }

    public bool InProcess => _jumpProcess != null;
    public float Duration { get; private set; }

    public void Jump(Vector3 startPosition, Vector3 endPosition)
    {
        if (InProcess)
            return;

        _jumpProcess = _coroutineRunner.StartCoroutine(JumpProcess(startPosition, endPosition));
    }

    private IEnumerator JumpProcess(Vector3 startPosition, Vector3 endPosition)
    {
        Duration = Vector3.Distance(startPosition, endPosition) / _speed;

        float progress = 0f;

        while (progress < Duration)
        {
            float yOffset = _yOffsetCurve.Evaluate(progress / Duration);

            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, progress / Duration) + Vector3.up * yOffset;

            _rigidbody.MovePosition(newPosition);

            progress += Time.deltaTime;

            yield return null;
        }

        _jumpProcess = null;
    }
}