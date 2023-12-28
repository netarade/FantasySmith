using CraftData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataManagement
{ 
    /// <summary>
    /// 주의 사항 - 유니티 전용 클래스는 저장 불가. 기본 자료형으로 저장하거나, 구조체 또는 클래스를 만들어 저장해야 합니다.
    /// </summary>
    public class PlayerBasicData : SaveData
    {
        /// <summary>
        /// 누적 플레이 타임
        /// </summary>
        public float playTime;

        /// <summary>
        /// 금화
        /// </summary>
        public int gold;

        /// <summary>
        /// 은화
        /// </summary>
        public int silver;

        /// <summary>
        /// 플레이어의 제작 가능 목록과 숙련도, 레시피 맞춘 횟수 정보가 담겨 있는 제작 관련 클래스 입니다.
        /// </summary>
        public Craftdic craftDic;        
             

        /// <summary>
        /// DataManager에서 Load메서드에서 새로운 GameData를 생성하기 위한 생성자입니다.<br/>
        /// 기존의 데이터가 없을 경우 사용됩니다.
        /// </summary>
        public PlayerBasicData()
        {            
            playTime = 0f;
            gold = 0;
            silver = 0;
            craftDic = new Craftdic();
        }

    }
    
}