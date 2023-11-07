using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingBar : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;
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
            yield return null;
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // 로딩 진행 상태
            progressbar.value = progress;

            if (progressbar.value < 0.9f) // 로딩이 100% 완료되면
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            }
            else if (operation.progress > 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }

            if (progressbar.value >= 1f && operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f); // 현재 씬에서 1초 머무르기
                operation.allowSceneActivation = true; // 다음 씬으로 전환
            }
        }
    }
}
