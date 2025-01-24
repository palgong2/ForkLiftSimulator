using UnityEngine;
using UnityEngine.UI;

public class VrSelectScript : MonoBehaviour
{
    public Button vrButton; // VR ��ư
    public Button keyboardButton; // Ű���� ��ư

    private Color selectedColor = Color.green; // ���õ� ��ư�� ����
    private Color unselectedColor = Color.gray; // ���õ��� ���� ��ư�� ����

    private void Start()
    {
        if (vrButton != null)
        {
            vrButton.onClick.AddListener(OnVrButtonClick);
        }

        // �ʱ� ���� ����
        UpdateButtonColors();
    }

    public void OnVrButtonClick()
    {
        SelectionManager.vrSelect = true;
        SelectionManager.keyboardSelect = false;
        SelectionManager.SaveMode(); // ���� ����
        Debug.Log("VR Select: " + SelectionManager.vrSelect);
        Debug.Log("Keyboard Select: " + SelectionManager.keyboardSelect);

        // ��ư ���� ������Ʈ
        UpdateButtonColors();
    }

    private void UpdateButtonColors()
    {
        if (vrButton != null)
        {
            vrButton.GetComponent<Image>().color = SelectionManager.vrSelect ? selectedColor : unselectedColor;
        }

        if (keyboardButton != null)
        {
            keyboardButton.GetComponent<Image>().color = SelectionManager.keyboardSelect ? selectedColor : unselectedColor;
        }
    }
}
