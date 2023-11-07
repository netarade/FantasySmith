using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using CraftData;

/*
 * [파일 목적]
 * 게임 세이브와 로드에 필요한 데이터 클래스 구성 
 * 
 * [작업 사항]  
 * <v1.0 - 2023_1106_최원준>
 * 1- 초기 GameData 설정
 * 
 * <v1.1 - 2023_1106_최원준
 * 1- Transform 변수 제거, 클래스 CraftableWeaponList 변수 추가
 * 2- 주석 수정
 * 
 * <v1.2 - 2023_1108_최원준>
 * 1- Inventory 클래스가 게임오브젝트 리스트를 포함하고 있기 때문에 저장안되는 문제가 있음을 알고
 * 내부적으로 ItemInfo 리스트로 만들어 보려고 시도하였음. 
 * 또 다른 문제점이 있는데 ItemInfo는 Image컴포넌트를 포함하고 있기 때문에 클래스 구조상 직렬화하기 힘들다고 생각됨.
 * 
 */

namespace DataManagement
{


    /// <summary>
    /// 주의 사항 - 유니티 전용 클래스는 저장 불가. 기본 자료형으로 저장하거나, 구조체 또는 클래스를 만들어 저장한다. 
    /// </summary>
    [Serializable]           // 인스펙터에 아래의 클래스가 보여진다.
    public class GameData
    {
        /// <summary>
        /// 누적 플레이 타임
        /// </summary>
        public float playTime;

        /// <summary>
        /// 금화
        /// </summary>
        public float gold;

        /// <summary>
        /// 은화
        /// </summary>
        public float silver;

        /// <summary>
        /// 플레이어 변수
        /// </summary>
        public Transform playerTr;


        /// <summary>
        /// 현재 플레이어가 보관하고 있는 인벤토리 정보입니다. 게임오브젝트를 보관하는 weapList와 playerMiscList 및 InventoryMaxCount 등이 있습니다.
        /// </summary>
        public PlayerInventory inventory;


        /// <summary>
        /// 제작 가능 목록을 종류 별 string name으로 보관하는 리스트들의 집합 클래스 입니다.
        /// </summary>
        public CraftableWeaponList craftableWeaponList;

        /// <summary>
        /// 장비 숙련도 목록을 name과 CraftProficincy구조체형 으로 보관하여 빠르게 접근을 가능하게 해줍니다.
        /// </summary>
        public Dictionary<string, CraftProficiency> proficiencyDic;
               










        
        // 미구현 클래스 및 메서드
        //public PlayerInventory saveInventory
        //{
        //    set { loadInventory=new SerializableInventory( value ); }
        //}

        //SerializableInventory loadInventory;

    }

    //[Serializable]
    //public class SerializableInventory
    //{
    //    public List<ItemInfo> weapList;
    //    public List<ItemInfo> miscList;
    //    public int InventoryMaxCount; 

    //    public SerializableInventory(PlayerInventory inventory)
    //    {
    //        foreach( GameObject item in inventory.weapList )
    //           this.weapList.Add( item.GetComponent<ItemInfo>() );

    //        foreach( GameObject item in inventory.miscList )
    //            this.miscList.Add( item.GetComponent<ItemInfo>() );

    //        this.InventoryMaxCount = inventory.InventoryMaxCount;
    //    }

    //    public PlayerInventory Load()
    //    {
    //        CreateManager.instance.CreateItemByItemInfo

    //        for(int i=0; i<weapList.Count; i++)
    //            weapList[i].item.Type == ItemType.Misc
            
    //        PlayerInventory loadInventory = new PlayerInventory();
    //    }

    //}


}