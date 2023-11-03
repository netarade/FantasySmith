using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingBar : MonoBehaviour
{
    public Slider progressbar;
    public float loadingTime = 3.0f; // �ε� �ð�(��)

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("Main_Scene");
        operation.allowSceneActivation = false; // ���� ������ ��ȯ�� ����

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // �ε� ���� ����
            progressbar.value = progress;

            if (progress >= 1f) // �ε��� 100% �Ϸ�Ǹ�
            {
                operation.allowSceneActivation = true; // ���� ������ ��ȯ ���
            }

            yield return null;
        }
    }
}
