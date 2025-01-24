using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static bool vrSelect = true; // VR ���� ����
    public static bool keyboardSelect = false;

    private const string SelectedModeKey = "SelectedMode"; // PlayerPrefs Ű

    private void Awake()
    {
        // ����� ��� �ҷ�����
        if (PlayerPrefs.HasKey(SelectedModeKey))
        {
            string savedMode = PlayerPrefs.GetString(SelectedModeKey);
            vrSelect = savedMode == "VR";
            keyboardSelect = savedMode == "Keyboard";
        }
        else
        {
            // �⺻��: VR
            vrSelect = true;
            keyboardSelect = false;
        }
    }

    public static void SaveMode()
    {
        // ���� ���� ���� ����
        string modeToSave = vrSelect ? "VR" : "Keyboard";
        PlayerPrefs.SetString(SelectedModeKey, modeToSave);
        PlayerPrefs.Save();
    }
}
