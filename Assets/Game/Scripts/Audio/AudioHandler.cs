using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler
{
    private const float OffVolumeValue = -80f;
    private const float OnVolumeValue = 0f;

    private const string MusicKey = "MusicVolume";
    private const string SoundsKey = "SoundsVolume";

    private const string BaseSnapshotKey = "Snapshot";
    private const string DefeatSnapshotKey = "DefeatEffect";

    private AudioMixer _audioMixer;

    public AudioHandler(AudioMixer audioMixer)
    {
        _audioMixer = audioMixer;
    }

    public bool IsMusicOn() => IsVolumeOn(MusicKey);
    public bool IsSoundOn() => IsVolumeOn(SoundsKey);

    public void OffMusic() => OffVolume(MusicKey);
    public void OnMusic()=> OnVolume(MusicKey);

    public void OffSounds() => OffVolume(SoundsKey);
    public void OnSounds()=> OnVolume(SoundsKey);

    public void SwitchToDefeatEffect()
    {
        AudioMixerSnapshot defeatEffect = _audioMixer.FindSnapshot(DefeatSnapshotKey);
        defeatEffect.TransitionTo(0.5f);
    }

    public void SwitchToBaseEffect()
    {
        AudioMixerSnapshot baseSnapshot = _audioMixer.FindSnapshot(BaseSnapshotKey);
        baseSnapshot.TransitionTo(0.5f);
    }

    private bool IsVolumeOn(string key) => _audioMixer.GetFloat(key, out float volume) && Mathf.Abs(volume - OnVolumeValue) <= 0.01f;

    private void OnVolume(string key) => _audioMixer.SetFloat(key, OnVolumeValue);

    private void OffVolume(string key) => _audioMixer.SetFloat(key, OffVolumeValue);
}
