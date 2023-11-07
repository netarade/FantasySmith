using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVoluem;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVoluem;
    public int channels;
    int channelIndex;

    public enum Sfx { Btn } // ���� ȿ���� ���並 �Է�

    AudioSource[] sfxPlayers;

    public Slider bgmSlider; // BGM �����̴�
    public Slider sfxSlider; // SFX �����̴�

    private bool isFadingOut = false;
    private float fadeDuration = 2.0f; // ������ ������ ������ �� �ð� (��)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �ִ� ��� �ߺ��� ��ũ��Ʈ�� �ı�
        }
    }

    void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVoluem;
        bgmPlayer.clip = bgmClip;

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVoluem;
        }
    }

    void Update()
    {
        if (isFadingOut)
        {
            float startVolume = bgmPlayer.volume;
            float t = 0;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                bgmPlayer.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                if (bgmPlayer.volume == 0)
                {
                    isFadingOut = false;
                    bgmPlayer.Stop();
                    break;
                }
            }
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            isFadingOut = true;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;
            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void OnBGMSliderValueChanged()
    {
        // ����� �����̴� ���� ����� �� ȣ��˴ϴ�.
        bgmVoluem = bgmSlider.value;
        bgmPlayer.volume = bgmVoluem;
    }

    public void OnSFXSliderValueChanged()
    {
        // ȿ���� �����̴� ���� ����� �� ȣ��˴ϴ�.
        sfxVoluem = sfxSlider.value;
        UpdateSFXVolume(); // SFX ���� ������Ʈ
    }

    void UpdateSFXVolume()
    {
        // ��� SFX �÷��̾��� ������ ������Ʈ
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index].volume = sfxVoluem;
        }
    }
}
