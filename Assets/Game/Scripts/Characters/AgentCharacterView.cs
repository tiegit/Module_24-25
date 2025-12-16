using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AgentCharacterView : MonoBehaviour, IDamageAnimator
{
    private readonly int WalkingVelocityKey = Animator.StringToHash("Velocity");
    private readonly int JumpSpeedKey = Animator.StringToHash("JumpSpeed");
    private readonly int InJumpProcessKey = Animator.StringToHash("InJumpProcess");
    private readonly int IsExplodedKey = Animator.StringToHash("IsExploded");
    private readonly int IsDyingKey = Animator.StringToHash("IsDying");

    private readonly string InjuredLayer = "Injured Layer";
    private readonly string JumpLayer = "Jump Layer";

    private Animator _animator;
    private AgentCharacter _character;
    private bool _currentJumpStatus;
    private float _originalSpeed;
    private AnimationClip _jumpClip;

    public void Initialize(AgentCharacter character)
    {
        _character = character;

        _animator = GetComponent<Animator>();


        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Standing Jump")
            {
                _jumpClip = clip;

                break;
            }
        }

        _originalSpeed = _jumpClip.apparentSpeed;
        _currentJumpStatus = _character.InJumpProcess;
    }

    private void Update()
    {
        if (_character.CurrentVelocity.magnitude > 0.05f)
            StartRunning(_character.CurrentVelocity.magnitude / _character.MaxSpeed);
        else
            StopRunning();

        if (_currentJumpStatus != _character.InJumpProcess)
        {
            _currentJumpStatus = _character.InJumpProcess;

            ToggleJumpLayer(_currentJumpStatus);

            _animator.SetBool(InJumpProcessKey, _currentJumpStatus);

            if (_currentJumpStatus)
            {
                if (_jumpClip.length > 0)
                {
                    float newSpeed = _jumpClip.length / _character.JumpDuration;

                    _animator.SetFloat(JumpSpeedKey, newSpeed);
                }
            }
            else
            {
                _animator.SetFloat(JumpSpeedKey, _originalSpeed);
            }
        }
    }

    public void TakeDamage() => _animator.SetTrigger(IsExplodedKey);

    public void SetInjuredLayer()
    {
        int layerIndex = _animator.GetLayerIndex(InjuredLayer);

        if (layerIndex != -1)
            _animator.SetLayerWeight(layerIndex, 1);
    }

    public void ResumeMove() => _character.ResumeMove(); // это сделано чтобы с помощью ключа-события из анимации дернуть окончание анимации

    public void DyingAnimation() => _animator.SetBool(IsDyingKey, true);

    private void StopRunning() => _animator.SetFloat(WalkingVelocityKey, 0);

    private void StartRunning(float speedRatio) => _animator.SetFloat(WalkingVelocityKey, speedRatio);

    private void ToggleJumpLayer(bool value)
    {
        int layerIndex = _animator.GetLayerIndex(JumpLayer);

        if (layerIndex != -1)
        {
            if (value)
                _animator.SetLayerWeight(layerIndex, 1);
            else
                _animator.SetLayerWeight(layerIndex, 0);
        }
    }
}