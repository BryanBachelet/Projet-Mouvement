using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatutController : Player_Settings
{
    private Text textShow;

    private void Start()
    {
        textShow = GetComponent<Text>();
    }

    void Update()
    {
        if (IsGamepad)
        {
            textShow.text = "Gamepad";
        }
        else
        {
            textShow.text = "KeyBoard";
        }
    }
}
