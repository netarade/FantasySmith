using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemData;
using CraftData;

/*
* [작업 사항]  
* <v1.0 - 2023_1106_최원준>
* 1- 초기 클래스 정의
* 
* 2- 인벤토리 인터엑티브 로직을 담당
* 각 탭을 클릭했을 때의 메서드가 정의되어 있음.
* 
* 3- 현재 전체 탭 클릭에 대한 로직이 불완전하게 구현되어있음.
* 
* <v1.1 - 2023_1106_최원준>
* 1-ItemPointerStatusWindow에서 상태창 참조가 동적할당으로는 제대로 잡히지 않아 
* 게임 시작 시 참조하는 변수를 미리 잡아놓도록 static 참조 변수를 선언.
* 
* <v1.2 - 2023_1108_최원준>
* 1- 인벤토리 i키를 통해 닫는 기능 구현
* 2- 스크립트 부착위치를 Inventory 판넬에서 GameController 오브젝트로 옮김
* 
* <v1.3 - 2023_1109_최원준>
* 1- InventroyOnOffSwitch 철자 수정
* 
* <v2.0 - 2023_1120_최원준> 
* 1- InventoryManagement의 역할 설정
* 
* a. 게임 시작 시 인벤토리의 슬롯(오브젝트)을 생성하고, 슬롯 리스트(오브젝트) 주소를 관리하는 역할. 
* (개념 인벤토리의 게임 상 오브젝트를 관리하는 역할). 슬롯 생성 메서드를 보유 (인벤토리의 각 탭 별 제한 만큼 생성해줘야 한다.)
* 
* b. 플레이어의 개념 인벤토리를 참조하는 역할.(기존의 PlayerInven 스크립트의 역할을 통합하고 CraftManager클래스는 제거) 
* 게임 시작 시 플레이어 인벤토리를 세이브 로드하여 참조하고 있다. 씬 전환 시에도 참조 유지. 
* 
* c. 인벤토리 및 상태창의 숨김 드러남을 관리. OnOff 메서드를 보유.
* 
* d. 탭버튼 클릭시의 처리에 관한 메서드 보유
* 
* <v2.1 - 2023_1121_최원준>
* 1- BtnItemCreateToInventory(아이템 생성 테스트 버튼) 메서드 삭제
* 클래스와 연관성 없는 기능이므로 
* 
* 2- list, list 초기화 구문 삭제 
* 인벤토리 클래스 구성이 바뀌었으므로
* 
* <v2.2 - 2023_1122_최원준>
* 1- 각 슬롯리스트 선언 및 참조, 슬롯리스트에 슬롯 프리팹 동적할당 진행 중  
* 
* <v3.0 - 2023_1215_최원준>
* 1- 인벤토리가 존재하는 캔버스 수정으로 하위 인벤토리 및 상태창 위치, 버튼들의 계층구조 변경 및 참조 변경 (태그로 캔버스 참조)
* 2- 탭버튼 기능 수정 진행 중
* 
* <v3.1 - 2023_1216_최원준>
* 1- 슬롯리스트가 전체탭, 개별탭 별로 있던 것을 전체탭만 존재하도록 축소하였음. (SlotListAllTr에서 SlotListTr로 변경)
* 2- 개별 탭 클릭 시 빈리스트로 옮기고, 개별 탭의 갯수만큼만 슬롯이 보이도록 하며, 해당 탭의 인덱스를 참조하여 위치를 옮기도록 기능 수정 완료
* 3- 현재 활성화 중인 탭을 enum을 활용하여 기록하고 탭 버튼 클릭 시 중복 실행되지 않도록 수정
* 
* <v4.0 - 2023-1217_최원준>
* 1- statusWindow를 삭제- 이유는 상태창이 인벤토리와 독립적으로 작동 가능하기 때문에 
* 상태창 로직을 가지고 있는 ItemPointerSatusWindow.cs 스크립트에 있는 것이 낫다고 판단
* 
* 2- 인벤토리 오브젝트에 캔버스 그룹 컴포넌트를 등록 후 InventoryOnOffSwitch 메서드에서 이를 조절하는 방식으로 변경
* (내부의 인벤토리 이미지와 텍스트를 매번 직접 찾아서 참조하는 방식에서 변경)
* 
*/

/// <summary>
/// 인벤토리 인터엑티브 로직을 담당합니다. 컴포넌트로 붙여야 하며, 별다른 인스턴스 선언 접근이 필요하지 않습니다.<br/>
/// 인벤토리 닫기, 각 종류 탭 버튼 클릭 시 동작이 정의되어 있습니다<br/>
/// 이 스크립트의 외부에서 다양한 참조를 할 수 있습니다.<br/>
/// </summary>
public class InventoryManagement : MonoBehaviour
{
    [SerializeField] Button[] btnTap;           // 버튼 탭을 눌렀을 때 각 탭에 맞는 아이템이 표시되도록 하기 위한 참조

    public Transform InventoryPanelTr;          // 인벤토리 판넬을 참조하여 껐다 켰다 하기 위함.
   
    [SerializeField] GameObject slotPrefab;     // 슬롯을 동적으로 생성하기 위한 프리팹 참조
    public Transform slotListTr;                // 인벤토리 전체탭 슬롯리스트의 트랜스폼 참조

    public static bool isInventoryOn;           // On상태 기록하기 위한 변수
    CanvasGroup inventoryCvGroup;               // 인벤토리의 캔버스 그룹

    Inventory inventory;                        // 플레이어의 인벤토리 정보
    public Transform emptyListTr;               // 아이템을 보이지 않는 곳에 잠시 옮겨둘 오브젝트(뷰포트의 1번째 자식)

    enum eTapType { All, Weap, Misc }           // 어떤 종류의 탭인지를 보여주는 열거형
    eTapType activeTab;                         // 현재 활성화 중인 탭을 저장 (버튼 클릭 시 중복 실행을 방지하기 위해)



    void Start()
    {
        Transform canvas = GameObject.FindWithTag("CANVAS_CHARACTER").transform;    // 캐릭터 캔버스 참조
        InventoryPanelTr = canvas.GetChild(0);                                      // 인벤토리 판넬 참조
        slotListTr = InventoryPanelTr.GetChild(0).GetChild(0).GetChild(0);          // 뷰포트-컨텐트-전체 슬롯리스트
        emptyListTr = InventoryPanelTr.GetChild(0).GetChild(1);                     // 뷰포트-EmptyList

        
        // 탭 버튼 컴포넌트 참조 후 버튼의 이벤트를 등록합니다.(int값 인자를 다르게 줘서 등록합니다)
        for( int i = 0; i<3; i++ )
        {          
            btnTap[i]=InventoryPanelTr.GetChild( 1 ).GetChild( i ).GetComponent<Button>(); //인벤토리-Buttons하위에 존재
            btnTap[i].onClick.AddListener( () => BtnTapClick( i ) );
        }

        btnTap[0].Select();         // 첫 시작 시 항상 전체탭을 Select로 표시해줍니다.
        activeTab = eTapType.All;   // 활성화중인 탭을 전체탭으로 설정
        
        // 인벤토리의 모든 이미지와 텍스트 참조, 캔버스그룹 참조
        inventoryCvGroup = InventoryPanelTr.GetComponent<CanvasGroup>();

        // 게임 시작 시 인벤토리 판넬을 꺼둔다.
        InventoryOnOffSwitch(false);
        isInventoryOn = false;          //초기에 인벤토리는 꺼진 상태


        //플레이어 오브젝트를 참조하여 인벤토리 정보를 받아옵니다
        PlayerInven invenInfo = GameObject.FindWithTag("Player").GetComponent<PlayerInven>();
        inventory = invenInfo.GetComponent<Inventory>();


        // 플레이어 인벤토리 정보(전체 탭 슬롯 칸수)를 참조하여 슬롯을 동적으로 생성
        for(int i=0; i< invenInfo.inventory.AllCountLimit; i++)
            Instantiate(slotPrefab, slotListTr);
    }



    /// <summary>
    /// 인벤토리 아이콘 클릭 시 OnOff 스위치
    /// </summary>
    public void InventoryQuit()
    {
        InventoryOnOffSwitch( !isInventoryOn );     // 반대 상태로 넣어준다.
        isInventoryOn = !isInventoryOn;             // 상태 변화를 기록한다.
    }


    /// <summary>
    /// 인벤토리 키 맵핑 I키를 누르면 인벤토리를 종료합니다 (나중에 InputSystem방식으로 변경해야 합니다)
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            InventoryOnOffSwitch( !isInventoryOn );     // 반대 상태로 넣어준다.
            isInventoryOn = !isInventoryOn;             // 상태 변화를 기록한다.
        }        
    }


    /// <summary>
    /// 인벤토리의 모든 이미지와 텍스트를 꺼줍니다.
    /// </summary>
    public void InventoryOnOffSwitch(bool onOffState )
    {

        inventoryCvGroup.blocksRaycasts = onOffState;   // 그룹의 블록 레이캐스트를 조절해줍니다
        inventoryCvGroup.alpha = onOffState ? 1f : 0f;  // 그룹의 투명도를 조절해줍니다
        



        //Image[] inventoryImgArr;    // 인벤토리의 모든 이미지
        //Text[] inventoryTextArr;    // 인벤토리의 모든 텍스트        
        //inventoryImgArr = InventoryPanelTr.GetComponentsInChildren<Image>();  
        //inventoryTextArr = InventoryPanelTr.GetComponentsInChildren<Text>();

        //foreach(Image image in inventoryImgArr)
        //    image.enabled = onOffState;
        //foreach(Text text in inventoryTextArr)
        //    text.enabled = onOffState;

        //// 인벤토리의 이미지를 키고 끌때마다 현재 슬롯에 있는 아이템의 이미지와 텍스트도 동일한 상태로 동기화 해줍니다.        
        //ItemInfo[] itemInfos = slotListTr.GetComponentsInChildren<ItemInfo>();
        //foreach(ItemInfo info in itemInfos)
        //{
        //    info.gameObject.GetComponent<Image>().enabled = onOffState;

        //    if(info.Item.Type==ItemType.Misc)   //잡화 아이템의 경우 중첩 텍스트가 있다.
        //    info.gameObject.GetComponentInChildren<Text>().enabled = onOffState;
        //}

        
    }



    /// <summary>
    /// 버튼 클릭 시 각탭에 해당하는 아이템만 보여주기 위한 메서드입니다. 각 탭의 버튼 클릭 이벤트에 등록해야 합니다.
    /// </summary>
    /// <param name="btnIdx"></param>
    public void BtnTapClick( int btnIdx )
    {
        
        if(btnIdx==0)       // All 탭
        {   
            if(activeTab==eTapType.All)                         // 현재 활성화 중인 탭이 전체 탭이라면 재실행하지 않는다.
                return;
            
            activeTab = eTapType.All;                           // 활성화 중인 탭 변경 
            MoveSlotItemToEmptyList();                          // 빈리스트로 모든 아이템 이동
            AdjustSlotListNum(eTapType.All);                    // 전체 탭의 갯수만큼 슬롯을 늘리거나 줄임
            LocateDicItemToSlotList(inventory.weapDic, true);   // 전체탭 인덱스를 기준으로 모든 아이템 배치
            LocateDicItemToSlotList(inventory.miscDic, true);   
        }
        else if(btnIdx==1)  // 무기 탭버튼 클릭
        {
            if(activeTab==eTapType.Weap)                        // 현재 활성화 중인 탭이 무기 탭이라면 재실행하지 않는다.
                return;
            
            activeTab = eTapType.Weap;                          // 활성화 중인 탭 변경
            MoveSlotItemToEmptyList();                          // 빈리스트로 모든 아이템 이동
            AdjustSlotListNum(eTapType.Weap);                   // 무기 탭의 갯수만큼 슬롯을 늘리거나 줄임
            LocateDicItemToSlotList(inventory.weapDic, false);  // 개별 탭 인덱스를 기준으로 모든 아이템 배치
        }
        else if(btnIdx==2)  // 잡화 탭버튼 클릭
        {
            if(activeTab==eTapType.Misc)                         // 현재 활성화 중인 탭이 잡화 탭이라면 재실행하지 않는다.
                return;
            
            activeTab = eTapType.Misc;                          // 활성화 중인 탭 변경
            MoveSlotItemToEmptyList();                          // 빈리스트로 모든 아이템 이동
            AdjustSlotListNum(eTapType.Misc);                   // 잡화 탭의 갯수만큼 슬롯을 늘리거나 줄임
            LocateDicItemToSlotList(inventory.miscDic, false);  // 개별탭 인덱스를 기준으로 모든 아이템 배치
        }

    }


    /// <summary>
    /// 각 탭을 클릭했을 때 해당 탭의 칸 수만큼 슬롯의 갯수를 줄이거나 늘려줍니다
    /// </summary>
    private void AdjustSlotListNum( eTapType tapType )
    {
        switch(tapType)
        {
            case eTapType.All:
                for(int i=0; i<inventory.AllCountLimit; i++)
                    slotListTr.GetChild(i).gameObject.SetActive(true);   // 전체 탭의 갯수만큼 켜줍니다
                break;

            case eTapType.Weap:
                for(int i=0; i<inventory.WeapCountLimit; i++)                   
                    slotListTr.GetChild(i).gameObject.SetActive(true);   // 무기 탭의 갯수만큼 켜줍니다

                for(int i=inventory.WeapCountLimit; i<inventory.AllCountLimit; i++)
                    slotListTr.GetChild(i).gameObject.SetActive(false);  // 나머지는 꺼줍니다    
                break;

            case eTapType.Misc:
                for(int i=0; i<inventory.MiscCountLimit; i++)
                    slotListTr.GetChild(i).gameObject.SetActive(true);   // 잡화 탭의 갯수만큼 켜줍니다       
                
                for(int i=inventory.MiscCountLimit; i<inventory.AllCountLimit; i++)
                    slotListTr.GetChild(i).gameObject.SetActive(false);  // 나머지는 꺼줍니다
                break;
        }        
    }


    /// <summary>
    /// 현재 슬롯 리스트에 있는 오브젝트를 일시적으로 빈 리스트로 옮겨주는 메서드입니다
    /// </summary>
    private void MoveSlotItemToEmptyList()
    {
        for( int i = 0; i<inventory.AllCountLimit; i++ )        // 전체 슬롯리스트의 모든 슬롯 순회
        {
            if( slotListTr.GetChild(i).childCount!=0 )       // i번째 슬롯에 아이템이 들어있다면
            {
                // 슬롯에 들어있는 아이템 오브젝트를 모두 잠시 오브젝트 emptyList로 이동하고, 위치를 수정
                slotListTr.GetChild(i).GetChild(0).SetParent( emptyListTr, false );
                slotListTr.GetChild(i).GetChild(0).localPosition = Vector3.zero;
            }
        }
    }

    /// <summary>
    /// 딕셔너리에 담긴 아이템의 인덱스 정보를 바탕으로 슬롯리스트로 배치해주는 메서드입니다.
    /// 전달인자로 해당 아이템 종류의 딕셔너리와, 전체 인덱스 기준인지 개별 인덱스 기준인지를 전달하여야 합니다.
    /// </summary>
    private void LocateDicItemToSlotList(Dictionary<string, List<GameObject>> itemDic, bool isIndexAll)
    {        
        int targetIdx;  // 타겟 인덱스 설정

        foreach( List<GameObject> list in itemDic.Values )  // 딕셔너리에서 게임오브젝트 리스트를 하나씩 가져옴
        {
            for( int i = 0; i<list.Count; i++ )             // 리스트에 들어있는 중복 오브젝트의 수만큼 하나씩 i를 증가
            {                
                if(isIndexAll)                                                      // 전체탭 기준이라면
                    targetIdx = list[i].GetComponent<ItemInfo>().Item.SlotIndexAll; // 전체 인덱스 정보를 읽어들임
                else                                                                
                    targetIdx = list[i].GetComponent<ItemInfo>().Item.SlotIndex;    // 개별 탭 인덱스 정보를 읽어들임             
                                
                list[i].transform.SetParent( slotListTr.GetChild( targetIdx ), false ); // 해당 인덱스 정보에 맞게 배치한다.
                list[i].transform.localPosition = Vector3.zero;                            // 로컬위치를 칸에 맞게 수정
            }
        }
    }




}
