using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Jump : MonoBehaviour
{
    public float JumpValue;
    public  bool Grounded;
    public int JumpNumber;
    public int JumpCount;

    Collider m_Collider;
    RaycastHit m_Hit;
    // Start is called before the first frame update
    void Start()
    {
        m_Collider = gameObject.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Grounded || JumpCount < JumpNumber)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button4)|| Input.GetKeyDown(KeyCode.Space))
            {
                
                Player_BasicMouvement.rigidbodyPlayer.AddForce(transform.up * JumpValue, ForceMode.Impulse);
                Debug.Log("JUMP");
                JumpCount++;
            }
        }

        bool checkGround = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, -transform.up, out m_Hit, transform.rotation, 1);
        if (checkGround)
        {
            Grounded = true;
            JumpCount = 0;
            //Output the name of the Collider your Box hit
            Debug.Log("Hit : " + m_Hit.collider.name);
        }
        else
        {
            Grounded = false;
        }
    }
}
