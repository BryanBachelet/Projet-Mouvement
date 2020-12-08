using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigation : MonoBehaviour
{
    public Transform[] listScreen;
    public float timeTravelBtwnScreen;

    private Vector3 initialPosition;
    private bool isMoving = false;
    private int currentScreenToMoveOn = 0;
    private int currentScreenNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            if (Vector3.Distance(transform.position, listScreen[currentScreenToMoveOn].position) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, listScreen[currentScreenToMoveOn].position, timeTravelBtwnScreen * Time.deltaTime);
            }
            else
            {
                isMoving = false;
                currentScreenNumber = currentScreenToMoveOn;
            }

            if (Vector3.Distance(transform.position, listScreen[currentScreenToMoveOn].position) < Vector3.Distance(initialPosition, listScreen[currentScreenToMoveOn].position) / 2)
            {
                Debug.Log("TestMoving");
                listScreen[currentScreenNumber].gameObject.SetActive(false);
                listScreen[currentScreenToMoveOn].gameObject.SetActive(true);
            }
        }

    }

    public void MoveToScreen(int screenNumber)
    {
        if(!isMoving)
        {
            isMoving = true;
            initialPosition = transform.position;
            currentScreenToMoveOn = screenNumber;
        }

    }
}
