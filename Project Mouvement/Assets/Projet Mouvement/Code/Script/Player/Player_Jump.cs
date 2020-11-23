using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Jump : Player_Settings
{
    [Header("Jump Caracteristique")]
    public float jumpValue = 10;
    public float gravityForce = 10;
    public int jumpNumber;
    public int jumpCount;

    public float jumpTimerGravity =1;

    private float jump_CountGravity;  
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

        if (jumpCount < jumpNumber || player_Surface == Player_Surface.Wall)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        if (jump_CountGravity > jumpTimerGravity)
        {
            player_Rigid.AddForce(-Vector3.up * gravityForce, ForceMode.Acceleration);
            
        }
        else
        {
            jump_CountGravity += Time.deltaTime;
        }

        if (player_Rigid.velocity.y <= -1f)
        {
            player_MouvementUp = Player_MouvementUp.Fall;
        }
        if (player_MouvementUp == Player_MouvementUp.Null)
        {

            jumpCount = 0;
            jump_CountGravity = 0;
            player_MouvementUp = Player_MouvementUp.Null;
        }


    }
     
    private void Jump()
    {
        player_MouvementUp = Player_MouvementUp.Jump;
        player_Rigid.AddForce(Vector3.up * jumpValue, ForceMode.Impulse);
        jumpCount++;
        
    }

}
