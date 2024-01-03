using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/*
 * <v1.0 - 2023_1231_�ֿ���>
 * 1- FindNearstSlotIdx itemName�� itemType�� ���ڷ� �޴� �����ε� �޼��� ����
 * 
 * <v1.1 - 2024_0102_�ֿ���>
 * 1- ������ ���� �˻���� 
 * IsEnoughOverlapCount �޼��� �ۼ�
 * 
 * 2- ������ ���� ���� �� ��Ͽ��� ���� ��� 
 * IsEnoughOverlapCount �޼��忡 �������Ҹ�� �߰� �� SetOverlapCount �޼��� �ۼ�
 * 
 * (���� ���� ����_0102)
 * 1- �̸��� ������ ����ü ���·� ���� �迭�� �����ϴ� �����ε� �޼��� �����ϱ�
 * 
 * 
 * <v2.0 - 2024_0103_�ֿ���>
 * 1- IsEnoughOverlapCount�޼��� �����������ڿ� ���� ����ó�� �߰� 
 *  
 * 2- IsExist �߰�
 * ����ȭ �������� �����ϴ��� ���θ� ��ȯ�ϴ� �޼���, �������� ����
 * 
 * 3- ItemPair ����ü �߰�
 * ���ڷ� �̸��� ���� ���� ������ �� �ֵ��� ����
 * 
 * 4- ItemPair�� ItemPair�迭�� ���ڷ� �޴� IsEnough�޼��� ����
 * ���ڷ� ������ �������� �ִ���, �������� ��������� �˻��ϰ� �������� �Ǵ� ������ ���ֵ��� ����
 * 
 * (�̽�)
 * 1- IsEnough, IsEnoughOverlapCount, IsExist �޼����� ��ȯ���� ItemInfo�� �����ؾ��Ѵ�
 * ������ ������ 0�̵� ������ ������Ʈ�� InfoŬ�������� ����ó���ؾ� �ϱ� ����
 * 
 * 
 * 
 */


namespace InventoryManagement
{

    /// <summary>
    /// ������ �̸��� ������ ��Ÿ���� ����ü�Դϴ�.<br/>
    /// �κ��丮 �޼����� �������ڷ� ���Ǹ�,<br/>
    /// �ش� �������� �̸��� �κ��丮�� �����ϴ� ��, ������ ������ ����� �� Ȯ���ϴ� �뵵�� ���˴ϴ�.
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
        












        /// <summary>
        /// �κ��丮�� �����ϴ� �ش� �̸��� ������ ������ ������Ű�ų� ���ҽ�ŵ�ϴ�.<br/>
        /// ���ڷ� �ش� ������ �̸��� ������ �����ؾ� �մϴ�.<br/><br/>
        /// ������ ������ ���ҽ�Ű���� ���� ���ڷ� ������ �����Ͽ��� �ϸ�,<br/>
        /// ���� ������ ���ҷ� ���� 0�̵Ǹ� �������� �κ��丮 ��Ͽ��� �����ϰ� �ı��ؾ� �մϴ�.<br/><br/>
        /// 
        /// ������ ������ ������Ű���� ���� ���ڷ� ����� �����Ͽ��� �ϸ�,<br/>
        /// ������ �ִ� ���� �������� ���� �� �̻� ������ ������Ű�� ���ϴ� ���� ������ �ʰ� ������ ��ȯ�մϴ�.<br/><br/>
        /// �ֽ� ��, ������ ������ ���ҿ��θ� ������ ���ֽ��ϴ�. (�⺻��: �ֽż�)<br/><br/>
        /// ** ������ �̸��� �ش� �κ��丮�� �������� �ʰų�, ��ȭ�������� �ƴ� ��� ���ܸ� �߻���ŵ�ϴ�. **
        /// </summary>
        /// <returns></returns>
        public int SetOverlapCount(string itemName, int inCount, bool isLatestReduce=true)
        {
            List<GameObject> itemObjList = GetItemObjectList(itemName);     // �κ��丮�� ������ ������Ʈ ����Ʈ ����
            ItemType itemType = GetItemTypeInExists(itemName);              // ������ ���� ����

            
            // �ش� ������ �̸����� �κ��丮 ������ �������� �ʴ� ���, �������� ������ ��ȭ�������� �ƴ� ��� ����ó��
            if(itemObjList==null)
                throw new Exception("�������� �� �κ��丮�� �������� �ʽ��ϴ�.");
            else if(itemType != ItemType.Misc)
                throw new Exception("��ȭ �������� �ƴմϴ�.");

            ItemInfo itemInfo;            // ������ ���� ����
            int remainCount = inCount;    // ���� �Ǵ� �����ϰ� ���� ���� (�ʱⰪ : ���� ��������)
                                    

            // �ֽż� ����
            if( isLatestReduce )
            {
                for( int i = itemObjList.Count-1; i>=0; i-- )   // ������Ʈ ����Ʈ�� ������ �ε������� ������Ʈ�� �ϳ��� ������ �ɴϴ�.
                {
                    itemInfo=itemObjList[i].GetComponent<ItemInfo>();
                    remainCount=itemInfo.SetOverlapCount( remainCount );    // ���������� �־ ���ο� ���� ������ ��ȯ�޽��ϴ�.

                    if( remainCount==0 )        // for���� ������ ���� reaminCount�� 0�� �� ���, ���� ������ 0�� �Ǿ����� ��ȯ
                        return 0;
                }
            }
            // �����ȼ� ���� 
            else                
            { 
                for( int i=0; i<=itemObjList.Count-1; i++ )     // ������Ʈ ����Ʈ�� ������ �ε������� ������Ʈ�� �ϳ��� ������ �ɴϴ�.
                {                    
                    itemInfo=itemObjList[i].GetComponent<ItemInfo>();
                    remainCount=itemInfo.SetOverlapCount( remainCount );    // ���������� �־ ���ο� ���� ������ ��ȯ�޽��ϴ�.

                    if( remainCount==0 )        // for���� ������ ���� reaminCount�� 0�� �� ���, ���� ������ 0�� �Ǿ����� ��ȯ
                        return 0;
                }
            }

            return remainCount;     // for���� ���� �Ŀ��� remainCount�� 0�� ���� ���� ���, ���� ������ ��ȯ
        }

        

        /// <summary>
        /// �κ��丮�� �ش� �̸��� �������� ��ø������ ��������� Ȯ���ϴ� �޼����Դϴ�.<br/>
        /// ���ڷ� ������ �̸��� ������ �ʿ��մϴ�. (���� ������ �� �⺻ ������ 1���Դϴ�.)<br/><br/>
        /// ���� ° ���ڷ� ���� ���Ҹ�带 �����ϸ� �������� ��ø������ ����ϴٸ� ���ڷ� ���� ������ŭ ���ҽ�Ű��, <br/>
        /// 0�� ������ ��� �������� �κ��丮���� �����ϰ� �ı� ��ŵ�ϴ�.<br/>
        /// �ֽ� ��, ������ ������ ���ҿ��θ� ������ ���ֽ��ϴ�. (�⺻��: �ֽż�)<br/><br/>
        /// ** �ش� �̸��� �������� �������� �ʰų�, ��ȭ �������� �ƴϰų�, ������ �߸� �����ߴٸ� ���ܸ� �߻���ŵ�ϴ�. **
        /// </summary>
        /// <returns>������ ��ø������ ����ϸ� true��, ������� ������ false�� ��ȯ</returns>
        public bool IsEnoughOverlapCount(string itemName, int overlapCount=1, bool isReduce=false, bool isLatestReduce=true)
        {           
            ItemType itemType = GetItemTypeInExists(itemName);              // �̸��� ���� �������� ���� ����
            List<GameObject> itemObjList = GetItemObjectList(itemName);     // �̸��� ���� ������ ������Ʈ ����Ʈ ����

            // �ش� ������ �̸����� �κ��丮 ������ �������� �ʴ� ���, �������� ������ ��ȭ�������� �ƴ� ��� ����ó��
            if( itemObjList == null )
                throw new Exception("�ش� �������� �κ��丮�� �������� �ʽ��ϴ�.");
            else if( itemType != ItemType.Misc)
                throw new Exception("�ش� �������� ��ȭ�������� �ƴմϴ�.");
            else if( overlapCount<=0 )
                throw new Exception("���� �������ڴ� 1�̻��̾�� �մϴ�.");
                                    
                                    
            int totalCount = 0;                         // ��ø������ ���� ��Ű�� ���� ���� ����
            bool isTotalEnough = false;                 // ��ø������ ������� Ȯ���ϱ� ���� ����

            foreach(GameObject itemObj in itemObjList)  // �������� �ϳ��� ������ ������ �н��ϴ�.
            {
                ItemMisc itemMisc = (ItemMisc)itemObj.GetComponent<ItemInfo>().Item;
                totalCount += itemMisc.OverlapCount;    // ��ø������ ������ŵ�ϴ�.
                   
                // �հ� ���� ������ ���� ������ ���ڷ� ��� �� �������� ū�� Ȯ���մϴ�.
                if(totalCount >= overlapCount)  
                {
                    isTotalEnough = true;    // isTotalEnough�� true�� ����� ���������ϴ�.
                    break;
                }
            }

            if(isTotalEnough)       // �հ� ���� ������ ���ڷ� ���� ������ �ʰ��ϴ� ���
            {
                if( isReduce )      // ���� �����, ��� ���θ� ��ȯ�ϱ� ���� �ش� ������ŭ ���ҽ�ŵ�ϴ�.
                    SetOverlapCount(itemName, -overlapCount, isLatestReduce);

                    return true;
            }
            // �հ� ���� ������ ���ڷ� ���� ������ �ʰ����� ���ϴ� ��� ���и� ��ȯ�մϴ�.
            else
                return false;
        }



        /// <summary>
        /// �������� �κ��丮�� �����ϴ��� ���θ� ��ȯ�մϴ�.<br/>
        /// �ֽ� ��, ������ ������ �������θ� ������ ���ֽ��ϴ�. (�⺻��: ��������, �ֽż�)<br/><br/>
        /// *** �������� ������ ������ ������� ������Ʈ ������ �����ϴ��� ���θ� ��ȯ�մϴ�. ***<br/>
        /// </summary>
        /// <returns>�������� �������� �ʴ� ��� false��, �������� �����ϰų� ������ ������ ��� true�� ��ȯ</returns>
        public bool IsExist(string itemName, bool isRemove=false, bool isLatestRemove=true)
        {
            List<GameObject> itemObjList = GetItemObjectList(itemName);     // �κ��丮�� ������ ������Ʈ ����Ʈ ����
           
            if( itemObjList==null ) // �κ��丮�� ������Ʈ ����Ʈ�� �������� �ʴ� ���
            {
                return false;
            }           
            else                    // ������Ʈ ����Ʈ�� �����ϴ� ���
            {
                if(isRemove)        // ���� ����� ���
                    RemoveItem(itemName, isLatestRemove);

                return true;
            }
        }


            
        


        /// <summary>
        /// �������� ������ ������� �������� �κ��丮�� �����ϴ���, ��ȭ�������̶�� ���������� ������� ���θ� ��ȯ�մϴ�.<br/>
        /// ���� �������� �����ϰų�, ��ȭ�������� ��� �������� ����ϴٸ� ���� �Ǵ� ���Ҹ� ������ �� �ֽ��ϴ�.<br/>
        /// (�⺻��: ���Ҹ�� ����, �ֽż� ����)<br/><br/>
        /// *** ����ȭ �������� ��� ���� ���� �����մϴ�. ��ȭ�������� ��� ������ 1�̻��� �ƴϸ� ���ܸ� �߻���ŵ�ϴ�. ***
        /// </summary>
        /// <returns>���� �� ������ ��� ������ �����ϴ� ��� true��, ������ �������� �ʴ� ��� false�� ��ȯ, ������ ������ ��� ���Ҹ� ����</returns>
        public bool IsEnough(ItemPair pair, bool isReduce=false, bool isLatestReduce=true)
        {
            ItemType itemType = GetItemTypeInExists(pair.itemName);
            
            //��ȭ�������� ��� - �����˻� �� �������Ҹ޼��� ȣ��, ��ȭ�������� �ƴ� ��� - ���翩�� �� ���Ÿ޼��� ȣ��
            if( itemType==ItemType.Misc )   
                return IsEnoughOverlapCount( pair.itemName, pair.overlapCount, isReduce, isLatestReduce );            
            else
                return IsExist(pair.itemName, isReduce, isLatestReduce);
        }


        /// <summary>
        /// �������� ������ ������� �������� �κ��丮�� �����ϴ���, ��ȭ�������̶�� ���������� ������� ���θ� ��ȯ�մϴ�.<br/>
        /// ���� �������� �����ϰų�, ��ȭ�������� ��� �������� ����ϴٸ� ���� �Ǵ� ���Ҹ� ������ �� �ֽ��ϴ�.<br/>
        /// (�⺻��: ���Ҹ�� ����, �ֽż� ����)<br/><br/>
        /// *** ����ȭ �������� ��� ���� ���� �����մϴ�. ��ȭ�������� ��� ������ 1�̻��� �ƴϸ� ���ܸ� �߻���ŵ�ϴ�. ***
        /// </summary>
        /// <returns>���� �� ������ ��� ������ �����ϴ� ��� true��, ������ �������� �ʴ� ��� false�� ��ȯ, ������ ������ ��� ���Ҹ� ����</returns>
        public bool IsEnough( ItemPair[] pairs, bool isReduce=false, bool isLatestReduce=true)
        {
            int allEnough=0;
                        
            // ��� �������� ������ �����ϴ� �� Ȯ��
            foreach(ItemPair pair in pairs )
            {
                if( IsEnough(pair) )
                    allEnough++;
            }
                                    

            // ��� ������ �����ϴ� ���
            if(allEnough==pairs.Length)
            {
                // ���Ҹ���� ���
                if(isReduce)
                {
                    foreach(ItemPair pair in pairs )
                        IsEnough(pair, isReduce, isLatestReduce);
                }

                return true;
            }
            // �ϳ��� ������ �������� �ʴ� ��� ���и� ��ȯ
            else
                return false;

        }


                

        



    }

    

}