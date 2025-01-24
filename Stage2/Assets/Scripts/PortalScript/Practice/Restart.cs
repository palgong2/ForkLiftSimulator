using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public FadeController fadeController;

    public void LoadScene()
    {
        Time.timeScale = 1f;

        // ���̵� �ƿ� ����
        fadeController.FadeOut();

        // ���̵� �ƿ� �� �� ��ȯ ����
        Invoke(nameof(ChangeScene), fadeController.fadeDuration);

        // ���̵� �ƿ� �� ������Ʈ ����
        Invoke(nameof(DestroyForkLift), fadeController.fadeDuration);
    }

    void ChangeScene()
    {
        // �� ��ȯ ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void DestroyForkLift()
    {
        // ������Ʈ ����
        Destroy(GameObject.FindWithTag("ForkLift"));
    }
}
