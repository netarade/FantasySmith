using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InventoryManagement;
using ItemData;
using System;

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
* <v7.1 - 2024_0111_최원준>
* 1- 서바이벌 장르에 맞게 아이템 클래스를 변경하면서 탭관련 로직을 전체,퀘스트아이템 탭에 맞게 수정
* 
* <v8.0 - 2024_0112_최원준>
* 1- 인벤토리 클래스 구조 변경으로 인해
* MoveActiveItemToSlot에서 dicLen을 dicLen-1까지만 하던것을 수정하고, if문으로 dicType 퀘스트 아이템 검사문을 집어넣음.
* 
* 2- 탭을 사용자가 직접 설정할 수 있도록 모듈화 
* TabType을 정의하고, 해당 배열 변수를 인스펙터뷰 상에서 직접 설정할 수 있도록 하였음.
* 
* 3- 탭버튼을 사용자가 정의한 showTabType 길이 만큼 동적할당하고, 해당 탭으로 이름도 맞춰주고, 이벤트를 등록해주도록 변경
* 
* 4- 기존에 ItemType에 맞게 정의 되어있던 메서드들을 TabType에 맞게 수정
* 
* 5- TabType과 ItemType간의 변환 메서드를 추가, 관련 변수를 추가
* 
* 
* <v8.1 - 2024_0113_최원준>
* 1- 인벤토리의 오픈 상태를 InventoryInfo클래스에서 관리할 필요성이 생겨 관련 메서드및 변수를 이전하였음.
* 
* <v8.2 - 2024_0114_최원준>
* 1- 인벤토리 닫기 버튼인 X버튼을 눌렀을 때의 BtnInventoryClose메서드를 정의
* 다른 인벤토리와 연결 중일 때는 연결 상태를 해제하고, 연결 상태가 아닐때는 자신의 창만 닫는 동작을 실행
* 
* <V9.0 - 2024_0115_최원준>
* 1- TapType 관련 정보와 메서드를 Inventory클래스로 이전하였으며, 메서드는 static으로 두어 접근성을 증가시킴.
* 이유는 Inventory클래스 내부적으로도 어떤 탭을 기준으로 Add할지, SlotIndex를 계산할지 여부가 필요하기 때문
* 
* 2- showTabType을 Interactive클래스에서 입력받아 초기화 하는 것을 Initializer클래스로 옮겨서 초기화하도록 변경
* 이유는 Inventory클래스에서 생성시 어떤 탭을 기준으로 Add할지 알아야 하기 때문에 초기화값을 넘겨받을 필요가 있기 때문
* 
* 3- showTabType을 CreateActiveTabBtn메서드에서 Initilizer로부터 값을 읽어들이도록 변경 
* 
* 4- 변수명 tabKindItemTypeList를 tabKindList로 변경
* 
* 5- 변수명 btnTabIdx를 btnTabType으로 변경
* 
* 6- MoveActiveItemToSlot메서드의 코드를 전체탭과 개별탭으로 구분하던 것을 하나의 로직으로 작성
* ConvertTabTypeToItemTypeList메서드에서 curActiveTab에 따라서 ItemType을 List에 담아주기 때문
* 
* <v9.1 - 2024_0116_최원준>
* 1- AdjustSlotCount메서드에서 개별탭의 제한 수를 딕셔너리 제한수를 누적시켜 구하던 것에서
* 바로 인덱스를 통해 구하도록 변경
* 
* 2- AdjustSlotCount메서드 내부 전체 탭의 제한 수를 구할 때 
* GetSlotCountLimitTab메서드를 사용하던 것을 slotCoutLimitTab변수를 바로 사용하도록 수정
* 
* 3- UpdateAllItemActiveTabInfo메서드 내부의 GetItemDicIgnoreExsists호출 매개변수 수정
* (ItemType)i 직접 전달에서 dicType[i]를 전달하도록 변경
* 
* 4- MoveSlotItemToEmptyList메서드명 MoveCurSlotItemToEmptyList로 변경하고,
* 전체 탭제한 기준으로 반복문을 돌려 아이템을 이동시키던 부분을 현재 액티브 탭의 슬롯 제한 개수로 변경.
* 또한 메서드 호출 위치를 curActiveTab 정보가 바뀌기 이전에 바로 호출해주는 것으로 변경
* 
* 5- MoveActiveItemToSlot메서드명 MoveCurActiveItemToSlot으로 변경
* (현재 탭정보와 일치하는 모든 아이템을 이동시킨다는 의미)
* 
* 6- UpdateAllItemActiveTabInfo메서드 내부 UpdateActiveTabInfo의 호출인자로 curActiveTab 추가
* 
* 7- BtnTapClick메서드 isActiveTabAll 구하는 부분을 if문에서 이항연산자로 변경
* 
* 8- CurActiveTab프로퍼티를 선언하여, ItemInfo에서 AddItem이 이루어질 때 최신탭정보를 개별적으로 수정하도록 하였음.
* 
* <v9.2- 2024_0116_최원준>
* 1- UpdateAllItemActiveTabInfo메서드 내부 GetItemDic관련 null검사문 추가
* 
* <v9.3 - 2024_0124_최원준>
* 1- Inventory클래스의 딕셔너리 저장형식을 GameObject기반에서 ItemInfo로 변경하면서 관련 메서드 수정
* (UpdateAllItemActiveTabInfo)
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
    GameObject slotPrefab;          // 슬롯을 동적으로 생성하기 위한 프리팹 참조
        
          
    TabType[] showTabType;    // Initializer의 인스펙터뷰에서 지정한 활성화 시킬 탭의 종류      
    TabType curActiveTab;       // 현재 활성화 중인 탭을 저장 (버튼 클릭 시 중복 실행을 방지하기 위해)
    bool isActiveTabAll;        // 현재 활성화 중인 탭이 전체 기준인지, 개별 기준인지 여부를 반환
    
    // 탭에 해당하는 아이템 타입을 담을 임시 리스트
    List<ItemType> tabKindList = new List<ItemType>();     



    /// <summary>
    /// 아이템이 선택중인지 여부를 반환하거나 설정합니다.<br/>
    /// 아이템이 1개라도 선택중이라면 다른 아이템은 선택할 수 없습니다.<br/>
    /// ItemDrag에서 정보를 받고 수정합니다.
    /// </summary>
    public bool IsItemSelecting {get; set;} = false;


    /// <summary>
    /// 현재 활성화 탭의 정보를 반환합니다.
    /// </summary>
    public TabType CurActiveTab { get {  return curActiveTab; } }   

    /// <summary>
    /// 현재 활성화탭이 전체 기준인지, 개별 기준인지 여부를 반환합니다.
    /// </summary>
    public bool IsActiveTabAll { get{return isActiveTabAll; } }






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
        
        slotListTr = inventoryTr.GetChild(0).GetChild(0).GetChild(0);  // 뷰포트-컨텐트-전체 슬롯리스트
        slotPrefab = slotListTr.GetChild(0).gameObject;                // 슬롯 리스트 하위에 미리 1개가 추가되어 있음
        emptyListTr = inventoryTr.GetChild(0).GetChild(1);             // 뷰포트-EmptyList
                                 
        CreateInventorySlot(caller);    // 인벤토리 슬롯 생성
        CreateActiveTabBtn();           // 액티브탭 버튼 생성
    }


    
    /// <summary>
    /// InventoryInfo에서 slotCoutLimit정보를 전달 받아서 슬롯을 생성하는 메서드입니다.<br/>
    /// slotCountLimit정보가 최신화된 상태에서 호출하여야 합니다.<br/>
    /// *** 동일 인벤토리의 InventoryInfo가 아니면 호출할 수 없습니다. ***
    /// </summary>
    private void CreateInventorySlot(InventoryInfo caller)
    {                          
        if( caller != inventoryInfo )
            throw new Exception("호출자를 확인하여 주세요. 동일 인벤토리에서만 호출 할 수 있습니다.");
                      
        // 내부 인벤토리에서 탭제한 수를 참조하기 위해 전체탭의 인덱스를 계산합니다.
        int allTabIdx = inventory.GetTabIndex(ItemType.None);

        // 플레이어 인벤토리 정보(전체 탭 슬롯 칸수)를 참조하여 슬롯을 동적으로 생성
        // 현재 인벤토리에 슬롯이 한 개 들어있으므로 하나를 감하고 생성
        for( int i = 0; i<caller.inventory.slotCountLimitTab[allTabIdx]-1; i++ )
            Instantiate( slotPrefab, slotListTr );
    }







    /// <summary>
    /// 버튼 액티브 탭을 동적으로 생성하고 이벤트를 등록합니다.
    /// </summary>
    private void CreateActiveTabBtn()
    {        
        // 보여줄 지정 탭타입을 이니셜라이저 클래스로부터 읽어들입니다.
        showTabType = inventoryTr.GetComponent<InventoryInitializer>().showTabType;

        // 사용자가 지정한 액티브탭 갯수
        int activeTabNum = showTabType.Length;

        // 미리 생성되어있는 탭버튼 1개를 참조합니다.
        Button tabBtnPrefab = inventoryTr.GetChild(1).GetChild(0).GetComponent<Button>();
               
        // 버튼 배열을 참조할 갯수를 설정합니다.
        btnTap = new Button[activeTabNum];         
               
        for(int i=0; i<activeTabNum; i++)
        {
            //람다식 클로져 특성으로 값을 그대로쓰지 않고 새롭게 할당합니다.
            TabType btnTabType = showTabType[i];   
                        
            // 버튼을 새롭게 생성하여 하이러키에 배치하고, 이름 설정 및 이벤트를 연결합니다.
            btnTap[i] = Instantiate(tabBtnPrefab.gameObject, tabBtnPrefab.transform.parent).GetComponent<Button>();
            btnTap[i].onClick.AddListener( () => BtnTapClick( btnTabType ) );
            btnTap[i].gameObject.name = GetTabObjectName(btnTabType);
            btnTap[i].GetComponentInChildren<Text>().text = GetTabTextName(btnTabType);
        }
        
        // 모든 탭버튼을 생성을 완료하였다면 기존 탭 버튼은 비활성화합니다.
        tabBtnPrefab.gameObject.SetActive(false);
        
        
        // 탭을 정의하지 않았다면 항상 전체탭을 기준으로 표시해주고,
        if( activeTabNum==0 )
        {
            isActiveTabAll = true;
            curActiveTab = TabType.All;
        }
        // 탭을 정의하였다면, 첫 시작 시 항상 첫번째 탭을 기준으로 Select로 표시해줍니다.
        else
        {            
            isActiveTabAll = (showTabType[0]==TabType.All) ? true : false;
            curActiveTab = showTabType[0];
            btnTap[0].Select();
        }

    }


    


    /// <summary>
    /// 인자로 들어온 탭 종류의 게임 상에 보여질 텍스트명을 얻습니다.
    /// </summary>
    /// <returns>해당 탭의 텍스트 명</returns>
    public string GetTabTextName(TabType tabType)
    {
        string name = "";

        if(tabType==TabType.All)
            name = "전체";
        else if(tabType==TabType.Quest)
            name = "퀘스트";
        else if(tabType == TabType.Misc)   
            name = "잡화";
        else if(tabType==TabType.Equip)
            name = "장비";
        else
            throw new Exception("탭 이름이 설정되지 않았습니다.");

        return name;
    }

    /// <summary>
    /// 인자로 들어온 탭 종류의 오브젝트의 이름을 얻습니다.
    /// </summary>
    /// <returns>해당 탭의 오브젝트 명</returns>
    public string GetTabObjectName(TabType tabType)
    {
        string name = "";

        if(tabType==TabType.All)
            name = "ActiveTab-All";
        else if(tabType==TabType.Quest)
            name = "ActiveTab-Quest";
        else if(tabType==TabType.Misc)   
            name = "ActiveTab-Misc";
        else if(tabType==TabType.Equip)
            name = "ActiveTab-Weapon";
        else
            throw new Exception("탭 오브젝트명이 설정되지 않았습니다.");

        return name;
    }







    





















    /// <summary>
    /// X버튼을 눌러 인벤토리를 닫을 때 호출해주는 메서드입니다.<br/>
    /// 다른 인벤토리와 연결 상태라면 연결을 해제하고 모든 인벤토리 창을 닫으며,<br/>
    /// 연결 상태가 아니라면 자신의 인벤토리 창만 닫습니다.
    /// </summary>
    public void BtnInventoryClose()
    {
        if(inventoryInfo.IsConnect)
            inventoryInfo.DisconnectInventory();
        else
            inventoryInfo.InitOpenState(false);
    }












    /// <summary>
    /// 버튼 클릭 시 각탭에 해당하는 아이템만 보여주기 위한 메서드입니다. 각 탭의 버튼 클릭 이벤트에 등록해야 합니다.
    /// </summary>
    public void BtnTapClick( TabType btnTabType )
    {        
        // 현재 이벤트가 일어난 탭인덱스가 현재 활성화중인 탭과 동일하다면 재실행하지 않습니다.
        if( curActiveTab==btnTabType )      
            return;
                
        MoveCurSlotItemToEmptyList();           // (탭정보가 바뀌기전에) 현재 탭의 슬롯에 있는 모든 아이템을 빈리스트로 이동합니다.

        // 인자로 들어온 최신 탭정보를 현재 활성화 탭으로 지정합니다.
        curActiveTab = btnTabType;

        // 탭선택에 따라 탭 활성화 변수를 전체탭인지, 개별탭인지 설정합니다.
        isActiveTabAll = (btnTabType==TabType.All)? true : false;

        UpdateAllItemActiveTabInfo();           // 인벤토리의 모든 아이템에 활성화 탭 정보를 반영합니다
        AdjustSlotCount();                      // 활성화 탭의 갯수만큼 슬롯을 늘리거나 줄입니다
        MoveCurActiveItemToSlot();              // 활성화 탭 종류와 동일한 딕셔너리의 모든 아이템을 슬롯에 배치합니다       
    }



    /// <summary>
    /// 인벤토리가 현재 보유하고 있는 모든 사전의 아이템에 활성화탭 정보를 반영합니다.<br/>
    /// </summary>
    private void UpdateAllItemActiveTabInfo()
    {
        // 하나씩 가져와서 참조 할 딕셔너리 변수 선언
        Dictionary<string, List<ItemInfo>> itemDic;

        for( int i = 0; i<inventory.dicLen; i++ )
        {
            itemDic=inventory.GetItemDic( inventory.dicType[i] );          // CurDicLen 갯수만큼 딕셔너리를 하나씩 변경합니다.

            // 아이템사전이 존재하지 않거나 내부 리스트가 존재하지 않는다면 다음 사전을 찾습니다
            if(itemDic == null || itemDic.Count==0)
                continue;

            foreach( List<ItemInfo> itemInfoList in itemDic.Values )      // 해당 딕셔너리의 ItemInfo리스트를 가져옵니다.
            {
                foreach( ItemInfo itemInfo in itemInfoList )               // ItemInfo리스트에서 ItemInfo를 하나씩 가져옵니다.
                    itemInfo.UpdateActiveTabInfo(this, curActiveTab, isActiveTabAll);   // 아이템의 활성화 탭 정보를 최신화합니다.
            }
        }
    }

    /// <summary>
    /// 각 탭을 클릭했을 때 해당 탭의 칸 수만큼 슬롯의 갯수를 줄이거나 늘려줍니다
    /// </summary>
    private void AdjustSlotCount()
    {
        // 현재 탭에 해당하는 슬롯 갯수
        int curTabSlotCount = inventory.slotCountLimitTab[(int)curActiveTab];

        // 전체 탭에 해당하는 슬롯 갯수
        int allTabSlotCount = inventory.slotCountLimitTab[(int)TabType.All];

        // 현재 탭의 갯수만큼 켜줍니다
        for(int i=0; i<curTabSlotCount; i++)                   
            slotListTr.GetChild(i).gameObject.SetActive(true);   

        // 나머지는 꺼줍니다 
        for(int i=curTabSlotCount; i<allTabSlotCount; i++)
            slotListTr.GetChild(i).gameObject.SetActive(false);  
    }




    /// <summary>
    /// 현재 슬롯 리스트에 있는 오브젝트를 일시적으로 빈 리스트로 옮겨주는 메서드입니다
    /// </summary>
    private void MoveCurSlotItemToEmptyList()
    {
        
        for( int i = 0; i<inventory.slotCountLimitTab[(int)curActiveTab]; i++ )     // 현재 슬롯리스트의 모든 슬롯 순회
        {
            if( slotListTr.GetChild(i).childCount!=0 )          // i번째 슬롯에 아이템이 들어있다면
            {
                // 슬롯에 들어있는 아이템 오브젝트를 모두 잠시 오브젝트 emptyList로 이동하고, 위치를 수정합니다
                slotListTr.GetChild(i).GetChild(0).SetParent(emptyListTr, false);
            }
        }
    }


    /// <summary>
    /// 현재 활성화 중인 탭 종류와 동일한 딕셔너리의 모든 아이템을 슬롯에 배치합니다<br/>
    /// 내부적으로 InventoryInfo 클래스의 개별 딕셔너리에 존재하는 모든 아이템의 위치 정보를 업데이트하는 메서드를 활용합니다
    /// </summary>
    private void MoveCurActiveItemToSlot()
    {
        // 탭종류에 해당하는 아이템 종류와 길이를 구합니다.
        int tabKindLen = Inventory.ConvertTabTypeToItemTypeList(ref tabKindList, curActiveTab);

        // 탭종류에 해당하는 모든 아이템의 포지션을 업데이트 합니다.
        for(int i=0; i<tabKindLen; i++)
            inventoryInfo.UpdateDicItemPosition( tabKindList[i] );  
    }

    





}
