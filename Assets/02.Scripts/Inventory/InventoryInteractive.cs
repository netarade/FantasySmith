using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InventoryManagement;
using ItemData;
using System.Runtime.CompilerServices;
using System;
using Unity.VisualScripting;
using UnityEngine.Events;

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
* <4.1 -2023_1221_최원준>
* 1- 스크립트 부착위치 GameController오브젝트에서 Canvas-Chracter로 수정
* 
* <v4.2 - 2023_1221_최원준>
* 1- inventory의 잘못된 참조 수정
* PlayerInven변수인 invenInfo.GetComponent<Inventory>()를 참조하고 있었으나, invenInfo.inventory로 변경
* 
* <v4.3 - 2023_1226_최원준>
* 1- Start문을 Awake문으로 변경
* 이유는 인벤토리의 슬롯이 빨리 생성되어야 아이템 오브젝트를 슬롯에 담을 수 있기 때문
* (인벤토리를 로드하면서 UpdateAllItemInfo()메서드를 호출하기 위해)
* 
* 2- InventoryOnOffSwitch private메서드로 변경 및 내부의 주석삭제
* 
* <v5.0 - 2023_1229_최원준>
* 1- 클래스및 파일명 InventoryManagement에서 InventoryInteractive로 변경
* 2- 스크립트 부착위치 Canvas-Character에서 Inventory오브젝트로 변경에 따라 inventory참조 변경
* 3- 변수명 inventoryPanelTr을 inventoryTr로 변경
* 
* <v5.1 - 2023_1230_최원준>
* 1- 참조하던 슬롯 프리팹을 삭제. 인벤토리 내부에 슬롯을 하나 넣어두고 이 오브젝트를 복제하여 생성하도록 변경
* 
* 2- 인벤토리 슬롯을 수량만큼 동적 생성하는 코드를 InventoryInfo 클래스로 옮김.
* 현재 클래스에서는 인벤토리의 인터렉티브 기능만 담당하도록 하기 위하여
* 
* 3- activeTab -> curActiveTab 변수명 변경
* 
* <v5.2 - 2023_1231_최원준>
* 1- eTapType과 activeTab을 public 선언 후, activeTab 정보를 InventoryInfo클래스에서 알 수 있도록 변경
*
*
* <v5.3 - 2024_0101_최원준>
* 1- 내부변수 isActiveTabAll 변수를 두고 IsActiveTabAll 프로퍼티를 선언해서 전체탭 기준인지, 개별탭 기준인지 
* 아이템 쪽에서 Update Position할 인덱스를 알 수 있도록 하였음.
* 
* <v6.0 -2024_0102_최원준
* 1- curActiveTab private변수로 설정하여 변경을 막음.
* 2- InventoryOnOffSwitch 변수명 -> SwitchInventoryAppear로 변경
* 3- InventoryCvGroup 변수명 -> InventoryCG로 변경
* 4- isInventoryOn static키워드 삭제
* 5- eTapType 열거형 삭제 -> 탭의 종류를 ItemType으로 통일, 전체탭을 ItemType.None으로 설정
* inventory의 메서드 호출에 인자로 넣기 편하게 하기 위하여.
* 6- AdjustSlotListNum 메서드명 AdjustSlotCount로 변경
* 7- LocateDicItemToSlotList 메서드명 MoveActiveItemToSlotList로 변경
* 8- MoveActiveItemToSlot메서드 추가
* 9- 모든 메서드 활성화탭 기준으로 간소화, BtnTapClick 공통로직 최적화 작성
* 
* [수정예정]
* 1- LocateDicItemToSlotList의 필요성
* 아이템을 inventory에서 정보를 읽어오는데 그럴필요없이 현재 슬롯에 들어오는 아이템의 정보만 읽어들이면 됨.
* 
* 2- AdjustSlotListNum에서 슬롯 칸의 제한개수를 읽어오는데 참조와 연산에 걸리는 시간이 있으므로
* 초기에 저장해서 사용하고 변동이 있을 시 이벤트로 받을 수 있도록 해야 한다.
* 
* [수정예정_0102]
* 1- ActiveTab변동 정보를 슬롯에 존재하는(x)->인벤토리에 존재하는 모든 하위 아이템에게 전달하고 위치정보를 업데이트 시켜야함.
* (OnItemChanged메서드 호출)
* 
* 
* <v6.1 - 2024_0104_최원준>
* 1- UpdateAllActiveTabInfo 메서드 내부의 SetActiveTabInfo코드를 UpdaeteActiveTabInfo로 변경
* 호출인자를 따로 주는 것이 아니라 호출자를 현재 아이템이 속한 interactive로 고정시킴으로서 외부호출의 위험성을 줄임.
* 
* <v7.0 - 2024_0110_최원준>
* 1- 메서드 InventoryQuit를 InventoryOpenSwitch로 변경 후
* InventoryInfo 스크립트의 UpdateOpenState를 호출하여 오픈창의 상태를 반영하도록 수정
* ( 인벤토리 창 열고닫는 메서드로 InputSystem과 아이콘X클릭 양쪽에서 활용 예정)
* 
* 2- itemInfo의 UpdateActiveTabInfo를 안전성을 위해 호출자를 매개변수로 넣어서 호출하는 방식으로 변경.
* 
* 3- public 접근제한자 제거, enum eTabType 삭제
* 4- 슬롯의 동적 생성을 Info클래스에서 옮겨온 후 메서드로 정의하였고, 이 메서드를 Info클래스에서 호출하는 형식으로 변경
* 
* 5- (Interactive클래스의 역할 설정)
* 초기화하는데 InventroyInfo 클래스의 정보 의존성이 발생하며, 
* 즉시 사용하는 것이 아니라 특정 액션 이벤트가 발생 시 사용하면 되므로 InventoryInfo 클래스에 초기화를 맡기는 것으로 변경하였음.
* (Awake문에서 각자 초기화를 진행하면 호출시점이 명확하지 않기 때문에 
* CreateInventorySlot의 호출이 발생할 때 info클래스 관련 참조값이 잡혀있지 않는 문제가 발생하여 참조값을 한 번더 잡아줘야함.)
* 
* 6- Awkae문 삭제 및 코드를 Initialize메서드를 만들어 이동. Info클래스에서 호출방식으로 변경
* 
* 7- 탭이벤트 등록 시 람다식 내부에 i가 참조값으로 들어가기 때문에 모든 버튼이벤트가 마지막 참조값으로 호출되는 것을 발견
* (람다식 클로저 특성) 수정하기 위해 내부 for문에서 새롭게 int변수를 할당해서 i값을 받아서 넣는 형태로 구현
* 
* 
* 
*/

/// <summary>
/// 인벤토리 인터엑티브 로직을 담당합니다. 컴포넌트로 붙여야 하며, 별다른 인스턴스 선언 접근이 필요하지 않습니다.<br/>
/// 인벤토리 닫기, 각 종류 탭 버튼 클릭 시 동작이 정의되어 있습니다<br/>
/// 이 스크립트의 외부에서 다양한 참조를 할 수 있습니다.<br/>
/// 현재 Canvas-Character에 부착되어 있습니다.<br/>
/// </summary>
public class InventoryInteractive : MonoBehaviour
{   
    Transform inventoryTr;          // 인벤토리 판넬을 참조하여 껐다 켰다 하기 위함.   
    InventoryInfo inventoryInfo;    // 플레이어의 인벤토리 정보
    Inventory inventory;            // 인벤토리 스크립트 내부 인벤토리 데이터

    Transform slotListTr;           // 인벤토리 전체탭 슬롯리스트의 트랜스폼 참조
    Transform emptyListTr;          // 아이템을 보이지 않는 곳에 잠시 옮겨둘 오브젝트(뷰포트의 1번째 자식)

    Button[] btnTap;                // 버튼 탭을 눌렀을 때 각 탭에 맞는 아이템이 표시되도록 하기 위한 참조

    bool isInventoryOn;             // On상태 기록하기 위한 변수
    CanvasGroup inventoryCG;        // 인벤토리의 캔버스 그룹

    ItemType curActiveTab;          // 현재 활성화 중인 탭을 저장 (버튼 클릭 시 중복 실행을 방지하기 위해)
    bool isActiveTabAll;            // 현재 활성화 중인 탭이 전체 기준인지, 개별 기준인지 여부를 반환


    
    GameObject slotPrefab;          // 슬롯을 동적으로 생성하기 위한 프리팹 참조







    /// <summary>
    /// 아이템이 선택중인지 여부를 반환하거나 설정합니다.<br/>
    /// 아이템이 1개라도 선택중이라면 다른 아이템은 선택할 수 없습니다.<br/>
    /// ItemDrag에서 정보를 받고 수정합니다.
    /// </summary>
    public bool IsItemSelecting {get; set;} = false;

    /// <summary>
    /// 현재 활성화탭이 전체 기준인지, 개별 기준인지 여부를 반환합니다.
    /// </summary>
    public bool IsActiveTabAll { get{return isActiveTabAll; } }


    /// <summary>
    /// 현재 인벤토리 오브젝트가 켜져 있는지 여부를 반환합니다.
    /// </summary>
    public bool IsInventoryOn { get { return isInventoryOn;} } 



    /// <summary>
    /// 이 스크립트가 동작하기 위해서 InventoryInfo클래스에서 호출해줘야 할 메서드입니다.<br/>
    /// 해당 InventoryInfo에서 Load가 이루어진 이후에 내부 정보를 바탕으로 초기화를 진행합니다.<br/><br/>
    /// *** 호출 InventoryInfo와 InventoryInteractive의 계층이 다르다면 예외가 발생합니다. ***
    /// </summary>
    public void Initialize(InventoryInfo caller)
    {
        inventoryTr = transform;                                       // 인벤토리 판넬 참조  

        if(caller.transform != inventoryTr)
            throw new Exception("초기화를 진행할 수 없는 호출자입니다. 확인하여 주세요.");

        inventoryInfo = caller;                                        // Info 클래스 참조 등록
        inventory = inventoryInfo.inventory;                           // 내부 인벤토리 정보 참조 등록
        
        inventoryCG = inventoryTr.GetComponent<CanvasGroup>();         // 인벤토리의 캔버스그룹 참조
        slotListTr = inventoryTr.GetChild(0).GetChild(0).GetChild(0);  // 뷰포트-컨텐트-전체 슬롯리스트
        slotPrefab = slotListTr.GetChild(0).gameObject;                // 슬롯 리스트 하위에 미리 1개가 추가되어 있음
        emptyListTr = inventoryTr.GetChild(0).GetChild(1);             // 뷰포트-EmptyList
                                                                       
        CreateActiveTabBtn();   // 액티브탭 버튼 생성
        InitOpenState();        // 인벤토리 오픈 상태 초기화
    }


    /// <summary>
    /// 버튼 액티브 탭을 동적으로 생성하고 이벤트를 등록합니다.
    /// </summary>
    private void CreateActiveTabBtn()
    {        
        // 액티브탭 갯수 - 향후 inventory의 dicNum+1로 수정예정
        int activeTabNum = 3;

        // 버튼 배열을 참조할 갯수를 설정합니다.
        btnTap = new Button[activeTabNum]; 

        // 미리 생성되어있는 전체탭의 참조값을 가져 온 후 이벤트를 연결합니다.
        btnTap[0] = inventoryTr.GetChild(1).GetChild(0).GetComponent<Button>();
        btnTap[0].onClick.AddListener( () => BtnTapClick( 0 ) );
        

        // 나머지 탭을 전체탭을 바탕으로 동적할당하여 이벤트를 연결합니다.
        for(int i=1; i<activeTabNum; i++)
        {
            int idx = i;
            btnTap[i] = Instantiate(btnTap[0].gameObject, btnTap[0].transform.parent).GetComponent<Button>();
            btnTap[i].onClick.AddListener( () => BtnTapClick( idx ) );
        }
        
        // 버튼 이름과 텍스트를 설정합니다. (향후 인벤토리 클래스 구조 변경 후 수정예정) 
        btnTap[1].gameObject.name = "ActiveTab-Weap";
        btnTap[1].GetComponentInChildren<Text>().text = "무기";
        
        btnTap[2].gameObject.name = "ActiveTab-Misc";
        btnTap[2].GetComponentInChildren<Text>().text = "잡화";
                
        


        btnTap[0].Select();            // 첫 시작 시 항상 전체탭을 Select로 표시해줍니다.
        curActiveTab = ItemType.None;  // 활성화중인 탭을 전체탭으로 설정
        isActiveTabAll = true;         // 전체탭 기준 상태변수 설정
    }


    /// <summary>
    /// 인벤토리 창의 상태 초기화를 진행합니다.
    /// </summary>
    private void InitOpenState()
    {
        // 게임 시작 시 인벤토리 판넬을 꺼둔다.
        SwitchInventoryAppear(false);
        isInventoryOn = false;          //초기에 인벤토리는 꺼진 상태        
    }







    /// <summary>
    /// InventoryInfo에서 slotCoutLimit정보를 전달 받아서 슬롯을 생성하는 메서드입니다.<br/>
    /// slotCountLimit정보가 최신화된 상태에서 호출하여야 합니다.<br/>
    /// *** 동일 인벤토리의 InventoryInfo가 아니면 호출할 수 없습니다. ***
    /// </summary>
    public void CreateInventorySlot(InventoryInfo caller)
    {                          
        if( caller != inventoryInfo )
            throw new Exception("호출자를 확인하여 주세요. 동일 인벤토리에서만 호출 할 수 있습니다.");
                      

        // 플레이어 인벤토리 정보(전체 탭 슬롯 칸수)를 참조하여 슬롯을 동적으로 생성
        // 현재 인벤토리에 슬롯이 한 개 들어있으므로 하나를 감하고 생성
        for( int i = 0; i<caller.inventory.SlotCountLimitAll-1; i++ )
            Instantiate( slotPrefab, slotListTr );
    }










    /// <summary>
    /// 인벤토리 창을 열고닫는 메서드입니다.<br/>
    /// 플레이어 InputSystem에서의 I키를 누르거나 인벤토리 아이콘 x아이콘 클릭 시 호출됩니다.<br/>
    /// </summary>
    public void InventoryOpenSwitch()
    {
        SwitchInventoryAppear( !isInventoryOn );    // 호출 시 마다 반대 상태로 넣어줍니다
        isInventoryOn = !isInventoryOn;             // 상태 변화를 반대로 기록합니다

        inventoryInfo.UpdateOpenState(this, isInventoryOn);     // 상태를 Info클래스에 반영합니다.
    }



    /// <summary>
    /// 인벤토리의 모든 이미지와 텍스트를 꺼줍니다.
    /// </summary>
    private void SwitchInventoryAppear(bool onOffState )
    {
        inventoryCG.blocksRaycasts = onOffState;   // 그룹의 블록 레이캐스트를 조절해줍니다
        inventoryCG.alpha = onOffState ? 1f : 0f;  // 그룹의 투명도를 조절해줍니다
    }



    /// <summary>
    /// 버튼 클릭 시 각탭에 해당하는 아이템만 보여주기 위한 메서드입니다. 각 탭의 버튼 클릭 이벤트에 등록해야 합니다.
    /// </summary>
    /// <param name="btnIdx"></param>
    public void BtnTapClick( int btnIdx )
    {
        if(btnIdx==0)       // All 탭
        {
            if( curActiveTab==ItemType.None )     // 현재 활성화 중인 탭이 전체 탭이라면 재실행하지 않는다.
                return;
            
            curActiveTab = ItemType.None;       // 활성화 중인 탭 변경 
            isActiveTabAll = true;              // 활성화 기준 전체로 변경
        }
        else if(btnIdx==1)  // 무기 탭버튼 클릭
        {
            if(curActiveTab==ItemType.Weapon)   // 현재 활성화 중인 탭이 무기 탭이라면 재실행하지 않는다.
                return;

            curActiveTab = ItemType.Weapon;     // 활성화 중인 탭 변경
            isActiveTabAll = false;             // 활성화 기준 전체로 변경
        }
        else if(btnIdx==2)  // 잡화 탭버튼 클릭
        {
            if(curActiveTab==ItemType.Misc)     // 현재 활성화 중인 탭이 잡화 탭이라면 재실행하지 않는다.
                return;

            curActiveTab = ItemType.Misc;       // 활성화 중인 탭 변경
            isActiveTabAll = false;             // 활성화 기준 전체로 변경
        }

        UpdateAllItemActiveTabInfo();           // 인벤토리의 모든 아이템에 활성화 탭 정보를 반영합니다
        MoveSlotItemToEmptyList();              // 현재 슬롯에 있는 모든 아이템을 빈리스트로 이동합니다
        AdjustSlotCount();                      // 활성화 탭의 갯수만큼 슬롯을 늘리거나 줄입니다
        MoveActiveItemToSlot();                 // 활성화 탭 종류와 동일한 딕셔너리의 모든 아이템을 슬롯에 배치합니다       
    }



    /// <summary>
    /// 모든 아이템에 활성화탭 정보를 반영합니다.<br/>
    /// </summary>
    private void UpdateAllItemActiveTabInfo()
    {
        //// 하나씩 가져와서 참조 할 딕셔너리 변수 선언
        Dictionary<string, List<GameObject>> itemDic;

        for( int i = 0; i<inventory.CurDicLen; i++ )
        {
            itemDic=inventory.GetItemDicIgnoreExsists( (ItemType)i );      // CurDicLen 갯수만큼 딕셔너리를 하나씩 변경합니다.

            foreach( List<GameObject> itemObjList in itemDic.Values )      // 해당 딕셔너리의 오브젝트리스트를 가져옵니다.
            {
                foreach( GameObject itemObj in itemObjList )               // 오브젝트리스트에서 오브젝트를 하나씩 가져옵니다.
                {
                    ItemInfo itemInfo = itemObj.GetComponent<ItemInfo>();  // 아이템 정보를 읽어들입니다.

                    itemInfo.UpdateActiveTabInfo(this, isActiveTabAll);    // 활성화 탭 정보를 현재 활성화탭 기준으로 변경합니다.
                }

            }
        }
    }

    /// <summary>
    /// 각 탭을 클릭했을 때 해당 탭의 칸 수만큼 슬롯의 갯수를 줄이거나 늘려줍니다
    /// </summary>
    private void AdjustSlotCount()
    {
        int curSlotCount = inventory.GetItemSlotCountLimit(curActiveTab);
        int allSlotCount = inventory.GetItemSlotCountLimit(ItemType.None);
                
        for(int i=0; i<curSlotCount; i++)                   
            slotListTr.GetChild(i).gameObject.SetActive(true);   // 현재 탭의 갯수만큼 켜줍니다

        for(int i=curSlotCount; i<allSlotCount; i++)
            slotListTr.GetChild(i).gameObject.SetActive(false);  // 나머지는 꺼줍니다 
    }


    /// <summary>
    /// 현재 슬롯 리스트에 있는 오브젝트를 일시적으로 빈 리스트로 옮겨주는 메서드입니다
    /// </summary>
    private void MoveSlotItemToEmptyList()
    {
        for( int i = 0; i<inventory.SlotCountLimitAll; i++ )    // 전체 슬롯리스트의 모든 슬롯 순회
        {
            if( slotListTr.GetChild(i).childCount!=0 )          // i번째 슬롯에 아이템이 들어있다면
            {
                // 슬롯에 들어있는 아이템 오브젝트를 모두 잠시 오브젝트 emptyList로 이동하고, 위치를 수정합니다
                slotListTr.GetChild(i).GetChild(0).SetParent( emptyListTr, false );
            }
        }
    }


    /// <summary>
    /// 현재 활성화 중인 탭 종류와 동일한 딕셔너리의 모든 아이템을 슬롯에 배치합니다<br/>
    /// 내부적으로 InventoryInfo 클래스의 개별 딕셔너리에 존재하는 모든 아이템의 위치 정보를 업데이트하는 메서드를 활용합니다
    /// </summary>
    private void MoveActiveItemToSlot()
    {
        // 현재 활성화 탭이 전체 탭인 경우 모든 딕셔너리를 대상으로 호출
        if( isActiveTabAll )
        {
            print(inventory.CurDicLen);
            for( int i = 0; i<inventory.CurDicLen; i++ )
                inventoryInfo.UpdateDicItemPosition( (ItemType)i );
        }
        // 현재 활성화 탭이 개별 탭인 경우 개별 딕셔너리를 대상으로 호출
        else
            inventoryInfo.UpdateDicItemPosition( curActiveTab );
    }

    








}
