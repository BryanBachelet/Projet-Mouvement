using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Slide : Player_Settings
{
    private Rigidbody player_Rigidbody;
    private CapsuleCollider myCollider;
    // Start is called before the first frame update
    void Start()
    {
      player_Rigidbody =  GetComponent<Rigidbody>();
      myCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //myCollider.height = 1;
            //myCollider.center = new Vector3(0,-0.5f,0);
            //Camera.main.transform.position = new Vector3(0, 0, 0);
            player_MotorMouvement = Player_MotorMouvement.Slide;
            Camera.main.transform.GetChild(0).gameObject.SetActive(true);

        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            player_Rigidbody.AddForce(transform.forward * 10, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            //myCollider.height = 2;
            //myCollider.center = new Vector3(0, 0f, 0);
            //Camera.main.transform.position = new Vector3(0, 0.5f, 0);
            player_MotorMouvement = Player_MotorMouvement.Run;
            Camera.main.transform.GetChild(0).gameObject.SetActive(false);
        }   //
    }
}
