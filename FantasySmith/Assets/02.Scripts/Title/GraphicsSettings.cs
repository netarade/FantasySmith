using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    public Button highQualityButton;
    public Button mediumQualityButton;
    public Button lowQualityButton;

    void Start()
    {
        // �� ��ư�� Ŭ�� �̺�Ʈ �ڵ鷯�� �߰�
        highQualityButton.onClick.AddListener(SetHighQuality);
        mediumQualityButton.onClick.AddListener(SetMediumQuality);
        lowQualityButton.onClick.AddListener(SetLowQuality);
    }

    void SetHighQuality()
    {
        QualitySettings.SetQualityLevel(2); // �� ǰ��
        // ������ ���� (PlayerPrefs �Ǵ� �ٸ� ���� ��� ���)
    }

    void SetMediumQuality()
    {
        QualitySettings.SetQualityLevel(1); // �߰� ǰ��
        // ������ ���� (PlayerPrefs �Ǵ� �ٸ� ���� ��� ���)
    }

    void SetLowQuality()
    {
        QualitySettings.SetQualityLevel(0); // ���� ǰ��
        // ������ ���� (PlayerPrefs �Ǵ� �ٸ� ���� ��� ���)
    }
}
