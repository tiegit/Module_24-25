using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterView : IDamageAnimator, IUpdatable
{
    private const string HitToBodyAnimationName = "Hit To Body";
    private const string EdgeKey = "_Edge";

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
    private SkinnedMeshRenderer[] _renderers;

    private float _hitToBodyClipLength;

    public CharacterView(Animator animator,
                         IMovable movable,
                         IDamagable damagable,
                         MonoBehaviour coroutineRunner)
    {
        _movable = movable;
        _damagable = damagable;
        _animator = animator;
        _coroutineRunner = coroutineRunner;

        _renderers = _animator.GetComponentsInChildren<SkinnedMeshRenderer>();

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
        if (_movable.InSpawnProcess(out float elapsedTime))
            SetFloatFor(_renderers, EdgeKey, 1f - elapsedTime / _movable.TimeToSpawn);

        if (_movable.CurrentHorizontalVelocity.magnitude > 0.05f)
            StartRunning(_movable.CurrentHorizontalVelocity.magnitude / _movable.MaxSpeed);
        else
            StopRunning();

        if (_damagable.IsInjured != _isCharacterIsInjured)
        {
            _isCharacterIsInjured = _damagable.IsInjured;
            SetInjuredLayer(_isCharacterIsInjured);
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
        _coroutineRunner.StartCoroutine(TakingDamage());
    }

    private IEnumerator TakingDamage()
    {
        _movable.SetDamageStatus(true);

        while (_animator.GetCurrentAnimatorStateInfo(0).IsName(HitToBodyAnimationName) == false)
            yield return null;

        yield return new WaitForSeconds(_hitToBodyClipLength);

        _movable.SetDamageStatus(false);
    }

    private void StopRunning() => _animator.SetFloat(WalkingVelocity, 0);

    private void StartRunning(float speedRatio) => _animator.SetFloat(WalkingVelocity, speedRatio);

    private void SetInjuredLayer(bool isInjured)
    {
        int layerIndex = _animator.GetLayerIndex(InjuredLayer);

        if (layerIndex != -1)
        {
            float weight = isInjured ? 1f : 0f;

            _animator.SetLayerWeight(layerIndex, weight);
        }
    }

    private void DyingAnimation()
    {
        _animator.SetBool(IsDying, true);
        _coroutineRunner.StartCoroutine(Disappearing());
    }

    private IEnumerator Disappearing()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _movable.TimeDeathDisappear)
        {
            elapsedTime += Time.deltaTime;

            SetFloatFor(_renderers, EdgeKey, elapsedTime / _movable.TimeDeathDisappear);

            yield return null;
        }

        SetFloatFor(_renderers, EdgeKey, 1f);
    }

    private void SetFloatFor(SkinnedMeshRenderer[] renderers, string key, float param)
    {
        foreach (SkinnedMeshRenderer renderer in renderers)
            renderer.material.SetFloat(key, param);
    }
}
