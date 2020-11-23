using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CheckState : Player_Settings
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the player is on the ground
        Debug.DrawRay(transform.position, transform.right * -0.50001f,Color.cyan);
        if (Physics.Raycast(transform.position, -transform.up, 1.001f))
        {
            player_Surface = Player_Surface.Grounded;
        }
        else if(Physics.Raycast(transform.position,transform.right,0.6f)|| Physics.Raycast(transform.position, transform.right, -0.6f))
        {
            player_Surface = Player_Surface.Wall;

        }
        else
        {
            player_Surface = Player_Surface.Air;
        }
        Debug.Log(player_Surface);
    }

   
}
