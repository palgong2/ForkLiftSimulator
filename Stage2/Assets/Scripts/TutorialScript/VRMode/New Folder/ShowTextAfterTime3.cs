using UnityEngine;
using UnityEngine.UI; // Text 컴포넌트를 사용할 때
using TMPro;
using System.Collections; // TextMeshPro를 사용할 때

public class ShowTextAfterTime2 : MonoBehaviour
{
    public TextMeshProUGUI uiText; // TextMeshPro를 사용하는 경우
    // public Text uiText; // UI Text를 사용하는 경우

    public float delay = 15f; // 각 상태의 지연 시간 (초)

    string text = "핸들 옆에 있는 레버는 변속레버입니다.\r\n변속레버 기준으로 위는 전진 아래는 후진 가운데는 중립입니다";

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
