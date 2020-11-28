using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraVisualEffect : Player_Settings
{
    public PostProcessVolume myVolumePPP;
    Camera myCameraComponent;
    [Header("ChromatiqueAberration")]
    public ChromaticAberration myC_AIntensity;
    public AnimationCurve ani_CurveSpeed;
    private float tempsEcouleSpeed;
    public float timeMaxSpeed = 1f;

    public bool resetSpeed = false;

    [Header("FieldsOfView")]
    public AnimationCurve ani_CurveAcceleration;
    private float ValueAcceleration;
    public float timeMaxAcceleration = 1f;

    private bool resetAcceleration = false;
    // Start is called before the first frame update
    void Start()
    {
        //myVolumePPP = gameObject.GetComponent<PostProcessVolume>();
        myVolumePPP.profile.TryGetSettings(out myC_AIntensity);
        myCameraComponent = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(resetSpeed)
        {
            tempsEcouleSpeed = 0;
            resetSpeed = false;
        }
        if(tempsEcouleSpeed < timeMaxSpeed)
        {
            tempsEcouleSpeed += Time.deltaTime;
        }
        myC_AIntensity.intensity.value = ani_CurveSpeed.Evaluate(tempsEcouleSpeed);
    }

    public void FieldOfViewValue(float currentSpeed)
    {
        myCameraComponent.fieldOfView = ani_CurveAcceleration.Evaluate(currentSpeed);
    }
    
    


}
