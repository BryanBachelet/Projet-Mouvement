using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    static bool startLoading = false;
    bool levelPrecise;
    static public int sceneToLoad = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startLoading)
        {
            if(levelPrecise)
            {
                StartCoroutine(LoadYourAsyncScene(LevelSelection.currentLevel + 1));
            }
            else
            {
                StartCoroutine(LoadYourAsyncScene(0));
            }



            startLoading = false;
        }
    }

    public  void LoadScene(bool preciseLevel)
    {
        levelPrecise = preciseLevel;
        startLoading = true;
    }
    public  void LeaveTheGame()
    {
        Application.Quit(); 
    }
    public IEnumerator LoadYourAsyncScene(int sceneNumber)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNumber);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            if(!levelPrecise)
            {
                LevelSelection.currentLevel = 0;
            }
            //FMODUnity.RuntimeManager.GetBus("Master").stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
            yield return null;
        }
    }
}
