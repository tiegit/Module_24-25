using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private AudioHandler _audioHandler;

    public void Initialize(AudioHandler audioHandler) => _audioHandler = audioHandler;

    public void CustomUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _audioSource.pitch = Random.Range(1f, 1.8f);
            _audioSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
            OffMusic();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            OnMusic();

        if (Input.GetKeyDown(KeyCode.Alpha4))
            OffSounds();

        if (Input.GetKeyDown(KeyCode.Alpha5))
            OnSounds();

        if (Input.GetKeyDown(KeyCode.Alpha6))
            _audioHandler.SwitchToDefeatEffect();

        if (Input.GetKeyDown(KeyCode.Alpha7))
            _audioHandler.SwitchToBaseEffect();
    }
    public void MusicOffButtonClicked()
    {
        OffMusic();
        OffSounds();
    }

    public void MusicOnButtonClicked()
    {
        OnMusic();
        OnSounds();
    }

    private void OnSounds()
    {
        _audioHandler.OnSounds();
        Debug.Log($"Sound is on: " + _audioHandler.IsSoundOn);
    }

    private void OffSounds()
    {
        _audioHandler.OffSounds();
        Debug.Log($"Sound is on: " + _audioHandler.IsSoundOn);
    }

    private void OnMusic()
    {
        _audioHandler.OnMusic();
        Debug.Log($"Music is on: " + _audioHandler.IsMusicOn);
    }

    private void OffMusic()
    {
        _audioHandler.OffMusic();
        Debug.Log($"Music is on: " + _audioHandler.IsMusicOn);
    }
}
