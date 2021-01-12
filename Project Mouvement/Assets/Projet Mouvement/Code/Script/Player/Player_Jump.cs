using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player_Speed))]
[RequireComponent(typeof(Player_Input))]
public class Player_Jump : Player_Settings
{

    //---- Variable -----

    [Header("Debug")]
    [SerializeField]
    private bool activeDebug = false;
    public Player_MouvementUp player;


    [Header("Jump Caracteristique")]
    public float heightJumpForce = 10f;
    public float forwardJumpForce = 15f;

    public float border_Bonus = 5f;
    public int jumpNumber = 2;
    public int jumpCount;

    [Header("Air Control")]
    public float speedAirControl = 10f;
    public float maxSpeedOfJump = 25f;

    [Header("Jump Boost")]
    public float offsetJump = 3;
    public LayerMask surfaceObstacle;


    //----- System Variable --- 
    private bool callJump = false;
    private float front = 0;
    private float side = 0;

    private bool isReset = false;
    private bool inputReset;
    private bool isKeyPress;

    private Vector3 airControlSpeed = new Vector3();
    // ---- JumpBoost ----
    private Collider surfaceHit;

    //---------- Essential Component Reference ------
    private Rigidbody rigidbodyPlayer;
    private Player_Speed player_Speed;
    private Player_Input player_Input;
    private Player_CheckState player_CheckState;

    //--------- Additionel Component Reference -------
    private Player_WallRun player_WallRun;
    private Camera_Controlle camera_Controlle;

    // Start is called before the first frame update
    void Start()
    {
        InitReference();
    }

    private void InitReference()
    {
        GetCameraControlle(activeDebug);
        GetPlayerRigidBody(activeDebug);
        GetPlayerSpeed(activeDebug);
        GetPlayerWallRun(activeDebug);
        GetPlayerInput(activeDebug);
        GetPlayerCheckState(activeDebug);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player_MotorMouvement != Player_MotorMouvement.WallRun)
        {
            CheckBorderSurface();
            //Event Input
            if (callJump)
            {
                Jump();
                callJump = false;
                isReset = false;
            }

            // Air Control -- Put in Fix Update
            if (player_MouvementUp == Player_MouvementUp.Fall || player_MouvementUp == Player_MouvementUp.Jump)
            {
                Vector3 inputDir = GetMouvementInput();

                Vector3 speedDir =
                    (transform.forward * (front * speedAirControl * Time.deltaTime)) +
                    (transform.right * (side * speedAirControl * Time.deltaTime));

                rigidbodyPlayer.velocity = rigidbodyPlayer.velocity + speedDir;
                rigidbodyPlayer.velocity = Vector3.ClampMagnitude(rigidbodyPlayer.velocity, player_Speed.maximumSpeed + maxSpeedOfJump);
            }


        }

    }

    public void Update()
    {

        player = player_MouvementUp;

        if (!GetJumpInput()) inputReset = true;

        //---------- Active Jump -----------------
        if (ConditionChecker())
        {
            if (activeDebug) Debug.Log("Jump!");
            callJump = true;
            ChangeState();
            player = player_MouvementUp;
            AddtoJumpCount();
            player_CheckState.DeactiveFrameGroundDectection();
            inputReset = false;



        }
        
        if (player_Surface == Player_Surface.Grounded && player_MouvementUp != Player_MouvementUp.Jump && !isReset)
        {
            isReset = ResetJumpStat();
            if (activeDebug)
                Debug.Log("Jump has been reset");
        }




    }

    private bool ResetJumpStat()
    {
        RestJumpCount(activeDebug);
        return true;
    }

    private bool GetJumpInput()
    {
        bool keyState = false;
        if (IsGamepad)
        {
            keyState = player_Input.GetInputPress(player_Input.JumpGp);
        }
        else
        {
            keyState = player_Input.GetInputPress(player_Input.JumpPc);
        }
        return keyState;
    }

    private Vector3 GetMouvementInput()
    {
        front = 0;
        side = 0;

        if (!IsGamepad)
        {
            front = player_Input.GetAxis(player_Input.forwardPc, player_Input.backPc);
            side = player_Input.GetAxis(player_Input.leftPc, player_Input.rightPc);

        }
        else
        {
            front = player_Input.GetAxeValue("Vertical"); ;
            side = player_Input.GetAxeValue("Horizontal");
        }

        Vector3 dir = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(side, 0, front).normalized;

        return dir.normalized;
    }

    /// <summary>
    /// Regroup all conditions to active the jump
    /// </summary>
    /// <returns></returns>
    private bool ConditionChecker()
    {
        //Check Input
        if (!GetJumpInput() || !inputReset)
            return false;

        isKeyPress = GetJumpInput();


        //Check Jump Number 
        if (jumpCount > jumpNumber) return false;

        if (player_MouvementUp == Player_MouvementUp.Jump) return false;

        return true;
    }

    private void ChangeState()
    {
        player_MouvementUp = Player_MouvementUp.Jump;
    }

    private void AddtoJumpCount()
    {
        jumpCount++;
    }



    /// <summary>
    /// Add the power of jum to the avatar
    /// </summary>
    private void Jump()
    {
        if (camera_Controlle != null) camera_Controlle.offSetToMove = new Vector3(0, 1, 0);

        rigidbodyPlayer.AddForce(
            Vector3.up * (heightJumpForce + Mathf.Abs(rigidbodyPlayer.velocity.y) + GetBorderForce()),
            ForceMode.Impulse);

        rigidbodyPlayer.AddForce(
            GetMouvementInput() * (forwardJumpForce + GetBorderForce()),
            ForceMode.Impulse);
    }


    // Refaire Wall Run Jump
    public void Jump(Vector3 dir, float power)
    {
        // Set Player Jump State 
        ChangeState();

        // Add jump Count 
        AddtoJumpCount();

        rigidbodyPlayer.AddForce(
            Vector3.up * (heightJumpForce + Mathf.Abs(rigidbodyPlayer.velocity.y) + GetBorderForce()),
            ForceMode.Impulse);

        rigidbodyPlayer.AddForce(
            dir.normalized * (forwardJumpForce + GetBorderForce()),
            ForceMode.Impulse);

    }

    public void RestJumpCount(bool debug)
    {
        jumpCount = 0;
        if (debug) 
        Debug.Log("Jump Count Reset !");
    }

    public void CheckBorderSurface()
    {

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 2, surfaceObstacle))
        {
            if (activeDebug)
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            surfaceHit = hit.collider;

            //Debug.Log("" + hit.collider.name);
        }
        else
        {
            if (activeDebug)
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

    private float GetBorderForce()
    {
        if (JumpBoostBorder()) return border_Bonus;

        return 0;
    }

    #region Get Script Reference

    private void GetCameraControlle(bool debug)
    {
        camera_Controlle = GetComponent<Camera_Controlle>();

        if (camera_Controlle != null)
        {
            if (debug)
            {
                Debug.Log("Camera Controlle Find");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Camera Controlle on the object");
            }
        }
    }

    private void GetPlayerWallRun(bool debug)
    {
        player_WallRun = GetComponent<Player_WallRun>();

        if (player_WallRun != null)
        {
            if (debug)
            {
                Debug.Log("Player Wall Run Find");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Player Wall Run on the object");
            }
        }
    }

    private void GetPlayerSpeed(bool debug)
    {
        player_Speed = GetComponent<Player_Speed>();

        if (player_Speed != null)
        {
            if (debug)
            {
                Debug.Log("Player Speed Find");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Player Speed on the object");
            }
        }
    }

    private void GetPlayerInput(bool debug)
    {
        player_Input = GetComponent<Player_Input>();
        if (player_Input != null)
        {
            if (debug)
            {
                Debug.Log("Player Input Find");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Player Input on the object");
            }
        }
    }

    private void GetPlayerRigidBody(bool debug)
    {
        rigidbodyPlayer = GetComponent<Rigidbody>();
        if (rigidbodyPlayer != null)
        {
            if (debug)
            {
                Debug.Log("Rigidbody Find");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Player Rigidbody on the object");
            }
        }
    }

    private void GetPlayerCheckState(bool debug)
    {
        player_CheckState = GetComponent<Player_CheckState>();
        if (player_CheckState != null)
        {
            if (debug)
            {
                Debug.Log("Player Check State Find");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to Player Check Stateon the object");
            }
        }
    }

    #endregion

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
