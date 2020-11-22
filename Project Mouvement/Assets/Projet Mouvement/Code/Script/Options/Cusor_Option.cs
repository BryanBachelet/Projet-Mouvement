using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cusor_Option : MonoBehaviour
{
    [Header("Parameter")]
    public bool isVisible = false;
    public CursorLockMode cursorLockMode = CursorLockMode.Locked;
    [Header("Cursor Style")]
    public CursorMode cursorMode;

    // Start is called before the first frame update
    void Start()
    {
        SetCursorParameter();
    }

    public void SetCursorParameter()
    {
        Cursor.visible = isVisible;
        Cursor.lockState = cursorLockMode;
    }

    public void SetCursorStyle(Texture2D texture, Vector2 hotspot, CursorMode mode)
    {
        Cursor.SetCursor(texture, hotspot, mode);
        cursorMode = mode;
    }
}
