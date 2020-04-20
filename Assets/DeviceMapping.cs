using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceMapping : MonoBehaviour
{
    public KeyCode HorizontalLeft;
    public KeyCode HorizontalRight;

    public KeyCode VerticalUp;
    public KeyCode VerticalDown;

    public KeyCode Zbutton;
    public KeyCode Xbutton;
    public KeyCode SpaceButton;

    private void Awake()
    {
        DeviceInput.SetMapping(this);
    }
}
