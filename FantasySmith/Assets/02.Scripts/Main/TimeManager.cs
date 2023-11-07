using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Text timeText;
    public DateManager dateManager; // 날짜 스크립트 참조
    private float gameHour = 8f; // 게임 시간 (8시부터 시작)

    private void Start()
    {
        // 게임 시간 스케일을 12로 설정하여 1시간이 5분 동안 진행되도록 합니다.
        Time.timeScale = 12f;
    }

    private void Update()
    {
        // 게임 시간 업데이트
        gameHour += Time.deltaTime / 3600; // 1 시간에 3600초
        if (gameHour >= 24f)
        {
            gameHour = 8f;
            // 하루가 지날 때 날짜 스크립트를 업데이트합니다.
            dateManager.AdvanceDay();
        }

        // 게임 시간을 시:분 형식으로 변환
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

        // UI Text에 현재 시간 표시
        timeText.text = timeString;
    }
}
