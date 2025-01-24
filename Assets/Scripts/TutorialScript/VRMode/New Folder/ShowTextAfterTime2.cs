using UnityEngine;
using UnityEngine.UI; // Text ������Ʈ�� ����� ��
using TMPro;
using System.Collections; // TextMeshPro�� ����� ��

public class ShowTextAfterTime3 : MonoBehaviour
{
    public TextMeshProUGUI uiText; // TextMeshPro�� ����ϴ� ���
    // public Text uiText; // UI Text�� ����ϴ� ���

    public float delay = 15f; // �� ������ ���� �ð� (��)

    string text = "������ ���� ������ �߿�\r\n�� �ڿ� �ִ� ������ ���̵�극��ũ �����Դϴ�\r\n����� ������ ����������� ���̵�극��ũ ON,\r\n������ �и� ���̵�극��ũ OFF�Դϴ�\r\n";

    void Start()
    {
        StartCoroutine(ChangeTextSequence());
    }

    // Coroutine�� �̿��� �ؽ�Ʈ ���� ����
    private IEnumerator ChangeTextSequence()
    {
        yield return new WaitForSeconds(delay);
        uiText.text = text;
    }
}
