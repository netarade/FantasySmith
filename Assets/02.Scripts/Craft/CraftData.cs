using System;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using WorldItemData;
using Newtonsoft.Json;

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
 * 3- 제작목록을 통합목록 리스트에서 Craftdic 클래스로 변경하여 각 종류별 이름을 보관하도록 변경
 * => 제작 1단계에서 드롭다운 버튼으로 무기 종류별로 제작 목록을 보여줄 필요성이 있기 때문 
 * 
 * <v3.0 - 2023_1112_최원준>
 * 1- 인벤토리 변수 이름변경(InvnetoryMaxCount->TotalMaxCount)
 * 2- 인벤토리 클래스명 PlayerInventory에서 Inventory로 변경
 * 
 * <v4.0 - 2023_1113_최원준>
 * 1- 분량 관계로 Ivnentory 클래스를 Inventory.cs파일로 분리하였음. (namespace는 그대로)
 * 
 * <v5.0 - 2023_1119_최원준>
 * 1- CraftProficiency의 멤버변수 name을 추가하고 생성자 조정
 * 
 * 2- CraftableWeaponList를 CraftDic 클래스로 이름 변경하였으며, 전반적인 수정과 통합. 
 * 내부 리스트를 딕셔너리로 변경하여 빠른 참조를 가능하게 하였음.
 * 
 * 3- CraftDic 클래스의 역할에 따른 기능 수정 중임.
 * 이름과 숙련도를 함께 관리할 필요성 (완료)
 * 제작 목록에 있는지 없는지 검사하는 기능
 * 해당이름의 제작 숙련도를 올려주는 기능.
 * 제작 목록이 채워진다면 다른 제작목록을 해방하는 기능과 해방을 알려주는 기능이 있어야함.
 * 현재 CreateManager에서 숙련 목록을 채워주고 있지만, CraftDic 자체에서 CreateManager의 월드 아이템을 참고하여 스스로 채워야 한다.  (완료)
 * 
 * 4- 기타 주석 수정
 * 
 * <v5.1 - 2023_1122>
 * 1- 딕셔너리 멤버변수 주석 수정
 * 
 * <v5.2 - 2023_1221>
 * 1- weaponDic참조를 CreateManager의 싱글톤에서 참조하던 것을 따로 MonoBehaviour를 상속하지 않는 스크립트인 WorldItemData_Weapon의 참조로 변경
 * Craftdic스크립트가 Monobehviour를 상속하지 않기 때문에 싱글톤의 참조가 어렵기 때문
 * 
 * <v6.0 - 2023_1222_최원준>
 * 1- private변수를 직렬화하기 위해 [JsonProperty] 어트리뷰트를 추가하였음
 * 
 */


namespace CraftData
{   
    /// <summary>
    /// 플레이어 제작 가능 무기의 목록입니다. 무기의 종류 별로 string name을 저장하는 리스트들을 보유하고 있습니다.
    /// </summary>
    [Serializable]
    public class Craftdic
    {
        /// <summary>
        /// 제작가능한 검-장검 목록입니다. 아이템 이름으로 접근하면 해당 장비의 숙련도 정보를 가진 구조체를 할당 받습니다.
        /// </summary>
        public Dictionary<string, CraftProficiency> swordDic;

        /// <summary>
        /// 제작가능한 활-보우 목록입니다. 아이템 이름으로 접근하면 해당 장비의 숙련도 정보를 가진 구조체를 할당 받습니다.
        /// </summary>
        public Dictionary<string, CraftProficiency> bowDic;


        /// <summary>
        /// 게임을 처음 시작하여 제작 목록을 처음 생성하는 경우 초급 아이템만 해방된 채로 제작 목록이 생성됩니다.
        /// </summary>
        public Craftdic()
        {
            swordDic = new Dictionary<string, CraftProficiency> ();
            bowDic = new Dictionary<string, CraftProficiency> ();           

            Dictionary<string, Item> weaponDic = new WorldItem().weapDic;         // 월드 무기사전을 참조합니다.

            foreach( Item item in weaponDic.Values )                                // 모든 무기사전에서 하나씩 꺼냅니다.
            {
                ItemWeapon weapItem = (ItemWeapon)item;                             // 하나씩 꺼냈을 때 value값은 Item형이며, 더 많은 정보 참조를 위해 ItemWeapon형으로 변환합니다.

                if( weapItem.BasicGrade==ItemGrade.Low )                            // 무기 사전에서 꺼낸 아이템이 초급 무기라면,
                {
                    if( weapItem.WeaponType==WeaponType.Sword )                             // 검-장검 타입이라면,                
                        swordDic.Add( weapItem.Name, new CraftProficiency(weapItem.Name) );     // 플레이어 제작 목록 검-장검 목록에 추가합니다.                
                    else if( weapItem.WeaponType==WeaponType.Bow )                          // 활-보우 타입이라면,
                        bowDic.Add( weapItem.Name, new CraftProficiency(weapItem.Name) );       // 플레이어 제작 목록 활-보우 목록에 추가합니다.
                }
            }
        }
    }

        


    /// <summary>
    /// 제작 관련 속성들을 모아놓은 클래스로서 제작에 필요한 플레이어의 데이터에 해당
    /// </summary>
    [Serializable]
    public struct CraftProficiency
    {   
        [JsonProperty] private string sName;               // 제작 장비의 이름
        [JsonProperty] private int iProficiency;           // 제작 장비의 제작 숙련도
        [JsonProperty] private int iRecipieHitCount;       // 제작 장비의 레시피 횟수 
        
        /// <summary>
        /// 제작 장비의 이름입니다.
        /// </summary>
        public string Name { get { return sName; } }

        /// <summary>
        /// 제작 장비의 숙련도 입니다. 제한 범위는 0~100 이내로 설정 가능합니다.
        /// </summary>
        public int Proficiency
        {
            set {  iProficiency = Mathf.Clamp(value, 0, 100); }    
            get { return iProficiency; }
        }

        /// <summary>
        /// 플레이어가 제작 장비의 레시피를 정확하게 맞힌 횟수입니다.
        /// </summary>
        public int RecipieHitCount 
        { 
            get { return iRecipieHitCount; } 
            set { iRecipieHitCount = value; }
        }
        
        /// <summary>
        /// 이름만 가지고 제작 정보를 생성하는 경우 처음 생성한 것으로 판단하며, 숙련도와 레시피 맞춘 횟수가 0으로 초기화됩니다.
        /// </summary>
        public CraftProficiency(string name)
        {
            sName = name;
            iProficiency = 0;
            iRecipieHitCount = 0;
        }

        /// <summary>
        /// 기존의 제작 정보가 있는 경우 각 인자를 입력하여 직접 초기화 합니다.
        /// </summary>
        /// <param name="name">제작 장비의 이름</param>
        /// <param name="proficiency">제작 숙련도</param>
        /// <param name="hitCount">레시피를 맞춘 횟수</param>
        public CraftProficiency(string name, int proficiency, int hitCount)
        {
            sName = name;
            iProficiency= proficiency;
            iRecipieHitCount= hitCount;
        }
    }


}
