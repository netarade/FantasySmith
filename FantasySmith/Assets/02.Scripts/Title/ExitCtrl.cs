using UnityEngine;

public class ExitCtrl : MonoBehaviour
{
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서는 종료됩니다.
#else
        Application.Quit(); // 빌드된 애플리케이션에서는 종료됩니다.
#endif
    }
}
