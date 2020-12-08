using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class Player_Slide : Player_Settings
{
    private Rigidbody player_Rigidbody;
    private CapsuleCollider myCollider;

    private bool checkAerial = false;
    private bool checkSlide = false;

    public AnimationCurve accelerationOnSlide;
    private float tempsEcouleAccelerationSlide = 0;
    private bool resetAcceleration = false;
    private GameObject particulEffectAcceleration;
    private ParticleSystem.MainModule effectParticule;

    [EventRef]
    public string AerialSound;
    FMOD.Studio.EventInstance AerialInstance;

    [EventRef]
    public string SlideSound;
    public static FMOD.Studio.EventInstance SlideInstance;
    float volume;
    // Start is called before the first frame update
    void Start()
    {
        player_Rigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<CapsuleCollider>();
        particulEffectAcceleration = Camera.main.transform.GetChild(0).gameObject;
        effectParticule = particulEffectAcceleration.GetComponent<ParticleSystem>().main;
        if (AerialSound != null)
        {
            AerialInstance = RuntimeManager.CreateInstance(AerialSound);
            RuntimeManager.AttachInstanceToGameObject(AerialInstance, transform, GetComponent<Rigidbody>());

        }
        if (SlideSound != null)
        {
            SlideInstance = RuntimeManager.CreateInstance(SlideSound);
            RuntimeManager.AttachInstanceToGameObject(SlideInstance, transform, gameObject.GetComponent<Rigidbody>());

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!MacroFunction.isPause)
        {
            if(resetAcceleration)
            {
                resetAcceleration = false;
                tempsEcouleAccelerationSlide = 0;
            }
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
            if (player_Surface == Player_Surface.Grounded && player_MotorMouvement == Player_MotorMouvement.Slide)
            {
                if (!checkSlide)
                {

                    checkSlide = true;
                    SlideInstance.setParameterByName("JumpOnSlide", 0.0f);
                    SlideInstance.setVolume(1);
                    Debug.Log((double)SlideInstance.getVolume(out volume));
                    SlideInstance.start();
                }

            }
            else
            {
                if (checkSlide)
                {
                    //SlideInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    volume -= 1.5f * Time.deltaTime;
                    Debug.Log("Volume is : " + volume);
                    if (volume > 0)
                    {
                        SlideInstance.setVolume(volume);
                    }
                    else
                    {
                        SlideInstance.setVolume(0f);
                        checkSlide = false;
                    }




                }

            }
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                //myCollider.height = 1;
                //myCollider.center = new Vector3(0,-0.5f,0);
                //Camera.main.transform.position = new Vector3(0, 0, 0);
                player_MotorMouvement = Player_MotorMouvement.Slide;
                particulEffectAcceleration.SetActive(true);
                resetAcceleration = true;

                //if (!checkAerial)
                //{
                //    checkAerial = true;
                //    AerialInstance.start();
                //    AerialInstance.setParameterByName("SpeedFaceOf", 0.6f);
                //
                //}

            }
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.Mouse1))
            {
                if(!resetAcceleration)
                {
                    tempsEcouleAccelerationSlide += Time.deltaTime;
                }
                player_Rigidbody.AddForce(transform.forward * accelerationOnSlide.Evaluate(tempsEcouleAccelerationSlide), ForceMode.Impulse);
                effectParticule.startSpeed = 410 * (accelerationOnSlide.Evaluate(tempsEcouleAccelerationSlide) / 10);


            }
            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.Mouse1))
            {
                //myCollider.height = 2;
                //myCollider.center = new Vector3(0, 0f, 0);
                //Camera.main.transform.position = new Vector3(0, 0.5f, 0);
                player_MotorMouvement = Player_MotorMouvement.Run;
                particulEffectAcceleration.SetActive(false);

                //if (checkAerial)
                //{
                //    checkAerial = false;
                //    AerialInstance.setParameterByName("SpeedFaceOf", 0f);
                //
                //}
            }   //
        }

    }
}
