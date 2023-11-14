using ItemData;
using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * [파일 목적]
 * 플레이어 제작에 필요한 데이터이며, 직렬화되어 Save 및 Load 되어야 할 집합
 * 
 * [인벤토리의 기능]
 * 1- 직렬화 역직렬화
 * 2- 해당 아이템 삭제 시 사전목록에서 삭제, 수량 수정, 위치 반영, 
 * 
 * 
 * [작업 사항]
 * <v1.0 - 2023_1113_최원준>
 * 1- 분량 관계로 Ivnentory 클래스를 Inventory.cs파일로 분리하였음 (namespace는 그대로)
 * 
 * <v2.0 - 2023_1114_최원준>
 * 1- 인벤토리 클래스 멤버인 장비 및 잡화아이템 게임오브젝트 리스트를 딕셔너리-게임오브젝트리스트로 변경하기로 계획
 * 그 이유는 현재 수량 변경이 일어나면 리스트를 순차적으로 빼서 하나씩 정보를 열어보는 비효율이 발생하고 있기 때문이며,
 * 특히 삭제시 결정된 아이템을 목록에서 삭제하기 위해 매번 리스트를 하나씩 꺼내봐야하면 연산 자원소모가 많이 발생하기 때문
 * 
 * 
 */




namespace CraftData
{
    /// <summary>
    /// 인벤토리를 정의하는 클래스, 내부에 GameObject를 저장하는 weapList와 miscList가 있다. save, Load시 저장해야 한다.
    /// </summary>
    [Serializable]
    public class Inventory
    {
        /// <summary>
        /// 플레이어가 보유하고 있는 무기 아이템 목록입니다. 아이템 이름을 통해 해당 아이템 오브젝트를 보관하는 리스트에 접근할 수 있습니다.
        /// </summary>
        public Dictionary< string, List<GameObject> > weapDic;

        /// <summary>
        /// 플레이어가 보유하고 있는 잡화 아이템 목록 입니다. 아이템 이름을 통해 해당 아이템 오브젝트를 보관하는 리스트에 접근할 수 있습니다.
        /// </summary>
        public Dictionary< string, List<GameObject> > miscDic;


        /// <summary>
        /// 플레이어 인벤토리의 각 탭에 공통으로 주어지는 초기 칸 수입니다.
        /// </summary>
        public static int InitialCount = 50;

        /// <summary>
        /// 플레이어 인벤토리의 전체 최대 칸의 갯수 입니다. 게임 중에 업그레이드 등으로 인해 변동될 수 있습니다.
        /// </summary>
        public int TotalMaxCount {get; set;}

        /// <summary>
        /// 플레이어 인벤토리에 종류와 상관없이 모든 아이템이 차지하고 있는 현재 칸 수입니다.
        /// </summary>
        public int TotalCurCount { get{ return WeapCount + MiscCount; } }

        /// <summary>
        /// 무기 아이템이 인벤토리에 차지하고 있는 칸의 총 갯수입니다.
        /// </summary>
        public int WeapCount { 
            get{ 
                int count = 0;
                foreach(List<GameObject> objList in miscDic.Values)
                    count += objList.Count;    
                // 딕셔너리에서 게임오브젝트 리스트를 꺼내고 리스트의 Count 프로퍼티를 통해 게임오브젝트 숫자를 누적시킵니다.
                return count; } }
        
        /// <summary>
        /// 잡화 아이템이 인벤토리에 차지하고 있는 칸의 총 갯수입니다.
        /// </summary>
        public int MiscCount { get{
                int count = 0;
                foreach(List<GameObject> objList in weapDic.Values)
                    count += objList.Count;    
                // 딕셔너리에서 게임오브젝트 리스트를 꺼내고 리스트의 Count 프로퍼티를 통해 게임오브젝트 숫자를 누적시킵니다.
                return count; } }


        /// <summary>
        /// 현재 인벤토리가 보관하고 있는 사전 중에서 원하는 종류의 아이템 인스턴스 리스트를 만들어 줍니다.<br/>
        /// 오브젝트를 제외한 아이템의 순수한 정보만 읽어들일 필요가 있을 때,<br/>
        /// 게임 오브젝트 리스트를 직렬화 할 때, 씬을 넘어가기 전에 전달해야하는 용도로 사용합니다.
        /// </summary>
        public List<Item> GetItemList(ItemType itemType)
        {
            List<Item> itemList = new List<Item>();
            switch(itemType)
            {
                case ItemType.Weapon:
                    foreach( List<GameObject> objList in weapDic.Values ) // 무기사전에서 게임오브젝트 리스트를 하나씩 꺼내어
                    {
                        for(int i=0; i<objList.Count; i++)                // 리스트의 게임오브젝트를 모두 가져옵니다.
                            itemList.Add( objList[i].GetComponent<ItemInfo>().Item );   // item 스크립트를 하나씩 꺼내어 저장합니다.
                    }
                    break;

                case ItemType.Misc:
                    foreach( List<GameObject> objList in miscDic.Values ) // 잡화사전에서 게임오브젝트 리스트를 하나씩 꺼내어
                    {
                        for(int i=0; i<objList.Count; i++)                // 리스트의 게임오브젝트를 모두 가져옵니다.
                            itemList.Add( objList[i].GetComponent<ItemInfo>().Item );   // item 스크립트를 하나씩 꺼내어 저장합니다.
                    }
                    break;
            }
            return itemList;    // item 정보가 하나씩 저장되어있는 itemList를 반환합니다.
        }

        /// <summary>
        /// 종류 별 아이템 정보가 담겨있는 리스트를 집어넣으면 해당 딕셔너리로 변환시켜 줍니다.<br/>
        /// 아이템 리스트를 역직렬화 하여 로드 할때, 씬을 넘어왔을 때 다시 인벤토리 목록으로 변환해야하는 용도로 사용합니다.
        /// </summary>
        public void SetObjectList( ItemType itemType, List<Item> itemList )
        {
            switch(itemType)
            {
                case ItemType.Weapon:
                    foreach(Item item in itemList) // 아이템 리스트에서 개념 아이템 정보를 하나씩 꺼내옵니다.
                    {
                        // 아이템 정보를 바탕으로 하는 게임오브젝트를 만듭니다.
                        GameObject itemObject = CreateManager.instance.CreateItemByInfo( item );

                        if( weapDic[item.Name] == null )            // 사전에 해당하는 아이템의 이름이 없다면
                        {
                            List<GameObject> itemObjList = new List<GameObject>();
                            itemObjList.Add( itemObject );          // 새로운 리스트를 만들어 오브젝트를 넣습니다.
                            weapDic.Add(item.Name, itemObjList );   // 사전에 아이템 이름과, 리스트를 추가합니다.
                        }
                        else // 사전에 해당하는 아이템의 이름이 있다면
                        {
                            weapDic[item.Name].Add(itemObject);     // 사전에 접근하여, 해당 리스트에 오브젝트만 추가합니다.
                        }
                    }
                    break;

                case ItemType.Misc:
                    foreach(Item item in itemList) // 아이템 리스트에서
                    {
                        // 아이템 정보를 바탕으로 하는 게임오브젝트를 만듭니다.
                        GameObject itemObject = CreateManager.instance.CreateItemByInfo( item );

                        if( miscDic[item.Name] == null )            // 사전에 해당하는 아이템의 이름이 없다면
                        {
                            List<GameObject> itemObjList = new List<GameObject>();
                            itemObjList.Add( itemObject );          // 새로운 리스트를 만들어 오브젝트를 넣습니다.
                            miscDic.Add(item.Name, itemObjList );   // 사전에 아이템 이름과, 리스트를 추가합니다.
                        }
                        else // 사전에 해당하는 아이템의 이름이 있다면
                        {
                            miscDic[item.Name].Add(itemObject);     // 사전에 접근하여, 해당 리스트에 오브젝트만 추가합니다.
                        }
                    }
                    break;
                    
                // 참고) 잡화아이템의 최대 수량 검사를 따로 하지 않습니다.
                // 아이템 정보 상에 MaxCount이상일 때 처리 로직이 있어야 하기 때문이며,
                // 씬 이동이나 로드 등을 거치면서 해당 아이템 정보를 가진 오브젝트 그대로 전달해야 하기 때문입니다. 
            }
        }





        /// <summary>
        /// 플레이어가 관리하고 있는 목록의 갯수입니다. 무기, 잡화 종류밖에 없다면 2입니다.
        /// </summary>
        public int dicNum {get;}

        public void AddItem( GameObject itemObject )
        {
            Item item = itemObject.GetComponent<ItemInfo>().item;
            switch(item.Type)
            {
                case ItemType.Weapon:
                    weapList.Add(itemObject);
                    break;
                
                case ItemType.Misc:
                    miscList.Add(itemObject);
                    break;
            }
        }

        /// <summary>
        /// 기본 인벤토리 생성자입니다. 새로운 게임을 시작할 때 사용해주세요.
        /// </summary>
        public Inventory()
        {            
            weapDic = new Dictionary< string, List<GameObject> >();
            miscDic = new Dictionary< string, List<GameObject> >();
            dicNum = 2;

            TotalMaxCount = InitialCount * dicNum;
        }

        /// <summary>
        /// 플레이어 정보가 있는 경우 기존 정보를 바탕으로 인벤토리를 만들어주는 생성자입니다. 
        /// </summary>
        /// <param name="weapItemInfoList"></param>
        /// <param name="miscList"></param>
        /// <param name="InventoryMaxCount"></param>
        public Inventory(List<Item> weapItemInfoList, List<Item> miscList, int InventoryMaxCount )
        {
            this.weapList = weapList;
            this.miscList = miscList;
            this.TotalMaxCount = InventoryMaxCount;
        }
    }
}
