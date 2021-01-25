using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player_Speed))]
[RequireComponent(typeof(Player_Input))]

public class Player_Slide : Player_Settings
{

    [Header("Input Variable")]
    public KeyCode pcInput;
    public KeyCode gamepadInput;

    public AnimationCurve accelerationOnSlide;

    private float tempsEcouleAccelerationSlide = 0;
    private bool resetAcceleration = false;
    private bool checkAerial = false;
    private bool checkSlide = false;

    //------- Variable -----------------
    [Header("Debug")]
    public bool activeDebug;

    [Header("Slider Parameter")]
    public float speedMinimum = 1f;
    [Range(0, 1)]
    public float colliderSlideHeight = 0.5f;
    public float accelerationTimer = 0.5f;
    public float speedGain = 15f;
    public float speedLose = 5f;
    public LayerMask layer;

    //---------- System Variable ---------

    public int descent = 0;
    public float accelerationCountdown = 0f;
    private float timeBeforeGain = 0.5f;
    private float trackOfCurrentTime = 0f;
    private bool addSpeed = false;

    //------- Sound Variable ---------
    [Header("Sound")]
    public float volume;

    [EventRef]
    public string AerialSound;
    FMOD.Studio.EventInstance AerialInstance;

    [EventRef]
    public string SlideSound;
    public static FMOD.Studio.EventInstance SlideInstance;

    // -------- Essential Component Reference ---------

    private Rigidbody rigidbodyPlayer;
    private Player_Input player_Input;
    private Player_Speed player_Speed;
    private Player_CheckState player_CheckState;
    private CapsuleCollider player_Collider;

    //--------- Additionel Component Reference -----------
    private GameObject particulEffectAcceleration;
    private ParticleSystem.MainModule effectParticule;
    private Camera_Controlle camera_Controller;

    // Start is called before the first frame update
    void Start()
    {
        InitReference();
        InitSound();
    }

    private void InitReference()
    {
        GetPlayerRigidBody(activeDebug);
        GetPlayerInput(activeDebug);
        GetPlayerCheckState(activeDebug);
        GetPlayerSpeed(activeDebug);
        GetPlayerCapsuleCollider(activeDebug);
        GetCameraController(activeDebug);
        GetSlideParticule(activeDebug);
    }

    private void InitSound()
    {
        InitSlideSound(activeDebug);
        InitAerialSound(activeDebug);
    }


    // Update is called once per frame
    void Update()
    {
        //Enter in the slide
        EnterCondition(activeDebug);

        // Exit the slide Pos
        if (!CheckObstacle(activeDebug))
        {
            if (!player_Input.GetInputPress(player_Input.SlidePc) && !player_Input.GetInputPress(player_Input.slideGp))
            {
                if (player_MotorMouvement == Player_MotorMouvement.Slide)
                {
                    ExitSlide(activeDebug);
                }
            }
        }


        //Behavior of slide
        descent = PlayerInDecent();

        if (player_MotorMouvement == Player_MotorMouvement.Slide)
        {

            // ------ Rotation du personnage ----------
            float front = CheckInputFront();
            SetPlayerRotatation(false, front);


            //------ Ranger dans des fonctions -----------
            if (accelerationCountdown < accelerationTimer)
            {
                if (descent <= 0)
                {
                    accelerationCountdown += Time.deltaTime;
                }
                player_Speed.AccelerationPlayerSpeed((speedGain / accelerationTimer));
                rigidbodyPlayer.velocity = transform.forward * player_Speed.currentSpeed;
                if (descent == 1)
                {

                    if (!addSpeed)
                    {
                        player_Speed.IncreamenteMaxSpeed();
                        addSpeed = true;
                        Debug.Log("Max Speed Increase = " + player_Speed.maximumSpeed);
                    }
                    if (addSpeed && Time.time > trackOfCurrentTime)
                    {
                        addSpeed = false;
                        trackOfCurrentTime = Time.time + timeBeforeGain;
                    }
                }
                if (activeDebug)
                    Debug.Log("Speed = " + player_Speed.currentSpeed);
            }
            else
            {
                if (descent == 1)
                {
                    accelerationCountdown = 0;
                }
                player_Speed.DeccelerationPlayerSpeed(speedLose);
                rigidbodyPlayer.velocity = transform.forward * player_Speed.currentSpeed;
                if (activeDebug)
                    Debug.Log("Speed = " + player_Speed.currentSpeed);
            }


        }



        // Bryan Code -----------------------
        //if (Input.GetKeyDown(pcInput) || Input.GetKeyDown(gamepadInput))
        //{
        //    player_MotorMouvement = Player_MotorMouvement.Slide;
        //    Debug.Log("Active Slide");


        //}

        //if (Input.GetKeyUp(pcInput) && Input.GetKeyUp(gamepadInput))
        //{
        //    player_MotorMouvement = Player_MotorMouvement.Run;
        //    Debug.Log("Déactiver Slide");


        //}

        ////-----------------------------------


        //if (!MacroFunction.isPause)
        //{
        //    if (resetAcceleration)
        //    {
        //        resetAcceleration = false;
        //        tempsEcouleAccelerationSlide = 0;
        //    }
        //    if (player_Surface == Player_Surface.Air || player_MotorMouvement == Player_MotorMouvement.Slide)
        //    {
        //        if (!checkAerial)
        //        {

        //            checkAerial = true;
        //            AerialInstance.start();
        //        }

        //    }
        //    else
        //    {
        //        if (checkAerial)
        //        {
        //            AerialInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //            checkAerial = false;
        //        }

        //    }
        //    if (player_Surface == Player_Surface.Grounded && player_MotorMouvement == Player_MotorMouvement.Slide)
        //    {
        //        if (!checkSlide)
        //        {

        //            checkSlide = true;

        //            SlideInstance.setParameterByName("JumpOnSlide", 0.0f);
        //            SlideInstance.setVolume(1);
        //            Debug.Log((double)SlideInstance.getVolume(out volume));
        //            SlideInstance.start();
        //        }

        //    }
        //    else
        //    {
        //        if (checkSlide)
        //        {
        //            //SlideInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //            volume -= 1.5f * Time.deltaTime;
        //            //Debug.Log("Volume is : " + volume);
        //            if (volume > 0)
        //            {
        //                SlideInstance.setVolume(volume);
        //            }
        //            else
        //            {
        //                SlideInstance.setVolume(0f);
        //                checkSlide = false;
        //            }




        //        }

        //    }
        //    if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Mouse1))
        //    {
        //        //myCollider.height = 1;
        //        //myCollider.center = new Vector3(0,-0.5f,0);
        //        //Camera.main.transform.position = new Vector3(0, 0, 0);
        //        player_MotorMouvement = Player_MotorMouvement.Slide;
        //        camera_Controller.offSetToMove = new Vector3(0, 0.25f, 0);
        //        camera_Controller.tempsTransition = 0.8f;
        //        if (particulEffectAcceleration != null)
        //        {
        //            particulEffectAcceleration.SetActive(true);
        //        }
        //        resetAcceleration = true;

        //        //if (!checkAerial)
        //        //{
        //        //    checkAerial = true;
        //        //    AerialInstance.start();
        //        //    AerialInstance.setParameterByName("SpeedFaceOf", 0.6f);
        //        //
        //        //}

        //    }
        //    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.Mouse1))
        //    {
        //        if (!resetAcceleration)
        //        {
        //            tempsEcouleAccelerationSlide += Time.deltaTime;
        //        }
        //        rigidbodyPlayer.AddForce(transform.forward * accelerationOnSlide.Evaluate(tempsEcouleAccelerationSlide), ForceMode.Impulse);
        //        if (particulEffectAcceleration != null)
        //        {
        //            effectParticule.startSpeed = 410 * (accelerationOnSlide.Evaluate(tempsEcouleAccelerationSlide) / 10);
        //        }


        //    }
        //    if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.Mouse1))
        //    {
        //        //myCollider.height = 2;
        //        //myCollider.center = new Vector3(0, 0f, 0);
        //        //Camera.main.transform.position = new Vector3(0, 0.5f, 0);
        //        player_MotorMouvement = Player_MotorMouvement.Run;
        //        camera_Controller.offSetToMove = new Vector3(0, 1f, 0);
        //        camera_Controller.tempsTransition = 0.5f;
        //        if (particulEffectAcceleration != null)
        //        {
        //            particulEffectAcceleration.SetActive(false);
        //        }
        //        //if (checkAerial)
        //        //{
        //        //    checkAerial = false;
        //        //    AerialInstance.setParameterByName("SpeedFaceOf", 0f);
        //        //
        //        //}
        //    }   //
        //}

    }

    #region Enter in Slide

    private void EnterCondition(bool debug)
    {
        if (player_Input.GetInputPress(player_Input.SlidePc) || player_Input.GetInputPress(player_Input.slideGp))
        {


            if (player_MotorMouvement != Player_MotorMouvement.Slide)
            {


                if (EnoughSpeed(debug))
                {
                    if (debug)
                        Debug.Log("Activation du slide");
                    ActivationOfSlide(debug);
                }
            }
        }
    }

    private void ActivationOfSlide(bool debug)
    {
        player_MotorMouvement = Player_MotorMouvement.Slide; // --- Changement d'état -------
        player_Collider.height = colliderSlideHeight; //-- Changement de taille du collider
        player_Collider.center = new Vector3(0f, -0.5f, 0f);
        if (debug)
            Debug.Log("Slide Activation !");
    }

    #endregion

    private bool EnoughSpeed(bool debug)
    {
        bool isEnoughSpeed = false;

        if (player_Speed.currentSpeed > speedMinimum)
        {
            isEnoughSpeed = true;
            if (debug)
                Debug.Log("Player has enough speed");
        }
        else
        {
            isEnoughSpeed = false;
            if (debug)
                Debug.Log("Player hasn't enough speed");
        }

        return isEnoughSpeed;
    }

    public void SetPlayerRotatation(bool debug, float front)
    {
        front = Mathf.Clamp(front, 0, 1.1f);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(transform.position + ((transform.forward * front).normalized * player_Speed.currentSpeed * Time.deltaTime), -Vector3.up, out hit, 3f, layer);

        Vector3 anglePlayer = Tool_SurfaceTopographie.GetTopo(hit.normal, transform, debug);

        transform.rotation = Quaternion.Euler(anglePlayer.x, transform.rotation.eulerAngles.y, anglePlayer.z);
        // Debug.Log("Hit is " + hit.collider.name);
        Debug.Log("X Angle = " + anglePlayer.x + " Z Angle = " + anglePlayer.z);
    }

    private int PlayerInDecent()
    {

        if (transform.eulerAngles.x < 180f && transform.eulerAngles.x > 5f)
        {
            return 1;

        }
        if (transform.eulerAngles.x > 180)
        {
            return -1;
        }
        return 0;


    }

    private float CheckInputFront()
    {
        float inputGet = 0;
        if (IsGamepad)
            inputGet = player_Input.GetAxeValue(player_Input.FrontAxisGp);
        else
            inputGet = player_Input.GetAxis(player_Input.forwardPc, player_Input.backPc);

        return inputGet;
    }


    #region ExitCondition


    private bool CheckObstacle(bool debug)
    {

        if (debug)
        {
            Debug.DrawRay(transform.position - (Vector3.up * 0.5f) + (transform.forward * -1f), Vector3.up * 100, Color.green);
            Debug.DrawRay(transform.position - (Vector3.up * 0.5f) + (transform.forward * 1f), Vector3.up * 100, Color.green);
        }
        if (Physics.Raycast(transform.position - (Vector3.up * 0.5f) + (transform.forward * -1f), Vector3.up, 3f) || Physics.Raycast(transform.position - (Vector3.up * 0.5f) + (transform.forward * 1f), Vector3.up, 3f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ExitSlide(bool debug)
    {
        player_MotorMouvement = Player_MotorMouvement.Null;
        player_Collider.center = new Vector3(0f, 0f, 0f);
        player_Collider.height = 2f;
        accelerationCountdown = 0f;
        Debug.Log("Exit Slide");
    }

    #endregion


    #region Get Reference Script

    private void GetPlayerSpeed(bool debug)
    {
        player_Speed = GetComponent<Player_Speed>();

        if (player_Speed != null)
        {
            if (debug)
            {
                Debug.Log("Player Speed Find");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Player Speed on the object");
            }
        }
    }

    private void GetPlayerInput(bool debug)
    {
        player_Input = GetComponent<Player_Input>();
        if (player_Input != null)
        {
            if (debug)
            {
                Debug.Log("Player Input Find");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Player Input on the object");
            }
        }
    }

    private void GetPlayerRigidBody(bool debug)
    {
        rigidbodyPlayer = GetComponent<Rigidbody>();
        if (rigidbodyPlayer != null)
        {
            if (debug)
            {
                Debug.Log("Rigidbody Find");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Player Rigidbody on the object");
            }
        }
    }

    private void GetPlayerCheckState(bool debug)
    {
        player_CheckState = GetComponent<Player_CheckState>();
        if (player_CheckState != null)
        {
            if (debug)
            {
                Debug.Log("Player Check State Find");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to Player Check Stateon the object");
            }
        }
    }

    private void GetPlayerCapsuleCollider(bool debug)
    {
        player_Collider = GetComponent<CapsuleCollider>();
        if (player_Collider != null)
        {
            if (debug)
            {
                Debug.Log("Player Capsule Collider Find");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to Player Capsule Colllider the object");
            }
        }

    }

    private void GetCameraController(bool debug)
    {
        camera_Controller = Camera.main.GetComponent<Camera_Controlle>();
        if (camera_Controller != null)
        {
            if (debug)
            {
                Debug.Log("Camera Controller Find");
            }
        }
        else
        {
            if (debug)
            {
                Debug.LogWarning("You need to put Camera Controller the main caméra");
            }
        }

    }

    private void GetSlideParticule(bool debug)
    {
        if (Camera.main.GetComponent<CameraVisualEffect>().slideParticule)
        {
            particulEffectAcceleration = Camera.main.GetComponent<CameraVisualEffect>().slideParticule;
            if (particulEffectAcceleration.GetComponent<ParticleSystem>())
            {
                effectParticule = particulEffectAcceleration.GetComponent<ParticleSystem>().main;
                if (debug)
                    Debug.Log("Slide Particule Setup");
            }
            else
            {
                if (debug)
                    Debug.Log("You need to put a particule System on the gameobject of Camera Visual Effect");
            }
        }
        else
        {
            if (debug)
                Debug.Log("You need to put Slide particule in the Camera Visual Effect on the main camera");
        }


    }


    #endregion

    #region Sound Initialisation

    private void InitSlideSound(bool debug)
    {
        if (SlideSound != null)
        {
            SlideInstance = RuntimeManager.CreateInstance(SlideSound);
            RuntimeManager.AttachInstanceToGameObject(SlideInstance, transform, gameObject.GetComponent<Rigidbody>());
            if (debug)
                Debug.Log("Slide Sound initiate");
        }
        else
        {
            if (debug)
                Debug.Log("No Sound found for Slide");
        }
    }

    private void InitAerialSound(bool debug)
    {
        if (AerialSound != null)
        {
            AerialInstance = RuntimeManager.CreateInstance(AerialSound);
            RuntimeManager.AttachInstanceToGameObject(AerialInstance, transform, GetComponent<Rigidbody>());
            if (debug)
                Debug.Log("Aerial Sound initiate");
        }
        else
        {
            if (debug)
                Debug.Log("No Sound found for Aerial");
        }

    }

    #endregion    
}
