using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WallRun : Player_Settings
{
    [Header("Input Setting")]
    public KeyCode Forward = KeyCode.Z;
    public KeyCode Back = KeyCode.S;

    public float accelerationSpeed = 50;
    public float maxValue = 10;

    private Player_Jump playerJump;
    private Player_CheckState checkState;
    private Rigidbody player_Rigid;
    // Start is called before the first frame update
    void Start()
    {
        player_Rigid = GetComponent<Rigidbody>();
        checkState = GetComponent<Player_CheckState>();
        playerJump = GetComponent<Player_Jump>();
    }

    // Update is called once per frame
    void Update()
    {


        if (player_Surface == Player_Surface.Wall)
        {
            if (player_MouvementUp == Player_MouvementUp.Fall)
            {         
                ActiveWallRun();
            }
        }

        if (player_MotorMouvement == Player_MotorMouvement.WallRun)
        {
            float front = GetAxis(Forward, Back, true);
            Debug.Log("Check Wall = "+ checkState.wallSide);
            // Search Normal && Rervese
            Vector3 dirMouvemement = GetParalle(checkState.wallSide);
            float anglePlayer = Vector3.Angle(Vector3.forward, dirMouvemement.normalized);
            anglePlayer = SetNegativeAngle(anglePlayer);
            transform.rotation = Quaternion.Euler(transform.rotation.x, anglePlayer, transform.rotation.z);
            
            player_Rigid.AddForce(transform.forward * front * accelerationSpeed, ForceMode.Acceleration);
            Vector3 mouvementPlayer = new Vector3(player_Rigid.velocity.x, 0, player_Rigid.velocity.z);
            mouvementPlayer = Vector3.ClampMagnitude(mouvementPlayer, maxValue);
            player_Rigid.velocity = new Vector3(mouvementPlayer.x, 0, mouvementPlayer.z);
            if(checkState.wallSide == 0)
            {
                DeactiveWallRun();
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
            {
                DeactiveWallRun();

                playerJump.Jump(Vector3.up + (transform.right * 2 * checkState.wallSide));
            }
        }

    }

    private float SetNegativeAngle(float angle)
    {
        if (angle > 180)
        {
            angle = -360 + angle;
        }

        return angle;
    }

    public void ActiveWallRun()
    {
        player_MotorMouvement = Player_MotorMouvement.WallRun;
        player_MouvementUp = Player_MouvementUp.Null;
        player_Rigid.velocity = new Vector3(player_Rigid.velocity.x, 0, player_Rigid.velocity.z);
        player_Rigid.useGravity = false;
    }

    public void DeactiveWallRun()
    {
        player_MotorMouvement = Player_MotorMouvement.Null;
        player_MouvementUp = Player_MouvementUp.Jump;
        player_Rigid.useGravity = true;
    }

    public Vector3 GetParalle(float wallside)
    {
        wallside = wallside == 0 ? 1 : wallside;
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(transform.position, transform.right * wallside, out hit, 2f);
        Vector3 paralleleVector = Quaternion.Euler(0, 90 * wallside, 0) * hit.normal; 
          
        return paralleleVector;
    }

    // Doublons
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
