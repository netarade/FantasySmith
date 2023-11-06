using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using CraftData;
using System;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

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
 * 1- 무기 아이템 사전 아이템 클래스 추가 설계로 인한 주석처리
 * 2- 잡화 및 무기아이템 사전 입력 진행 중(미완료)
 * 
 * <v3.3 - 2023_1106_최원준>
 * 1- 무기, 잡화 아이템 사전 입력완료
 * 2- 탭 클릭 이벤트 추가
 * 각 탭을 클릭할 때 해당 오브젝트만 보여지도록 하였음.
 * 
 * 3-
 */


/// <summary>
/// 아이템 생성에 관한 로직을 담당하는 클래스이며,<br/>
/// 아이템 생성을 원하는 시점에 싱글톤 인스턴스를 참조하여 관련 메서드를 호출하면 됩니다.
/// </summary>
public class CreateManager : MonoBehaviour
{
    [SerializeField] ItemImageCollection iicMiscBase;           // 인스펙터 뷰 상에서 등록할 잡화 기본 아이템 이미지 집합
    [SerializeField] ItemImageCollection iicMiscAdd;            // 인스펙터 뷰 상에서 등록할 잡화 추가 아이템 이미지 집합
    [SerializeField] ItemImageCollection iicMiscOther;          // 인스펙터 뷰 상에서 등록할 잡화 기타 아이템 이미지 집합
    [SerializeField] ItemImageCollection iicWeaponSword;        // 인스펙터 뷰 상에서 등록할 무기 검 아이템 이미지 집합
    [SerializeField] ItemImageCollection iicWeaponBow;          // 인스펙터 뷰 상에서 등록할 무기 활 아이템 이미지 집합

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

    private Button btnTapAll;               // 버튼 탭을 눌렀을 때 각 탭에 맞는 아이템이 표시되도록 하기 위한 참조
    private Button btnTapWeap;
    private Button btnTapMisc;



    List<string> playerCraftableList;
    






    // 아이템이 Change되었음을 반영하여 PlyerInventory를 사용하는 모든 스크립트가 변경된 inventory를 참조하도록 한다.
    // Delegate





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

    
    void Start()
    {         
        itemPrefab = Resources.Load<GameObject>("ItemOrigin");  // 리소스 폴더에서 원본 프리팹 가져오기
        inventoryMaxCount = 50;                                 // 인벤토리 최대 수치이며, 이는 게임 상에서 변경될 가능 성이 있다.
        slotListTr = GameObject.Find("SlotList").transform;     // 슬롯리스트를 인식 시켜 남아있는 슬롯을 확인할 것이다.

        // 인스펙터뷰 상에서 달아놓은 스프라이트 이미지 집합을 참조한다.
        iicMiscBase = GameObject.Find("ImageCollections").transform.GetChild(0).GetComponent<ItemImageCollection>();
        iicMiscAdd = GameObject.Find("ImageCollections").transform.GetChild(1).GetComponent<ItemImageCollection>();
        iicMiscOther = GameObject.Find("ImageCollections").transform.GetChild(2).GetComponent<ItemImageCollection>();
        iicWeaponSword = GameObject.Find("ImageCollections").transform.GetChild(3).GetComponent<ItemImageCollection>();
        iicWeaponBow = GameObject.Find("ImageCollections").transform.GetChild(4).GetComponent<ItemImageCollection>();
        
        // 모든 월드 아이템 등록
        CreateAllItemDictionary();


        // 탭 버튼 참조
        btnTapAll = GameObject.Find("Inventory").transform.GetChild(1).GetComponent<Button>();
        btnTapWeap = GameObject.Find("Inventory").transform.GetChild(2).GetComponent<Button>();
        btnTapMisc = GameObject.Find("Inventory").transform.GetChild(3).GetComponent<Button>();
        btnTapMisc.Select();    // 첫 시작 시 Select 표시

        btnTapAll.onClick.AddListener( () => BtnTapClick(0) );
        btnTapWeap.onClick.AddListener( () => BtnTapClick(1) );
        btnTapMisc.onClick.AddListener( () => BtnTapClick(2) );

        
        
        
        



        






        #region 플레이어의 장비 아이템 복제 예시( 제작, 아이템 구매 등을 통해 인벤토리에 아이템이 생성된다.)
        weapList = new List<GameObject>();  // 원래는 게임 시작 시 로드 하여 인벤토리를 관리한다.      
        miscList = new List<GameObject>();  

        // 상점 아이템 구매 예시
        CreateItemToNearstSlot(miscList, "철", 2);

        // 무기아이템 제작 예시
        CreateItemToNearstSlot(weapList, "철 검");
        ItemCraftWeapon craftWeapon = (ItemCraftWeapon)weapList[weapList.Count-1].GetComponent<ItemInfo>().item;    //마지막에 들어간 아이템
        craftWeapon.BasePerformance = 750f;

        
        CreateItemToNearstSlot(weapList, "미스릴 검");
        CreateItemToNearstSlot(weapList, "얼음칼날");

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

    /// <summary>
    /// 버튼 클릭 시 각탭에 해당하는 아이템만 보여주기 위한 메서드입니다. 각 탭의 버튼 클릭 이벤트에 등록해야 합니다.
    /// </summary>
    /// <param name="btnIdx"></param>
    public void BtnTapClick( int btnIdx )
    {
        

        if(btnIdx==0)       // All 탭
        {   
            if( slotListTr.parent.GetChild(4).childCount==0 ) //오브젝트 emptyList에 아무것도 담겨 있지 않다면, 실행하지 않는다.
                return;


            for(int i=0; i<weapList.Count; i++) //무기 리스트를 하나씩 배치
            {
                for(int j=0; j<slotListTr.childCount; j++) // 슬롯을 차례로 순회
                {
                    if(slotListTr.GetChild(j).childCount==0)
                    {
                        weapList[i].transform.SetParent( slotListTr.GetChild(j), false );     // 빈 공간에 배치
                        weapList[i].transform.localPosition = Vector3.zero;
                        break;
                    }                
                } 
            }
            for(int i=0; i<miscList.Count; i++) //기타 리스트를 하나씩 배치
            {
                for(int j=0; j<slotListTr.childCount; j++) // 슬롯을 차례로 순회
                {
                    if(slotListTr.GetChild(j).childCount==0)
                    {
                        miscList[i].transform.SetParent( slotListTr.GetChild(j), false );     // 빈 공간에 배치
                        miscList[i].transform.localPosition = Vector3.zero;
                        break;
                    }                
                } 
            } 
              
        }
        else if(btnIdx==1)  // Weapon 탭
        {
            for(int i=0; i<slotListTr.childCount; i++) // 슬롯의 갯수만큼
            {
                if( slotListTr.GetChild(i).childCount!=0 ) // 슬롯에 들어있는 오브젝트를 모두 잠시 오브젝트 emptyList로 이동한다.
                {
                    slotListTr.GetChild(i).GetChild(0).localPosition = Vector3.zero;
                    slotListTr.GetChild(i).GetChild(0).SetParent(slotListTr.parent.GetChild(4), false);
                }
            }

            for( int i = 0; i<weapList.Count; i++ ) // 무기 리스트의 모든 아이템 정보를 읽어들인다.
            {
                int targetIdx = weapList[i].GetComponent<ItemInfo>().item.SlotIndex;
                weapList[i].transform.SetParent( slotListTr.GetChild(targetIdx), false );     // 해당 정보에 맞게 배치한다.
                weapList[i].transform.localPosition = Vector3.zero;                                                                                              
            }
        }
        else if(btnIdx==2)  // Misc 탭
        {
            for(int i=0; i<slotListTr.childCount; i++) // 슬롯의 갯수 만큼
            {
                if( slotListTr.GetChild(i).childCount!=0 ) // 슬롯에 들어있는 오브젝트를 모두 잠시 오브젝트 emptyList로 이동한다.
                {
                    slotListTr.GetChild(i).GetChild(0).localPosition = Vector3.zero;
                    slotListTr.GetChild(i).GetChild(0).SetParent(slotListTr.parent.GetChild(4), false);
                }
            }

            for( int i = 0; i<miscList.Count; i++ ) // 기타 리스트의 모든 아이템 정보를 읽어들인다. 해당 정보에 맞게 배치한다.
            {
                int targetIdx = miscList[i].GetComponent<ItemInfo>().item.SlotIndex;
                miscList[i].transform.SetParent( slotListTr.GetChild(targetIdx), false );     
                miscList[i].transform.localPosition = Vector3.zero;                                                                                              
            }
        }

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
            { "철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000000", "철", 3.0f, iicMiscBase.icArrImg[0] ) },
            { "강철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000001", "강철", 5.0f, iicMiscBase.icArrImg[1] ) },
            { "흑철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000002", "흑철", 7.0f, iicMiscBase.icArrImg[2] ) },
            { "미스릴", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000003", "미스릴", 20.0f, iicMiscBase.icArrImg[3] ) }, 
            { "코발트", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000004", "코발트", 16.0f, iicMiscBase.icArrImg[4] ) },
            { "티타늄", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000005", "티타늄", 25.0f, iicMiscBase.icArrImg[5] ) },
            { "오리하르콘", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000006", "오리하르콘", 60.0f, iicMiscBase.icArrImg[6] ) },
            { "단단한 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000007", "단단한 나뭇가지", 2.0f, iicMiscBase.icArrImg[7] ) },
            { "튼튼한 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000008", "튼튼한 나뭇가지", 3.0f, iicMiscBase.icArrImg[8] ) },
            { "가벼운 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000009", "가벼운 나뭇가지", 4.0f, iicMiscBase.icArrImg[9] ) },
            { "부드러운 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000010", "부드러운 나뭇가지", 5.0f, iicMiscBase.icArrImg[10] ) },
            { "엘프의 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000011", "엘프의 나뭇가지", 20.0f, iicMiscBase.icArrImg[11] ) },
            { "축복받은 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000012", "축복받은 나뭇가지", 40.0f, iicMiscBase.icArrImg[12] ) },

            { "하늘의 룬", new ItemMisc( ItemType.Misc, MiscType.Additive, "0000300", "하늘의 룬", 100.0f, iicMiscAdd.icArrImg[0] ) },
            { "불타는 심장", new ItemMisc( ItemType.Misc, MiscType.Additive, "0000301", "불타는 심장", 100.0f, iicMiscAdd.icArrImg[1] ) },
            { "영롱한 구슬", new ItemMisc( ItemType.Misc, MiscType.Additive, "0000302", "영롱한 구슬", 100.0f, iicMiscAdd.icArrImg[2] ) },
            { "요정의 목화", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000303", "요정의 목화", 100.0f, iicMiscAdd.icArrImg[3] ) }, 
            { "달의 조각", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000304", "달의 조각", 30.0f, iicMiscAdd.icArrImg[4] ) },
            { "신비한 조각", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000305", "신비한 조각", 30.0f, iicMiscAdd.icArrImg[5] ) },
            { "크롬 결정", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000306", "크롬 결정", 25.0f, iicMiscAdd.icArrImg[6] ) },
            { "수은 결정", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000307", "수은 결정", 20.0f, iicMiscAdd.icArrImg[7] ) },
            { "얼음 결정", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000308", "얼음 결정", 15.0f, iicMiscAdd.icArrImg[8] ) },
            { "백금", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000309", "백금", 60.0f, iicMiscAdd.icArrImg[9] ) },
            { "흑요석", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000310", "흑요석", 40.0f, iicMiscAdd.icArrImg[10] ) },
            { "금", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000311", "금", 30.0f, iicMiscAdd.icArrImg[11] ) },
            { "은", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000312", "은", 20.0f, iicMiscAdd.icArrImg[12] ) },
            { "점토", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000313", "점토", 2.0f, iicMiscAdd.icArrImg[13] ) },
            { "솜뭉치", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000314", "솜뭉치", 1.0f, iicMiscAdd.icArrImg[14] ) },
            { "물소의 뿔", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000315", "물소의 뿔", 15.0f, iicMiscAdd.icArrImg[15] ) },
            { "짐승 가죽", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000316", "짐승 가죽", 6.0f, iicMiscAdd.icArrImg[16] ) },
            
            { "초급 물리의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000700", "초급 물리의 각인", 50.0f, iicMiscOther.icArrImg[0] ) },
            { "초급 공속의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000701", "초급 공속의 각인", 50.0f, iicMiscOther.icArrImg[1] ) },
            { "초급 흡혈의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000702", "초급 흡혈의 각인", 50.0f, iicMiscOther.icArrImg[2] ) },
            { "초급 사격의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000703", "초급 사격의 각인", 50.0f, iicMiscOther.icArrImg[3] ) },
            { "초급 피해의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000704", "초급 피해의 각인", 50.0f, iicMiscOther.icArrImg[4] ) },
            { "중급 물리의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000705", "중급 물리의 각인", 125.0f, iicMiscOther.icArrImg[0] ) },
            { "중급 공속의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000706", "중급 공속의 각인", 125.0f, iicMiscOther.icArrImg[1] ) },
            { "중급 흡혈의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000707", "중급 흡혈의 각인", 125.0f, iicMiscOther.icArrImg[2] ) },
            { "중급 사격의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000708", "중급 사격의 각인", 125.0f, iicMiscOther.icArrImg[3] ) },
            { "중급 피해의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000709", "중급 피해의 각인", 125.0f, iicMiscOther.icArrImg[4] ) },
            { "고급 물리의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000710", "고급 물리의 각인", 200.0f, iicMiscOther.icArrImg[0] ) },
            { "고급 공속의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000711", "고급 공속의 각인", 200.0f, iicMiscOther.icArrImg[1] ) },
            { "고급 흡혈의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000712", "고급 흡혈의 각인", 200.0f, iicMiscOther.icArrImg[2] ) },
            { "고급 사격의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000713", "고급 사격의 각인", 200.0f, iicMiscOther.icArrImg[3] ) },
            { "고급 피해의 각인", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000714", "고급 피해의 각인", 200.0f, iicMiscOther.icArrImg[4] ) },
            
            { "나무", new ItemMisc( ItemType.Misc, MiscType.Fire, "0000600", "나무", 1.0f, iicMiscOther.icArrImg[5] ) },
            { "석탄", new ItemMisc( ItemType.Misc, MiscType.Fire, "0000601", "석탄", 6.0f, iicMiscOther.icArrImg[6] ) },
            { "석유", new ItemMisc( ItemType.Misc, MiscType.Fire, "0000602", "석유", 15.0f, iicMiscOther.icArrImg[7] ) },
            { "속성석", new ItemMisc( ItemType.Misc, MiscType.Attribute,"0000610", "속성석", 500.0f, iicMiscOther.icArrImg[8] ) }, 
        };

        weaponDic=new Dictionary<string, Item>()
        {
            /*** 검 ***/
            { "철 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001000", "철 검", 10.0f, iicWeaponSword.icArrImg[0]
                , ItemGrade.Low, 10, 100, 1.0f, 10, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("철",2)} 
                , new CraftMaterial[]{new CraftMaterial("점토",1)} 
                , Recipie.Eu) 
            },
            { "강철 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001001", "강철 검", 20.0f, iicWeaponSword.icArrImg[1]
                , ItemGrade.Low, 12, 100, 1.0f, 18, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("강철",2), new CraftMaterial("철",1)}
                , new CraftMaterial[]{new CraftMaterial("점토",2)}
                , Recipie.Na)
            },
            { "미스릴 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001002", "미스릴 검", 40.0f, iicWeaponSword.icArrImg[2] 
                , ItemGrade.Low, 18, 100, 1.0f, 10, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",2), new CraftMaterial("강철",1),new CraftMaterial("철",1)}
                , new CraftMaterial[]{new CraftMaterial("은",2)}
                , Recipie.Ma)
            
            },
            { "흑철 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001003", "흑철 검", 60.0f, iicWeaponSword.icArrImg[3] 
                , ItemGrade.Low, 23, 100, 1.0f, 40, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("흑철",2), new CraftMaterial("강철",3)}
                , new CraftMaterial[]{new CraftMaterial("점토",3)}
                , Recipie.Ga)
            },
            { "프레첼", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001004", "프레첼", 80.0f, iicWeaponSword.icArrImg[4] 
                , ItemGrade.Low, 25, 100, 1.0f, 20, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",2), new CraftMaterial("강철",2),new CraftMaterial("흑철",1)}
                , new CraftMaterial[]{new CraftMaterial("점토",2),new CraftMaterial("흑요석",1)}
                , Recipie.Da)
            },


            { "얼음칼날", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001100", "얼음칼날", 120.0f, iicWeaponSword.icArrImg[5]
                , ItemGrade.Medium, 36, 150, 1.0f, 25, EnumAttribute.Water
                , new CraftMaterial[]{new CraftMaterial("미스릴",7)}
                , new CraftMaterial[]{new CraftMaterial("얼음 결정",5)}
                , Recipie.Da)
            },
            { "백은의 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001101", "백은의 검", 180.0f, iicWeaponSword.icArrImg[6]
                , ItemGrade.Medium, 38, 150, 1.0f, 30, EnumAttribute.Wind
                , new CraftMaterial[]{new CraftMaterial("티타늄",2),new CraftMaterial("미스릴",4)}
                , new CraftMaterial[]{new CraftMaterial("백금",5)}
                , Recipie.Ga)
            },
            { "기사단의 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001102", "기사단의 검", 150.0f, iicWeaponSword.icArrImg[7] 
                , ItemGrade.Medium, 48, 150, 1.0f, 30, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("티타늄",1),new CraftMaterial("미스릴",3),new CraftMaterial("강철",2)}
                , new CraftMaterial[]{new CraftMaterial("금",3),new CraftMaterial("은",2)}
                , Recipie.Na)
            },
            { "아밍 소드", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001103", "아밍 소드", 110.0f, iicWeaponSword.icArrImg[8] 
                , ItemGrade.Medium, 32, 150, 1.0f, 12, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("티타늄",1),new CraftMaterial("미스릴",3)}
                , new CraftMaterial[]{new CraftMaterial("금",2),new CraftMaterial("흑요석",2)}
                , Recipie.Ma)
            },
            { "하플랑", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001104", "하플랑", 100.0f, iicWeaponSword.icArrImg[9] 
                , ItemGrade.Medium, 38, 150, 1.0f, 20, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",4),new CraftMaterial("강철",2)}
                , new CraftMaterial[]{new CraftMaterial("은",3),new CraftMaterial("크롬 결정",1)}
                , Recipie.Ee)
            },

            { "천공의 지배", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001200", "천공의 지배", 680.0f, iicWeaponSword.icArrImg[10]
                , ItemGrade.High, 80, 200, 1.10f, 10, EnumAttribute.Wind
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",3),new CraftMaterial("티타늄",5),new CraftMaterial("미스릴",10)}
                , new CraftMaterial[]{new CraftMaterial("하늘의 룬",2), new CraftMaterial("수은 결정",5)}
                , Recipie.Da)
            },
            { "파멸의 불꽃", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001201", "파멸의 불꽃", 640.0f, iicWeaponSword.icArrImg[11]
                , ItemGrade.High, 105, 200, 1.10f, 30, EnumAttribute.Fire
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",5),new CraftMaterial("티타늄",10),new CraftMaterial("미스릴",3)}
                , new CraftMaterial[]{new CraftMaterial("불타는 심장",1), new CraftMaterial("신비한 조각",5)}
                , Recipie.Ra)
            },
            { "듀란달", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001202", "듀란달", 480.0f, iicWeaponSword.icArrImg[12] 
                , ItemGrade.High, 120, 200, 1.10f, 50, EnumAttribute.Wind
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",3),new CraftMaterial("티타늄",5),new CraftMaterial("흑철",8)}
                , new CraftMaterial[]{new CraftMaterial("금",2),new CraftMaterial("달의 조각",5)}
                , Recipie.Ma)
            },
            { "미스틸테인", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001203", "미스틸테인", 700.0f, iicWeaponSword.icArrImg[13] 
                , ItemGrade.High, 95, 200, 1.10f, 25, EnumAttribute.Gold
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",5),new CraftMaterial("미스릴",5),new CraftMaterial("코발트",12)}
                , new CraftMaterial[]{new CraftMaterial("신비한 조각",5),new CraftMaterial("영롱한 구슬",2)}
                , Recipie.Ra)
            },
            { "크시포스", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001204", "크시포스", 350.0f, iicWeaponSword.icArrImg[14] 
                , ItemGrade.High, 80, 200, 1.10f, 30, EnumAttribute.Earth
                , new CraftMaterial[]{new CraftMaterial("미스릴",13),new CraftMaterial("강철",6)}
                , new CraftMaterial[]{new CraftMaterial("은",2),new CraftMaterial("백금",2), new CraftMaterial("크롬 결정",3)}
                , Recipie.Ga)
            },

            /**** 활 ****/
            { "사냥꾼의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008000", "사냥꾼의 활", 10.0f, iicWeaponBow.icArrImg[0]
                , ItemGrade.Low, 10, 100, 0.8f, 10, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("철",3), new CraftMaterial("단단한 나뭇가지",3)} 
                , null 
                , Recipie.Eu) 
            },
            { "롱 보우", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008001", "롱 보우", 20.0f, iicWeaponBow.icArrImg[1]
                , ItemGrade.Low, 12, 100, 0.8f, 18, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("철",5), new CraftMaterial("부드러운 나뭇가지",2)}
                , new CraftMaterial[]{new CraftMaterial("물소의 뿔",2)}
                , Recipie.Na)
            },
            { "컴뱃 보우", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008002", "컴뱃 보우", 40.0f, iicWeaponBow.icArrImg[2] 
                , ItemGrade.Low, 18, 100, 0.8f, 10, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("강철",5), new CraftMaterial("단단한 나뭇가지",2)}
                , null
                , Recipie.Ma)
            
            },
            { "양치기의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008003", "양치기의 활", 60.0f, iicWeaponBow.icArrImg[3] 
                , ItemGrade.Low, 23, 100, 0.8f, 40, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("철",3), new CraftMaterial("가벼운 나뭇가지",2)}
                , new CraftMaterial[]{new CraftMaterial("솜뭉치",4)}
                , Recipie.Ga)
            },
            { "도망자의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008004", "도망자의 활", 80.0f, iicWeaponBow.icArrImg[4] 
                , ItemGrade.Low, 25, 100, 0.8f, 20, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("철",4), new CraftMaterial("가벼운 나뭇가지",3)}
                , new CraftMaterial[]{new CraftMaterial("짐승가죽",2)}
                , Recipie.Da)
            },


            { "샤이닝 보우", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008100", "샤이닝 보우", 120.0f, iicWeaponBow.icArrImg[5]
                , ItemGrade.Medium, 36, 150, 0.8f, 25, EnumAttribute.Water
                , new CraftMaterial[]{new CraftMaterial("티타늄",5),new CraftMaterial("단단한 나뭇가지",5)}
                , new CraftMaterial[]{new CraftMaterial("백금",3)}
                , Recipie.Da)
            },
            { "이글 보우", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008101", "이글 보우", 180.0f, iicWeaponBow.icArrImg[6]
                , ItemGrade.Medium, 38, 150, 0.8f, 30, EnumAttribute.Wind
                , new CraftMaterial[]{new CraftMaterial("코발트",6),new CraftMaterial("튼튼한 나뭇가지",2)}
                , new CraftMaterial[]{new CraftMaterial("수은 결정",4)}
                , Recipie.Ra)
            },
            { "탄궁", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008102", "탄궁", 150.0f, iicWeaponBow.icArrImg[7] 
                , ItemGrade.Medium, 48, 150, 0.8f, 30, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",5),new CraftMaterial("부드러운 나뭇가지",4)}
                , new CraftMaterial[]{new CraftMaterial("물소의 뿔",3)}
                , Recipie.Ma)
            },
            { "각궁", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008103", "각궁", 110.0f, iicWeaponBow.icArrImg[8] 
                , ItemGrade.Medium, 32, 150, 0.8f, 12, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",4),new CraftMaterial("가벼운 나뭇가지",3)}
                , new CraftMaterial[]{new CraftMaterial("크롬 결정",5)}
                , Recipie.Ee)
            },
            { "추격자의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008104", "추격자의 활", 100.0f, iicWeaponBow.icArrImg[9] 
                , ItemGrade.Medium, 38, 150, 0.8f, 20, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("흑철",3),new CraftMaterial("가벼운 나뭇가지",5)}
                , new CraftMaterial[]{new CraftMaterial("짐승 가죽",4)}
                , Recipie.Ga)
            },


            { "취풍 파르티아", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008200", "취풍 파르티아", 450.0f, iicWeaponBow.icArrImg[10]
                , ItemGrade.High, 80, 200, 0.88f, 16, EnumAttribute.Wind
                , new CraftMaterial[]{new CraftMaterial("티타늄",7),new CraftMaterial("미스릴",13),new CraftMaterial("엘프의 나뭇가지",3)}
                , new CraftMaterial[]{new CraftMaterial("하늘의 룬",4), new CraftMaterial("크롬 결정",3)}
                , Recipie.Da)
            },
            { "피닉스의 속삭임", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008201", "피닉스의 속삭임", 640.0f, iicWeaponBow.icArrImg[11]
                , ItemGrade.High, 105, 200, 0.88f, 28, EnumAttribute.Fire
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",3),new CraftMaterial("티타늄",14),new CraftMaterial("축복받은 나뭇가지",4)}
                , new CraftMaterial[]{new CraftMaterial("불타는 심장",1), new CraftMaterial("영롱한 구슬",3)}
                , Recipie.Ga)
            },
            { "태양의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008202", "태양의 활", 530.0f, iicWeaponBow.icArrImg[12] 
                , ItemGrade.High, 120, 200, 0.88f, 50, EnumAttribute.Fire
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",7),new CraftMaterial("흑철",18),new CraftMaterial("엘프의 나뭇가지",5)}
                , new CraftMaterial[]{new CraftMaterial("불타는 심장",3),new CraftMaterial("하늘의 룬",2)}
                , Recipie.Ra)
            },
            { "장미의 신궁", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008203", "장미의 신궁", 480.0f, iicWeaponBow.icArrImg[13] 
                , ItemGrade.High, 95, 200, 0.88f, 32, EnumAttribute.Earth
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",5),new CraftMaterial("코발트",14),new CraftMaterial("축복받은 나뭇가지",3)}
                , new CraftMaterial[]{new CraftMaterial("요정의 목화",7)}
                , Recipie.Na)
            },
            { "파마의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008204", "파마의 활", 420.0f, iicWeaponBow.icArrImg[14] 
                , ItemGrade.High, 80, 200, 0.88f, 36, EnumAttribute.Gold
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",5),new CraftMaterial("미스릴",11),new CraftMaterial("축복받은 나뭇가지",4)}
                , new CraftMaterial[]{new CraftMaterial("신비한 조각",4),new CraftMaterial("영롱한 구슬",1)}
                , Recipie.Ma)
            }

        };


        //playerCraftableList = DataManager.Load().playerCraftableList;
        
        if( playerCraftableList!=null )
            return;
            
        playerCraftableList = new List<string>();


    }





}
