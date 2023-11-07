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

    public enum Sfx { Btn } // 넣을 효과음 개요를 입력

    AudioSource[] sfxPlayers;

    public Slider bgmSlider; // BGM 슬라이더
    public Slider sfxSlider; // SFX 슬라이더

    private bool isFadingOut = false;
    private float fadeDuration = 2.0f; // 서서히 음악을 꺼지게 할 시간 (초)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있는 경우 중복된 스크립트를 파괴
        }
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVoluem;
        bgmPlayer.clip = bgmClip;

        // 효과음 플레이어 초기화
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
        // 배경음 슬라이더 값이 변경될 때 호출됩니다.
        bgmVoluem = bgmSlider.value;
        bgmPlayer.volume = bgmVoluem;
    }

    public void OnSFXSliderValueChanged()
    {
        // 효과음 슬라이더 값이 변경될 때 호출됩니다.
        sfxVoluem = sfxSlider.value;
        UpdateSFXVolume(); // SFX 볼륨 업데이트
    }

    void UpdateSFXVolume()
    {
        // 모든 SFX 플레이어의 볼륨을 업데이트
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index].volume = sfxVoluem;
        }
    }
}
