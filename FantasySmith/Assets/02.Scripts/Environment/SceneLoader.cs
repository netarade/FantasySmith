using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        // ���� ���� �ε��մϴ�. ���� ���� �ε����� ����ϰų� ���� �̸��� ����� �� �ֽ��ϴ�.
        SceneManager.LoadScene("Loading");
    }
}
