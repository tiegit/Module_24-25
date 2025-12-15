using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private GameObject _pointerView;

    private IPointerTargetOwner _pointerTargetOwner;

    private bool _isVisible = true;

    public void Initialize( IPointerTargetOwner pointerTargetOwner)
    {
        _pointerTargetOwner = pointerTargetOwner;

        Hide();
    }

    private void Update()
    {
        if (transform.position != _pointerTargetOwner.TargetPosition)
            transform.position = _pointerTargetOwner.TargetPosition;

        if (_pointerTargetOwner.HasTarget)
            ShowAt();
        else
            Hide();
    }

    private void ShowAt()
    {
        if (_isVisible)
            return;

        _pointerView.gameObject.SetActive(true);

        _isVisible = true;
    }

    private void Hide()
    {
        if (_isVisible == false)
            return;

        _pointerView.gameObject.SetActive(false);

        _isVisible = false;
    }
}