using UnityEngine;
using UnityEngine.UI;

public class KeyboardSelectScript : MonoBehaviour
{
    public Button keyboardButton; // Ű���� ��ư
    public Button vrButton; // VR ��ư

    private Color selectedColor = Color.green; // ���õ� ��ư�� ����
    private Color unselectedColor = Color.gray; // ���õ��� ���� ��ư�� ����

    private void Start()
    {
        if (keyboardButton != null)
        {
            keyboardButton.onClick.AddListener(OnKeyboardButtonClick);
        }

        // �ʱ� ���� ����
        UpdateButtonColors();
    }

    public void OnKeyboardButtonClick()
    {
        SelectionManager.vrSelect = false;
        SelectionManager.keyboardSelect = true;
        SelectionManager.SaveMode(); // ���� ����
        Debug.Log("VR Select: " + SelectionManager.vrSelect);
        Debug.Log("Keyboard Select: " + SelectionManager.keyboardSelect);

        // ��ư ���� ������Ʈ
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
