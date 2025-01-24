using UnityEngine;
using UnityEngine.UI; // Text ������Ʈ�� ����� ��
using TMPro;
using System.Collections; // TextMeshPro�� ����� ��

public class ChangeText : MonoBehaviour
{
    public TextMeshProUGUI uiText; // TextMeshPro�� ����ϴ� ���
    // public Text uiText; // UI Text�� ����ϴ� ���

    public float delay = 5f; // �� ������ ���� �ð� (��)

    string emptyText = "";
    string welcomeText = "�ȳ��ϼ���! ������ Ʃ�丮�� ���Ű��� ȯ���մϴ�";
    string multiLineText = "�� ������ �ִ� �ؽ�Ʈ�� �о�� \n Ʃ�丮���� ������ �ֽø� �˴ϴ�. ";

    public string[] texts;

    void Start()
    {
        // �ؽ�Ʈ �迭 �ʱ�ȭ
        texts = new string[] { emptyText, welcomeText, multiLineText, emptyText };

        // Coroutine�� ���� �ؽ�Ʈ ���� ���� ����
        StartCoroutine(ChangeTextSequence());
    }

    // Coroutine�� �̿��� �ؽ�Ʈ ���� ����
    private IEnumerator ChangeTextSequence()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            uiText.text = texts[i];
            yield return new WaitForSeconds(delay);
        }
    }
}
