using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_SurfaceTopographie : MonoBehaviour
{
    public static void GetTopo(Vector3 normalSurface , Transform posStart)
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

        //---------------- DEBUG ---------------------------
        //Debug.DrawRay(posStart.position, sideProjection.normalized * 100, Color.green);
        //Debug.DrawRay(posStart.position, frontProjection.normalized * 100, Color.magenta);
        //Debug.DrawRay(posStart.position, player2dDirection * 100, Color.yellow);
        //Debug.Log("Front = " + frontAngle.ToString("F1") + "// Side = " + sideAngle.ToString("F1"));
        // -------------- DEBUG ----------------------------
        
        // Return value
    }
}
