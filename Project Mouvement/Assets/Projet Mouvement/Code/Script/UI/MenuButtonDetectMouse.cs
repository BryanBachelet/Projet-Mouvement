using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonDetectMouse : MonoBehaviour
{
    public bool mouse_over = false;

    private NeonSpirteAnimation uiAssociateImg;
    EventSystem myEventSystem;

    private void Start()
    {
        myEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        uiAssociateImg = gameObject.GetComponent<NeonSpirteAnimation>();

    }
    void Update()
    {
        
        if (mouse_over)
        {
            Debug.Log("Mouse Over");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        uiAssociateImg.isChangingAnim = true;
        Debug.Log("Mouse enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        uiAssociateImg.isChangingAnim = false;
        Debug.Log("Mouse exit");
    }

    public void OnMouseOver()
    {
        mouse_over = true;
        uiAssociateImg.isChangingAnim = true;
        Debug.Log("Mouse enter");
    }

    public void OnMouseExit()
    {
        mouse_over = false;
        uiAssociateImg.isChangingAnim = false;
        Debug.Log("Mouse exit");
    }
}
