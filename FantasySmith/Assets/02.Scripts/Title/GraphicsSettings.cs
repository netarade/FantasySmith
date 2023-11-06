using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    public Button highQualityButton;
    public Button mediumQualityButton;
    public Button lowQualityButton;

    void Start()
    {
        // 각 버튼에 클릭 이벤트 핸들러를 추가
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
