using UnityEngine;
using UnityEngine.UI; // Text ������Ʈ�� ����� ��
using TMPro;
using System.Collections; // TextMeshPro�� ����� ��

public class KeyboardAfterTime : MonoBehaviour
{
    public TextMeshProUGUI uiText; // TextMeshPro�� ����ϴ� ���
    // public Text uiText; // UI Text�� ����ϴ� ���

    public float delay = 15f; // �� ������ ���� �ð� (��)

    string text = "W / S �� ���� �� ���� \n A / D �� ��ȸ�� �� ��ȸ���Դϴ�!";

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
