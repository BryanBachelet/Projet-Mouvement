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
        Debug.Log(player_Surface);
        if (Physics.Raycast(transform.position, -transform.up, 1.001f))
        {
            player_Surface = Player_Surface.Grounded;
            player_MouvementUp = Player_MouvementUp.Null;
           
            return;
        }
        else if(Physics.Raycast(transform.position,transform.right,1f)|| Physics.Raycast(transform.position, transform.right, -1f))
        {
            player_Surface = Player_Surface.Wall;
            return;
        }
        else
        {
            player_Surface = Player_Surface.Air;
        }
    }

   
}
