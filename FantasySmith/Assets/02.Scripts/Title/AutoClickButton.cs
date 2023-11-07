using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoClickButton : MonoBehaviour
{
    public Button myButton; // ���� ���۰� �Բ� ������ ���·� ���� ��ư

    void Start()
    {
        // ��ư�� Ȱ��ȭ
        myButton.interactable = true;

        // �̺�Ʈ �ý����� ���� ��ư�� �����ϰ� Ŭ�� �̺�Ʈ�� ȣ��
        EventSystem.current.SetSelectedGameObject(myButton.gameObject);
        myButton.onClick.Invoke();
    }
}
