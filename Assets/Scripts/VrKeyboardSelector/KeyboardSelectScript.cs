using UnityEngine;
using UnityEngine.UI;

public class KeyboardSelectScript : MonoBehaviour
{
    public Button keyboardButton; // 키보드 버튼
    public Button vrButton; // VR 버튼

    private Color selectedColor = Color.green; // 선택된 버튼의 색상
    private Color unselectedColor = Color.gray; // 선택되지 않은 버튼의 색상

    private void Start()
    {
        if (keyboardButton != null)
        {
            keyboardButton.onClick.AddListener(OnKeyboardButtonClick);
        }

        // 초기 상태 설정
        UpdateButtonColors();
    }

    public void OnKeyboardButtonClick()
    {
        SelectionManager.vrSelect = false;
        SelectionManager.keyboardSelect = true;
        SelectionManager.SaveMode(); // 상태 저장
        Debug.Log("VR Select: " + SelectionManager.vrSelect);
        Debug.Log("Keyboard Select: " + SelectionManager.keyboardSelect);

        // 버튼 색상 업데이트
        UpdateButtonColors();
    }

    private void UpdateButtonColors()
    {
        if (keyboardButton != null)
        {
            keyboardButton.GetComponent<Image>().color = SelectionManager.keyboardSelect ? selectedColor : unselectedColor;
        }

        if (vrButton != null)
        {
            vrButton.GetComponent<Image>().color = SelectionManager.vrSelect ? selectedColor : unselectedColor;
        }
    }
}
