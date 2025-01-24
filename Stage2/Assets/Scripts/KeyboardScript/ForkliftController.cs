using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkliftController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float motorTorque = 100;
    [SerializeField] private float breakForce = 30;
    [SerializeField] private float maxSteerAngle = 45;

    [Header("Lift")]
    [SerializeField] private Transform lift;
    [SerializeField] private float speedLift;
    [SerializeField] private float maxDownLift = 2.2f;
    [SerializeField] private float maxUpLift = 9.5f;

    [Header("Colider")]
    [SerializeField] private WheelCollider frontLeftWheelColider;
    [SerializeField] private WheelCollider frontRightWheelColider;
    [SerializeField] private WheelCollider rearLeftWheelColider;
    [SerializeField] private WheelCollider rearRightWheelColider;

    [Header("Transform")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [Header("SteeringWheel")]
    [SerializeField] private Transform steeringWheelTransform;

    private float horizontalInput;
    private float verticalInput;
    private bool isBrake;

    private float brakeTorque;
    private float steerAngle;
    private float currentSteerAngle = 0; // 현재 핸들의 회전 각도

    private bool isLiftDown;
    private bool isLiftUp;

    private void FixedUpdate()
    {
        GetInput();
        HandleTorque();
        HandleSteering();
        UpdateWheelPosition();
        HandleLift();
    }

    private void GetInput()
    {
        horizontalInput = -Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBrake = Input.GetKey(KeyCode.Space);

        if (Input.GetKey(KeyCode.I))
        {
            isLiftUp = true;
            isLiftDown = false;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            isLiftUp = false;
            isLiftDown = true;
        }
        else
        {
            isLiftUp = false;
            isLiftDown = false;
        }
    }

    private void HandleTorque()
    {
        frontLeftWheelColider.motorTorque = verticalInput * motorTorque;
        frontRightWheelColider.motorTorque = verticalInput * motorTorque;
        rearLeftWheelColider.motorTorque = verticalInput * motorTorque;
        rearRightWheelColider.motorTorque = verticalInput * motorTorque;

        brakeTorque = (isBrake) ? breakForce : 0;
        frontLeftWheelColider.brakeTorque = brakeTorque;
        frontRightWheelColider.brakeTorque = brakeTorque;
        rearLeftWheelColider.brakeTorque = brakeTorque;
        rearRightWheelColider.brakeTorque = brakeTorque;
    }

    private void HandleSteering()
    {
        // 현재 핸들의 회전 각도를 업데이트
        currentSteerAngle += maxSteerAngle * horizontalInput * Time.deltaTime;
        currentSteerAngle = Mathf.Clamp(currentSteerAngle, -maxSteerAngle, maxSteerAngle);

        // 바퀴의 회전 각도를 업데이트
        rearLeftWheelColider.steerAngle = currentSteerAngle;
        rearRightWheelColider.steerAngle = currentSteerAngle;

        // 핸들의 회전 각도를 업데이트
        steeringWheelTransform.localRotation = Quaternion.Euler(0, 0, currentSteerAngle);
    }

    private void UpdateWheelPosition()
    {
        ChangeWheelPosition(frontLeftWheelColider, frontLeftWheelTransform);
        ChangeWheelPosition(frontRightWheelColider, frontRightWheelTransform);
        ChangeWheelPosition(rearLeftWheelColider, rearLeftWheelTransform);
        ChangeWheelPosition(rearRightWheelColider, rearRightWheelTransform);
    }

    private void ChangeWheelPosition(WheelCollider wheelColider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelColider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void HandleLift()
    {
        float y = lift.localPosition.y;
        if (isLiftUp)
        {
            y += speedLift * Time.deltaTime;
            y = Mathf.Clamp(y, maxDownLift, maxUpLift);

            lift.localPosition = new Vector3(lift.localPosition.x, y, lift.localPosition.z);
        }
        else if (isLiftDown)
        {
            y -= speedLift * Time.deltaTime;
            y = Mathf.Clamp(y, maxDownLift, maxUpLift);

            lift.localPosition = new Vector3(lift.localPosition.x, y, lift.localPosition.z);
        }
    }
}





