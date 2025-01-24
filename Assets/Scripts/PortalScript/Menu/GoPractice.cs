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
        // ���̵� �ƿ� �� �� ��ȯ ����
        Invoke(nameof(ChangeScene), fadeController.fadeDuration);

        // ���̵� �ƿ� �� ������Ʈ ����
        Invoke(nameof(DestroyPlayer), fadeController.fadeDuration);
    }

    void ChangeScene()
    {
        // �� ��ȯ ����
        SceneManager.LoadScene(2);
    }

    private void DestroyPlayer()
    {
        // ������Ʈ ����
        Destroy(GameObject.FindWithTag("Player"));
    }
}
