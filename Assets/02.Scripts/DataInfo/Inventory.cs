using ItemData;
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
 * 
 */




namespace CraftData
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
        /// �÷��̾� �κ��丮�� ���� ���� ĭ�� ���� �� �Դϴ�. ���� �߿� ���׷��̵� ������ ���� ������ �� �ֽ��ϴ�.
        /// </summary>
        public int WeapCountLimit { get; }

        /// <summary>
        /// �÷��̾� �κ��丮�� ��ȭ ���� ĭ�� ���� �� �Դϴ�. ���� �߿� ���׷��̵� ������ ���� ������ �� �ֽ��ϴ�.
        /// </summary>
        public int MiscCountLimit { get; }
        
        /// <summary>
        /// �÷��̾� �κ��丮�� ��ü ���� ĭ�� ���� �� �Դϴ�. ���� �߿� ���׷��̵� ������ ���� ������ �� �ֽ��ϴ�.
        /// </summary>
        public int AllCountLimit { get{return WeapCountLimit+MiscCountLimit; } }


        /**** �ڵ����� �����Ǵ� �Ӽ��� ****/

        /// <summary>
        /// �÷��̾� �κ��丮�� �� �ǿ� �������� �־����� �ʱ� ĭ�� ���� ���Դϴ�.
        /// </summary>
        public int InitialCountLimit { get{ return 50; } }
        
        
        
        /// <summary>
        /// �÷��̾� �κ��丮�� ������ ������� ��� �������� �����ϰ� �ִ� ���� ĭ ���Դϴ�.
        /// </summary>
        public int TotalCount { get{ return WeapCount + MiscCount; } }

        /// <summary>
        /// ���� �������� �κ��丮�� �����ϰ� �ִ� ĭ�� �� �����Դϴ�.
        /// </summary>
        public int WeapCount { 
            get{ 
                int count = 0;
                foreach(List<GameObject> objList in miscDic.Values)
                    count += objList.Count;    
                // ��ųʸ����� ���ӿ�����Ʈ ����Ʈ�� ������ ����Ʈ�� Count ������Ƽ�� ���� ���ӿ�����Ʈ ���ڸ� ������ŵ�ϴ�.
                return count; } }
        
        /// <summary>
        /// ��ȭ �������� �κ��丮�� �����ϰ� �ִ� ĭ�� �� �����Դϴ�.
        /// </summary>
        public int MiscCount { get{
                int count = 0;
                foreach(List<GameObject> objList in weapDic.Values)
                    count += objList.Count;    
                // ��ųʸ����� ���ӿ�����Ʈ ����Ʈ�� ������ ����Ʈ�� Count ������Ƽ�� ���� ���ӿ�����Ʈ ���ڸ� ������ŵ�ϴ�.
                return count; } }


        /// <summary>
        /// ���� �κ��丮�� �����ϰ� �ִ� ���� �߿��� ���ϴ� ������ ������ �ν��Ͻ� ����Ʈ�� ����� �ݴϴ�.<br/>
        /// ������Ʈ�� ������ �������� ������ ������ �о���� �ʿ䰡 ���� ��,<br/>
        /// ���� ������Ʈ ����Ʈ�� ����ȭ �� ��, ���� �Ѿ�� ���� �����ؾ��ϴ� �뵵�� ����մϴ�.
        /// </summary>
        /// <param name="itemType"> GameObjectŸ�Կ��� ItemŸ������ ����Ʈȭ �� ��������� ������ �����ּ���. Weapon, Misc���� �ֽ��ϴ�. </param>
        public List<Item> ConvertDicToItemList(ItemType itemType)
        {
            List<Item> itemList = new List<Item>();
            switch(itemType)
            {
                case ItemType.Weapon:
                    foreach( List<GameObject> objList in weapDic.Values ) // ����������� ���ӿ�����Ʈ ����Ʈ�� �ϳ��� ������
                    {
                        for(int i=0; i<objList.Count; i++)                // ����Ʈ�� ���ӿ�����Ʈ�� ��� �����ɴϴ�.
                            itemList.Add( objList[i].GetComponent<ItemInfo>().Item );   // item ��ũ��Ʈ�� �ϳ��� ������ �����մϴ�.
                    }
                    break;

                case ItemType.Misc:
                    foreach( List<GameObject> objList in miscDic.Values ) // ��ȭ�������� ���ӿ�����Ʈ ����Ʈ�� �ϳ��� ������
                    {
                        for(int i=0; i<objList.Count; i++)                // ����Ʈ�� ���ӿ�����Ʈ�� ��� �����ɴϴ�.
                            itemList.Add( objList[i].GetComponent<ItemInfo>().Item );   // item ��ũ��Ʈ�� �ϳ��� ������ �����մϴ�.
                    }
                    break;
            }
            return itemList;    // item ������ �ϳ��� ����Ǿ��ִ� itemList�� ��ȯ�մϴ�.
        }

        /// <summary>
        /// ���� �� ������ ������ ����ִ� ����Ʈ�� ��������� �ش� ��ųʸ��� ��ȯ���� �ݴϴ�.<br/>
        /// �ε� �Ҷ��� ���� ��ȯ���� �� ������ ����Ʈ�� ������ȭ �Ͽ� �ٽ� �κ��丮 ������� ��ȯ�ؾ��ϴ� �뵵�� ����մϴ�.
        /// </summary>
        /// <param name="itemType">�ش� itemList�� item�� ����</param>
        /// <param name="itemList">��ųʸ��� ��ȯ ��ų item������ �����Ǿ� �ִ� List</param>
        public void ConvertItemListToDic( ItemType itemType, List<Item> itemList )
        {
            switch(itemType)
            {
                case ItemType.Weapon:
                    foreach(Item item in itemList) // ������ ����Ʈ���� ���� ������ ������ �ϳ��� �����ɴϴ�.
                    {
                        // ������ ������ �������� �ϴ� ���ӿ�����Ʈ�� ����ϴ�.
                        GameObject itemObject = CreateManager.instance.CreateItemByInfo( item );

                        if( weapDic[item.Name] == null )            // ������ �ش��ϴ� �������� �̸��� ���ٸ�
                        {
                            List<GameObject> itemObjList = new List<GameObject>();
                            itemObjList.Add( itemObject );          // ���ο� ����Ʈ�� ����� ������Ʈ�� �ֽ��ϴ�.
                            weapDic.Add(item.Name, itemObjList );   // ������ ������ �̸���, ����Ʈ�� �߰��մϴ�.
                        }
                        else // ������ �ش��ϴ� �������� �̸��� �ִٸ�
                        {
                            weapDic[item.Name].Add(itemObject);     // ������ �����Ͽ�, �ش� ����Ʈ�� ������Ʈ�� �߰��մϴ�.
                        }
                    }
                    break;

                case ItemType.Misc:
                    foreach(Item item in itemList) // ������ ����Ʈ����
                    {
                        // ������ ������ �������� �ϴ� ���ӿ�����Ʈ�� ����ϴ�.
                        GameObject itemObject = CreateManager.instance.CreateItemByInfo( item );

                        if( miscDic[item.Name] == null )            // ������ �ش��ϴ� �������� �̸��� ���ٸ�
                        {
                            List<GameObject> itemObjList = new List<GameObject>();
                            itemObjList.Add( itemObject );          // ���ο� ����Ʈ�� ����� ������Ʈ�� �ֽ��ϴ�.
                            miscDic.Add(item.Name, itemObjList );   // ������ ������ �̸���, ����Ʈ�� �߰��մϴ�.
                        }
                        else // ������ �ش��ϴ� �������� �̸��� �ִٸ�
                        {
                            miscDic[item.Name].Add(itemObject);     // ������ �����Ͽ�, �ش� ����Ʈ�� ������Ʈ�� �߰��մϴ�.
                        }
                    }
                    break;
                    
                // ����) ��ȭ�������� �ִ� ���� �˻縦 ���� ���� �ʽ��ϴ�.
                // ������ ���� �� MaxCount�̻��� �� ���� ó�� ������ ������, Inventory���� �̸� MaxCount�� ���� ���ο� ������Ʈ ���� ���� ó���ߴٰ� �Ǵ��մϴ�.
                // �� �̵��̳� �ε� ���� ��ġ�鼭 �ش� ������ ������ ���� ������Ʈ �״�� �����ؾ� �մϴ�.
            }
        }

        /// <summary>
        /// �⺻ �κ��丮 �������Դϴ�. ���ο� ������ ������ �� ������ּ���.
        /// </summary>
        public Inventory()
        {            
            weapDic = new Dictionary< string, List<GameObject> >();
            miscDic = new Dictionary< string, List<GameObject> >();
            WeapCountLimit = InitialCountLimit;
            MiscCountLimit = InitialCountLimit;
        }

        /// <summary>
        /// ������ ����ȭ �� �κ��丮�� �����ϴ� ��� �ش� ������ �������� �ٽ� �κ��丮�� ������ִ� �������Դϴ�. 
        /// </summary>
        public Inventory( SerializableInventory savedInventory )
        {
            ConvertItemListToDic(ItemType.Weapon, savedInventory.weapList);
            ConvertItemListToDic(ItemType.Misc, savedInventory.miscList);
            WeapCountLimit = savedInventory.WeapCountLimit;
            MiscCountLimit = savedInventory.MiscCountLimit;
        }
    }




    /// <summary>
    /// ����ȭ ������ �κ��丮 Ŭ�����Դϴ�.<br/>
    /// ������ �κ��丮���� GameObject ������ ����Ʈ�� Item ������ ����Ʈ�� ��ȯ�Ͽ� �����մϴ�.<br/>
    /// </summary>
    [Serializable]
    public class SerializableInventory
    {
        public List<Item> weapList;
        public List<Item> miscList;
        public int WeapCountLimit {get;}
        public int MiscCountLimit {get;}
        

        /// <summary>
        /// ������ �κ��丮 Ŭ������ �ν��Ͻ��� ���ڷ� �޾Ƽ� ����ȭ ������ �κ��丮 �ν��Ͻ��� ������ �ݴϴ�.
        /// </summary>
        public SerializableInventory( Inventory inventory )
        {
            weapList = inventory.ConvertDicToItemList(ItemType.Weapon);
            miscList = inventory.ConvertDicToItemList(ItemType.Misc);
            WeapCountLimit = inventory.WeapCountLimit;
            MiscCountLimit = inventory.MiscCountLimit;
        }
    }

}
