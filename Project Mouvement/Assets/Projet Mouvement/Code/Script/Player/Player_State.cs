using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_State : MonoBehaviour
{
    public enum Player_Body {Idle,Moving,Menu}
    public enum Player_MouvementType{Jump,Slide,Run, Grappin, Fall, Null}
    public enum Player_Surface {Grounded,Wall,Air}

    protected Player_Body player_Body = Player_Body.Idle;
    protected Player_MouvementType player_MouvementType = Player_MouvementType.Null;
    protected Player_Surface player_Surface = Player_Surface.Grounded;

}
