using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Slide : Player_Settings
{
    private Rigidbody player_Rigidbody;
    // Start is called before the first frame update
    void Start()
    {
      player_Rigidbody =  GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            player_Rigidbody.AddForce(transform.forward * 10, ForceMode.Impulse);
        }
    }
}
