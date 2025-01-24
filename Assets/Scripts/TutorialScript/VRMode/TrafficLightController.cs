using System.Collections;
using UnityEngine;
using TMPro;

public class TrafficLightController : MonoBehaviour
{
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;

    public float greenLightDelay = 10f; // Player�� ���� �� �ʷϺ��� ����������� ���� �ð�

    private bool playerOnTrigger = false; // Player�� ��Ҵ��� ����

    public TextMeshProUGUI uiText; // TextMeshPro�� ����ϴ� ���
    public float delay = 5f; // �� ������ ���� �ð� (��)

    string breakText = "���� Ʈ���е带 ������ �극��ũ����!\r\n�Ͼ�ٴ� ������ �����ż�\r\n��ٷȴٰ� �ʷϺ��� �Ǹ� ������ּ���!";
    string startText = "���!";

    private void Start()
    {
        // ó������ �������� ������ ���� ����
        redLight.SetActive(true);
        yellowLight.SetActive(false);
        greenLight.SetActive(false);

        // �ʱ� �ؽ�Ʈ ����
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
            uiText.text = startText; // �ʷϺҷ� �ٲ�� "���!" �ؽ�Ʈ ǥ��
        }
    }
}


