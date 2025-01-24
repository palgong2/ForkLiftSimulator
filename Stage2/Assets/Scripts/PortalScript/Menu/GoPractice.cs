using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.Extras;

public class GoPractice : MonoBehaviour
{
    public FadeController fadeController;
    public void LoadScene()
    {
        fadeController.FadeOut();
        // 페이드 아웃 후 씬 전환 실행
        Invoke(nameof(ChangeScene), fadeController.fadeDuration);

        // 페이드 아웃 후 오브젝트 삭제
        Invoke(nameof(DestroyPlayer), fadeController.fadeDuration);
    }

    void ChangeScene()
    {
        // 씬 전환 로직
        SceneManager.LoadScene(2);
    }

    private void DestroyPlayer()
    {
        // 오브젝트 삭제
        Destroy(GameObject.FindWithTag("Player"));
    }
}
