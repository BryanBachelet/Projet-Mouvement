using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player_BasicMouvement : MonoBehaviour
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


    private Rigidbody rigidbodyPlayer;



    // Start is called before the first frame update
    void Start()
    {
        //Initiate Component
        rigidbodyPlayer = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        // Input of the player
        float front = GetAxis(Forward, Back, true);
        float side = GetAxis(Right, Left, true);

        Debug.Log("Front = " +front);

      
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
