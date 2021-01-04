using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class Player_Jump : Player_Settings
{
    [Header("Jump Caracteristique")]
    public float jumpValue = 10;
    public float gravityForce = 10;
    public int jumpNumber;
    public int jumpCount;

    public float jumpTimerGravity = 1;

    public float jump_CountGravity;

    public float border_Bonus = 15;


    public LayerMask surfaceObstacle;
    Collider surfaceHit;
    public Vector3 offsetCollider;
    public float offsetJump = 3;


    private bool isKeyPress;

    public bool checkGrounded = false;

    [EventRef]
    public string groundedSound;


    private Rigidbody player_Rigid;
    private Player_CheckState checkState;
    private Player_WallRun player_WallRun;
    private Camera_Controlle myCameraControl;

    // Start is called before the first frame update
    void Start()
    {
        player_Rigid = GetComponent<Rigidbody>();
        checkState = GetComponent<Player_CheckState>();
        player_WallRun = GetComponent<Player_WallRun>();
        myCameraControl = Camera.main.GetComponent<Camera_Controlle>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player_MotorMouvement != Player_MotorMouvement.WallRun)
        {
            CheckBorderSurface();
            //Event Input
            if (isKeyPress)
            {
                Jump();
                isKeyPress = false;
            }
            // Gravity Fall
            if (player_MouvementUp == Player_MouvementUp.Jump || player_MouvementUp == Player_MouvementUp.Fall)
            {
                if (jump_CountGravity > jumpTimerGravity || player_MotorMouvement == Player_MotorMouvement.Slide)
                {
                    player_Rigid.AddForce(-Vector3.up * gravityForce, ForceMode.Acceleration);
                }
                else
                {
                    //CountDown Gravity
                    jump_CountGravity += Time.fixedDeltaTime;
                }
            }
            if (jump_CountGravity > jumpTimerGravity || player_MotorMouvement == Player_MotorMouvement.Slide)
            {

                player_Rigid.AddForce(-Vector3.up * gravityForce, ForceMode.Acceleration);
            }


        }

    }

    public void Update()
    {


        if (jumpCount < jumpNumber || player_Surface == Player_Surface.Wall)
        {
            if (player_MotorMouvement != Player_MotorMouvement.WallRun)
            {
                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
                {
                    isKeyPress = true;
                }
            }
        }

        if (player_MouvementUp == Player_MouvementUp.Fall && player_Surface == Player_Surface.Wall && player_MotorMouvement != Player_MotorMouvement.WallRun)
        {
            player_WallRun.ActivateWallRun();
            return;
        }

        // Change State Fall
        if (player_Rigid.velocity.y <= -3f)
        {
            player_MouvementUp = Player_MouvementUp.Fall;

        }

        //Reset Mouvement Up State & Jump
        if (player_MouvementUp == Player_MouvementUp.Null)
        {
            jumpCount = 0;
            jump_CountGravity = 0;
            player_MouvementUp = Player_MouvementUp.Null;
        }

        
    }


    private void Jump()
    {
        myCameraControl.offSetToMove = new Vector3(0, 1, 0);
        // Set Player Jump State 
        player_MouvementUp = Player_MouvementUp.Jump;
        // Add jump Count 
        jumpCount++;

        // Check If It's border Jump
        if (JumpBoostBorder())
        {
            // Add Force to jump & Cancel gravity + Border Bonus
            player_Rigid.AddForce(Vector3.up * (jumpValue + Mathf.Abs(player_Rigid.velocity.y) + border_Bonus), ForceMode.Impulse);

        }
        else
        {
            player_Rigid.AddForce(Vector3.up * (jumpValue + Mathf.Abs(player_Rigid.velocity.y)), ForceMode.Impulse);
        }
        if (jumpCount < jumpNumber || player_Surface == Player_Surface.Wall)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
            {
                isKeyPress = true;
            }
        }
        if (player_Surface == Player_Surface.Grounded)
        {
            if (!checkGrounded)
            {

                checkGrounded = true;
                RuntimeManager.PlayOneShot(groundedSound, transform.position);

            }
        }
        else
        {

            jump_CountGravity = 0;

        }
        checkGrounded = false;


    }

    public void Jump(Vector3 dir, float power)
    {
        
        // Set Player Jump State 
        player_MouvementUp = Player_MouvementUp.Jump;
        // Add jump Count 
        jumpCount++;
        // Check If It's border Jump
        if (JumpBoostBorder())
        {
            // Add Force to jump & Cancel gravity + Border Bonus
            player_Rigid.AddForce(dir.normalized * (power + Mathf.Abs(player_Rigid.velocity.y) + border_Bonus), ForceMode.Impulse);
        }
        else
        {
            player_Rigid.AddForce(dir.normalized * (power + Mathf.Abs(player_Rigid.velocity.y)), ForceMode.Impulse);
        }
        jump_CountGravity = 0;
        checkGrounded = false;
    }

    public void RestJumpCount()
    {
        jumpCount = 0;
        Debug.Log("Jump Count Reset !");
    }

    public void CheckBorderSurface()
    {

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 2, surfaceObstacle))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            surfaceHit = hit.collider;

            //Debug.Log("" + hit.collider.name);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000, Color.red);
            surfaceHit = null;
        }

    }

    public bool JumpBoostBorder()
    {
        if (surfaceHit != null)
        {

            Transform myHitTransform = surfaceHit.transform;
            if (surfaceHit.transform.position.z + ((myHitTransform.localScale.z - offsetJump) / 2) < transform.position.z && transform.position.z < ((myHitTransform.localScale.z) / 2))
            {
                return true;
                //Debug.Log("In green zone");
            }
            else if (surfaceHit.transform.position.x + ((myHitTransform.localScale.x - offsetJump) / 2) < transform.position.x && transform.position.x < ((myHitTransform.localScale.x) / 2))
            {
                return true;
                //Debug.Log("In green zone");
            }
            else if (surfaceHit.transform.position.z - ((myHitTransform.localScale.z - offsetJump) / 2) > transform.position.z && transform.position.z > ((myHitTransform.localScale.z) / 2))
            {
                return true;
                //Debug.Log("In green zone");
            }
            else if (surfaceHit.transform.position.x - ((myHitTransform.localScale.x - offsetJump) / 2) > transform.position.x && transform.position.x > ((myHitTransform.localScale.x) / 2))
            {
                return true;
                //Debug.Log("In green zone");
            }
            else
            {
                return false;
            }

        }
        else
        {
            return false;
        }

    }

    private void OnDrawGizmos()
    {
        if (surfaceHit != null)
        {
            Vector3 positionWorld = /*surfaceHit.transform.parent.position - surfaceHit.transform.parent.parent.position*/ /*surfaceHit.transform.position + surfaceHit.transform.parent.position + surfaceHit.transform.parent.parent.position*/ surfaceHit.transform.position;
            //Matrix4x4 rotationMatrix = Matrix4x4.TRS(positionWorld , surfaceHit.transform.parent.rotation, surfaceHit.transform.parent.lossyScale);
            //Gizmos.matrix = rotationMatrix;
            MeshFilter myMesh = surfaceHit.GetComponent<MeshFilter>();
            Transform myHitTransform = surfaceHit.transform;

            Gizmos.color = Color.green;
            Gizmos.DrawMesh(myMesh.sharedMesh, 0, myHitTransform.position, myHitTransform.rotation, new Vector3(myHitTransform.localScale.x, myHitTransform.localScale.y, myHitTransform.localScale.z));
            Gizmos.color = Color.red;
            Gizmos.DrawMesh(myMesh.sharedMesh, 0, myHitTransform.position, myHitTransform.rotation, new Vector3(myHitTransform.localScale.x - offsetJump, myHitTransform.localScale.y, myHitTransform.localScale.z - offsetJump));
            Gizmos.DrawLine(new Vector3(surfaceHit.transform.position.x, surfaceHit.transform.position.y + 1, surfaceHit.transform.position.z), new Vector3(surfaceHit.transform.position.x /*+ myHitTransform.localScale.x*/, surfaceHit.transform.position.y + 1, surfaceHit.transform.position.z + ((myHitTransform.localScale.z - offsetJump) / 2)));
            Gizmos.DrawLine(new Vector3(surfaceHit.transform.position.x, surfaceHit.transform.position.y + 1, surfaceHit.transform.position.z), new Vector3(surfaceHit.transform.position.x + ((myHitTransform.localScale.x - offsetJump) / 2), surfaceHit.transform.position.y + 1, surfaceHit.transform.position.z /*+ (myHitTransform.localScale.z / 2)*/));
            Gizmos.DrawLine(new Vector3(surfaceHit.transform.position.x, surfaceHit.transform.position.y + 1, surfaceHit.transform.position.z), new Vector3(surfaceHit.transform.position.x /*+ myHitTransform.localScale.x*/, surfaceHit.transform.position.y + 1, surfaceHit.transform.position.z - ((myHitTransform.localScale.z - offsetJump) / 2)));
            Gizmos.DrawLine(new Vector3(surfaceHit.transform.position.x, surfaceHit.transform.position.y + 1, surfaceHit.transform.position.z), new Vector3(surfaceHit.transform.position.x - ((myHitTransform.localScale.x - offsetJump) / 2), surfaceHit.transform.position.y + 1, surfaceHit.transform.position.z /*+ (myHitTransform.localScale.z / 2)*/));
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(surfaceHit.transform.position.x, surfaceHit.transform.position.y + 2, surfaceHit.transform.position.z), new Vector3(surfaceHit.transform.position.x /*+ myHitTransform.localScale.x*/, surfaceHit.transform.position.y + 2, surfaceHit.transform.position.z + ((myHitTransform.localScale.z) / 2)));
            Gizmos.DrawLine(new Vector3(surfaceHit.transform.position.x, surfaceHit.transform.position.y + 2, surfaceHit.transform.position.z), new Vector3(surfaceHit.transform.position.x + ((myHitTransform.localScale.x) / 2), surfaceHit.transform.position.y + 2, surfaceHit.transform.position.z /*+ (myHitTransform.localScale.z / 2)*/));
            Gizmos.DrawLine(new Vector3(surfaceHit.transform.position.x, surfaceHit.transform.position.y + 2, surfaceHit.transform.position.z), new Vector3(surfaceHit.transform.position.x /*+ myHitTransform.localScale.x*/, surfaceHit.transform.position.y + 2, surfaceHit.transform.position.z - ((myHitTransform.localScale.z) / 2)));
            Gizmos.DrawLine(new Vector3(surfaceHit.transform.position.x, surfaceHit.transform.position.y + 2, surfaceHit.transform.position.z), new Vector3(surfaceHit.transform.position.x - ((myHitTransform.localScale.x) / 2), surfaceHit.transform.position.y + 2, surfaceHit.transform.position.z /*+ (myHitTransform.localScale.z / 2)*/));
            //Gizmos.DrawWireCube(new Vector3(positionWorld.x, positionWorld.y, positionWorld.z + ((surfaceHit.transform.localScale.z / 2) - 1)), new Vector3(surfaceHit.transform.localScale.x, surfaceHit.transform.localScale.y, 2));
            //Gizmos.DrawWireCube(new Vector3(positionWorld.x, positionWorld.y, positionWorld.z + (-(surfaceHit.transform.localScale.z / 2) + 1)), new Vector3(surfaceHit.transform.localScale.x, surfaceHit.transform.localScale.y, 2));
            //Gizmos.DrawWireCube(new Vector3(positionWorld.x + ((surfaceHit.transform.localScale.x / 2) - 1), positionWorld.y, positionWorld.z), new Vector3(2, surfaceHit.transform.localScale.y, surfaceHit.transform.localScale.z));
            //Gizmos.DrawWireCube(new Vector3(positionWorld.x + (-(surfaceHit.transform.localScale.x / 2) + 1), positionWorld.y, positionWorld.z), new Vector3(2, surfaceHit.transform.localScale.y, surfaceHit.transform.localScale.z));
            if (surfaceHit.transform.position.z + ((myHitTransform.localScale.z - offsetJump) / 2) < transform.position.z && transform.position.z < ((myHitTransform.localScale.z) / 2))
            {
                //Debug.Log("In green zone");
            }
            else if (surfaceHit.transform.position.x + ((myHitTransform.localScale.x - offsetJump) / 2) < transform.position.x && transform.position.x < ((myHitTransform.localScale.x) / 2))
            {
                //Debug.Log("In green zone");
            }
            else
            {
                //Debug.Log("In red zone");
            }
            //Debug.Log((myHitTransform.localScale.x - (myHitTransform.localScale.x - 2)) + transform.position)
            //Debug.Log(surfaceHit.bounds);
            //Debug.Log(positionWorld);
        }

    }

}
