using InventoryManagement;
using ItemData;
using System;
using System.Collections.Generic;

/*
 * <v1.1 - 2023_1231_�ֿ���>
 * 1- ������ ���� 
 * WeapSlotCountLimit -> SlotCountLimitWeap
 * MiscSlotCountLimit -> SlotCountLimitMisc
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
              

        /// <summary>
        /// DataManager���� Load�޼��忡�� ���ο� GameData�� �����ϱ� ���� �������Դϴ�.<br/>
        /// ������ �����Ͱ� ���� ��� ���˴ϴ�.
        /// </summary>
        public InventorySaveData()
        {
            savedInventory = new SInventory();     // ���ο� ����ȭ �κ��丮 ����  
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
        public int slotCountLimitWeap;
        public int slotCountLimitMisc;
        
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
            inventory.SlotCountLimitWeap = this.slotCountLimitWeap;
            inventory.SlotCountLimitMisc = this.slotCountLimitMisc;

            return inventory;
        }

        /// <summary>
        /// �κ��丮�� ����ȭ�Ͽ� �����ϴ� �޼����Դϴ�. ���ڷ� ������ �κ��丮�� �����Ͽ��� �մϴ�.
        /// </summary>
        public void Serialize(Inventory inventory)
        {
            this.weapList=inventory.SerializeDicToItemList<ItemWeapon>();
            this.miscList=inventory.SerializeDicToItemList<ItemMisc>();
            this.slotCountLimitWeap = inventory.SlotCountLimitWeap;
            this.slotCountLimitMisc = inventory.SlotCountLimitMisc;
        }




    }



    
}