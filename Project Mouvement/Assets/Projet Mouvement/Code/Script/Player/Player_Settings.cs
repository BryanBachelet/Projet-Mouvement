using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Settings : Player_State
{
    protected static bool IsGamepad;


    public void StopPlayer(Rigidbody player_RigidBody)
    {
        player_RigidBody.velocity = Vector3.zero;
    }
}
