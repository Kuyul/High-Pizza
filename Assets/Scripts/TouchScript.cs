using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchScript : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public CameraScript CamScript;

    private bool dragging;

    private Vector2 currentPos;
    private Vector2 originPos;

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        originPos = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }

    private void Update()
    {
        if (dragging)
        {
            currentPos = Input.mousePosition;
            if (originPos.x - currentPos.x > 0)
            {
                CamScript.MoveLeft();
            }
            else
            {
                CamScript.MoveRight();
            }
        }
        else
        {
            CamScript.MoveMiddle();
        }
    }
}
