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

            // ���̵� �ƿ� ����
            fadeController.FadeOut();

            // ���̵� �ƿ� �� �� ��ȯ ����
            Invoke(nameof(FadeOutAndRestartScene), fadeController.fadeDuration);

            // ���̵� �ƿ� �� ������Ʈ ����
            Invoke(nameof(DestroyForkLift), fadeController.fadeDuration);
        }
    }

    private void FadeOutAndRestartScene()
    {
        // �� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void DestroyForkLift()
    {
        // ���� ������Ʈ ����
        Destroy(GameObject.FindWithTag("ForkLift"));
    }
}
