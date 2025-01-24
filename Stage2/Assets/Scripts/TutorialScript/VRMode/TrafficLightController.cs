using System.Collections;
using UnityEngine;
using TMPro;

public class TrafficLightController : MonoBehaviour
{
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;

    public float greenLightDelay = 10f; // Player가 닿은 후 초록불이 켜지기까지의 지연 시간

    private bool playerOnTrigger = false; // Player가 닿았는지 여부

    public TextMeshProUGUI uiText; // TextMeshPro를 사용하는 경우
    public float delay = 5f; // 각 상태의 지연 시간 (초)

    string breakText = "왼쪽 트랙패드를 누르면 브레이크에요!\r\n하얀바닥 안으로 들어오셔서\r\n기다렸다가 초록불이 되면 출발해주세요!";
    string startText = "출발!";

    private void Start()
    {
        // 처음에는 빨간불이 무한정 켜져 있음
        redLight.SetActive(true);
        yellowLight.SetActive(false);
        greenLight.SetActive(false);

        // 초기 텍스트 설정
        uiText.text = breakText;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ForkLift"))
        {
            playerOnTrigger = true;
            StartCoroutine(GreenLightDelay());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ForkLift"))
        {
            playerOnTrigger = false;
        }
    }

    private IEnumerator GreenLightDelay()
    {
        yield return new WaitForSeconds(greenLightDelay);
        if (playerOnTrigger)
        {
            greenLight.SetActive(true);
            redLight.SetActive(false);
            yellowLight.SetActive(false);
            uiText.text = startText; // 초록불로 바뀌면 "출발!" 텍스트 표시
        }
    }
}


