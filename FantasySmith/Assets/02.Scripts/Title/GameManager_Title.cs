using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Time
public class GameManager_Title : MonoBehaviour
{
    public Button highQualityButton;
    public Button mediumQualityButton;
    public Button lowQualityButton;

    void Start()
    {
        AudioManager.instance.PlayBgm(true);
    }

    void Update()
    {
        highQualityButton.onClick.AddListener(SetHighQuality);
        mediumQualityButton.onClick.AddListener(SetMediumQuality);
        lowQualityButton.onClick.AddListener(SetLowQuality);
    }

    void SetHighQuality()
    {
        QualitySettings.SetQualityLevel(2); // 고 품질
        // 설정을 저장 (PlayerPrefs 또는 다른 저장 방법 사용)
    }

    void SetMediumQuality()
    {
        QualitySettings.SetQualityLevel(1); // 중간 품질
        // 설정을 저장 (PlayerPrefs 또는 다른 저장 방법 사용)
    }

    void SetLowQuality()
    {
        QualitySettings.SetQualityLevel(0); // 낮은 품질
        // 설정을 저장 (PlayerPrefs 또는 다른 저장 방법 사용)
    }
}