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

    //public float playT;
    //public float C_playT;
    //public float H_playT;
    //public int year;
    //public int month;
    //public int day;
    //public int hour;
    //public bool isOpen = false;
    //PlayerAnimationCtrl playerAnimationCtrl;

    void Start()
    {
        AudioManager_Title.instance.PlayBgm(true);
    }

    void Update()
    {
        highQualityButton.onClick.AddListener(SetHighQuality);
        mediumQualityButton.onClick.AddListener(SetMediumQuality);
        lowQualityButton.onClick.AddListener(SetLowQuality);

        //playerAnimationCtrl = GameObject.Find("Player").GetComponent<PlayerAnimationCtrl>();
        //C_playT = 여기에 저장된 총 플레이 시간 데이터 불러오기
        //H_playT = C_playT % 5
        //hout = C_playT / 5
        //day = hour / 24
        //month = day / 30
        //year = month / 12
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
}