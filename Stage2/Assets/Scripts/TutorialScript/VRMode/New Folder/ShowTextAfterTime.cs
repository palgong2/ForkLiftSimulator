using UnityEngine;
using UnityEngine.UI; // Text 컴포넌트를 사용할 때
using TMPro;
using System.Collections; // TextMeshPro를 사용할 때

public class ShowTextAfterTime : MonoBehaviour
{
    public TextMeshProUGUI uiText; // TextMeshPro를 사용하는 경우
    // public Text uiText; // UI Text를 사용하는 경우

    public float delay = 15f; // 각 상태의 지연 시간 (초)

    string text = "양 옆의 설명을 보고 전진 및 후진을 연습해 보세요!";

    void Start()
    {
        StartCoroutine(ChangeTextSequence());
    }

    // Coroutine을 이용한 텍스트 변경 순서
    private IEnumerator ChangeTextSequence()
    {
        yield return new WaitForSeconds(delay);
        uiText.text = text;
    }
}
