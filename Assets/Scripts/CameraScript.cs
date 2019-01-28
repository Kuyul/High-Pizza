using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float moveSpeed=1.5f;
    private float MaxCamOffset = 0.7f;
    private bool goLeft;
    private bool goRight;
    private bool goMiddle;

    private Vector3 localPosReference;

    private void Start()
    {
        localPosReference = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (goLeft)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(localPosReference.x - MaxCamOffset, transform.localPosition.y, transform.localPosition.z), moveSpeed * Time.deltaTime);
        }
        else if (goRight)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(localPosReference.x + MaxCamOffset, transform.localPosition.y, transform.localPosition.z), moveSpeed * Time.deltaTime);
        }
        else if (goMiddle)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(localPosReference.x , transform.localPosition.y, transform.localPosition.z), moveSpeed * Time.deltaTime);
        }
    }

    // moves camera back to origin
    public void MoveMiddle()
    {
        goLeft = false;
        goRight = false;
        goMiddle = true;
    }
    public void MoveLeft()
    {
        goLeft = true;
        goRight = false;
        goMiddle = false;
    }
    public void MoveRight()
    {      
        goLeft = false;
        goRight = true;
        goMiddle = false;
    }
}
