using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    [SerializeField] Texture2D defaultCursorImg; // �⺻ Ŀ�� �̹���
    [SerializeField] Texture2D clickedCursorImg; // Ŭ������ �� ����� Ŀ�� �̹���

    private bool isMouseButtonDown = false;

    void Start()
    {
        // �⺻ �̹����� �����ϰ� �̹����� ���� ��� �𼭸��� hotspot���� ����
        Cursor.SetCursor(defaultCursorImg, new Vector2(10, 10), CursorMode.ForceSoftware);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư�� Ŭ������ ��
        {
            // Ŭ�� �̹����� �����ϰ� �̹����� ���� ��� �𼭸��� hotspot���� ����
            Cursor.SetCursor(clickedCursorImg, new Vector2(10, 10), CursorMode.ForceSoftware);
            isMouseButtonDown = true;
        }

        if (isMouseButtonDown && Input.GetMouseButtonUp(0)) // ���콺 ���� ��ư Ŭ���� ������ ��
        {
            // �⺻ �̹����� �����ϰ� �̹����� ���� ��� �𼭸��� hotspot���� ����
            Cursor.SetCursor(defaultCursorImg, new Vector2(10, 10), CursorMode.ForceSoftware);
            isMouseButtonDown = false;
        }
    }
}
