using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    [SerializeField] Texture2D defaultCursorImg; // 기본 커서 이미지
    [SerializeField] Texture2D clickedCursorImg; // 클릭했을 때 사용할 커서 이미지

    private bool isMouseButtonDown = false;

    void Start()
    {
        // 기본 이미지를 설정하고 이미지의 왼쪽 상단 모서리를 hotspot으로 지정
        Cursor.SetCursor(defaultCursorImg, new Vector2(10, 10), CursorMode.ForceSoftware);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 클릭했을 때
        {
            // 클릭 이미지를 설정하고 이미지의 왼쪽 상단 모서리를 hotspot으로 지정
            Cursor.SetCursor(clickedCursorImg, new Vector2(10, 10), CursorMode.ForceSoftware);
            isMouseButtonDown = true;
        }

        if (isMouseButtonDown && Input.GetMouseButtonUp(0)) // 마우스 왼쪽 버튼 클릭을 놓았을 때
        {
            // 기본 이미지로 복원하고 이미지의 왼쪽 상단 모서리를 hotspot으로 지정
            Cursor.SetCursor(defaultCursorImg, new Vector2(10, 10), CursorMode.ForceSoftware);
            isMouseButtonDown = false;
        }
    }
}
