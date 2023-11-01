using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoClickButton : MonoBehaviour
{
    public Button myButton; // 게임 시작과 함께 눌러진 상태로 만들 버튼

    void Start()
    {
        // 버튼을 활성화
        myButton.interactable = true;

        // 이벤트 시스템을 통해 버튼을 선택하고 클릭 이벤트를 호출
        EventSystem.current.SetSelectedGameObject(myButton.gameObject);
        myButton.onClick.Invoke();
    }
}
