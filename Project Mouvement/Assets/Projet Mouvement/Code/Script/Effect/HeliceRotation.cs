using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliceRotation : MonoBehaviour
{
    public float speed_Rotation = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.right, speed_Rotation * Time.deltaTime);
    }
}
