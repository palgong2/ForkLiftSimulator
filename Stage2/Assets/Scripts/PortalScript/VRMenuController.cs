using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.Extras;

public class VRMenuController : MonoBehaviour
{
    public GameObject MenuPopup;  // 메뉴 팝업
    public SteamVR_Action_Boolean menu = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Menu"); // 메뉴 버튼
    public SteamVR_LaserPointer[] lp;  // 레이저 포인터 배열 (왼손과 오른손 레이저 포인터)
    public GameObject leftHand; // 왼손 오브젝트
    public GameObject rightHand; // 오른손 오브젝트

    public GameObject light; // 조명 오브젝트

    public Button cancelButton;  // 취소 버튼

    private GameObject leftLaserObject; // 왼손 레이저 포인터 오브젝트
    private GameObject rightLaserObject; // 오른손 레이저 포인터 오브젝트

    private bool isMenuActive = false;  // 메뉴가 활성화되어 있는지 여부
    private Color originalAmbientLight;  // 원래의 ambient light 색상

    void Start()
    {
        // 레이저 포인터의 부모가 되는 오브젝트들 초기화
        leftLaserObject = leftHand.transform.Find("New Game Object")?.gameObject;
        rightLaserObject = rightHand.transform.Find("New Game Object")?.gameObject;

        // 디버깅용: 각 레이저 오브젝트가 실제로 찾아졌는지 확인
        if (leftLaserObject != null)
            Debug.Log("Left Laser Object found!");
        else
            Debug.Log("Left Laser Object NOT found!");

        if (rightLaserObject != null)
            Debug.Log("Right Laser Object found!");
        else
            Debug.Log("Right Laser Object NOT found!");

        // 메뉴는 초기 상태에서 비활성화
        MenuPopup.SetActive(false);

        // 레이저 비활성화
        if (leftLaserObject != null) leftLaserObject.SetActive(false);
        if (rightLaserObject != null) rightLaserObject.SetActive(false);

        // 원래 ambient light 색상 저장
        originalAmbientLight = RenderSettings.ambientLight;

        cancelButton.onClick.AddListener(CancelMenu);
    }

    void Update()
    {
        // 매 프레임마다 오른손 레이저 오브젝트를 체크
        if (rightLaserObject == null)
        {
            rightLaserObject = rightHand.transform.Find("New Game Object")?.gameObject;
            if (rightLaserObject != null)
                Debug.Log("Right Laser Object found at runtime!");
                rightLaserObject.SetActive(false);  // 동적으로 찾았을 때도 비활성화 상태로 시작
        }

        // 매 프레임마다 왼손 레이저 오브젝트를 체크
        if (leftLaserObject == null)
        {
            leftLaserObject = leftHand.transform.Find("New Game Object")?.gameObject;
            if (leftLaserObject != null)
                Debug.Log("Left Laser Object found at runtime!");
            leftLaserObject.SetActive(false);  // 동적으로 찾았을 때도 비활성화 상태로 시작
        }

        // 메뉴 버튼이 눌렸을 때만
        if (menu.GetStateDown(SteamVR_Input_Sources.Any))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isMenuActive = !isMenuActive;  // 메뉴 상태 반전

        // 메뉴 팝업의 활성화 여부를 토글
        MenuPopup.SetActive(isMenuActive);

        // 레이저 포인터 활성화/비활성화
        if (leftLaserObject != null) leftLaserObject.SetActive(isMenuActive);
        if (rightLaserObject != null) rightLaserObject.SetActive(isMenuActive);

        // 메뉴 활성화 시 게임 일시정지, 비활성화 시 게임 재개
        if (isMenuActive)
        {
            light.SetActive(false); // 조명 비활성화
            Time.timeScale = 0f; // 게임 일시정지
            RenderSettings.ambientLight = new Color(0.2f, 0.2f, 0.2f); // 어두운 색상으로 변경
        }
        else
        {
            light.SetActive(true); // 조명 활성화
            Time.timeScale = 1f; // 게임 재개
            RenderSettings.ambientLight = originalAmbientLight; // 원래의 밝기로 복원
        }
    }

    void CancelMenu()
    {
        // 메뉴가 활성화되어 있을 때만 취소 기능이 동작하도록
        if (isMenuActive)
        {

            light.SetActive(true); // 조명 활성화
            isMenuActive = false;

            // 메뉴 팝업을 닫고, 레이저 포인터를 비활성화
            MenuPopup.SetActive(false);
            if (leftLaserObject != null) leftLaserObject.SetActive(false);
            if (rightLaserObject != null) rightLaserObject.SetActive(false);

            // 시간을 다시 흐르게
            Time.timeScale = 1f;
        }
    }
}
