﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //Declare public variables
    public Transform spline1;
    public Transform spline2;
    public Transform rightHandIK;
    public Transform leftHandIK;
    public Transform rightPizzaPlate;
    public Transform leftPizzaPlate;
    public Animator Anim;

    public float TransitionSpeed = 2.0f; //How many seconds for IK animation transition
    public float RotationSpeed = 2.0f; //Degrees per second
    public float CameraDivide = 5;
    public float CameraAdjustScalePerDivide = 2.0f;
    public float RotateSpeedPerDivide = 2.0f;
    public float FallingAngle = 30.0f;
    public float SplineRotationAdjustSpeed = 0.5f;
    public float WalkSpeed = 2.0f;
    public float WeightScale = 1.0f;
    public float StateChangeAngle = 3.0f;

    //Declare (Left) State positions
    public Vector3[] Leftspline1Positions;
    public Vector3[] Leftspline2Positions;
    public Vector3[] LeftleftHandIKpositions;
    public Vector3[] LeftrightHandIKpositions;

    //Declare (Right) State positions
    public Vector3[] Rightspline1Positions;
    public Vector3[] Rightspline2Positions;
    public Vector3[] RightleftHandIKpositions;
    public Vector3[] RightrightHandIKpositions;

    //Declare (Left) State rotations
    public Vector3[] LeftSpline1Rotations;
    public Vector3[] LeftSpline2Rotations;
    //Declare (Right) State rotations
    public Vector3[] RightSpline1Rotations;
    public Vector3[] RightSpline2Rotations;

    //Declare original positions
    private Vector3 spline1Orig;
    private Vector3 spline2Orig;
    private Vector3 rightHandIKOrig;
    private Vector3 leftHandIKOrig;

    //Declare private variables
    private Rigidbody rb;
    private float DivideLength;
    private float MiddlePixel;
    private Vector3 NextPoint;
    private int State = 0; //e.g State: 0 = balanced, -1 = pizza weight 1 to left, 2 = pizza weight 2 to right
    private float fallValue;

    //Control variables
    public bool control1;
    public bool release;
    public float sensitivity = 50f;
    public float forceIncreaseFactor = 3.0f;
    private bool isDragging = false;
    private bool addingForce = false;
    private Vector3 initialTouchPos;
    private float timeApplied = 0;
    private int Unbalance = 0;
    private float NumDivides = 0;

    //Huehuehue :)
    void Start()
    {
        spline1Orig = spline1.transform.localPosition;
        spline2Orig = spline2.transform.localPosition;
        rightHandIKOrig = rightHandIK.transform.localPosition;
        leftHandIKOrig = leftHandIK.transform.localPosition;
        DivideLength = Camera.main.pixelWidth/2/CameraDivide; //Works out the number of pixels per camera divide
        MiddlePixel = Camera.main.pixelWidth / 2;
        fallValue = Quaternion.Euler(0, 0, FallingAngle).z;
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.forward * WalkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //When rotated x degrees, call gameover
        if (Mathf.Abs(transform.rotation.z) > fallValue)
        {
            GameControl.instance.GameOver();
            if(transform.rotation.z > 0)
            {
                Anim.SetTrigger("fallLeft");
            }
            else
            {
                Anim.SetTrigger("fallRight");
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            initialTouchPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            // Control 1 - increase force for amount off touch x position offset from center

            if (control1)
            {

                if (!release)
                {
                    var mousePos = Input.mousePosition;
                    var numDivides = (mousePos.x - initialTouchPos.x) / DivideLength;
                    Debug.Log(numDivides);

                    var mouseAdjust = Mathf.Abs(numDivides * CameraAdjustScalePerDivide);
                    var angleAdjust = numDivides * RotateSpeedPerDivide;

                    spline1.localPosition = Vector3.MoveTowards(spline1.localPosition, spline1Orig, mouseAdjust);
                    spline2.localPosition = Vector3.MoveTowards(spline2.localPosition, spline2Orig, mouseAdjust);
                    rightHandIK.localPosition = Vector3.MoveTowards(rightHandIK.localPosition, rightHandIKOrig, mouseAdjust);
                    leftHandIK.localPosition = Vector3.MoveTowards(leftHandIK.localPosition, leftHandIKOrig, mouseAdjust);
                    transform.Rotate(Vector3.forward * -angleAdjust * Time.deltaTime);
                }else
                {
                    var mousePos = Input.mousePosition;
                    NumDivides += (mousePos.x - initialTouchPos.x) / DivideLength;
                    initialTouchPos = mousePos;
                }
            }
           
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                addingForce = false;
                timeApplied = 1;
            }
        }


        if (release)
        {
            //The aim for the player is to return the character to its original position
            var mouseAdjust = Mathf.Abs(NumDivides * CameraAdjustScalePerDivide);
            var angleAdjust = NumDivides * RotateSpeedPerDivide;

            spline1.localPosition = Vector3.MoveTowards(spline1.localPosition, spline1Orig, mouseAdjust);
            spline2.localPosition = Vector3.MoveTowards(spline2.localPosition, spline2Orig, mouseAdjust);
            rightHandIK.localPosition = Vector3.MoveTowards(rightHandIK.localPosition, rightHandIKOrig, mouseAdjust);
            leftHandIK.localPosition = Vector3.MoveTowards(leftHandIK.localPosition, leftHandIKOrig, mouseAdjust);
            transform.Rotate(Vector3.forward * -angleAdjust * Time.deltaTime);
        }

        //Depending on where the pizza unbalance is, rotate towards that side of the axis
        var RotateRate = RotationSpeed * Unbalance * WeightScale;
        transform.Rotate(Vector3.forward * RotateRate * Time.deltaTime);
        SetState();
        AdjustBalancingPosition();
    }

    //Depending on the degree of unbalance, adjust the spine and hand positions to a pre-designated localposition
    private void AdjustBalancingPosition()
    {
        var index = Mathf.Abs(State) - 1;
        var startPos = spline1Orig;
        var angle = transform.rotation.z;
        var value = Mathf.Abs(angle) / (Quaternion.Euler(0, 0, StateChangeAngle).z * 3);
        //If the unbalance is heavier to the left
        if (State < 0)
        {
            spline1.localPosition = value * (Leftspline1Positions[2] - startPos);
            spline2.localPosition = value * (Leftspline2Positions[2] - startPos);
            rightHandIK.localPosition = value * (LeftrightHandIKpositions[2] - startPos);
            leftHandIK.localPosition = value * (LeftleftHandIKpositions[2] - startPos);
            /*
            //Make transition to target position for the amount of unbalanced weight
            spline1.localPosition = Vector3.MoveTowards(spline1.localPosition, Leftspline1Positions[index], TransitionSpeed);
            spline2.localPosition = Vector3.MoveTowards(spline2.localPosition, Leftspline2Positions[index], TransitionSpeed);
            rightHandIK.localPosition = Vector3.MoveTowards(rightHandIK.localPosition, LeftrightHandIKpositions[index], TransitionSpeed);
            leftHandIK.localPosition = Vector3.MoveTowards(leftHandIK.localPosition, LeftleftHandIKpositions[index], TransitionSpeed);
            spline1.localRotation = Quaternion.Lerp(spline1.localRotation, Quaternion.Euler(LeftSpline1Rotations[index]), SplineRotationAdjustSpeed);
            spline2.localRotation = Quaternion.Lerp(spline2.localRotation, Quaternion.Euler(LeftSpline2Rotations[index]), SplineRotationAdjustSpeed);
            */
        }
        else if (State >= 0)
        {
            spline1.localPosition = value * (Rightspline1Positions[2] - startPos);
            spline2.localPosition = value * (Rightspline2Positions[2] - startPos);
            rightHandIK.localPosition = value * (RightrightHandIKpositions[2] - startPos);
            leftHandIK.localPosition = value * (RightleftHandIKpositions[2] - startPos);
            /*
            var index = State - 1;
            //Make transition to target position for the amount of unbalanced weight
            spline1.localPosition = Vector3.MoveTowards(spline1.localPosition, Rightspline1Positions[index], TransitionSpeed);
            spline2.localPosition = Vector3.MoveTowards(spline2.localPosition, Rightspline2Positions[index], TransitionSpeed);
            rightHandIK.localPosition = Vector3.MoveTowards(rightHandIK.localPosition, RightrightHandIKpositions[index], TransitionSpeed);
            leftHandIK.localPosition = Vector3.MoveTowards(leftHandIK.localPosition, RightleftHandIKpositions[index], TransitionSpeed);
            spline1.localRotation = Quaternion.Lerp(spline1.localRotation, Quaternion.Euler(RightSpline1Rotations[index]), SplineRotationAdjustSpeed);
            spline2.localRotation = Quaternion.Lerp(spline2.localRotation, Quaternion.Euler(RightSpline2Rotations[index]), SplineRotationAdjustSpeed);
            */
        }/*
        else
        {
            spline1.localPosition = Vector3.MoveTowards(spline1.localPosition, spline1Orig, TransitionSpeed);
            spline2.localPosition = Vector3.MoveTowards(spline2.localPosition, spline2Orig, TransitionSpeed);
            rightHandIK.localPosition = Vector3.MoveTowards(rightHandIK.localPosition, rightHandIKOrig, TransitionSpeed);
            leftHandIK.localPosition = Vector3.MoveTowards(leftHandIK.localPosition, leftHandIKOrig, TransitionSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, RotationSpeed * Time.deltaTime);
        }*/
    }

    //Add -1 to add weight to the left
    //Add +1 to add weight to the right
    public void AddState(int value)
    {
        State += value;
    }

    //Sets the state of the player
    public void SetState()
    {
        var angle = transform.rotation.z;
        var stateChangeAngle = Quaternion.Euler(0, 0, StateChangeAngle).z;
        var value = angle / stateChangeAngle;
        if (Mathf.Abs(value) <= Rightspline1Positions.Length)
        {
            State = -(int)(value + 1);
        }
    }

    public void AddRight()
    {
        Unbalance--;
        Debug.Log(Unbalance);
    }

    public void AddLeft()
    {
        Unbalance++;
        Debug.Log(Unbalance);
    }

    public int GetUnbalance()
    {
        return Unbalance;
    }
}
