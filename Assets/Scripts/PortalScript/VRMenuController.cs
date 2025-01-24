using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.Extras;

public class VRMenuController : MonoBehaviour
{
    public GameObject MenuPopup;  // �޴� �˾�
    public SteamVR_Action_Boolean menu = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Menu"); // �޴� ��ư
    public SteamVR_LaserPointer[] lp;  // ������ ������ �迭 (�޼հ� ������ ������ ������)
    public GameObject leftHand; // �޼� ������Ʈ
    public GameObject rightHand; // ������ ������Ʈ

    public GameObject light; // ���� ������Ʈ

    public Button cancelButton;  // ��� ��ư

    private GameObject leftLaserObject; // �޼� ������ ������ ������Ʈ
    private GameObject rightLaserObject; // ������ ������ ������ ������Ʈ

    private bool isMenuActive = false;  // �޴��� Ȱ��ȭ�Ǿ� �ִ��� ����
    private Color originalAmbientLight;  // ������ ambient light ����

    void Start()
    {
        // ������ �������� �θ� �Ǵ� ������Ʈ�� �ʱ�ȭ
        leftLaserObject = leftHand.transform.Find("New Game Object")?.gameObject;
        rightLaserObject = rightHand.transform.Find("New Game Object")?.gameObject;

        // ������: �� ������ ������Ʈ�� ������ ã�������� Ȯ��
        if (leftLaserObject != null)
            Debug.Log("Left Laser Object found!");
        else
            Debug.Log("Left Laser Object NOT found!");

        if (rightLaserObject != null)
            Debug.Log("Right Laser Object found!");
        else
            Debug.Log("Right Laser Object NOT found!");

        // �޴��� �ʱ� ���¿��� ��Ȱ��ȭ
        MenuPopup.SetActive(false);

        // ������ ��Ȱ��ȭ
        if (leftLaserObject != null) leftLaserObject.SetActive(false);
        if (rightLaserObject != null) rightLaserObject.SetActive(false);

        // ���� ambient light ���� ����
        originalAmbientLight = RenderSettings.ambientLight;

        cancelButton.onClick.AddListener(CancelMenu);
    }

    void Update()
    {
        // �� �����Ӹ��� ������ ������ ������Ʈ�� üũ
        if (rightLaserObject == null)
        {
            rightLaserObject = rightHand.transform.Find("New Game Object")?.gameObject;
            if (rightLaserObject != null)
                Debug.Log("Right Laser Object found at runtime!");
                rightLaserObject.SetActive(false);  // �������� ã���� ���� ��Ȱ��ȭ ���·� ����
        }

        // �� �����Ӹ��� �޼� ������ ������Ʈ�� üũ
        if (leftLaserObject == null)
        {
            leftLaserObject = leftHand.transform.Find("New Game Object")?.gameObject;
            if (leftLaserObject != null)
                Debug.Log("Left Laser Object found at runtime!");
            leftLaserObject.SetActive(false);  // �������� ã���� ���� ��Ȱ��ȭ ���·� ����
        }

        // �޴� ��ư�� ������ ����
        if (menu.GetStateDown(SteamVR_Input_Sources.Any))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isMenuActive = !isMenuActive;  // �޴� ���� ����

        // �޴� �˾��� Ȱ��ȭ ���θ� ���
        MenuPopup.SetActive(isMenuActive);

        // ������ ������ Ȱ��ȭ/��Ȱ��ȭ
        if (leftLaserObject != null) leftLaserObject.SetActive(isMenuActive);
        if (rightLaserObject != null) rightLaserObject.SetActive(isMenuActive);

        // �޴� Ȱ��ȭ �� ���� �Ͻ�����, ��Ȱ��ȭ �� ���� �簳
        if (isMenuActive)
        {
            light.SetActive(false); // ���� ��Ȱ��ȭ
            Time.timeScale = 0f; // ���� �Ͻ�����
            RenderSettings.ambientLight = new Color(0.2f, 0.2f, 0.2f); // ��ο� �������� ����
        }
        else
        {
            light.SetActive(true); // ���� Ȱ��ȭ
            Time.timeScale = 1f; // ���� �簳
            RenderSettings.ambientLight = originalAmbientLight; // ������ ���� ����
        }
    }

    void CancelMenu()
    {
        // �޴��� Ȱ��ȭ�Ǿ� ���� ���� ��� ����� �����ϵ���
        if (isMenuActive)
        {

            light.SetActive(true); // ���� Ȱ��ȭ
            isMenuActive = false;

            // �޴� �˾��� �ݰ�, ������ �����͸� ��Ȱ��ȭ
            MenuPopup.SetActive(false);
            if (leftLaserObject != null) leftLaserObject.SetActive(false);
            if (rightLaserObject != null) rightLaserObject.SetActive(false);

            // �ð��� �ٽ� �帣��
            Time.timeScale = 1f;
        }
    }
}
