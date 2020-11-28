using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_State : MonoBehaviour
{
    public enum Player_Body { Idle, Moving, Menu }
    public enum Player_MouvementUp { Jump, Grappin, Fall, Null }
    public enum Player_MotorMouvement { Run, Slide, WallRun, Null }
    public enum Player_Surface { Grounded, Wall, Air }

    protected static Player_Body player_Body = Player_Body.Idle;
    protected static Player_MouvementUp player_MouvementUp = Player_MouvementUp.Null;
    protected static Player_MotorMouvement player_MotorMouvement = Player_MotorMouvement.Run;
    protected static Player_Surface player_Surface = Player_Surface.Grounded;
   

}
