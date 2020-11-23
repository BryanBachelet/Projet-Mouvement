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

    [Header("Controlle Setting")]
    public bool controllerGamePad = false;

    [SerializeField]
    [Header("Feedback")]
    private Text uiText;

    private float currentSpeed;
    static public Rigidbody rigidbodyPlayer;
    private float tempsEcouleResetTemps = 0;
    private Player_Jump player_Jump;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody>();
        player_Jump = GetComponent<Player_Jump>();
        IsGamepad = controllerGamePad;
    }

    void FixedUpdate()
    {
        float front, side = 0;

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

        //Player acceleration
        Vector3 dirMouvement = new Vector3(side, 0, front).normalized;
        if (!Physics.Raycast(transform.position , dirMouvement, 0.6f))
        {
            rigidbodyPlayer.AddForce(transform.forward * dirMouvement.z * accelerationValue, forceMode);
            rigidbodyPlayer.AddForce(transform.right * dirMouvement.x * accelerationValue, forceMode);

            //Clamp velocity on Z & X axes
            Vector3 mouvementPlayer = new Vector3(rigidbodyPlayer.velocity.x, 0, rigidbodyPlayer.velocity.z);
            mouvementPlayer = Vector3.ClampMagnitude(mouvementPlayer, maxValue);
            mouvementPlayer.y = rigidbodyPlayer.velocity.y;
            rigidbodyPlayer.velocity = mouvementPlayer;
        }





    }

    private void Update()
    {

        tempsEcouleResetTemps += Time.deltaTime;
        if (tempsEcouleResetTemps >= 1)
        {
            uiText.text = "" + rigidbodyPlayer.velocity.magnitude;
            tempsEcouleResetTemps = 0;
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
}
