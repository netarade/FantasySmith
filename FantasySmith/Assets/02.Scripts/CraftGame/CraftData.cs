using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1105_최원준>
 * 1- 제작관련 클래스 파일분리   
 */
namespace CraftData
{

    /// <summary>
    /// 제작 숙련도 관련 속성들을 모아놓은 클래스로서 플레이어가 아이템을 생성하게 되는 즉시 가지고 있어야 할 목록에 들어가게될 집합입니다.
    /// </summary>
    public class CraftProficiency
    {        
        private int proficiency;
        /// <summary>
        /// 장비의 제작 숙련도를 기록
        /// </summary>
        public int Proficiency
        {
            set {  proficiency = Mathf.Clamp(value, 0, 100); }    
            get { return proficiency; }
        }

        private int recipieHitCount;
        /// <summary>
        /// 레시피가 정확하게 맞은 횟수
        /// </summary>
        public int RecipieHitCount 
        { 
            get { return recipieHitCount; } 
            set { recipieHitCount = value; }
        }

        /// <summary>
        /// 멤버의 모든 값을 0으로 초기화하여 생성합니다.
        /// </summary>
        public CraftProficiency()
        {
            proficiency = 0;
            RecipieHitCount = 0;
        }
    }


}
