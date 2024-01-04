using ItemData;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
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
 * <v3.1 - 2024_0102_최원준>
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
 * <v3.2 - 2024_0102_최원준>
 * 1- ItemType enum을 int형으로 반환받는 메서드인 GetItemTypeIndexIgnoreExists 메서드를 추가
 * 
 * 
 * 
 * 
 * [추가 사항]
 * 1- AddItem 메서드로 아이템을 넣지만 포지션 업데이트를 외부 InventoryInfo스크립트에서 해줘야 함.
 * (포지션 업데이트를 현재활성화 탭기준으로 해놔야 한다.)
 * 
 * 2- AddItem 메서드로 인벤토리에 넣을 때, 개별 슬롯 인덱스와 전체 슬롯 인덱스를 둘다 찾아서 넣어줘야 한다.
 * (CreateManager도 수정필요)
 * 
 * 
 * <v4.0 - 2024_0103_최원준>
 * 1- RemoveItem메서드의 반환값을 bool이 아니라 GameObject로 변경
 * 이유는 Inventory클래스에서는 오브젝트의 파괴 처리를 못하기 때문에 Info클래스로 참조값을 넘겨야 하기 때문
 * 
 * 2- GameObject, Item 클래스를 인자로 받는 오버로딩 메서드 제거
 * 코드가 쓸데없이 길어지기 때문에 필요 시 추가할 예정
 * 
 * 3- AddItem메서드 주석 수정
 * 
 * <v4.1 - 2024_0103_최원준>
 * 1- RemoveItem에서 ItemInfo를 받는 메서드, isLatest를 받는 인자를 삭제하고, 검색 삭제 방식이 아닌 참조삭제 방식으로 구현
 * 이유는 동일 아이템을 아무것이나 삭제하는 것이 아니라 수량등의 정보를 조절을 한 특정아이템을 정확히 목록에서 삭제해야 하는 경우가 생기기 때문 
 * 
 * 2- GetItemObjectList메서드 내부에 isNewIfNotExist 인자 옵션 추가
 * 아이템 오브젝트리스트가 인벤토리 내부 사전에 존재하지 않아도 새롭게 생성 및 추가하여 반환이 가능하도록 구현 
 * 
 * 3- UpdateAllItemInfo 메서드를 InventoryInfo클래스로 옮김.
 * InventoryInfo 클래스에서 인벤토리 로드시 사용하였으나, 내부처리보다 외부처리에 가깝고
 * 인벤토리에 존재하는 모든 아이템을 대상으로 OnItemCreated메서드를 호출하여야 하나 전달인자로 InventoryInfo 참조를 전달해야 하기 때문
 * 
 * 4- GetItemMaxOverlapCount메서드 구현
 * 월드 아이템으로부터 정보를 받아서 아이템 별 동일한 최대 충첩허용 갯수를 반환하는 용도 
 * (신규 아이템 추가 시 어느정도의 오브젝트를 추가해야하는 지 알기 위해 오브젝트가 아예 없는 경우에 참조를 못하게 되므로 필요)
 * 
 * 
 * <v4.2 - 2024_0104_최원준>
 * 1- AddItemToDic 내부에 SetCurItemObjCount메서드문장 삽입
 * 
 * 
 */

namespace InventoryManagement
{    
    public partial class Inventory
    {
        

        /// <summary>
        /// ItemInfo 컴포넌트를 인자로 전달받아 해당 아이템 오브젝트에 해당하는 사전을 찾아서 아이템을 넣어주는 메서드입니다.<br/><br/>
        /// *** 인자로 들어온 컴포넌트 참조값이 없다면 예외를 발생시킵니다. ***<br/>
        /// </summary>
        /// <returns>아이템 추가 성공시 true를, 현재 인벤토리에 들어갈 공간이 부족하다면 false를 반환합니다</returns>
        public bool AddItem(ItemInfo itemInfo)
        {   
            if(itemInfo==null)
                throw new Exception("아이템 스크립트가 존재하지 않는 오브젝트입니다. 확인하여 주세요.");

            // 아이템 타입을 확인합니다.
            ItemType itemType = itemInfo.Item.Type;

            // 잡화 아이템과 비잡화 아이템으로 구분하여 메서드를 호출합니다.
            switch(itemType)
            {
                // 타입이 불분명한 경우 예외처리
                case ItemType.None:
                    throw new Exception("해당 아이템의 종류가 명확하지 않습니다. 확인하여 주세요.");

                case ItemType.Misc:
                    return AddItemMisc(itemInfo);   // 별도의 추가 메서드를 호출합니다.

                default:
                    return AddItemToDic(itemInfo);  // 바로 딕셔너리에 추가합니다.
            }            
        }
                
  
        /// <summary>
        /// 추가 할 ItemInfo 컴포넌트를 인자로 전달받아 해당 사전에 아이템 오브젝트를 넣어주는 메서드입니다.<br/>
        /// 해당 종류의 아이템 1개가 들어갈 빈 슬롯이 있어야 합니다.
        /// </summary>
        /// <returns>해당 아이템 종류의 아이템이 들어갈 빈 슬롯이 없다면 false, 아이템 추가에 성공 시 true를 반환합니다.</returns>
        public bool AddItemToDic(ItemInfo itemInfo)
        {          
            // 아이템 이름과 종류를 참조합니다.
            string itemName = itemInfo.Item.Name;
            ItemType itemType = itemInfo.Item.Type;

            // 남은 슬롯이 없다면 실패를 반환합니다.
            if( GetCurRemainSlotCount(itemType) == 0 )            
                return false;   

            
            // 아이템 타입을 기반으로 딕셔너리를 결정합니다.
            Dictionary<string, List<GameObject>> itemDic = GetItemDicIgnoreExsists(itemType);

            if( !itemDic.ContainsKey(itemName) )            // 해당 사전에 오브젝트 리스트가 존재하지 않는 경우
            {
                List<GameObject> itemObjList = new List<GameObject>();  // 신규 오브젝트 리스트를 만듭니다
                itemObjList.Add(itemInfo.gameObject);                   // 신규 오브젝트 리스트에 아이템 오브젝트를 추가합니다
                itemDic.Add(itemName, itemObjList);                     // 사전에 신규 오브젝트 리스트를 추가합니다.
            }
            else                                            // 해당 사전에 오브젝트 리스트가 존재하는 경우 
            {
                itemDic[itemName].Add(itemInfo.gameObject); // 기존 오브젝트 리스트에 접근하여 게임오브젝트를 넣습니다.
            }            
            
            SetCurItemObjCount(itemType, 1);                // 해당 아이템 종류의 현재 오브젝트의 갯수를 증가시킵니다.
            return true;                                    // 성공을 반환합니다.
        }

        

        /// <summary>
        /// 잡화 아이템 오브젝트를 기존의 인벤토리에 추가하여 줍니다.<br/>
        /// 기존의 동일한 잡화아이템이 최대 수량이 채워지지 않았다면, 들어온 아이템의 수량을 감소시켜 기존 아이템의 수량을 채웁니다.
        /// <br/><br/>
        /// 들어온 아이템의 수량이 0이 되었다면 오브젝트를 파괴하고,<br/> 
        /// 0이되지 않았다면 해당 아이템을 새롭게 인벤토리 슬롯에 추가합니다.<br/><br/>
        /// 기본적으로 슬롯에서 앞선 순서로 채워지며 이를 변경할 수 있습니다. (기본값: 오래된순)<br/>
        /// </summary>
        /// <returns>잡화 아이템이 들어갈 공간이 없다면 false를, 아이템 추가에 성공한 경우 true를 반환합니다.</returns>
        private bool AddItemMisc(ItemInfo itemInfo, bool isLatestModify=false)
        {
            // 아이템이 들어갈 공간이 없는 경우 실패를 반환합니다.
            if( GetCurRemainSlotCount(itemInfo.Item.Type) == 0 )
                return false;

            // 아이템 정보를 전달하여 기존아이템에 채우기를 실행하고, 남은 수량을 반환받습니다.
            int afterCount = FillExistItemOverlapCount(itemInfo, isLatestModify);

            // 수량채우기가 종료된 후 
            if( afterCount==0 )
                GameObject.Destroy( itemInfo.gameObject );  // 남은 수량이 0이 되었다면, 인자로 전달받은 오브젝트 삭제
            else
                AddItemToDic(itemInfo);                     // 남은 수량이 존재한다면, 해당 아이템을 사전에 추가

            return true;
        }


        /// <summary>
        /// 잡화 아이템을 인벤토리에 추가하기 전에,<br/>
        /// 동일한 이름의 기존 잡화 아이템에 수량을 채워주고, 남은 수량을 반환합니다.<br/>
        /// 기존 잡화아이템이 없다면 채우지 못하고 남은 수량을 그대로 반환합니다.<br/><br/>
        /// 인자로 전달한 ItemInfo의 수량은 채운만큼 감소되어 있으며,<br/>
        /// 수량이 0이 될때 까지 채우지만 삭제는 별도로 하지 않습니다.<br/><br/>
        /// 슬롯 순서 상 앞선 아이템부터 채우며, 변경할 수 있습니다.(기본값: 오래된순)<br/><br/>
        /// *** 전달한 아이템이 잡화아이템이 아닌 경우 예외를 발생시킵니다. ***<br/>
        /// </summary>
        /// <returns>기존 아이템에 모든 수량을 다 채운 경우 0을 반환, 다 채우지 못한 경우 남은 수량을 반환</returns>
        public int FillExistItemOverlapCount(ItemInfo itemInfo, bool isLatestModify=false)
        {            
            if(itemInfo.Item.Type != ItemType.Misc)
                throw new Exception("전달 받은 아이템이 잡화 아이템이 아닙니다. 확인하여 주세요.");

            ItemMisc newItemMisc = (ItemMisc)itemInfo.Item;

            // 오브젝트 리스트 참조를 설정합니다.
            List<GameObject> itemObjList = GetItemObjectList( newItemMisc.Name );                       

            // 기존 아이템에 수량채우기 실행 전 후를 비교할 수량을 현재 아이템의 수량으로 설정합니다.
            int beforeCount = newItemMisc.OverlapCount; 
            int afterCount = newItemMisc.OverlapCount;

            // 기존 오브젝트 리스트가 있는 경우는
            if( itemObjList!=null )
            { 
                // 오름차순 정렬을 시행합니다.
                itemObjList.Sort(CompareBySlotIndexEach);

                // 슬롯 순서로 정렬 된 아이템을 접근 기준에 따라 하나씩 꺼내어 기존 아이템에 수량채우기를 실행합니다.
                if( isLatestModify )
                {
                    for( int i = itemObjList.Count-1; i>=0; i-- )
                        if( FillCountInLoopByOrder(i) ) { break; } // 내부적으로 true를 반환하면 빠져나갑니다.                                     
                }
                else
                {
                    for( int i = 0; i<=itemObjList.Count-1; i++ )   
                        if( FillCountInLoopByOrder(i) ) { break; } // 내부적으로 true를 반환하면 빠져나갑니다.  
                }
            }

            // 채워주고 남은 수량을 반환합니다.
            return afterCount;

            



            // 기존 아이템에 접근 순서를 다르게 하여 채우기 위한 반복문용 내부 메서드입니다.
            bool FillCountInLoopByOrder(int idx)
            {
                // 기존 아이템에 인덱스를 통한 접근을 하여 정보를 불러옵니다.
                ItemMisc oldItemMisc = (ItemMisc)itemObjList[idx].GetComponent<ItemInfo>().Item;
                    
                // 기존 아이템에 현재 남은수량을 최대 허용치까지 채우고 남은 수량을 반환받습니다.
                afterCount = oldItemMisc.SetOverlapCount(beforeCount);

                // 줄어든 수량만큼 신규아이템의 수량을 감소시켜 줍니다.
                newItemMisc.SetOverlapCount(beforeCount-afterCount);
                    
                // 다음 반복문에 사용하기 위하여 현재수량과 일치시켜 줍니다. 
                beforeCount = afterCount;


                // 남은 수량이 0이되면 외부 호출 금지를 활성화합니다.
                if(afterCount==0)
                    return true;
                else
                    return false;
            }
        }





        
        
        /// <summary>
        /// 특정 아이템의 ItemInfo 컴포넌트를 인자로 받아서 인벤토리의 딕셔너리 목록에서 제거해주는 메서드 입니다.<br/>
        /// 검색 방식이 아니라 직접 참조하여 목록에서 제거하므로, 특정 아이템을 직접 제거하는 용도로 사용합니다.<br/>
        /// *** 참조 값이 존재하지 않으면 예외를 반환합니다. ***
        /// </summary>
        /// <returns>딕셔너리 목록의 제거에 성공한 경우 해당 아이템의 ItemInfo 참조값을, 목록에 없는 아이템인 경우 null을 반환합니다.</returns>
        public ItemInfo RemoveItem(ItemInfo itemInfo)
        {
            // 전달인자의 예외처리
            if(itemInfo==null)
                throw new Exception("아이템 스크립트가 존재하지 않는 오브젝트입니다. 확인하여 주세요.");
            
            // 이름을 기반으로 해당 딕셔너리 참조를 받습니다.
            Dictionary<string, List<GameObject>> itemDic = GetItemDicInExists(itemInfo.Item.Name);            

            // 인벤토리 목록에서 없는 아이템 예외처리
            if(itemDic==null)
                throw new Exception("해당 아이템이 인벤토리 내부에 존재하지 않습니다. 확인하여 주세요.");

            // List<GameObject>에 직접 GameObject 인스턴스를 전달하여 목록에서 제거합니다.
            itemDic[itemInfo.Item.Name].Remove(itemInfo.gameObject);

            // 목록에서 제거한 참조값을 다시 반환합니다. 
            return itemInfo;
        }
        
        /// <summary>
        /// 아이템의 이름을 인자로 받아서 해당 아이템을 검색하여 인벤토리의 딕셔너리 목록에서 제거해주는 메서드 입니다.<br/>
        /// 인자를 통해 최신순으로 제거 할 것인지, 오래된 순으로 제거할 것인지를 결정할 수 있습니다. 기본은 최신순입니다.<br/>
        /// </summary>
        /// <returns>딕셔너리 목록의 제거에 성공한 경우 해당 아이템의 ItemInfo 참조값을, 목록에 없는 아이템인 경우 null을 반환합니다.</returns>
        public ItemInfo RemoveItem(string itemName, bool isLatest=true)
        {
            // 이름을 통해 현재 아이템이 담긴 딕셔너리가 있는지 조사합니다
            Dictionary<string, List<GameObject>> itemDic = GetItemDicInExists(itemName);    
               
            // 딕셔너리가 존재하지 않는다면, 실패를 반환합니다
            if(itemDic==null)       
                return null;       
            
            // 딕셔너리에서 오브젝트 리스트를 받습니다
            List<GameObject> itemObjList = itemDic[itemName]; 

            // 해당 오브젝트 리스트를 참고하여 아이템의 타입을 미리 얻습니다
            ItemType itemType = itemObjList[0].GetComponent<ItemInfo>().Item.Type;            
            
            ItemInfo targetItemInfo = null;  // 반환할 아이템과 참조할 인덱스를 설정합니다.
            int targetIdx;

            if( isLatest )
                targetIdx = itemObjList.Count-1;  // 최신 순
            else
                targetIdx = 0;                    //오래된 순

            targetItemInfo =itemObjList[targetIdx].GetComponent<ItemInfo>();  // 반환 할 아이템 참조값을 얻습니다.
            itemObjList.RemoveAt(targetIdx);        // 해당 인덱스의 아이템을 제거합니다            
        
            SetCurItemObjCount(itemType, -1);       // 해당 아이템 종류의 현재 오브젝트의 갯수를 감소시킵니다.      
                        
            return targetItemInfo;            // 목록에서 제거한 아이템을 반환합니다.     
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
        /// 아이템 이름이 이 인벤토리의 딕셔너리 내부에 존재하여야 합니다.
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
        /// <returns>*** 해당하는 이름의 아이템이 월드 아이템 목록에 존재하지 않는 경우 예외를 던집니다. ***</returns>
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
        /// <returns>*** 해당하는 이름이 월드 아이템 목록에 존재하지 않는 경우 예외를 던집니다. ***</returns>
        public Dictionary<string, List<GameObject>> GetItemDicIgnoreExsists(string itemName)
        {
            CreateManager createManager = GameObject.FindWithTag("GameController").GetComponent<CreateManager>();
            
            if( createManager.GetWorldItemType(itemName) == GetItemTypeIgnoreExists

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
        /// 아이템 이름을 기반으로 해당 아이템의 오브젝트 리스트 참조값을 반환합니다<br/>
        /// 해당 오브젝트 리스트가 존재하지 않는 경우 null을 반환하지만,<br/>
        /// isNewIfNotExist 옵션을 통해 인벤토리 내부에 새롭게 생성하여 반환하기 여부를 결정할 수 있습니다.
        /// </summary>
        /// <returns>해당하는 이름의 아이템이 인벤토리 목록에 존재하는 경우 GameObject형식의 리스트를, 존재하지 않는 경우 null을 반환합니다</returns>
        public List<GameObject> GetItemObjectList(string itemName, bool isNewIfNotExist=false)
        {
            if(weapDic.ContainsKey(itemName))
                return weapDic[itemName];
            else if(miscDic.ContainsKey(itemName))
                return miscDic[itemName];
            // 해당 리스트가 인벤토리 내부에 없는 경우
            else 
            {                                   
                if(isNewIfNotExist)     // 새로 생성하기 옵션이 설정된 경우 
                {
                    List<GameObject> itemObjList = new List<GameObject>();          // 오브젝트 리스트를 새로 만듭니다.
                    GetItemDicIgnoreExsists(itemName).Add(itemName, itemObjList);   // 인벤토리 사전에 오브젝트 리스트를 집어넣습니다.
                    return itemObjList;                                             // 생성된 오브젝트 리스트 참조를 반환합니다.
                }
                else                    
                    return null;
            }
        }


        /// <summary>
        /// 아이템 이름을 입력하여 해당 아이템의 종류 enum을 int형으로 반환받습니다. (인벤토리에 존재하지 않아도 됩니다.) <br/>
        /// *** 아이템이 월드 사전에 존재하지 않는다면 예외를 던집니다. ***
        /// </summary>
        /// <returns>해당 아이템의 ItemType의 int형 반환 값</returns>
        public int GetItemTypeIndexIgnoreExists(string itemName)
        {           
            return (int)GetItemTypeIgnoreExists(itemName);
        }



        /// <summary>
        /// 해당 아이템의 이름을 인자로 넣어 아이템의 중첩 최대 갯수를 반환 받습니다.<br/>
        /// 아이템 별로 동일한 최대 중첩 가능 갯수를 알기 위한 메서드입니다.<br/><br/>
        /// 현재 IsAbleToAddMisc메서드에서 내부적으로 사용됩니다.<br/><br/>
        /// *** 해당하는 이름의 아이템이 없는 경우 예외가 발생합니다.<br/>
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns>아이템의 최대 갯수를 반환합니다.</returns>
        public int GetItemMaxOverlapCount(string itemName)
        {
            CreateManager createManager = GameObject.FindWithTag("GameController").GetComponent<CreateManager>();

            Dictionary<string, Item> worldDic = createManager.GetWorldDic(itemName);

            return ((ItemMisc)worldDic[itemName]).MaxOverlapCount;
        }


        


    }
}
