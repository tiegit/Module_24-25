using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterView : MonoBehaviour, IDamageAnimator
{
    private readonly int WalkingVelocity = Animator.StringToHash("Velocity");
    private readonly int IsExploded = Animator.StringToHash("IsExploded");
    private readonly int IsDying = Animator.StringToHash("IsDying");

    private readonly string InjuredLayer = "Injured Layer";

    private ICharacter _character;
    private Animator _animator;

    public void Initialize(ICharacter character)
    {
        _character = character;

        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_character.CurrentHorizontalVelocity.magnitude > 0.05f)
            StartRunning(_character.CurrentHorizontalVelocity.magnitude / _character.MaxSpeed);
        else
            StopRunning();
    }

    public void TakeDamage() => _animator.SetTrigger(IsExploded);

    public void SetInjuredLayer()
    {
        int layerIndex = _animator.GetLayerIndex(InjuredLayer);

        if (layerIndex != -1)
            _animator.SetLayerWeight(layerIndex, 1);
    }

    public void ResumeMove() => _character.ResumeMove();

    public void DyingAnimation() => _animator.SetBool(IsDying, true);

    private void StopRunning() => _animator.SetFloat(WalkingVelocity, 0);

    private void StartRunning(float speedRatio) => _animator.SetFloat(WalkingVelocity, speedRatio);
}