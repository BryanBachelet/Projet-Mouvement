using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_SurfaceTopographie : MonoBehaviour
{
    public static Vector3 GetTopo(Vector3 normalSurface, Transform posStart, bool ActivateDebug)
    {
        // Get player direction
        // Get surfaceNormal 

        // Projetction Plan => Front 
        Vector3 frontProjection = Vector3.ProjectOnPlane(posStart.forward, normalSurface);
        // Projection Plan => Side;
        Vector3 sideProjection = Vector3.ProjectOnPlane(posStart.right, normalSurface);

        // Search Angle Front
        float frontAngle = Vector3.Angle(posStart.forward, frontProjection);
        // Search Angle Side
        float sideAngle = Vector3.Angle(posStart.right, sideProjection);

        Vector3 angle = new Vector3(sideAngle, 0, frontAngle);

        if (ActivateDebug)
        {

            //---------------- DEBUG ---------------------------
            Debug.DrawRay(posStart.position, sideProjection.normalized * 100, Color.green);
            Debug.DrawRay(posStart.position, frontProjection.normalized * 100, Color.magenta);
            Debug.Log("Front = " + frontAngle.ToString("F1") + "// Side = " + sideAngle.ToString("F1"));
            // -------------- DEBUG ----------------------------
        }

        //Return Value
        return angle;
    }
}
