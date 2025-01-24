using System.Collections;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    public FadeController fadeController;

    public void Exit()
    {
        // 페이드 아웃을 시작하고, 완료된 후 애플리케이션을 종료합니다.
        fadeController.FadeOut();
        Invoke(nameof(OnFadeOutComplete), fadeController.fadeDuration);
    }

    private void OnFadeOutComplete()
    {
        // 페이드 아웃이 완료된 후 애플리케이션 종료
        //#if UNITY_EDITOR
        //        UnityEditor.EditorApplication.isPlaying = false;
        //#else
                Application.Quit();
        //#endif
    }
}
