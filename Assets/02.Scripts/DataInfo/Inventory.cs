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
 * => 그 이유는 현재 수량 변경이 일어나면 리스트를 순차적으로 빼서 하나씩 정보를 열어보는 비효율이 발생하고 있기 때문이며,
 * 특히 삭제시 결정된 아이템을 목록에서 삭제하기 위해 매번 리스트를 하나씩 꺼내봐야하면 연산 자원소모가 많이 발생하기 때문
 * 
 * <v3.0 - 2023_1119_최원준>
 * 1- 메서드 이름 변경 - GetItemList -> ConvertDicToItemList
 * 2- 메서드 이름 변경 및 매개변수 변경 - SetObjectList(ItemType, List<Item>) -> ConvertItemListToDic(List<Item>)
 * 3- 프로퍼티 이름 변경 - TotalMaxCount -> AllCountLimit, TotalCurCount->TotalCount
 * 4- InitialCount -> InitialCountLimit으로 변경 및 static 변수에서 프로퍼티로 변경. (메모리 공간 낭비를 막음)
 * 
 * 5- 새로운 멤버변수 추가 - weapCountLimit, miscCountLimit 
 * 인벤토리 개별탭의 칸수에 제한이 있는 방식으로 변경해야 하기 때문
 * 칸수를 change한다고 할 때 전체 칸수를 늘리면 나중에 개별 칸수를 지정하지 못하기 때문이며, 
 * item을 add할 때도 개별 제한이 있어야 현재 칸수와 비교하여 add시킬 수 있으며,
 * 개별 제한 칸 수가 정해지면 TotalCountLimit은 자동으로 지정할 수 있기 때문
 *  
 * 6- 기본 생성자 수정
 * 인벤토리 초기 생성시, 사전초기화와 개별 제한 칸 수 초기화가 이루어짐.
 * 
 * <v4.0 - 2023_1120_최원준>
 * 1- 현 스크립트의 Inventory 클래스를 partial 클래스로 전환
 * 아이템 추가, 검색, 제거 관련 로직을 다음 파일에서 할 예정
 * 2- 슬롯 리스트를 멤버로 추가
 * 3- Inventory 클래스의 TotalCountLimit 변수명을 AllCountLimit으로 변경
 * 4- Inventory 클래스와 SerializableInventory 클래스의 weapCountLimit, miscCountLimit 프로퍼티화 및 변수명 앞머리 대문자로 변경
 * 5- Serializable 클래스의 생성자에  WeapCountLimit, MiscCountLimit의 초기화가 빠져있던 점 수정
 * 
 * <v4.1 - 2023_1122_최원준>
 * 1- 무기, 기타 사전의 주석 보완
 * 
 * <v4.2 - 2023_1221_최원준>
 * 1- 분할클래스인 Inventory_p2.cs 파일 추가
 * AddItem메서드를 추가하였음.
 * 
 * <v5.0 - 2023_1221_최원준>
 * 1- WeapCount와 MiscCount가 서로 miscDic과 weapDic의 Values를 바꿔서 참조하던 점 수정
 * 
 * 2- 새로운 게임을 시작하는 경우 DataManager의 Load메서드로 GameData가 생성은 되지만
 * 디폴트 생성자가 아닌 Inventory( SerializableInventory savedInventory ) 생성자가 호출됨으로 인해
 * 딕셔너리가 초기화되지 않아 null레퍼런스가 발생하였습니다.
 * 이를 없애기 위해 해당 생성자에 딕셔너리의 null값 검사를 추가하여 새로운 딕셔너리를 생성하도록 하였습니다.
 * 
 * <v5.1 - 2023_1224_최원준>
 * 1- 아이템 생성시 ConvertItemListToDic 메서드에서
 * The given key '철' was not present in the dictionary. 오류가 발생하여
 * miscDic[item.Name]==null로 검사하는 구문을 miscDic.ContainsKey(item.Name) 구문으로 변경
 * 
 * <v6.0 - 2023_1226_최원준>
 * 1- SerializableInventory의 weapList, miscList를 Item 타입에서 개별 타입인 ItemWeapon, ItemMisc타입으로 변경
 * 2- ConvertDicToItemList과 ConvertItemListToDic 메서드 개별적으로 사전에 따라 중복로직으로 작성되어있던 것을 일반화메서드로 변경
 * 
 * <v7.0 - 2023_1226_최원준>
 * 1- DeserializeItemListToDic의 디버그 출력용 메서드라인 제거
 * 
 * 2- SerializableInventory의 WeapCountLimit, MiscCountLimit변수 자동구현 프로퍼티가 아닌 public변수로 변경
 * 
 * 3- SerializableInventory의 클래스 명을 SInventory로 변경하고 Serialize메서드와 Deserialize메서드를 구현하였음.
 * 이유는 STransform과 사용에 일관성을 주어 헷깔리지 않게 하기 위함
 * 
 * 4- Inventory클래스의 SerialzableInventory를 인자로 받는 생성자 제거
 * SInventory의 Deserialize메서드에서 새롭게 인벤토리를 생성해서 하나씩 값을 넣어줘서 인벤토리클래스를 반환하게끔 구현
 * 
 * 5- WeapCountLimit, MiscCountLimit 프로퍼티 set기능 추가
 * 
 * <v7.1 - 2023_1227_최원준>
 * 1- Item프리팹의 계층구조 변경으로 인해 SerializeDicToItemList메서드에서 GetComponent로 ItemInfo를 참조하던 것을 GetComponentInChildren으로 변경
 * 
 * 
 */




namespace CraftData
{
    /// <summary>
    /// 인벤토리를 정의하는 클래스, 딕셔너리에 이름참조를 통하여 접근하면
    /// 내부에 GameObject를 저장하는 weapList와 miscList가 있습니다.<br/> 
    /// ******Save, Load시 인벤토리 클래스를 저장해야 합니다.******
    /// </summary>
    [Serializable]
    public partial class Inventory
    {
        /**** 세이브 로드 시 저장해야할 속성들 ****/

        /// <summary>
        /// 플레이어가 보유하고 있는 무기 아이템 목록입니다. 아이템 이름을 통해 해당 아이템 오브젝트를 보관하는 리스트에 접근할 수 있습니다.
        /// </summary>
        public Dictionary< string, List<GameObject> > weapDic;

        /// <summary>
        /// 플레이어가 보유하고 있는 잡화 아이템 목록 입니다. 아이템 이름을 통해 해당 아이템 오브젝트를 보관하는 리스트에 접근할 수 있습니다.
        /// </summary>
        public Dictionary< string, List<GameObject> > miscDic;
                        
        /// <summary>
        /// 플레이어 인벤토리의 무기 탭의 칸의 제한 수 입니다. 게임 중에 업그레이드 등으로 인해 변동될 수 있습니다.
        /// </summary>
        public int WeapCountLimit { get; set; }

        /// <summary>
        /// 플레이어 인벤토리의 잡화 탭의 칸의 제한 수 입니다. 게임 중에 업그레이드 등으로 인해 변동될 수 있습니다.
        /// </summary>
        public int MiscCountLimit { get; set; }
        


        

        /**** 자동으로 지정되는 속성들 ****/

        /// <summary>
        /// 플레이어 인벤토리의 전체 탭의 칸의 제한 수 입니다. 게임 중에 업그레이드 등으로 인해 변동될 수 있습니다.
        /// </summary>
        public int AllCountLimit { get{return WeapCountLimit+MiscCountLimit; } }


        /// <summary>
        /// 플레이어 인벤토리의 각 탭에 공통으로 주어지는 초기 칸의 제한 수입니다.
        /// </summary>
        public int InitialCountLimit { get{ return 50; } }
                
        
        /// <summary>
        /// 플레이어 인벤토리에 종류와 상관없이 모든 아이템이 차지하고 있는 현재 칸 수입니다.
        /// </summary>
        public int TotalCount { get{ return WeapCount + MiscCount; } }

        /// <summary>
        /// 무기 아이템이 인벤토리에 차지하고 있는 칸의 총 갯수입니다.
        /// </summary>
        public int WeapCount { 
            get{ 
                int count = 0;
                foreach(List<GameObject> objList in weapDic.Values)
                    count += objList.Count;    
                // 딕셔너리에서 게임오브젝트 리스트를 꺼내고 리스트의 Count 프로퍼티를 통해 게임오브젝트 숫자를 누적시킵니다.
                return count; } }
        
        /// <summary>
        /// 잡화 아이템이 인벤토리에 차지하고 있는 칸의 총 갯수입니다.
        /// </summary>
        public int MiscCount { get{
                int count = 0;
                foreach(List<GameObject> objList in miscDic.Values)
                    count += objList.Count;    
                // 딕셔너리에서 게임오브젝트 리스트를 꺼내고 리스트의 Count 프로퍼티를 통해 게임오브젝트 숫자를 누적시킵니다.
                return count; } }


        /// <summary>
        /// 현재 인벤토리가 보관하고 있는 사전 중에서 원하는 종류의 아이템 인스턴스 리스트를 만들어 줍니다.<br/>
        /// 오브젝트를 제외한 아이템의 순수한 정보만 읽어들일 필요가 있을 때,<br/>
        /// 게임 오브젝트 리스트를 직렬화 할 때, 씬을 넘어가기 전에 전달해야하는 용도로 사용합니다.
        /// </summary>
        /// <param name="itemType"> GameObject타입에서 Item타입으로 리스트화 할 사전목록의 종류를 정해주세요. Weapon, Misc등이 있습니다. </param>
        public List<T> SerializeDicToItemList<T>() where T : Item
        {
            List<T> itemList = new List<T>();       // 새로운 T형식 아이템리스트 생성
            Dictionary<string, List<GameObject>> itemDic = null;

            if( typeof(T)==typeof(ItemWeapon) )     // 무기 종류라면 인벤토리의 무기 사전 참조
                    itemDic = weapDic;          
            else if( typeof(T)==typeof(ItemMisc) )  // 잡화 종류라면 인벤토리의 잡화 사전 참조
                itemDic = miscDic;              
              

            foreach( List<GameObject> objList in itemDic.Values )                // 해당 사전에서 게임오브젝트 리스트를 하나씩 꺼내어
            {
                for(int i=0; i<objList.Count; i++)                               // 리스트의 게임오브젝트를 모두 가져옵니다.
                    itemList.Add( (T)objList[i].GetComponentInChildren<ItemInfo>().Item ); // item 스크립트를 하나씩 꺼내어 T형식으로 저장합니다.
            }

            return itemList;                                                     // item 정보가 하나씩 저장되어있는 itemList를 반환합니다.
        }

        /// <summary>
        /// 종류 별 아이템 정보가 담겨있는 리스트를 집어넣으면 해당 딕셔너리로 변환시켜 줍니다.<br/>
        /// 로드 할때나 씬을 전환했을 때 아이템 리스트를 역직렬화 하여 다시 인벤토리 목록으로 변환해야하는 용도로 사용합니다.
        /// </summary>
        /// <param name="itemType">해당 itemList의 item의 종류</param>
        /// <param name="itemList">딕셔너리로 변환 시킬 item정보가 보관되어 있는 List</param>
        public void DeserializeItemListToDic<T>( List<T> itemList ) where T : Item
        {
            Dictionary<string, List<GameObject>> itemDic = null;    // 어떤 딕셔너리를 참조할 것인지 선언

            
            if( typeof(T)==typeof(ItemWeapon))
                itemDic = weapDic;
            else if( typeof(T)==typeof(ItemMisc) )
                itemDic = miscDic;
                        
            foreach(Item item in itemList) // 아이템 리스트에서 개념 아이템 정보를 하나씩 꺼내옵니다.
            {
                // 아이템 정보를 바탕으로 하는 게임오브젝트를 만듭니다.
                GameObject itemObject = CreateManager.instance.CreateItemByInfo( item );
                        
                if( !itemDic.ContainsKey(item.Name) )       // 사전에 해당하는 아이템의 이름이 없다면
                {
                    // 새로운 리스트를 만들어 오브젝트를 넣습니다.
                    List<GameObject> itemObjList = new List<GameObject> { itemObject };          
                    
                    itemDic.Add(item.Name, itemObjList );   // 인벤토리 사전에 아이템 이름으로 오브젝트 리스트를 추가합니다.
                }
                else                                        // 사전에 해당하는 아이템의 이름이 있다면
                {
                    itemDic[item.Name].Add(itemObject);     // 인벤토리 사전에 접근하여, 해당 리스트에 오브젝트만 추가합니다.
                }

            }
            
            

            // 참고) 잡화아이템의 최대 수량 검사를 따로 하지 않습니다.
            // 아이템 정보 상에 MaxCount이상일 때 예외 처리 로직이 있으며, Inventory에서 미리 MaxCount일 때의 새로운 오브젝트 생성 등을 처리했다고 판단합니다.
            // 씬 이동이나 로드 등을 거치면서 해당 아이템 정보를 가진 오브젝트 그대로 전달해야 합니다.
        }



        /// <summary>
        /// 기본 인벤토리 생성자입니다. 새로운 게임을 시작할 때 사용해주세요.
        /// </summary>
        public Inventory()
        {
            weapDic=new Dictionary<string, List<GameObject>>();
            miscDic=new Dictionary<string, List<GameObject>>();
            WeapCountLimit=InitialCountLimit;
            MiscCountLimit=InitialCountLimit;
        }
               
    }




    /// <summary>
    /// 직렬화 가능한 인벤토리 클래스입니다.<br/>
    /// 기존의 인벤토리에서 GameObject 형식의 리스트를 Item 형식의 리스트로 변환하여 보관합니다.<br/>
    /// </summary>
    [Serializable]
    public class SInventory
    {
        public List<ItemWeapon> weapList;
        public List<ItemMisc> miscList;
        public int WeapCountLimit;
        public int MiscCountLimit;
        
        /// <summary>
        /// SerializableInventory의 디폴트 생성자입니다. 직렬화 가능한 인벤토리 인스턴스를 생성해 줍니다.
        /// </summary>
        public SInventory()
        {
            Serialize(new Inventory());
        }


        /// <summary>
        /// 기존의 인벤토리 클래스의 인스턴스를 인자로 받아서 직렬화 가능한 인벤토리 인스턴스를 생성해 줍니다.
        /// </summary>
        public SInventory( Inventory inventory )
        {
            Serialize(inventory);
        }

         /// <summary>
        /// 저장 되어있는 인벤토리를 역직렬화하여 로드하는 메서드입니다.<br/>
        /// 로드되어있는 내부변수를 자동으로 변환하여 Inventory 인스턴스를 반환받습니다.
        /// </summary>
        public Inventory Deserialize()
        {
            Inventory inventory = new Inventory();
                      
            inventory.DeserializeItemListToDic<ItemWeapon>( this.weapList );
            inventory.DeserializeItemListToDic<ItemMisc>( this.miscList );
            inventory.WeapCountLimit = this.WeapCountLimit;
            inventory.MiscCountLimit = this.MiscCountLimit;

            return inventory;
        }

        /// <summary>
        /// 인벤토리를 직렬화하여 저장하는 메서드입니다. 인자로 저장할 인벤토리를 전달하여야 합니다.
        /// </summary>
        public void Serialize(Inventory inventory)
        {
            this.weapList=inventory.SerializeDicToItemList<ItemWeapon>();
            this.miscList=inventory.SerializeDicToItemList<ItemMisc>();
            this.WeapCountLimit = inventory.WeapCountLimit;
            this.MiscCountLimit = inventory.MiscCountLimit;
        }




    }

}
