using ItemData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * [���� ����]
 * �÷��̾� ���ۿ� �ʿ��� �������̸�, ����ȭ�Ǿ� Save �� Load �Ǿ�� �� ����
 * 
 * [�κ��丮�� ���]
 * 1- ����ȭ ������ȭ
 * 2- �ش� ������ ���� �� ������Ͽ��� ����, ���� ����, ��ġ �ݿ�, 
 * 
 * 
 * [�۾� ����]
 * <v1.0 - 2023_1113_�ֿ���>
 * 1- �з� ����� Ivnentory Ŭ������ Inventory.cs���Ϸ� �и��Ͽ��� (namespace�� �״��)
 * 
 * <v2.0 - 2023_1114_�ֿ���>
 * 1- �κ��丮 Ŭ���� ����� ��� �� ��ȭ������ ���ӿ�����Ʈ ����Ʈ�� ��ųʸ�-���ӿ�����Ʈ����Ʈ�� �����ϱ�� ��ȹ
 * => �� ������ ���� ���� ������ �Ͼ�� ����Ʈ�� ���������� ���� �ϳ��� ������ ����� ��ȿ���� �߻��ϰ� �ֱ� �����̸�,
 * Ư�� ������ ������ �������� ��Ͽ��� �����ϱ� ���� �Ź� ����Ʈ�� �ϳ��� ���������ϸ� ���� �ڿ��Ҹ� ���� �߻��ϱ� ����
 * 
 * <v3.0 - 2023_1119_�ֿ���>
 * 1- �޼��� �̸� ���� - GetItemList -> ConvertDicToItemList
 * 2- �޼��� �̸� ���� �� �Ű����� ���� - SetObjectList(ItemType, List<Item>) -> ConvertItemListToDic(List<Item>)
 * 3- ������Ƽ �̸� ���� - TotalMaxCount -> AllCountLimit, TotalCurCount->TotalCount
 * 4- InitialCount -> InitialCountLimit���� ���� �� static �������� ������Ƽ�� ����. (�޸� ���� ���� ����)
 * 
 * 5- ���ο� ������� �߰� - weapCountLimit, miscCountLimit 
 * �κ��丮 �������� ĭ���� ������ �ִ� ������� �����ؾ� �ϱ� ����
 * ĭ���� change�Ѵٰ� �� �� ��ü ĭ���� �ø��� ���߿� ���� ĭ���� �������� ���ϱ� �����̸�, 
 * item�� add�� ���� ���� ������ �־�� ���� ĭ���� ���Ͽ� add��ų �� ������,
 * ���� ���� ĭ ���� �������� TotalCountLimit�� �ڵ����� ������ �� �ֱ� ����
 *  
 * 6- �⺻ ������ ����
 * �κ��丮 �ʱ� ������, �����ʱ�ȭ�� ���� ���� ĭ �� �ʱ�ȭ�� �̷����.
 * 
 * <v4.0 - 2023_1120_�ֿ���>
 * 1- �� ��ũ��Ʈ�� Inventory Ŭ������ partial Ŭ������ ��ȯ
 * ������ �߰�, �˻�, ���� ���� ������ ���� ���Ͽ��� �� ����
 * 2- ���� ����Ʈ�� ����� �߰�
 * 3- Inventory Ŭ������ TotalCountLimit �������� AllCountLimit���� ����
 * 4- Inventory Ŭ������ SerializableInventory Ŭ������ weapCountLimit, miscCountLimit ������Ƽȭ �� ������ �ոӸ� �빮�ڷ� ����
 * 5- Serializable Ŭ������ �����ڿ�  WeapCountLimit, MiscCountLimit�� �ʱ�ȭ�� �����ִ� �� ����
 * 
 * <v4.1 - 2023_1122_�ֿ���>
 * 1- ����, ��Ÿ ������ �ּ� ����
 * 
 * <v4.2 - 2023_1221_�ֿ���>
 * 1- ����Ŭ������ Inventory_p2.cs ���� �߰�
 * AddItem�޼��带 �߰��Ͽ���.
 * 
 * <v5.0 - 2023_1221_�ֿ���>
 * 1- WeapCount�� MiscCount�� ���� miscDic�� weapDic�� Values�� �ٲ㼭 �����ϴ� �� ����
 * 
 * 2- ���ο� ������ �����ϴ� ��� DataManager�� Load�޼���� GameData�� ������ ������
 * ����Ʈ �����ڰ� �ƴ� Inventory( SerializableInventory savedInventory ) �����ڰ� ȣ������� ����
 * ��ųʸ��� �ʱ�ȭ���� �ʾ� null���۷����� �߻��Ͽ����ϴ�.
 * �̸� ���ֱ� ���� �ش� �����ڿ� ��ųʸ��� null�� �˻縦 �߰��Ͽ� ���ο� ��ųʸ��� �����ϵ��� �Ͽ����ϴ�.
 * 
 * <v5.1 - 2023_1224_�ֿ���>
 * 1- ������ ������ ConvertItemListToDic �޼��忡��
 * The given key 'ö' was not present in the dictionary. ������ �߻��Ͽ�
 * miscDic[item.Name]==null�� �˻��ϴ� ������ miscDic.ContainsKey(item.Name) �������� ����
 * 
 * <v6.0 - 2023_1226_�ֿ���>
 * 1- SerializableInventory�� weapList, miscList�� Item Ÿ�Կ��� ���� Ÿ���� ItemWeapon, ItemMiscŸ������ ����
 * 2- ConvertDicToItemList�� ConvertItemListToDic �޼��� ���������� ������ ���� �ߺ��������� �ۼ��Ǿ��ִ� ���� �Ϲ�ȭ�޼���� ����
 * 
 * <v7.0 - 2023_1226_�ֿ���>
 * 1- DeserializeItemListToDic�� ����� ��¿� �޼������ ����
 * 
 * 2- SerializableInventory�� WeapCountLimit, MiscCountLimit���� �ڵ����� ������Ƽ�� �ƴ� public������ ����
 * 
 * 3- SerializableInventory�� Ŭ���� ���� SInventory�� �����ϰ� Serialize�޼���� Deserialize�޼��带 �����Ͽ���.
 * ������ STransform�� ��뿡 �ϰ����� �־� ����� �ʰ� �ϱ� ����
 * 
 * 4- InventoryŬ������ SerialzableInventory�� ���ڷ� �޴� ������ ����
 * SInventory�� Deserialize�޼��忡�� ���Ӱ� �κ��丮�� �����ؼ� �ϳ��� ���� �־��༭ �κ��丮Ŭ������ ��ȯ�ϰԲ� ����
 * 
 * 5- WeapCountLimit, MiscCountLimit ������Ƽ set��� �߰�
 * 
 * <v7.1 - 2023_1227_�ֿ���>
 * 1- Item�������� �������� �������� ���� SerializeDicToItemList�޼��忡�� GetComponent�� ItemInfo�� �����ϴ� ���� GetComponentInChildren���� ����
 * 
 * <v7.2 - 2023_1229_�ֿ���>
 * 1- ���ӽ����̽��� CraftData���� InventoryManagement�� ����
 * 2- SInventoryŬ���� SaveData_Inventory�� �̵�
 * 
 * <v7.3 - 2023_1230_�ֿ���>
 * 1- ���� ��������� ��ųʸ��� privateó��
 * ������ ���̺귯�� ����ڰ� �κ��丮�� ���� ���� �� �Ǽ��� �����ϱ� ����.
 * 
 * <v8.0 - 2023_1231_�ֿ���>
 * 1- WeapCountLimit,MiscCountLimit�� -> WeapSlotCountLimit, MiscSlotCountLimit���� �̸� ����
 * ���� ���� �� (������ ĭ ���� ��)��� �ǹ̸� �и��ϰ� �Ͽ���
 * 
 * 2- curWeapItemCount, curMiscItemCount ���� �߰�
 * ���� ����ִ� ������Ʈ ������ �����ϴ� ������ ����
 * 
 * 3- TotalCount�� TotalCurItemCount�� ���� 
 * curWeapItemCount, curMiscItemCount �������� ���
 * 
 * 4- ������ WeapCount, MiscCount ������Ƽ�� �����ϰ� GetCurItemCount, SetCurItemCount, GetItemCountLimit �޼��带 �߰�
 * 
 * �ε� �� curWeapItemCount�� curMiscItemCount�� �ʱ�ȭ �ϰų�,
 * AddItem, RemoveItem���� ���������� ������ ī��Ʈ�� ������ ���� �� �̿��ϵ��� ����
 *  
 * 
 * 5- weapDic, miscDic�� JsonProperty�� �ٿ����Ҵ� �� ����
 * 
 * 6- DeserializeItemListToDic ����ȭ,
 * Inventory_2�� ���ǵǾ��ִ� �⺻ �޼��带 ������� ��������
 * 
 * <v8.1 - 2023_1231_�ֿ���>
 * 1- ���� �ε����� �ӽ÷� ��� ���� indexList ������ ����
 * 2- ������ ���� 
 * AllCountLimit -> SlotCountLimitAll
 * WeapSlotCountLimit -> SlotCountLimitWeap
 * MiscSlotCountLimit -> SlotCountLimitMisc
 * 
 * 3- GetCurItemCountLimit�޼��� ����, GetItemSlotCountLimit�� ���Ӱ� ����,
 * ���ڸ� �� ���� �� ��ü ������ �ִ� ���� ���� ��ȯ�ϵ��� �����Ͽ���,
 * ���ڸ� ���� �� ItemType�� ���� �ִ� ���� ���� ���� ��ȯ�ϵ��� ����.
 * 
 * 4- GetCurItemCount�޼��� ���� ��ųʸ� count==0���� Ȯ���ϴ� �ڵ� ����
 * 
 * 
 * <v8.2 - 2024_0102_�ֿ���>
 * 1- ��ųʸ��� ���̸� ��ȯ�ϴ� ������Ƽ �߰�
 * 2- CurItemCount, CurItemWeapCout, CurItemMiscCount�� CurItemObjCount�� �̸�����
 * 
 * 
 * 
 * 
 * [�̽�_0102]
 * 1- ��ųʸ� �������� �Ϲ�ȭ ��ų �� �־����.
 * 2- curDicLen ������Ƽ�� ���� ��ųʸ��� ����� ���θ� ��ȯ�ؾ� �ϴµ� �̸� ���� enum������ ���� ������ �־�� �Ѵ�.
 * 
 * 
 */




namespace InventoryManagement
{
    /// <summary>
    /// �κ��丮�� �����ϴ� Ŭ����, ��ųʸ��� �̸������� ���Ͽ� �����ϸ�
    /// ���ο� GameObject�� �����ϴ� weapList�� miscList�� �ֽ��ϴ�.<br/> 
    /// ******Save, Load�� �κ��丮 Ŭ������ �����ؾ� �մϴ�.******
    /// </summary>
    [Serializable]
    public partial class Inventory
    {
        /**** ���̺� �ε� �� �����ؾ��� �Ӽ��� ****/

        /// <summary>
        /// �÷��̾ �����ϰ� �ִ� ���� ������ ����Դϴ�. ������ �̸��� ���� �ش� ������ ������Ʈ�� �����ϴ� ����Ʈ�� ������ �� �ֽ��ϴ�.
        /// </summary>
        public Dictionary< string, List<GameObject> > weapDic;

        /// <summary>
        /// �÷��̾ �����ϰ� �ִ� ��ȭ ������ ��� �Դϴ�. ������ �̸��� ���� �ش� ������ ������Ʈ�� �����ϴ� ����Ʈ�� ������ �� �ֽ��ϴ�.
        /// </summary>
        public Dictionary< string, List<GameObject> > miscDic;
                        
        /// <summary>
        /// �÷��̾� �κ��丮�� ���� ���� ���� ĭ ���� �� �Դϴ�. ���� �߿� ���׷��̵� ������ ���� ������ �� �ֽ��ϴ�.
        /// </summary>
        public int SlotCountLimitWeap { get; set; }

        /// <summary>
        /// �÷��̾� �κ��丮�� ��ȭ ���� ���� ĭ ���� �� �Դϴ�. ���� �߿� ���׷��̵� ������ ���� ������ �� �ֽ��ϴ�.
        /// </summary>
        public int SlotCountLimitMisc { get; set; }
        
        


        /**** �ε� �ÿ��� �ҷ��� �� ���� �߿� ���������� ���� �� �����ؾ� �� �Ӽ� ****/            

        /// <summary>
        /// ���� �κ��丮�� ����ִ� ���� ������ ������Ʈ�� ������ ��ȯ�ްų� �����մϴ�.
        /// </summary>
        public int CurWeapItemObjCount {get; set;} = 0;  // ���ο� �κ��丮 ���� �� 0���� �ʱ�ȭ
        
        /// <summary>
        /// ���� �κ��丮�� ����ִ� ��ȭ ������ ������Ʈ�� ������ ��ȯ�ްų� �����մϴ�.
        /// </summary>
        public int CurMiscItemObjCount {get; set;} = 0;  // ���ο� �κ��丮 ���� �� 0���� �ʱ�ȭ
                        



        /**** �ڵ����� �����Ǵ� ���� �Ӽ��� ****/
        
        /// <summary>
        /// �÷��̾� �κ��丮�� �� �ǿ� �������� �־����� �ʱ� ĭ�� ���� ���Դϴ�.
        /// </summary>
        public int InitialCountLimit { get{ return 50; } }        

        /// <summary>
        /// �÷��̾� �κ��丮�� ��ü ���� ĭ�� ���� �� �Դϴ�. ���� �߿� ���׷��̵� ������ ���� ������ �� �ֽ��ϴ�.
        /// </summary>
        public int SlotCountLimitAll { get{return SlotCountLimitWeap+SlotCountLimitMisc; } }        
        
        /// <summary>
        /// �÷��̾� �κ��丮�� ������ ������� ��� �������� �����ϰ� �ִ� ���� ĭ ���Դϴ�.
        /// </summary>
        public int TotalCurItemCount { get{ return CurWeapItemObjCount + CurMiscItemObjCount; } }
        
        /// <summary>
        /// ���� �κ��丮�� ��ųʸ� ���̸� ��ȯ�մϴ�.
        /// </summary>
        public int CurDicLen { get { return (int)ItemType.None-1;} } //��ü �����̹Ƿ� ���� ������ ������ �κ��丮������ ������� �����Ƿ� �����ʿ�

        
        /**** ���������θ� ����ϴ� �Ӽ� ****/
        List<int> indexList = new List<int>();





        
        
        /// <summary>
        /// ���� �κ��丮�� ����ִ� ������ ������Ʈ�� ������ �����Ͽ� ��ȯ�մϴ�.<br/>
        /// �ʱ� �ε� �� �κ��丮�� �ε� �� ������Ʈ�� ���� �� CurItemCount�� �����ϱ� ���� �ʿ��մϴ�.<br/><br/>
        /// ���ڷ� ItemType�� �����Ͽ� �ش� ItemType�� ��ųʸ����� ������Ʈ ������ ī�����մϴ�.
        /// </summary>
        /// <returns>������ Ÿ�԰� ��ġ�ϴ� �κ��丮�� ���� �Ǿ��ִ� ������Ʈ�� ������ ��ȯ�մϴ�.</returns>
        public int GetCurItemCount(ItemType itemType) 
        { 
            int count = 0;

            // ������ Ÿ���� ������� ��ųʸ��� ���մϴ�.
            Dictionary<string, List<GameObject>> itemDic = GetItemDicIgnoreExsists(itemType);

            // �ش� �κ��丮 ��ųʸ��� ����ִ� ����Ʈ�� �ϳ��� ���ٸ�, �ٷ� 0�� ��ȯ�մϴ�.
            if(itemDic.Count==0)        
                return 0;

            // �ش� ��ųʸ����� ���ӿ�����Ʈ ����Ʈ�� ������ ����Ʈ�� Count ������Ƽ�� ���� ���ӿ�����Ʈ ���ڸ� ������ŵ�ϴ�.
            foreach(List<GameObject> objList in itemDic.Values)
                count += objList.Count;   
            
            return count;  
        }
        
        /// <summary>
        /// ItemType�� ������� � ������ curItemCount�� ���� �Ǵ� ���� ��ų �� �����ϴ� �޼����Դϴ�.<br/>
        /// inCount�� ������ �����ϸ� �������� ���� ������ ���ҽ�ŵ�ϴ�.<br/>
        /// AddItem�޼��忡�� ���������� ���ǰ� �ֽ��ϴ�.<br/><br/>
        /// 
        /// *** ���ڷ� �ش� �κ��丮�� �ش����� �ʴ� ItemType�� ������ ��� ���ܸ� �߻���ŵ�ϴ�. ***
        /// </summary>
        public void SetCurItemObjCount(ItemType itemType, int inCount)
        {
            switch(itemType)
            {
                case ItemType.Weapon:
                    CurWeapItemObjCount += inCount;
                    break;

                case ItemType.Misc:
                    CurMiscItemObjCount += inCount;
                    break;

                default:
                    throw new Exception("���ڷ� ���� �� ������ ������ �ش� �κ��丮�� ���� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");
            }
        }

        /// <summary>
        /// ������ ������ ���� ������ ���� ���� ��ȯ�մϴ�. <br/>
        /// ItemType�� ���ڷ� ���޹޽��ϴ�.<br/>
        /// *** ���ڸ� �� ���� �Ǵ� ItemType.None ���� �� ��ü ���� �ε����� �������� ��ȯ�մϴ�.(�⺻ ��) *** <br/>
        /// </summary>
        /// <returns>���ڷ� ���� �� ������ �ִ� ���� ���� ��ȯ�մϴ�</returns>
        public int GetItemSlotCountLimit(ItemType itemType=ItemType.None)
        {
            int slotCountLimit;
                        
            switch(itemType)
            { 
                case ItemType.Weapon:
                    slotCountLimit = SlotCountLimitWeap; 
                    break;

                case ItemType.Misc:
                    slotCountLimit = SlotCountLimitMisc;
                    break;

                default:
                    slotCountLimit = SlotCountLimitAll;
                    break;
            }            

            return slotCountLimit;
        }


        /// <summary>
        /// ���� �κ��丮�� ����ִ� ������ ������Ʈ�� ��ü ������ �����Ͽ� ��ȯ�մϴ�
        /// </summary>
        /// <returns>���� �κ��丮�� �����ϴ� ��� �������� �� �����Դϴ�.</returns>
        public int GetCurItemCountAll()
        {
            int count = 0;
            int dicLen = (int)ItemType.None;

            for(int i=0; i<dicLen; i++)
                count += GetCurItemCount((ItemType)i);

            return count;
        }






        /// <summary>
        /// ���� �κ��丮�� �����ϰ� �ִ� ���� �߿��� ���ϴ� ������ ������ �ν��Ͻ� ����Ʈ�� ����� �ݴϴ�.<br/>
        /// ������Ʈ�� ������ �������� ������ ������ �о���� �ʿ䰡 ���� ��,<br/>
        /// ���� ������Ʈ ����Ʈ�� ����ȭ �� ��, ���� �Ѿ�� ���� �����ؾ��ϴ� �뵵�� ����մϴ�.
        /// </summary>
        /// <param name="itemType"> GameObjectŸ�Կ��� ItemŸ������ ����Ʈȭ �� ��������� ������ �����ּ���. Weapon, Misc���� �ֽ��ϴ�. </param>
        public List<T> SerializeDicToItemList<T>() where T : Item
        {
            List<T> itemList = new List<T>();       // ���ο� T���� �����۸���Ʈ ����
            

            // ���� �κ��丮���� � ������ ���� �� ������ T�� �������� �����մϴ�.
            Dictionary<string, List<GameObject>> itemDic = null;

            if( typeof(T)==typeof(ItemWeapon) )     // ���� ������� �κ��丮�� ���� ���� ����
                itemDic = weapDic;          
            else if( typeof(T)==typeof(ItemMisc) )  // ��ȭ ������� �κ��丮�� ��ȭ ���� ����
                itemDic = miscDic;              
              

            // �ش� ������ �������� �ϳ��� �ҷ��ͼ� ���� Ŭ���� ����(T����) ���� �����մϴ�.
            foreach( List<GameObject> objList in itemDic.Values )                // �ش� �������� ���ӿ�����Ʈ ����Ʈ�� �ϳ��� ������
            {
                for(int i=0; i<objList.Count; i++)                               // ����Ʈ�� ���ӿ�����Ʈ�� ��� �����ɴϴ�.
                    itemList.Add( (T)objList[i].GetComponentInChildren<ItemInfo>().Item ); // item ��ũ��Ʈ�� �ϳ��� ������ T�������� �����մϴ�.
            }

            // item ������ �ϳ��� ����Ǿ��ִ� itemList�� ��ȯ�մϴ�.
            return itemList;                                                     
        }

        /// <summary>
        /// ���� �� ������ ������ ����ִ� ����Ʈ�� ��������� �ش� ��ųʸ��� ��ȯ���� �ݴϴ�.<br/>
        /// �ε� �Ҷ��� ���� ��ȯ���� �� ������ ����Ʈ�� ������ȭ �Ͽ� �ٽ� �κ��丮 ������� ��ȯ�ؾ��ϴ� �뵵�� ����մϴ�.
        /// </summary>
        public void DeserializeItemListToDic<T>( List<T> itemList ) where T : Item
        {
            foreach(Item item in itemList) // ������ ����Ʈ���� ���� ������ ������ �ϳ��� �����ɴϴ�.
            {                        
                // item������ ������� �������� ���Ӱ� �����Ͽ� �κ��丮�� �־��ݴϴ�. (Invnetory_p2�� ���ǵǾ� �ֽ��ϴ�.)
                AddItem(item);
            }
        }



        /// <summary>
        /// �⺻ �κ��丮 �������Դϴ�. ���ο� ������ ������ �� ������ּ���.
        /// </summary>
        public Inventory()
        {
            weapDic=new Dictionary<string, List<GameObject>>();
            miscDic=new Dictionary<string, List<GameObject>>();
            SlotCountLimitWeap=InitialCountLimit;
            SlotCountLimitMisc=InitialCountLimit;
        }
               
    }




    

}
