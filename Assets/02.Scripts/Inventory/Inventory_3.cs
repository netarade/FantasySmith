using ItemData;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
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
 * <v2.1 - 2024_0103_�ֿ���>
 * (�̽�_0103) 
 * 1- SetOverlapCount�޼��忡��
 * ������Ʈ����Ʈ���� �ϳ��� �������� �о�ͼ� ���� ������ �����Ű�� �ִµ�,
 * �κ��丮 ���� �� ������Ʈ ����Ʈ ������ �Ͼ�� ������ �ִ�.
 * => ������ �������� ��Ƽ� �ѹ��� ����Ʈ���� �����ϴ� ���·� �����ؾ�
 * 
 * 
 * (�̽�_0104)
 * 1- ������ �� �����ؼ� �����ؾ� �ϴµ� ���� �� ������Ʈ ������ �о���̰� �ִ�.
 * ������, ���� ���ӿ����� slotIndex������ �����ؾ� �ϹǷ�, slotIndex�� ������� �����ؼ� ��������� �Ѵ�.
 * 
 * <v3.0 - 2024_0104_�ֿ���>
 * 1- InventoryInfo�� ������ �����͸� �����ϱ� ���� passItemInfoList�� �����ϰ�
 * SetOverlapCount�޼��忡�� ���������� ������ �������� �Ͻ����� ��Ƽ� �����ϴ� �������� �����Ͽ���.
 * 
 * �޼��� ���ο��� �������� �ʰ� InfoŬ������ �����ϴ� ������ ���������� InventoryŬ������ ���������, 
 * ������Ʈ ������ InventoryInfoŬ�������� �Ѱž� �ϱ� �����̸�,
 * ������ �ٽ� �ǵ����� �۾��� �ʿ��� ���� �ֱ� ���� 
 * (ex. ��ȭ������ 50���� �Ծ��µ� ������ ������� �ʴٸ�, ������ �����ۿ� �� �������� �˾ƾ� �Ѵ�.)
 * 
 * 2- �������� ������ isLatestRemove, Reduce���� isLatestModify�� ����
 * 
 * 3- IsAbleToAddMisc�޼��� �߰�
 * IsEnough, IsExist ���� ���ҳ� ���Ÿ� �������� ������ 
 * �� �޼���� �߰� �ϱ����� ���� ������ ���� �������, �׸��� ���ο� �������� �����ϱ� ���� ���԰����� �ִ��� ���θ� ��ȯ�ޱ� ����.
 * 
 * 
 * <v3.1 - 2024_0104_�ֿ���>
 * 1- IsAbleToAddMisc�޼��忡�� ���ɿ��θ� ��ȯ�ؾ��ϴµ� ������Ʈ����Ʈ�� ���� �����ϰ��־��� �� ����
 * 2- ���ı��� �޼��� ������ CompareByIndex�� CompareBySlotIndexEach�� ����
 * 3- SetOverlapCount�޼��� while���� ���� ��������, ���������� �����ϴ� ������ ���� �޼��带 ���� for�� ȣ��� ����
 * 4- SetOverlapCount �޼����� �Ű����� passList�� removeList�� �����ϰ� ������ ���ڷ� ����
 * 
 * 
 * <v3.2 - 2024_0105_�ֿ���>
 * 1- FindNearstSlotIdx�ڵ� ���� ����
 * a. �������� �ƹ��͵� ������� ������ �ʱⰪ�� 0���� �ְ��Ͽ���.
 * b. for�� ���� break�� �߰� (Ÿ���ε����� ã���� ���̻� ���������� �ؾ���)
 * c. �ε��� ����Ʈ�� �������� ���������鼭 ������ ���� �����ִ� ���, ���� �ε����� Ÿ������ �����ϴ� ���� �߰�
 * 
 * <v3.3 - 2024_0108_�ֿ���>
 * 1- IsAbleToAddMisc�޼��� ���� �������� ���� �����ѵ��� �Ұ����� ��ȯ�ϴ� �ڵ带 ����
 * reamainCount>0�϶� ���԰����� �����Ҷ� false�� ��ȯ�ϰ�, remainCount<=0 ������ �� ������ true�� ��ȯ�Ͽ��� ������ false�� ��ȯ�Ͽ��� �� ����
 * �߰��� remainCount�� Ž�� ���� �� 0���Ϸ� �������ٸ� �ٷ� true�� ��ȯ�ϵ��� ����
 * 
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
        /// InventoryInfoŬ������ �ʿ� �� ������ ������ �����ϱ� ���� ����ϴ� ItemInfo�� �ӽ������� ��� ����Ʈ�Դϴ�.<br/>
        /// ���� ���۸���Ʈ �� ���� �� ��ü���ڷ� SetOverlapCount�޼��忡�� ���������� ���˴ϴ�.
        /// </summary>
        List<ItemInfo> tempInfoList = new List<ItemInfo>();


        /// <summary>
        /// ���� ����� ������ �ε����� ���մϴ�. � �̸��� �������� ���� �������� <br/>
        /// ������ Ȥ�� ��ü���� �ε����� ��ȯ�ް��� �ϴ��� ���θ� �����Ͽ��� �մϴ�. (�⺻��: ��ü��)<br/><br/>
        /// *** �̸��� �ش��ϴ� �������� ���� ������ �������� �ʴ� ��� ���ܰ� �߻��մϴ�.<br/>
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
                             
            // �ε��� ����Ʈ�� �������� �ƹ��͵� ���� ��� 0�� ��ȯ
            if(indexList.Count==0)
                return 0;

            // �������� ������ �ش��ϴ� ������ ĭ ���� ���� ���մϴ�. (���ڷ� ������ itemType�� None�̶�� ��ü ���� ���� ���� ���մϴ�.)
            int slotCountLimit = GetItemSlotCountLimit(itemType); 

            // ���� �ε��� ���� 
            // 0, 1, 2, 3, 4
            // 0, 1, 4, 6, 9

            // �ε��� ���ڱ��� i�� 0���� ������Ű�鼭 �� ������ ã���ϴ� 
            for(int i=0; i<slotCountLimit; i++)
            {
                // i��° �ε�������Ʈ�� ����� �ε����� i�� ��ġ���� �ʴ´ٸ� ������ �� ������ �Ǵ�
                if( indexList[i]!=i )
                {
                    findSlotIdx=i;
                    break;
                }
                //�ε��� ����Ʈ�� �������� ���������鼭 ������ ���� �����ִ� ���
                else if( i==indexList.Count-1 && i!=slotCountLimit-1 ) 
                {
                    findSlotIdx = i+1; // ���� �ε����� Ÿ������ ����
                    break;
                }
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
        /// ����Ʈ�� Sort�޼��忡 �ε��� ���ı����� �����ϱ� ���� ���� �޼���<br/>
        /// ��ȭ�������� ���� ���� �ε����� �������� �ؼ� ������������ ���ĵ˴ϴ�.<br/>
        /// </summary>
        public static int CompareBySlotIndexEach(GameObject itemObj1, GameObject itemObj2)
        {
            ItemMisc itemMisc1 = (ItemMisc)itemObj1.GetComponent<ItemInfo>().Item;
            ItemMisc itemMisc2 = (ItemMisc)itemObj2.GetComponent<ItemInfo>().Item;
            return itemMisc1.SlotIndex.CompareTo(itemMisc2.SlotIndex);
        }


        /// <summary>
        /// �κ��丮�� �����ϴ� �ش� �̸��� ������ ������ ������Ű�ų� ���ҽ�ŵ�ϴ�.<br/>
        /// ���ڷ� �ش� ������ �̸��� ����, ���� ��� �������� �������� ���� ���� ����Ʈ, ��������� �����ؾ� �մϴ�.<br/><br/>
        /// 
        /// ������ ������ ���ҽ�Ű���� ���� ���ڷ� ������ ����, ������Ű���� ����� �����մϴ�.<br/>
        /// �� �������� �ּ�, �ִ� ������ �����ϸ� ���� �������� ������ ������ �����մϴ�.<br/><br/>
        /// 
        /// ���� �������� ������ ���ҷ� ���� 0 �̵Ǹ� �������� �κ��丮 ��Ͽ��� �����ϰ� ���� ����Ʈ�� �������� �߰��մϴ�.<br/>
        /// ���� ���۸���Ʈ�� null�� �����ϸ�, �������� �������� �ʰ� ������Ʈ�� ��� �����մϴ�.(�⺻��:null)<br/>
        /// ���� ������ ���� �κ��丮 ��Ͽ��� �����ϰų�, ���۸���Ʈ�� �߰����� �ʽ��ϴ�.<br/><br/>
        /// 
        /// ��� ���� �������� �� �̻� ������ �������� ���ϴ� ���� ������ �ʰ� ������ ��ȯ�մϴ�.<br/>
        /// �ʰ� �������� ���� ������Ʈ ���ο� ������ ���� ���ҷ� ���� ���� ������Ʈ ������ �޼��� ȣ���ڿ��� ������ �ֽ��ϴ�.<br/><br/>
        /// �ֽ� ��, ������ ������ ���� ���� ����� ������ ���ֽ��ϴ�. (�⺻��: �ֽż�)<br/><br/>
        /// ** ������ �̸��� �ش� �κ��丮�� �������� �ʰų�, ��ȭ�������� �ƴ� ��� ���ܸ� �߻���ŵ�ϴ�. **
        /// </summary>
        /// <returns>������ ������Ʈ�� ���� Ȥ�� �߰��ϰ� ���� �ʰ� ����, ��� ���� �����ۿ� �ش� ������ ���ٸ� 0�� ��ȯ</returns>
        public int SetOverlapCount(string itemName, int inCount, bool isLatestModify=true, List<ItemInfo> removeList=null)
        {
            List<GameObject> itemObjList = GetItemObjectList(itemName);     // �κ��丮�� ������ ������Ʈ ����Ʈ ����
            ItemType itemType = GetItemTypeInExists(itemName);              // ������ ���� ����
                        
            // �ش� ������ �̸����� �κ��丮 ������ �������� �ʴ� ���, �������� ������ ��ȭ�������� �ƴ� ��� ����ó��
            if(itemObjList==null)
                throw new Exception("�������� �� �κ��丮�� �������� �ʽ��ϴ�.");
            else if(itemType != ItemType.Misc)
                throw new Exception("��ȭ �������� �ƴմϴ�.");
            

            bool isInstantRemove = false;       // ��� �������� �Ǵ� ����

            // ���ڷ� ���޹��� ���� ����Ʈ�� null�̶�� ��� ���� Ȱ��ȭ
            if(removeList==null)
            {
                isInstantRemove = true;       
                removeList = this.tempInfoList;   // �ӽø���Ʈ�� ����
            }

            int remainCount = inCount;          // ���� �Ǵ� �����ϰ� ���� ���� (�ʱⰪ : ���� ��������)
            
            // ���� ���� �ε����� ���������Ͽ� ������������ �����մϴ�.
            itemObjList.Sort(CompareBySlotIndexEach);


            // �ֽż����� ������ �� ������ ������ ������ ���� �Ǵ��Ͽ� ���� �޼��带 �ݺ�ȣ���մϴ�.            
            if(isLatestModify)  
            {
                for(int i=itemObjList.Count-1; i>=0; i--)
                    if( SetCountInLoopByOrder(i) ) { break; }   // ȣ�� ������ Ȱ��ȭ�Ǹ� �ݺ����� �����մϴ�.
            }
            else                
            {
                for(int i=0; i<=itemObjList.Count-1; i++)
                    if( SetCountInLoopByOrder(i) ) { break; }   // ȣ�� ������ Ȱ��ȭ�Ǹ� �ݺ����� �����մϴ�.
            }


            // ���� ����Ʈ�� �ִ� �������� �������� ��ȸ�մϴ�. 
            for( int i=tempInfoList.Count-1; i>=0; i--)
            {
                // �κ��丮 ����Ʈ���� ����
                RemoveItem(tempInfoList[i]);
                                
                // ��� ������ Ȱ��ȭ ���ִٸ� ������Ʈ ����
                if(isInstantRemove)
                    GameObject.Destroy(tempInfoList[i].gameObject);    
            }

            // ��� ������ ��� ���۸���Ʈ �ʱ�ȭ
            if(isInstantRemove)
                tempInfoList.Clear();   

            // ȣ���ϰ� ���� ������ ��ȯ�մϴ�.
            return remainCount; 
            



            // �������� ��ȸ ������ ���� �ϳ��� ������ ���� ������ �����ϰ�,
            // ������ 0�̵� �������� tempInfoList�� ����ִ� ���� �޼����Դϴ�.
            bool SetCountInLoopByOrder(int idx)
            {
                // �ݺ����� �� ������ ������ ���� ������ ���� ����
                ItemInfo itemInfo=itemObjList[idx].GetComponent<ItemInfo>();
                ItemMisc itemMisc=(ItemMisc)itemInfo.Item;

                // ���������� �־ ���ο� ���� ������ ��ȯ�޽��ϴ�.
                remainCount=itemMisc.SetOverlapCount( remainCount );

                // ������ ���� ������Ʈ�� ��ø �ؽ�Ʈ�� �����մϴ�.
                itemInfo.UpdateCountTxt();

                // ������ �������� ������ 0�� �� ��쿡�� ���� ����Ʈ�� ����ϴ�.
                // �ٷ� �������� �ʰ� ���� ��� ������ �߰��� ����Ʈ�� Count ������ ����� �����Դϴ�.
                if( itemMisc.OverlapCount==0 )
                    tempInfoList.Add( itemInfo );

                // ���� ������ 0�̵� ��� �ܺ� ȣ�� ������ Ȱ��ȭ�մϴ�.
                if(remainCount==0)  
                    return true;
                else
                    return false;
            }

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
        public bool IsEnoughOverlapCount(string itemName, int overlapCount=1, bool isReduce=false, bool isLatestModify=true)
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
                    SetOverlapCount(itemName, -overlapCount, isLatestModify, null);

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
        public bool IsExist(string itemName, bool isRemove=false, bool isLatestModify=true)
        {
            List<GameObject> itemObjList = GetItemObjectList(itemName);     // �κ��丮�� ������ ������Ʈ ����Ʈ ����
           
            if( itemObjList==null ) // �κ��丮�� ������Ʈ ����Ʈ�� �������� �ʴ� ���
            {
                return false;
            }           
            else                    // ������Ʈ ����Ʈ�� �����ϴ� ���
            {
                if(isRemove)        // ���� ����� ���
                    RemoveItem(itemName, isLatestModify);

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
        public bool IsEnough(ItemPair pair, bool isReduce=false, bool isLatestModify=true)
        {
            ItemType itemType = GetItemTypeInExists(pair.itemName);
            
            //��ȭ�������� ��� - �����˻� �� �������Ҹ޼��� ȣ��, ��ȭ�������� �ƴ� ��� - ���翩�� �� ���Ÿ޼��� ȣ��
            if( itemType==ItemType.Misc )   
                return IsEnoughOverlapCount( pair.itemName, pair.overlapCount, isReduce, isLatestModify );            
            else
                return IsExist(pair.itemName, isReduce, isLatestModify);
        }


        /// <summary>
        /// �������� ������ ������� �������� �κ��丮�� �����ϴ���, ��ȭ�������̶�� ���������� ������� ���θ� ��ȯ�մϴ�.<br/>
        /// ���� �������� �����ϰų�, ��ȭ�������� ��� �������� ����ϴٸ� ���� �Ǵ� ���Ҹ� ������ �� �ֽ��ϴ�.<br/>
        /// (�⺻��: ���Ҹ�� ����, �ֽż� ����)<br/><br/>
        /// *** ����ȭ �������� ��� ���� ���� �����մϴ�. ��ȭ�������� ��� ������ 1�̻��� �ƴϸ� ���ܸ� �߻���ŵ�ϴ�. ***
        /// </summary>
        /// <returns>���� �� ������ ��� ������ �����ϴ� ��� true��, ������ �������� �ʴ� ��� false�� ��ȯ, ������ ������ ��� ���Ҹ� ����</returns>
        public bool IsEnough( ItemPair[] pairs, bool isReduce=false, bool isLatestModify=true)
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
                        IsEnough(pair, isReduce, isLatestModify);
                }

                return true;
            }
            // �ϳ��� ������ �������� �ʴ� ��� ���и� ��ȯ
            else
                return false;

        }

        


        /// <summary>
        /// ��ȭ �������� �ش� ���� ��ŭ �����Ѵٰ� ������ ��, �κ��丮�� ���� ��������� ��ȯ���ִ� �޼����Դϴ�.<br/>
        /// ������ �̸��� ������ ������ �־�� �մϴ�.<br/><br/>
        /// ���� ������ ������Ʈ�� �ִ� ���� GetCurRemainSlotCount�� ȣ���ϴ� ���� ���������� �����ϴ�.<br/><br/>
        /// *** ��ȭ �������� �ƴϰų�, �����������ڰ� 0���ϸ� ���ܸ� �����ϴ�. ***
        /// </summary>
        /// <returns>�������� �����ϱ� ���� ������ ����ϴٸ� true��, �����ϴٸ� false�� ��ȯ</returns>
        public bool IsAbleToAddMisc( string itemName, int overlapCount )
        {
            ItemType itemType = GetItemTypeIgnoreExists( itemName );

            // �ش� �������� ������ ��ȭ�������� �ƴ� ���� ������ �߸� ���� �� ��� ����ó��
            if( itemType!=ItemType.Misc )
                throw new Exception( "�ش� �̸� �������� ������ ��ȭ �������� �ƴմϴ�." );
            if( overlapCount<=0 )
                throw new Exception( "���� �������ڴ� 1�̻��̾�� �մϴ�." );

            // ���� ������ ���ʿ� ���� �������� ����
            int remainCount = overlapCount;

            // ������ �� �ִ���� ����
            int maxOverlapCount = GetItemMaxOverlapCount( itemName );

            // ���� ������ �������� �� �ִ� ���� ����
            int curRemainSlotCnt = GetCurRemainSlotCount( itemType );


            // ������Ʈ ����Ʈ ������ �����մϴ�.
            List<GameObject> itemObjList = GetItemObjectList( itemName );

            // ���� ������Ʈ ����Ʈ�� �ִ� ����
            if( itemObjList!=null )
            { 
                // �������� �ϳ��� ������ remainCount�� �����մϴ�.
                foreach( GameObject itemObj in itemObjList )
                {
                    ItemMisc itemMisc = (ItemMisc)itemObj.GetComponent<ItemInfo>().Item;

                    // �ش� �������� �ִ�������� ����� ���� ������ �� ���Ҵ� ���� ����մϴ�.
                    remainCount-=( maxOverlapCount-itemMisc.OverlapCount );

                    // ���� ������ ���� �����ۿ� ���԰��� �ϴٸ�, 
                    // ������Ʈ�� ���� ������ �ʿ� �����Ƿ� �� �̻� Ž���� �ߴ��ϰ� ������ ��ȯ�մϴ�.
                    if(remainCount<=0)
                        return true;
                }
            }
            

            
            // ������Ʈ ����Ʈ�� �ƿ� ���°���� �״���� ���� ���� �Ǵ�
            // ���� ������Ʈ ����Ʈ���� ���� ��Ų ��ŭ�� ���� ������ 0�̻��̶��,


            // ���� �� ������Ʈ ���� ���� : ���������� ���� �������� ������Ʈ 1���� �ִ�������� ���� �� (������ �������� ���)
            int createCnt = remainCount / maxOverlapCount;
            int remainder = remainCount % maxOverlapCount;

            // ������ �������� �ʾ�, ���� �Ҽ����� ���� ���� ������Ʈ�� �ϳ� �� �ʿ��ϹǷ� ���� ������ �ø��ϴ�. 
            if(remainder>0)
                createCnt++;


            // ���� ���� ������ �����ؾ� �� ������Ʈ �������� ���ų� ���ٸ� �������� ��ȯ
            if( curRemainSlotCnt >= createCnt )
                 return true; 
            // ���� ���� ������ �����ϴٸ�, ���� �Ұ��� ��ȯ
            else
                return false;
        }

                

        



    }

    

}