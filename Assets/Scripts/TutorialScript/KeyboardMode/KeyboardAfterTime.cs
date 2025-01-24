using UnityEngine;
using UnityEngine.UI; // Text 컴포넌트를 사용할 때
using TMPro;
using System.Collections; // TextMeshPro를 사용할 때

public class KeyboardAfterTime : MonoBehaviour
{
    public TextMeshProUGUI uiText; // TextMeshPro를 사용하는 경우
    // public Text uiText; // UI Text를 사용하는 경우

    public float delay = 15f; // 각 상태의 지연 시간 (초)

    string text = "W / S 는 전진 및 후진 \n A / D 는 좌회전 및 우회전입니다!";

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
