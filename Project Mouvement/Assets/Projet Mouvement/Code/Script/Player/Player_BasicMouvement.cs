using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player_Speed))]
[RequireComponent(typeof(Player_Input))]
public class Player_BasicMouvement : Player_Settings
{

    public ForceMode forceMode = ForceMode.Impulse;

    //--- Variable ---
    [Header("Debug")]
    public bool activeDebug;
    private float tempsEcouleResetTemps;

    [Header("Feedback")]
    public Text uiText;


    private float front;
    private float side;

    //--------- Essential Component Reference --------
    private Rigidbody rigidbodyPlayer;
    private Player_Input playerInput;
    private Player_Speed playerSpeed;

    //-------- Additionel Component Reference --------
    private CameraVisualEffect cameraVisualEffect;

    // Start is called before the first frame update
    void Start()
    {
        InitState();
    }

    private void InitState()
    {
        GetPlayerRigidBody(activeDebug);
        GetPlayerSpeed(activeDebug);
        GetPlayerInput(activeDebug);
        GetCameraVisualEffect(activeDebug);
    }

    void FixedUpdate()
    {
        if (player_Surface != Player_Surface.Grounded) return;


        Vector3 inputDir = new Vector3(side, 0, front).normalized;

        // --------------TEST----------
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(transform.position, -Vector3.up, out hit, 10f);
        Tool_SurfaceTopographie.GetTopo(hit.normal, transform, true);
        //--------------TEST----------

        //------------------ DEBUG--------------------
        Debug.DrawRay(transform.position - Vector3.up, (transform.forward  * front + transform.right * side) * 10f, Color.blue);    
        // Debug.Log(DetectionCollision(front, side));
        //------------------ DEBUG--------------------

      //  Debug.Log(DetectionCollision(front, side));
        if (!DetectionCollision(front, side))
        {
            rigidbodyPlayer.AddForce(transform.forward * inputDir.z * playerSpeed.accelerationSpeed, forceMode);
            rigidbodyPlayer.AddForce(transform.right * inputDir.x * playerSpeed.accelerationSpeed, forceMode);

            Vector3 mouvementPlayer = Vector3.ClampMagnitude(new Vector3(rigidbodyPlayer.velocity.x, rigidbodyPlayer.velocity.y, rigidbodyPlayer.velocity.z), playerSpeed.maximumSpeed);
            playerSpeed.currentSpeed = mouvementPlayer.magnitude;
            //mouvementPlayer.y = rigidbodyPlayer.velocity.y;
            rigidbodyPlayer.velocity = mouvementPlayer;

            //Clamp velocity on Z & X axes


            if (inputDir.magnitude == 0 && mouvementPlayer.magnitude > 1)
            {
              
                rigidbodyPlayer.velocity = rigidbodyPlayer.velocity.normalized * playerSpeed.currentSpeed;
                playerSpeed.DeccelerationPlayerSpeed();
            }
            SetUpState(front, side);
        }




    }


    private void Update()
    {
        if (!MacroFunction.isPause)
        {
            front = 0;
            side = 0;
            // Input of the player
            if (!IsGamepad)
            {
                front = playerInput.GetAxis(playerInput.forwardPc, playerInput.backPc);
                side = playerInput.GetAxis(playerInput.leftPc, playerInput.rightPc);

            }
            else
            {
                front = playerInput.GetAxeValue("Vertical"); ;
                side = playerInput.GetAxeValue("Horizontal");
            }



            EffectVisuel();
            DebugUI();
            SetPlayerRotatation();
            if (player_Surface != Player_Surface.Grounded) return;
            SetUpState(front, side);

        }

    }



    public void SetPlayerRotatation()
    {
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(transform.position , - Vector3.up, out hit, 10f);
        Vector3 anglePlayer = Tool_SurfaceTopographie.GetTopo(hit.normal, transform, false);
       // Debug.Log("X = " + anglePlayer.x + " Z =" + anglePlayer.z);
        transform.rotation =  Quaternion.Euler(anglePlayer.x, transform.rotation.eulerAngles.y, anglePlayer.z);
    }



    // Check if Obstacle in the front direction
    public bool DetectionCollision(float forward, float side)
    {
        bool IsDectect = false;
        Debug.DrawRay(transform.position - 0.9f*Vector3.up, (transform.forward * forward + transform.right * side).normalized * 100, Color.red);
        IsDectect = Physics.Raycast(transform.position - (0.8f * Vector3.up), (transform.forward * forward + transform.right * side), 1.1f);
        if (IsDectect) return IsDectect;
        Debug.DrawRay(transform.position + Vector3.up, (transform.forward * forward + transform.right * side).normalized * 100, Color.red);
        IsDectect = Physics.Raycast(transform.position + transform.up, transform.forward * forward + transform.right * side, 1.1f);
        Debug.Log(IsDectect); 

        return IsDectect;
    }

    public void SetUpState(float front, float side)
    {
        if (front != 0 && side != 0)
        {
            player_MotorMouvement = Player_MotorMouvement.Run;
        }
    }

    public void EffectVisuel()
    {
        if (!cameraVisualEffect) return;
        cameraVisualEffect.FieldOfViewValue(rigidbodyPlayer.velocity.magnitude);
    }

    public void DebugUI()
    {
        tempsEcouleResetTemps += Time.deltaTime;
        if (tempsEcouleResetTemps >= 1)
        {
            uiText.text = "" + rigidbodyPlayer.velocity.magnitude.ToString("F0");
            tempsEcouleResetTemps = 0;
        }
        if (rigidbodyPlayer.velocity.magnitude < 1)
        {
            if (!cameraVisualEffect) return;
            cameraVisualEffect.resetSpeed = true;
        }
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

    public void DeathReset()
    {
        StopPlayer(rigidbodyPlayer);
    }



    //--------- Get Reference -----------

    private void GetCameraVisualEffect(bool debug)
    {
        cameraVisualEffect = GetComponent<CameraVisualEffect>();

        if (cameraVisualEffect != null)
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

    private void GetPlayerSpeed(bool debug)
    {
        playerSpeed = GetComponent<Player_Speed>();

        if (playerSpeed != null)
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
        playerInput = GetComponent<Player_Input>();
        if (playerInput != null)
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
}
