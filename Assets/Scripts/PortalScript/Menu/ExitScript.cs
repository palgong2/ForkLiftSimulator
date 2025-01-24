using System.Collections;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    public FadeController fadeController;

    public void Exit()
    {
        // ���̵� �ƿ��� �����ϰ�, �Ϸ�� �� ���ø����̼��� �����մϴ�.
        fadeController.FadeOut();
        Invoke(nameof(OnFadeOutComplete), fadeController.fadeDuration);
    }

    private void OnFadeOutComplete()
    {
        // ���̵� �ƿ��� �Ϸ�� �� ���ø����̼� ����
        //#if UNITY_EDITOR
        //        UnityEditor.EditorApplication.isPlaying = false;
        //#else
                Application.Quit();
        //#endif
    }
}
