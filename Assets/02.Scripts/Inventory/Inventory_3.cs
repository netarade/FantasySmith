using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
 * 
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
        /// 인벤토리에 존재하는 해당 이름의 아이템 수량을 증가시키거나 감소시킵니다.<br/>
        /// 인자로 해당 아이템 이름과 수량을 지정해야 합니다.<br/><br/>
        /// 아이템 수량을 감소시키려면 수량 인자로 음수를 전달하여야 하며,<br/>
        /// 기존 수량이 감소로 인해 0이되면 아이템이 인벤토리 목록에서 제거하고 파괴해야 합니다.<br/><br/>
        /// 
        /// 아이템 수량을 증가시키려면 수량 인자로 양수를 전달하여야 하며,<br/>
        /// 아이템 최대 수량 제한으로 인해 더 이상 수량을 증가시키기 못하는 경우는 나머지 초과 수량을 반환합니다.<br/><br/>
        /// 최신 순, 오래된 순으로 감소여부를 결정할 수있습니다. (기본값: 최신순)<br/><br/>
        /// ** 아이템 이름이 해당 인벤토리에 존재하지 않거나, 잡화아이템이 아닌 경우 예외를 발생시킵니다. **
        /// </summary>
        /// <returns></returns>
        public int SetOverlapCount(string itemName, int inCount, bool isLatestReduce=true)
        {
            List<GameObject> itemObjList = GetItemObjectList(itemName);     // 인벤토리의 아이템 오브젝트 리스트 참조
            ItemType itemType = GetItemTypeInExists(itemName);              // 아이템 종류 참조

            
            // 해당 아이템 이름으로 인벤토리 사전이 존재하지 않는 경우, 아이템의 종류가 잡화아이템이 아닌 경우 예외처리
            if(itemObjList==null)
                throw new Exception("아이템이 이 인벤토리에 존재하지 않습니다.");
            else if(itemType != ItemType.Misc)
                throw new Exception("잡화 아이템이 아닙니다.");

            ItemInfo itemInfo;            // 아이템 정보 참조
            int remainCount = inCount;    // 가산 또는 감산하고 남은 수량 (초기값 : 수량 전달인자)
                                    

            // 최신순 감소
            if( isLatestReduce )
            {
                for( int i = itemObjList.Count-1; i>=0; i-- )   // 오브젝트 리스트의 마지막 인덱스부터 오브젝트를 하나씩 꺼내어 옵니다.
                {
                    itemInfo=itemObjList[i].GetComponent<ItemInfo>();
                    remainCount=itemInfo.SetOverlapCount( remainCount );    // 남은수량을 넣어서 새로운 남은 수량을 반환받습니다.

                    if( remainCount==0 )        // for문이 끝나기 전에 reaminCount가 0이 된 경우, 남은 수량이 0이 되었음을 반환
                        return 0;
                }
            }
            // 오래된순 감소 
            else                
            { 
                for( int i=0; i<=itemObjList.Count-1; i++ )     // 오브젝트 리스트의 마지막 인덱스부터 오브젝트를 하나씩 꺼내어 옵니다.
                {                    
                    itemInfo=itemObjList[i].GetComponent<ItemInfo>();
                    remainCount=itemInfo.SetOverlapCount( remainCount );    // 남은수량을 넣어서 새로운 남은 수량을 반환받습니다.

                    if( remainCount==0 )        // for문이 끝나기 전에 reaminCount가 0이 된 경우, 남은 수량이 0이 되었음을 반환
                        return 0;
                }
            }

            return remainCount;     // for문이 끝난 후에도 remainCount가 0이 되지 않은 경우, 남은 수량을 반환
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
        public bool IsEnoughOverlapCount(string itemName, int overlapCount=1, bool isReduce=false, bool isLatestReduce=true)
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
                    SetOverlapCount(itemName, -overlapCount, isLatestReduce);

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
        public bool IsExist(string itemName, bool isRemove=false, bool isLatestRemove=true)
        {
            List<GameObject> itemObjList = GetItemObjectList(itemName);     // 인벤토리의 아이템 오브젝트 리스트 참조
           
            if( itemObjList==null ) // 인벤토리에 오브젝트 리스트가 존재하지 않는 경우
            {
                return false;
            }           
            else                    // 오브젝트 리스트가 존재하는 경우
            {
                if(isRemove)        // 제거 모드인 경우
                    RemoveItem(itemName, isLatestRemove);

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
        public bool IsEnough(ItemPair pair, bool isReduce=false, bool isLatestReduce=true)
        {
            ItemType itemType = GetItemTypeInExists(pair.itemName);
            
            //잡화아이템인 경우 - 수량검사 및 수량감소메서드 호출, 잡화아이템이 아닌 경우 - 존재여부 및 제거메서드 호출
            if( itemType==ItemType.Misc )   
                return IsEnoughOverlapCount( pair.itemName, pair.overlapCount, isReduce, isLatestReduce );            
            else
                return IsExist(pair.itemName, isReduce, isLatestReduce);
        }


        /// <summary>
        /// 아이템의 종류와 상관없이 아이템이 인벤토리에 존재하는지, 잡화아이템이라면 수량까지도 충분한지 여부를 반환합니다.<br/>
        /// 또한 아이템이 존재하거나, 잡화아이템인 경우 수량까지 충분하다면 제거 또는 감소를 결정할 수 있습니다.<br/>
        /// (기본값: 감소모드 안함, 최신순 감소)<br/><br/>
        /// *** 비잡화 아이템의 경우 수량 값을 무시합니다. 잡화아이템의 경우 수량이 1이상이 아니면 예외를 발생시킵니다. ***
        /// </summary>
        /// <returns>전달 한 인자의 모든 조건을 충족하는 경우 true를, 조건을 충족하지 않는 경우 false를 반환, 조건이 충족한 경우 감소를 수행</returns>
        public bool IsEnough( ItemPair[] pairs, bool isReduce=false, bool isLatestReduce=true)
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
                        IsEnough(pair, isReduce, isLatestReduce);
                }

                return true;
            }
            // 하나라도 조건을 충족하지 않는 경우 실패를 반환
            else
                return false;

        }


                

        



    }

    

}