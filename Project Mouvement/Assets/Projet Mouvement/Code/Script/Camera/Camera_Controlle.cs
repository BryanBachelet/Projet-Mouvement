using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Slider sensitivitySlider;

    private float t;
    private Player_CheckState checkState;

    public Vector3 offSetToMove = Vector3.zero;
    public Vector3 offSetCurrent = Vector3.zero;
    public float tempsTransition;
    public bool debug;
    void Start()
    {
        transform.position = playerBody.position + offSetCurrent;
        Cursor.lockState = CursorLockMode.Locked;
        checkState = playerBody.GetComponent<Player_CheckState>();
    }


    void Update()
    {

        if (!MacroFunction.isPause)
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


            // Clamp Camera Rotation Y

            currentRot.x = SetNegativeAngle(currentRot.x, 270);
            if (player_MotorMouvement == Player_MotorMouvement.WallRun)
            {
                currentRot = ClampYRotationCameraWallRun(currentRot);
            }
            else
            {
                currentRot = new Vector3(Mathf.Clamp(currentRot.x, -90f, 90f), currentRot.y, 0);

            }


            Vector3 playerRot = new Vector3(playerBody.transform.rotation.eulerAngles.x, currentRot.y, playerBody.transform.rotation.eulerAngles.z);

            transform.rotation = Quaternion.Euler(currentRot);
            if (player_MotorMouvement != Player_MotorMouvement.WallRun || player_MotorMouvement != Player_MotorMouvement.Slide )
            {
                //playerBody.rotation = Quaternion.Euler(playerRot);
                playerBody.rotation = Quaternion.Euler(playerRot);
                // playerBody.Rotate(transform.up,addRot.y,Space.Self) ; 

            }
        }
        transform.position = playerBody.position + offSetCurrent;

        // During Wall Run
        if (player_MotorMouvement == Player_MotorMouvement.WallRun)
        {
            InclinationZCamera(speed, angle, false);
        }
        else
        {
            InclinationZCamera(speed, angle, true);
        }
        if (debug) Debug.Log("current offSet is : " + offSetCurrent + " And offSet to move is : " + offSetToMove);

        if (Vector3.Distance(offSetCurrent, offSetToMove) > 0.01f)
        {
            LerpingToNewPos();
        }
        else
        {
            if (player_MotorMouvement != Player_MotorMouvement.Slide)
            {
                offSetToMove = new Vector3(0, 1, 0);
            }
        }

        if (player_MotorMouvement == Player_MotorMouvement.Slide)
        {
            transform.position = playerBody.position + offSetCurrent + (Vector3.down * 0.5f);

        }

    }

    // A finir
    private Vector3 ClampYRotationCameraWallRun(Vector3 rot)
    {
        float angle = Vector3.SignedAngle(playerBody.transform.forward, transform.forward, Vector3.up);
        if (debug) Debug.LogWarning("Angle = " + angle);
        if (Mathf.Abs(angle) > 90)
        {
            angle = Mathf.Clamp(angle, -90f, 90f);
            rot = new Vector3(Mathf.Clamp(rot.x, -90f, 90f), playerBody.transform.eulerAngles.y + angle, rot.z);
        }
        else
        {
            rot = new Vector3(Mathf.Clamp(rot.x, -90f, 90f), rot.y, rot.z);
        }
        if (debug) Debug.LogWarning("Angle Clamp = " + angle);

        return rot;
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
                if (debug) Debug.Log("Caméra Mouvement is done");
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            t -= timer * Time.deltaTime;
            t = Mathf.Clamp(t, 0, 1);
            if (t == 0)
            {
                //----Debug.Log("Camera Return normal rotation");
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

    public void UpdateMouseSensitivity()
    {
        speed_CameraX = 180 * sensitivitySlider.value;
        speed_CameraY = 180 * sensitivitySlider.value;
    }

    public void LerpingToNewPos()
    {
        offSetCurrent = Vector3.Lerp(offSetCurrent, offSetToMove, tempsTransition);

    }
}
