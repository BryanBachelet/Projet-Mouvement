using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StartSetting : Player_Settings
{
    [Header("Start Game Setting")]
     public bool Gamepad = true;
    
    void Awake()
    {
        IsGamepad = Gamepad;
    }

    
}
