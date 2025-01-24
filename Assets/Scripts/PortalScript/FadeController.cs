using UnityEngine;
using Valve.VR; // SteamVR ���ӽ����̽�

public class FadeController : MonoBehaviour
{
    public Color fadeColor = Color.black; // ���̵� ����
    public float fadeDuration = 1f;    // ���̵� �ҿ� �ð�

    // ȭ���� ��ο����ٰ� ������� ȿ��
    public void FadeIn()
    {
        SteamVR_Fade.Start(fadeColor, 0);           // ��� ȭ���� ��Ӱ�
        SteamVR_Fade.Start(Color.clear, fadeDuration); // ������ ���
    }

    // ȭ���� ��Ҵٰ� ��ο����� ȿ��
    public void FadeOut()
    {
        SteamVR_Fade.Start(Color.clear, 0);        // ��� ȭ�� ���
        SteamVR_Fade.Start(fadeColor, fadeDuration); // ������ ��Ӱ�
    }
}
