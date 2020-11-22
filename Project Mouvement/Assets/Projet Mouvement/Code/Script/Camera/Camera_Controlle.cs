using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controlle : MonoBehaviour
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
        // Get Mouse Input
        float MouseInputX = Input.GetAxis("Mouse X");
        float MouseInputY = -Input.GetAxis("Mouse Y");
     
        // Camera Movement X & Y
        Vector3 addRot = new Vector3(MouseInputY * speed_CameraY * Time.deltaTime, MouseInputX * speed_CameraX * Time.deltaTime, 0);
        Vector3 currentRot = transform.rotation.eulerAngles + addRot;

        currentRot.x = SetNegativeAngle(currentRot.x);
        
        // Clamp Camera Rotation Y
        currentRot = new Vector3(Mathf.Clamp(currentRot.x, -90f, 90f), currentRot.y, 0);

        transform.rotation = Quaternion.Euler(currentRot);
        Vector3 playerRot = new Vector3(playerBody.transform.rotation.eulerAngles.x, currentRot.y, playerBody.transform.rotation.eulerAngles.z);
        playerBody.rotation = Quaternion.Euler(playerRot);
        transform.position = new Vector3(playerBody.position.x, transform.position.y, playerBody.position.z);
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
