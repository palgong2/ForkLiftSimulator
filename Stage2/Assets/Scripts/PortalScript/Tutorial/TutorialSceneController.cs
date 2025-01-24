using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TutorialSceneController : MonoBehaviour
{
    public Color fadeColor = Color.black; // 페이드 색상
    public float fadeDuration = 1f;    // 페이드 소요 시간

    private void Start()
    {
        // 씬 시작 시 페이드 인 (화면 밝게)
        SteamVR_Fade.Start(fadeColor, 0);           // 즉시 어두운 상태로 시작
        SteamVR_Fade.Start(Color.clear, fadeDuration); // 서서히 밝게
    }
}
