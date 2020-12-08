using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class NeonSpirteAnimation : MonoBehaviour
{
    public Texture[] textureNeonSheet;
    private int sheetLenght;

    public float tempsAnimation;
    private float tempsEcouleAnimation;

    private Material myCurrentMaterial;
    private int compteurAnimation;

    public bool isStopping = false;
    public bool isChangingAnim = true;
    public int rndMaxStopAdd;
    int addToStop;
    bool launchSound = false;
    bool isStop = false;
    private float TempsEcouleStop;
    public float stopTime = 10f;

    public Color initialMainColor;
    public Color initialRimColor;
    Renderer myRenderer;

    [EventRef]
    public string eventSound;

    FMOD.Studio.EventInstance neonSoundInstance;
    //public sprite
    // Start is called before the first frame update
    void Start()
    {
        addToStop = Random.Range(-3, rndMaxStopAdd);
        sheetLenght = textureNeonSheet.Length;
        Debug.Log(sheetLenght);

        myRenderer = gameObject.GetComponent<Renderer>();
        myCurrentMaterial = myRenderer.material;

        initialMainColor = myRenderer.material.GetColor("_MainColor");
        initialRimColor = myRenderer.material.GetColor("_RimColor");
        //myRenderer.material.shader = Shader.Find("MainColor");
        //myRenderer.material.shader = Shader.Find("RimColor");
        //Debug.Log(myRenderer.material.GetColor("_MainColor") + myRenderer.material.GetColor("_RimColor"));
        if(eventSound != null)
        {
            neonSoundInstance = RuntimeManager.CreateInstance(eventSound);
            RuntimeManager.AttachInstanceToGameObject(neonSoundInstance, transform, GetComponent<Rigidbody>());
            neonSoundInstance.start();
        }
        //



    }

    // Update is called once per frame
    void Update()
    {
        if(isChangingAnim)
        {
            tempsEcouleAnimation += Time.deltaTime;
            if (tempsEcouleAnimation >= tempsAnimation / sheetLenght)
            {
                if (compteurAnimation + 1 < sheetLenght)
                {
                    compteurAnimation += 1;
                }
                else
                {
                    compteurAnimation = 0;
                }
                tempsEcouleAnimation = 0;
                myCurrentMaterial.mainTexture = textureNeonSheet[compteurAnimation];
            }
        }

        if (isStopping)
        {
            TempsEcouleStop += Time.deltaTime;
            if (TempsEcouleStop > stopTime + addToStop)
            {
                if(!isStop)
                {
                    isStop = true;
                    myRenderer.material.SetColor("_RimColor", Color.black);
                    myRenderer.material.SetColor("_MainColor", Color.black);
                    transform.GetChild(0).gameObject.SetActive(true);
                    neonSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    //Debug.Log(TempsEcouleStop + "Inférieure");
                }
                if (TempsEcouleStop > stopTime  + addToStop + 2)
                {

                    myRenderer.material.SetColor("_MainColor", Color.Lerp(myRenderer.material.GetColor("_MainColor"), initialMainColor, 0.02f));
                    myRenderer.material.SetColor("_RimColor", Color.Lerp(myRenderer.material.GetColor("_RimColor"), initialRimColor, 0.02f));
                    if (!launchSound)
                    {
                        neonSoundInstance.start();
                        launchSound = true;
                    }
                    //Debug.Log(TempsEcouleStop + "Supérieure" + myRenderer.material.GetColor("_MainColor"));
                    //Debug.Log(TempsEcouleStop + "Supérieure" + myRenderer.material.GetColor("_RimColor"));

                }
                if(TempsEcouleStop > (stopTime  + addToStop + 2 )* 1.3f)
                {
                    TempsEcouleStop = 0;
                    //Debug.Log(TempsEcouleStop + "Reset");
                    transform.GetChild(0).gameObject.SetActive(false);
                    isStop = false;
                    launchSound = false;
                }



            }
        }

    }
}
