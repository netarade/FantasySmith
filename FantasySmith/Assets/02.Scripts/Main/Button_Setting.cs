using UnityEngine;
using UnityEngine.UI;

public class Button_Setting : MonoBehaviour
{
    public GameObject targetComponent; // 활성화 또는 비활성화할 대상 컴포넌트

    public AudioClip clip;

    public void ToggleComponent()
    {
        if (targetComponent != null)
        {
            targetComponent.SetActive(!targetComponent.activeSelf);
        }

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Btn);
    }
}
