using UnityEngine;
using Valve.VR.InteractionSystem;

public class ForkliftModeManager : MonoBehaviour
{
    public GameObject forklift; // 지게차 오브젝트
    public GameObject steeringWheel; // Steering_Wheel 오브젝트

    private MonoBehaviour forkliftController; // 키보드용 스크립트
    private MonoBehaviour handleScriptVR;     // VR용 스크립트
    private HandleRotateScript handleRotateScript; // Steering_Wheel에 연결된 HandleRotateScript

    private void Start()
    {
        // 지게차에서 필요한 스크립트 동적으로 가져오기
        if (forklift != null)
        {
            forkliftController = forklift.GetComponent<ForkliftController>();
            handleScriptVR = forklift.GetComponent<HandleScript>();

            // Steering_Wheel에서 HandleRotateScript 가져오기
            if (steeringWheel != null)
            {
                handleRotateScript = steeringWheel.GetComponent<HandleRotateScript>();
            }
        }

        if (forkliftController == null || handleScriptVR == null)
        {
            Debug.LogError("지게차 오브젝트에서 스크립트를 찾을 수 없습니다.");
            return;
        }

        if (handleRotateScript == null)
        {
            Debug.LogError("Steering_Wheel 오브젝트에서 HandleRotateScript를 찾을 수 없습니다.");
            return;
        }

        UpdateScriptState(); // 초기 상태 설정
    }

    private void Update()
    {
        // 선택 상태가 변경되었을 경우 스크립트 상태 업데이트
        if (SelectionManager.vrSelect && !handleScriptVR.enabled)
        {
            UpdateScriptState();
        }
        else if (SelectionManager.keyboardSelect && !forkliftController.enabled)
        {
            UpdateScriptState();
        }
    }

    private void UpdateScriptState()
    {
        if (forkliftController == null || handleScriptVR == null || handleRotateScript == null)
            return;

        // VR 모드 선택
        if (SelectionManager.vrSelect)
        {
            EnableScript(handleScriptVR, true);
            EnableScript(forkliftController, false);
            EnableScript(handleRotateScript, true); // VR 모드에서는 HandleRotateScript 켬
            Debug.Log("VR 모드 활성화");
        }
        // 키보드 모드 선택
        else if (SelectionManager.keyboardSelect)
        {
            EnableScript(handleScriptVR, false);
            EnableScript(forkliftController, true);
            EnableScript(handleRotateScript, false); // 키보드 모드에서는 HandleRotateScript 끔
            Debug.Log("키보드 모드 활성화");
        }
    }

    private void EnableScript(MonoBehaviour script, bool enable)
    {
        if (script != null)
        {
            script.enabled = enable;
        }
    }
}
