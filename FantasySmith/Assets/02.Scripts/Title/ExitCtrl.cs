using UnityEngine;

public class ExitCtrl : MonoBehaviour
{
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �����Ϳ����� ����˴ϴ�.
#else
        Application.Quit(); // ����� ���ø����̼ǿ����� ����˴ϴ�.
#endif
    }
}
