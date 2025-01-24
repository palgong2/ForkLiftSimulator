using System.Collections;
using UnityEngine;
using TMPro;

public class TrafficLightKeyboard : MonoBehaviour
{
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;

    public float greenLightDelay = 10f; // Player�� ���� �� �ʷϺ��� ����������� ���� �ð�

    private bool playerOnTrigger = false; // Player�� ��Ҵ��� ����

    public TextMeshProUGUI uiText; // TextMeshPro�� ����ϴ� ���
    public float delay = 5f; // �� ������ ���� �ð� (��)

    string breakTextkey = "�����̽��ٸ� ������ �극��ũ����!\r\n�Ͼ�ٴ� ������ �����ż�\r\n��ٷȴٰ� �ʷϺ��� �Ǹ� ������ּ���!";
    string startTextkey = "���!";

    private void Start()
    {
        // ó������ �������� ������ ���� ����
        redLight.SetActive(true);
        yellowLight.SetActive(false);
        greenLight.SetActive(false);

        // �ʱ� �ؽ�Ʈ ����
        uiText.text = breakTextkey;
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
            uiText.text = startTextkey; // �ʷϺҷ� �ٲ�� "���!" �ؽ�Ʈ ǥ��
        }
    }
}


