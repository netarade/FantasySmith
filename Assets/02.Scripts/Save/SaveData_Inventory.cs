using CreateManagement;
using InventoryManagement;
using ItemData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * <v1.1 - 2023_1231_�ֿ���>
 * 1- ������ ���� 
 * WeapSlotCountLimit -> SlotCountLimitWeap
 * MiscSlotCountLimit -> SlotCountLimitMisc
 * 
 * <V1.2 - 2024_0105_�ֿ���>
 * 1- Deserialize�޼��忡�� InventoryInfo�� �޵��� �Ͽ� 
 * �κ��丮�� ���Ӱ� ������ �� InventoryInfo�� ���ڷ� �־��ֵ��� ����
 * �̴� CreateManager�� �������� ��� ����
 * 
 * <v1.3 -2024_0111_�ֿ���>
 * 1- ������ Ŭ���� �߰��� ���� questList�� �߰�
 * 
 * <v2.0 - 2024_0112_�ֿ���>
 * 1- Initializer�� ���� �����ڸ� ȣ���ϵ��� ����
 * 2- slotCountLimit�� ���� ������ �ƴ϶� �迭�� ����
 * 
 * <v2.1 - 2024_0112_�ֿ���>
 * (�̽�)
 * 1- �̴ϼȶ����� ������ ���� JSON Deserialize�� ���� �ʴ� ������ �־ �ΰ��� ������� �ذ��� �����Ϸ��� ������
 * 
 * a. ����Ʈ �����ڸ� ȣ���� �� ���� Initialize�޼��带 ȣ�����ִ� ���
 * b. Initializer��ü�� ���� �����ϴ� ���
 * 
 * => ó������ b�̴ϼȶ������� ���� �����ϴ� ������� �����Ϸ� ������, 
 * �������� �����ϸ� �ȵǹǷ� ������ DicType, isReset�� ���� �����ؾ��ϴ� ����
 * 
 * a�� JSon�� Deserialize���ϰ� �ٷ� ��ȯ�ϱ� ���� Initialize�޼��带 �ѹ��� ȣ���Ͽ��� �ϴ� ������ �־���
 * 
 * => �ذ����� JSon�� Deserialize�� �̷������ �˾Ƽ� �Ű����� �����ڸ� ȣ���ع����� ������ �־����Ƿ�,
 * ����Ʈ �����ڸ� ȣ���ϰԲ� �� ����Ʈ ������ InventorySaveData�� SInventory�� ���� ������־���.
 * (�� ����Ʈ �����ڸ� ���� JSon�� �˾Ƽ� �迭 ũ����� ��Ƽ� �Ҵ��� �����ϱ� ����)
 * 
 * 
 * <v2.2 - 2024_0115_�ֿ���>
 * 1- slotCountLimit�������� slotCountLimitTap���� ����
 * �Ǻ� �������Ѽ��� ���� ������ �����ϸ鼭 
 * ������ slotCountLimitDic�� �������� �����Ͽ����� slotCountLimitTap�� �������� �����ϵ��� �Ͽ���.
 * 
 * <v2.3 - 2024_0116_�ֿ���>
 * 1- SInventory�����ڿ��� slotCountLimitTab�迭 ������ dicLen��ŭ �����ϴ� �Ϳ��� tabLen��ŭ �����ϵ��� ����
 * 
 * <v2.4 - 2024_0125_�ֿ���>
 * 1- ���� ItemBuildingŬ������ ItemMisc���� Item���� ��Ӱ��踦 �����Կ� ���� �ű� ItemType.Building �߰��Ͽ�
 * ���ο� ItemBuilding ����Ʈ�� buildingList������ �߰��ϰ� Serialize�� Deserialize�� �°� ���� 
 * 
 */


namespace DataManagement
{     
    /// <summary>
    /// ���� ���� - ����Ƽ ���� Ŭ������ ���� �Ұ�. �⺻ �ڷ������� �����ϰų�, ����ü �Ǵ� Ŭ������ ����� �����ؾ� �մϴ�.
    /// </summary>
    public class InventorySaveData : SaveData
    {        
        /// <summary>
        /// ����ȭ�Ǿ� ���� �Ǿ��ִ� �÷��̾��� �κ��丮�Դϴ�.<br/>
        /// ����� Serialize�޼��带 ���� InventoryŬ������ �ν��Ͻ��� ���ڷ� �����Ͽ� ȣ���ϰ�,<br/>
        /// �ε�� Deserialize�޼��带 ����ؼ� ���� Inventory Ŭ������ �ν��Ͻ��� ��ȯ�޾� ����ϼ���.
        /// </summary>
        public SInventory savedInventory;
        

        public InventorySaveData() { }


        /// <summary>
        /// DataManager���� Load�޼��忡�� ���ο� GameData�� �����ϱ� ���� �������Դϴ�.<br/>
        /// ������ �����Ͱ� ���� ��� ���˴ϴ�.
        /// </summary>
        public InventorySaveData(InventoryInitializer initializer)
        {
            if(initializer==null)
                throw new Exception("�̴ϼȶ������� ���޵��� �ʾҽ��ϴ�.");
            
            savedInventory = new SInventory(initializer);     // ���ο� ����ȭ �κ��丮 ����             
        }
              
    }


    /// <summary>
    /// ����ȭ ������ �κ��丮 Ŭ�����Դϴ�.<br/>
    /// ������ �κ��丮���� GameObject ������ ����Ʈ�� Item ������ ����Ʈ�� ��ȯ�Ͽ� �����մϴ�.<br/>
    /// </summary>
    [Serializable]
    public class SInventory
    {        
        /*** ���� Ŭ���� ���·� �����ؾ� �ϸ�, (������ Ŭ������ �߰��� �� ����) ��� Ÿ���� ��ųʸ� ������ �����ؾ� �մϴ�. ***/
        public List<ItemWeapon> weapList;
        public List<ItemMisc> miscList;
        public List<ItemQuest> questList;
        public List<ItemBuilding> buildingList;

        public int[] slotCountLimitTab;


        public SInventory() { }
                            

        /// <summary>
        /// SerializableInventory�� �������Դϴ�.<br/>
        /// InventoryInitializer���� ���� �� DicType[]�� ���ڷ� �޽��ϴ�. <br/>
        /// ����ȭ ������ �κ��丮 �ν��Ͻ��� ������ �ݴϴ�.
        /// </summary>
        public SInventory(InventoryInitializer initializer)
        {
            if(initializer== null)
                throw new Exception("�̴ϼȶ������� ���޵��� �ʾҽ��ϴ�.");

            // ���� ��ü ���̸�ŭ �� �迭�� �����մϴ�.
            int tabLen = (int)TabType.None;
            slotCountLimitTab = new int[tabLen];

            // ���ο� �κ��丮�� �̴ϼȶ������� �����Ͽ� ���� �� SInventory�� �ʱ�ȭ�մϴ�.
            Serialize( new Inventory(initializer) );
        }



        /// <summary>
        /// ���Ϸ� ���� �Ǿ��ִ� �κ��丮�� ���� ���� �߿� ����ϱ� ���Ͽ� ������ȭ�Ͽ� �ε��ϴ� �޼����Դϴ�.<br/>
        /// ���Ͽ� ����Ǿ� �ִ� SInventory�� ������ ���� Inventory �ν��Ͻ��� ����� ��ȯ�մϴ�.<br/><br/>
        /// Initializer�� CreateManager�� �����ؾ� �մϴ�.
        /// </summary>
        public Inventory Deserialize(InventoryInitializer initializer, CreateManager createManager)
        {
            if( initializer== null || createManager==null )
                throw new Exception("�̴ϼȶ������� createManager �������� ���޵��� �ʾҽ��ϴ�.");

            // �� �κ��丮�� �����, ���Ͽ� ����Ǿ��ִ� SInventory�� ������ Inventory�� �ű�ϴ�. (����Ⱑ ����˴ϴ�.)
            Inventory inventory = new Inventory(initializer, createManager);
                      
            inventory.DeserializeItemListToDic<ItemWeapon>( this.weapList );
            inventory.DeserializeItemListToDic<ItemMisc>( this.miscList );
            inventory.DeserializeItemListToDic<ItemQuest>( this.questList );
            inventory.DeserializeItemListToDic<ItemBuilding>( this.buildingList );

            for( int i = 0; i<inventory.tabLen; i++ )
                inventory.slotCountLimitTab[i] = this.slotCountLimitTab[i];
            
            return inventory;
        }

        /// <summary>
        /// �κ��丮�� ���� ����� �� ����ȭ�Ͽ� ���Ϸ� �����ϴ� �޼����Դϴ�.<br/>
        /// ���ڷ� ���� �߿� ������Ʈ �� �ֽ� �κ��丮�� ���޹޽��ϴ�.
        /// </summary>
        public void Serialize( Inventory inventory )
        {
            if(inventory==null)
                throw new Exception("�κ��丮 �������� ���޵��� �ʾҽ��ϴ�.");

            // SInventory�� �ֽ� Inventory�� ������ �Է��մϴ�.
            this.weapList=inventory.SerializeDicToItemList<ItemWeapon>();
            this.miscList=inventory.SerializeDicToItemList<ItemMisc>();
            this.questList=inventory.SerializeDicToItemList<ItemQuest>();
            this.buildingList = inventory.SerializeDicToItemList<ItemBuilding>();
            
            for(int i=0; i<inventory.tabLen; i++)
                this.slotCountLimitTab[i] = inventory.slotCountLimitTab[i];
        }




    }



    
}