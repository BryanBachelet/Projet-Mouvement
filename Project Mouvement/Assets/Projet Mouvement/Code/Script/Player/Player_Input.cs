using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    [Header("PC Control")]
    public KeyCode forwardPc = KeyCode.Z;
    public KeyCode backPc = KeyCode.S;
    public KeyCode rightPc = KeyCode.Q;
    public KeyCode leftPc = KeyCode.D;
    [Space]

    public KeyCode JumpPc = KeyCode.Space;
    public KeyCode SlidePc = KeyCode.LeftControl;
    public KeyCode EchapPc = KeyCode.Escape;

    [Header("Gampad Control")]
    public string FrontAxisGp = "Vertical";
    public string SideAxisGp = "Vertical";
    [Space]
    public KeyCode JumpGp = KeyCode.Joystick1Button0;
    public KeyCode slideGp = KeyCode.Joystick1Button1;
    public KeyCode EchapGp = KeyCode.Joystick1Button7;


    public bool GetInputPress(KeyCode inputCheck)
    {
        bool inputState = false;
        inputState = Input.GetKey(inputCheck);
        return inputState; 
    }

    public float GetAxeValue(string axeCheck)
    {
        float value = 0;
        value = Input.GetAxis(axeCheck);
        return value; 
    }


}
