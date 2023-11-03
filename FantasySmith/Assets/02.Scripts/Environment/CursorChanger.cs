using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    [SerializeField] Texture2D defaultCursorImg; // 기본 커서 이미지
    [SerializeField] Texture2D clickedCursorImg; // 클릭했을 때 사용할 커서 이미지

    private bool isMouseButtonDown = false;

    void Start()
    {
        defaultCursorImg = Resources.Load<Texture2D>("Images/defalt");
        clickedCursorImg = Resources.Load<Texture2D>("Images/click");
        Cursor.SetCursor(defaultCursorImg, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 클릭했을 때
        {
            Cursor.SetCursor(clickedCursorImg, Vector2.zero, CursorMode.ForceSoftware);
            isMouseButtonDown = true;
        }

        if (isMouseButtonDown && Input.GetMouseButtonUp(0)) // 마우스 왼쪽 버튼 클릭을 놓았을 때
        {
            Cursor.SetCursor(defaultCursorImg, Vector2.zero, CursorMode.ForceSoftware);
            isMouseButtonDown = false;
        }
    }
}
