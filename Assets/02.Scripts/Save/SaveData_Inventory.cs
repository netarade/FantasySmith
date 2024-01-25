using CreateManagement;
using InventoryManagement;
using ItemData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * <v1.1 - 2023_1231_최원준>
 * 1- 변수명 변경 
 * WeapSlotCountLimit -> SlotCountLimitWeap
 * MiscSlotCountLimit -> SlotCountLimitMisc
 * 
 * <V1.2 - 2024_0105_최원준>
 * 1- Deserialize메서드에서 InventoryInfo를 받도록 하여 
 * 인벤토리를 새롭게 생성할 때 InventoryInfo를 인자로 넣어주도록 수정
 * 이는 CreateManager의 참조값을 얻기 위함
 * 
 * <v1.3 -2024_0111_최원준>
 * 1- 아이템 클래스 추가로 인해 questList를 추가
 * 
 * <v2.0 - 2024_0112_최원준>
 * 1- Initializer를 통해 생성자를 호출하도록 구현
 * 2- slotCountLimit을 개별 변수가 아니라 배열로 구현
 * 
 * <v2.1 - 2024_0112_최원준>
 * (이슈)
 * 1- 이니셜라이저 전달을 통한 JSON Deserialize가 되지 않는 문제가 있어서 두가지 방법으로 해결을 진행하려고 했으나
 * 
 * a. 디폴트 생성자를 호출한 후 따로 Initialize메서드를 호출해주는 방식
 * b. Initializer자체를 같이 저장하는 방식
 * 
 * => 처음에는 b이니셜라이져를 같이 저장하는 방식으로 구현하려 했으나, 
 * 참조값을 저장하면 안되므로 내부의 DicType, isReset을 따로 저장해야하는 문제
 * 
 * a는 JSon의 Deserialize를하고 바로 반환하기 전에 Initialize메서드를 한번더 호출하여야 하는 문제가 있었음
 * 
 * => 해결방법은 JSon이 Deserialize가 이루어질때 알아서 매개변수 생성자를 호출해버리는 문제가 있었으므로,
 * 디폴트 생성자를 호출하게끔 빈 디폴트 생성자 InventorySaveData와 SInventory를 따로 만들어주었음.
 * (빈 디폴트 생성자를 만들어도 JSon이 알아서 배열 크기까지 잡아서 할당을 진행하기 때문)
 * 
 * 
 * <v2.2 - 2024_0115_최원준>
 * 1- slotCountLimit변수명을 slotCountLimitTap으로 수정
 * 탭별 슬롯제한수의 공유 개념을 도입하면서 
 * 기존에 slotCountLimitDic을 기준으로 저장하였으나 slotCountLimitTap을 기준으로 저장하도록 하였음.
 * 
 * <v2.3 - 2024_0116_최원준>
 * 1- SInventory생성자에서 slotCountLimitTab배열 생성시 dicLen만큼 생성하던 것에서 tabLen만큼 생성하도록 수정
 * 
 * <v2.4 - 2024_0125_최원준>
 * 1- 기존 ItemBuilding클래스를 ItemMisc에서 Item으로 상속관계를 변경함에 따라서 신규 ItemType.Building 추가하여
 * 새로운 ItemBuilding 리스트인 buildingList변수를 추가하고 Serialize와 Deserialize도 맞게 수정 
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
        

        public InventorySaveData() { }


        /// <summary>
        /// DataManager에서 Load메서드에서 새로운 GameData를 생성하기 위한 생성자입니다.<br/>
        /// 기존의 데이터가 없을 경우 사용됩니다.
        /// </summary>
        public InventorySaveData(InventoryInitializer initializer)
        {
            if(initializer==null)
                throw new Exception("이니셜라이저가 전달되지 않았습니다.");
            
            savedInventory = new SInventory(initializer);     // 새로운 직렬화 인벤토리 생성             
        }
              
    }


    /// <summary>
    /// 직렬화 가능한 인벤토리 클래스입니다.<br/>
    /// 기존의 인벤토리에서 GameObject 형식의 리스트를 Item 형식의 리스트로 변환하여 보관합니다.<br/>
    /// </summary>
    [Serializable]
    public class SInventory
    {        
        /*** 개별 클래스 상태로 저장해야 하며, (아이템 클래스가 추가될 때 마다) 모든 타입의 딕셔너리 변수를 선언해야 합니다. ***/
        public List<ItemWeapon> weapList;
        public List<ItemMisc> miscList;
        public List<ItemQuest> questList;
        public List<ItemBuilding> buildingList;

        public int[] slotCountLimitTab;


        public SInventory() { }
                            

        /// <summary>
        /// SerializableInventory의 생성자입니다.<br/>
        /// InventoryInitializer에서 세팅 한 DicType[]을 인자로 받습니다. <br/>
        /// 직렬화 가능한 인벤토리 인스턴스를 생성해 줍니다.
        /// </summary>
        public SInventory(InventoryInitializer initializer)
        {
            if(initializer== null)
                throw new Exception("이니셜라이져가 전달되지 않았습니다.");

            // 탭의 전체 길이만큼 탭 배열을 생성합니다.
            int tabLen = (int)TabType.None;
            slotCountLimitTab = new int[tabLen];

            // 새로운 인벤토리를 이니셜라이저를 전달하여 생성 후 SInventory를 초기화합니다.
            Serialize( new Inventory(initializer) );
        }



        /// <summary>
        /// 파일로 저장 되어있는 인벤토리를 게임 진행 중에 사용하기 위하여 역직렬화하여 로드하는 메서드입니다.<br/>
        /// 파일에 저장되어 있는 SInventory의 정보를 토대로 Inventory 인스턴스를 만들어 반환합니다.<br/><br/>
        /// Initializer와 CreateManager를 전달해야 합니다.
        /// </summary>
        public Inventory Deserialize(InventoryInitializer initializer, CreateManager createManager)
        {
            if( initializer== null || createManager==null )
                throw new Exception("이니셜라이져와 createManager 참조값이 전달되지 않았습니다.");

            // 새 인벤토리를 만들고, 파일에 저장되어있는 SInventory의 정보를 Inventory로 옮깁니다. (덮어쓰기가 진행됩니다.)
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
        /// 인벤토리를 씬이 종료된 후 직렬화하여 파일로 저장하는 메서드입니다.<br/>
        /// 인자로 게임 중에 업데이트 된 최신 인벤토리를 전달받습니다.
        /// </summary>
        public void Serialize( Inventory inventory )
        {
            if(inventory==null)
                throw new Exception("인벤토리 참조값이 전달되지 않았습니다.");

            // SInventory에 최신 Inventory의 정보를 입력합니다.
            this.weapList=inventory.SerializeDicToItemList<ItemWeapon>();
            this.miscList=inventory.SerializeDicToItemList<ItemMisc>();
            this.questList=inventory.SerializeDicToItemList<ItemQuest>();
            this.buildingList = inventory.SerializeDicToItemList<ItemBuilding>();
            
            for(int i=0; i<inventory.tabLen; i++)
                this.slotCountLimitTab[i] = inventory.slotCountLimitTab[i];
        }




    }



    
}