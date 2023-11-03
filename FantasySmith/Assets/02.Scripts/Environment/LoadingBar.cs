using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingBar : MonoBehaviour
{
    public Slider progressbar;
    public float loadingTime = 3.0f; // 로딩 시간(초)

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("Main_Scene");
        operation.allowSceneActivation = false; // 다음 씬으로 전환을 금지

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // 로딩 진행 상태
            progressbar.value = progress;

            if (progress >= 1f) // 로딩이 100% 완료되면
            {
                operation.allowSceneActivation = true; // 다음 씬으로 전환 허용
            }

            yield return null;
        }
    }
}
