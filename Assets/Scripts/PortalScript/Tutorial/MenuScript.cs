using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
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
        // �޴�������
        SceneManager.LoadScene(0);
    }

    private void DestroyForkLift()
    {
        // ���� ������Ʈ ����
        Destroy(GameObject.FindWithTag("ForkLift"));
    }
}
