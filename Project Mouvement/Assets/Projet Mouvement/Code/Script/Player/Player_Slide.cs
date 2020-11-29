using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class Player_Slide : Player_Settings
{
    private Rigidbody player_Rigidbody;
    private CapsuleCollider myCollider;

    private bool checkAerial = false;

    [EventRef]
    public string AerialSound;
    FMOD.Studio.EventInstance AerialInstance;
    // Start is called before the first frame update
    void Start()
    {
        player_Rigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<CapsuleCollider>();
        if (AerialSound != null)
        {
            AerialInstance = RuntimeManager.CreateInstance(AerialSound);
            RuntimeManager.AttachInstanceToGameObject(AerialInstance, transform, GetComponent<Rigidbody>());

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player_Surface == Player_Surface.Air || player_MotorMouvement == Player_MotorMouvement.Slide)
        {
            if (!checkAerial)
            {

                checkAerial = true;
                AerialInstance.start();
            }

        }
        else
        {
            if (checkAerial)
            {
                AerialInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                checkAerial = false;
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //myCollider.height = 1;
            //myCollider.center = new Vector3(0,-0.5f,0);
            //Camera.main.transform.position = new Vector3(0, 0, 0);
            player_MotorMouvement = Player_MotorMouvement.Slide;
            Camera.main.transform.GetChild(0).gameObject.SetActive(true);

            //if (!checkAerial)
            //{
            //    checkAerial = true;
            //    AerialInstance.start();
            //    AerialInstance.setParameterByName("SpeedFaceOf", 0.6f);
            //
            //}

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

            //if (checkAerial)
            //{
            //    checkAerial = false;
            //    AerialInstance.setParameterByName("SpeedFaceOf", 0f);
            //
            //}
        }   //
    }
}
