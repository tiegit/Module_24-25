using UnityEngine;

public class RandomAICharacterController : Controller
{
    private Character _character;
    private float _timeToChangeDirection;

    private Vector3 _inputDirection;
    private float _timer;

    public RandomAICharacterController(Character character, float timeToChangeDirection)
    {
        _character = character;
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

        _character.SetMoveDirection(_inputDirection);
        _character.SetRotationDirection(_inputDirection);
    }
}
