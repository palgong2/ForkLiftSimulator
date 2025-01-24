using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject VrUI;        // VR용 UI 오브젝트
    public GameObject KeyboardUI; // 키보드용 UI 오브젝트

    public GameObject breakZone; // 브레이크 텍스트

    private TrafficLightController trafficLightController;
    private TrafficLightKeyboard trafficLightKeyboard;

    private void Start()
    {
        // breakZone에서 TrafficLightController와 TrafficLightKeyboard 컴포넌트를 가져오기
        if (breakZone != null)
        {
            trafficLightController = breakZone.GetComponent<TrafficLightController>();
            trafficLightKeyboard = breakZone.GetComponent<TrafficLightKeyboard>();
        }

        if (trafficLightController == null || trafficLightKeyboard == null)
        {
            Debug.LogError("브레이크존에서 TrafficLightController 또는 TrafficLightKeyboard를 찾을 수 없습니다.");
        }

        UpdateUI(); // 초기 상태 설정
    }

    private void Update()
    {
        // VR 또는 키보드 선택 상태가 바뀌었을 때 UI 업데이트
        if (SelectionManager.vrSelect && !VrUI.activeSelf)
        {
            UpdateUI();
        }
        else if (SelectionManager.keyboardSelect && !KeyboardUI.activeSelf)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        // VR 모드 활성화
        if (SelectionManager.vrSelect)
        {
            ToggleUI(VrUI, true);
            ToggleUI(KeyboardUI, false);
            Debug.Log("VR UI 활성화");
            EnableScript(trafficLightController, true);
            EnableScript(trafficLightKeyboard, false);
        }
        // 키보드 모드 활성화
        else if (SelectionManager.keyboardSelect)
        {
            ToggleUI(VrUI, false);
            ToggleUI(KeyboardUI, true);
            Debug.Log("Keyboard UI 활성화");
            EnableScript(trafficLightController, false);
            EnableScript(trafficLightKeyboard, true);
        }
    }

    private void ToggleUI(GameObject uiObject, bool enable)
    {
        if (uiObject != null)
        {
            uiObject.SetActive(enable);
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
