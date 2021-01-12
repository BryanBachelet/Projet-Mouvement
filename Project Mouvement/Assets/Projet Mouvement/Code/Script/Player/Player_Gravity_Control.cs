using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Player_Gravity_Control : Player_Settings
{
    //------ Variable ------ 
    [Header("Debug")]
    public bool debugActive;

    [Header("Gravity Parameter")]
    [Range(0, -10f)]
    public float fallValueMin = -3;
    public float gainGravitySpeed = 10f;
    public float maxGravitySpeed = 50f;

    [EventRef]
    public string groundedSound;


    //----- Son ---- 
    private FMOD.Studio.EventInstance groundedSound_Fmod;

    //----- Reference -------
    private Player_CheckState player_CheckState;
    private Rigidbody player_rigidbody;





    // Start is called before the first frame update
    void Start()
    {
        groundedSound_Fmod = RuntimeManager.CreateInstance(groundedSound);
        GetPlayerCheckState(debugActive);
        GetPlayerRigidBody(debugActive);
    }

    // Update is called once per frame
    void Update()
    {

        // Check if le player est dans les air 
        // Check if la velocité y du player est supérieur à une valeur (Variable) 
        if (player_Surface == Player_Surface.Air && player_rigidbody.velocity.y < fallValueMin)
        {
            // Le player state passe en état de fall 
            player_MouvementUp = Player_MouvementUp.Fall;
           
        }

        // Check if le player state est en état de Fall
        if (player_MouvementUp == Player_MouvementUp.Fall)
        {
            // Augmentation du gain de vitesse de chute ( Variable) 
            player_rigidbody.velocity += Vector3.down * gainGravitySpeed * Time.deltaTime;
            // Vitesse de chute limité à un chiffre max ( Variable)
            float downVelocity = OneNumberUnderClamp(player_rigidbody.velocity.y, -maxGravitySpeed);

            player_rigidbody.velocity = new Vector3(player_rigidbody.velocity.x, downVelocity, player_rigidbody.velocity.z);

            // Check si le player va bientôr entré en  contact avec un objet

          
            
            // Alors Snap sur l'object
            Vector3 downPlayer = new Vector3(0, player_rigidbody.velocity.y, 0);

            if (GetVerticalCollision(debugActive, true, downPlayer.magnitude * Time.deltaTime))
            {
                groundedSound_Fmod.start();
            }

        }



    }


    //----------- Tools ----------
    private float OneNumberUnderClamp(float number, float minNumber)
    {
        float currrentNumber = number;
        if (currrentNumber < minNumber)
        {
            currrentNumber = minNumber;
        }

        return currrentNumber;
    }

    public bool GetVerticalCollision(bool debug)
    {
        bool collisionDectection = false;

        if (debug) Debug.DrawRay(transform.position - 0.9f * Vector3.up, transform.up * 100, Color.red);
        collisionDectection = Physics.Raycast(transform.position - (Vector3.up * 0.5f), transform.up * -1f, 10f);

        return collisionDectection;
    }

    public bool GetVerticalCollision(bool debug, bool activeSnap, float snapValue)
    {
        bool collisionDectection = false;


        if (debug) Debug.DrawRay(transform.position - 0.9f * Vector3.up, transform.up * 100, Color.red);
        RaycastHit hit = new RaycastHit();
        collisionDectection = Physics.Raycast(transform.position - (Vector3.up * 0.5f), transform.up * -1f, out hit, 10f);

        GetSnapPlayer(snapValue, hit, this.transform, this.transform.position, debug);

        return collisionDectection;
    }

    public bool GetHozirontalCollision(Vector3 direction, float speed, bool debug)
    {
        bool IsDectect = false;
        if (debug) Debug.DrawRay(transform.position - 0.9f * Vector3.up, (direction.normalized * 100), Color.red);
        IsDectect = Physics.Raycast(transform.position - (0.8f * Vector3.up), (direction.normalized * speed), 1.1f);

        if (IsDectect) return IsDectect;
        if (debug) Debug.DrawRay(transform.position + Vector3.up, direction.normalized * 100, Color.red);

        IsDectect = Physics.Raycast(transform.position + transform.up, (direction.normalized * speed), 1.1f);
        if (debug) Debug.Log(IsDectect);

        return false;
    }

    public bool GetSnapPlayer(float snapDistanceValue, RaycastHit hit, Transform playerPos, Vector3 objectPos, bool debug)
    {
        snapDistanceValue = snapDistanceValue < 1.5f ? 1.5f : snapDistanceValue;
     
        if (Vector3.Distance(objectPos, hit.point) < snapDistanceValue)
        {
            // Change Player pos 
            playerPos.position = hit.point + Vector3.up * 1f;

            // Change player velocity y to  0
            player_rigidbody.velocity = new Vector3(player_rigidbody.velocity.x, 0, player_rigidbody.velocity.z);

            // Change Player state du mouvement vertical
            player_MouvementUp = Player_MouvementUp.Null;
          
           // Debug.Break();
            if (debug) Debug.Log("Object has been snap");

            return true;
        }
        else
        {


            return false;
        }
    }

    #region Get Reference Function

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
                Debug.LogWarning("You need to put Player Check State on the object");
            }
        }
    }

    private void GetPlayerRigidBody(bool debug)
    {

        player_rigidbody = GetComponent<Rigidbody>();

        if (player_rigidbody != null)
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
                Debug.LogWarning("You need to put Rigidbody on the object");
            }
        }
    }

    #endregion



}
