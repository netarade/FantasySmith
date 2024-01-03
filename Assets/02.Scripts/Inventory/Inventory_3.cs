using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

/*
 * <v1.0 - 2023_1231_최원준>
 * 1- FindNearstSlotIdx itemName과 itemType을 인자로 받는 오버로등 메서드 구현
 * 
 * <v1.1 - 2024_0102_최원준>
 * 1- 아이템 수량 검색기능 
 * IsEnoughOverlapCount 메서드 작성
 * 
 * 2- 아이템 수량 조절 및 목록에서 제거 기능 
 * IsEnoughOverlapCount 메서드에 수량감소모드 추가 및 SetOverlapCount 메서드 작성
 * 
 * (향후 구현 예정_0102)
 * 1- 이름과 수량을 구조체 형태로 만들어서 배열로 전달하는 오버로딩 메서드 구현하기
 * 
 * 
 * <v2.0 - 2024_0103_최원준>
 * 1- IsEnoughOverlapCount메서드 수량전달인자에 대한 예외처리 추가 
 *  
 * 2- IsExist 추가
 * 비잡화 아이템이 존재하는지 여부를 반환하는 메서드, 삭제까지 가능
 * 
 * 3- ItemPair 구조체 추가
 * 인자로 이름과 수량 쌍을 전달할 수 있도록 설계
 * 
 * 4- ItemPair과 ItemPair배열을 인자로 받는 IsEnough메서드 정의
 * 인자로 전달한 아이템이 있는지, 수량까지 충분한지를 검사하고 수량감소 또는 제거할 수있도록 구현
 * 
 * (이슈)
 * 1- IsEnough, IsEnoughOverlapCount, IsExist 메서드의 반환값을 ItemInfo로 변경해야한다
 * 이유는 수량이 0이된 아이템 오브젝트를 Info클래스에서 삭제처리해야 하기 때문
 * 
 * <v2.1 - 2024_0103_최원준>
 * (이슈_0103) 
 * 1- SetOverlapCount메서드에서
 * 오브젝트리스트에서 하나씩 아이템을 읽어와서 수량 정보를 변경시키고 있는데,
 * 인벤토리 제거 시 오브젝트 리스트 변동이 일어나는 문제가 있다.
 * => 삭제할 아이템을 모아서 한번에 리스트에서 제거하는 형태로 구현해야
 * 
 * 
 * (이슈_0104)
 * 1- 제거할 때 정렬해서 제거해야 하는데 현재 들어간 오브젝트 순서로 읽어들이고 있다.
 * 하지만, 실제 게임에서는 slotIndex순으로 제거해야 하므로, slotIndex를 기반으로 정렬해서 제거해줘야 한다.
 * 
 * <v3.0 - 2024_0104_최원준>
 * 1- InventoryInfo에 아이템 데이터를 전송하기 위한 passItemInfoList를 선언하고
 * SetOverlapCount메서드에서 내부적으로 삭제할 아이템을 일시적을 담아서 전달하는 형식으로 구현하였음.
 * 
 * 메서드 내부에서 삭제하지 않고 Info클래스로 전송하는 이유는 정보수정은 Inventory클래스가 담당하지만, 
 * 오브젝트 권한은 InventoryInfo클래스에게 넘거야 하기 때문이며,
 * 수량을 다시 되돌리는 작업이 필요할 때가 있기 때문 
 * (ex. 잡화아이템 50개를 먹었는데 슬롯이 비어있지 않다면, 기존의 아이템에 다 들어가는지를 알아야 한다.)
 * 
 * 2- 전달인자 변수명 isLatestRemove, Reduce등을 isLatestModify로 통일
 * 
 * 3- IsAbleToAddMisc메서드 추가
 * IsEnough, IsExist 등은 감소나 제거를 목적으로 하지만 
 * 이 메서드는 추가 하기위해 기존 수량에 들어가기 충분한지, 그리고 새로운 아이템을 생성하기 위한 슬롯공간이 있는지 여부를 반환받기 위함.
 * 
 */


namespace InventoryManagement
{

    /// <summary>
    /// 아이템 이름과 수량을 나타내는 구조체입니다.<br/>
    /// 인벤토리 메서드의 전달인자로 사용되며,<br/>
    /// 해당 아이템의 이름이 인벤토리에 존재하는 지, 아이템 수량은 충분한 지 확인하는 용도로 사용됩니다.
    /// </summary>
    public struct ItemPair
    {
        public string itemName;
        public int overlapCount;

        public ItemPair(string itemName, int overlapCount)
        {
            this.itemName = itemName;
            this.overlapCount = overlapCount;
        }
    }




    public partial class Inventory
    {
        /// <summary>
        /// InventoryInfo클래스에 필요 시 아이템 정보를 전송하기 위해 사용하는 ItemInfo를 임시적으로 담는 리스트입니다.<br/>
        /// 현재 전송리스트 미 전달 시 대체인자로 SetOverlapCount메서드에서 내부적으로 사용됩니다.
        /// </summary>
        List<ItemInfo> tempInfoList = new List<ItemInfo>();


        /// <summary>
        /// 가장 가까운 슬롯의 인덱스를 구합니다. 어떤 이름의 아이템을 넣을 것인지와 <br/>
        /// 개별탭 혹은 전체탭의 인덱스를 반환받고자 하는지 여부를 전달하여야 합니다. (기본값: 전체탭)<br/>
        /// </summary>
        /// <returns>true을 전달할 경우 전체탭 슬롯 인덱스를 반환하며, false는 개별탭 슬롯 인덱스를 반환합니다. 
        /// 슬롯에 자리가 없다면 -1을 반환합니다.</returns>
        public int FindNearstSlotIdx(string itemName, bool isSlotIndexAll = true)
        {
            if(isSlotIndexAll)  // 전체 인덱스 모드가 설정되어 있는 경우
            {
                return FindNearstSlotIdx(ItemType.None);                // 전체 슬롯 인덱스를 기준으로 가까운 슬롯 인덱스를 반환받습니다.                
            }
            else                // 개별 인덱스 모드가 설정되어 있는 경우
            {
                ItemType itemType = GetItemTypeIgnoreExists(itemName);  // 아이템의 이름 기반으로 아이템 종류를 구합니다.
                return FindNearstSlotIdx(itemType);                     // 개별 슬롯인덱스를 기준으로 가까운 슬롯 인덱스를 반환받습니다.
            }
        }

        /// <summary>
        /// 가장 가까운 슬롯의 인덱스를 구합니다. 어떤 종류의 아이템을 넣을 것인지 인자로 전달하여야 합니다.<br/>
        /// ItemType인자에 해당하는 개별탭 슬롯 인덱스를 반환합니다. ItemType.None을 전달할 경우 전체탭의 슬롯 인덱스를 반환합니다. (기본값: 전체탭)
        /// </summary>
        /// <returns>ItemType.None을 전달할 경우 전체탭 슬롯 인덱스를 반환하며, 이외의 인자는 개별탭 슬롯 인덱스를 반환합니다. 
        /// 슬롯에 자리가 없다면 -1을 반환합니다.</returns>
        public int FindNearstSlotIdx(ItemType itemType = ItemType.None)
        {
            bool isSlotIndexAll;

            // 전달 받은 itemType인자를 기준으로 개별 슬롯을 구할 지, 전체 슬롯을 구할 지 여부를 판단합니다.
            if(itemType == ItemType.None)
                isSlotIndexAll = true;
            else
                isSlotIndexAll = false;
                        
            indexList.Clear();       // 기존의 인덱스 리스트를 지웁니다
            int findSlotIdx = -1;    // 찾을 슬롯 인덱스를 선언하고 초기값을 -1으로 설정합니다.


            // 아이템의 ItemType을 기반으로 해당하는 딕셔너리를 구합니다
            Dictionary<string, List<GameObject>> itemDic;                   


            if(isSlotIndexAll)      // 전체 슬롯 인덱스를 기반으로 구하는 경우
            {
                int dicLen = (int)ItemType.None;    // 개별 사전의 총 갯수를 설정합니다.

                for(int i=0; i<dicLen; i++)               
                {
                    // 존재하는 모든 딕셔너리 사전을 하나씩 가져옵니다.
                    itemDic = GetItemDicIgnoreExsists((ItemType)i); 

                    // 오브젝트를 하나씩 읽어들여서 아이템 인덱스를 인덱스 리스트에 집어넣습니다.
                    ReadSlotIdxToIndexList(indexList, itemDic, isSlotIndexAll);
                }  
            }
            else                    // 개별 슬롯 인덱스를 기반으로 구하는 경우
            {
                // 해당 아이템 종류의 사전을 구합니다. 
                itemDic = GetItemDicIgnoreExsists(itemType);    

                // 오브젝트를 하나씩 읽어들여서 아이템 인덱스를 인덱스 리스트에 집어넣습니다.
                ReadSlotIdxToIndexList(indexList, itemDic, isSlotIndexAll);
            }

            indexList.Sort();   // 인덱스 리스트를 오름차순으로 정렬합니다.
            
            // 아이템의 종류에 해당하는 슬롯의 칸 제한 수를 구합니다. (인자로 전달한 itemType이 None이라면 전체 슬롯 제한 수를 구합니다.)
            int slotCountLimit = GetItemSlotCountLimit(itemType); 

            // 인덱스 숫자까지 i를 0부터 증가시키면서 빈 슬롯을 찾습니다 
            for(int i=0; i<slotCountLimit; i++)
            {                
                if(i > indexList.Count-1)      // i가 마지막 인덱스보다 크다면, 더 이상 슬롯을 찾지 않고 i를 반환합니다.
                    findSlotIdx = i;
                else if(indexList[i] != i)     // i번째 인덱스리스트에 저장된 인덱스와 i가 일치하지 않는다면 슬롯이 빈 것으로 판단   
                    findSlotIdx = i;                 
            }

            //찾은 슬롯 인덱스를 반환합니다.
            return findSlotIdx;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReadSlotIdxToIndexList(List<int> indexList, Dictionary<string, List<GameObject>> itemDic, bool isSlotIndexAll )
        {   
            // 딕셔너리에 아무 아이템이 들어있지 않다면 바로 종료합니다.
            if( itemDic==null || itemDic.Count==0)
                return;

            // 해당 사전에서 아이템 정보를 하나씩 읽어옵니다.
            foreach(List<GameObject> itemObjList in itemDic.Values)     
            {
                foreach(GameObject itemObj in itemObjList)
                {
                    Item item = itemObj.GetComponent<ItemInfo>().Item;   

                    if( isSlotIndexAll )  
                        indexList.Add(item.SlotIndexAll);   // 전체 슬롯 인덱스 기반으로 구하는 경우
                    else                
                        indexList.Add(item.SlotIndex);      // 개별 슬롯 인덱스 기반으로 구하는 경우
                } 
            }
        }
        









        /// <summary>
        /// 리스트의 Sort메서드에 인덱스 정렬기준을 전달하기 위한 기준 메서드<br/>
        /// 잡화아이템의 개별 슬롯 인덱스를 기준으로 해서 오름차순으로 정렬됩니다.<br/>
        /// </summary>
        public static int CompareByIndex(GameObject itemObj1, GameObject itemObj2)
        {
            ItemMisc itemMisc1 = (ItemMisc)itemObj1.GetComponent<ItemInfo>().Item;
            ItemMisc itemMisc2 = (ItemMisc)itemObj2.GetComponent<ItemInfo>().Item;
            return itemMisc1.SlotIndex.CompareTo(itemMisc2.SlotIndex);
        }

        /// <summary>
        /// 인벤토리에 존재하는 해당 이름의 아이템 수량을 증가시키거나 감소시킵니다.<br/>
        /// 인자로 해당 아이템 이름과 수량, 삭제 대상 아이템의 참조값을 받을 전송 리스트, 조정방식을 전달해야 합니다.<br/><br/>
        /// 
        /// 아이템 수량을 감소시키려면 수량 인자로 음수를 전달, 증가시키려면 양수를 전달합니다.<br/>
        /// 각 아이템의 최소, 최대 수량에 도달하면 다음 아이템의 나머지 수량을 조절합니다.<br/><br/>
        /// 
        /// 기존 아이템이 수량이 감소로 인해 0 이되면 아이템이 인벤토리 목록에서 제거하고 전송 리스트에 아이템을 추가합니다.<br/>
        /// 만약 전송리스트에 참조값을 전달하지 않는다면, 아이템을 전송하지 않고 오브젝트를 즉시 삭제합니다.(기본값)<br/>
        /// 수량 증가의 경우는 인벤토리 목록에서 제거하거나, 전송리스트에 추가하지 않습니다.<br/><br/>
        /// 
        /// 모든 기존 아이템이 더 이상 수량을 조절하지 못하는 경우는 나머지 초과 수량을 반환합니다.<br/>
        /// 초과 수량으로 인한 오브젝트 새로운 생성과 수량 감소로 인한 기존 오브젝트 삭제는 메서드 호출자에게 권한이 있습니다.<br/><br/>
        /// 최신 순, 오래된 순으로 수량 조절 방식을 결정할 수있습니다. (기본값: 최신순)<br/><br/>
        /// ** 아이템 이름이 해당 인벤토리에 존재하지 않거나, 잡화아이템이 아닌 경우 예외를 발생시킵니다. **
        /// </summary>
        /// <returns>기존의 오브젝트에 감소 혹은 추가하고 남은 초과 수량, 모든 기존 아이템에 해당 수량이 들어갔다면 0을 반환</returns>
        public int SetOverlapCount(string itemName, int inCount, List<ItemInfo> passList=null ,bool isLatestModify=true)
        {
            List<GameObject> itemObjList = GetItemObjectList(itemName);     // 인벤토리의 아이템 오브젝트 리스트 참조
            ItemType itemType = GetItemTypeInExists(itemName);              // 아이템 종류 참조
                        
            // 해당 아이템 이름으로 인벤토리 사전이 존재하지 않는 경우, 아이템의 종류가 잡화아이템이 아닌 경우 예외처리
            if(itemObjList==null)
                throw new Exception("아이템이 이 인벤토리에 존재하지 않습니다.");
            else if(itemType != ItemType.Misc)
                throw new Exception("잡화 아이템이 아닙니다.");
            

            bool isInstantRemove = false;       // 즉시 삭제여부 판단 변수

            // 인자로 전달받은 전송 리스트가 null이라면 즉시 삭제 활성화
            if(passList==null)
            {
                isInstantRemove = true;       
                passList = this.tempInfoList;   // 임시리스트로 설정
            }



            ItemInfo itemInfo;                  // 아이템 컴포넌트 참조
            ItemMisc itemMisc;                  // 내부적으로 수정할 정보 클래스 참조
            int remainCount = inCount;          // 가산 또는 감산하고 남은 수량 (초기값 : 수량 전달인자)
            

            // 개별 슬롯 인덱스를 기준으로하여 오름차순 정렬
            itemObjList.Sort(CompareByIndex);

            // isLatestReduce에 따라 반복문을 돌릴 조건을 설정합니다.
            int idx;
            int end;
            int change;
            
            if(isLatestModify)  // 최신 순 수정
            {
                idx = itemObjList.Count-1;
                end = 0;
                change = -1;
            }
            else                // 오래된 순 수정
            {
                idx = 0;
                end = itemObjList.Count-1;
                change = 1;
            }

            // 인자로 들어온 수량이 0이 되거나 마지막 인덱스에 도달한 경우 종료합니다.
            while( remainCount!=0 || idx != end+change )
            {
                itemInfo = itemObjList[idx].GetComponent<ItemInfo>();
                itemMisc = (ItemMisc)itemInfo.Item;

                // 남은수량을 넣어서 새로운 남은 수량을 반환받습니다.
                remainCount = itemMisc.SetOverlapCount( remainCount );

                // 수정이 끝난 오브젝트의 중첩 텍스트를 수정합니다.
                itemInfo.UpdateCountTxt();

                // 변경한 아이템의 수량이 0이 된 경우에는 삭제 리스트에 담습니다.
                // 바로 제거하지 않고 따로 담는 이유는 중간에 리스트의 Count 변동이 생기기 때문입니다.
                if( itemMisc.OverlapCount==0 )
                    tempInfoList.Add(itemInfo);          
                
                // 증감 연산을 합니다.
                idx += change;      
            }

            // 삭제 리스트에 있는 아이템을 역순으로 순회합니다. 
            for( int i=tempInfoList.Count-1; i>=0; i--)
            {
                // 인벤토리 리스트에서 제거
                RemoveItem(tempInfoList[i]);
                                
                // 즉시 삭제가 활성화 되있다면 오브젝트 삭제
                if(isInstantRemove)
                    GameObject.Destroy(tempInfoList[i].gameObject);    
            }

            // 즉시 삭제의 경우 전송리스트 초기화
            if(isInstantRemove)
                tempInfoList.Clear();   

            // 호출하고 남은 수량을 반환합니다.
            return remainCount;     
        }

        

        /// <summary>
        /// 인벤토리에 해당 이름의 아이템의 중첩수량이 충분한지를 확인하는 메서드입니다.<br/>
        /// 인자로 아이템 이름과 수량이 필요합니다. (수량 미전달 시 기본 수량은 1개입니다.)<br/><br/>
        /// 세번 째 인자로 수량 감소모드를 선택하면 아이템의 중첩수량이 충분하다면 인자로 들어온 수량만큼 감소시키며, <br/>
        /// 0에 도달한 경우 아이템을 인벤토리에서 제거하고 파괴 시킵니다.<br/>
        /// 최신 순, 오래된 순으로 감소여부를 결정할 수있습니다. (기본값: 최신순)<br/><br/>
        /// ** 해당 이름의 아이템이 존재하지 않거나, 잡화 아이템이 아니거나, 수량을 잘못 전달했다면 예외를 발생시킵니다. **
        /// </summary>
        /// <returns>아이템 중첩수량이 충분하면 true를, 충분하지 않으면 false를 반환</returns>
        public bool IsEnoughOverlapCount(string itemName, int overlapCount=1, bool isReduce=false, bool isLatestModify=true)
        {           
            ItemType itemType = GetItemTypeInExists(itemName);              // 이름을 통한 아이템의 종류 구분
            List<GameObject> itemObjList = GetItemObjectList(itemName);     // 이름을 통한 아이템 오브젝트 리스트 참조

            // 해당 아이템 이름으로 인벤토리 사전이 존재하지 않는 경우, 아이템의 종류가 잡화아이템이 아닌 경우 예외처리
            if( itemObjList == null )
                throw new Exception("해당 아이템이 인벤토리에 존재하지 않습니다.");
            else if( itemType != ItemType.Misc)
                throw new Exception("해당 아이템이 잡화아이템이 아닙니다.");
            else if( overlapCount<=0 )
                throw new Exception("수량 전달인자는 1이상이어야 합니다.");
                                    
                                    
            int totalCount = 0;                         // 중첩수량을 누적 시키기 위한 변수 설정
            bool isTotalEnough = false;                 // 중첩수량이 충분한지 확인하기 위한 변수

            foreach(GameObject itemObj in itemObjList)  // 아이템을 하나씩 꺼내어 정보를 읽습니다.
            {
                ItemMisc itemMisc = (ItemMisc)itemObj.GetComponent<ItemInfo>().Item;
                totalCount += itemMisc.OverlapCount;    // 중첩수량을 누적시킵니다.
                   
                // 합계 누적 수량을 구할 때마다 인자로 들어 온 수량보다 큰지 확인합니다.
                if(totalCount >= overlapCount)  
                {
                    isTotalEnough = true;    // isTotalEnough를 true로 만들고 빠져나갑니다.
                    break;
                }
            }

            if(isTotalEnough)       // 합계 누적 수량이 인자로 들어온 수량을 초과하는 경우
            {
                if( isReduce )      // 삭제 모드라면, 충분 여부를 반환하기 전에 해당 수량만큼 감소시킵니다.
                    SetOverlapCount(itemName, -overlapCount, null ,isLatestModify);

                    return true;
            }
            // 합계 누적 수량이 인자로 들어온 수량을 초과하지 못하는 경우 실패를 반환합니다.
            else
                return false;
        }



        /// <summary>
        /// 아이템이 인벤토리에 존재하는지 여부를 반환합니다.<br/>
        /// 최신 순, 오래된 순으로 삭제여부를 결정할 수있습니다. (기본값: 삭제안함, 최신순)<br/><br/>
        /// *** 아이템의 종류나 수량과 상관없이 오브젝트 단위로 존재하는지 여부만 반환합니다. ***<br/>
        /// </summary>
        /// <returns>아이템이 존재하지 않는 경우 false를, 아이템이 존재하거나 삭제에 성공한 경우 true를 반환</returns>
        public bool IsExist(string itemName, bool isRemove=false, bool isLatestModify=true)
        {
            List<GameObject> itemObjList = GetItemObjectList(itemName);     // 인벤토리의 아이템 오브젝트 리스트 참조
           
            if( itemObjList==null ) // 인벤토리에 오브젝트 리스트가 존재하지 않는 경우
            {
                return false;
            }           
            else                    // 오브젝트 리스트가 존재하는 경우
            {
                if(isRemove)        // 제거 모드인 경우
                    RemoveItem(itemName, isLatestModify);

                return true;
            }
        }


            
        


        /// <summary>
        /// 아이템의 종류와 상관없이 아이템이 인벤토리에 존재하는지, 잡화아이템이라면 수량까지도 충분한지 여부를 반환합니다.<br/>
        /// 또한 아이템이 존재하거나, 잡화아이템인 경우 수량까지 충분하다면 제거 또는 감소를 결정할 수 있습니다.<br/>
        /// (기본값: 감소모드 안함, 최신순 감소)<br/><br/>
        /// *** 비잡화 아이템의 경우 수량 값을 무시합니다. 잡화아이템의 경우 수량이 1이상이 아니면 예외를 발생시킵니다. ***
        /// </summary>
        /// <returns>전달 한 인자의 모든 조건을 충족하는 경우 true를, 조건을 충족하지 않는 경우 false를 반환, 조건이 충족한 경우 감소를 수행</returns>
        public bool IsEnough(ItemPair pair, bool isReduce=false, bool isLatestModify=true)
        {
            ItemType itemType = GetItemTypeInExists(pair.itemName);
            
            //잡화아이템인 경우 - 수량검사 및 수량감소메서드 호출, 잡화아이템이 아닌 경우 - 존재여부 및 제거메서드 호출
            if( itemType==ItemType.Misc )   
                return IsEnoughOverlapCount( pair.itemName, pair.overlapCount, isReduce, isLatestModify );            
            else
                return IsExist(pair.itemName, isReduce, isLatestModify);
        }


        /// <summary>
        /// 아이템의 종류와 상관없이 아이템이 인벤토리에 존재하는지, 잡화아이템이라면 수량까지도 충분한지 여부를 반환합니다.<br/>
        /// 또한 아이템이 존재하거나, 잡화아이템인 경우 수량까지 충분하다면 제거 또는 감소를 결정할 수 있습니다.<br/>
        /// (기본값: 감소모드 안함, 최신순 감소)<br/><br/>
        /// *** 비잡화 아이템의 경우 수량 값을 무시합니다. 잡화아이템의 경우 수량이 1이상이 아니면 예외를 발생시킵니다. ***
        /// </summary>
        /// <returns>전달 한 인자의 모든 조건을 충족하는 경우 true를, 조건을 충족하지 않는 경우 false를 반환, 조건이 충족한 경우 감소를 수행</returns>
        public bool IsEnough( ItemPair[] pairs, bool isReduce=false, bool isLatestModify=true)
        {
            int allEnough=0;
                        
            // 모든 아이템이 조건을 충족하는 지 확인
            foreach(ItemPair pair in pairs )
            {
                if( IsEnough(pair) )
                    allEnough++;
            }
                                    

            // 모두 조건을 충족하는 경우
            if(allEnough==pairs.Length)
            {
                // 감소모드인 경우
                if(isReduce)
                {
                    foreach(ItemPair pair in pairs )
                        IsEnough(pair, isReduce, isLatestModify);
                }

                return true;
            }
            // 하나라도 조건을 충족하지 않는 경우 실패를 반환
            else
                return false;

        }



        /// <summary>
        /// 잡화 아이템이 인벤토리에 들어가기 충분한지를 반환해주는 메서드입니다.<br/>
        /// 아이템 이름과 생성할 수량을 넣어야 합니다.<br/><br/>
        /// *** 잡화 아이템이 아니거나, 수량전달인자가 0이하면 예외를 던집니다. ***
        /// </summary>
        /// <returns>아이템을 생성하기 위한 슬롯이 충분하다면 true를, 부족하다면 false를 반환</returns>
        public bool IsAbleToAddMisc(string itemName, int overlapCount)
        {
            // 기존 인벤토리 내부에 아이템이 없어도 새롭게 할당하기 위해 참조를 설정합니다.
            ItemType itemType = GetItemTypeIgnoreExists(itemName);              // 이름을 통한 아이템의 종류 구분
            List<GameObject> itemObjList = GetItemObjectList(itemName, true);   // 이름을 통한 아이템 오브젝트 리스트 참조

            // 해당 아이템의 종류가 잡화아이템이 아닌 경우와 수량이 잘못 전달 된 경우 예외처리
            if( itemType != ItemType.Misc)
                throw new Exception("해당 아이템이 잡화아이템이 아닙니다.");
            else if( overlapCount<=0 )
                throw new Exception("수량 전달인자는 1이상이어야 합니다.");
                

            // 남은 수량을 최초에 들어온 수량으로 설정
            int remainCount = overlapCount; 
            
            // 아이템 별 최대수량 참조
            int maxOverlapCount = GetItemMaxOverlapCount(itemName);
            
            // 현재 아이템 생성가능 한 최대 슬롯 갯수
            int curRemainSlotCnt = GetItemSlotCountLimit(ItemType.Misc) - GetCurItemCount(ItemType.Misc);

            // 기존 오브젝트 리스트가 있는 경우는 아이템을 하나씩 꺼내어 remainCount를 감산합니다.
            foreach(GameObject itemObj in itemObjList)
            {
                ItemMisc itemMisc = (ItemMisc)itemObj.GetComponent<ItemInfo>().Item;

                // 해당 아이템을 최대수량으로 만들기 위한 수량이 얼마 남았는 지를 계산합니다.
                remainCount -= (maxOverlapCount - itemMisc.OverlapCount);                  
            }

            
            // 생성 할 오브젝트 갯수 설정 : 최종적으로 남은 수량에서 오브젝트 1개별 최대수량으로 나눈 몫 (나누어 떨어지는 경우)
            int createCnt = remainCount / maxOverlapCount;
            int remainder = remainCount % maxOverlapCount;

            // 나누어 떨어지지 않아, 남는 소수점이 생길 때는 오브젝트가 하나 더 필요하므로 생성 갯수를 올립니다. 
            if(remainder>0)
                createCnt++;


            // 오브젝트 리스트가 아예 없는경우의 그대로인 남은 수량 또는
            // 기존 오브젝트 리스트에서 제외 시킨 만큼의 남은 수량이 0이상이라면,
            if(remainCount>0)       
            {       
                // 남은 슬롯 갯수가 생성해야 할 오브젝트 갯수보다 많거나 같다면 생성가능 반환
                if( curRemainSlotCnt >= createCnt )
                    return true;                    
            }

            // 성공을 반환하지 못했다면, (즉 생성이 불가능하다면) 실패를 반환 
            return false;
        }

                

        



    }

    

}