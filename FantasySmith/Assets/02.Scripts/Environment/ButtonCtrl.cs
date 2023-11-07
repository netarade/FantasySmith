using UnityEngine;
using UnityEngine.UI;

public class ButtonCtrl : MonoBehaviour
{
    public GameObject targetComponent; // Ȱ��ȭ �Ǵ� ��Ȱ��ȭ�� ��� ������Ʈ

    public AudioClip clip;

    public void ToggleComponent()
    {
        if (targetComponent != null)
        {
            targetComponent.SetActive(!targetComponent.activeSelf);
        }

        AudioManager_Title.instance.PlaySfx(AudioManager_Title.Sfx.Btn);
    }


}
