using UnityEngine;

public class PlayerMovementInputHandler
{
    private PlayerInput _playerInput;
    private ClickToMoveController _controller;

    private Camera _camera;

    public PlayerMovementInputHandler(PlayerInput playerInput, ClickToMoveController controller)
    {
        _playerInput = playerInput;
        _controller = controller;

        _camera = Camera.main;
    }

    public void Update()
    {
        if (_playerInput.LeftMouseButtonDown)
            TrySetMovePointFromMouseClick();
    }

    private bool TrySetMovePointFromMouseClick()
    {
        Ray ray = _camera.ScreenPointToRay(_playerInput.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
            return _controller.SetTargetPoint(hit.point);

        return false;
    }
}