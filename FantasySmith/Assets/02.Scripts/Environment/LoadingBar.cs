using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingBar : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;
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
            yield return null;
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // �ε� ���� ����
            progressbar.value = progress;

            if (progressbar.value < 0.9f) // �ε��� 100% �Ϸ�Ǹ�
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            }
            else if (operation.progress > 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }

            if (progressbar.value >= 1f && operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f); // ���� ������ 1�� �ӹ�����
                operation.allowSceneActivation = true; // ���� ������ ��ȯ
            }
        }
    }
}
