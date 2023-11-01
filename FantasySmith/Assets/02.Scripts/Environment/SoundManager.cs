using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // ��� ���� ����� �ҽ�
    //public AudioClip backgroundMusicClip; // ��� ���� ����� Ŭ��
    public AudioSource soundEffect; // ȿ���� ����� �ҽ�
    //public AudioClip buttonClickSound; // ��ư Ŭ�� ȿ���� ����� Ŭ��
    public Slider backgroundMusicSlider; // ��� ���� ���� ���� �����̴�
    public Slider soundEffectSlider; // ȿ���� ���� ���� �����̴�

    void Start()
    {
        // �����̴��� ���� ����� �� �̺�Ʈ�� �����ϵ��� ����
        backgroundMusicSlider.onValueChanged.AddListener(ChangeBackgroundMusicVolume);
        soundEffectSlider.onValueChanged.AddListener(ChangeSoundEffectVolume);
    }

    // ��� ���� ��� �Լ�
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            
            backgroundMusic.Play();
        }
    }

    // ��ư Ŭ�� ȿ���� ��� �Լ�
    public void PlayButtonClickSound()
    {
        if (soundEffect != null)
        {
           
            soundEffect.Play();
        }
    }

    // ��� ���� ���� ���� �Լ�
    public void ChangeBackgroundMusicVolume(float newVolume)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = newVolume;
        }
    }

    // ȿ���� ���� ���� �Լ�
    public void ChangeSoundEffectVolume(float newVolume)
    {
        if (soundEffect != null)
        {
            soundEffect.volume = newVolume;
        }
    }
}
