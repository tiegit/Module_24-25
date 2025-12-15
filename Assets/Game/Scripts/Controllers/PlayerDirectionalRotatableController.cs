using UnityEngine;

public class PlayerDirectionalRotatableController : Controller
{
    private readonly PlayerInput _playerInput;
    private IDirectionalRotatable _rotatable;

    public PlayerDirectionalRotatableController(PlayerInput playerInput, IDirectionalRotatable rotatable)
    {
        _playerInput = playerInput;
        _rotatable = rotatable;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        Vector3 inputDirection = new Vector3(_playerInput.HorizontalInput, 0, _playerInput.Verticalinput);

        _rotatable.SetRotationDirection(inputDirection);
    }
}
