using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandleScript : MonoBehaviour
{
    [Header("Haptic Feedback")]
    public SteamVR_Action_Vibration hapticAction; // 진동 액션

    [Header("LeverPosition")]
    public Transform gearLeverPosition; // 기어 레버 위치
    public Transform liftLeverPosition; // 리프트 레버 위치
    public Transform tiltLeverPosition; // 틸트 레버 위치
    public Transform sideBreakLeverPosition; // 사이드브레이크 레버 위치

    [Header("Setting")]
    [SerializeField] private float motorTorque = 50; // 모터 토크
    [SerializeField] private float breakForce = 300; // 브레이크 힘
    [SerializeField] private float maxSteerAngle = 45; // 최대 조향 각도

    [Header("SteamVR Input Actions")]
    public SteamVR_Action_Boolean RightTrackBtn = SteamVR_Input.GetBooleanAction("RightTrackBtn"); // 전진 버튼
    public SteamVR_Action_Boolean LeftTrackBtn = SteamVR_Input.GetBooleanAction("LeftTrackBtn");  // 후진 버튼

    [Header("SteeringWheel")]
    [SerializeField] private Transform steeringWheelTransform; // 핸들 트랜스폼

    [Header("Colider")]
    [SerializeField] private WheelCollider frontLeftWheelColider; // 앞 왼쪽 바퀴 콜라이더
    [SerializeField] private WheelCollider frontRightWheelColider; // 앞 오른쪽 바퀴 콜라이더
    [SerializeField] private WheelCollider rearLeftWheelColider; // 뒤 왼쪽 바퀴 콜라이더
    [SerializeField] private WheelCollider rearRightWheelColider; // 뒤 오른쪽 바퀴 콜라이더

    [Header("Transform")]
    [SerializeField] private Transform frontLeftWheelTransform; // 앞 왼쪽 바퀴 트랜스폼
    [SerializeField] private Transform frontRightWheelTransform; // 앞 오른쪽 바퀴 트랜스폼
    [SerializeField] private Transform rearLeftWheelTransform; // 뒤 왼쪽 바퀴 트랜스폼
    [SerializeField] private Transform rearRightWheelTransform; // 뒤 오른쪽 바퀴 트랜스폼

    [Header("Lift")]
    [SerializeField] private Transform lift; // 리프트 트랜스폼
    [SerializeField] private float speedLift = 0.75f; // 리프트 속도
    [SerializeField] private float maxDownLift = 2.2f; // 리프트 최대 하강 높이
    [SerializeField] private float maxUpLift = 9.5f; // 리프트 최대 상승 높이

    [Header("Tilt")]
    [SerializeField] private float speedTilt = 1.5f; // 틸트 속도
    [SerializeField] private float tiltMaxAngle = 3f;
    [SerializeField] private float tiltMinAngle = -10f;

    float verticalInput; // 수직 입력 값
    string gearLeverLastState = ""; // 이전 기어레버 상태 저장
    string liftLeverLastState = ""; // 이전 리프트레버 상태 저장
    string tiltLeverLastState = ""; // 이전 틸트레버 상태 저장
    string sideBreakLeverLastState = ""; // 이전 사이드브레이크레버 상태 저장

    private bool isLiftLeverDown; // 리프트레버 하강 여부
    private bool isLiftLeverUp; // 리프트레버 상승 여부

    private bool isTiltLeverDown; // 틸트레버 하강 여부
    private bool isTiltLeverUp; // 틸트레버 상승 여부

    private bool isSideBreakLeverDown; // 사이드브레이크레버 하강 여부
    private bool isSideBreakLeverUp; // 사이드브레이크레버 상승 여부

    private bool isBrakeApplied; // 일반 브레이크 적용 여부

    private void FixedUpdate()
    {
        HandleSteering(); // 조향 처리
        UpdateWheelTransforms(); // 바퀴 트랜스폼 업데이트
        GearLeverState(); // 기어 레버 상태 처리
        LiftLeverState(); // 리프트 레버 상태 처리
        TiltLeverState(); // 틸트 레버 상태 처리
        SideBreakLeverState(); // 사이드브레이크 레버 상태 처리
        HandleTorque(); // 토크 처리
        MoveLift(); // 리프트 이동 처리
        useTilt(); // 틸트 기능 처리
    }

    private void HandleTorque()
    {
        // 각 바퀴에 모터 토크 적용
        frontLeftWheelColider.motorTorque = verticalInput * motorTorque;
        frontRightWheelColider.motorTorque = verticalInput * motorTorque;
        rearLeftWheelColider.motorTorque = verticalInput * motorTorque;
        rearRightWheelColider.motorTorque = verticalInput * motorTorque;
        Debug.Log("motorTorque : " + verticalInput * motorTorque);
        Debug.Log("breakForce" + breakForce);
    }

    private void HandleSteering()
    {
        // 핸들의 Z 회전값을 기준으로 방향 계산
        float steeringZRotation = steeringWheelTransform.localEulerAngles.z;

        // 핸들 회전값을 0 ~ 360에서 -180 ~ 180 범위로 변환
        if (steeringZRotation > 180)
            steeringZRotation -= 360;

        // -60 ~ 60 사이의 회전값을 바퀴의 조향 각도로 매핑
        float steerAngle = Mathf.Clamp((steeringZRotation / 60f) * maxSteerAngle, -maxSteerAngle, maxSteerAngle);

        // 뒷바퀴의 조향 각도 설정
        rearLeftWheelColider.steerAngle = steerAngle;
        rearRightWheelColider.steerAngle = steerAngle;
    }

    private void GearLeverState()
    {
        // 기어 레버의 현재 위치를 기준으로 상태 결정
        float gearLeverCurrentPosition = gearLeverPosition.localPosition.z;
        string gearLeverCurrentState = "";

        if (gearLeverCurrentPosition >= -2.50f && gearLeverCurrentPosition <= -2.20f)   // 후진 상태
        {
            gearLeverCurrentState = "Reverse";
            Debug.Log("변속레버가 후진 상태입니다.");
            HandleTrackpadInput(-1f); // 후진
        }
        else if (gearLeverCurrentPosition >= -3.50f && gearLeverCurrentPosition <= -2.51f)  // 중립 상태
        {
            gearLeverCurrentState = "Neutral";
            Debug.Log("변속레버가 중립 상태입니다.");
            ApplyBrake(true, isSideBreakLeverDown); // 중립 상태에서는 브레이크
        }
        else if (gearLeverCurrentPosition >= -3.80f && gearLeverCurrentPosition <= -3.51f)  // 전진 상태
        {
            gearLeverCurrentState = "Forward";
            Debug.Log("변속레버가 전진 상태입니다.");
            HandleTrackpadInput(1f); // 전진
        }

        // 기어 상태가 변경되었을 때만 진동 실행
        if (gearLeverCurrentState != gearLeverLastState)
        {
            TriggerHapticFeedback(); // 진동 실행
            Debug.Log($"레버 상태 변경: {gearLeverCurrentState}");
        }

        gearLeverLastState = gearLeverCurrentState; // 현재 상태 저장
    }

    private void LiftLeverState()
    {
        // 리프트 레버의 현재 위치를 기준으로 상태 결정
        float liftLeverCurrentPosition = liftLeverPosition.localPosition.z;
        string liftLeverCurrentState = "";

        if (liftLeverCurrentPosition >= -2.50f && liftLeverCurrentPosition <= -2.20f)   // 리프트 상승
        {
            isLiftLeverUp = false;
            isLiftLeverDown = true;
            liftLeverCurrentState = "Reverse";
            Debug.Log("리프트레버가 후진 상태입니다.");
        }
        else if (liftLeverCurrentPosition >= -3.50f && liftLeverCurrentPosition <= -2.51f)  // 중립 상태
        {
            isLiftLeverUp = false;
            isLiftLeverDown = false;
            liftLeverCurrentState = "Neutral";
            Debug.Log("리프트레버가 중립 상태입니다.");
        }
        else if (liftLeverCurrentPosition >= -3.80f && liftLeverCurrentPosition <= -3.51f)  // 리프트 하강
        {
            isLiftLeverUp = true;
            isLiftLeverDown = false;
            liftLeverCurrentState = "Forward";
            Debug.Log("리프트레버가 전진 상태입니다.");
        }

        // 리프트 상태가 변경되었을 때만 진동 실행
        if (liftLeverCurrentState != liftLeverLastState)
        {
            TriggerHapticFeedback(); // 진동 실행
            Debug.Log($"레버 상태 변경: {liftLeverCurrentState}");
        }

        liftLeverLastState = liftLeverCurrentState; // 현재 상태 저장
    }

    private void TiltLeverState()
    {
        // 틸트 레버의 현재 위치를 기준으로 상태 결정
        float tiltLeverCurrentPosition = tiltLeverPosition.localPosition.z;
        string tiltLeverCurrentState = "";
        if (tiltLeverCurrentPosition >= -2.50f && tiltLeverCurrentPosition <= -2.20f)   // Fork 안기
        {
            isTiltLeverUp = false;
            isTiltLeverDown = true;
            tiltLeverCurrentState = "Reverse";
            Debug.Log("틸트레버가 후진 상태입니다.");
        }
        else if (tiltLeverCurrentPosition >= -3.50f && tiltLeverCurrentPosition <= -2.51f)  // Fork 중립
        {
            isTiltLeverUp = false;
            isTiltLeverDown = false;
            tiltLeverCurrentState = "Neutral";
            Debug.Log("틸트레버가 중립 상태입니다.");
        }
        else if (tiltLeverCurrentPosition >= -3.80f && tiltLeverCurrentPosition <= -3.51f)  // Fork 내리기
        {
            isTiltLeverUp = true;
            isTiltLeverDown = false;
            tiltLeverCurrentState = "Forward";
            Debug.Log("틸트레버가 전진 상태입니다.");
        }
        // 틸트 상태가 변경되었을 때만 진동 실행
        if (tiltLeverCurrentState != tiltLeverLastState)
        {
            TriggerHapticFeedback(); // 진동 실행
            Debug.Log($"레버 상태 변경: {tiltLeverCurrentState}");
        }
        tiltLeverLastState = tiltLeverCurrentState; // 현재 상태 저장
    }

    private void SideBreakLeverState()
    {
        // 사이드브레이크 레버의 현재 위치를 기준으로 상태 결정
        float sideBreakLeverCurrentPosition = sideBreakLeverPosition.localPosition.z;
        string sideBreakLeverCurrentState = "";
        if (sideBreakLeverCurrentPosition >= -3.00f && sideBreakLeverCurrentPosition <= -2.51f)   // 사이드브레이크 걸기
        {
            isSideBreakLeverUp = false;
            isSideBreakLeverDown = true;
            sideBreakLeverCurrentState = "Engaged";
            ApplyBrake(true, true); // 사이드브레이크 적용
            Debug.Log("사이드브레이크가 걸렸습니다.");
        }
        else if (sideBreakLeverCurrentPosition >= -2.50f && sideBreakLeverCurrentPosition <= -2.20f)  // 사이드브레이크 풀기
        {
            isSideBreakLeverUp = true;
            isSideBreakLeverDown = false;
            sideBreakLeverCurrentState = "Released";
            ApplyBrake(isBrakeApplied, false); // 사이드브레이크 해제
            Debug.Log("사이드브레이크가 풀렸습니다.");
        }

        // 사이드브레이크 상태가 변경되었을 때만 진동 실행
        if (sideBreakLeverCurrentState != sideBreakLeverLastState)
        {
            TriggerHapticFeedback(); // 진동 실행
            Debug.Log($"사이드브레이크 상태 변경: {sideBreakLeverCurrentState}");
            sideBreakLeverLastState = sideBreakLeverCurrentState; // 현재 상태 저장
        }
    }

    private void TriggerHapticFeedback()
    {
        // 진동 액션 실행 (컨트롤러 진동)
        hapticAction.Execute(0, 0.1f, 75, 0.5f, SteamVR_Input_Sources.RightHand); // 오른손 컨트롤러
        hapticAction.Execute(0, 0.1f, 75, 0.5f, SteamVR_Input_Sources.LeftHand);  // 왼손 컨트롤러
    }


    private void HandleTrackpadInput(float direction)
    {
        Debug.Log("오른쪽 트랙패드 상태: " + RightTrackBtn.GetState(SteamVR_Input_Sources.RightHand));  // 트랙패드 상태 확인

        // 오른쪽 트랙패드가 눌려있는 동안만 이동
        if (RightTrackBtn.GetState(SteamVR_Input_Sources.RightHand))
        {
            verticalInput = direction;
            ApplyBrake(false, isSideBreakLeverDown);  // 전진/후진 시 일반 브레이크 해제, 사이드브레이크 상태 유지
            Debug.Log("오른쪽 트랙패드 눌림: 전진/후진");
        }
        // 왼쪽 트랙패드가 눌려있는 동안만 브레이크 적용
        else if (LeftTrackBtn.GetState(SteamVR_Input_Sources.LeftHand))
        {
            ApplyBrake(true, isSideBreakLeverDown);  // 일반 브레이크 적용, 사이드브레이크 상태 유지
            Debug.Log("왼쪽 트랙패드 눌림");
        }
        else
        {
            verticalInput = 0;
            Debug.Log("트랙패드 미사용");
        }
    }

    private void ApplyBrake(bool apply, bool sideBrake)
    {
        isBrakeApplied = apply;

        if (apply || sideBrake)
        {
            // 브레이크 적용
            frontLeftWheelColider.brakeTorque = breakForce;
            frontRightWheelColider.brakeTorque = breakForce;
            rearLeftWheelColider.brakeTorque = breakForce;
            rearRightWheelColider.brakeTorque = breakForce;

            verticalInput = 0;  // 브레이크가 적용되면 차량 정지
            Debug.Log("브레이크 적용됨, verticalInput : " + verticalInput + "\n  breakForce : " + breakForce);
        }
        else
        {
            // 브레이크 해제
            frontLeftWheelColider.brakeTorque = 0;
            frontRightWheelColider.brakeTorque = 0;
            rearLeftWheelColider.brakeTorque = 0;
            rearRightWheelColider.brakeTorque = 0;
            Debug.Log("브레이크 해제됨");
        }
    }

    private void UpdateWheelTransforms()
    {
        // 각 바퀴의 위치와 회전 업데이트
        UpdateWheelPositionAndRotation(frontLeftWheelColider, frontLeftWheelTransform);
        UpdateWheelPositionAndRotation(frontRightWheelColider, frontRightWheelTransform);
        UpdateWheelPositionAndRotation(rearLeftWheelColider, rearLeftWheelTransform);
        UpdateWheelPositionAndRotation(rearRightWheelColider, rearRightWheelTransform);
    }

    private void UpdateWheelPositionAndRotation(WheelCollider wheelColider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;

        // WheelCollider에서 월드 좌표계의 위치와 회전값 가져오기
        wheelColider.GetWorldPose(out position, out rotation);

        // Transform에 적용
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }

    private void MoveLift()
    {
        // 리프트의 현재 위치를 기준으로 이동 처리
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