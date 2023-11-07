using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    [SerializeField] Texture2D defaultCursorImg; // �⺻ Ŀ�� �̹���
    [SerializeField] Texture2D clickedCursorImg; // Ŭ������ �� ����� Ŀ�� �̹���

    private bool isMouseButtonDown = false;

    void Start()
    {
        defaultCursorImg = Resources.Load<Texture2D>("Images/defalt");
        clickedCursorImg = Resources.Load<Texture2D>("Images/click");
        Cursor.SetCursor(defaultCursorImg, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư�� Ŭ������ ��
        {
            Cursor.SetCursor(clickedCursorImg, Vector2.zero, CursorMode.ForceSoftware);
            isMouseButtonDown = true;
        }

        if (isMouseButtonDown && Input.GetMouseButtonUp(0)) // ���콺 ���� ��ư Ŭ���� ������ ��
        {
            Cursor.SetCursor(defaultCursorImg, Vector2.zero, CursorMode.ForceSoftware);
            isMouseButtonDown = false;
        }
    }
}
