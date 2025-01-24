using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMenu : MonoBehaviour
{
    public FadeController fadeController;

    public void LoadScene()
    {
        Time.timeScale = 1f;
        fadeController.FadeOut();
        // 페이드 아웃 후 씬 전환 실행
        Invoke(nameof(ChangeScene), fadeController.fadeDuration);
        // 페이드 아웃 후 오브젝트 삭제
        Invoke(nameof(DestroyForkLift), fadeController.fadeDuration);
    }

    void ChangeScene()
    {
        // 씬 전환 로직
        SceneManager.LoadScene(0);
    }

    private void DestroyForkLift()
    {
        // 오브젝트 삭제
        Destroy(GameObject.FindWithTag("ForkLift"));
    }
}
