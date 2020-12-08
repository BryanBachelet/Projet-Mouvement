using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WallRun : Player_Settings
{
    [Header("Input Setting")]
    public KeyCode Forward = KeyCode.Z;
    public KeyCode Back = KeyCode.S;


    //---Variable---

    private float enterSpeed;

    //---- Components ----
    private Player_Jump playerJump;
    private Rigidbody playerRigid;
    private Player_CheckState checkState;


    private void Start()
    {
        GetPlayerJump();
        GetPlayerRigidBody();
        GetPlayerCheckWall();
    }


    private void Update()
    {
        if(checkState.wallSide == 0)
        {
            DeactiveWallRun();
        }
    }


    public void DeactiveWallRun()
    {
        SetGravity(true);
        player_MotorMouvement = Player_MotorMouvement.Null;
    }

    public void ActivateWallRun()
    {
        Debug.Log("Active Wall Run");
        player_MotorMouvement = Player_MotorMouvement.WallRun;
        SetGravity(false);
        GetCurrentHorizontalSpeed();
        SetNewVelocitySpeed();
        playerJump.RestJumpCount();
    }


    private void GetCurrentHorizontalSpeed()
    {
        enterSpeed = new Vector3(playerRigid.velocity.x, 0, playerRigid.velocity.z).magnitude;
        Debug.Log("Enter horizontal speed = " + enterSpeed.ToString("F0"));
    }



    private void SetNewVelocitySpeed() 
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.right * checkState.wallSide, out hit, 10f);
        Vector3 wallDir = Quaternion.Euler(0, 90 * checkState.wallSide, 0)* hit.normal;
        playerRigid.angularVelocity = Vector3.zero;
        playerRigid.velocity = Vector3.zero;
        playerRigid.velocity = wallDir.normalized * enterSpeed;
        Debug.Log("Dir parallèle ="+ wallDir + " Normal =" + hit.normal);
        Debug.Log("Velocity =" + playerRigid.velocity);
    }

    

    private void SetGravity(bool isGravity)
    {
        playerRigid.useGravity = isGravity;
        Debug.Log("Gravity = " + playerRigid.useGravity);
    }

    private void GetPlayerCheckWall()
    {
        checkState = GetComponent<Player_CheckState>();
        if(checkState != null)
        {
            Debug.Log("Check State");
        }
        else
        {
            Debug.LogWarning("You need to put Player_CheckState on the object");
        }
    }

    private void GetPlayerRigidBody()
    {
        playerRigid = GetComponent<Rigidbody>();
        if (playerRigid != null)
        {
            Debug.Log("Rigidbody Find");
        }
        else
        {
            Debug.LogWarning("You need to put Player_Jump on the object");
        }
    }

    private void GetPlayerJump()
    {
        playerJump = GetComponent<Player_Jump>();
        if (playerJump != null)
        {
            Debug.Log("Player_Jump Find");
        }
        else
        {
            Debug.LogWarning("You need to put Player_Jump on the object");
        }
    }



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
