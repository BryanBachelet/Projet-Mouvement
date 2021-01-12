using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player_Speed))]
[RequireComponent(typeof(Player_Input))]
public class Player_WallRun : Player_Settings
{
    [Header("Input Setting")]
    public KeyCode Forward = KeyCode.Z;
    public KeyCode Back = KeyCode.S;

    //----- Variable -------------

    [Header("Debug")]
    public bool activeDebug = false;


    //--- Systeme Variable---
    
    private float enterSpeed;
    private float wallRunSide;
    private GameObject wallRunning;

    //---- Essentiat Components Reference ----
    private Rigidbody rigidbodyPlayer;
    private Player_Speed player_Speed;
    private Player_Input player_Input;
    private Player_Jump player_Jump;
    private Player_CheckState player_CheckState;


    private void Start()
    {
        InitReference();
    }

    private void InitReference()
    {
        GetPlayerSpeed(activeDebug);
        GetPlayerInput(activeDebug);
        GetPlayerJump(activeDebug);
        GetPlayerRigidBody(activeDebug);
        GetPlayerCheckWall(activeDebug);
    }


    private void Update()
    {
        // Check if Wall Run est activé
        // Check s'il y a un mur 
        // Check les états du player
        if(player_Surface == Player_Surface.Wall && player_MouvementUp == Player_MouvementUp.Fall && player_MotorMouvement != Player_MotorMouvement.WallRun)
        {
            if (activeDebug) Debug.Log("Enter Wall Ride");
            // Check si l'angle du mur est à 90° degré
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(transform.position, transform.right * player_CheckState.wallSide, out hit, 10f);
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            if (activeDebug) Debug.Log("Angle = " + angle);
            if (angle == 90)
            { 
                //Récupérer le côté du Wall Run
                wallRunSide = player_CheckState.wallSide;
            }
        }


        if (player_MotorMouvement == Player_MotorMouvement.WallRun)
        {
            //Quit Wall Run Fonction
            CheckWallSide();
            JumpQuit();

            // Minimum Speed Wall Run
            SetNewVelocitySpeed(10f, 10f);
        }
    }

    //Quit Wall Run 
    private void CheckWallSide()
    {
        if (player_CheckState.wallSide == 0)
        {
            DeactiveWallRun();
        }

    }

    //Jump Wall Run
    private void JumpQuit()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
        {
            player_Jump.Jump(Vector3.up + Vector3.right * 3 + Vector3.forward * 2, 20f);
            DeactiveWallRun();
        }
    }

    // Déactivation du Wall Run 
    public void DeactiveWallRun()
    {
        SetGravity(true);
        player_MotorMouvement = Player_MotorMouvement.Null;
    }

    // Activate Wall Run
    public void ActivateWallRun()
    {
        Debug.Log("Active Wall Run");
        player_MotorMouvement = Player_MotorMouvement.WallRun;
        SetGravity(false);
        GetCurrentHorizontalSpeed();
        SetNewVelocitySpeed();
        player_Jump.RestJumpCount(activeDebug);
    }


    private void GetCurrentHorizontalSpeed()
    {
        enterSpeed = new Vector3(rigidbodyPlayer.velocity.x, 0, rigidbodyPlayer.velocity.z).magnitude;
        Debug.Log("Enter horizontal speed = " + enterSpeed.ToString("F0"));
    }



    private Vector3 GetWallDirection()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.right * player_CheckState.wallSide, out hit, 10f);
        Vector3 wallDir = Quaternion.Euler(0, 90 * player_CheckState.wallSide, 0) * hit.normal;
        Debug.Log("Dir parallèle =" + wallDir + " Normal =" + hit.normal);
        return wallDir;
    }

    private void SetNewVelocitySpeed()
    {
        Vector3 wallDir = GetWallDirection();
        rigidbodyPlayer.angularVelocity = Vector3.zero;
        rigidbodyPlayer.velocity = Vector3.zero;
        rigidbodyPlayer.velocity = wallDir.normalized * enterSpeed;
        Debug.Log("Velocity =" + rigidbodyPlayer.velocity);
    }

    private void SetNewVelocitySpeed(float speed, float speedMin)
    {
        if (speedMin > rigidbodyPlayer.velocity.magnitude)
        {
            Vector3 wallDir = GetWallDirection();
            rigidbodyPlayer.angularVelocity = Vector3.zero;
            rigidbodyPlayer.velocity = Vector3.zero;
            rigidbodyPlayer.velocity = wallDir.normalized * speed;
            Debug.Log("Velocity =" + rigidbodyPlayer.velocity);
        }
    }



    private void SetGravity(bool isGravity)
    {
        rigidbodyPlayer.useGravity = isGravity;
        Debug.Log("Gravity = " + rigidbodyPlayer.useGravity);
    }

    #region Get Reference

    private void GetPlayerCheckWall(bool debug)
    {
        player_CheckState = GetComponent<Player_CheckState>();
        if (player_CheckState != null)
        {
            if (debug)
            {
                Debug.Log("Check State is Find");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Player_CheckState on the object");
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
                Debug.Log("Rigidbody finds");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Rigidbody on the object");
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
                Debug.Log("Player Speed finds");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Player_Speed on the object");
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
                Debug.Log("Player Input finds");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Player_Input on the object");
            }
        }
    }

    private void GetPlayerJump(bool debug)
    {
        player_Jump = GetComponent<Player_Jump>();
        if (player_Jump != null)
        {
            if (debug)
            {
                Debug.Log("Player Jump finds");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Player Jump on the object");
            }
        }
    }

    #endregion


    //----------- TOOLS ----------

    private float SetNegativeAngle(float angle)
    {
        if (angle > 180)
        {
            angle = -360 + angle;
        }

        return angle;
    }

    public float GetAxis(KeyCode Positif, KeyCode Negatif, bool Press)
    {
        float axisValue = 0;
        if (Input.GetKey(Positif))
        {
            axisValue += 1;
        }
        if (Input.GetKey(Negatif))
        {
            axisValue -= 1;
        }
        axisValue = Mathf.Clamp(axisValue, -1, 1);


        return axisValue;
    }
}
