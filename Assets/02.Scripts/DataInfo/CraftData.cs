using System;
using System.Collections.Generic;
using UnityEngine;
/*
 * [파일 목적]
 * 플레이어 제작에 필요한 데이터이며, 직렬화되어 Save 및 Load 되어야 할 집합
 * 
 * [작업 사항]  
 * <v1.0 - 2023_1105_최원준>
 * 1- 제작관련 클래스 파일분리 
 * 
 * <v2.0 - 2023_1106_최원준>
 * 1- 플레이어 인벤토리 클래스 추가, CraftProficiency 구조체에 itemList 포함
 * 2- 주석처리
 * 
 * 3- 제작목록을 통합목록 리스트에서 CraftableWeaponList 클래스로 변경하여 각 종류별 이름을 보관하도록 변경
 * => 제작 1단계에서 드롭다운 버튼으로 무기 종류별로 제작 목록을 보여줄 필요성이 있기 때문 
 * 
 * <v3.0 - 2023_1112_최원준>
 * 1- 인벤토리 변수 이름변경(InvnetoryMaxCount->TotalMaxCount)
 * 2- 인벤토리 클래스명 PlayerInventory에서 Inventory로 변경
 * 
 * <v4.0 - 2023_1113_최원준>
 * 1- 분량 관계로 Ivnentory 클래스를 Inventory.cs파일로 분리하였음. (namespace는 그대로)
 */


namespace CraftData
{   
    /// <summary>
    /// 플레이어 제작 가능 무기의 목록입니다. 무기의 종류 별로 string name을 저장하는 리스트들을 보유하고 있습니다.
    /// </summary>
    [Serializable]
    public class CraftableWeaponList
    {
        /// <summary>
        /// 제작가능한 검-장검 목록입니다. string weaponName으로 접근하십시오.
        /// </summary>
        public List<string> swordList;

        /// <summary>
        /// 제작가능한 활-보우 목록입니다. string weaponName으로 접근하십시오.
        /// </summary>
        public List<string> bowList;

        public CraftableWeaponList()
        {
            swordList = new List<string>();
            bowList = new List<string>();
        }
    }

        


    /// <summary>
    /// 제작 관련 속성들을 모아놓은 클래스로서 제작에 필요한 플레이어의 데이터에 해당
    /// </summary>
    [Serializable]
    public struct CraftProficiency
    {   
        private int iProficiency;
        /// <summary>
        /// 장비의 제작 숙련도를 기록
        /// </summary>
        public int Proficiency
        {
            set {  iProficiency = Mathf.Clamp(value, 0, 100); }    
            get { return iProficiency; }
        }

        private int iRecipieHitCount;
        /// <summary>
        /// 레시피가 정확하게 맞은 횟수
        /// </summary>
        public int RecipieHitCount 
        { 
            get { return iRecipieHitCount; } 
            set { iRecipieHitCount = value; }
        }
        
        public CraftProficiency(int proficiency, int hitCount)
        {
            iProficiency= proficiency;
            iRecipieHitCount= hitCount;
        }
    }


}
