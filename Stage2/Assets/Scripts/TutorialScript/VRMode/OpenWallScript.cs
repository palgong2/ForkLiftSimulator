using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenWallScript : MonoBehaviour
{
    public GameObject wall; // 삭제할 문 오브젝트
    public TextMeshProUGUI uiText1;
    public TextMeshProUGUI uiText2; // 변경할 텍스트

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pallet"))
        {
            Destroy(wall); // 문 오브젝트 삭제
            uiText1.text = "문이열렸어요!"; // 텍스트를 변경
            uiText2.text = ""; // 텍스트를 빈 문자열로 변경
        }
    }
}



