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
        Vector3 frontProjection = Vector3.ProjectOnPlane(Quaternion.Euler(0, PositifAngle(posStart.eulerAngles.y), 0) * Vector3.forward, normalSurface);
        // Projection Plan => Side;
        Vector3 sideProjection = Vector3.ProjectOnPlane(Quaternion.Euler(0, PositifAngle(posStart.eulerAngles.y), 0) * Vector3.right, normalSurface);

        // Search Angle Front
        float sideAngle = Vector3.SignedAngle(frontProjection, Quaternion.Euler(0, PositifAngle(posStart.eulerAngles.y), 0) * Vector3.forward, Vector3.right);

        // Search Angle Side
        float frontAngle = Vector3.SignedAngle(sideProjection, Quaternion.Euler(0, PositifAngle(posStart.eulerAngles.y), 0) * Vector3.right, Vector3.forward);

        float side = CheckVectorUp(sideProjection);
        float front = CheckVectorUp(frontProjection);

        Vector3 angler = (Quaternion.FromToRotation(posStart.up, normalSurface) * posStart.rotation).eulerAngles;
        Vector3 angle = new Vector3(angler.x, 0, angler.z);


        if (ActivateDebug)
        {

            //---------------- DEBUG ---------------------------
            Debug.DrawRay(posStart.position, sideProjection.normalized * 100, Color.green);
            Debug.DrawRay(posStart.position, frontProjection.normalized * 100, Color.magenta);
            Debug.Log("Side = " + angle.x.ToString("F1") + "// Front = " + angle.z.ToString("F1"));
            // -------------- DEBUG ----------------------------
        }

        //Return Value
        return angle;
    }
    public static float CheckVectorUp(Vector3 vectorGive)
    {
        if (vectorGive.y > 0)
        {
            return -1f;
        }
        if (vectorGive.y < 0 || vectorGive.y == 0)
        {
            return 1f;
        }

        return 1f;
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
    private static float PositifAngle(float angle)
    {
        if (angle < 0)
        {
            angle = 360 - angle;
        }

        return angle;
    }
}
