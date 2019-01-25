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

    //Declare (Left) State positions
    public Vector3[] Leftspline1Positions;
    public Vector3[] Leftspline2Positions;
    public Vector3[] LeftrightHandIKpositions;
    public Vector3[] LeftleftHandIKpositions;

    //Declare (Right) State positions
    public Vector3[] Rightspline1Positions;
    public Vector3[] Rightspline2Positions;
    public Vector3[] RightrightHandIKpositions;
    public Vector3[] RightleftHandIKpositions;

    //Declare original positions
    private Vector3 spline1Orig;
    private Vector3 spline2Orig;
    private Vector3 rightHandIKOrig;
    private Vector3 leftHandIKOrig;

    //Declare private variables
    private float DivideLength;
    private float MiddlePixel;
    private bool MoveToNextPoint = false;
    private Vector3 NextPoint;
    private int State = 0; //e.g State: 0 = balanced, -1 = pizza weight 1 to left, 2 = pizza weight 2 to right

    //Control 2 variables
    public float sensitivity = 50f;
    public float forceIncreaseFactor = 3.0f;
    private bool isDragging = false;
    private bool addingForce = false;
    private Vector3 initialTouchPos;
    private float timeApplied = 0;

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
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            initialTouchPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            /* Control 1 - increase force for amount off touch x position offset from center
            var mousePos = Input.mousePosition;
            var numDivides = (mousePos.x - MiddlePixel)/ DivideLength;

            //The aim for the player is to return the character to its original position
            var mouseAdjust = Mathf.Abs(numDivides * CameraAdjustScalePerDivide);
            var angleAdjust = numDivides * RotateSpeedPerDivide;
            */

            //Control 2
            var diff = Input.mousePosition.x - initialTouchPos.x;
            if (isDragging)
            {
                if(Mathf.Abs(diff) >= sensitivity)
                {
                    isDragging = false;
                    addingForce = true;
                    Debug.Log("Starting to add force...");
                }
            }

            if (addingForce)
            {
                timeApplied += Time.deltaTime * forceIncreaseFactor;
                var mouseAdjust = CameraAdjustScalePerDivide * timeApplied;
                var angleAdjust = RotateSpeedPerDivide * timeApplied;
                if(diff < 0)
                {
                    angleAdjust = -angleAdjust;
                }

                spline1.localPosition = Vector3.MoveTowards(spline1.localPosition, spline1Orig, mouseAdjust);
                Debug.Log("spline 1 localposition: " + spline1.localPosition);
                Debug.Log("spline 1 originalPosition: " + spline1Orig);
                spline2.localPosition = Vector3.MoveTowards(spline2.localPosition, spline2Orig, mouseAdjust);
                rightHandIK.localPosition = Vector3.MoveTowards(rightHandIK.localPosition, rightHandIKOrig, mouseAdjust);
                leftHandIK.localPosition = Vector3.MoveTowards(leftHandIK.localPosition, leftHandIKOrig, mouseAdjust);
                transform.Rotate(Vector3.forward * -angleAdjust * Time.deltaTime);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            addingForce = false;
            timeApplied = 1;
        }
        AdjustBalancingPosition();
    }

    //Depending on the degree of unbalance, adjust the spine and hand positions to a pre-designated localposition
    private void AdjustBalancingPosition()
    {
        //If the unbalance is heavier to the left
        if (State < 0)
        {
            var index = Mathf.Abs(State) - 1;
            //Make transition to target position for the amount of unbalanced weight
            spline1.localPosition = Vector3.MoveTowards(spline1.localPosition, Leftspline1Positions[index], TransitionSpeed);
            spline2.localPosition = Vector3.MoveTowards(spline2.localPosition, Leftspline2Positions[index], TransitionSpeed);
            rightHandIK.localPosition = Vector3.MoveTowards(rightHandIK.localPosition, LeftrightHandIKpositions[index], TransitionSpeed);
            leftHandIK.localPosition = Vector3.MoveTowards(leftHandIK.localPosition, LeftleftHandIKpositions[index], TransitionSpeed);
            transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
        }
        else if (State > 0)
        {
            var index = State - 1;
            //Make transition to target position for the amount of unbalanced weight
            spline1.localPosition = Vector3.MoveTowards(spline1.localPosition, Rightspline1Positions[index], TransitionSpeed);
            spline2.localPosition = Vector3.MoveTowards(spline2.localPosition, Rightspline2Positions[index], TransitionSpeed);
            rightHandIK.localPosition = Vector3.MoveTowards(rightHandIK.localPosition, RightrightHandIKpositions[index], TransitionSpeed);
            leftHandIK.localPosition = Vector3.MoveTowards(leftHandIK.localPosition, RightleftHandIKpositions[index], TransitionSpeed);
            transform.Rotate(Vector3.back * RotationSpeed * Time.deltaTime);
        }
    }

    //Add -1 to add weight to the left
    //Add +1 to add weight to the right
    public void AddState(int value)
    {
        State += value;
    }
}
