using UnityEngine;
using UnityEngine.UI; // Text 컴포넌트를 사용할 때
using TMPro;
using System.Collections; // TextMeshPro를 사용할 때

public class ShowTextAfterTime3 : MonoBehaviour
{
    public TextMeshProUGUI uiText; // TextMeshPro를 사용하는 경우
    // public Text uiText; // UI Text를 사용하는 경우

    public float delay = 15f; // 각 상태의 지연 시간 (초)

    string text = "오른쪽 많은 레버들 중에\r\n맨 뒤에 있는 레버는 사이드브레이크 레버입니다\r\n사용자 쪽으로 당겨져있으면 사이드브레이크 ON,\r\n앞으로 밀면 사이드브레이크 OFF입니다\r\n";

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
