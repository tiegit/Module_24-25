using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AgentCharacterJumpView : IUpdatable
{
    private const string JumpAnimationName = "Standing Jump";

    private readonly int JumpSpeedKey = Animator.StringToHash("JumpSpeed");
    private readonly int InJumpProcessKey = Animator.StringToHash("InJumpProcess");

    private readonly string JumpLayer = "Jump Layer";

    private Animator _animator;
    private IJumper _jumper;
    private bool _currentJumpStatus;
    private float _originalJumpClipSpeed;
    private AnimationClip _jumpClip;

    public AgentCharacterJumpView(Animator animator, IJumper jumper)
    {
        _animator = animator;
        _jumper = jumper;

        _currentJumpStatus = _jumper.InJumpProcess;

        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == JumpAnimationName)
            {
                _jumpClip = clip;

                break;
            }
        }

        _originalJumpClipSpeed = _jumpClip.apparentSpeed;
    }

    public void Update(float deltaTime)
    {
        if (_currentJumpStatus != _jumper.InJumpProcess)
        {
            _currentJumpStatus = _jumper.InJumpProcess;

            ToggleJumpLayer(_currentJumpStatus);

            _animator.SetBool(InJumpProcessKey, _currentJumpStatus);

            if (_currentJumpStatus)
            {
                if (_jumpClip.length > 0)
                {
                    float newSpeed = _jumpClip.length / _jumper.JumpDuration;

                    _animator.SetFloat(JumpSpeedKey, newSpeed);
                }
            }
            else
            {
                _animator.SetFloat(JumpSpeedKey, _originalJumpClipSpeed);
            }
        }
    }

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