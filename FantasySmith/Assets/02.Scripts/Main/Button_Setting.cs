using UnityEngine;
using UnityEngine.UI;

public class Button_Setting : MonoBehaviour
{
    public GameObject targetComponent; // Ȱ��ȭ �Ǵ� ��Ȱ��ȭ�� ��� ������Ʈ

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
