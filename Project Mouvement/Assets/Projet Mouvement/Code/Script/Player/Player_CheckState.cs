using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CheckState : Player_Settings
{
    //----- Variable -----------
    public float wallSide;
    public Player_Surface currentstate;
    public float tempsEcouleAir = 0;
    public float[] palierFlexion;

    static public bool CameraFlexion = false;

    private int frameDeactivate = 5;
    private int frameCount;
    private bool activeGroundDectection = true;

    //------ Reference-------- 
    private Camera_Controlle s_CC;
    private Rigidbody player_rigidbody;


    private void Start()
    {
        //Ranger les recherche de référence 
        s_CC = Camera.main.GetComponent<Camera_Controlle>();
        player_rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Debug.DrawRay(transform.position, -transform.up * 1.01f, Color.red);
        currentstate = player_Surface;


        
        //Check if the player is on the ground

        if (Physics.Raycast(transform.position, -transform.up, 1.3f) && activeGroundDectection)
        {
            player_rigidbody.useGravity = false;
            SetGrounded();
            return;
        }

        if (Physics.Raycast(transform.position, transform.right, 3) || Physics.Raycast(transform.position, -transform.right, 3))
        {
            GetSide();
            return;
        }

        player_Surface = Player_Surface.Air;

        player_rigidbody.useGravity = true;
        wallSide = 0;
        if (player_Surface == Player_Surface.Air)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            tempsEcouleAir += Time.deltaTime;
        }

        if (!activeGroundDectection)
        {
            if (frameCount > frameDeactivate)
            {
                activeGroundDectection = true;
            }
            else
            {
                frameCount++;
            }
        }


    }

    public void DeactiveFrameGroundDectection()
    {
        activeGroundDectection = false;
        frameCount = 0;
    }

    private void SetGrounded()
    {
        player_Surface = Player_Surface.Grounded;

        // CheckFlexionForce();
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
