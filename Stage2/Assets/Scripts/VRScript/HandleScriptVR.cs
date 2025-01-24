using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandleScript : MonoBehaviour
{
    [Header("Haptic Feedback")]
    public SteamVR_Action_Vibration hapticAction; // ���� �׼�

    [Header("LeverPosition")]
    public Transform gearLeverPosition; // ��� ���� ��ġ
    public Transform liftLeverPosition; // ����Ʈ ���� ��ġ
    public Transform tiltLeverPosition; // ƿƮ ���� ��ġ
    public Transform sideBreakLeverPosition; // ���̵�극��ũ ���� ��ġ

    [Header("Setting")]
    [SerializeField] private float motorTorque = 50; // ���� ��ũ
    [SerializeField] private float breakForce = 300; // �극��ũ ��
    [SerializeField] private float maxSteerAngle = 45; // �ִ� ���� ����

    [Header("SteamVR Input Actions")]
    public SteamVR_Action_Boolean RightTrackBtn = SteamVR_Input.GetBooleanAction("RightTrackBtn"); // ���� ��ư
    public SteamVR_Action_Boolean LeftTrackBtn = SteamVR_Input.GetBooleanAction("LeftTrackBtn");  // ���� ��ư

    [Header("SteeringWheel")]
    [SerializeField] private Transform steeringWheelTransform; // �ڵ� Ʈ������

    [Header("Colider")]
    [SerializeField] private WheelCollider frontLeftWheelColider; // �� ���� ���� �ݶ��̴�
    [SerializeField] private WheelCollider frontRightWheelColider; // �� ������ ���� �ݶ��̴�
    [SerializeField] private WheelCollider rearLeftWheelColider; // �� ���� ���� �ݶ��̴�
    [SerializeField] private WheelCollider rearRightWheelColider; // �� ������ ���� �ݶ��̴�

    [Header("Transform")]
    [SerializeField] private Transform frontLeftWheelTransform; // �� ���� ���� Ʈ������
    [SerializeField] private Transform frontRightWheelTransform; // �� ������ ���� Ʈ������
    [SerializeField] private Transform rearLeftWheelTransform; // �� ���� ���� Ʈ������
    [SerializeField] private Transform rearRightWheelTransform; // �� ������ ���� Ʈ������

    [Header("Lift")]
    [SerializeField] private Transform lift; // ����Ʈ Ʈ������
    [SerializeField] private float speedLift = 0.75f; // ����Ʈ �ӵ�
    [SerializeField] private float maxDownLift = 2.2f; // ����Ʈ �ִ� �ϰ� ����
    [SerializeField] private float maxUpLift = 9.5f; // ����Ʈ �ִ� ��� ����

    [Header("Tilt")]
    [SerializeField] private float speedTilt = 1.5f; // ƿƮ �ӵ�
    [SerializeField] private float tiltMaxAngle = 3f;
    [SerializeField] private float tiltMinAngle = -10f;

    float verticalInput; // ���� �Է� ��
    string gearLeverLastState = ""; // ���� ���� ���� ����
    string liftLeverLastState = ""; // ���� ����Ʈ���� ���� ����
    string tiltLeverLastState = ""; // ���� ƿƮ���� ���� ����
    string sideBreakLeverLastState = ""; // ���� ���̵�극��ũ���� ���� ����

    private bool isLiftLeverDown; // ����Ʈ���� �ϰ� ����
    private bool isLiftLeverUp; // ����Ʈ���� ��� ����

    private bool isTiltLeverDown; // ƿƮ���� �ϰ� ����
    private bool isTiltLeverUp; // ƿƮ���� ��� ����

    private bool isSideBreakLeverDown; // ���̵�극��ũ���� �ϰ� ����
    private bool isSideBreakLeverUp; // ���̵�극��ũ���� ��� ����

    private bool isBrakeApplied; // �Ϲ� �극��ũ ���� ����

    private void FixedUpdate()
    {
        HandleSteering(); // ���� ó��
        UpdateWheelTransforms(); // ���� Ʈ������ ������Ʈ
        GearLeverState(); // ��� ���� ���� ó��
        LiftLeverState(); // ����Ʈ ���� ���� ó��
        TiltLeverState(); // ƿƮ ���� ���� ó��
        SideBreakLeverState(); // ���̵�극��ũ ���� ���� ó��
        HandleTorque(); // ��ũ ó��
        MoveLift(); // ����Ʈ �̵� ó��
        useTilt(); // ƿƮ ��� ó��
    }

    private void HandleTorque()
    {
        // �� ������ ���� ��ũ ����
        frontLeftWheelColider.motorTorque = verticalInput * motorTorque;
        frontRightWheelColider.motorTorque = verticalInput * motorTorque;
        rearLeftWheelColider.motorTorque = verticalInput * motorTorque;
        rearRightWheelColider.motorTorque = verticalInput * motorTorque;
        Debug.Log("motorTorque : " + verticalInput * motorTorque);
        Debug.Log("breakForce" + breakForce);
    }

    private void HandleSteering()
    {
        // �ڵ��� Z ȸ������ �������� ���� ���
        float steeringZRotation = steeringWheelTransform.localEulerAngles.z;

        // �ڵ� ȸ������ 0 ~ 360���� -180 ~ 180 ������ ��ȯ
        if (steeringZRotation > 180)
            steeringZRotation -= 360;

        // -60 ~ 60 ������ ȸ������ ������ ���� ������ ����
        float steerAngle = Mathf.Clamp((steeringZRotation / 60f) * maxSteerAngle, -maxSteerAngle, maxSteerAngle);

        // �޹����� ���� ���� ����
        rearLeftWheelColider.steerAngle = steerAngle;
        rearRightWheelColider.steerAngle = steerAngle;
    }

    private void GearLeverState()
    {
        // ��� ������ ���� ��ġ�� �������� ���� ����
        float gearLeverCurrentPosition = gearLeverPosition.localPosition.z;
        string gearLeverCurrentState = "";

        if (gearLeverCurrentPosition >= -2.50f && gearLeverCurrentPosition <= -2.20f)   // ���� ����
        {
            gearLeverCurrentState = "Reverse";
            Debug.Log("���ӷ����� ���� �����Դϴ�.");
            HandleTrackpadInput(-1f); // ����
        }
        else if (gearLeverCurrentPosition >= -3.50f && gearLeverCurrentPosition <= -2.51f)  // �߸� ����
        {
            gearLeverCurrentState = "Neutral";
            Debug.Log("���ӷ����� �߸� �����Դϴ�.");
            ApplyBrake(true, isSideBreakLeverDown); // �߸� ���¿����� �극��ũ
        }
        else if (gearLeverCurrentPosition >= -3.80f && gearLeverCurrentPosition <= -3.51f)  // ���� ����
        {
            gearLeverCurrentState = "Forward";
            Debug.Log("���ӷ����� ���� �����Դϴ�.");
            HandleTrackpadInput(1f); // ����
        }

        // ��� ���°� ����Ǿ��� ���� ���� ����
        if (gearLeverCurrentState != gearLeverLastState)
        {
            TriggerHapticFeedback(); // ���� ����
            Debug.Log($"���� ���� ����: {gearLeverCurrentState}");
        }

        gearLeverLastState = gearLeverCurrentState; // ���� ���� ����
    }

    private void LiftLeverState()
    {
        // ����Ʈ ������ ���� ��ġ�� �������� ���� ����
        float liftLeverCurrentPosition = liftLeverPosition.localPosition.z;
        string liftLeverCurrentState = "";

        if (liftLeverCurrentPosition >= -2.50f && liftLeverCurrentPosition <= -2.20f)   // ����Ʈ ���
        {
            isLiftLeverUp = false;
            isLiftLeverDown = true;
            liftLeverCurrentState = "Reverse";
            Debug.Log("����Ʈ������ ���� �����Դϴ�.");
        }
        else if (liftLeverCurrentPosition >= -3.50f && liftLeverCurrentPosition <= -2.51f)  // �߸� ����
        {
            isLiftLeverUp = false;
            isLiftLeverDown = false;
            liftLeverCurrentState = "Neutral";
            Debug.Log("����Ʈ������ �߸� �����Դϴ�.");
        }
        else if (liftLeverCurrentPosition >= -3.80f && liftLeverCurrentPosition <= -3.51f)  // ����Ʈ �ϰ�
        {
            isLiftLeverUp = true;
            isLiftLeverDown = false;
            liftLeverCurrentState = "Forward";
            Debug.Log("����Ʈ������ ���� �����Դϴ�.");
        }

        // ����Ʈ ���°� ����Ǿ��� ���� ���� ����
        if (liftLeverCurrentState != liftLeverLastState)
        {
            TriggerHapticFeedback(); // ���� ����
            Debug.Log($"���� ���� ����: {liftLeverCurrentState}");
        }

        liftLeverLastState = liftLeverCurrentState; // ���� ���� ����
    }

    private void TiltLeverState()
    {
        // ƿƮ ������ ���� ��ġ�� �������� ���� ����
        float tiltLeverCurrentPosition = tiltLeverPosition.localPosition.z;
        string tiltLeverCurrentState = "";
        if (tiltLeverCurrentPosition >= -2.50f && tiltLeverCurrentPosition <= -2.20f)   // Fork �ȱ�
        {
            isTiltLeverUp = false;
            isTiltLeverDown = true;
            tiltLeverCurrentState = "Reverse";
            Debug.Log("ƿƮ������ ���� �����Դϴ�.");
        }
        else if (tiltLeverCurrentPosition >= -3.50f && tiltLeverCurrentPosition <= -2.51f)  // Fork �߸�
        {
            isTiltLeverUp = false;
            isTiltLeverDown = false;
            tiltLeverCurrentState = "Neutral";
            Debug.Log("ƿƮ������ �߸� �����Դϴ�.");
        }
        else if (tiltLeverCurrentPosition >= -3.80f && tiltLeverCurrentPosition <= -3.51f)  // Fork ������
        {
            isTiltLeverUp = true;
            isTiltLeverDown = false;
            tiltLeverCurrentState = "Forward";
            Debug.Log("ƿƮ������ ���� �����Դϴ�.");
        }
        // ƿƮ ���°� ����Ǿ��� ���� ���� ����
        if (tiltLeverCurrentState != tiltLeverLastState)
        {
            TriggerHapticFeedback(); // ���� ����
            Debug.Log($"���� ���� ����: {tiltLeverCurrentState}");
        }
        tiltLeverLastState = tiltLeverCurrentState; // ���� ���� ����
    }

    private void SideBreakLeverState()
    {
        // ���̵�극��ũ ������ ���� ��ġ�� �������� ���� ����
        float sideBreakLeverCurrentPosition = sideBreakLeverPosition.localPosition.z;
        string sideBreakLeverCurrentState = "";
        if (sideBreakLeverCurrentPosition >= -3.00f && sideBreakLeverCurrentPosition <= -2.51f)   // ���̵�극��ũ �ɱ�
        {
            isSideBreakLeverUp = false;
            isSideBreakLeverDown = true;
            sideBreakLeverCurrentState = "Engaged";
            ApplyBrake(true, true); // ���̵�극��ũ ����
            Debug.Log("���̵�극��ũ�� �ɷȽ��ϴ�.");
        }
        else if (sideBreakLeverCurrentPosition >= -2.50f && sideBreakLeverCurrentPosition <= -2.20f)  // ���̵�극��ũ Ǯ��
        {
            isSideBreakLeverUp = true;
            isSideBreakLeverDown = false;
            sideBreakLeverCurrentState = "Released";
            ApplyBrake(isBrakeApplied, false); // ���̵�극��ũ ����
            Debug.Log("���̵�극��ũ�� Ǯ�Ƚ��ϴ�.");
        }

        // ���̵�극��ũ ���°� ����Ǿ��� ���� ���� ����
        if (sideBreakLeverCurrentState != sideBreakLeverLastState)
        {
            TriggerHapticFeedback(); // ���� ����
            Debug.Log($"���̵�극��ũ ���� ����: {sideBreakLeverCurrentState}");
            sideBreakLeverLastState = sideBreakLeverCurrentState; // ���� ���� ����
        }
    }

    private void TriggerHapticFeedback()
    {
        // ���� �׼� ���� (��Ʈ�ѷ� ����)
        hapticAction.Execute(0, 0.1f, 75, 0.5f, SteamVR_Input_Sources.RightHand); // ������ ��Ʈ�ѷ�
        hapticAction.Execute(0, 0.1f, 75, 0.5f, SteamVR_Input_Sources.LeftHand);  // �޼� ��Ʈ�ѷ�
    }


    private void HandleTrackpadInput(float direction)
    {
        Debug.Log("������ Ʈ���е� ����: " + RightTrackBtn.GetState(SteamVR_Input_Sources.RightHand));  // Ʈ���е� ���� Ȯ��

        // ������ Ʈ���е尡 �����ִ� ���ȸ� �̵�
        if (RightTrackBtn.GetState(SteamVR_Input_Sources.RightHand))
        {
            verticalInput = direction;
            ApplyBrake(false, isSideBreakLeverDown);  // ����/���� �� �Ϲ� �극��ũ ����, ���̵�극��ũ ���� ����
            Debug.Log("������ Ʈ���е� ����: ����/����");
        }
        // ���� Ʈ���е尡 �����ִ� ���ȸ� �극��ũ ����
        else if (LeftTrackBtn.GetState(SteamVR_Input_Sources.LeftHand))
        {
            ApplyBrake(true, isSideBreakLeverDown);  // �Ϲ� �극��ũ ����, ���̵�극��ũ ���� ����
            Debug.Log("���� Ʈ���е� ����");
        }
        else
        {
            verticalInput = 0;
            Debug.Log("Ʈ���е� �̻��");
        }
    }

    private void ApplyBrake(bool apply, bool sideBrake)
    {
        isBrakeApplied = apply;

        if (apply || sideBrake)
        {
            // �극��ũ ����
            frontLeftWheelColider.brakeTorque = breakForce;
            frontRightWheelColider.brakeTorque = breakForce;
            rearLeftWheelColider.brakeTorque = breakForce;
            rearRightWheelColider.brakeTorque = breakForce;

            verticalInput = 0;  // �극��ũ�� ����Ǹ� ���� ����
            Debug.Log("�극��ũ �����, verticalInput : " + verticalInput + "\n  breakForce : " + breakForce);
        }
        else
        {
            // �극��ũ ����
            frontLeftWheelColider.brakeTorque = 0;
            frontRightWheelColider.brakeTorque = 0;
            rearLeftWheelColider.brakeTorque = 0;
            rearRightWheelColider.brakeTorque = 0;
            Debug.Log("�극��ũ ������");
        }
    }

    private void UpdateWheelTransforms()
    {
        // �� ������ ��ġ�� ȸ�� ������Ʈ
        UpdateWheelPositionAndRotation(frontLeftWheelColider, frontLeftWheelTransform);
        UpdateWheelPositionAndRotation(frontRightWheelColider, frontRightWheelTransform);
        UpdateWheelPositionAndRotation(rearLeftWheelColider, rearLeftWheelTransform);
        UpdateWheelPositionAndRotation(rearRightWheelColider, rearRightWheelTransform);
    }

    private void UpdateWheelPositionAndRotation(WheelCollider wheelColider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;

        // WheelCollider���� ���� ��ǥ���� ��ġ�� ȸ���� ��������
        wheelColider.GetWorldPose(out position, out rotation);

        // Transform�� ����
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }

    private void MoveLift()
    {
        // ����Ʈ�� ���� ��ġ�� �������� �̵� ó��
        float y = lift.localPosition.y;
        if (isLiftLeverUp)
        {
            y += speedLift * Time.deltaTime;
            y = Mathf.Clamp(y, maxDownLift, maxUpLift);

            lift.localPosition = new Vector3(lift.localPosition.x, y, lift.localPosition.z);
        }
        else if (isLiftLeverDown)
        {
            y -= speedLift * Time.deltaTime;
            y = Mathf.Clamp(y, maxDownLift, maxUpLift);

            lift.localPosition = new Vector3(lift.localPosition.x, y, lift.localPosition.z);
        }
    }

    private void useTilt()
    {
        float x = lift.localEulerAngles.x;
        if (x > 180) x -= 360;
        if (isTiltLeverDown)
        {
            x += speedTilt * Time.deltaTime;
            x = Mathf.Clamp(x, tiltMinAngle, tiltMaxAngle);
            lift.localEulerAngles = new Vector3(x, lift.localEulerAngles.y, lift.localEulerAngles.z);
        }
        else if (isTiltLeverUp)
        {
            x -= speedTilt * Time.deltaTime;
            x = Mathf.Clamp(x, tiltMinAngle, tiltMaxAngle);
            lift.localEulerAngles = new Vector3(x, lift.localEulerAngles.y, lift.localEulerAngles.z);
        }
    }
}