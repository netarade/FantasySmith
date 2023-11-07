using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //public float playT;
    //public float C_playT;
    //public float H_playT;
    //public int year;
    //public int month;
    //public int day;
    //public int hour;
    //public bool isOpen = false;
    //PlayerAnimationCtrl playerAnimationCtrl;

    public Text dateText;
    public Text timeText;

    private int currentDay = 1;
    private int currentMonth = 1;
    private int currentYear = 2023;

    private float gameHour = 8f;

    public Text todayGoldText;
    public Text todaySilverText;
    public Text totalGoldText;
    public Text totalSilverText;

    private int todayGold = 0; // 오늘 번 금화
    private int todaySilver = 0; // 오늘 번 은화
    private int totalGold = 0; // 보유 금화
    private int totalSilver = 0; // 보유 은화

    public Text textDisplay; //장소 텍스트
    public string newText;

    private bool isInsideZone = false;

    void Start()
    {
        AudioManager.instance.PlayBgm(true);

        UpdateDateText();
        UpdateTimeText();

        UpdateMoneyText();
    }

    void Update()
    {
        //playerAnimationCtrl = GameObject.Find("Player").GetComponent<PlayerAnimationCtrl>();
        //C_playT = 여기에 저장된 총 플레이 시간 데이터 불러오기
        //H_playT = C_playT % 5
        //hout = C_playT / 5
        //day = hour / 24
        //month = day / 30
        //year = month / 12

        gameHour += Time.deltaTime / 3600;
        if (gameHour >= 24f)
        {
            gameHour = 8f;
            AdvanceDay();

        }

        UpdateTimeText();
    }

    private void UpdateDateText()
    {
        string date = string.Format("{0:D4}-{1:D2}-{2:D2}", currentYear, currentMonth, currentDay);
        dateText.text = date;
    }

    private void UpdateTimeText()
    {
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
        timeText.text = timeString;
    }

    private void AdvanceDay()
    {
        currentDay++;
        if (currentDay > 30)
        {
            currentDay = 1;
            currentMonth++;
            if (currentMonth > 12)
            {
                currentMonth = 1;
                currentYear++;
            }
        }

        UpdateDateText();
    }

    //void FixedUpdate()
    //{
    //    //게임오버가 아니고, 플레이어 캐릭터가 존재할때
    //    //if(GameManager.GameOver == false && playerAnimationCtrl.Player != null)
    //    playT = Time.realtimeSinceStartup;
    //    C_playT += Time.deltaTime;
    //    H_playT += Time.deltaTime;

    //    if (H_playT >= 5)
    //    {
    //        H_playT = 0;
    //        hour += 1;
    //        if (hour > 24)
    //        {
    //            hour = hour - 24;
    //            day += 1;
    //            if (day > 30)
    //            {
    //                day = 0;
    //                month += 1;
    //                if (month > 12)
    //                {
    //                    month = 0;
    //                    year += 1;
    //                }
    //            }
    //        }
    //    }
    //    if (isOpen == true)
    //    {
    //        hour += 5;
    //        day += 1;
    //        if (hour > 24)
    //        {
    //            hour = hour - 24;
    //            if (day > 30)
    //            {
    //                day = 0;
    //                month += 1;
    //                if (month > 12)
    //                {
    //                    month = 0;
    //                    year += 1;
    //                }
    //            }
    //        }
    //    }
    //}

    private void UpdateMoneyText()
    {
        todayGoldText.text = "" + todayGold;
        todaySilverText.text = "" + todaySilver;
        totalGoldText.text = "" + totalGold;
        totalSilverText.text = "" + totalSilver;
    }

    public void EarnTodayGold(int amount)
    {
        todayGold += amount;
        UpdateMoneyText();
    }

    public void EarnTodaySilver(int amount)
    {
        todaySilver += amount;
        UpdateMoneyText();
    }

    public void ConvertTodayToTotal()
    {
        totalGold += todayGold;
        totalSilver += todaySilver;
        todayGold = 0;
        todaySilver = 0;
        UpdateMoneyText();
    }

    private void OnTriggerEnter(Collider other) //장소 텍스트
    {
        if (other.CompareTag("Player"))
        {
            isInsideZone = true;
            textDisplay.text = newText;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInsideZone = false;
            textDisplay.text = "";
        }
    }

}