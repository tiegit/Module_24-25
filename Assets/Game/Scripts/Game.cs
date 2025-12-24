public class Game
{
    private readonly PlayerInput _playerInput;
    private readonly HealthPackSpawner _healthPackSpawner;

    public Game(PlayerInput playerInput, HealthPackSpawner healthPackSpawner)
    {
        _playerInput = playerInput;
        _healthPackSpawner = healthPackSpawner;

        _healthPackSpawner.ToggleHealthPackSpawning();
    }

    public void CustomUpdate()
    {
        if (_playerInput.FKeyPressed)
            _healthPackSpawner.ToggleHealthPackSpawning();
    }
}