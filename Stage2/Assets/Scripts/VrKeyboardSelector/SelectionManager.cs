using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static bool vrSelect = true; // VR 선택 상태
    public static bool keyboardSelect = false;

    private const string SelectedModeKey = "SelectedMode"; // PlayerPrefs 키

    private void Awake()
    {
        // 저장된 모드 불러오기
        if (PlayerPrefs.HasKey(SelectedModeKey))
        {
            string savedMode = PlayerPrefs.GetString(SelectedModeKey);
            vrSelect = savedMode == "VR";
            keyboardSelect = savedMode == "Keyboard";
        }
        else
        {
            // 기본값: VR
            vrSelect = true;
            keyboardSelect = false;
        }
    }

    public static void SaveMode()
    {
        // 현재 선택 상태 저장
        string modeToSave = vrSelect ? "VR" : "Keyboard";
        PlayerPrefs.SetString(SelectedModeKey, modeToSave);
        PlayerPrefs.Save();
    }
}
