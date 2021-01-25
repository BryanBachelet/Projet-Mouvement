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
    public float minLimiteSpeed = 10f;
    public float maximumSpeed = 10f;
    public float gainValueMaxSpeed = 10f;
    public float deccelerationMaxSpeed = 10f;

    [Header("Action Momentum")]
    public int actionSucceed = 0;
    public float timeMomentum = 5f;
    public float currentTimeMomentum = 0f;
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
        else
        {
            DecrementeMaxSpeed();
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

    public void DeccelerationPlayerSpeed(float decceleration)
    {
        currentSpeed -= decceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maximumSpeed);
    }

    /// <summary>
    /// Augmente la vitesse actuelle de l'avatar
    /// </summary>
    public void AccelerationPlayerSpeed()
    {
        currentSpeed += (accelerationSpeed * multiplicateurAcceleration) * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maximumSpeed);
    }

    public void AccelerationPlayerSpeed(float acceleration)
    {
        currentSpeed += (acceleration * multiplicateurAcceleration) * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maximumSpeed);
    }

    /// <summary>
    /// Augmente la vitesse maximum possible pour l'avatar
    /// </summary>
    /// <param name="timeValue"></param>
    public void IncreamenteMaxSpeed()
    {
        actionSucceed++;
        currentTimeMomentum = timeMomentum;
        maximumSpeed += gainValueMaxSpeed;
        momentumActive = true;
    }

    /// <summary>
    /// Réduit la vitesse maximum posssible de l'avatar
    /// </summary>
    public void DecrementeMaxSpeed()
    {
        
        maximumSpeed -= deccelerationMaxSpeed * Time.deltaTime;
        maximumSpeed = Mathf.Clamp(maximumSpeed, minLimiteSpeed, Mathf.Infinity);
        actionSucceed = (int)(maximumSpeed / gainValueMaxSpeed);
    }




}
