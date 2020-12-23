using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GeneralFunction : MonoBehaviour
{

    private static Rigidbody GetPlayerRigidBody(GameObject objectAim, Rigidbody rigidbody, bool debug)
    {
        rigidbody = objectAim.GetComponent<Rigidbody>();
        if (rigidbody != null)
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
        return rigidbody;
    }
}
