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
        Vector3 frontProjection = Vector3.ProjectOnPlane(Quaternion.Euler(0,posStart.eulerAngles.y,0) *Vector3.forward  , normalSurface);
        // Projection Plan => Side;
        Vector3 sideProjection = Vector3.ProjectOnPlane(Quaternion.Euler(0, posStart.eulerAngles.y, 0) * Vector3.right, normalSurface);

        // Search Angle Front
        float frontAngle = Vector3.Angle(Quaternion.Euler(0, posStart.eulerAngles.y, 0) *Vector3.forward, frontProjection) ;
        
        // Search Angle Side
        float sideAngle = Vector3.Angle(Quaternion.Euler(0, posStart.eulerAngles.y, 0) * Vector3.right, sideProjection);

        Vector3 angle = new Vector3(Mathf.Sign(ConversionAngle(posStart.eulerAngles.y)) * frontAngle, 0, Mathf.Sign(ConversionAngle(posStart.eulerAngles.y)) * sideAngle);
     

        if (ActivateDebug)
        {

            //---------------- DEBUG ---------------------------
            Debug.DrawRay(posStart.position, sideProjection.normalized * 100, Color.green);
            Debug.DrawRay(posStart.position, frontProjection.normalized * 100, Color.magenta);
            //----Debug.Log("Front = " + frontAngle.ToString("F1") + "// Side = " + sideAngle.ToString("F1"));
            // -------------- DEBUG ----------------------------
        }

        //Return Value
        return angle;
    }
    public static float ConversionAngle(float angle)
    {

        if (angle > 180)
        {
            angle = angle - 360;
        }
        if (angle < -180)
        {
            angle = angle + 360;
        }

        return angle;
    }
}
