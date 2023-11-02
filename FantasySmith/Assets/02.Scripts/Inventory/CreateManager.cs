using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using System;
using UnityEditor.UIElements;

/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1101_최원준>
 * 1 - 테스트용 클래스 작성
 * 
 * <v1.1 - 2023_1101_최원준>
 * 1 - 아이템 클래스 기반으로 아이템 초기화, 구매, 제작, 숙련도 변화 예시를 만들어봄.
 * 2 - 기타 주석 처리
 * 
 */
public class CreateManager : MonoBehaviour
{
    public ItemImageCollection iicMisc;                     // 인스펙터 뷰 상에서 등록할 잡화아이템 이미지 집합
    public ItemImageCollection iicWeapon;                   // 인스펙터 뷰 상에서 등록할 무기아이템 이미지 집합

    Dictionary<string, Item> miscDic;                       // 게임 시작 시 넣어 둘 전체 잡화아이템 사전 
    Dictionary<string, Item> weaponDic;                     // 게임 시작 시 넣어 둘 전체 무기아이템 사전 

    Dictionary<string, CraftProficiency> playerCraftDic;    // 제작 성공 시 증가하는 숙련도 목록
    List<Item> playerInventory;                             // 아이템을 넣어서 관리하는 인벤토리



    void Start()
    {       
        
        #region 초기 아이템목록 초기화 예시
        // 플레이어와 상관없이 게임 시스템 자체가 들고 있어야할 집합이며,
        // 플레이어는 아이템이 생성될 때 이 집합에서 복제해서 들고있게 될 것이다.

        miscDic=new Dictionary<string, Item>()
        {
            { "철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000000", "철", 3.0f, iicMisc.icArrImg[0] ) },
            { "강철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000001", "강철", 5.0f, iicMisc.icArrImg[1] ) },
            { "흑철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000002", "흑철", 7.0f, iicMisc.icArrImg[2] ) },
            { "미스릴", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000003", "미스릴", 20.0f, iicMisc.icArrImg[3] ) }
        };
        weaponDic=new Dictionary<string, Item>()
        {
            { "철검", new ItemWeapon( ItemType.Weapon, WeaponType.Sword, "0001000", "철검", 10.0f, iicWeapon.icArrImg[0]) },
            { "강철검", new ItemWeapon( ItemType.Weapon, WeaponType.Sword, "0001001", "강철검", 20.0f, iicWeapon.icArrImg[0]) }
        };

        #endregion



        #region 플레이어의 장비 아이템 복제 예시( 제작, 아이템 구매 등을 통해 인벤토리에 아이템이 생성된다.)

        // 인벤토리 슬롯의 남아있는 칸을 확인하여야 한다.
        int maxCount = 50;
        if( playerInventory.Count >= maxCount )
            return;

        // 사전의 아이템을 클론하여 (아이템 원형을 복제해서) 아이템을 생성한다.
        ItemWeapon weaponItem = (ItemWeapon)weaponDic["철검"].Clone();

        /**** 1. 빈오브젝트 만들어서 컴포넌트 붙인다음 집어넣기 ****/
        /*         
        gameobject gameobject = new gameobject();     
        gameobject.addcomponent<iteminfo>();
        gameobject.getcomponent<iteminfo>().item = weaponitem;
        //관리가 어려워 지므로 사용하지 않는다.
        */

        /**** 2. 이미 있는 오브젝트를 복제 생성하여 Item을 넣어주기 ****/
        
        GameObject itemPrefab = Resources.Load<GameObject>("ItemOrigin");   //start구문에서 이루어지는 동작
        // 슬롯리스트의 해당 슬롯에 게임 상에서 보여 질 오브젝트를 생성하여 배치한다.
        GameObject itemObject = Instantiate( itemPrefab, GameObject.Find( "Inventory" ).transform.GetChild(0).GetChild(0) );
                                                            
        // 스크립트 상의 item에 사전에서 클론한 아이템을 참조하게 한다.        
        itemObject.GetComponent<ItemInfo>().item = weaponItem;
        itemObject.GetComponent<ItemInfo>().OnItemAdded(); // 아이템 오브젝트에 아이템참조값을 넣었다면 이미지를 반영해야한다.

        //gameObject.AddComponent();
        playerInventory.Add(weaponItem);    // 개념적 인벤토리에 아이템을 넣어준다.
        
        #endregion








        #region 플레이어의 잡화 아이템 구매 예시

        string purchasedItemName = "미스릴";  // 잡화아이템 구매이름이라고 가정한다.
        int purchasedCnt = 5;                // 잡화아이템의 구매횟수이다.

        foreach( Item item in playerInventory )
        {
            if( item.Name==purchasedItemName )
            {
                ( (ItemMisc)item ).InventoryCount=purchasedCnt;  // 인벤토리의 아이템카운트만 올린다.                
            }
            else
            {
                ItemMisc purchasedItem = (ItemMisc)miscDic["미스릴"].Clone();
                purchasedItem.InventoryCount=purchasedCnt;

                playerInventory.Add( purchasedItem );     // 인벤토리 리스트에 새롭게 추가한다.
            }
        }

        #endregion








        #region 숙련도, 레시피 적용 예시

        playerCraftDic=new Dictionary<string, CraftProficiency>();

        if( !playerCraftDic.ContainsKey( "철검" ) ) //현재 숙련도 목록에 이름이 존재하지 않는다면, (목록에 넣고 수정한다.)
            playerCraftDic.Add( "철검", new CraftProficiency() );

        playerCraftDic["철검"].Proficiency++;

        // if(isRecipieSucced)                          
        playerCraftDic["철검"].RecipieHitCount++; // 레시피까지 성공해서 만들었을 때      

        #endregion


    }


}
