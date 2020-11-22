using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Player_BasicMouvement : MonoBehaviour
{
    static public bool controllerGamePad = false;
    public bool controllerGamePadInspector = false;
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

    [SerializeField]
    private Text uiText;
    
    private float currentSpeed;
    private Rigidbody rigidbodyPlayer;

    private float tempsEcouleResetTemps = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Initiate Component
        rigidbodyPlayer = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        controllerGamePad = controllerGamePadInspector;
    }
    void FixedUpdate()
    {
        tempsEcouleResetTemps += Time.deltaTime;
        if(tempsEcouleResetTemps >= 1)
        {
            uiText.text = ""+rigidbodyPlayer.velocity.magnitude;
            tempsEcouleResetTemps = 0;
        }
        float front = 0;
        float side = 0;
        // Input of the player
        if (!controllerGamePad)
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
        rigidbodyPlayer.AddForce(transform.forward * front * accelerationValue, ForceMode.Acceleration);
        rigidbodyPlayer.AddForce(transform.right * side * accelerationValue, ForceMode.Acceleration);
        rigidbodyPlayer.velocity = Vector3.ClampMagnitude(rigidbodyPlayer.velocity, 10f);


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

        Debug.Log("Axis =" + axisValue);
        return axisValue;
    }
}
