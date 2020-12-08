using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public RectTransform[] levelList;
    public GameObject enterLevelButton;

    static public int currentLevel = 0;
    public int levelToGoOnSelection = 0;

    public float timeTravelBtwnScreen;

    private Vector3[] initialPosition = new Vector3[2];
    private bool isMovingNext = false;
    private bool isMovingLast = false;

    public float posXToVarie;
    // Start is called before the first frame update
    void Start()
    {
        enterLevelButton.transform.GetChild(0).GetComponent<Text>().text = "Enter level : " + (currentLevel + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingNext)
        {
            if (levelList[currentLevel].position.x - 1 > (initialPosition[0].x - posXToVarie))
            {
                levelList[currentLevel].position = new Vector3(Mathf.Lerp(levelList[currentLevel].position.x, initialPosition[0].x - posXToVarie, timeTravelBtwnScreen * Time.deltaTime), levelList[currentLevel].position.y, levelList[currentLevel].position.z);
                levelList[currentLevel + 1].position = new Vector3(Mathf.Lerp(levelList[currentLevel + 1].position.x, initialPosition[1].x - posXToVarie, timeTravelBtwnScreen * Time.deltaTime), levelList[currentLevel + 1].position.y, levelList[currentLevel + 1].position.z);
                Debug.Log(levelList[currentLevel].position.x + "//" + (initialPosition[0].x - posXToVarie));
            }
            else
            {
                isMovingNext = false;
                currentLevel = levelToGoOnSelection;
                enterLevelButton.transform.GetChild(0).GetComponent<Text>().text = "Enter level : " + (currentLevel + 1);
            }

            //if (Vector3.Distance(transform.position, listScreen[currentScreenToMoveOn].position) < Vector3.Distance(initialPosition, listScreen[currentScreenToMoveOn].position) / 2)
            //{
            //    Debug.Log("TestMoving");
            //    listScreen[currentScreenNumber].gameObject.SetActive(false);
            //    listScreen[currentScreenToMoveOn].gameObject.SetActive(true);
            //}
        }

        if (isMovingLast)
        {
            if (levelList[currentLevel].position.x + 1 < (initialPosition[0].x + posXToVarie))
            {
                levelList[currentLevel].position = new Vector3(Mathf.Lerp(levelList[currentLevel].position.x, initialPosition[0].x + posXToVarie, timeTravelBtwnScreen * Time.deltaTime), levelList[currentLevel].position.y, levelList[currentLevel].position.z);
                levelList[currentLevel - 1].position = new Vector3(Mathf.Lerp(levelList[currentLevel - 1].position.x, initialPosition[1].x + posXToVarie, timeTravelBtwnScreen * Time.deltaTime), levelList[currentLevel - 1].position.y, levelList[currentLevel - 1].position.z);
            }
            else
            {
                isMovingLast = false;
                currentLevel = levelToGoOnSelection;
                enterLevelButton.transform.GetChild(0).GetComponent<Text>().text = "Enter level : " + (currentLevel +1);
            }

            //if (Vector3.Distance(transform.position, listScreen[currentScreenToMoveOn].position) < Vector3.Distance(initialPosition, listScreen[currentScreenToMoveOn].position) / 2)
            //{
            //    Debug.Log("TestMoving");
            //    listScreen[currentScreenNumber].gameObject.SetActive(false);
            //    listScreen[currentScreenToMoveOn].gameObject.SetActive(true);
            //}
        }

    }

    public void MoveToNextLevelSelection()
    {
        if (!isMovingNext)
        {

            if (currentLevel + 1 <= levelList.Length -1)
            {
                isMovingNext = true;
                initialPosition[0] = levelList[currentLevel].position;
                initialPosition[1] = levelList[currentLevel + 1].position;
                levelToGoOnSelection = currentLevel + 1;
            }

        }

    }

    public void MoveToLastLevelSelection()
    {
        if (!isMovingLast)
        {

            if (currentLevel - 1 >= 0)
            {
                isMovingLast = true;
                initialPosition[0] = levelList[currentLevel].position;
                initialPosition[1] = levelList[currentLevel - 1].position;
                levelToGoOnSelection = currentLevel - 1;
            }

        }

    }
}
