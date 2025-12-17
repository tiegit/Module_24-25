public class MovementControllerHandler
{
    private PlayerInput _playerInput;
    private ClickToMoveController _playerMoveController;
    private DirectionalMovableAutoPatrolController _playerAutoPatrolController;
    private float _idleBehaviourSwitchTime;

    private float _idleTimer;

    private Controller _currentController;

    public MovementControllerHandler(PlayerInput playerInput,
                                     ClickToMoveController playerMoveController,
                                     DirectionalMovableAutoPatrolController playerAutoPatrolController,
                                     float idleBehaviourSwitchTime)
    {
        _playerInput = playerInput;
        _playerMoveController = playerMoveController;
        _playerAutoPatrolController = playerAutoPatrolController;
        _idleBehaviourSwitchTime = idleBehaviourSwitchTime;

        _playerAutoPatrolController.Disable();
        _playerMoveController.Disable();

        SetController(_playerMoveController);
    }

    private void SetController(Controller сontroller)
    {
        if (_currentController != сontroller)
        {
            _currentController?.Disable();
            _currentController = сontroller;
            _currentController.Enable();
        }
    }

    public void Update(float deltaTime)
    {
        if (_playerInput.LeftMouseButtonDown)
        {
            _idleTimer = 0f;

            SetController(_playerMoveController);
        }
        else
        {
            if (!_playerMoveController.HasTarget)
            {
                _idleTimer += deltaTime;

                if (_idleTimer >= _idleBehaviourSwitchTime)
                    SetController(_playerAutoPatrolController);
            }
            else
            {
                _idleTimer = 0f;
            }
        }
    }
}
