using UnityEngine;

public class PlayerClickInputHandler
{
    private PlayerInput _playerInput;
    private Camera _camera;
    private int _ignoreMineLayerMask;

    public PlayerClickInputHandler(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        _camera = Camera.main;

        _ignoreMineLayerMask = ~LayerMask.GetMask("Mine");
    }

    public Vector3 HitPoint { get; private set; }
    public bool HasHit { get; private set; }

    public void UpdateInput()
    {
        if (_playerInput.LeftMouseButtonDown)
            TrySetPointFromMouseClick();
    }

    private void TrySetPointFromMouseClick()
    {
        Ray ray = _camera.ScreenPointToRay(_playerInput.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _ignoreMineLayerMask))
        {
            HitPoint = hit.point;
            HasHit = true;
        }
        else
        {
            HitPoint = Vector3.zero;
            HasHit = false;
        }
    }
}