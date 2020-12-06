using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public static float mouseSensitivity = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivity = Input.GetAxis("Mouse X");   
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
