using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * <v1.0 - 2023_1231_최원준>
 * 1- FindNearstSlotIdx itemName과 itemType을 인자로 받는 오버로등 메서드 구현
 * 
 * <v1.1 - 2024_0102_최원준>
 * 1- 아이템 수량 검색기능
 * 
 * 2- 아이템 수량 조절 및 목록에서 제거 기능
 * 
 */


namespace InventoryManagement
{
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
        

        public bool isEnough(string itemName, int overlapCount)
        {
            return true;
        }

                
        public int SetOverlapCount(GameObject itemObj)
        {
            ItemInfo itemInfo = itemObj.GetComponent<ItemInfo>();
            return SetOverlapCount(itemInfo);
        }

        public int SetOverlapCount(ItemInfo itemInfo)
        {
            Item item = itemInfo.Item;
            return SetOverlapCount(item);
        }

        public int SetOverlapCount(Item item)
        {
            ((ItemMisc)item).SetOverlapCount()
        }

        public string SetOverlapCount(string itemName)
        {

        }





    }

    

}