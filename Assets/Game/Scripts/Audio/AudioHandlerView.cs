using UnityEngine;
using UnityEngine.UI;

public class AudioHandlerView : MonoBehaviour
{
    [SerializeField] private Button _musicOnButton;
    [SerializeField] private Button _musicOffButton;

    private AudioHandler _audioHandler;
    private bool _previousMute;

    public void Initialize(AudioHandler audioHandler)
    {
        _audioHandler = audioHandler;

        UpdateButtons();
    }

    public void CustomUpdate()
    {
        bool currentMute = _audioHandler.IsMute;

        if (_previousMute != currentMute)
        {
            _previousMute = currentMute;
            UpdateButtons();
        }
    }

    private void UpdateButtons()
    {
        _musicOnButton.interactable = _audioHandler.IsMute;
        _musicOffButton.interactable = !_audioHandler.IsMute;
    }
}
