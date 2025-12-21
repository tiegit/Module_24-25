using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterView : IDamageAnimator, IUpdatable
{
    private const string HitToBodyAnimationName = "Hit To Body";

    private readonly int WalkingVelocity = Animator.StringToHash("Velocity");
    private readonly int IsExploded = Animator.StringToHash("IsExploded");
    private readonly int IsDying = Animator.StringToHash("IsDying");

    private readonly string InjuredLayer = "Injured Layer";

    private Animator _animator;
    private IMovable _movable;
    private readonly IDamagable _damagable;

    private bool _isCharacterIsInjured;
    private bool _isCharacterDead;
    private MonoBehaviour _coroutineRunner;
    private float _hitToBodyClipLength;

    public CharacterView(Animator animator, IMovable movable, IDamagable damagable, MonoBehaviour coroutineRunner)
    {
        _movable = movable;
        _damagable = damagable;
        _animator = animator;
        _coroutineRunner = coroutineRunner;

        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == HitToBodyAnimationName)
            {
                _hitToBodyClipLength = clip.length;

                break;
            }
        }
    }

    public void Update(float deltaTime)
    {
        if (_movable.CurrentHorizontalVelocity.magnitude > 0.05f)
            StartRunning(_movable.CurrentHorizontalVelocity.magnitude / _movable.MaxSpeed);
        else
            StopRunning();

        if (_isCharacterIsInjured == false && _damagable.IsInjured)
        {
            _isCharacterIsInjured = true;

            SetInjuredLayer();
        }

        if (_isCharacterDead == false && _damagable.IsDead)
        {
            _isCharacterDead = true;

            DyingAnimation();
        }
    }

    public void TakeDamage()
    {
        _animator.SetTrigger(IsExploded);
        _coroutineRunner.StartCoroutine(WaitForHitAnimationAndResume());
    }

    private IEnumerator WaitForHitAnimationAndResume()
    {
        while (_animator.GetCurrentAnimatorStateInfo(0).IsName(HitToBodyAnimationName) == false)
            yield return null;

        yield return new WaitForSeconds(_hitToBodyClipLength);

        ResumeMove();
    }

    private void ResumeMove() => _movable.ResumeMove();

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