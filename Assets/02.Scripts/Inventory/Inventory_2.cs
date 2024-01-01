using ItemData;
using System;
using System.Collections.Generic;
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
 * [�߰� ����]
 * 1- AddItem �޼���� �������� ������ ������ ������Ʈ�� �ܺ� InventoryInfo��ũ��Ʈ���� ����� ��.
 * (������ ������Ʈ�� ����Ȱ��ȭ �Ǳ������� �س��� �Ѵ�.)
 * 
 * 2- AddItem �޼���� �κ��丮�� ���� ��, ���� ���� �ε����� ��ü ���� �ε����� �Ѵ� ã�Ƽ� �־���� �Ѵ�.
 * (CreateManager�� �����ʿ�)
 */

namespace InventoryManagement
{    
    public partial class Inventory
    {
        public bool AddItem( string itemName )
        {
            ItemType itemType = GetItemTypeIgnoreExists(itemName);
            
            // ���� ���� ������Ʈ�� ������ ���� ���� ĭ ���� ���� ���ų� ũ�ٸ� �� �̻� �߰��� �� �����Ƿ� false�� ��ȯ�մϴ�.
            if(GetCurItemCount(itemType) >= GetItemSlotCountLimit(itemType) )            
                return false;        
            
            //���忡 �������� �ϳ� �����մϴ�.
            GameObject itemObject = CreateManager.instance.CreateItemToWorld(null, itemName);
            ItemInfo itemInfo = itemObject.GetComponent<ItemInfo>();
            itemInfo.SetSlot
        }

        /// <summary>
        /// Item �ν��Ͻ��� ���ڷ� ���޹޾� ������Ʈ�� ���Ӱ� �����ϰ�, �ش��ϴ� ������ ã�Ƽ� �־��ִ� �޼����Դϴ�. <br/><br/>
        /// *** Item Ŭ������ �ν��Ͻ��� ���ڷ� ������ ��� ������Ʈ�� ���Ӱ� �����˴ϴ�. *** 
        /// </summary>
        /// <returns>������ �߰� ������ true��, ���� �κ��丮�� �� ������ �����ϴٸ� false�� ��ȯ�մϴ�</returns>
        public bool AddItem( Item item )
        {
            // ���� ���� ������Ʈ�� ������ ���� ���� ĭ ���� ���� ���ų� ũ�ٸ� �� �̻� �߰��� �� �����Ƿ� false�� ��ȯ�մϴ�.
            if(GetCurItemCount(item.Type) >= GetItemSlotCountLimit(item.Type) )            
                return false;        

            GameObject itemObject = CreateManager.instance.CreateItemByInfo( item );    //���Գѹ��� ��������Ѵ�.
            ItemInfo itemInfo = itemObject.GetComponent<ItemInfo>();

            itemInfo.SlotIndexAll = itemInfo.FindNearstRemainActiveSlotIdx()


            return AddItem( itemObject );
        }



        /// <summary>
        /// ������ ������Ʈ�� ���ڷ� ���޹޾� �ش��ϴ� ������ ã�Ƽ� �־��ִ� �޼����Դϴ�.<br/><br/>
        /// *** ������ ������Ʈ�� ���ڷ� ������ ��� �� ������Ʈ�� �κ��丮�� �־��ݴϴ�. *** 
        /// </summary>
        /// <returns>������ �߰� ������ true��, ���� �κ��丮�� �� ������ �����ϴٸ� false�� ��ȯ�մϴ�</returns>
        public bool AddItem(GameObject itemObj)
        {
            ItemInfo itemInfo = itemObj.GetComponent<ItemInfo>();
            return AddItem(itemInfo); 
        }


        /// <summary>
        /// ItemInfo ������Ʈ�� ���ڷ� ���޹޾� �ش� ������ ������Ʈ�� �ش��ϴ� ������ ã�Ƽ� �־��ִ� �޼����Դϴ�.<br/><br/>
        /// *** ItemInfo ������Ʈ�� ���ڷ� ������ ��� �ش� ������Ʈ�� �����Ǿ��ִ� ���� ������Ʈ�� �κ��丮�� �־��ݴϴ�. *** 
        /// </summary>
        /// <returns>������ �߰� ������ true��, ���� �κ��丮�� �� ������ �����ϴٸ� false�� ��ȯ�մϴ�</returns>
        public bool AddItem(ItemInfo itemInfo)
        {   
            //������ Ÿ���� Ȯ���մϴ�.
            ItemType itemType = itemInfo.Item.Type;

            // ���� ������Ʈ�� ������ ���� ĭ ���� ���� ���ų� ũ�ٸ� �� �̻� �߰��� �� �����Ƿ� false�� ��ȯ�մϴ�.
            if(GetCurItemCount(itemType) >= GetItemSlotCountLimit(itemType) )            
                return false;        


            // ������ Ÿ���� ������� ��ųʸ��� �����մϴ�.
            Dictionary<string, List<GameObject>> itemDic = GetItemDicIgnoreExsists(itemType);

            AddItemToDic(itemDic, itemInfo);    // ã�� ��ųʸ��� itemInfo ������Ʈ �������� �����Ͽ� ������Ʈ�� �߰��մϴ�.
            SetCurItemCount(itemType, 1);       // �ش� ������ ������ ���� ������Ʈ�� ������ ������ŵ�ϴ�.
            return true;                        // ������ ��ȯ�մϴ�.
        }
        
        
        /// <summary>
        /// ������ �ֱ⸦ ���ϴ� ������ ItemInfo ������Ʈ�� ���ڷ� ���޹޾� �ش� ������ ������ ������Ʈ�� �־��ִ� �޼����Դϴ�.
        /// </summary>
        private void AddItemToDic(Dictionary<string, List<GameObject>> itemDic, ItemInfo itemInfo)
        {
            string itemName = itemInfo.Item.Name;

            if( !itemDic.ContainsKey(itemName) )        // �ش� ������ ������Ʈ ����Ʈ�� �������� �ʴ� ���
            {
                List<GameObject> itemObjList = new List<GameObject>();  // ������Ʈ ����Ʈ�� ���� ����ϴ�
                itemObjList.Add(itemInfo.gameObject);                   // ������Ʈ ����Ʈ�� ������ ������Ʈ�� �߰��մϴ�
                itemDic.Add(itemName, itemObjList);                     // ������ ������Ʈ ����Ʈ�� ����ֽ��ϴ�.
            }
            else                                            // �ش� ������ ������Ʈ ����Ʈ�� �����ϴ� ��� 
            {
                itemDic[itemName].Add(itemInfo.gameObject); // ������Ʈ ����Ʈ�� �����Ͽ� ���ӿ�����Ʈ�� �ֽ��ϴ�.
            }            
        }






        /// <summary>
        /// ������ ������Ʈ�� ���ڷ� �޾Ƽ� �κ��丮�� ��ųʸ� ��Ͽ��� �������ִ� �޼��� �Դϴ�. <br/>
        /// ���ڸ� ���� �ֽż����� ���� �� ������, ������ ������ ������ �������� ������ �� �ֽ��ϴ�. �⺻�� �ֽż��Դϴ�.
        /// </summary>
        /// <returns>��ųʸ� ����� ���ſ� ������ ��� true��, ������ ��� false�� ��ȯ�մϴ�.</returns>
        public bool RemoveItem(GameObject itemObj, bool isLatest=true)
        {
            Item item = itemObj.GetComponent<ItemInfo>().Item;            
            return RemoveItem(item);
        }
        
        /// <summary>
        /// ItemInfo ������Ʈ�� ���ڷ� �޾Ƽ� �κ��丮�� ��ųʸ� ��Ͽ��� �������ִ� �޼��� �Դϴ�.<br/>
        /// ���ڸ� ���� �ֽż����� ���� �� ������, ������ ������ ������ �������� ������ �� �ֽ��ϴ�. �⺻�� �ֽż��Դϴ�. 
        /// </summary>
        /// <returns>��ųʸ� ����� ���ſ� ������ ��� true��, ������ ��� false�� ��ȯ�մϴ�.</returns>
        public bool RemoveItem(ItemInfo itemInfo, bool isLatest=true)
        {
            Item item = itemInfo.Item;            
            return RemoveItem(itemInfo.Item, isLatest);
        }
        
        /// <summary>
        /// Item �ν��Ͻ��� ���ڷ� �޾Ƽ� �κ��丮�� ��ųʸ� ��Ͽ��� �������ִ� �޼��� �Դϴ�.<br/>
        /// ���ڸ� ���� �ֽż����� ���� �� ������, ������ ������ ������ �������� ������ �� �ֽ��ϴ�. �⺻�� �ֽż��Դϴ�. 
        /// </summary>
        /// <returns>��ųʸ� ����� ���ſ� ������ ��� true��, ������ ��� false�� ��ȯ�մϴ�.</returns>
        public bool RemoveItem(Item item, bool isLatest=true)
        {
            string itemName = item.Name;
            return RemoveItem(itemName, isLatest);
        }

        /// <summary>
        /// �������� �̸��� ���ڷ� �޾Ƽ� �κ��丮�� ��ųʸ� ��Ͽ��� �������ִ� �޼��� �Դϴ�.<br/>
        /// ���ڸ� ���� �ֽż����� ���� �� ������, ������ ������ ������ �������� ������ �� �ֽ��ϴ�. �⺻�� �ֽż��Դϴ�. 
        /// </summary>
        /// <returns>��ųʸ� ����� ���ſ� ������ ��� true��, ������ ��� false�� ��ȯ�մϴ�.</returns>
        public bool RemoveItem(string itemName, bool isLatest=true)
        {
            // �̸��� ���� ���� �������� ��� ��ųʸ��� �ִ��� �����մϴ�
            Dictionary<string, List<GameObject>> itemDic = GetItemDicInExists(itemName);    

            // ��ųʸ��� �������� �ʴ´ٸ�, ���и� ��ȯ�մϴ�
            if(itemDic==null)       
                return false;       

            // ��ųʸ����� ������Ʈ ����Ʈ�� �޽��ϴ�
            List<GameObject> itemObjList = itemDic[itemName];

            // �ش� ������Ʈ ����Ʈ�� �����Ͽ� �������� Ÿ���� �̸� ����ϴ�
            ItemType itemType = itemObjList[0].GetComponent<ItemInfo>().Item.Type;

            if(isLatest)     
                itemObjList.RemoveAt(itemObjList.Count-1);  // �ֽż����� �����մϴ�  
            else
                itemObjList.RemoveAt(0);                    // �����ȼ����� �����մϴ�
            
            // �ش� ������ ������ ���� ������Ʈ�� ������ ���ҽ�ŵ�ϴ�.
            SetCurItemCount(itemType, -1);       

            // ������ ��ȯ�մϴ�
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
            ItemInfo itemInfo = itemObj.GetComponent<ItemInfo>();
            return SetOverlapCount(itemInfo);
        }

        public string SetOverlapCount(string itemName)
        {

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
        /// �������� ��ųʸ� ���ο� �����Ͽ��� �մϴ�.
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
        /// <returns>*** �ش��ϴ� �̸��� �������� �κ��丮 ��Ͽ� �������� �ʴ� ��� ���ܸ� �����ϴ�. ***</returns>
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
        /// <returns>*** �ش��ϴ� �̸��� �������� �κ��丮 ��Ͽ� �������� �ʴ� ��� ���ܸ� �����ϴ�. ***</returns>
        public Dictionary<string, List<GameObject>> GetItemDicIgnoreExsists(string itemName)
        {
            WorldItem worldItem = new WorldItem();       // ���� GetComponent������� ���濹��

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
        /// ������ �̸��� ������� �ش� �������� ������Ʈ ����Ʈ �������� ��ȯ�մϴ�
        /// </summary>
        /// <returns>�ش��ϴ� �̸��� �������� �κ��丮 ��Ͽ� �������� �ʴ� ��� null�� ��ȯ�մϴ�</returns>
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
            foreach( List<GameObject> objList in weapDic.Values )                   // ����������� ���ӿ�����Ʈ ����Ʈ�� �ϳ��� ������
            {
                for(int i=0; i<objList.Count; i++)                                  // ����Ʈ�� ���ӿ�����Ʈ�� ��� �����ɴϴ�.
                    objList[i].GetComponentInChildren<ItemInfo>().OnItemChanged();  // item ��ũ��Ʈ�� �ϳ��� ������ OnItemChnaged�޼��带 ȣ���մϴ�.
            }

            foreach( List<GameObject> objList in miscDic.Values )                   // ��ȭ�������� ���ӿ�����Ʈ ����Ʈ�� �ϳ��� ������
            {
                for(int i=0; i<objList.Count; i++)                                  // ����Ʈ�� ���ӿ�����Ʈ�� ��� �����ɴϴ�.
                    objList[i].GetComponentInChildren<ItemInfo>().OnItemChanged();  // item ��ũ��Ʈ�� �ϳ��� ������ OnItemChnaged�޼��带 ȣ���մϴ�.
            }      
        }


    }
}
