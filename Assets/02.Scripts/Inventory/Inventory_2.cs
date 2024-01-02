using ItemData;
using System;
using System.Collections.Generic;
using UnityEngine;
using WorldItemData;

/*
 * [작업 사항]  
 * <v1.0 - 2023_1121_최원준>
 * 1- 초기 분할 클래스 정의
 * AddItem메서드 추가 - CreateManager의 CreateItemToNearstSlot메서드 호출 기능
 * 
 * <v1.1 -2023_1121_최원준>
 * 1- 분할클래스를 만들면서 MonoBehaviour를 지우지 않아 new 키워드 경고가 발생하여 제거.
 * 
 * <v1.2 - 2023_1122_최원준>
 * 1- 인벤토리가 로드되었을 때 아이템 오브젝트가 원위치를 시킬 수 있도록 UpdateAllItem메서드 추가
 * 
 * <v2.0 - 2023_1226_최원준>
 * 1- UpdateAllItem메서드를 UpdateAllItemInfo메서드로 변경
 * 2- LoadAllItem 주석처리 
 * - 개별 종류로 직렬화했을 때 잘 불러와진다면 해당 메서드 삭제 예정
 * 
 * <v2.1 - 2023_1227_최원준>
 * 1- Item 프리팹 계층구조 변경으로 인해(3D 오브젝트 하위 2D오브젝트) 
 * UpdateAllItemInfo메서드의 ItemInfo를 참조하는 GetComponent메서드를 GetComponentInChildren으로 변경
 * 
 * <v2.2 - 2023_1229_최원준>
 * 1- 네임스페이스명 CraftData에서 InventoryManagement로 변경
 * 2- 파일명 변경 Inventory_p2-> Inventory_2
 * 
 * <v2.3 - 2023_1230_최원준>
 * 1- CreateItem메서드 추가
 * AddItem메서드의 내용을 옮김.
 * 
 * <v3.0 - 2023_1231_최원준>
 * 1- CreateItem메서드 삭제 하고 AddItem메서드에서 ItemInfo 또는 GameObject를 인자로 전달받아서 해당 딕셔너리에 넣도록 구현
 * 
 * 2- AddItem, RemoveItem, SetOverlapCount 인자별 오버로딩 정의
 * 
 * 3- 다양한 참조를 돕는 메서드 정의
 * IsContainsItemName - 아이템 이름으로 해당 인벤토리에 존재하는지 
 * 
 * GetItemDicInExists - 현재 인벤토리 목록에 이미 존재하는 아이템을 담을 딕셔너리를 반환
 * GetItemDicIgnoreExists - 현재 인벤토리에 목록에 존재하지 않는 아이템을 담을 딕셔너리를 반환
 * 
 * GetItemTypeInExists - 딕셔너리에 존재하는 아이템의 타입반환
 * GetItemTypeIgnoreExists - 월드 딕셔너리에 존재하는 아이템의 타입 반환
 * GetItemObjectList - 딕셔너리에서 오브젝트 리스트를 반환
 * 
 * 
 * <v3.1 - 2023_0102_최원준>
 * 1- AddItem메서드 string, Item클래스 기반 오버로딩 메서드 삭제
 * 이유는 미리 생성되어있는 오브젝트가 아닌 경우 포지션 정보 업데이트 메서드 호출이 곤란하기 때문에
 * 결국 InventoryInfo메서드에서 완성해야 할 기능이므로 이미 생성되어있는 오브젝트나 스크립트를 넣는 메서드만 있는것이 낫다고 판단.
 * 
 * (Remove메서드의 오버로딩을 놔두는 이유는 string기반으로 검색이 메인이기 때문에, 나머지 오버로딩은 Info클래스에서 그대로 재사용 가능)
 * 
 * 2- AddItem GameObject인자를 받는 메서드 예외처리 작성
 * 
 * 3- SetOverlapCount메서드 삭제 및 관련 기능 Inventory_3.cs로 구현
 * 
 * 4- AddItem 및 RemoveItem에 예외처리 문장 추가
 * 5- RemoveItem GameObject와 ItemInfo 인자 메서드를 Item인자 메서드 호출에서 바로 string 메서드로 호출되도록 변경 
 * 
 * 
 * 
 * [추가 사항]
 * 1- AddItem 메서드로 아이템을 넣지만 포지션 업데이트를 외부 InventoryInfo스크립트에서 해줘야 함.
 * (포지션 업데이트를 현재활성화 탭기준으로 해놔야 한다.)
 * 
 * 2- AddItem 메서드로 인벤토리에 넣을 때, 개별 슬롯 인덱스와 전체 슬롯 인덱스를 둘다 찾아서 넣어줘야 한다.
 * (CreateManager도 수정필요)
 */

namespace InventoryManagement
{    
    public partial class Inventory
    {
        /// <summary>
        /// 아이템 오브젝트를 인자로 전달받아 해당하는 사전을 찾아서 넣어주는 메서드입니다.<br/><br/>
        /// *** 아이템 오브젝트를 인자로 전달한 경우 이 오브젝트를 인벤토리에 넣어줍니다. *** 
        /// </summary>
        /// <returns>아이템 추가 성공시 true를, 현재 인벤토리에 들어갈 공간이 부족하다면 false를 반환합니다</returns>
        public bool AddItem(GameObject itemObj)
        {
            if(itemObj == null)
                throw new Exception("존재하지 않는 참조값입니다. 확인하여 주세요.");

            ItemInfo itemInfo = itemObj.GetComponent<ItemInfo>();

            if(itemInfo==null)
                throw new Exception("아이템 스크립트가 존재하지 않는 오브젝트입니다. 확인하여 주세요.");

            return AddItem(itemInfo); 
        }


        /// <summary>
        /// ItemInfo 컴포넌트를 인자로 전달받아 해당 아이템 오브젝트에 해당하는 사전을 찾아서 넣어주는 메서드입니다.<br/><br/>
        /// *** ItemInfo 컴포넌트를 인자로 전달한 경우 해당 컴포넌트가 부착되어있는 실제 오브젝트를 인벤토리에 넣어줍니다. *** 
        /// </summary>
        /// <returns>아이템 추가 성공시 true를, 현재 인벤토리에 들어갈 공간이 부족하다면 false를 반환합니다</returns>
        public bool AddItem(ItemInfo itemInfo)
        {   
            if(itemInfo==null)
                throw new Exception("아이템 스크립트가 존재하지 않는 오브젝트입니다. 확인하여 주세요.");

            //아이템 타입을 확인합니다.
            ItemType itemType = itemInfo.Item.Type;

            // 현재 오브젝트의 갯수가 슬롯 칸 제한 수와 같거나 크다면 더 이상 추가할 수 없으므로 false를 반환합니다.
            if(GetCurItemCount(itemType) >= GetItemSlotCountLimit(itemType) )            
                return false;        


            // 아이템 타입을 기반으로 딕셔너리를 결정합니다.
            Dictionary<string, List<GameObject>> itemDic = GetItemDicIgnoreExsists(itemType);

            AddItemToDic(itemDic, itemInfo);    // 찾은 딕셔너리에 itemInfo 컴포넌트 참조값을 전달하여 오브젝트를 추가합니다.
            SetCurItemObjCount(itemType, 1);       // 해당 아이템 종류의 현재 오브젝트의 갯수를 증가시킵니다.
            return true;                        // 성공을 반환합니다.
        }
        
        
        /// <summary>
        /// 아이템 넣기를 원하는 사전과 ItemInfo 컴포넌트를 인자로 전달받아 해당 사전에 아이템 오브젝트를 넣어주는 메서드입니다.
        /// </summary>
        private void AddItemToDic(Dictionary<string, List<GameObject>> itemDic, ItemInfo itemInfo)
        {
            string itemName = itemInfo.Item.Name;

            if( !itemDic.ContainsKey(itemName) )        // 해당 사전에 오브젝트 리스트가 존재하지 않는 경우
            {
                List<GameObject> itemObjList = new List<GameObject>();  // 오브젝트 리스트를 새로 만듭니다
                itemObjList.Add(itemInfo.gameObject);                   // 오브젝트 리스트에 아이템 오브젝트를 추가합니다
                itemDic.Add(itemName, itemObjList);                     // 사전에 오브젝트 리스트를 집어넣습니다.
            }
            else                                            // 해당 사전에 오브젝트 리스트가 존재하는 경우 
            {
                itemDic[itemName].Add(itemInfo.gameObject); // 오브젝트 리스트에 접근하여 게임오브젝트를 넣습니다.
            }            
        }






        /// <summary>
        /// 아이템 오브젝트를 인자로 받아서 인벤토리의 딕셔너리 목록에서 제거해주는 메서드 입니다. <br/>
        /// 인자를 통해 최신순으로 제거 할 것인지, 오래된 순으로 제거할 것인지를 결정할 수 있습니다. 기본은 최신순입니다.
        /// </summary>
        /// <returns>딕셔너리 목록의 제거에 성공한 경우 true를, 실패한 경우 false를 반환합니다.</returns>
        public bool RemoveItem(GameObject itemObj, bool isLatest=true)
        {
            if(itemObj == null)
                throw new Exception("존재하지 않는 참조값입니다. 확인하여 주세요.");

            ItemInfo itemInfo = itemObj.GetComponent<ItemInfo>();   
                                   
            if(itemInfo==null)
                throw new Exception("아이템 스크립트가 존재하지 않는 오브젝트입니다. 확인하여 주세요.");

            return RemoveItem(itemInfo.Item.Name, isLatest);
        }
        
        /// <summary>
        /// ItemInfo 컴포넌트를 인자로 받아서 인벤토리의 딕셔너리 목록에서 제거해주는 메서드 입니다.<br/>
        /// 인자를 통해 최신순으로 제거 할 것인지, 오래된 순으로 제거할 것인지를 결정할 수 있습니다. 기본은 최신순입니다. 
        /// </summary>
        /// <returns>딕셔너리 목록의 제거에 성공한 경우 true를, 실패한 경우 false를 반환합니다.</returns>
        public bool RemoveItem(ItemInfo itemInfo, bool isLatest=true)
        {
            if(itemInfo==null)
                throw new Exception("아이템 스크립트가 존재하지 않는 오브젝트입니다. 확인하여 주세요.");

            Item item = itemInfo.Item;          
            return RemoveItem(itemInfo.Item.Name, isLatest);
        }
        
        /// <summary>
        /// Item 인스턴스를 인자로 받아서 인벤토리의 딕셔너리 목록에서 제거해주는 메서드 입니다.<br/>
        /// 인자를 통해 최신순으로 제거 할 것인지, 오래된 순으로 제거할 것인지를 결정할 수 있습니다. 기본은 최신순입니다. 
        /// </summary>
        /// <returns>딕셔너리 목록의 제거에 성공한 경우 true를, 목록에 없는 아이템인 경우 false를 반환합니다.</returns>
        public bool RemoveItem(Item item, bool isLatest=true)
        {
            string itemName = item.Name;
            return RemoveItem(itemName, isLatest);
        }

        /// <summary>
        /// 아이템의 이름을 인자로 받아서 인벤토리의 딕셔너리 목록에서 제거해주는 메서드 입니다.<br/>
        /// 인자를 통해 최신순으로 제거 할 것인지, 오래된 순으로 제거할 것인지를 결정할 수 있습니다. 기본은 최신순입니다. 
        /// </summary>
        /// <returns>딕셔너리 목록의 제거에 성공한 경우 true를, 목록에 없는 아이템인 경우 false를 반환합니다.</returns>
        public bool RemoveItem(string itemName, bool isLatest=true)
        {
            // 이름을 통해 현재 아이템이 담긴 딕셔너리가 있는지 조사합니다
            Dictionary<string, List<GameObject>> itemDic = GetItemDicInExists(itemName);    

            // 딕셔너리가 존재하지 않는다면, 실패를 반환합니다
            if(itemDic==null)       
                return false;       

            // 딕셔너리에서 오브젝트 리스트를 받습니다
            List<GameObject> itemObjList = itemDic[itemName];

            // 해당 오브젝트 리스트를 참고하여 아이템의 타입을 미리 얻습니다
            ItemType itemType = itemObjList[0].GetComponent<ItemInfo>().Item.Type;

            if(isLatest)     
                itemObjList.RemoveAt(itemObjList.Count-1);  // 최신순으로 제거합니다  
            else
                itemObjList.RemoveAt(0);                    // 오래된순으로 제거합니다
            
            // 해당 아이템 종류의 현재 오브젝트의 갯수를 감소시킵니다.
            SetCurItemObjCount(itemType, -1);       

            // 성공을 반환합니다
            return true;            
        }








        
        /// <summary>
        /// 해당 아이템이름을 기반으로 아이템이 인벤토리의 딕셔너리 목록에서 존재하는지 여부를 반환합니다
        /// </summary>
        /// <returns>해당하는 이름의 아이템이 인벤토리 목록에 존재하는 경우 true, 존재하지 않는 경우 false를 반환합니다</returns>
        public bool IsContainsItemName(string itemName)
        {
            if(weapDic.ContainsKey(itemName))
                return true;
            else if(miscDic.ContainsKey(itemName))
                return true;
            else
                return false;
        }


        /// <summary>
        /// 아이템 이름을 기반으로 해당 아이템의 ItemType을 반환합니다<br/>
        /// 아이템이 딕셔너리 내부에 존재하여야 합니다.
        /// </summary>
        /// <returns>해당하는 이름의 아이템이 현재 인벤토리 목록에 존재하지 않는 경우 ItemType.None을 반환합니다</returns>
        public ItemType GetItemTypeInExists(string itemName)
        {
            if(weapDic.ContainsKey(itemName))
                return ItemType.Weapon;
            else if(miscDic.ContainsKey(itemName))
                return ItemType.Misc;
            else
                return ItemType.None;
        }

        /// <summary>
        /// 아이템 이름을 기반으로 해당 아이템의 ItemType 값을 반환합니다<br/>
        /// 아이템이 현재 인벤토리의 목록에 존재하지 않아도 ItemType 값을 얻을 수 있습니다.<br/>
        /// </summary>
        /// <returns>*** 해당하는 이름의 아이템이 인벤토리 목록에 존재하지 않는 경우 예외를 던집니다. ***</returns>
        public ItemType GetItemTypeIgnoreExists(string itemName)
        {
            WorldItem worldItem = new WorldItem();       // 추후 GetComponent기반으로 변경예정

            if(worldItem.weapDic.ContainsKey(itemName))
                return ItemType.Weapon;
            else if(worldItem.miscDic.ContainsKey(itemName))
                return ItemType.Misc;
            else
                throw new Exception("해당하는 아이템이 월드 사전 목록에 존재하지 않습니다. 아이템의 이름을 확인하여 주세요.");
        }


        

        /// <summary>
        /// 아이템 이름을 기반으로 해당 아이템의 딕셔너리 참조값을 반환합니다<br/>
        /// 아이템이 딕셔너리 내부에 존재하여야 합니다.
        /// </summary>
        /// <returns>해당하는 이름의 아이템이 인벤토리 목록에 존재하지 않는 경우 null을 반환합니다</returns>
        public Dictionary<string, List<GameObject>> GetItemDicInExists(string itemName)
        {
            if(weapDic.ContainsKey(itemName))
                return weapDic;
            else if(miscDic.ContainsKey(itemName))
                return miscDic;
            else
                return null;
        }

        /// <summary>
        /// 아이템 이름을 기반으로 해당 아이템의 딕셔너리 참조값을 반환합니다<br/>
        /// 아이템이 현재 인벤토리의 목록에 존재하지 않아도 사전 참조값을 얻을 수 있습니다.<br/>
        /// </summary>
        /// <returns>*** 해당하는 이름의 아이템이 인벤토리 목록에 존재하지 않는 경우 예외를 던집니다. ***</returns>
        public Dictionary<string, List<GameObject>> GetItemDicIgnoreExsists(string itemName)
        {
            WorldItem worldItem = new WorldItem();       // 추후 GetComponent기반으로 변경예정

            if(worldItem.weapDic.ContainsKey(itemName))
                return weapDic;
            else if(worldItem.miscDic.ContainsKey(itemName))
                return miscDic;
            else
                throw new Exception("해당하는 아이템이 월드 사전 목록에 존재하지 않습니다. 아이템의 이름을 확인하여 주세요.");
        }


        /// <summary>
        /// 아이템의 타입을 기반으로 해당 아이템의 딕셔너리 참조값을 반환합니다<br/>
        /// 아이템이 현재 인벤토리의 목록에 존재하지 않아도 사전 참조값을 얻을 수 있습니다.<br/>
        /// </summary>
        /// <returns>itemType인자가 ItemType.None으로 전달 된 경우 null을 반환합니다</returns>
        public Dictionary<string, List<GameObject>> GetItemDicIgnoreExsists(ItemType itemType)
        {
            switch(itemType)
            {
                case ItemType.Weapon:
                    return weapDic;
                case ItemType.Misc:
                    return miscDic;
                default :
                    return null;
            }
        }
        

        /// <summary>
        /// 아이템 이름을 기반으로 해당 아이템의 오브젝트 리스트 참조값을 반환합니다
        /// </summary>
        /// <returns>해당하는 이름의 아이템이 인벤토리 목록에 존재하지 않는 경우 null을 반환합니다</returns>
        public List<GameObject> GetItemObjectList(string itemName)
        {
            if(weapDic.ContainsKey(itemName))
                return weapDic[itemName];
            else if(miscDic.ContainsKey(itemName))
                return miscDic[itemName];
            else
                return null;
        }
















        public void UpdateAllItemInfo()
        {
            foreach( List<GameObject> objList in weapDic.Values )                   // 무기사전에서 게임오브젝트 리스트를 하나씩 꺼내어
            {
                for(int i=0; i<objList.Count; i++)                                  // 리스트의 게임오브젝트를 모두 가져옵니다.
                    objList[i].GetComponentInChildren<ItemInfo>().OnItemCreated();  // item 스크립트를 하나씩 꺼내어 OnItemChnaged메서드를 호출합니다.
            }

            foreach( List<GameObject> objList in miscDic.Values )                   // 잡화사전에서 게임오브젝트 리스트를 하나씩 꺼내어
            {
                for(int i=0; i<objList.Count; i++)                                  // 리스트의 게임오브젝트를 모두 가져옵니다.
                    objList[i].GetComponentInChildren<ItemInfo>().OnItemCreated();  // item 스크립트를 하나씩 꺼내어 OnItemChnaged메서드를 호출합니다.
            }      
        }


    }
}
