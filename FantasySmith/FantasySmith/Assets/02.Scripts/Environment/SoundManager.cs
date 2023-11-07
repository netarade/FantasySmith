using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // ��� ���� ����� �ҽ�

    void Awake()
    {
        PlayBackgroundMusic(); // ���� ���� �� ��� ������ ���
    }

    public void PlayBackgroundMusic()
    {
        // ��� ������ �ε��ϰ� ���
        AudioClip music = Resources.Load<AudioClip>("Genshin"); // ��� ���� ���� �̸�
        if (music != null)
        {
            backgroundMusic.clip = music;
            backgroundMusic.Play();
        }
        else
        {
            Debug.LogWarning("������ ã�� �� �����ϴ�: Genshin");
        }
    }
}
