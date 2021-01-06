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
    private Vector3 SpherePos;

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
        if (player_Surface != Player_Surface.Grounded || player_MouvementUp == Player_MouvementUp.Jump) return;

        Vector3 inputDir = new Vector3(side, 0, front).normalized;

        if (!DetectionCollision(front, side, activeDebug))
        {

            if (inputDir.magnitude != 0)
            {
                rigidbodyPlayer.AddForce(transform.forward * inputDir.z * playerSpeed.accelerationSpeed, forceMode);
                rigidbodyPlayer.AddForce(transform.right * inputDir.x * playerSpeed.accelerationSpeed, forceMode);

                Vector3 mouvementPlayer = Vector3.ClampMagnitude(new Vector3(rigidbodyPlayer.velocity.x, rigidbodyPlayer.velocity.y, rigidbodyPlayer.velocity.z), playerSpeed.maximumSpeed);
                playerSpeed.currentSpeed = mouvementPlayer.magnitude;

                rigidbodyPlayer.velocity = mouvementPlayer;
            }

            if (inputDir.magnitude == 0 && rigidbodyPlayer.velocity.magnitude > 0)
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
            if (player_Surface != Player_Surface.Grounded || player_MouvementUp == Player_MouvementUp.Jump) return;
            SetPlayerRotatation(activeDebug);
            SetUpState(front, side);
            
        }

    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(SpherePos, 3f);
    }

    public void SetPlayerRotatation(bool debug)
    {
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(transform.position + ((transform.forward * front + transform.right * side).normalized * playerSpeed.currentSpeed * Time.deltaTime), -Vector3.up, out hit, 3f);
        SpherePos = hit.point;
        Vector3 anglePlayer = Tool_SurfaceTopographie.GetTopo(hit.normal, transform, debug);

        transform.rotation = Quaternion.Euler(anglePlayer.x, transform.rotation.eulerAngles.y, anglePlayer.z);
    }



    // Check if Obstacle in the front direction
    public bool DetectionCollision(float forward, float side, bool debug)
    {
        bool IsDectect = false;
        if (debug) Debug.DrawRay(transform.position - 0.9f * Vector3.up, (transform.forward * forward + transform.right * side).normalized * 100, Color.red);
        IsDectect = Physics.Raycast(transform.position - (0.8f * Vector3.up), (transform.forward * forward + transform.right * side), 1.1f);

        if (IsDectect) return IsDectect;
        if (debug) Debug.DrawRay(transform.position + Vector3.up, (transform.forward * forward + transform.right * side).normalized * 100, Color.red);

        IsDectect = Physics.Raycast(transform.position + transform.up, transform.forward * forward + transform.right * side, 1.1f);
        if (debug) Debug.Log(IsDectect);

        return false;
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
                Debug.Log("Camera Visual Effect Find");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Camera Visual Effect on the object");
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
