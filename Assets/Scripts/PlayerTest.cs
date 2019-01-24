using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    //Declare public variables
    public Transform spline1;
    public Transform spline2;
    public Transform rightHandIK;
    public Transform leftHandIK;

    public float TransitionSpeed = 2.0f; //How many seconds for IK animation transition
    public float RotationSpeed = 2.0f; //Degrees per second
    public float CameraDivide = 5;
    public float CameraAdjustScalePerDivide = 2.0f;
    public float RotateSpeedPerDivide = 2.0f;

    //Declare State positions
    public Vector3 Leftspline1Positions;
    public Vector3 Leftspline2Positions;
    public Vector3 LeftrightHandIKpositions;
    public Vector3 LeftleftHandIKpositions;

    //Declare original positions
    private Vector3 spline1Orig;
    private Vector3 spline2Orig;
    private Vector3 rightHandIKOrig;
    private Vector3 leftHandIKOrig;
    private float DivideLength;
    private float MiddlePixel;

    private bool MoveToNextPoint = false;
    private Vector3 NextPoint;

    //Huehuehue :)
    void Start()
    {
        spline1Orig = spline1.transform.localPosition;
        spline2Orig = spline2.transform.localPosition;
        rightHandIKOrig = rightHandIK.transform.localPosition;
        leftHandIKOrig = leftHandIK.transform.localPosition;
        DivideLength = Camera.main.pixelWidth/2/CameraDivide; //Works out the number of pixels per camera divide
        MiddlePixel = Camera.main.pixelWidth / 2;
        Debug.Log(Camera.main.pixelWidth);
        Debug.Log(DivideLength);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //TODO: Calculate the mouse position offset from the center
            var mousePos = Input.mousePosition;
            var numDivides = (mousePos.x - MiddlePixel)/ DivideLength;
            Debug.Log("mousePosition: " + Input.mousePosition);


            //The aim for the player is to return the character to its original position
            var mouseAdjust = numDivides * CameraAdjustScalePerDivide;
            var angleAdjust = numDivides * RotateSpeedPerDivide;
            Debug.Log("angle Adjust: " + spline1Orig);

            spline1.localPosition = Vector3.MoveTowards(spline1.localPosition, spline1Orig, mouseAdjust);
            spline2.localPosition = Vector3.MoveTowards(spline2.localPosition, spline2Orig, mouseAdjust);
            rightHandIK.localPosition = Vector3.MoveTowards(rightHandIK.localPosition, rightHandIKOrig, mouseAdjust);
            leftHandIK.localPosition = Vector3.MoveTowards(leftHandIK.localPosition, leftHandIKOrig, mouseAdjust);
            transform.Rotate(Vector3.forward * -angleAdjust * Time.deltaTime);
        }

        //Make transition to target position for the amount of unbalanced weight
        spline1.localPosition = Vector3.MoveTowards(spline1.localPosition, Leftspline1Positions, TransitionSpeed);
        spline2.localPosition = Vector3.MoveTowards(spline2.localPosition, Leftspline2Positions, TransitionSpeed);
        rightHandIK.localPosition = Vector3.MoveTowards(rightHandIK.localPosition, LeftrightHandIKpositions, TransitionSpeed);
        leftHandIK.localPosition = Vector3.MoveTowards(leftHandIK.localPosition, LeftleftHandIKpositions, TransitionSpeed);
        transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);

    }
}
