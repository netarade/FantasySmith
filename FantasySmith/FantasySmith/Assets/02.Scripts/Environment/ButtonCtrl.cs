using UnityEngine;
using UnityEngine.UI;

public class ButtonCtrl : MonoBehaviour
{
    public GameObject targetComponent; // Ȱ��ȭ �Ǵ� ��Ȱ��ȭ�� ��� ������Ʈ

    public void ToggleComponent()
    {
        if (targetComponent != null)
        {
            targetComponent.SetActive(!targetComponent.activeSelf);
        }
    }
}
