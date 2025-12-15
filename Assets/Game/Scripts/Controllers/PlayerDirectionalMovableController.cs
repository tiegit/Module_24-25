using UnityEngine;

public class PlayerDirectionalMovableController : Controller
{
    private readonly PlayerInput _playerInput;
    private IDirectionalMovable _movable;

    public PlayerDirectionalMovableController(PlayerInput playerInput, IDirectionalMovable movable)
    {
        _playerInput = playerInput;
        _movable = movable;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        Vector3 inputDirection = new Vector3(_playerInput.HorizontalInput, 0, _playerInput.Verticalinput);

        _movable.SetMoveDirection(inputDirection);
    }
}
