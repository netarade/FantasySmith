using ItemData;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using WorldItemData;

/*
 * [�۾� ����]  
 * <v1.0 - 2023_1121_�ֿ���>
 * 1- �ʱ� ���� Ŭ���� ����
 * AddItem�޼��� �߰� - CreateManager�� CreateItemToNearstSlot�޼��� ȣ�� ���
 * 
 * <v1.1 -2023_1121_�ֿ���>
 * 1- ����Ŭ������ ����鼭 MonoBehaviour�� ������ �ʾ� new Ű���� ��� �߻��Ͽ� ����.
 * 
 * <v1.2 - 2023_1122_�ֿ���>
 * 1- �κ��丮�� �ε�Ǿ��� �� ������ ������Ʈ�� ����ġ�� ��ų �� �ֵ��� UpdateAllItem�޼��� �߰�
 * 
 * <v2.0 - 2023_1226_�ֿ���>
 * 1- UpdateAllItem�޼��带 UpdateAllItemInfo�޼���� ����
 * 2- LoadAllItem �ּ�ó�� 
 * - ���� ������ ����ȭ���� �� �� �ҷ������ٸ� �ش� �޼��� ���� ����
 * 
 * <v2.1 - 2023_1227_�ֿ���>
 * 1- Item ������ �������� �������� ����(3D ������Ʈ ���� 2D������Ʈ) 
 * UpdateAllItemInfo�޼����� ItemInfo�� �����ϴ� GetComponent�޼��带 GetComponentInChildren���� ����
 * 
 * <v2.2 - 2023_1229_�ֿ���>
 * 1- ���ӽ����̽��� CraftData���� InventoryManagement�� ����
 * 2- ���ϸ� ���� Inventory_p2-> Inventory_2
 * 
 * <v2.3 - 2023_1230_�ֿ���>
 * 1- CreateItem�޼��� �߰�
 * AddItem�޼����� ������ �ű�.
 * 
 * <v3.0 - 2023_1231_�ֿ���>
 * 1- CreateItem�޼��� ���� �ϰ� AddItem�޼��忡�� ItemInfo �Ǵ� GameObject�� ���ڷ� ���޹޾Ƽ� �ش� ��ųʸ��� �ֵ��� ����
 * 
 * 2- AddItem, RemoveItem, SetOverlapCount ���ں� �����ε� ����
 * 
 * 3- �پ��� ������ ���� �޼��� ����
 * IsContainsItemName - ������ �̸����� �ش� �κ��丮�� �����ϴ��� 
 * 
 * GetItemDicInExists - ���� �κ��丮 ��Ͽ� �̹� �����ϴ� �������� ���� ��ųʸ��� ��ȯ
 * GetItemDicIgnoreExists - ���� �κ��丮�� ��Ͽ� �������� �ʴ� �������� ���� ��ųʸ��� ��ȯ
 * 
 * GetItemTypeInExists - ��ųʸ��� �����ϴ� �������� Ÿ�Թ�ȯ
 * GetItemTypeIgnoreExists - ���� ��ųʸ��� �����ϴ� �������� Ÿ�� ��ȯ
 * GetItemObjectList - ��ųʸ����� ������Ʈ ����Ʈ�� ��ȯ
 * 
 * 
 * <v3.1 - 2024_0102_�ֿ���>
 * 1- AddItem�޼��� string, ItemŬ���� ��� �����ε� �޼��� ����
 * ������ �̸� �����Ǿ��ִ� ������Ʈ�� �ƴ� ��� ������ ���� ������Ʈ �޼��� ȣ���� ����ϱ� ������
 * �ᱹ InventoryInfo�޼��忡�� �ϼ��ؾ� �� ����̹Ƿ� �̹� �����Ǿ��ִ� ������Ʈ�� ��ũ��Ʈ�� �ִ� �޼��常 �ִ°��� ���ٰ� �Ǵ�.
 * 
 * (Remove�޼����� �����ε��� ���δ� ������ string������� �˻��� �����̱� ������, ������ �����ε��� InfoŬ�������� �״�� ���� ����)
 * 
 * 2- AddItem GameObject���ڸ� �޴� �޼��� ����ó�� �ۼ�
 * 
 * 3- SetOverlapCount�޼��� ���� �� ���� ��� Inventory_3.cs�� ����
 * 
 * 4- AddItem �� RemoveItem�� ����ó�� ���� �߰�
 * 5- RemoveItem GameObject�� ItemInfo ���� �޼��带 Item���� �޼��� ȣ�⿡�� �ٷ� string �޼���� ȣ��ǵ��� ���� 
 * 
 * <v3.2 - 2024_0102_�ֿ���>
 * 1- ItemType enum�� int������ ��ȯ�޴� �޼����� GetItemTypeIndexIgnoreExists �޼��带 �߰�
 * 
 * 
 * 
 * 
 * [�߰� ����]
 * 1- AddItem �޼���� �������� ������ ������ ������Ʈ�� �ܺ� InventoryInfo��ũ��Ʈ���� ����� ��.
 * (������ ������Ʈ�� ����Ȱ��ȭ �Ǳ������� �س��� �Ѵ�.)
 * 
 * 2- AddItem �޼���� �κ��丮�� ���� ��, ���� ���� �ε����� ��ü ���� �ε����� �Ѵ� ã�Ƽ� �־���� �Ѵ�.
 * (CreateManager�� �����ʿ�)
 * 
 * 
 * <v4.0 - 2024_0103_�ֿ���>
 * 1- RemoveItem�޼����� ��ȯ���� bool�� �ƴ϶� GameObject�� ����
 * ������ InventoryŬ���������� ������Ʈ�� �ı� ó���� ���ϱ� ������ InfoŬ������ �������� �Ѱܾ� �ϱ� ����
 * 
 * 2- GameObject, Item Ŭ������ ���ڷ� �޴� �����ε� �޼��� ����
 * �ڵ尡 �������� ������� ������ �ʿ� �� �߰��� ����
 * 
 * 3- AddItem�޼��� �ּ� ����
 * 
 * <v4.1 - 2024_0103_�ֿ���>
 * 1- RemoveItem���� ItemInfo�� �޴� �޼���, isLatest�� �޴� ���ڸ� �����ϰ�, �˻� ���� ����� �ƴ� �������� ������� ����
 * ������ ���� �������� �ƹ����̳� �����ϴ� ���� �ƴ϶� �������� ������ ������ �� Ư���������� ��Ȯ�� ��Ͽ��� �����ؾ� �ϴ� ��찡 ����� ���� 
 * 
 * 2- GetItemObjectList�޼��� ���ο� isNewIfNotExist ���� �ɼ� �߰�
 * ������ ������Ʈ����Ʈ�� �κ��丮 ���� ������ �������� �ʾƵ� ���Ӱ� ���� �� �߰��Ͽ� ��ȯ�� �����ϵ��� ���� 
 * 
 * 3- UpdateAllItemInfo �޼��带 InventoryInfoŬ������ �ű�.
 * InventoryInfo Ŭ�������� �κ��丮 �ε�� ����Ͽ�����, ����ó������ �ܺ�ó���� ������
 * �κ��丮�� �����ϴ� ��� �������� ������� OnItemCreated�޼��带 ȣ���Ͽ��� �ϳ� �������ڷ� InventoryInfo ������ �����ؾ� �ϱ� ����
 * 
 * 4- GetItemMaxOverlapCount�޼��� ����
 * ���� ���������κ��� ������ �޾Ƽ� ������ �� ������ �ִ� ��ø��� ������ ��ȯ�ϴ� �뵵 
 * (�ű� ������ �߰� �� ��������� ������Ʈ�� �߰��ؾ��ϴ� �� �˱� ���� ������Ʈ�� �ƿ� ���� ��쿡 ������ ���ϰ� �ǹǷ� �ʿ�)
 * 
 * 
 * <v4.2 - 2024_0104_�ֿ���>
 * 1- AddItemToDic ���ο� SetCurItemObjCount�޼��幮�� ����
 * 
 * 
 */

namespace InventoryManagement
{    
    public partial class Inventory
    {
        

        /// <summary>
        /// ItemInfo ������Ʈ�� ���ڷ� ���޹޾� �ش� ������ ������Ʈ�� �ش��ϴ� ������ ã�Ƽ� �������� �־��ִ� �޼����Դϴ�.<br/><br/>
        /// *** ���ڷ� ���� ������Ʈ �������� ���ٸ� ���ܸ� �߻���ŵ�ϴ�. ***<br/>
        /// </summary>
        /// <returns>������ �߰� ������ true��, ���� �κ��丮�� �� ������ �����ϴٸ� false�� ��ȯ�մϴ�</returns>
        public bool AddItem(ItemInfo itemInfo)
        {   
            if(itemInfo==null)
                throw new Exception("������ ��ũ��Ʈ�� �������� �ʴ� ������Ʈ�Դϴ�. Ȯ���Ͽ� �ּ���.");

            // ������ Ÿ���� Ȯ���մϴ�.
            ItemType itemType = itemInfo.Item.Type;

            // ��ȭ �����۰� ����ȭ ���������� �����Ͽ� �޼��带 ȣ���մϴ�.
            switch(itemType)
            {
                // Ÿ���� �Һи��� ��� ����ó��
                case ItemType.None:
                    throw new Exception("�ش� �������� ������ ��Ȯ���� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");

                case ItemType.Misc:
                    return AddItemMisc(itemInfo);   // ������ �߰� �޼��带 ȣ���մϴ�.

                default:
                    return AddItemToDic(itemInfo);  // �ٷ� ��ųʸ��� �߰��մϴ�.
            }            
        }
                
  
        /// <summary>
        /// �߰� �� ItemInfo ������Ʈ�� ���ڷ� ���޹޾� �ش� ������ ������ ������Ʈ�� �־��ִ� �޼����Դϴ�.<br/>
        /// �ش� ������ ������ 1���� �� �� ������ �־�� �մϴ�.
        /// </summary>
        /// <returns>�ش� ������ ������ �������� �� �� ������ ���ٸ� false, ������ �߰��� ���� �� true�� ��ȯ�մϴ�.</returns>
        public bool AddItemToDic(ItemInfo itemInfo)
        {          
            // ������ �̸��� ������ �����մϴ�.
            string itemName = itemInfo.Item.Name;
            ItemType itemType = itemInfo.Item.Type;

            // ���� ������ ���ٸ� ���и� ��ȯ�մϴ�.
            if( GetCurRemainSlotCount(itemType) == 0 )            
                return false;   

            
            // ������ Ÿ���� ������� ��ųʸ��� �����մϴ�.
            Dictionary<string, List<GameObject>> itemDic = GetItemDicIgnoreExsists(itemType);

            if( !itemDic.ContainsKey(itemName) )            // �ش� ������ ������Ʈ ����Ʈ�� �������� �ʴ� ���
            {
                List<GameObject> itemObjList = new List<GameObject>();  // �ű� ������Ʈ ����Ʈ�� ����ϴ�
                itemObjList.Add(itemInfo.gameObject);                   // �ű� ������Ʈ ����Ʈ�� ������ ������Ʈ�� �߰��մϴ�
                itemDic.Add(itemName, itemObjList);                     // ������ �ű� ������Ʈ ����Ʈ�� �߰��մϴ�.
            }
            else                                            // �ش� ������ ������Ʈ ����Ʈ�� �����ϴ� ��� 
            {
                itemDic[itemName].Add(itemInfo.gameObject); // ���� ������Ʈ ����Ʈ�� �����Ͽ� ���ӿ�����Ʈ�� �ֽ��ϴ�.
            }            
            
            SetCurItemObjCount(itemType, 1);                // �ش� ������ ������ ���� ������Ʈ�� ������ ������ŵ�ϴ�.
            return true;                                    // ������ ��ȯ�մϴ�.
        }

        

        /// <summary>
        /// ��ȭ ������ ������Ʈ�� ������ �κ��丮�� �߰��Ͽ� �ݴϴ�.<br/>
        /// ������ ������ ��ȭ�������� �ִ� ������ ä������ �ʾҴٸ�, ���� �������� ������ ���ҽ��� ���� �������� ������ ä��ϴ�.
        /// <br/><br/>
        /// ���� �������� ������ 0�� �Ǿ��ٸ� ������Ʈ�� �ı��ϰ�,<br/> 
        /// 0�̵��� �ʾҴٸ� �ش� �������� ���Ӱ� �κ��丮 ���Կ� �߰��մϴ�.<br/><br/>
        /// �⺻������ ���Կ��� �ռ� ������ ä������ �̸� ������ �� �ֽ��ϴ�. (�⺻��: �����ȼ�)<br/>
        /// </summary>
        /// <returns>��ȭ �������� �� ������ ���ٸ� false��, ������ �߰��� ������ ��� true�� ��ȯ�մϴ�.</returns>
        private bool AddItemMisc(ItemInfo itemInfo, bool isLatestModify=false)
        {
            // �������� �� ������ ���� ��� ���и� ��ȯ�մϴ�.
            if( GetCurRemainSlotCount(itemInfo.Item.Type) == 0 )
                return false;

            // ������ ������ �����Ͽ� ���������ۿ� ä��⸦ �����ϰ�, ���� ������ ��ȯ�޽��ϴ�.
            int afterCount = FillExistItemOverlapCount(itemInfo, isLatestModify);

            // ����ä��Ⱑ ����� �� 
            if( afterCount==0 )
                GameObject.Destroy( itemInfo.gameObject );  // ���� ������ 0�� �Ǿ��ٸ�, ���ڷ� ���޹��� ������Ʈ ����
            else
                AddItemToDic(itemInfo);                     // ���� ������ �����Ѵٸ�, �ش� �������� ������ �߰�

            return true;
        }


        /// <summary>
        /// ��ȭ �������� �κ��丮�� �߰��ϱ� ����,<br/>
        /// ������ �̸��� ���� ��ȭ �����ۿ� ������ ä���ְ�, ���� ������ ��ȯ�մϴ�.<br/>
        /// ���� ��ȭ�������� ���ٸ� ä���� ���ϰ� ���� ������ �״�� ��ȯ�մϴ�.<br/><br/>
        /// ���ڷ� ������ ItemInfo�� ������ ä�ŭ ���ҵǾ� ������,<br/>
        /// ������ 0�� �ɶ� ���� ä������ ������ ������ ���� �ʽ��ϴ�.<br/><br/>
        /// ���� ���� �� �ռ� �����ۺ��� ä���, ������ �� �ֽ��ϴ�.(�⺻��: �����ȼ�)<br/><br/>
        /// *** ������ �������� ��ȭ�������� �ƴ� ��� ���ܸ� �߻���ŵ�ϴ�. ***<br/>
        /// </summary>
        /// <returns>���� �����ۿ� ��� ������ �� ä�� ��� 0�� ��ȯ, �� ä���� ���� ��� ���� ������ ��ȯ</returns>
        public int FillExistItemOverlapCount(ItemInfo itemInfo, bool isLatestModify=false)
        {            
            if(itemInfo.Item.Type != ItemType.Misc)
                throw new Exception("���� ���� �������� ��ȭ �������� �ƴմϴ�. Ȯ���Ͽ� �ּ���.");

            ItemMisc newItemMisc = (ItemMisc)itemInfo.Item;

            // ������Ʈ ����Ʈ ������ �����մϴ�.
            List<GameObject> itemObjList = GetItemObjectList( newItemMisc.Name );                       

            // ���� �����ۿ� ����ä��� ���� �� �ĸ� ���� ������ ���� �������� �������� �����մϴ�.
            int beforeCount = newItemMisc.OverlapCount; 
            int afterCount = newItemMisc.OverlapCount;

            // ���� ������Ʈ ����Ʈ�� �ִ� ����
            if( itemObjList!=null )
            { 
                // �������� ������ �����մϴ�.
                itemObjList.Sort(CompareBySlotIndexEach);

                // ���� ������ ���� �� �������� ���� ���ؿ� ���� �ϳ��� ������ ���� �����ۿ� ����ä��⸦ �����մϴ�.
                if( isLatestModify )
                {
                    for( int i = itemObjList.Count-1; i>=0; i-- )
                        if( FillCountInLoopByOrder(i) ) { break; } // ���������� true�� ��ȯ�ϸ� ���������ϴ�.                                     
                }
                else
                {
                    for( int i = 0; i<=itemObjList.Count-1; i++ )   
                        if( FillCountInLoopByOrder(i) ) { break; } // ���������� true�� ��ȯ�ϸ� ���������ϴ�.  
                }
            }

            // ä���ְ� ���� ������ ��ȯ�մϴ�.
            return afterCount;

            



            // ���� �����ۿ� ���� ������ �ٸ��� �Ͽ� ä��� ���� �ݺ����� ���� �޼����Դϴ�.
            bool FillCountInLoopByOrder(int idx)
            {
                // ���� �����ۿ� �ε����� ���� ������ �Ͽ� ������ �ҷ��ɴϴ�.
                ItemMisc oldItemMisc = (ItemMisc)itemObjList[idx].GetComponent<ItemInfo>().Item;
                    
                // ���� �����ۿ� ���� ���������� �ִ� ���ġ���� ä��� ���� ������ ��ȯ�޽��ϴ�.
                afterCount = oldItemMisc.SetOverlapCount(beforeCount);

                // �پ�� ������ŭ �űԾ������� ������ ���ҽ��� �ݴϴ�.
                newItemMisc.SetOverlapCount(beforeCount-afterCount);
                    
                // ���� �ݺ����� ����ϱ� ���Ͽ� ��������� ��ġ���� �ݴϴ�. 
                beforeCount = afterCount;


                // ���� ������ 0�̵Ǹ� �ܺ� ȣ�� ������ Ȱ��ȭ�մϴ�.
                if(afterCount==0)
                    return true;
                else
                    return false;
            }
        }





        
        
        /// <summary>
        /// Ư�� �������� ItemInfo ������Ʈ�� ���ڷ� �޾Ƽ� �κ��丮�� ��ųʸ� ��Ͽ��� �������ִ� �޼��� �Դϴ�.<br/>
        /// �˻� ����� �ƴ϶� ���� �����Ͽ� ��Ͽ��� �����ϹǷ�, Ư�� �������� ���� �����ϴ� �뵵�� ����մϴ�.<br/>
        /// *** ���� ���� �������� ������ ���ܸ� ��ȯ�մϴ�. ***
        /// </summary>
        /// <returns>��ųʸ� ����� ���ſ� ������ ��� �ش� �������� ItemInfo ��������, ��Ͽ� ���� �������� ��� null�� ��ȯ�մϴ�.</returns>
        public ItemInfo RemoveItem(ItemInfo itemInfo)
        {
            // ���������� ����ó��
            if(itemInfo==null)
                throw new Exception("������ ��ũ��Ʈ�� �������� �ʴ� ������Ʈ�Դϴ�. Ȯ���Ͽ� �ּ���.");
            
            // �̸��� ������� �ش� ��ųʸ� ������ �޽��ϴ�.
            Dictionary<string, List<GameObject>> itemDic = GetItemDicInExists(itemInfo.Item.Name);            

            // �κ��丮 ��Ͽ��� ���� ������ ����ó��
            if(itemDic==null)
                throw new Exception("�ش� �������� �κ��丮 ���ο� �������� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");

            // List<GameObject>�� ���� GameObject �ν��Ͻ��� �����Ͽ� ��Ͽ��� �����մϴ�.
            itemDic[itemInfo.Item.Name].Remove(itemInfo.gameObject);

            // ��Ͽ��� ������ �������� �ٽ� ��ȯ�մϴ�. 
            return itemInfo;
        }
        
        /// <summary>
        /// �������� �̸��� ���ڷ� �޾Ƽ� �ش� �������� �˻��Ͽ� �κ��丮�� ��ųʸ� ��Ͽ��� �������ִ� �޼��� �Դϴ�.<br/>
        /// ���ڸ� ���� �ֽż����� ���� �� ������, ������ ������ ������ �������� ������ �� �ֽ��ϴ�. �⺻�� �ֽż��Դϴ�.<br/>
        /// </summary>
        /// <returns>��ųʸ� ����� ���ſ� ������ ��� �ش� �������� ItemInfo ��������, ��Ͽ� ���� �������� ��� null�� ��ȯ�մϴ�.</returns>
        public ItemInfo RemoveItem(string itemName, bool isLatest=true)
        {
            // �̸��� ���� ���� �������� ��� ��ųʸ��� �ִ��� �����մϴ�
            Dictionary<string, List<GameObject>> itemDic = GetItemDicInExists(itemName);    
               
            // ��ųʸ��� �������� �ʴ´ٸ�, ���и� ��ȯ�մϴ�
            if(itemDic==null)       
                return null;       
            
            // ��ųʸ����� ������Ʈ ����Ʈ�� �޽��ϴ�
            List<GameObject> itemObjList = itemDic[itemName]; 

            // �ش� ������Ʈ ����Ʈ�� �����Ͽ� �������� Ÿ���� �̸� ����ϴ�
            ItemType itemType = itemObjList[0].GetComponent<ItemInfo>().Item.Type;            
            
            ItemInfo targetItemInfo = null;  // ��ȯ�� �����۰� ������ �ε����� �����մϴ�.
            int targetIdx;

            if( isLatest )
                targetIdx = itemObjList.Count-1;  // �ֽ� ��
            else
                targetIdx = 0;                    //������ ��

            targetItemInfo =itemObjList[targetIdx].GetComponent<ItemInfo>();  // ��ȯ �� ������ �������� ����ϴ�.
            itemObjList.RemoveAt(targetIdx);        // �ش� �ε����� �������� �����մϴ�            
        
            SetCurItemObjCount(itemType, -1);       // �ش� ������ ������ ���� ������Ʈ�� ������ ���ҽ�ŵ�ϴ�.      
                        
            return targetItemInfo;            // ��Ͽ��� ������ �������� ��ȯ�մϴ�.     
        }








        
        /// <summary>
        /// �ش� �������̸��� ������� �������� �κ��丮�� ��ųʸ� ��Ͽ��� �����ϴ��� ���θ� ��ȯ�մϴ�
        /// </summary>
        /// <returns>�ش��ϴ� �̸��� �������� �κ��丮 ��Ͽ� �����ϴ� ��� true, �������� �ʴ� ��� false�� ��ȯ�մϴ�</returns>
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
        /// ������ �̸��� ������� �ش� �������� ItemType�� ��ȯ�մϴ�<br/>
        /// ������ �̸��� �� �κ��丮�� ��ųʸ� ���ο� �����Ͽ��� �մϴ�.
        /// </summary>
        /// <returns>�ش��ϴ� �̸��� �������� ���� �κ��丮 ��Ͽ� �������� �ʴ� ��� ItemType.None�� ��ȯ�մϴ�</returns>
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
        /// ������ �̸��� ������� �ش� �������� ItemType ���� ��ȯ�մϴ�<br/>
        /// �������� ���� �κ��丮�� ��Ͽ� �������� �ʾƵ� ItemType ���� ���� �� �ֽ��ϴ�.<br/>
        /// </summary>
        /// <returns>*** �ش��ϴ� �̸��� �������� ���� ������ ��Ͽ� �������� �ʴ� ��� ���ܸ� �����ϴ�. ***</returns>
        public ItemType GetItemTypeIgnoreExists(string itemName)
        {
            WorldItem worldItem = new WorldItem();       // ���� GetComponent������� ���濹��

            
            if(worldItem.weapDic.ContainsKey(itemName))
                return ItemType.Weapon;
            else if(worldItem.miscDic.ContainsKey(itemName))
                return ItemType.Misc;
            else
                throw new Exception("�ش��ϴ� �������� ���� ���� ��Ͽ� �������� �ʽ��ϴ�. �������� �̸��� Ȯ���Ͽ� �ּ���.");
        }


        

        /// <summary>
        /// ������ �̸��� ������� �ش� �������� ��ųʸ� �������� ��ȯ�մϴ�<br/>
        /// �������� ��ųʸ� ���ο� �����Ͽ��� �մϴ�.
        /// </summary>
        /// <returns>�ش��ϴ� �̸��� �������� �κ��丮 ��Ͽ� �������� �ʴ� ��� null�� ��ȯ�մϴ�</returns>
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
        /// ������ �̸��� ������� �ش� �������� ��ųʸ� �������� ��ȯ�մϴ�<br/>
        /// �������� ���� �κ��丮�� ��Ͽ� �������� �ʾƵ� ���� �������� ���� �� �ֽ��ϴ�.<br/>
        /// </summary>
        /// <returns>*** �ش��ϴ� �̸��� ���� ������ ��Ͽ� �������� �ʴ� ��� ���ܸ� �����ϴ�. ***</returns>
        public Dictionary<string, List<GameObject>> GetItemDicIgnoreExsists(string itemName)
        {
            CreateManager createManager = GameObject.FindWithTag("GameController").GetComponent<CreateManager>();
            
            if( createManager.GetWorldItemType(itemName) == GetItemTypeIgnoreExists

            if(worldItem.weapDic.ContainsKey(itemName))
                return weapDic;
            else if(worldItem.miscDic.ContainsKey(itemName))
                return miscDic;
            else
                throw new Exception("�ش��ϴ� �������� ���� ���� ��Ͽ� �������� �ʽ��ϴ�. �������� �̸��� Ȯ���Ͽ� �ּ���.");
        }


        /// <summary>
        /// �������� Ÿ���� ������� �ش� �������� ��ųʸ� �������� ��ȯ�մϴ�<br/>
        /// �������� ���� �κ��丮�� ��Ͽ� �������� �ʾƵ� ���� �������� ���� �� �ֽ��ϴ�.<br/>
        /// </summary>
        /// <returns>itemType���ڰ� ItemType.None���� ���� �� ��� null�� ��ȯ�մϴ�</returns>
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
        /// ������ �̸��� ������� �ش� �������� ������Ʈ ����Ʈ �������� ��ȯ�մϴ�<br/>
        /// �ش� ������Ʈ ����Ʈ�� �������� �ʴ� ��� null�� ��ȯ������,<br/>
        /// isNewIfNotExist �ɼ��� ���� �κ��丮 ���ο� ���Ӱ� �����Ͽ� ��ȯ�ϱ� ���θ� ������ �� �ֽ��ϴ�.
        /// </summary>
        /// <returns>�ش��ϴ� �̸��� �������� �κ��丮 ��Ͽ� �����ϴ� ��� GameObject������ ����Ʈ��, �������� �ʴ� ��� null�� ��ȯ�մϴ�</returns>
        public List<GameObject> GetItemObjectList(string itemName, bool isNewIfNotExist=false)
        {
            if(weapDic.ContainsKey(itemName))
                return weapDic[itemName];
            else if(miscDic.ContainsKey(itemName))
                return miscDic[itemName];
            // �ش� ����Ʈ�� �κ��丮 ���ο� ���� ���
            else 
            {                                   
                if(isNewIfNotExist)     // ���� �����ϱ� �ɼ��� ������ ��� 
                {
                    List<GameObject> itemObjList = new List<GameObject>();          // ������Ʈ ����Ʈ�� ���� ����ϴ�.
                    GetItemDicIgnoreExsists(itemName).Add(itemName, itemObjList);   // �κ��丮 ������ ������Ʈ ����Ʈ�� ����ֽ��ϴ�.
                    return itemObjList;                                             // ������ ������Ʈ ����Ʈ ������ ��ȯ�մϴ�.
                }
                else                    
                    return null;
            }
        }


        /// <summary>
        /// ������ �̸��� �Է��Ͽ� �ش� �������� ���� enum�� int������ ��ȯ�޽��ϴ�. (�κ��丮�� �������� �ʾƵ� �˴ϴ�.) <br/>
        /// *** �������� ���� ������ �������� �ʴ´ٸ� ���ܸ� �����ϴ�. ***
        /// </summary>
        /// <returns>�ش� �������� ItemType�� int�� ��ȯ ��</returns>
        public int GetItemTypeIndexIgnoreExists(string itemName)
        {           
            return (int)GetItemTypeIgnoreExists(itemName);
        }



        /// <summary>
        /// �ش� �������� �̸��� ���ڷ� �־� �������� ��ø �ִ� ������ ��ȯ �޽��ϴ�.<br/>
        /// ������ ���� ������ �ִ� ��ø ���� ������ �˱� ���� �޼����Դϴ�.<br/><br/>
        /// ���� IsAbleToAddMisc�޼��忡�� ���������� ���˴ϴ�.<br/><br/>
        /// *** �ش��ϴ� �̸��� �������� ���� ��� ���ܰ� �߻��մϴ�.<br/>
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns>�������� �ִ� ������ ��ȯ�մϴ�.</returns>
        public int GetItemMaxOverlapCount(string itemName)
        {
            CreateManager createManager = GameObject.FindWithTag("GameController").GetComponent<CreateManager>();

            Dictionary<string, Item> worldDic = createManager.GetWorldDic(itemName);

            return ((ItemMisc)worldDic[itemName]).MaxOverlapCount;
        }


        


    }
}
