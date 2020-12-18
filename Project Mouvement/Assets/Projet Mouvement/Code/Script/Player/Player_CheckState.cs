using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CheckState : Player_Settings
{
    public float wallSide;

    void Update()
    {
        //Check if the player is on the ground

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

        Debug.DrawRay(transform.position, -transform.right * 1.25f, Color.green);

    }


    private void SetGrounded()
    {
        player_Surface = Player_Surface.Grounded;
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

}
