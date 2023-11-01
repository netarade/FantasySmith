using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // 배경 음악 오디오 소스
    //public AudioClip backgroundMusicClip; // 배경 음악 오디오 클립
    public AudioSource soundEffect; // 효과음 오디오 소스
    //public AudioClip buttonClickSound; // 버튼 클릭 효과음 오디오 클립
    public Slider backgroundMusicSlider; // 배경 음악 볼륨 조절 슬라이더
    public Slider soundEffectSlider; // 효과음 볼륨 조절 슬라이더

    void Start()
    {
        // 슬라이더의 값이 변경될 때 이벤트를 수신하도록 설정
        backgroundMusicSlider.onValueChanged.AddListener(ChangeBackgroundMusicVolume);
        soundEffectSlider.onValueChanged.AddListener(ChangeSoundEffectVolume);
    }

    // 배경 음악 재생 함수
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            
            backgroundMusic.Play();
        }
    }

    // 버튼 클릭 효과음 재생 함수
    public void PlayButtonClickSound()
    {
        if (soundEffect != null)
        {
           
            soundEffect.Play();
        }
    }

    // 배경 음악 볼륨 조절 함수
    public void ChangeBackgroundMusicVolume(float newVolume)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = newVolume;
        }
    }

    // 효과음 볼륨 조절 함수
    public void ChangeSoundEffectVolume(float newVolume)
    {
        if (soundEffect != null)
        {
            soundEffect.volume = newVolume;
        }
    }
}
