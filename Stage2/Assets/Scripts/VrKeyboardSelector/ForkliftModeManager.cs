using UnityEngine;
using Valve.VR.InteractionSystem;

public class ForkliftModeManager : MonoBehaviour
{
    public GameObject forklift; // ������ ������Ʈ
    public GameObject steeringWheel; // Steering_Wheel ������Ʈ

    private MonoBehaviour forkliftController; // Ű����� ��ũ��Ʈ
    private MonoBehaviour handleScriptVR;     // VR�� ��ũ��Ʈ
    private HandleRotateScript handleRotateScript; // Steering_Wheel�� ����� HandleRotateScript

    private void Start()
    {
        // ���������� �ʿ��� ��ũ��Ʈ �������� ��������
        if (forklift != null)
        {
            forkliftController = forklift.GetComponent<ForkliftController>();
            handleScriptVR = forklift.GetComponent<HandleScript>();

            // Steering_Wheel���� HandleRotateScript ��������
            if (steeringWheel != null)
            {
                handleRotateScript = steeringWheel.GetComponent<HandleRotateScript>();
            }
        }

        if (forkliftController == null || handleScriptVR == null)
        {
            Debug.LogError("������ ������Ʈ���� ��ũ��Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        if (handleRotateScript == null)
        {
            Debug.LogError("Steering_Wheel ������Ʈ���� HandleRotateScript�� ã�� �� �����ϴ�.");
            return;
        }

        UpdateScriptState(); // �ʱ� ���� ����
    }

    private void Update()
    {
        // ���� ���°� ����Ǿ��� ��� ��ũ��Ʈ ���� ������Ʈ
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

        // VR ��� ����
        if (SelectionManager.vrSelect)
        {
            EnableScript(handleScriptVR, true);
            EnableScript(forkliftController, false);
            EnableScript(handleRotateScript, true); // VR ��忡���� HandleRotateScript ��
            Debug.Log("VR ��� Ȱ��ȭ");
        }
        // Ű���� ��� ����
        else if (SelectionManager.keyboardSelect)
        {
            EnableScript(handleScriptVR, false);
            EnableScript(forkliftController, true);
            EnableScript(handleRotateScript, false); // Ű���� ��忡���� HandleRotateScript ��
            Debug.Log("Ű���� ��� Ȱ��ȭ");
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
