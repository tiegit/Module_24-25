using UnityEngine;
using UnityEngine.Audio;

public class AudioExample : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioMixer _audioMixer;

    private AudioHandler _audioHandler;

    private void Awake()
    {
        _audioHandler = new AudioHandler(_audioMixer);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _audioSource.pitch = Random.Range(1f, 1.8f);
            _audioSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _audioHandler.OffMusic();
            Debug.Log($"Music is on: " + _audioHandler.IsMusicOn());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _audioHandler.OnMusic();
            Debug.Log($"Music is on: " + _audioHandler.IsMusicOn());
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _audioHandler.OffSounds();
            Debug.Log($"Sound is on: " + _audioHandler.IsSoundOn());
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _audioHandler.OnSounds();
            Debug.Log($"Sound is on: " + _audioHandler.IsSoundOn());
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _audioHandler.SwitchToDefeatEffect();
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _audioHandler.SwitchToBaseEffect();
        }
    }
}
