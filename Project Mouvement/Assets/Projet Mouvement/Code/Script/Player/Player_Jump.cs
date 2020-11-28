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

    public float jumpTimerGravity = 1;

    public float jump_CountGravity;


    public LayerMask surfaceObstacle;
    Collider surfaceHit;
    public Vector3 offsetCollider;


    private Rigidbody player_Rigid;
    private Player_CheckState checkState;
    private bool isKeyPress;


    // Start is called before the first frame update
    void Start()
    {
        player_Rigid = GetComponent<Rigidbody>();
        checkState = GetComponent<Player_CheckState>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckBorderSurface();
        if (isKeyPress)
        {
            Jump();
            isKeyPress = false;
        }

        if (jump_CountGravity > jumpTimerGravity || player_MotorMouvement == Player_MotorMouvement.Slide)
        {
            player_Rigid.AddForce(-Vector3.up * gravityForce, ForceMode.Acceleration);

        }
        else
        {
            jump_CountGravity += Time.fixedDeltaTime;
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

    public void Update()
    {
        if (jumpCount < jumpNumber || player_Surface == Player_Surface.Wall)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
            {
                isKeyPress = true;
            }
        }
    }


    private void Jump()
    {
        player_MouvementUp = Player_MouvementUp.Jump;
        jumpCount++;
        if (jump_CountGravity >= jumpTimerGravity)
        {
            player_Rigid.AddForce(Vector3.up * (jumpValue * jumpCount), ForceMode.Impulse);
        }
        else
        {
            player_Rigid.AddForce(Vector3.up * jumpValue, ForceMode.Impulse);
        }

        jump_CountGravity = 0;


    }

    public void CheckBorderSurface()
    {

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 2, surfaceObstacle))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            surfaceHit = hit.collider;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000, Color.red);
            surfaceHit = null;
        }

    }

    private void OnDrawGizmos()
    {
        if (surfaceHit != null)
        {
            Vector3 positionWorld = /*surfaceHit.transform.parent.position - surfaceHit.transform.parent.parent.position*/ /*surfaceHit.transform.position + surfaceHit.transform.parent.position + surfaceHit.transform.parent.parent.position*/ surfaceHit.transform.position;
            //Matrix4x4 rotationMatrix = Matrix4x4.TRS(positionWorld , surfaceHit.transform.parent.rotation, surfaceHit.transform.parent.lossyScale);
            //Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(new Vector3(positionWorld.x, positionWorld.y, positionWorld.z + ((surfaceHit.transform.localScale.z / 2) - 1)), new Vector3(surfaceHit.transform.localScale.x, surfaceHit.transform.localScale.y, 2));
            Gizmos.DrawWireCube(new Vector3(positionWorld.x, positionWorld.y, positionWorld.z + (-(surfaceHit.transform.localScale.z / 2) + 1)), new Vector3(surfaceHit.transform.localScale.x, surfaceHit.transform.localScale.y, 2));
            Gizmos.DrawWireCube(new Vector3(positionWorld.x + ((surfaceHit.transform.localScale.x / 2) - 1), positionWorld.y, positionWorld.z), new Vector3(2, surfaceHit.transform.localScale.y, surfaceHit.transform.localScale.z));
            Gizmos.DrawWireCube(new Vector3(positionWorld.x + (-(surfaceHit.transform.localScale.x / 2) + 1), positionWorld.y, positionWorld.z), new Vector3(2, surfaceHit.transform.localScale.y, surfaceHit.transform.localScale.z));

            Debug.Log(surfaceHit.bounds);
            Debug.Log(positionWorld);
        }

    }

}
