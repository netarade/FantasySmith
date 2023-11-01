using UnityEngine;
using UnityEngine.UI;

public class ButtonCtrl : MonoBehaviour
{
    public GameObject targetComponent; // 활성화 또는 비활성화할 대상 컴포넌트

    public void ToggleComponent()
    {
        if (targetComponent != null)
        {
            targetComponent.SetActive(!targetComponent.activeSelf);
        }
    }
}
