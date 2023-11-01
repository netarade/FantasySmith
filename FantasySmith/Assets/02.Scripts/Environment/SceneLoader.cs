using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        // 다음 씬을 로드합니다. 씬의 빌드 인덱스를 사용하거나 씬의 이름을 사용할 수 있습니다.
        SceneManager.LoadScene("Loading");
    }
}
