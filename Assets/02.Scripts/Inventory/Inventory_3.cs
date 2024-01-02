using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * <v1.0 - 2023_1231_�ֿ���>
 * 1- FindNearstSlotIdx itemName�� itemType�� ���ڷ� �޴� �����ε� �޼��� ����
 * 
 * <v1.1 - 2024_0102_�ֿ���>
 * 1- ������ ���� �˻����
 * 
 * 2- ������ ���� ���� �� ��Ͽ��� ���� ���
 * 
 */


namespace InventoryManagement
{
    public partial class Inventory
    {

        
        /// <summary>
        /// ���� ����� ������ �ε����� ���մϴ�. � �̸��� �������� ���� �������� <br/>
        /// ������ Ȥ�� ��ü���� �ε����� ��ȯ�ް��� �ϴ��� ���θ� �����Ͽ��� �մϴ�. (�⺻��: ��ü��)<br/>
        /// </summary>
        /// <returns>true�� ������ ��� ��ü�� ���� �ε����� ��ȯ�ϸ�, false�� ������ ���� �ε����� ��ȯ�մϴ�. 
        /// ���Կ� �ڸ��� ���ٸ� -1�� ��ȯ�մϴ�.</returns>
        public int FindNearstSlotIdx(string itemName, bool isSlotIndexAll = true)
        {
            if(isSlotIndexAll)  // ��ü �ε��� ��尡 �����Ǿ� �ִ� ���
            {
                return FindNearstSlotIdx(ItemType.None);                // ��ü ���� �ε����� �������� ����� ���� �ε����� ��ȯ�޽��ϴ�.                
            }
            else                // ���� �ε��� ��尡 �����Ǿ� �ִ� ���
            {
                ItemType itemType = GetItemTypeIgnoreExists(itemName);  // �������� �̸� ������� ������ ������ ���մϴ�.
                return FindNearstSlotIdx(itemType);                     // ���� �����ε����� �������� ����� ���� �ε����� ��ȯ�޽��ϴ�.
            }
        }

        /// <summary>
        /// ���� ����� ������ �ε����� ���մϴ�. � ������ �������� ���� ������ ���ڷ� �����Ͽ��� �մϴ�.<br/>
        /// ItemType���ڿ� �ش��ϴ� ������ ���� �ε����� ��ȯ�մϴ�. ItemType.None�� ������ ��� ��ü���� ���� �ε����� ��ȯ�մϴ�. (�⺻��: ��ü��)
        /// </summary>
        /// <returns>ItemType.None�� ������ ��� ��ü�� ���� �ε����� ��ȯ�ϸ�, �̿��� ���ڴ� ������ ���� �ε����� ��ȯ�մϴ�. 
        /// ���Կ� �ڸ��� ���ٸ� -1�� ��ȯ�մϴ�.</returns>
        public int FindNearstSlotIdx(ItemType itemType = ItemType.None)
        {
            bool isSlotIndexAll;

            // ���� ���� itemType���ڸ� �������� ���� ������ ���� ��, ��ü ������ ���� �� ���θ� �Ǵ��մϴ�.
            if(itemType == ItemType.None)
                isSlotIndexAll = true;
            else
                isSlotIndexAll = false;
                        
            indexList.Clear();       // ������ �ε��� ����Ʈ�� ����ϴ�
            int findSlotIdx = -1;    // ã�� ���� �ε����� �����ϰ� �ʱⰪ�� -1���� �����մϴ�.


            // �������� ItemType�� ������� �ش��ϴ� ��ųʸ��� ���մϴ�
            Dictionary<string, List<GameObject>> itemDic;                   


            if(isSlotIndexAll)      // ��ü ���� �ε����� ������� ���ϴ� ���
            {
                int dicLen = (int)ItemType.None;    // ���� ������ �� ������ �����մϴ�.

                for(int i=0; i<dicLen; i++)               
                {
                    // �����ϴ� ��� ��ųʸ� ������ �ϳ��� �����ɴϴ�.
                    itemDic = GetItemDicIgnoreExsists((ItemType)i); 

                    // ������Ʈ�� �ϳ��� �о�鿩�� ������ �ε����� �ε��� ����Ʈ�� ����ֽ��ϴ�.
                    ReadSlotIdxToIndexList(indexList, itemDic, isSlotIndexAll);
                }  
            }
            else                    // ���� ���� �ε����� ������� ���ϴ� ���
            {
                // �ش� ������ ������ ������ ���մϴ�. 
                itemDic = GetItemDicIgnoreExsists(itemType);    

                // ������Ʈ�� �ϳ��� �о�鿩�� ������ �ε����� �ε��� ����Ʈ�� ����ֽ��ϴ�.
                ReadSlotIdxToIndexList(indexList, itemDic, isSlotIndexAll);
            }

            indexList.Sort();   // �ε��� ����Ʈ�� ������������ �����մϴ�.
            
            // �������� ������ �ش��ϴ� ������ ĭ ���� ���� ���մϴ�. (���ڷ� ������ itemType�� None�̶�� ��ü ���� ���� ���� ���մϴ�.)
            int slotCountLimit = GetItemSlotCountLimit(itemType); 

            // �ε��� ���ڱ��� i�� 0���� ������Ű�鼭 �� ������ ã���ϴ� 
            for(int i=0; i<slotCountLimit; i++)
            {                
                if(i > indexList.Count-1)      // i�� ������ �ε������� ũ�ٸ�, �� �̻� ������ ã�� �ʰ� i�� ��ȯ�մϴ�.
                    findSlotIdx = i;
                else if(indexList[i] != i)     // i��° �ε�������Ʈ�� ����� �ε����� i�� ��ġ���� �ʴ´ٸ� ������ �� ������ �Ǵ�   
                    findSlotIdx = i;                 
            }

            //ã�� ���� �ε����� ��ȯ�մϴ�.
            return findSlotIdx;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReadSlotIdxToIndexList(List<int> indexList, Dictionary<string, List<GameObject>> itemDic, bool isSlotIndexAll )
        {   
            // ��ųʸ��� �ƹ� �������� ������� �ʴٸ� �ٷ� �����մϴ�.
            if( itemDic==null || itemDic.Count==0)
                return;

            // �ش� �������� ������ ������ �ϳ��� �о�ɴϴ�.
            foreach(List<GameObject> itemObjList in itemDic.Values)     
            {
                foreach(GameObject itemObj in itemObjList)
                {
                    Item item = itemObj.GetComponent<ItemInfo>().Item;   

                    if( isSlotIndexAll )  
                        indexList.Add(item.SlotIndexAll);   // ��ü ���� �ε��� ������� ���ϴ� ���
                    else                
                        indexList.Add(item.SlotIndex);      // ���� ���� �ε��� ������� ���ϴ� ���
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