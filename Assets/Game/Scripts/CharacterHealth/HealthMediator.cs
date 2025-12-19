public class HealthMediator
{
    private CharacterView _characterView;

    public HealthMediator(CharacterView characterView) => _characterView = characterView;

    public void TakeDamage() => _characterView.TakeDamage();
}