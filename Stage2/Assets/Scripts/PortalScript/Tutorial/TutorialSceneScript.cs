using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSceneScript : MonoBehaviour
{
    public FadeController fadeController;
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("ForkLift"))
        {
            isTriggered = true;

            // 페이드 아웃 시작
            fadeController.FadeOut();

            // 페이드 아웃 후 씬 전환 실행
            Invoke(nameof(FadeOutAndRestartScene), fadeController.fadeDuration);

            // 페이드 아웃 후 오브젝트 삭제
            Invoke(nameof(DestroyForkLift), fadeController.fadeDuration);
        }
    }

    private void FadeOutAndRestartScene()
    {
        // 씬 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void DestroyForkLift()
    {
        // 닿은 오브젝트 삭제
        Destroy(GameObject.FindWithTag("ForkLift"));
    }
}
