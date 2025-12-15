using UnityEngine;

public class PlayerCharacterController : Controller
{
    private readonly PlayerInput _playerInput;
    private Character _character;

    public PlayerCharacterController(PlayerInput playerInput, Character character)
    {
        _playerInput = playerInput;
        _character = character;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        Vector3 inputDirection = new Vector3(_playerInput.HorizontalInput, 0, _playerInput.Verticalinput);

        _character.SetMoveDirection(inputDirection);
        _character.SetRotationDirection(inputDirection);
    }
}
