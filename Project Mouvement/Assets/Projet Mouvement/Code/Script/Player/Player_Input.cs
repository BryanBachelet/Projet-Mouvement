using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : Player_Settings
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
    public KeyCode ChangeControllerPc = KeyCode.T;

    [Header("Gampad Control")]
    public string FrontAxisGp = "Vertical";
    public string SideAxisGp = "Vertical";
    [Space]
    public KeyCode JumpGp = KeyCode.Joystick1Button0;
    public KeyCode slideGp = KeyCode.Joystick1Button1;
    public KeyCode EchapGp = KeyCode.Joystick1Button7;
    public KeyCode ChangeControllerGp = KeyCode.Joystick1Button9;

    public void Update()
    {
        ChangeController();
    }

    //Change Type of Controller
    private void ChangeController()
    {
        if (GetInputPress(ChangeControllerPc) || GetInputPress(ChangeControllerGp))
        {
            IsGamepad = !IsGamepad;
        }
    }

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

    public float GetAxis(KeyCode Positif, KeyCode Negatif)
    {
        float axisValue = 0;
        if (Input.GetKey(Positif))
        {
            axisValue += 1;
        }
        if (Input.GetKey(Negatif))
        {
            axisValue -= 1;
        }
        axisValue = Mathf.Clamp(axisValue, -1, 1);


        return axisValue;
    }


}
