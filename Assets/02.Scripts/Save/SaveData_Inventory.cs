using InventoryManagement;
using ItemData;
using System;
using System.Collections.Generic;

/*
 * <v1.1 - 2023_1231_최원준>
 * 1- 변수명 변경 
 * WeapSlotCountLimit -> SlotCountLimitWeap
 * MiscSlotCountLimit -> SlotCountLimitMisc
 * 
 */


namespace DataManagement
{     
    /// <summary>
    /// 주의 사항 - 유니티 전용 클래스는 저장 불가. 기본 자료형으로 저장하거나, 구조체 또는 클래스를 만들어 저장해야 합니다.
    /// </summary>
    public class InventorySaveData : SaveData
    {
        
        /// <summary>
        /// 직렬화되어 저장 되어있는 플레이어의 인벤토리입니다.<br/>
        /// 저장시 Serialize메서드를 기존 Inventory클래스의 인스턴스를 인자로 전달하여 호출하고,<br/>
        /// 로드시 Deserialize메서드를 사용해서 기존 Inventory 클래스의 인스턴스를 반환받아 사용하세요.
        /// </summary>
        public SInventory savedInventory;
              

        /// <summary>
        /// DataManager에서 Load메서드에서 새로운 GameData를 생성하기 위한 생성자입니다.<br/>
        /// 기존의 데이터가 없을 경우 사용됩니다.
        /// </summary>
        public InventorySaveData()
        {
            savedInventory = new SInventory();     // 새로운 직렬화 인벤토리 생성  
        }


    }


    /// <summary>
    /// 직렬화 가능한 인벤토리 클래스입니다.<br/>
    /// 기존의 인벤토리에서 GameObject 형식의 리스트를 Item 형식의 리스트로 변환하여 보관합니다.<br/>
    /// </summary>
    [Serializable]
    public class SInventory
    {
        public List<ItemWeapon> weapList;
        public List<ItemMisc> miscList;
        public int slotCountLimitWeap;
        public int slotCountLimitMisc;
        
        /// <summary>
        /// SerializableInventory의 디폴트 생성자입니다. 직렬화 가능한 인벤토리 인스턴스를 생성해 줍니다.
        /// </summary>
        public SInventory()
        {
            Serialize(new Inventory());
        }


        /// <summary>
        /// 기존의 인벤토리 클래스의 인스턴스를 인자로 받아서 직렬화 가능한 인벤토리 인스턴스를 생성해 줍니다.
        /// </summary>
        public SInventory( Inventory inventory )
        {
            Serialize(inventory);
        }

         /// <summary>
        /// 저장 되어있는 인벤토리를 역직렬화하여 로드하는 메서드입니다.<br/>
        /// 로드되어있는 내부변수를 자동으로 변환하여 Inventory 인스턴스를 반환받습니다.
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
        /// 인벤토리를 직렬화하여 저장하는 메서드입니다. 인자로 저장할 인벤토리를 전달하여야 합니다.
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