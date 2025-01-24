using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenWallScript : MonoBehaviour
{
    public GameObject wall; // ������ �� ������Ʈ
    public TextMeshProUGUI uiText1;
    public TextMeshProUGUI uiText2; // ������ �ؽ�Ʈ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pallet"))
        {
            Destroy(wall); // �� ������Ʈ ����
            uiText1.text = "���̿��Ⱦ��!"; // �ؽ�Ʈ�� ����
            uiText2.text = ""; // �ؽ�Ʈ�� �� ���ڿ��� ����
        }
    }
}



