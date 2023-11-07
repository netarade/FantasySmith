using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Text timeText;
    public DateManager dateManager; // ��¥ ��ũ��Ʈ ����
    private float gameHour = 8f; // ���� �ð� (8�ú��� ����)

    private void Start()
    {
        // ���� �ð� �������� 12�� �����Ͽ� 1�ð��� 5�� ���� ����ǵ��� �մϴ�.
        Time.timeScale = 12f;
    }

    private void Update()
    {
        // ���� �ð� ������Ʈ
        gameHour += Time.deltaTime / 3600; // 1 �ð��� 3600��
        if (gameHour >= 24f)
        {
            gameHour = 8f;
            // �Ϸ簡 ���� �� ��¥ ��ũ��Ʈ�� ������Ʈ�մϴ�.
            dateManager.AdvanceDay();
        }

        // ���� �ð��� ��:�� �������� ��ȯ
        int hours = Mathf.FloorToInt(gameHour);
        int minutes = Mathf.FloorToInt((gameHour - hours) * 60);
        string amPm = (hours < 12) ? "AM" : "PM";

        if (hours > 12)
        {
            hours -= 12;
        }
        else if (hours == 0)
        {
            hours = 12;
        }

        string timeString = string.Format("{0:D2}:{1:D2} {2}", hours, minutes, amPm);

        // UI Text�� ���� �ð� ǥ��
        timeText.text = timeString;
    }
}
