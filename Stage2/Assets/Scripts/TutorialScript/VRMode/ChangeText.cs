using UnityEngine;
using UnityEngine.UI; // Text 컴포넌트를 사용할 때
using TMPro;
using System.Collections; // TextMeshPro를 사용할 때

public class ChangeText : MonoBehaviour
{
    public TextMeshProUGUI uiText; // TextMeshPro를 사용하는 경우
    // public Text uiText; // UI Text를 사용하는 경우

    public float delay = 5f; // 각 상태의 지연 시간 (초)

    string emptyText = "";
    string welcomeText = "안녕하세요! 지게차 튜토리얼에 오신것을 환영합니다";
    string multiLineText = "맵 곳곳에 있는 텍스트를 읽어가며 \n 튜토리얼을 진행해 주시면 됩니다. ";

    public string[] texts;

    void Start()
    {
        // 텍스트 배열 초기화
        texts = new string[] { emptyText, welcomeText, multiLineText, emptyText };

        // Coroutine을 통해 텍스트 변경 순서 실행
        StartCoroutine(ChangeTextSequence());
    }

    // Coroutine을 이용한 텍스트 변경 순서
    private IEnumerator ChangeTextSequence()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            uiText.text = texts[i];
            yield return new WaitForSeconds(delay);
        }
    }
}
