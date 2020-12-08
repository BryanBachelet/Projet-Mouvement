using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controlle : Player_Settings
{
    [Header("Référence")]
    public Transform playerBody;

    [Header("Controller")]
    public float speed_CameraX = 180;
    public float speed_CameraY = 180;

    [Header("WallRun")]
    public float speed = 5f;
    public float angle = 30f;

    private float t;
    private Player_CheckState checkState;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        checkState = playerBody.GetComponent<Player_CheckState>();
    }


    void Update()
    {
        float MouseInputX, MouseInputY = 0;
        // Get Mouse Input
        if (!IsGamepad)
        {
            MouseInputX = Input.GetAxis("Mouse X");
            MouseInputY = -Input.GetAxis("Mouse Y");
        }
        else
        {
            MouseInputX = Input.GetAxis("Horizontal2");
            MouseInputY = Input.GetAxis("Vertical2");
        }


        // Camera Movement X & Y
        Vector3 addRot = new Vector3(MouseInputY * speed_CameraY * Time.deltaTime, MouseInputX * speed_CameraX * Time.deltaTime, 0);
        Vector3 currentRot = transform.rotation.eulerAngles + addRot;
        currentRot.x = SetNegativeAngle(currentRot.x, 275);
        // Clamp Camera Rotation Y

        //---------------------------- A Faire ------------------------

        //if (player_MotorMouvement != Player_MotorMouvement.WallRun)
        //{
        //    currentRot = new Vector3(Mathf.Clamp(currentRot.x, -90f, 90f), currentRot.y, transform.eulerAngles.z);
        //}
        //else
        //{

        //    Quaternion start = Quaternion.Euler(currentRot);
        //    currentRot = ClampYRotationCameraWallRun(start);
        //}

        //------------------------------------------------------------

        currentRot = new Vector3(Mathf.Clamp(currentRot.x, -90f, 90f), currentRot.y, transform.eulerAngles.z);

        transform.rotation = Quaternion.Euler(currentRot);
        Vector3 playerRot = new Vector3(0, currentRot.y, 0);
        if (player_MotorMouvement != Player_MotorMouvement.WallRun)
        {
            playerBody.rotation = Quaternion.Euler(playerRot);
        }
        if (player_MotorMouvement == Player_MotorMouvement.Slide)
        {
            transform.position = new Vector3(playerBody.position.x, playerBody.position.y, playerBody.position.z);
        }
        else
        {
            transform.position = new Vector3(playerBody.position.x, playerBody.position.y + 0.5f, playerBody.position.z);
        }
        // During Wall Run
        if (player_MotorMouvement == Player_MotorMouvement.WallRun)
        {
            InclinationZCamera(speed, angle, false);
        }
        else
        {
            InclinationZCamera(speed, angle, true);
        }

    }

    // A finir
    private Vector3 ClampYRotationCameraWallRun(Quaternion rot)
    {
        return rot.eulerAngles;
    }


    //Inclination Camera Z
    private void InclinationZCamera(float timer, float angle, bool inverse)
    {
        Quaternion startRot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        Quaternion endRot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle * checkState.wallSide);
        if (!inverse)
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            t += timer * Time.deltaTime;
            t = Mathf.Clamp(t, 0, 1);
            if (t == 1)
            {
                Debug.Log("Caméra Mouvement is done");
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            t -= timer * Time.deltaTime;
            t = Mathf.Clamp(t, 0, 1);
            if (t == 0)
            {
                Debug.Log("Camera Return normal rotation");
            }
        }

    }

    private float PositifAngle(float angle)
    {
        if (angle < 0)
        {
            angle = 360 - angle;
        }

        return angle;
    }

    private float ConversionAngle(float angle)
    {

        if (angle > 180)
        {
            angle = angle - 360;
        }
        if (angle < -180)
        {
            angle = angle + 360;
        }

        return angle;
    }


    //----------------------TOOL------------

    private float SetNegativeAngle(float angle, float value)
    {
        if (angle > value)
        {
            angle = -360 + angle;
        }

        return angle;
    }
}
