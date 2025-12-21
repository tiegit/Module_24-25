public class HealthMediator
{
    private IDamageAnimator _characterView;

    public HealthMediator(IDamageAnimator characterView) => _characterView = characterView;

    public void TakeDamage() => _characterView.TakeDamage();
}