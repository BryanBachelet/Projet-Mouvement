using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Player_BasicMouvement : Player_Settings
{

    //Need to inherited to player setting

    // Register Input of mouvement
    [Header("Input Setting")]
    public KeyCode Forward = KeyCode.Z;
    public KeyCode Back = KeyCode.S;
    public KeyCode Left = KeyCode.Q;
    public KeyCode Right = KeyCode.D;

    [Header("Mouvement Setting")]
    public float accelerationValue = 1;
    public float deccelerationValue = 1;
    public float maxValue = 10;
    public ForceMode forceMode = ForceMode.Impulse;


    [SerializeField]
    [Header("Feedback")]
    private Text uiText;

    private float front;
    private float side;

    private float currentSpeed;
    static public Rigidbody rigidbodyPlayer;
    CameraVisualEffect myCVEscript;
    private float tempsEcouleResetTemps = 0;
    private Player_Jump player_Jump;

    // Start is called before the first frame update
    void Start()
    {
        InitState();
    }

    public void InitState()
    {
        rigidbodyPlayer = GetComponent<Rigidbody>();
        player_Jump = GetComponent<Player_Jump>();

        myCVEscript = Camera.main.GetComponent<CameraVisualEffect>();
    }

    void FixedUpdate()
    {
        //Player acceleration
        Vector3 dirMouvement = new Vector3(side, 0, front).normalized;
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(transform.position, -Vector3.up, out hit, 10f);
        Tool_SurfaceTopographie.GetTopo(hit.normal, transform);
        //------------------ DEBUG--------------------
        Debug.DrawRay(transform.position - Vector3.up, (transform.forward * front + transform.right * side) * 10f, Color.blue);
      // Debug.Log(DetectionCollision(front, side));
        //------------------ DEBUG--------------------
        if (player_MotorMouvement != Player_MotorMouvement.WallRun)
        {
            if (!DetectionCollision(front, side))

            {
                rigidbodyPlayer.AddForce(transform.forward * dirMouvement.z * accelerationValue, forceMode);
                rigidbodyPlayer.AddForce(transform.right * dirMouvement.x * accelerationValue, forceMode);

                //Clamp velocity on Z & X axes
                Vector3 mouvementPlayer = new Vector3(rigidbodyPlayer.velocity.x, 0, rigidbodyPlayer.velocity.z);
                mouvementPlayer = Vector3.ClampMagnitude(mouvementPlayer, maxValue);
                mouvementPlayer.y = rigidbodyPlayer.velocity.y;
                rigidbodyPlayer.velocity = mouvementPlayer;

                SetUpState(front, side);
            }

            if (side == 0 && front == 0 && rigidbodyPlayer.velocity.magnitude > 1)
            {
                rigidbodyPlayer.velocity = new Vector3(rigidbodyPlayer.velocity.x * 0.90f, rigidbodyPlayer.velocity.y, rigidbodyPlayer.velocity.z * 0.90f);
            }
        }


        front = 0;
        side = 0;
    }


    private void Update()
    {
        if (player_MotorMouvement != Player_MotorMouvement.WallRun)
        {
            // Input of the player
            if (!IsGamepad)
            {
                front = GetAxis(Forward, Back, true);
                side = GetAxis(Right, Left, true);

            }
            else
            {
                front = Input.GetAxis("Vertical");
                side = Input.GetAxis("Horizontal");
            }
        }

        if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.Joystick1Button9))
        {
            IsGamepad = !IsGamepad;
        }


        EffectVisuel();
        DebugUI();
    }

    // Check if Obstacle in the front direction
    public bool DetectionCollision(float forward, float side)
    {
        bool IsDectect = false;
        IsDectect = Physics.Raycast(transform.position - Vector3.up, (transform.forward * forward + transform.right * side), 1.1f);
        if (IsDectect) return IsDectect;
        IsDectect = Physics.Raycast(transform.position + Vector3.up, transform.forward * forward + transform.right * side, 1.1f);

        return IsDectect;
    }

    public void SetUpState(float front, float side)
    {
        if (player_MotorMouvement != Player_MotorMouvement.Slide)
        {
            if (front == 0 && side == 0)
            {
                player_MotorMouvement = Player_MotorMouvement.Null;
            }
            else
            {
                player_MotorMouvement = Player_MotorMouvement.Run;
            }
        }
    }

    public void EffectVisuel()
    {
        //  myCVEscript.FieldOfViewValue(rigidbodyPlayer.velocity.magnitude);
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
            // myCVEscript.resetSpeed = true;
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
}
