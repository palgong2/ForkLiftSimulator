using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject VrUI;        // VR�� UI ������Ʈ
    public GameObject KeyboardUI; // Ű����� UI ������Ʈ

    public GameObject breakZone; // �극��ũ �ؽ�Ʈ

    private TrafficLightController trafficLightController;
    private TrafficLightKeyboard trafficLightKeyboard;

    private void Start()
    {
        // breakZone���� TrafficLightController�� TrafficLightKeyboard ������Ʈ�� ��������
        if (breakZone != null)
        {
            trafficLightController = breakZone.GetComponent<TrafficLightController>();
            trafficLightKeyboard = breakZone.GetComponent<TrafficLightKeyboard>();
        }

        if (trafficLightController == null || trafficLightKeyboard == null)
        {
            Debug.LogError("�극��ũ������ TrafficLightController �Ǵ� TrafficLightKeyboard�� ã�� �� �����ϴ�.");
        }

        UpdateUI(); // �ʱ� ���� ����
    }

    private void Update()
    {
        // VR �Ǵ� Ű���� ���� ���°� �ٲ���� �� UI ������Ʈ
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
        // VR ��� Ȱ��ȭ
        if (SelectionManager.vrSelect)
        {
            ToggleUI(VrUI, true);
            ToggleUI(KeyboardUI, false);
            Debug.Log("VR UI Ȱ��ȭ");
            EnableScript(trafficLightController, true);
            EnableScript(trafficLightKeyboard, false);
        }
        // Ű���� ��� Ȱ��ȭ
        else if (SelectionManager.keyboardSelect)
        {
            ToggleUI(VrUI, false);
            ToggleUI(KeyboardUI, true);
            Debug.Log("Keyboard UI Ȱ��ȭ");
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
