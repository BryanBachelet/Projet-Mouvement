using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Speed : MonoBehaviour
{
    [Header("Player Speed")]
    public float currentSpeed = 0f;
    public float accelerationSpeed = 10f;
    public float deccelerationSpeed = 10f;
    public float multiplicateurAcceleration = 1f;


    [Header("Maximum Speed")]
    public float minLimiteSpeed;
    public float maximumSpeed;
    public float gainValueMaxSpeed;
    public float deccelerationMaxSpeed;

   [Header("Action Momentum")]
    public int actionSucceed;
    public float timeMomentum;
    public float currentTimeMomentum;
    public bool momentumActive = false;


    public void Start()
    {
        momentumActive = false;
    }

    private void Update()
    {
        MomentumGestion();
       
    }

    /// <summary>
    /// Gestion du temps de momentum
    /// </summary>
    private void MomentumGestion()
    {
        if (momentumActive)
        {
            currentTimeMomentum -= Time.deltaTime;
            currentTimeMomentum = Mathf.Clamp(currentTimeMomentum, 0, timeMomentum);
            if (currentTimeMomentum == 0)
            {
                momentumActive = false;
            }
        }
    }

    /// <summary>
    /// Réduit la vitesse actuelle de l'avatar 
    /// </summary>
    public void DeccelerationPlayerSpeed()
    {
        currentSpeed -= deccelerationSpeed * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maximumSpeed);
    }

    /// <summary>
    /// Augmente la vitesse actuelle de l'avatar
    /// </summary>
    public void AccelerationPlayerSpeed()
    {
        currentSpeed += (accelerationSpeed * multiplicateurAcceleration) * Time.deltaTime ;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maximumSpeed);
    }

    /// <summary>
    /// Augmente la vitesse maximum possible pour l'avatar
    /// </summary>
    /// <param name="timeValue"></param>
    public void IncreamenteMaxSpeed(float timeValue)
    {
        actionSucceed++;
        currentTimeMomentum = timeMomentum;
        maximumSpeed += gainValueMaxSpeed * timeValue;
        momentumActive = true;
    }

    /// <summary>
    /// Réduit la vitesse maximum posssible de l'avatar
    /// </summary>
    public void DecrementeMaxSpeed()
    {
        maximumSpeed -= deccelerationMaxSpeed * Time.deltaTime;
        actionSucceed = (int)(maximumSpeed / gainValueMaxSpeed);
    }




}
