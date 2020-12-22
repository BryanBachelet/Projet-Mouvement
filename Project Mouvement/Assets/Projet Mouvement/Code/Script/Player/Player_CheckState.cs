using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CheckState : Player_Settings
{
    public float wallSide;
    public Player_Surface currentstate;
    static public bool CameraFlexion = false;

    private Camera_Controlle s_CC;
    public float tempsEcouleAir = 0;
    public float[] palierFlexion;
    private void Start()
    {
        s_CC = Camera.main.GetComponent<Camera_Controlle>();
    }
    void Update()
    {
        //Check if the player is on the ground
        currentstate = player_Surface;
        if (Physics.Raycast(transform.position, -transform.up, 1.001f))
        {
            
            SetGrounded();
            return;
        }
        if (Physics.Raycast(transform.position, transform.right, 3) || Physics.Raycast(transform.position, -transform.right, 3))
        {
            GetSide();
            return;
        }

        player_Surface = Player_Surface.Air;
        wallSide = 0;
        if (player_Surface == Player_Surface.Air)
        {
            tempsEcouleAir += Time.deltaTime;
        }
        Debug.DrawRay(transform.position, -transform.right * 1.25f, Color.green);

    }


    private void SetGrounded()
    {
        player_Surface = Player_Surface.Grounded;
        CheckFlexionForce();
        player_MouvementUp = Player_MouvementUp.Null;
        if (player_MotorMouvement == Player_MotorMouvement.WallRun)
        {
            player_MotorMouvement = Player_MotorMouvement.Null;
        }
        wallSide = 0;
    }

    private void GetSide()
    {
        string directionString = "";
        if (Physics.Raycast(transform.position, transform.right, 3))
        {
            wallSide = 1;
            directionString = "Right";

        }
        if (Physics.Raycast(transform.position, -transform.right, 3))
        {
            wallSide = -1;
            directionString = "Left";

        }
        player_Surface = Player_Surface.Wall;
        Debug.Log("Close to " + directionString + " Wall ");

    }

    private void CheckFlexionForce()
    {
        if (tempsEcouleAir > 0 && tempsEcouleAir <= palierFlexion[0])
        {
            s_CC.offSetToMove = new Vector3(0, 0.8f, 0);
            s_CC.tempsTransition = 0.4f;
            Debug.Log("tombé au niveau 1");
        }
        else if (tempsEcouleAir >= palierFlexion[0] && tempsEcouleAir <= palierFlexion[1])
        {
            s_CC.offSetToMove = new Vector3(0, 0.5f, 0);
            s_CC.tempsTransition = 0.3f;
            Debug.Log("tombé au niveau 2");
        }
        else if (tempsEcouleAir >= palierFlexion[1] && tempsEcouleAir <= palierFlexion[2])
        {
            s_CC.offSetToMove = new Vector3(0, 0.3f, 0);
            s_CC.tempsTransition = 0.2f;
            Debug.Log("tombé au niveau 3");
        }
        else if (tempsEcouleAir >= palierFlexion[2] && tempsEcouleAir <= palierFlexion[3])
        {
            s_CC.offSetToMove = new Vector3(0, 0.1f, 0);
            s_CC.tempsTransition = 0.1f;
            Debug.Log("tombé au niveau 4");
        }
        else if (tempsEcouleAir >= palierFlexion[3])
        {
            s_CC.offSetToMove = new Vector3(0, 0f, 0);
            s_CC.tempsTransition = 0.1f;
            Debug.Log("tombé au niveau 5");
        }
        tempsEcouleAir = 0;
    }

}
