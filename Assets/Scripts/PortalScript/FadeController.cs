using UnityEngine;
using Valve.VR; // SteamVR 네임스페이스

public class FadeController : MonoBehaviour
{
    public Color fadeColor = Color.black; // 페이드 색상
    public float fadeDuration = 1f;    // 페이드 소요 시간

    // 화면이 어두워졌다가 밝아지는 효과
    public void FadeIn()
    {
        SteamVR_Fade.Start(fadeColor, 0);           // 즉시 화면을 어둡게
        SteamVR_Fade.Start(Color.clear, fadeDuration); // 서서히 밝게
    }

    // 화면이 밝았다가 어두워지는 효과
    public void FadeOut()
    {
        SteamVR_Fade.Start(Color.clear, 0);        // 즉시 화면 밝게
        SteamVR_Fade.Start(fadeColor, fadeDuration); // 서서히 어둡게
    }
}
