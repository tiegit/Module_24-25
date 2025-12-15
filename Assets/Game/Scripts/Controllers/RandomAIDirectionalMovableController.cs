using UnityEngine;

public class RandomAIDirectionalMovableController : Controller
{
    private IDirectionalMovable _movable;
    private float _timeToChangeDirection;

    private Vector3 _inputDirection;
    private float _timer;

    public RandomAIDirectionalMovableController(IDirectionalMovable movable, float timeToChangeDirection)
    {
        _movable = movable;
        _timeToChangeDirection = timeToChangeDirection;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        _timer += Time.deltaTime;

        if (_timer >= _timeToChangeDirection)
        {
            _inputDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            _timer = 0;
        }

        _movable.SetMoveDirection(_inputDirection);
    }
}
