using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Jump : Player_Settings
{
    [Header("Jump Caracteristique")]
    // public float jumpValue ;
    public int jumpNumber;
    public int jumpCount;
    //  public float jumpUpTime;
    public float jump_HeightMax = 10;
    public float jump_speed = 7.5f;


    private bool isFall;
    private float jumpUpCount;
    private Rigidbody player_Rigid;
    private Player_CheckState checkState;
    // Start is called before the first frame update
    void Start()
    {
        player_Rigid = GetComponent<Rigidbody>();
        checkState = GetComponent<Player_CheckState>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (jumpCount < jumpNumber )
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Space))
            {
                player_MouvementUp = Player_MouvementUp.Jump;

                player_Rigid.AddForce(Vector3.up * jump_HeightMax, ForceMode.Impulse);

                jumpCount++;

            }
        }

        if (player_Rigid.velocity.y <= -1f)
        {
            player_MouvementUp = Player_MouvementUp.Fall;
        }
        if (player_MouvementUp == Player_MouvementUp.Null)
        {

            jumpCount = 0;
            player_MouvementUp = Player_MouvementUp.Null;
        }


    }


}
