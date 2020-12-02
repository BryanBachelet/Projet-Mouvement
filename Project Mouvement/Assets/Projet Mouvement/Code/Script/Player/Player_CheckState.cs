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
            player_Surface = Player_Surface.Grounded;
            player_MouvementUp = Player_MouvementUp.Null;
            wallSide = 0;
            return;
        }
         if (Physics.Raycast(transform.position, transform.right, 1.25f) || Physics.Raycast(transform.position, -transform.right, 1.25f))
        {

            GetSide();
            return;
        }
        else
        {
            player_Surface = Player_Surface.Air;
            wallSide = 0;
        }
        Debug.DrawRay(transform.position, -transform.right * 1.25f,Color.green);
    }

    public void GetSide()
    {
        if (Physics.Raycast(transform.position, transform.right, 1.25f)) 
        {
            wallSide = 1;
           
        }
        if(Physics.Raycast(transform.position, -transform.right, 1.25f))
        {
            wallSide = -1;

        }
        player_Surface = Player_Surface.Wall;
    }

}
