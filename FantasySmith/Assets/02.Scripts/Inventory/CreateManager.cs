using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using CraftData;
using System;

/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1101_최원준>
 * 1- 테스트용 클래스 작성
 * 
 * <v1.1 - 2023_1101_최원준>
 * 1- 아이템 클래스 기반으로 아이템 초기화, 구매, 제작, 숙련도 변화 예시를 만들어봄.
 * 2- 기타 주석 처리
 * 
 * <v2.0 - 2023_1103_최원준>
 * 1- 중복, 복잡한 로직 메서드화하여 간소화 및 재사용 가능하게 변경
 * 
 * 2- playerInventory가 Item을 가지는 것이 아니라 GameObject를 가지도록 수정.
 * playerInventory는 게임 시작 시 플레이어가 가지게 될 리스트 변수이며, 게임 종료와 시작 시 저장 및 로드해야 할 변수이다.
 * 이 리스트에는 개념아이템이 아니라 아이템오브젝트를 보관해야 한다. 아이템 오브젝트에는 개념아이템 정보를 포함한다.
 * 
 * <v3.0 - 2023_1104_최원준>
 * 1- 플레이어 인벤토리를 GameObject를 보관하는 weapList, miscList로 분리하여 관리하고 구조체로 통합
 * 
 * <v3.1 - 2023_1105_최원준>
 * 1- ItemInfo의 OnItemAdded 메서드의 호출 방식 내부자동 호출 변경으로 인해서 
 * 해당 메서드 사용구문을 삭제
 * 2- CreateManager의 접근 편리성과 딕셔너리 및 메서드 항시 상주화 필요성으로 인해 싱글톤으로 수정. 
 * 
 * <v3.2 - 2023_1106_최원준>
 * 1- 무기 아이템 사전 클래스 설계로 인한 주석처리
 */


/// <summary>
/// 아이템 생성에 관한 로직을 담당하는 클래스이며,<br/>
/// 아이템 생성을 원하는 시점에 싱글톤 인스턴스를 참조하여 관련 메서드를 호출하면 됩니다.
/// </summary>
public class CreateManager : MonoBehaviour
{
    [SerializeField] ItemImageCollection iicMisc;           // 인스펙터 뷰 상에서 등록할 잡화아이템 이미지 집합
    [SerializeField] ItemImageCollection iicWeapon;         // 인스펙터 뷰 상에서 등록할 무기아이템 이미지 집합

    public Dictionary<string, Item> miscDic;                // 게임 시작 시 넣어 둘 전체 잡화아이템 사전 
    public Dictionary<string, Item> weaponDic;              // 게임 시작 시 넣어 둘 전체 무기아이템 사전
    Dictionary<string, CraftProficiency> playerCraftDic;    // 제작 성공 시 증가하는 숙련도 목록

    [SerializeField] Transform slotListTr;                  // 인벤토리의 슬롯 오브젝트의 트랜스폼 참조


    GameObject itemPrefab;                                  // 리소스 폴더 또는 직접 드래그해서 복제할 오브젝트를 등록해야 한다.        
        
    int inventoryMaxCount;                      // 인벤토리의 최대 칸수 - 시설 업그레이드 등에 따라 가변 될 수 있다.
    public static CreateManager instance;       // 매니저 싱글톤 인스턴스 생성



    public struct PlayerInventory
    {
        List<GameObject> weapList;
        List<GameObject> miscList; 
    }

    List<GameObject> weapList;              // 무기 아이템을 넣어서 관리하는 인벤토리
    List<GameObject> miscList;              // 잡화 아이템을 넣어서 관리하는 인벤토리

    public void Awake()
    {
        if(instance==null)
        {
            instance = this;            
            DontDestroyOnLoad(instance);
        }
        else if(instance != null )
            Destroy(this.gameObject);           // 싱글톤 이외 인스턴스는 삭제
    }






    /// <summary>
    /// 가장 가까운 슬롯에 원하는 아이템을 생성합니다.<br/>
    /// 현재 인벤토리 슬롯에 빈자리가 없다면 false를 반환합니다.<br/>
    /// * 이름이 일치하지 않을 경우 예외를 발생시킵니다. *
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inventoryList">플레이어의 인벤토리 변수가 필요합니다. 해당 리스트에 아이템 오브젝트를 추가하여 줍니다.</param>
    /// <param name="itemName">아이템 테이블을 참고하여 한글명을 기입해주세요. 띄어쓰기에 유의해야 합니다. ex)철, 철 검, 미스릴, 미스릴 검</param>
    /// <param name="count">생성을 원하는 갯수입니다. 하나의 슬롯에 중첩하여 생성될 것입니다. 기본 값은 1이며, 장비류는 반드시 1개만 생성됩니다.</param>
    /// <returns></returns>
    public bool CreateItemToNearstSlot(List<GameObject> inventoryList, string itemName, int count = 1)
    {
        int findSlotIdx = FindNearstRemainSlotIdx();

        if( findSlotIdx == -1 ) // 인벤토리의 남아있는 칸이 없다면 생성하지 못한다.
            return false;
                
        
        // 사전의 개념 아이템을 클론하여 (아이템 원형을 복제해서) 또 다른 개념 아이템을 생성한다.
        Item itemClone;

        if( weaponDic.ContainsKey( itemName ) )
        {
            itemClone=(ItemWeapon)weaponDic[itemName].Clone();
        }
        else if( miscDic.ContainsKey( itemName ) )
        {
            itemClone=(ItemMisc)miscDic[itemName].Clone();
            ((ItemMisc)itemClone).InventoryCount=count;         // 중첩 갯수를 지정합니다.
        }
        else
        {
            throw new Exception("아이템 명이 정확하게 일치하지 않습니다. 확인하여 주세요.");
        }
                
        // 개념 아이템의 슬롯의 인덱스 정보를 찾은 위치로 수정한다.
        itemClone.SlotIndex = findSlotIdx;

        // 슬롯리스트의 해당 슬롯에 게임 상에서 보여 질 오브젝트를 생성하여 배치한다.
        GameObject itemObject = Instantiate( itemPrefab, slotListTr.GetChild(findSlotIdx) );                

        // 스크립트 상의 item에 사전에서 클론한 아이템을 참조하게 한다.        
        itemObject.GetComponent<ItemInfo>().Item = itemClone;

        inventoryList.Add(itemObject);    // 인벤토리는 GameObject 인스턴스를 보관함으로서 transform정보와 개념 아이템을 정보를 포함하게 된다.
        
        return true;
    }

    /// <summary>
    /// 남아있는 슬롯 중에서 가장 작은 인덱스를 반환합니다. 슬롯이 꽉 찬경우 -1을 반환합니다.
    /// </summary>
    public int FindNearstRemainSlotIdx() 
    {
        int findIdx = -1;

        for( int i = 0; i<slotListTr.childCount; i++ )
        {
            if( slotListTr.GetChild(i).childCount!=0 )  // 해당 슬롯리스트에 자식이 있다면 다음 슬롯리스트로 넘어간다.
                continue;

            findIdx = i;
            break;
        }

        return findIdx;     // findIdx가 수정되지 않았다면 -1을 반환한다. 수정되었다면 0이상의 인덱스값을 반환한다.
    }

    /// <summary>
    /// 월드의 모든 개념 아이템을 담아두는 딕셔너리를 초기화합니다.
    /// </summary>
    private void CreateAllItemDictionary()
    {
        // 플레이어와 상관없이 게임 시스템 자체가 들고 있어야할 집합이며,
        // 플레이어는 아이템이 생성될 때 이 집합에서 복제해서 들고있게 될 것이다.

        miscDic=new Dictionary<string, Item>()
        {           //0x1000
            { "철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000000", "철", 3.0f, iicMisc.icArrImg[0] ) },
            { "강철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000001", "강철", 5.0f, iicMisc.icArrImg[1] ) },
            { "흑철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000002", "흑철", 7.0f, iicMisc.icArrImg[2] ) },
            { "미스릴", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000003", "미스릴", 20.0f, iicMisc.icArrImg[3] ) }
        };

        //CraftMaterial = new CraftMaterial("철", 2);


        //weaponDic=new Dictionary<string, Item>()
        //{
        //    { "철 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001000", "철 검", 10.0f, iicWeapon.icArrImg[0]
        //        , ItemGrade.Low, 10, 100, 1.0f, 10, Attribute.None
        //        ,
        //    ) },
        //    { "강철 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001001", "강철 검", 20.0f, iicWeapon.icArrImg[1]
                                        
        //    ) }
        //};
    }

    
    void Start()
    { 

        itemPrefab = Resources.Load<GameObject>("ItemOrigin");  // 리소스 폴더에서 원본 프리팹 가져오기
        inventoryMaxCount = 50;                                 // 인벤토리 최대 수치이며, 이는 게임 상에서 변경될 가능 성이 있다.
        slotListTr = GameObject.Find("SlotList").transform;     // 슬롯리스트를 인식 시켜 남아있는 슬롯을 확인할 것이다.

        // 인스펙터뷰 상에서 달아놓은 스프라이트 이미지 집합을 참조한다.
        iicMisc = GameObject.Find("ImageCollections").transform.GetChild(0).GetComponent<ItemImageCollection>();
        iicWeapon = GameObject.Find("ImageCollections").transform.GetChild(1).GetComponent<ItemImageCollection>();
        
        // 모든 월드 아이템 등록
        CreateAllItemDictionary();









        #region 플레이어의 장비 아이템 복제 예시( 제작, 아이템 구매 등을 통해 인벤토리에 아이템이 생성된다.)
        weapList = new List<GameObject>();        
        miscList = new List<GameObject>();



        CreateItemToNearstSlot(miscList, "철", 5);
        CreateItemToNearstSlot(weapList, "철 검");

        #endregion





        #region 플레이어의 잡화 아이템 구매 예시

        string purchasedItemName = "미스릴";  // 잡화아이템 구매이름이라고 가정한다.
        int purchasedCnt = 5;                // 잡화아이템의 구매횟수이다.

        foreach( GameObject itemObj in miscList )
        {
            ItemMisc item = (ItemMisc)itemObj.GetComponent<ItemInfo>().item;      // 오브젝트가 가지고 있는 아이템 참조값을 받아온다.

            if( item.Name==purchasedItemName )                      // NPC에게서 구매할 아이템이 현재 인벤토리의 아이템 이름과 같다면,
            {
                item.InventoryCount=purchasedCnt;     // 인벤토리의 아이템카운트만 올린다.                
            }
            else                                                    // NPC에게서 구매할 아이템이 현재 인벤토리에 없다면,
            {
                CreateItemToNearstSlot( miscList, purchasedItemName, purchasedCnt );
                break;
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
