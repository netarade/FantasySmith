using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // 배경 음악 오디오 소스

    void Awake()
    {
        PlayBackgroundMusic(); // 게임 시작 시 배경 음악을 재생
    }

    public void PlayBackgroundMusic()
    {
        // 배경 음악을 로드하고 재생
        AudioClip music = Resources.Load<AudioClip>("Genshin"); // 배경 음악 파일 이름
        if (music != null)
        {
            backgroundMusic.clip = music;
            backgroundMusic.Play();
        }
        else
        {
            Debug.LogWarning("음원을 찾을 수 없습니다: Genshin");
        }
    }
}
