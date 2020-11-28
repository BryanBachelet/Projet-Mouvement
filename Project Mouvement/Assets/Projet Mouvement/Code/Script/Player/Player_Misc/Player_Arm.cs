using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Arm : MonoBehaviour
{
    public float arm_Timer;
    public float arm_Ecart;
    public bool invert;
    public float arm_Counter;
    public bool isUp;
    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        GetTimer();
    }

    public void GetTimer()
    {

        if (arm_Counter >= arm_Timer)
        {
            isUp = false;
        }
        if (arm_Counter <= 0)
        {
            isUp = true;
        }

        if (isUp)
        {
            arm_Counter += Time.deltaTime;
        }
        else
        {
            arm_Counter -= Time.deltaTime;
        }
    }

    public void MoveArm()
    {
        float t = arm_Counter / arm_Timer;
        if (invert)
        {
            transform.localPosition = Vector3.Lerp(startPosition, startPosition - Vector3.up * arm_Ecart, t);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(startPosition, startPosition + Vector3.up * arm_Ecart, t);
        }
    }
}
