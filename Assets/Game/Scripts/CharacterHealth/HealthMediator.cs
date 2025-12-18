public class HealthMediator
{
    private ICharacter _character;
    private CharacterView _characterView;

    public HealthMediator(ICharacter character, CharacterView characterView)
    {
        _character = character;
        _characterView = characterView;
    }

    public void TakeDamage() => _characterView.TakeDamage();

    public void SetDeathState()
    {
        _character.SetDeathState(true);
        _characterView.DyingAnimation();
    }

    public void SetInjuredState()
    {
        _character.SetMoveSpeed(_character.InjuredMoveSpeed);
        _characterView.SetInjuredLayer();
    }
}