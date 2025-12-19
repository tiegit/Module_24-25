using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterView : IDamageAnimator, IUpdatable
{
    private readonly int WalkingVelocity = Animator.StringToHash("Velocity");
    private readonly int IsExploded = Animator.StringToHash("IsExploded");
    private readonly int IsDying = Animator.StringToHash("IsDying");

    private readonly string InjuredLayer = "Injured Layer";

    private Animator _animator;
    private IMovable _movable;

    private bool _isCharacterIsInjured;
    private bool _isCharacterDead;

    public CharacterView(Animator animator, IMovable character)
    {
        _movable = character;
        _animator = animator;
    }

    public void Update(float deltaTime)
    {
        if (_movable.CurrentHorizontalVelocity.magnitude > 0.05f)
            StartRunning(_movable.CurrentHorizontalVelocity.magnitude / _movable.MaxSpeed);
        else
            StopRunning();

        if (_isCharacterIsInjured == false && _movable.IsInjured)
        {
            _isCharacterIsInjured = true;

            SetInjuredLayer();
        }

        if (_isCharacterDead == false && _movable.IsDead)
        {
            _isCharacterDead = true;

            DyingAnimation();
        }
    }

    public void TakeDamage() => _animator.SetTrigger(IsExploded);

    public void ResumeMove() => _movable.ResumeMove();

    private void StopRunning() => _animator.SetFloat(WalkingVelocity, 0);

    private void StartRunning(float speedRatio) => _animator.SetFloat(WalkingVelocity, speedRatio);

    private void SetInjuredLayer()
    {
        int layerIndex = _animator.GetLayerIndex(InjuredLayer);

        if (layerIndex != -1)
            _animator.SetLayerWeight(layerIndex, 1);
    }

    private void DyingAnimation() => _animator.SetBool(IsDying, true);
}