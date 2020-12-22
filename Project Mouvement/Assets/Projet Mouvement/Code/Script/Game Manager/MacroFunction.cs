using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MacroFunction : MonoBehaviour
{
    public int timeResetGame;
    public int timeLeaveGame;

    private float tempsEcouleResetGame;
    private float tempsEcouleLeaveGame;

    public Image filledImageReset;
    public Image filledImageLeave;
    public GameObject pauseUIContainer;

    public static bool isPause = false;

    public GameObject SceneLoaderObject;

    public AnimationCurve glitchEffect1;
    public AnimationCurve glitchEffect2;

    private Kino.AnalogGlitch camAnaGlitch;
    public bool isOut = false;
    // Start is called before the first frame update
    void Start()
    {
        camAnaGlitch = Camera.main.GetComponent<Kino.AnalogGlitch>();
        pauseUIContainer.SetActive(isPause);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -10)
        {
            camAnaGlitch.scanLineJitter = glitchEffect1.Evaluate(-transform.position.y / 33);
            camAnaGlitch.colorDrift = glitchEffect2.Evaluate(-transform.position.y / 33);
            if(transform.position.y < -100)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            tempsEcouleResetGame = 0;
        }
        else if(Input.GetKey(KeyCode.R))
        {
            tempsEcouleResetGame += Time.deltaTime;
            filledImageReset.fillAmount = tempsEcouleResetGame / timeResetGame;
            camAnaGlitch.scanLineJitter = glitchEffect1.Evaluate(tempsEcouleResetGame);
            camAnaGlitch.colorDrift = glitchEffect2.Evaluate(tempsEcouleResetGame);
            if (tempsEcouleResetGame >= timeResetGame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }
        else if(Input.GetKeyUp(KeyCode.R))
        {
            tempsEcouleResetGame = 0;
            camAnaGlitch.scanLineJitter = glitchEffect1.Evaluate(tempsEcouleResetGame);
            camAnaGlitch.colorDrift = glitchEffect2.Evaluate(tempsEcouleResetGame);
            filledImageReset.fillAmount = tempsEcouleResetGame / timeResetGame;

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            tempsEcouleLeaveGame = 0;
            if(isPause)
            {
                isPause = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                isPause = true;

            }
            pauseUIContainer.SetActive(isPause);
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            tempsEcouleLeaveGame += Time.deltaTime;
            filledImageLeave.fillAmount = tempsEcouleLeaveGame / timeLeaveGame;
            if (tempsEcouleLeaveGame >= timeLeaveGame)
            {
                SceneLoaderObject.GetComponent<SceneLoader>().LoadScene(false);
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            tempsEcouleLeaveGame = 0;
            filledImageLeave.fillAmount = tempsEcouleLeaveGame / timeLeaveGame;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "GameSpace")
        {
            isOut = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GameSpace")
        {
            isOut = false;
        }
    }
}
