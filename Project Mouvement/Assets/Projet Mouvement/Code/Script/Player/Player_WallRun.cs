using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player_Speed))]
[RequireComponent(typeof(Player_Input))]
public class Player_WallRun : Player_Settings
{

    //----- Variable -------------

    [Header("Debug")]
    public bool activeDebug = false;

    [Header("Wall Run")]
    public float timerReplace = 0.6f;

    public float wallRunResetTimer = 0.7f;
    public float minSpeedValueToWallRun = 5;


    //--- Systeme Variable---

    private float enterSpeed;
    private float wallRunSide;
    private GameObject wallRunning;
    //Lerp Position
    private float t;
    private float countDownLerp = 0f;

    //Wall Run Reset
    public float countdownWrReset = 0;
    public bool isReset = true;

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

        // Enter in Wall Run
        if (player_Surface == Player_Surface.Wall && player_MouvementUp == Player_MouvementUp.Fall && player_MotorMouvement != Player_MotorMouvement.WallRun && isReset)
        {
            if (activeDebug) Debug.Log("Enter Wall Ride");

            CheckWallAngle();
        }


        if (player_MotorMouvement == Player_MotorMouvement.WallRun)
        {
            //Quit Wall Run Fonction
            CheckWallSide(activeDebug);

            // Jump to get ou wall run
            JumpQuit(activeDebug);

            //Check if front Wall
            CheckFrontWall(activeDebug);
            // Check la vitesse du joueur
            CheckPlayerSpeed(activeDebug);

            // Check Slide Input
            SlideInput(activeDebug);

            //  Check Input
            InputCheck(activeDebug);

            //Player Reposition
            ReplacementPlayer();

            //Player Acceleration 
            AccelerationWallRun(activeDebug);

            GetCurrentHorizontalSpeed(activeDebug);
            // Minimum Speed Wall Run
            SetNewVelocitySpeed();
        }

        //Timer Reset Player Wall Run
        if (player_MotorMouvement != Player_MotorMouvement.WallRun)
        {
            if (!isReset)
            {
                if (countdownWrReset > wallRunResetTimer)
                {
                    isReset = true;
                    countdownWrReset = 0;
                }
                countdownWrReset += Time.deltaTime;
            }
        }
    }

    #region Function of Exit

    //Quit Wall Run 
    private void CheckWallSide(bool debug)
    {
        if (player_CheckState.wallSide == 0)
        {
            DeactiveWallRun();
            if (debug)
            {
                Debug.Assert(player_CheckState.wallSide == 0);
                Debug.Log("No Wall Side");
            }

        }

    }

    //Jump Wall Run
    private void JumpQuit(bool debug)
    {
        if (player_Input.GetInputPress(player_Input.JumpPc) || player_Input.GetInputPress(player_Input.JumpGp))
        {
            player_Jump.Jump(Vector3.up + Vector3.right * 3 + Vector3.forward * 2, 20f);
            if (debug)
            {

                Debug.Log("Jump End");
            }
            DeactiveWallRun();
        }
    }


    // Check if slide input is done 
    private void SlideInput(bool debug)
    {
        if (player_Input.GetInputPress(player_Input.SlidePc) || player_Input.GetInputPress(player_Input.slideGp))
        {
            DeactiveWallRun();
            if (debug)
            {

                Debug.Log("Slide End");
            }
        }
    }

    // Check if Front Wall
    private void CheckFrontWall(bool debug)
    {
        if (Physics.Raycast(transform.position, transform.forward,2* player_Speed.currentSpeed * Time.deltaTime))
        {
            DeactiveWallRun();
            if (debug)
            {

                Debug.Log("Mur en face");
            }
        }
    }

    //Check Player Speed 
    private void CheckPlayerSpeed(bool debug)
    {
        if (player_Speed.currentSpeed < minSpeedValueToWallRun)
        {
            DeactiveWallRun();
            if (debug)
            {

                Debug.Log("Low Speed");
            }
        }

    }

    // Check si un Input est réaliser
    private void InputCheck(bool debug)
    {
        if (!player_Input.mouvementInputEnter)
        {
            DeactiveWallRun();
            if (debug)
            {

                Debug.Log("No Input");
            }
        }
    }

    // Déactivation du Wall Run 
    public void DeactiveWallRun()
    {
        SetGravity(true);
        player_MotorMouvement = Player_MotorMouvement.Null;
        rigidbodyPlayer.AddForce(transform.right * (-wallRunSide) * 5, ForceMode.Impulse);
        isReset = false;
    }

    #endregion

    #region Enter Function
    // Activate Wall Run

    public void CheckWallAngle()
    {
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(transform.position, transform.right * player_CheckState.wallSide, out hit, 10f);
        float angle = Vector3.Angle(Vector3.up, hit.normal);
        if (activeDebug) Debug.Log("Angle = " + angle);
        if (angle == 90)
        {
            //Récupérer le côté du Wall Run
            wallRunSide = player_CheckState.wallSide;
            //Activation du Wall Run State
            if (player_MotorMouvement != Player_MotorMouvement.WallRun)
            {
                ActivateWallRun();
                countDownLerp = 0;
            }
        }
    }

    public void ActivateWallRun()
    {
        Debug.Log("Active Wall Run");
        // Changement de state
        player_MotorMouvement = Player_MotorMouvement.WallRun;
        player_MouvementUp = Player_MouvementUp.Null;
        // Set Gravity 
        SetGravity(false);

        player_Speed.IncreamenteMaxSpeed();

        // Find Current Speed+
        GetCurrentHorizontalSpeed(activeDebug);
        // Reset Direction of deplacement
        SetNewVelocitySpeed();
        // Reset Jump Count
        player_Jump.RestJumpCount(activeDebug);
    }

    #endregion

    #region System Function

    /// <summary>
    /// Récupère la vitesse actuelle de l'avatar
    /// </summary>
    private void GetCurrentHorizontalSpeed(bool debug)
    {
        enterSpeed = player_Speed.currentSpeed;
        if (debug)
            Debug.Log("Enter horizontal speed = " + enterSpeed.ToString("F0"));
    }


    private void ReplacementPlayer()
    {
        // Lerp entre la position du joueur et la position souhaitée près du mur
        // Calcul de la distance / pos à atteindre
        Vector3 wallDir = transform.position - player_CheckState.hit.point;
        Vector3 newPos = player_CheckState.hit.point + wallDir.normalized * 1f;
        // Lerp de la position du joueur
        transform.position = Vector3.Lerp(transform.position, newPos, t);
        // Timer du lerp (Vitesse)
        t = countDownLerp / timerReplace;
        countDownLerp += Time.deltaTime;
    }

    /// <summary>
    /// Récupère la direction du mur 
    /// </summary>
    /// <returns></returns>
    private Vector3 GetWallDirection()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.right * player_CheckState.wallSide, out hit, 10f);
        Vector3 wallDir = Quaternion.Euler(0, 90 * player_CheckState.wallSide, 0) * hit.normal;
        Debug.Log("Dir parallèle =" + wallDir + " Normal =" + hit.normal);
        return wallDir;
    }

    /// <summary>
    /// Reset la nouvelle velocité 
    /// </summary>
    private void SetNewVelocitySpeed()
    {
        Vector3 wallDir = GetWallDirection();
        rigidbodyPlayer.angularVelocity = Vector3.zero;
        rigidbodyPlayer.velocity = Vector3.zero;
        rigidbodyPlayer.velocity = wallDir.normalized * enterSpeed;
        Debug.Log("Velocity =" + rigidbodyPlayer.velocity);
    }

    /// <summary>
    /// Reset la nouvelle velocité 
    /// </summary>
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
    /// <summary>
    /// Accélération du player
    /// </summary>
    private void AccelerationWallRun(bool debug)
    {
        if (player_Input.GetInputPress(player_Input.forwardPc) || player_Input.GetAxeValue(player_Input.FrontAxisGp) > 0)
        {
            player_Speed.AccelerationPlayerSpeed();
            if (debug)
                Debug.Log("Acceleraction");
            return;
        }

        if (player_Input.GetInputPress(player_Input.backPc) || player_Input.GetAxeValue(player_Input.FrontAxisGp) < 0)
        {
            player_Speed.DeccelerationPlayerSpeed();
            if (debug)
                Debug.Log("Decceleration");
            return;
        }
    }


    #endregion

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

    #region Tools

    private void SetGravity(bool isGravity)
    {
        rigidbodyPlayer.useGravity = isGravity;
        Debug.Log("Gravity = " + rigidbodyPlayer.useGravity);
    }

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

    #endregion 
}
