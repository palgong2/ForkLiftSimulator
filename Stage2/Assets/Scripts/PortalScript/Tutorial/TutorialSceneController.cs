using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TutorialSceneController : MonoBehaviour
{
    public Color fadeColor = Color.black; // ���̵� ����
    public float fadeDuration = 1f;    // ���̵� �ҿ� �ð�

    private void Start()
    {
        // �� ���� �� ���̵� �� (ȭ�� ���)
        SteamVR_Fade.Start(fadeColor, 0);           // ��� ��ο� ���·� ����
        SteamVR_Fade.Start(Color.clear, fadeDuration); // ������ ���
    }
}
