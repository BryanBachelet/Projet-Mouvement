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
    // Start is called before the first frame update
    void Start()
    {
        pauseUIContainer.SetActive(isPause);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            tempsEcouleResetGame = 0;
        }
        else if(Input.GetKey(KeyCode.R))
        {
            tempsEcouleResetGame += Time.deltaTime;
            filledImageReset.fillAmount = tempsEcouleResetGame / timeResetGame;
            if (tempsEcouleResetGame >= timeResetGame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }
        else if(Input.GetKeyUp(KeyCode.R))
        {
            tempsEcouleResetGame = 0;
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
                Application.Quit();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            tempsEcouleLeaveGame = 0;
            filledImageLeave.fillAmount = tempsEcouleLeaveGame / timeLeaveGame;
        }
    }
}
