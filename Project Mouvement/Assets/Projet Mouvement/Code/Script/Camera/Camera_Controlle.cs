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


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        float MouseInputX , MouseInputY = 0;
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

        currentRot.x = SetNegativeAngle(currentRot.x);
        
        // Clamp Camera Rotation Y
        currentRot = new Vector3(Mathf.Clamp(currentRot.x, -90f, 90f), currentRot.y, 0);

        transform.rotation = Quaternion.Euler(currentRot);
        Vector3 playerRot = new Vector3(playerBody.transform.rotation.eulerAngles.x, currentRot.y, playerBody.transform.rotation.eulerAngles.z);
        playerBody.rotation = Quaternion.Euler(playerRot);
        if(player_MotorMouvement == Player_MotorMouvement.Slide)
        {
            transform.position = new Vector3(playerBody.position.x, playerBody.position.y, playerBody.position.z);

        }
        else
        {
            transform.position = new Vector3(playerBody.position.x, playerBody.position.y + 0.5f, playerBody.position.z);
        }

    }

    private float SetNegativeAngle(float angle)
    {
        if (angle > 260)
        {
            angle = -360 + angle;
        }

        return angle;
    }
}
