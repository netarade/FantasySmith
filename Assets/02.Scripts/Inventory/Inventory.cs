using ItemData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
 * <v7.2 - 2023_1229_최원준>
 * 1- 네임스페이스명 CraftData에서 InventoryManagement로 변경
 * 2- SInventory클래스 SaveData_Inventory로 이동
 * 
 * <v7.3 - 2023_1230_최원준>
 * 1- 내부 멤버변수인 딕셔너리를 private처리
 * 이유는 라이브러리 사용자가 인벤토리를 직접 수정 시 실수를 방지하기 위함.
 * 
 * <v8.0 - 2023_1231_최원준>
 * 1- WeapCountLimit,MiscCountLimit을 -> WeapSlotCountLimit, MiscSlotCountLimit으로 이름 변경
 * 슬롯 제한 수 (아이템 칸 제한 수)라는 의미를 분명하게 하였음
 * 
 * 2- curWeapItemCount, curMiscItemCount 변수 추가
 * 현재 들어있는 오브젝트 갯수를 저장하는 변수로 설정
 * 
 * 3- TotalCount를 TotalCurItemCount로 변경 
 * curWeapItemCount, curMiscItemCount 기준으로 계산
 * 
 * 4- 기존의 WeapCount, MiscCount 프로퍼티를 삭제하고 GetCurItemCount, SetCurItemCount, GetItemCountLimit 메서드를 추가
 * 
 * 로드 시 curWeapItemCount과 curMiscItemCount를 초기화 하거나,
 * AddItem, RemoveItem에서 내부적으로 아이템 카운트의 변동이 있을 때 이용하도록 구성
 *  
 * 
 * 5- weapDic, miscDic에 JsonProperty를 붙여놓았던 것 삭제
 * 
 * 6- DeserializeItemListToDic 간소화,
 * Inventory_2에 정의되어있는 기본 메서드를 기반으로 로직구성
 * 
 * <v8.1 - 2023_1231_최원준>
 * 1- 슬롯 인덱스를 임시로 담기 위해 indexList 변수를 선언
 * 2- 변수명 변경 
 * AllCountLimit -> SlotCountLimitAll
 * WeapSlotCountLimit -> SlotCountLimitWeap
 * MiscSlotCountLimit -> SlotCountLimitMisc
 * 
 * 3- GetCurItemCountLimit메서드 제거, GetItemSlotCountLimit로 새롭게 만들어서,
 * 인자를 미 전달 시 전체 슬롯의 최대 제한 수를 반환하도록 구현하였고,
 * 인자를 전달 시 ItemType에 따라서 최대 슬롯 제한 수를 반환하도록 구현.
 * 
 * 4- GetCurItemCount메서드 내부 딕셔너리 count==0인지 확인하는 코드 삽입
 * 
 * 
 * <v8.2 - 2024_0102_최원준>
 * 1- 딕셔너리의 길이를 반환하는 프로퍼티 추가
 * 2- CurItemCount, CurItemWeapCout, CurItemMiscCount를 CurItemObjCount로 이름변경
 * 
 * 
 * [이슈_0102]
 * 1- 딕셔너리 변수명을 일반화 시킬 수 있어야함.
 * 2- curDicLen 프로퍼티는 현재 딕셔너리가 몇개인지 여부를 반환해야 하는데 이를 위해 enum변수를 따로 가지고 있어야 한다.
 * 
 * 
 * 
 * 
 * 
 * <v8.3 - 2024_0103_최원준>
 * 1- GetCurItem메서드 예외처리문 추가
 * 
 * 
 * <v8.4 - 2024_0104_최원준>
 * 1- 현재 남아있는 슬롯의 갯수를 반환하는 GetCurRemainSlotCount 메서드 추가
 * AddItem과 IsAbleToAddMisc메서드 등에서 아이템을 추가할 공간이 있나 살펴보기 위해 사용
 * 
 * 
 */




namespace InventoryManagement
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
        /// 플레이어 인벤토리의 무기 탭의 슬롯 칸 제한 수 입니다. 게임 중에 업그레이드 등으로 인해 변동될 수 있습니다.
        /// </summary>
        public int SlotCountLimitWeap { get; set; }

        /// <summary>
        /// 플레이어 인벤토리의 잡화 탭의 슬롯 칸 제한 수 입니다. 게임 중에 업그레이드 등으로 인해 변동될 수 있습니다.
        /// </summary>
        public int SlotCountLimitMisc { get; set; }
        
        


        /**** 로드 시에만 불러온 후 게임 중에 지속적으로 관리 및 참조해야 할 속성 ****/            

        /// <summary>
        /// 현재 인벤토리에 들어있는 무기 아이템 오브젝트의 갯수를 반환받거나 설정합니다.
        /// </summary>
        public int CurWeapItemObjCount {get; set;} = 0;  // 새로운 인벤토리 생성 시 0으로 초기화
        
        /// <summary>
        /// 현재 인벤토리에 들어있는 잡화 아이템 오브젝트의 갯수를 반환받거나 설정합니다.
        /// </summary>
        public int CurMiscItemObjCount {get; set;} = 0;  // 새로운 인벤토리 생성 시 0으로 초기화
                        



        /**** 자동으로 지정되는 고정 속성들 ****/
        
        /// <summary>
        /// 플레이어 인벤토리의 각 탭에 공통으로 주어지는 초기 칸의 제한 수입니다.
        /// </summary>
        public int InitialCountLimit { get{ return 50; } }        

        /// <summary>
        /// 플레이어 인벤토리의 전체 탭의 칸의 제한 수 입니다. 게임 중에 업그레이드 등으로 인해 변동될 수 있습니다.
        /// </summary>
        public int SlotCountLimitAll { get{return SlotCountLimitWeap+SlotCountLimitMisc; } }        
        
        /// <summary>
        /// 플레이어 인벤토리에 종류와 상관없이 모든 아이템이 차지하고 있는 현재 칸 수입니다.
        /// </summary>
        public int TotalCurItemCount { get{ return CurWeapItemObjCount + CurMiscItemObjCount; } }
        
        /// <summary>
        /// 현재 인벤토리의 딕셔너리 길이를 반환합니다.
        /// </summary>
        public int CurDicLen { get { return (int)ItemType.None-1;} } //전체 길이이므로 개별 종류를 가지는 인벤토리에서는 적용되지 않으므로 수정필요

        
        /**** 내부적으로만 사용하는 속성 ****/
        List<int> indexList = new List<int>();





        
        
        /// <summary>
        /// 현재 인벤토리에 들어있는 아이템 오브젝트의 갯수를 연산하여 반환합니다.<br/>
        /// 초기 로드 시 인벤토리에 로드 한 오브젝트를 넣은 후 CurItemCount를 연산하기 위해 필요합니다.<br/><br/>
        /// 인자로 ItemType을 전달하여 해당 ItemType의 딕셔너리에서 오브젝트 갯수를 카운팅합니다.<br/><br/>
        /// *** 해당 종류의 아이템 사전이 없으면 예외를 발생시킵니다. ***
        /// </summary>
        /// <returns>아이템 타입과 일치하는 인벤토리에 저장 되어있는 오브젝트의 갯수를 반환합니다.</returns>
        public int GetCurItemCount(ItemType itemType) 
        { 
            int count = 0;

            // 아이템 타입을 기반으로 딕셔너리를 구합니다.
            Dictionary<string, List<GameObject>> itemDic = GetItemDicIgnoreExsists(itemType);

            if(itemDic==null)
                throw new Exception("해당 종류의 아이템 사전이 존재하지 않습니다.");
            
            // 해당 인벤토리 딕셔너리에 들어있는 리스트가 하나도 없다면, 바로 0을 반환합니다.
            if(itemDic.Count==0)        
                return 0;

            // 해당 딕셔너리에서 게임오브젝트 리스트를 꺼내고 리스트의 Count 프로퍼티를 통해 게임오브젝트 숫자를 누적시킵니다.
            foreach(List<GameObject> objList in itemDic.Values)
                count += objList.Count;   
            
            return count;  
        }
        
        /// <summary>
        /// ItemType을 기반으로 어떤 종류의 curItemCount를 증가 또는 감소 시킬 지 결정하는 메서드입니다.<br/>
        /// inCount를 음수로 전달하면 아이템의 현재 수량을 감소시킵니다.<br/>
        /// AddItem메서드에서 내부적으로 사용되고 있습니다.<br/><br/>
        /// 
        /// *** 인자로 해당 인벤토리에 해당하지 않는 ItemType을 전달한 경우 예외를 발생시킵니다. ***
        /// </summary>
        public void SetCurItemObjCount(ItemType itemType, int inCount)
        {
            switch(itemType)
            {
                case ItemType.Weapon:
                    CurWeapItemObjCount += inCount;
                    break;

                case ItemType.Misc:
                    CurMiscItemObjCount += inCount;
                    break;

                default:
                    throw new Exception("인자로 전달 한 아이템 종류가 해당 인벤토리에 맞지 않습니다. 확인하여 주세요.");
            }
        }

        /// <summary>
        /// 아이템 종류에 따른 슬롯의 제한 수를 반환합니다. <br/>
        /// ItemType을 인자로 전달받습니다.<br/>
        /// *** 인자를 미 전달 또는 ItemType.None 전달 시 전체 슬롯 인덱스를 기준으로 반환합니다.(기본 값) *** <br/>
        /// </summary>
        /// <returns>인자로 전달 된 슬롯의 최대 제한 수를 반환합니다</returns>
        public int GetItemSlotCountLimit(ItemType itemType=ItemType.None)
        {
            int slotCountLimit;
                        
            switch(itemType)
            { 
                case ItemType.Weapon:
                    slotCountLimit = SlotCountLimitWeap; 
                    break;

                case ItemType.Misc:
                    slotCountLimit = SlotCountLimitMisc;
                    break;

                default:
                    slotCountLimit = SlotCountLimitAll;
                    break;
            }            

            return slotCountLimit;
        }


        /// <summary>
        /// 현재 인벤토리에 들어있는 아이템 오브젝트의 전체 갯수를 연산하여 반환합니다
        /// </summary>
        /// <returns>현재 인벤토리에 존재하는 모든 아이템의 총 갯수입니다.</returns>
        private int GetCurItemCountAll()
        {
            int count = 0;
            int dicLen = (int)ItemType.None;

            for(int i=0; i<dicLen; i++)
                count += GetCurItemCount((ItemType)i);

            return count;
        }

        /// <summary>
        /// 현재 인벤토리의 남아있는 슬롯 갯수를 반환합니다.<br/>
        /// 아이템 종류를 전달해야 합니다.<br/>
        /// *** 아이템의 종류에 해당하는 사전이 없다면 예외를 발생시킵니다. ***
        /// </summary>
        /// <returns>남아있는 슬롯이 없으면 0, 남아있는 슬롯이 있다면 해당 슬롯의 갯수</returns>
        public int GetCurRemainSlotCount(ItemType itemType)
        {
            return GetItemSlotCountLimit(itemType) - GetCurItemCount(itemType);
        }




        /// <summary>
        /// 현재 인벤토리가 보관하고 있는 사전 중에서 원하는 종류의 아이템 인스턴스 리스트를 만들어 줍니다.<br/>
        /// 오브젝트를 제외한 아이템의 순수한 정보만 읽어들일 필요가 있을 때,<br/>
        /// 게임 오브젝트 리스트를 직렬화 할 때, 씬을 넘어가기 전에 전달해야하는 용도로 사용합니다.
        /// </summary>
        /// <param name="itemType"> GameObject타입에서 Item타입으로 리스트화 할 사전목록의 종류를 정해주세요. Weapon, Misc등이 있습니다. </param>
        public List<T> SerializeDicToItemList<T>() where T : Item
        {
            List<T> itemList = new List<T>();       // 새로운 T형식 아이템리스트 생성
            

            // 현재 인벤토리에서 어떤 사전을 참조 할 것인지 T를 바탕으로 결정합니다.
            Dictionary<string, List<GameObject>> itemDic = null;

            if( typeof(T)==typeof(ItemWeapon) )     // 무기 종류라면 인벤토리의 무기 사전 참조
                itemDic = weapDic;          
            else if( typeof(T)==typeof(ItemMisc) )  // 잡화 종류라면 인벤토리의 잡화 사전 참조
                itemDic = miscDic;              
              

            // 해당 사전의 아이템을 하나씩 불러와서 하위 클래스 형식(T형식) 으로 저장합니다.
            foreach( List<GameObject> objList in itemDic.Values )                // 해당 사전에서 게임오브젝트 리스트를 하나씩 꺼내어
            {
                for(int i=0; i<objList.Count; i++)                               // 리스트의 게임오브젝트를 모두 가져옵니다.
                    itemList.Add( (T)objList[i].GetComponentInChildren<ItemInfo>().Item ); // item 스크립트를 하나씩 꺼내어 T형식으로 저장합니다.
            }

            // item 정보가 하나씩 저장되어있는 itemList를 반환합니다.
            return itemList;                                                     
        }

        /// <summary>
        /// 종류 별 아이템 정보가 담겨있는 리스트를 집어넣으면 해당 딕셔너리로 변환시켜 줍니다.<br/>
        /// 로드 할때나 씬을 전환했을 때 아이템 리스트를 역직렬화 하여 다시 인벤토리 목록으로 변환해야하는 용도로 사용합니다.
        /// </summary>
        public void DeserializeItemListToDic<T>( List<T> itemList ) where T : Item
        {
            foreach(Item item in itemList) // 아이템 리스트에서 개념 아이템 정보를 하나씩 꺼내옵니다.
            {                        
                // item정보를 기반으로 아이템을 새롭게 생성하여 인벤토리에 넣어줍니다. (Invnetory_p2에 정의되어 있습니다.)
                AddItem(item);
            }
        }



        /// <summary>
        /// 기본 인벤토리 생성자입니다. 새로운 게임을 시작할 때 사용해주세요.
        /// </summary>
        public Inventory()
        {
            weapDic=new Dictionary<string, List<GameObject>>();
            miscDic=new Dictionary<string, List<GameObject>>();
            SlotCountLimitWeap=InitialCountLimit;
            SlotCountLimitMisc=InitialCountLimit;
        }
               
    }




    

}
