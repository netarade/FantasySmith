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
        public int WeapCountLimit { get; set; }

        /// <summary>
        /// �÷��̾� �κ��丮�� ��ȭ ���� ĭ�� ���� �� �Դϴ�. ���� �߿� ���׷��̵� ������ ���� ������ �� �ֽ��ϴ�.
        /// </summary>
        public int MiscCountLimit { get; set; }
        


        

        /**** �ڵ����� �����Ǵ� �Ӽ��� ****/

        /// <summary>
        /// �÷��̾� �κ��丮�� ��ü ���� ĭ�� ���� �� �Դϴ�. ���� �߿� ���׷��̵� ������ ���� ������ �� �ֽ��ϴ�.
        /// </summary>
        public int AllCountLimit { get{return WeapCountLimit+MiscCountLimit; } }


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
                foreach(List<GameObject> objList in weapDic.Values)
                    count += objList.Count;    
                // ��ųʸ����� ���ӿ�����Ʈ ����Ʈ�� ������ ����Ʈ�� Count ������Ƽ�� ���� ���ӿ�����Ʈ ���ڸ� ������ŵ�ϴ�.
                return count; } }
        
        /// <summary>
        /// ��ȭ �������� �κ��丮�� �����ϰ� �ִ� ĭ�� �� �����Դϴ�.
        /// </summary>
        public int MiscCount { get{
                int count = 0;
                foreach(List<GameObject> objList in miscDic.Values)
                    count += objList.Count;    
                // ��ųʸ����� ���ӿ�����Ʈ ����Ʈ�� ������ ����Ʈ�� Count ������Ƽ�� ���� ���ӿ�����Ʈ ���ڸ� ������ŵ�ϴ�.
                return count; } }


        /// <summary>
        /// ���� �κ��丮�� �����ϰ� �ִ� ���� �߿��� ���ϴ� ������ ������ �ν��Ͻ� ����Ʈ�� ����� �ݴϴ�.<br/>
        /// ������Ʈ�� ������ �������� ������ ������ �о���� �ʿ䰡 ���� ��,<br/>
        /// ���� ������Ʈ ����Ʈ�� ����ȭ �� ��, ���� �Ѿ�� ���� �����ؾ��ϴ� �뵵�� ����մϴ�.
        /// </summary>
        /// <param name="itemType"> GameObjectŸ�Կ��� ItemŸ������ ����Ʈȭ �� ��������� ������ �����ּ���. Weapon, Misc���� �ֽ��ϴ�. </param>
        public List<T> SerializeDicToItemList<T>() where T : Item
        {
            List<T> itemList = new List<T>();       // ���ο� T���� �����۸���Ʈ ����
            Dictionary<string, List<GameObject>> itemDic = null;

            if( typeof(T)==typeof(ItemWeapon) )     // ���� ������� �κ��丮�� ���� ���� ����
                    itemDic = weapDic;          
            else if( typeof(T)==typeof(ItemMisc) )  // ��ȭ ������� �κ��丮�� ��ȭ ���� ����
                itemDic = miscDic;              
              

            foreach( List<GameObject> objList in itemDic.Values )                // �ش� �������� ���ӿ�����Ʈ ����Ʈ�� �ϳ��� ������
            {
                for(int i=0; i<objList.Count; i++)                               // ����Ʈ�� ���ӿ�����Ʈ�� ��� �����ɴϴ�.
                    itemList.Add( (T)objList[i].GetComponentInChildren<ItemInfo>().Item ); // item ��ũ��Ʈ�� �ϳ��� ������ T�������� �����մϴ�.
            }

            return itemList;                                                     // item ������ �ϳ��� ����Ǿ��ִ� itemList�� ��ȯ�մϴ�.
        }

        /// <summary>
        /// ���� �� ������ ������ ����ִ� ����Ʈ�� ��������� �ش� ��ųʸ��� ��ȯ���� �ݴϴ�.<br/>
        /// �ε� �Ҷ��� ���� ��ȯ���� �� ������ ����Ʈ�� ������ȭ �Ͽ� �ٽ� �κ��丮 ������� ��ȯ�ؾ��ϴ� �뵵�� ����մϴ�.
        /// </summary>
        /// <param name="itemType">�ش� itemList�� item�� ����</param>
        /// <param name="itemList">��ųʸ��� ��ȯ ��ų item������ �����Ǿ� �ִ� List</param>
        public void DeserializeItemListToDic<T>( List<T> itemList ) where T : Item
        {
            Dictionary<string, List<GameObject>> itemDic = null;    // � ��ųʸ��� ������ ������ ����

            
            if( typeof(T)==typeof(ItemWeapon))
                itemDic = weapDic;
            else if( typeof(T)==typeof(ItemMisc) )
                itemDic = miscDic;
                        
            foreach(Item item in itemList) // ������ ����Ʈ���� ���� ������ ������ �ϳ��� �����ɴϴ�.
            {
                // ������ ������ �������� �ϴ� ���ӿ�����Ʈ�� ����ϴ�.
                GameObject itemObject = CreateManager.instance.CreateItemByInfo( item );
                        
                if( !itemDic.ContainsKey(item.Name) )       // ������ �ش��ϴ� �������� �̸��� ���ٸ�
                {
                    // ���ο� ����Ʈ�� ����� ������Ʈ�� �ֽ��ϴ�.
                    List<GameObject> itemObjList = new List<GameObject> { itemObject };          
                    
                    itemDic.Add(item.Name, itemObjList );   // �κ��丮 ������ ������ �̸����� ������Ʈ ����Ʈ�� �߰��մϴ�.
                }
                else                                        // ������ �ش��ϴ� �������� �̸��� �ִٸ�
                {
                    itemDic[item.Name].Add(itemObject);     // �κ��丮 ������ �����Ͽ�, �ش� ����Ʈ�� ������Ʈ�� �߰��մϴ�.
                }

            }
            
            

            // ����) ��ȭ�������� �ִ� ���� �˻縦 ���� ���� �ʽ��ϴ�.
            // ������ ���� �� MaxCount�̻��� �� ���� ó�� ������ ������, Inventory���� �̸� MaxCount�� ���� ���ο� ������Ʈ ���� ���� ó���ߴٰ� �Ǵ��մϴ�.
            // �� �̵��̳� �ε� ���� ��ġ�鼭 �ش� ������ ������ ���� ������Ʈ �״�� �����ؾ� �մϴ�.
        }



        /// <summary>
        /// �⺻ �κ��丮 �������Դϴ�. ���ο� ������ ������ �� ������ּ���.
        /// </summary>
        public Inventory()
        {
            weapDic=new Dictionary<string, List<GameObject>>();
            miscDic=new Dictionary<string, List<GameObject>>();
            WeapCountLimit=InitialCountLimit;
            MiscCountLimit=InitialCountLimit;
        }
               
    }




    /// <summary>
    /// ����ȭ ������ �κ��丮 Ŭ�����Դϴ�.<br/>
    /// ������ �κ��丮���� GameObject ������ ����Ʈ�� Item ������ ����Ʈ�� ��ȯ�Ͽ� �����մϴ�.<br/>
    /// </summary>
    [Serializable]
    public class SInventory
    {
        public List<ItemWeapon> weapList;
        public List<ItemMisc> miscList;
        public int WeapCountLimit;
        public int MiscCountLimit;
        
        /// <summary>
        /// SerializableInventory�� ����Ʈ �������Դϴ�. ����ȭ ������ �κ��丮 �ν��Ͻ��� ������ �ݴϴ�.
        /// </summary>
        public SInventory()
        {
            Serialize(new Inventory());
        }


        /// <summary>
        /// ������ �κ��丮 Ŭ������ �ν��Ͻ��� ���ڷ� �޾Ƽ� ����ȭ ������ �κ��丮 �ν��Ͻ��� ������ �ݴϴ�.
        /// </summary>
        public SInventory( Inventory inventory )
        {
            Serialize(inventory);
        }

         /// <summary>
        /// ���� �Ǿ��ִ� �κ��丮�� ������ȭ�Ͽ� �ε��ϴ� �޼����Դϴ�.<br/>
        /// �ε�Ǿ��ִ� ���κ����� �ڵ����� ��ȯ�Ͽ� Inventory �ν��Ͻ��� ��ȯ�޽��ϴ�.
        /// </summary>
        public Inventory Deserialize()
        {
            Inventory inventory = new Inventory();
                      
            inventory.DeserializeItemListToDic<ItemWeapon>( this.weapList );
            inventory.DeserializeItemListToDic<ItemMisc>( this.miscList );
            inventory.WeapCountLimit = this.WeapCountLimit;
            inventory.MiscCountLimit = this.MiscCountLimit;

            return inventory;
        }

        /// <summary>
        /// �κ��丮�� ����ȭ�Ͽ� �����ϴ� �޼����Դϴ�. ���ڷ� ������ �κ��丮�� �����Ͽ��� �մϴ�.
        /// </summary>
        public void Serialize(Inventory inventory)
        {
            this.weapList=inventory.SerializeDicToItemList<ItemWeapon>();
            this.miscList=inventory.SerializeDicToItemList<ItemMisc>();
            this.WeapCountLimit = inventory.WeapCountLimit;
            this.MiscCountLimit = inventory.MiscCountLimit;
        }




    }

}
