using UnityEngine;

public class PlayerInput
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    public Vector3 MousePosition => Input.mousePosition;
    public bool LeftMouseButtonDown => Input.GetMouseButtonDown(0);
    public bool RightMouseButtonDown => Input.GetMouseButtonDown(1);

    public float HorizontalInput => Input.GetAxisRaw(Horizontal);
    public float Verticalinput => Input.GetAxisRaw(Vertical);

    public bool HKeyPressed => Input.GetKeyDown(KeyCode.H);
}