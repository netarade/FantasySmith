using UnityEngine;
using UnityEngine.UI;
using ItemData;
using InventoryManagement;
using System;

/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1102_최원준>
 * 1- 최초작성 및 주석처리
 * 
 * <v2.0 - 2023_1103_최원준>
 * 1- 주석 수정
 * 2- 이미지 컴포넌트 잡는 구문을 Start메서드에서 OnEnable로 변경
 * 인스턴스가 생성되어 이미지 컴포넌트를 잡기 시작하면 OnItemAdded와 호출 시점이 동시성을 가져서 스프라이트 이미지가 변경되지 않는다.
 * 
 * <v3.0 - 2023-1105_최원준>
 * 1- 개념아이템 변수인 item을 프로퍼티화 시켜서 set이 호출되었을 때 OnItemChanged()가 호출되도록 변경 
 * OnItemAdded는 private처리 및 내부 예외처리 구문 삭제
 *
 *<v4.0 - 2023_1108_최원준>
 *1- 아이템이 파괴될 때 정보를 저장하도록 구현하였으나, 모든아이템의 기록이 저장되지 않는 문제발생
 *=> 아이템쪽에서 파괴될떄마다 딕셔너리를 생성해서 CraftManager쪽에서 마지막에 한번 미리 생성해주도록 변경
 *
 *2- OnItemChanged 메서드 주석추가
 *
 *3- UpdateCountTxt 메서드 추가
 * 아이템 수량이 변경 될때 동적으로 텍스트를 수정해주도록 하였음.
 * item쪽에서 메서드를 가지고 있음으로 해서 편리한 접근성 확보.
 *
 *<v5.0 - 2023_1112_최원준>
 *1- OnItemAdded메서드 추가수정. (CreateManager쪽에서 중복코드 사용하고 있던 점 수정 및 통합, 주석 추가) 
 *
 *<v6.0 - 2023_1114_최원준>
 *1- OnItemAdded메서드를 OnItemChanged로 이름변경
 *2- ItemInfo 클래스 설명 주석 추가
 *3- private 메서드 public 메서드로 변경
 *4- 멤버 변수 item이 public되어있던 점을 private 처리. 반드시 프로퍼티를 통한 초기화를 위해
 *
 *<v7.0 - 2023_1116_최원준>
 *1- ItemInfo 클래스가 ItemImageCollection 멤버변수를 포함하여 외부이미지를 참조하도록 설정하였습니다. 
 *(CreateManager에 있던 참조 변수를 옮겨옴.)
 *
 *2- UpdateImage메서드를 수정하였습니다.
 *기존의 아이템 클래스가 ImageCollection 구조체 변수를 멤버로 포함하고 있던 점에서 ImageReferenceIndex 구조체 변수를 멤버로 포함하도록 바꾸었기 때문에
 *item의 ImageReferenceIndex 멤버변수로 부터 인덱스값을 받아와서 ImageCollection 변수에 접근하여 오브젝트에 이미지를 넣어주도록 수정.
 *
 *<v7.1 - 2023_1119_최원준>
 *1- OnDestroy()메서드 주석처리. 
 *Inventory클래스를 직렬화 가능하게 변경할 예정이므로
 *
 *2- RemoveItemObject 미구현 메서드 제거 - inventory클래스에서 구현
 *ItemInfo 클래스는 오브젝트의 정보만 최신화 시켜주는 역할을 하게 해야하기 때문이며, 
 *ItemInfo에서 item의 내부정보를 수정하는 메서드를 추가하기 시작하면, Inventory클래스에서의 기능을 중복 구현할 가능성이 커짐.
 *
 *<v8.0 - 2023_1216_최원준>
 *1- 아이템의 상태창 이미지 변수 statusImage 추가 및 UpdateImage메서드 내부 수정
 *
 *2- 아이템 파괴시 전달 로직 주석처리 되어있던 부분 제거
 *
 *3- slotList 변수명 slotListTr로 변경
 *
 *4- Transform imageCollectionsTr 임시변수 선언후 
 * GameObject.Find( "ImageCollections" ) 중복 호출 로직 수정
 *
 *<v8.1 - 2023_1217_최원준>
 *1- ItemImageCollection 변수들을 하나씩 참조하던 것을 배열로 만들어서 참조
 *
 *<v8.2 - 2023_1221_최원준>
 *1- GameObject.Find()메서드로 오브젝트를 검색하던 것을 빠른참조로 변경
 *
 *<v9.0 - 2023_1222_최원준>
 *1- 태그참조 철자오류 수정 (CANVAS_CHRACTER -> CANVAS_CHARACTER)
 *
 *2- ItemImageCollection[]의 배열을 참조만하고 생성을 안해서 뜨는 배열의 bounds오류 수정
 *
 *3- 아이템의 생성시점에 UpdateImage나 UpdatePosition을 호출하면 참조가 잡히지 않기 때문에 bounds오류가 뜨는데
 * OnItemChanged메서드를 아이템의 생성시점 호출이 아니라, 아이템의 등장 시점에 호출하도록 수정하였음.
 *
 *4 - SlotListTr이 뷰포트로 잡혀있던 점을 수정
 *
 *<v9.1 - 2023_1224_최원준>
 *1- Item프로퍼티의 주석일부 삭제
 *2- 컴포넌트 참조 구문 Start에서 OnEanble로 이동 및 정리
 *3- UpdataImage메서드 아이템 종류에 따른 중복로직 제거 후 간략화
 *
 *<v9.2 - 2023_1226_최원준>
 *1- 일부 디버그 출력메서드 정리
 *2- UpdatePosition에 slotListTr의 childCount 검사구문 추가
 *3- Item 프로퍼티 다시 주석해제 
 *
 *<v9.3 - 2023_1228_최원준>
 *1- 아이템 프리팹 계층 구조 변경으로 인해 (3D오브젝트 하위에 2D 오브젝트를 두는 구조)
 *각각 Transform, RectTransform itemTr와 itemRectTr 변수를 선언하여 자기트랜스폼 캐싱처리
 *
 *2- UpdatePosition 2D오브젝트의 부모를 변경하던 점을 최상위 3D오브젝트를 변경하도록 수정
 *
 *3- UpdatePosition 변수명을 UpdateInventoryPosition으로 변경
 *
 *<v9.4 - 2023_1228_최원준
 *1- 아이템 프리팹 계층구조 재변경으로 인해 (3D오브젝트, 2D오브젝트 전환방식)
 *코드를 2D기준으로 임시 변경 (itemTr->itemRectTr)
 *
 *
 *<v10.0 - 2023-1229_최원준>
 *1- UpdateSlotPosition에서 선택인자를 추가하여 슬롯리스트 정보를 주는 경우에는 해당 슬롯리스트의 인덱스로 포지션업데이트를 하며,
 *슬롯리스트 정보를 주지 않은 경우에는 아이템이 현재 담겨있는 슬롯을 기준으로 슬롯리스트를 참조해서 인덱스에 따른 포지션 업데이트를 하도록 변경
 *
 *<v10.1 - 2023_1230_최원준>
 *1- UpdateSlotPoisition메서드명을 UpdatePositionInSlotList로 변경 하고
 *슬롯리스트를 인자로 받도록 설정. 선택인자로 인자가 전달되지 않았다면 계층구조를 참조하여 자동계산하도록 변경
 *
 *2- OnItemChanged메서드를 외부에서 호출할 때 slotList를 인자로 받아서 호출가능하도록 변경,
 *UpdatePositionInSlotList에 slotList인자를 전달하여 호출. 마찬가지로 선택인자로 호출가능
 *
 *3- 계층구조 전환코드를 간단하게 수정
 *미리 ItemTr ItemRectTr을 잡아놓을 필요없이
 *전환이 이루어질 때의 메서드를 호출하면 2D가 메인이여야 할때는 하위 마지막자식 
 *
 *<v10.2 - 2023_1231_최원준>
 *1- Locate 2D, 3D 메서드
 *2- SetOverlapCount메서드
 *3- FindNearstRemainSlotIdx 메서드 inventoryInfo 매개변수 오버로딩
 *
 *<v10.3 - 2024_0101_최원준>
 *1- 아이템이 현재 활성화 기준의 탭을 확인하고 포지션업데이트를 해야 하므로, 인터렉티브 스크립트에서 활성화탭 기준 변수를 받아도록 하였음
 *2- prevDropEventCallerTr변수 설정 및 OnItemDrop메서드 추가
 *
 *3- FindNearstRemainSlotIdx 메서드 모두 삭제
 *InventoryInfo에 있어야 할 기능이며, 아이템이 인벤토리 정보를 참조하고 있기 때문에 인벤토리를 참조하여 호출하기만 하면 되기 때문
 *
 *4- OnEnable에서 슬롯리스트 설정 시 canvsTr을 find 메서드로 찾아서 처리하던 점 삭제하고,
 *UpdateInventoryInfo메서드 호출로 변경
 *
 *<v10.4 - 2024_0102_최원준>
 *1- OnItemDrop메서드 구현완료
 *어떤 아이템의 드롭 이벤트 발생시 외부 스크립트에서 호출하도록 설정
 *2- UpdateActiveTabInfo 메서드 정의하고 OnItemChanged 내부에 추가.
 *
 *
 * <v11.0 - 2023_0102_2_최원준>
 * 1- OnItemWorldDrop메서드의 매개변수를 dropEventCallerTr에서 worldPlaceTr로 변경
 * 
 * 2- Transfer2DToWorld메서드 수정
 * 
 * a. prevDropEventCallerTr참조가 최신화 안하고 null로 잡혀있던 점 수정
 * b. inventoryInfo.RemoveItem(this);가 null값 전달 시 없던 점 수정.
 * c. 예외처리 상황 isWorldPositioned기반으로 변경
 * 
 * 3- OnItemWorldDrop, Transfer2DToWorld메서드 Vector3, Quaternion인자 전달 기반 오버로딩, 반환값 void로 수정
 * 
 * 4- UpdateInventoryInfo메서드에서 prevDropEventCaller 정보를 최신화하는 코드 추가
 * 
 * 5- OnItemChanged메서드를 OnItemCreated로 변경 및 주석 수정, 인자로 InventoryInfo 스크립트를 받도록 설정.
 * 아이템이 인벤토리에 생성되는 상황에서 호출하는 것이 더욱 명확한 의미기이 때문
 * 
 * 6- OnEnable메서드 주석보완, canvasTr의 계층참조로 처리하던 코드 삭제
 * 
 * 7- UpdateActiveTabInfo 메서드 주석 보완 및
 * 인자를 제한적으로 받아 어떤 상황에서 호출해야 하는 지 명확하게 설정
 * 
 * 8- OnItemSlotDrop 주석 수정
 * 
 * 9- ChangeHierarchy메서드 선택인자 null값 추가
 * 3D->Slot에서 호출 시 인덱스를 입력하고 인덱스에 따른 Position Update를 따로 해주어야 하기 때문에 부모 설정이 필요하지 않음.
 * 
 * 10- Locate3DToSlot, SlotList, InventoryInfo등의 오버로딩 메서드 삭제 후 
 * Locate3DToInventory메서드 하나로 통일.
 * 
 * 11- 사용자가 호출 할 OnItemGain메서드 추가
 *
 *
 * <v11.1 - 2024_0102_최원준>
 * 1- ItemInfo 클래스 partial클래스로 정의
 * 2- 아이템 수량관련 기본 메서드를 ItemInfo_2.cs로 옮김
 * 3- emptyListTr 변수 추가
 * 아이템을 삭제하기 전 임시로 빈 공간으로 옮길 슬롯리스트
 * 
 * 
 * 
 *
 *
 *
 * [추후 수정해야할 점] 
 *  1- UpdateInventoryPosition이 현재 자신 인벤토리 기준으로 수정하고 있으나,
 *  나중에 UpdatePosition을 할 때 아이템이 보관함 슬롯의 인덱스 뿐만 아니라 어느 보관함에 담겨있는지도 정보가 있어야 한다.
 * 
 * 2- 슬롯드롭이벤트가 발생할 때 인벤토리 정보가 다르다면 이전 인벤토리에서 이 아이템 목록을 제거해야한다.
 *  계층변경이 일어날 때 인벤토리에서 이 아이템을 목록에 추가하거나 제거해야 한다.
 *  Drag에서 인벤토리 밖으로 빼냈을 때 인벤토리 목록에서 이 아이템을 제거해야 한다.
 *
 *
 *
 *
 * [이슈_0101] 
 * 1- 슬롯의 정보(인벤토리 정보) 업데이트 시점
 * a- Slot To Slot에서 SlotDrop이 일어날 때 Slot을 통해 받아야 한다.
 * b- ItemDrag해서 인벤토리 외부로 Drop할때 ItemDrag를 통해 받아야 한다.
 * c- ItemInfo에서 2D to World(월드정보인자), 3D to Slot(인벤토리정보인자) 할때 자체적으로 확인해야 한다.
 * => 외부에서 업데이트 메서드를 호출할 수 있게 해줘야한다.
 *
 * 2- 타 인벤토리로 업데이트가 이뤄질 때는 
 * slotIndexAll과 slotIndex 모두를 받야야 한다.
 * 이유는 한번 받은 상태에서 다른 탭변경을 시도하면 위치 정보가 안맞기 때문
 *
 *
 * [이슈_0102]
 * 1- UpdateImage에서 iicArr에 인덱스를 대입하여 참조값을 찾아가는 부분이 있는데, 
 * 모든 아이템이 이 메서드를 가지고 있을 필요가 없기때문에 특정 인스턴스에 요청해서 정보를 받아오면 된다고 생각되는데,
 * CreateManager에 요청하는 것이 적합하지만, 이미지 우선순위로 싱글톤에서 참조를 받아오면 시점이 늦기때문에 다른방안을 생각 중.
 * => 아이템 생성 전 미리 오브젝트를 만들어놓고 해당 메서드를 호출하도록 하는 방식으로 접근하면 어떨까 생각.
 * 
 * 2- OnEnable에서 OnItemCreate메서드를넣고 UpdateInventoryInfo를 따로 호출할지 생각 중.
 * => 이미지 우선순위로 OnItemCreate를 수동으로 호출로 결정, UpdateInventoryInfo메서드를 내부에 포함.
 * 
 * [나중에 수정할 것_0102]
 * 1- SetOverlap메서드 수정
 * 2- OnEnable에 있는 iic참조 관련 메서드 렌더링 관리 클래스쪽에서 호출 할 예정
 *
 */


/// <summary>
/// 게임 상의 아이템 오브젝트는 이 클래스를 컴포넌트로 가져야합니다.<br/><br/>
/// 
/// ItemInfo 스크립트가 컴포넌트로 붙은 아이템 오브젝트의 자체적인 기능은 다음과 같습니다.<br/>
/// (ItemInfo의 개념 아이템 인스턴스인 item이 할당될 때 자동으로 이루어집니다.)<br/><br/>
/// 
/// 1.오브젝트의 이미지를 개념 아이템의 정보와 대조하여 채웁니다.<br/>
/// 2.잡화아이템의 경우 중첩횟수를 아이템정보와 비교하여 표시하여 줍니다. 비잡화 아이템의 경우 텍스트를 끕니다.<br/>
/// 3.인벤토리 슬롯 상의 포지션을 아이템 정보와 대조하여 해당 슬롯에 위치시킵니다.<br/><br/>
/// 
/// 주의) 아이템의 내부 정보가 바뀔 때 마다 최신 정보를 오브젝트에 반영해야 합니다.<br/>
/// 1,2,3의 경우 각 메서드를 따로 호출 할 수 있으며 모든 것을 한번에 호출하는  OnItemChanged메서드가 있습니다.<br/>
/// </summary>
public partial class ItemInfo : MonoBehaviour
{
    /**** 아이템 고유 정보 ****/
    private Item item;             // 아이템의 실제 정보가 담긴 변수

    private Image itemImage;       // 아이템이 인벤토리에서 2D상에서 보여질 이미지 컴포넌트 참조  
    public Sprite innerSprite;     // 아이템이 인벤토리에서 보여질 이미지 스프라이트
    public Sprite statusSprite;    // 아이템이 상태창에서 보여질 이미지 스프라이트 (상태창 스크립트에서 참조를 하게 됩니다.)
    private Text countTxt;         // 잡화 아이템의 수량을 반영할 텍스트
            
    private RectTransform itemRectTr;       // 자기자신 2D 트랜스폼 참조(초기 계층 - 상위 부모)
    private Transform itemTr;               // 자기자신 3D 트랜스폼 참조(초기 계층 - 하위 마지막 자식)
    private CanvasGroup itemCG;             // 아이템의 캔버스 그룹 컴포넌트 (아이템이 월드로 나갔을 때 2D이벤트를 막기위한 용도) 
    

    /*** 아이템 외부 참조 정보 ***/
    public ItemImageCollection[] iicArr;                             // 인스펙터 뷰 상에서 등록할 아이템 이미지 집합 배열
    public enum eIIC { MiscBase,MiscAdd,MiscOther,Sword,Bow,Axe }    // 이미지 집합 배열의 인덱스 구분
    private readonly int iicNum = 6;                                 // 이미지 집합 배열의 갯수



    /*** 아이템 변동 정보 ***/

    /*** Locate2DToWorld 또는 Locate3DToWorld 메서드 호출 시 변동***/
    private bool isWorldPositioned;         // 아이템이 월드에 나와있는지 여부

    /**** InventoryInfoChange 메서드 호출 시 변동 ****/
    private Transform inventoryTr;              // 현재 아이템이 들어있는 인벤토리의 계층정보를 참조합니다.
    private Transform slotListTr;               // 현재 아이템이 담겨있는 슬롯리스트 트랜스폼 정보
    private Transform emptyListTr;              // 아이템을 임시로 이동 시킬 빈공간 리스트

    private InventoryInfo inventoryInfo;        // 현재 아이템이 참조 할 인벤토리정보 스크립트
    private InventoryInteractive interactive;   // 현재 아이템이 참조 할 인터렉티브 스크립트
    private Transform playerTr;                 // 현재 아이템을 소유하고 있는 플레이어 캐릭터 정보 참조
    private Transform playerDropTr;             // 플레이어가 아이템을 드롭시킬 때 아이템이 떨어질 위치


    /**** InventoryInfoChange 메서드 호출시 변동 ****/
    /**** inveractive에서 변동일어 날 때마다 변동*****/
    private bool isActiveTabAll;                // 현재 아이템이 담겨있는 인벤토리의 활성화 탭의 기준이 전체인지, 개별인지 여부

    /**** OnItemDrop, OnItemSlotDrop 이벤트 호출 시 변동 ****/
    private Transform prevDropEventCallerTr;    // 드랍이벤트가 발생할 때 이전의 드랍이벤트 호출자를 기억하기 위한 참조 변수 





    
    /*** 내부에서만 수정가능한 읽기전용 프로퍼티***/

    /// <summary>
    /// 아이템이 월드에 나와있는지 (3D 오브젝트 인지) 여부를 반환합니다.
    /// </summary>
    public bool IsWorldPositioned { get {return isWorldPositioned;} }
    
    /// <summary>
    /// 현재 아이템이 담긴 인벤토리의 정보입니다.
    /// </summary>
    public InventoryInfo InventoryInfo { get {return inventoryInfo;} }

    /// <summary>
    /// 현재 아이템이 담겨있는 슬롯리스트의 Transform을 반환합니다.<br/>
    /// </summary>
    public Transform SlotListTr { get {return slotListTr;} }







    /*** 내부 아이템의 속성을 변경해주는 프로퍼티 ***/

    /// <summary>
    /// 아이템 오브젝트가 해당하는 탭에서 몇번 째 슬롯에 들어있는 지 인덱스를 반환하거나 설정합니다.
    /// </summary>
    public int SlotIndex { get{return item.SlotIndex;} set{ item.SlotIndex=value;} }

    
    /// <summary>
    /// 아이템 오브젝트가 전체 탭에서 몇번 째 슬롯에 들어있는 지 인덱스를 반환하거나 설정합니다.
    /// </summary>
    public int SlotIndexAll { get{return item.SlotIndexAll;} set{item.SlotIndexAll=value;} }
      
    /// <summary>
    /// 아이템이 담겨있는 실제 정보를 직접 저장하거나 반환받습니다.<br/>
    /// 클론 한 Item 인스턴스를 저장하고, 저장 되어있는 인스턴스를 불러올 수 있습니다.<br/>
    /// </summary>
    public Item Item { set{ item=value; } get { return item; } }







    /// <summary>
    /// 이미지 컴포넌트를 잡는 우선순위를 높이기 위해 OnEnable 사용<br/>
    /// 자기자신의 컴포넌트 참조와 변동이 절대로 없는 외부 컴포넌트의 참조를 잡는데 사용합니다.<br/>
    /// </summary>
    private void OnEnable()
    {
        itemRectTr = transform.GetComponent<RectTransform>();   // 자기자신 2d 트랜스폼 참조(초기 계층 - 상위 부모)
        itemTr = itemRectTr.GetChild(itemRectTr.childCount-1);  // 자기자신 3d 트랜스폼 참조(초기 계층 - 하위 마지막 자식)

        itemImage = GetComponent<Image>();
        countTxt = GetComponentInChildren<Text>();
                
        // 인스펙터뷰 상에서 달아놓은 스프라이트 이미지 집합을 참조합니다.
        Transform imageCollectionsTr = GameObject.FindAnyObjectByType<CreateManager>().transform.GetChild(0);

        // 배열을 해당 갯수만큼 생성해줍니다.
        iicArr = new ItemImageCollection[iicNum];

        // 각 iicArr은 imageCollectionsTr의 하위 자식오브젝트로서 ItemImageCollection 스크립트를 컴포넌트로 가지고 있습니다
        for( int i = 0; i<iicNum; i++)
            iicArr[i] = imageCollectionsTr.GetChild(i).GetComponent<ItemImageCollection>();
        
        isWorldPositioned = false;
        itemCG = GetComponent<CanvasGroup>();
    }
    

    /// <summary>
    /// 아이템이 새롭게 생성되는경우에 호출해야 할 메서드입니다.<br/>
    /// 로드하여 기존 아이템 정보를 바탕으로 오브젝트를 생성하거나, 신규 아이템을 씬에 생성시킬 때 사용합니다.<br/><br/>
    /// 현재 Inventory의 Load메서드, Inventory의 CreateItem메서드, CreateManager의 CreateItemToWorld메서드에서 사용예정<br/>
    /// **** 인자로 전달받은 inventoryInfo의 slotIndex를 기반으로 오브젝트를 보여주기 때문에<br/>
    /// 신규 아이템을 생성 시에는 반드시 인덱스 정보를 입력 후 호출해야 합니다. ****<br/>
    /// </summary>
    public void OnItemCreated(InventoryInfo inventoryInfo)
    {
        /*** 인자 미 전달 시 예외처리 ***/
        if(inventoryInfo==null)
            throw new Exception("이 메서드는 인벤토리 정보가 반드시 필요합니다. 확인하여 주세요");

        /*** 인벤토리 오브젝트의 계층 정보 설정 ***/
        Transform IntenvoryTr = inventoryInfo.transform;

        /*** 아이템 오브젝트의 고유 정보를 읽어 들여 오브젝트에 반영 ***/
        UpdateImage();                      // 이미지 최신화
        UpdateCountTxt();                   // 중첩 수량 정보 최신화

        /*** 인벤토리 정보를 읽어들여 오브젝트에 반영 ***/
        UpdateInventoryInfo(IntenvoryTr);   // 인벤토리 정보를 최신화
        UpdateActiveTabInfo(interactive);   // 활성화 탭 최신화
        UpdatePositionInSlotList();         // 슬롯 위치 최신화
    }



    /// <summary>
    /// 아이템의 이미지 정보를 받아와서 오브젝트에 반영합니다.<br/>
    /// Item 클래스는 정의될 때 외부에서 참조할 이미지 인덱스를 저장하고 있습니다.<br/>
    /// 해당 인덱스를 참고하여 인스펙터뷰에 등록된 이미지를 참조합니다.
    /// </summary>
    public void UpdateImage()
    {
        if(iicArr.Length == 0 )     // 아이템 생성 시점에 iicArr을 참조하는 것을 방지하여 줍니다.
            return;

        int imgIdx = -1;            // 참조할 이미지 인덱스 선언
                   
        switch( Item.Type )         // 아이템의 메인타입을 구분합니다.
        {

            case ItemType.Weapon:
                ItemWeapon weapItem = (ItemWeapon)Item;
                WeaponType weaponType = weapItem.WeaponType;  // 아이템의 서브타입을 구분합니다.

                switch (weaponType)
                {
                    case WeaponType.Sword :             // 서브타입이 검이라면,
                        imgIdx = (int)eIIC.Sword;
                        break;
                    case WeaponType.Bow :               // 서브타입이 활이라면,
                        imgIdx = (int)eIIC.Bow;
                        break;
                }
                break;
                
            case ItemType.Misc:
                ItemMisc miscItem = (ItemMisc)Item; 
                MiscType miscType = miscItem.MiscType;

                switch (miscType)
                {
                    case MiscType.Basic :           // 서브타입이 기본 재료라면,
                        imgIdx = (int)eIIC.MiscBase;
                        break;
                    case MiscType.Additive :        // 서브타입이 추가 재료라면,
                        imgIdx = (int)eIIC.MiscAdd;
                        break;
                    default :                       // 서브타입이 기타 재료라면,
                        imgIdx = (int)eIIC.MiscOther;
                        break;
                }
                break;
        }

        // 아이템 오브젝트 이미지를 인스펙터뷰에 직렬화되어 있는 ItemImageCollection 클래스의 내부 구조체 배열 ImageColection[]에
        // 개념아이템이 고유 정보로 가지고 있는 ImageReferenceIndex 구조체의 인덱스를 가져와서 접근합니다.                
             
        innerSprite = iicArr[imgIdx].icArrImg[Item.ImageRefIndex.innerImgIdx].innerSprite;
        statusSprite = iicArr[imgIdx].icArrImg[Item.ImageRefIndex.statusImgIdx].statusSprite;

        // 참조한 스프라이트 이미지를 기반으로 아이템이 보여질 2D이미지를 장착합니다.
        itemImage.sprite = innerSprite;
    }


    /// <summary>
    /// 잡화 아이템의 중첩횟수를 동적으로 수정합니다. 잡화 아이템의 수량이 변경될 때 마다 호출해 주십시오.
    /// </summary>
    public void UpdateCountTxt()
    {
        if( Item.Type==ItemType.Misc )                // 잡화 아이템의 중첩 갯수를 표시합니다.
        {
            countTxt.enabled=true;
            countTxt.text = ((ItemMisc)Item).OverlapCount.ToString();
        }
        else
            countTxt.enabled = false;                // 잡화아이템이 아니라면 중첩 텍스트를 비활성화합니다.
    }
        
    /// <summary>
    /// 현재 아이템이 슬롯 리스트에 속해있다면 위치를 슬롯 인덱스에 맞게 최신화시켜줍니다.<br/>
    /// 슬롯 인덱스가 잘못되어 있다면 다른 위치로 이동 할 수 있습니다.<br/><br/>
    /// ** 아이템이 월드 상에 있거나 슬롯 참조가 안 잡힌 경우 예외를 던집니다. **<br/>
    /// ** 만약 슬롯 인덱스 정보가 다른 아이템과 동일하다면 같은 슬롯으로 중첩될 것입니다.(현재 중첩 허용상태) **
    /// </summary>
    public void UpdatePositionInSlotList(bool overlapMode=false)
    {
        // 아이템이 월드상에 나와있거나 참조가 잡히지 않았다면, 예외를 던집니다.
        if( isWorldPositioned && slotListTr == null )   
            throw new Exception("아이템 정보를 업데이트 할 수 있는 상황이 아닙니다. 확인하여 주세요.");

        // 슬롯 리스트에 슬롯이 생성되어있지 않다면 하위로직을 실행하지 않습니다.
        if( slotListTr.childCount==0 )
        {
            Debug.Log( "현재 슬롯이 생성되지 않은 상태입니다." );
            return;
        }
        else if( !overlapMode && slotListTr.childCount>1 )
        {
            throw new Exception("아이템 인덱스 정보가 일치하여 중복되는 아이템이 있습니다. 확인하여 주세요");
        }
                 
        // 현재 활성화 중인 탭을 기반으로 어떤 인덱스를 참조할지 설정합니다.
        int activeIndex = isActiveTabAll? item.SlotIndexAll : item.SlotIndex;
        itemRectTr.SetParent( slotListTr.GetChild(activeIndex) );     // 아이템의 부모를 해당 슬롯으로 설정합니다.
        itemRectTr.localPosition = Vector3.zero;                      // 로컬위치를 슬롯에 맞춥니다.
    }


    /// <summary>
    /// 아이템의 인벤토리 정보가 변경될 때 인자로 받은 Transform 정보를 기준으로 다시 설정해주는 메서드입니다.<br/>
    /// 이전 인벤토리 목록에서 해당 아이템을 제거하고, 새로운 인벤토리에 해당 아이템의 목록을 추가하여 줍니다.<br/><br/>
    /// 아이템이 기존 인벤토리에서 움직이고 있는 경우는 호출될 필요가 없습니다.<br/>
    /// 아이템이 새로운 인벤토리로 들어오거나, 기존 인벤토리에서 나갈 때 호출해줘야 합니다.<br/>
    /// </summary>
    public void UpdateInventoryInfo(Transform newInventoryTr)
    {        
        // null값이 전달된 경우는 월드로 나갔다고 판단합니다.
        if(newInventoryTr == null)  
        {
            inventoryTr = null;
            inventoryInfo = null;
            interactive = null;

            slotListTr = null;
            emptyListTr = null;

            playerTr = null;
            playerDropTr = null;

            prevDropEventCallerTr = null;
        }
        else // 다른 인벤토리로 전달된 경우
        {
            // 인벤토리 참조 정보를 업데이트 합니다.
            inventoryTr = newInventoryTr;
            inventoryInfo = inventoryTr.GetComponent<InventoryInfo>();
            interactive = inventoryTr.GetComponent<InventoryInteractive>();

            if(inventoryInfo == null || interactive == null )
                throw new Exception("인벤토리 정보 참조가 잘못되었습니다. 확인하여 주세요.");
                        
            slotListTr = inventoryTr.GetChild(0).GetChild(0).GetChild(0);
            emptyListTr = inventoryTr.GetChild(0).GetChild(1);

            playerTr = inventoryTr.parent.parent;
            playerDropTr = playerTr;                        // 플레이어 드롭정보로 나중에 따로 지정

            prevDropEventCallerTr = itemTr.parent;          // 이전 드롭이벤트 호출자를 현재 들어있는 슬롯으로 최신화
        }      
    }

    /// <summary>
    /// 활성화 탭 변경 시 interactive 스크립트에서 호출해주기 위한 메서드입니다.<br/>
    /// 호출자가 반드시 해당 인벤토리의 InventoryInteractive 스크립트여야 합니다.<br/>
    /// 따라서 이 스크립트에서 내부 호출 시에도 인벤토리 정보가 업데이트된 상태에서 사용해야 합니다.<br/><br/>
    /// **** 인벤토리 정보가 새롭게 업데이트 된 상태이거나, 활성화 탭이 변경되었을 때 반드시 호출해야 합니다. ****<br/>
    /// </summary>
    public void UpdateActiveTabInfo(InventoryInteractive caller)
    {
        if(caller == null)
            throw new Exception("탭 변경을 시도할 수 없는 호출자입니다. 확인하여 주세요.");
        
        isActiveTabAll = caller.IsActiveTabAll;     
    }


    /// <summary>
    /// 아이템 외부 스크립트인 인벤토리의 interactive스크립트에서 활성화 탭의 변경이 시도되었을 때<br/>
    /// 해당 인자로 아이템의 활성화 탭을 설정해주기 위한 메서드입니다.<br/><br/>
    /// UpdateActiveTabInfo는 아이템이 수동으로 필요한 순간에 정보를 받아야할 때 사용하는 반면,<br/>
    /// SetActiveTabInfo는 인벤토리의 전체 아이템에 정보를 반영해야 할 때 자동으로 정보를 전달 받는다는 점입니다.
    /// </summary>
    public void SetActiveTabInfo(bool isActiveTabAll)
    {
        this.isActiveTabAll = isActiveTabAll; 
    }






    /// <summary>
    /// 아이템이 인벤토리(2D)에서 월드(3D)로 드롭이 발생할 때 해당 아이템을 이동시키기 위하여 호출해줘야 하는 메서드입니다.<br/><br/>
    /// 지정 좌표를 입력 해 준다면 해당 지정 좌표 상에 아이템을 떨어트리며,<br/>
    /// 기본 값인 null로 준다면 플레이어 고정 좌표에 드랍합니다.<br/><br/>
    /// isSetParent옵션을 지정하면 해당 지정좌표 오브젝트의 하위 자식으로 만들어줍니다.<br/><br/>
    /// *** 이전 인벤토리 정보가 있지 않으면 예외를 발생시킵니다. (월드->월드로의 드랍은 허용하지 않습니다.) *** <br/>
    /// </summary>
    public void OnItemWorldDrop( Transform worldPlaceTr=null, bool isSetParent=false )
    {
        if(isWorldPositioned)
            throw new Exception("아이템이 이미 월드에 나와있는 상태입니다. 확인하여 주세요.");   
                      
        if( worldPlaceTr == null )
            Transfer2DToWorld(null, isSetParent);           // 월드 좌표를 지정하지 않은 경우      
        else
            Transfer2DToWorld(worldPlaceTr, isSetParent);   // 월드 좌표를 지정해 준 경우
    }

    /// <summary>
    /// 아이템이 인벤토리(2D)에서 월드(3D)로 드롭이 발생할 때 해당 아이템을 이동시키기 위하여 호출해줘야 하는 메서드입니다.<br/><br/>
    /// 지정 좌표를 입력 해 준다면 해당 지정 좌표 상에 아이템을 떨어트리며,<br/>
    /// 계층 구조는 항상 최상위 계층에 배치 됩니다.<br/><br/>
    /// *** 이전 인벤토리 정보가 있지 않으면 예외를 발생시킵니다. (월드->월드로의 드랍은 허용하지 않습니다.) *** <br/>
    /// </summary>
    public void OnItemWorldDrop( Vector3 worldPos, Quaternion worldRot )
    {
        if(isWorldPositioned)
            throw new Exception("아이템이 이미 월드에 나와있는 상태입니다. 확인하여 주세요.");

        Transfer2DToWorld(worldPos, worldRot);
    }



    /// <summary>
    /// 아이템을 2D에서 3D로 지정좌표를 입력하여 전송하는 메서드입니다.<br/>
    /// null값 전달 시 플레이어의 드롭좌표에 아이템을 드롭시키고<br/>
    /// 지정좌표를 준 경우 해당 위치로 아이템을 전송시킵니다.<br/>
    /// *** 기존의 인벤토리 정보가 없는 경우 예외를 발생시킵니다. ***<br/>
    /// </summary>
    private void Transfer2DToWorld(Transform worldPlaceTr, bool isSetParent=false)
    {
        if(inventoryInfo==null)
            throw new Exception("기존의 인벤토리 정보가 없습니다. 2D->World 메서드 호출이 맞는지 확인하여 주세요.");
        
        // 이 아이템을 이전 인벤토리에서 제거합니다.
        inventoryInfo.RemoveItem(this);         

        if(worldPlaceTr==null)   // 지정좌표를 주지 않은 경우
        {
            UpdateInventoryInfo(null);                  // 인벤토리 정보를 null값으로 전달하여 최신화 합니다.
            Locate2DToWorld(playerDropTr, isSetParent); // 플레이어의 드롭위치를 기반으로, 월드로 아이템을 전송합니다.
        }
        else                                            // 지정좌표를 준 경우
        {
            UpdateInventoryInfo(null);                  // 인벤토리 정보를 null값으로 전달하여 최신화 합니다.
            Locate2DToWorld(worldPlaceTr, isSetParent); // 지정 좌표로 월드로 아이템을 전송합니다.
        }
      
        prevDropEventCallerTr = worldPlaceTr;           // 성공이므로, 이전 드랍이벤트 호출자를 최신화 합니다.
    }

    /// <summary>
    /// 아이템을 2D에서 3D로 지정좌표를 입력하여 전송하는 메서드입니다.<br/>
    /// 지정좌표를 준 경우 해당 위치로 아이템을 전송시킵니다.<br/>
    /// *** 기존의 인벤토리 정보가 없는 경우 예외를 발생시킵니다. ***<br/>
    /// </summary>
    private void Transfer2DToWorld( Vector3 worldPos, Quaternion worldRot )
    {
        if(inventoryInfo==null)
            throw new Exception("기존의 인벤토리 정보가 없습니다. 2D->World 메서드 호출이 맞는지 확인하여 주세요.");
                
        inventoryInfo.RemoveItem(this);        // 이 아이템을 이전 인벤토리에서 제거합니다.
        UpdateInventoryInfo(null);             // 인벤토리 정보를 null값으로 전달하여 최신화 합니다.
        Locate2DToWorld(worldPos, worldRot);   // 지정 좌표로 월드로 아이템을 전송합니다.        
        
        prevDropEventCallerTr = null;          // 성공이므로, 이전 드랍이벤트 호출자를 최신화 합니다.
    }







    
    /// <summary>
    /// 아이템의 슬롯 드롭이 발생할 때 아이템을 이동시키고 정보를 이전하기 위하여 호출해줘야 하는 메서드입니다.<br/>
    /// 동일 혹은 타 인벤토리의 슬롯 간 드랍 발생시 사용합니다.<br/><br/>
    /// 슬롯->같은 인벤토리 슬롯 : 실패하지 않습니다. 기존 아이템이 있다면 위치를 교환합니다.<br/>
    /// 슬롯->다른 인벤토리 슬롯 : 빈자리가 없다면 실패합니다. 기존 아이템이 있다면 실패합니다.<br/>
    /// </summary>
    /// <returns>슬롯 드롭에 성공 시 true를 실패 시 false를 반환합니다.</returns>
    public bool OnItemSlotDrop( Transform callerSlotTr )
    {
        
        // 호출 인자가 전달되지 않았는지 검사
        if( callerSlotTr==null)
            throw new Exception("슬롯의 참조가 전달되지 않았습니다. 올바른 슬롯 드랍이벤트 호출인지 확인하여 주세요.");

        bool isCallerSlot = callerSlotTr.GetComponent<SlotDrop>() != null;
        bool isPrevCallerSlot = prevDropEventCallerTr.GetComponent<SlotDrop>() != null;
        
        // 호출자가 슬롯인지 검사
        if( !isCallerSlot )
            throw new Exception("전달인자가 슬롯이 아닙니다. 올바른 슬롯 드랍이벤트 호출인지 확인하여 주세요.");
        // 이전 호출자 정보가 슬롯인지 검사
        else if( !isPrevCallerSlot )
            throw new Exception("월드->슬롯의 드랍이벤트가 발생하였습니다. 올바른 슬롯 드랍이벤트 호출인지 확인하여 주세요.");



        // 현재 슬롯 호출자와 이전 드랍이벤트 호출자가 같다면,
        if(callerSlotTr==prevDropEventCallerTr)        
        {   
            return MoveSlotInSameListSlot(callerSlotTr);            // 동일한 인벤토리의 동일한 슬롯->슬롯으로의 이동
        }
        else
        {
            // 이전 드랍이벤트 호출자와 부모가 같다면(동일한 슬롯 리스트에서의 이동이라면)
            if( callerSlotTr.parent == prevDropEventCallerTr.parent) 
                return MoveSlotInSameListSlot(callerSlotTr);       // 동일 슬롯 간 이동
            else
                return MoveSlotToAnotherListSlot(callerSlotTr);    // 타 인벤토리 슬롯으로의 이동            
        }   
    }
    












    /// <summary>
    /// 아이템을 기존 슬롯에서 다른 슬롯으로 이동시켜 주는 메서드입니다.<br/>
    /// 슬롯의 자식인덱스를 읽어 들여 아이템에 적용시켜 주고, 아이템 오브젝트의 위치를 업데이트 합니다.<br/>
    /// 만약 슬롯에 이미 다른 아이템 오브젝트가 있다면 서로의 인덱스 정보와 위치를 교환합니다.<br/>
    /// </summary>
    /// <returns>현재는 실패조건이 없으므로 true를 반환합니다. (스위칭 불가한 아이템 등이 나오면 조절합니다.)</returns>
    private bool MoveSlotInSameListSlot(Transform nextSlotTr)
    {
        int nextSlotIdx = nextSlotTr.GetSiblingIndex();     // 다음 슬롯의 인덱스 해당 슬롯의 자식넘버를 참조합니다.
        
        if( nextSlotTr.childCount==0 )
        {
            // 활성화 중인 탭에 따른 이 아이템의 슬롯 인덱스 정보를 수정합니다.
            if(isActiveTabAll)
                item.SlotIndex = nextSlotIdx;
            else
                item.SlotIndexAll = nextSlotIdx;

            // 해당 정보로 위치정보를 업데이트 합니다.
            UpdatePositionInSlotList();
        }
        else if(nextSlotTr.childCount==1)
        {             
            // 슬롯에 담겨있는 바꿀 아이템의 정보를 가져옵니다.
            ItemInfo switchItemInfo = nextSlotTr.GetChild(0).GetComponent<ItemInfo>(); 
            
            // 활성화 중인 탭에 따른 두 아이템의 인덱스 정보를 수정합니다.
            if(isActiveTabAll)
            {
                switchItemInfo.SlotIndexAll = item.SlotIndexAll;  // 바꿀아이템의 인덱스를 이 아이템의 이전 슬롯의 인덱스로 넣어줍니다.      
                item.SlotIndexAll = nextSlotIdx;                  // 이 아이템의 인덱스를 바뀔 슬롯의 위치로 수정합니다.
            }
            else
            {
                switchItemInfo.SlotIndex = item.SlotIndex;          
                item.SlotIndex = nextSlotIdx;
            }
            
            switchItemInfo.UpdatePositionInSlotList();      // 바꿀 아이템의 위치 정보를 업데이트 합니다.
            UpdatePositionInSlotList();                     // 이 아이템의 위치 정보를 업데이트 합니다.
        }
        else  // 슬롯에 자식이 2개 이상인 경우 - 예외 처리
            throw new Exception("슬롯에 자식이 2개 이상 겹쳐있습니다. 확인하여 주세요.");

        prevDropEventCallerTr = nextSlotTr;                 // 성공했다면 이전 드랍이벤트 호출자를 최신화합니다. 

        return true;    // 현재는 실패조건이 없으므로 true를 반환합니다. (스위칭 불가한 아이템 등이 나오면 조절합니다.)
    }



    
    /// <summary>
    /// 아이템을 기존 인벤토리의 슬롯에서 다른 인벤토리의 슬롯으로 이동시켜주는 메서드입니다.<br/>
    /// 해당 인벤토리에 남는 자리가 있는지 옮기기 전에 확인하여 자리가 충분하다면<br/>
    /// 이전의 인벤토리 목록에서 이 아이템을 제거하고, 옮길 인벤토리로 모든 정보를 최신화하여 줍니다.<br/>
    /// 이는 버튼 등으로 아이템을 옮겨야 할 경우가 있기 때문입니다.<br/>
    /// </summary>
    /// <returns>새로운 인벤토리 슬롯에 남는 자리가 있는경우 true를, 남는자리가 없거나 해당 슬롯에 기존의 아이템이 있다면 false를 반환</returns>
    private bool MoveSlotToAnotherListSlot(Transform nextSlotTr)
    {
        if(nextSlotTr.childCount>=1)        // 슬롯에 아이템이 담겨있다면,
        {
            UpdatePositionInSlotList();     // 원위치로 되돌리고 실패를 반환합니다.
            return false;
        }

        // 인자로 전달한 슬롯의 계층정보를 기반으로 인벤토리 참조를 설정합니다
        Transform nextInventoryTr = nextSlotTr.parent.parent.parent.parent;
        InventoryInfo nextInventoryInfo = GetComponent<InventoryInfo>();

        // 새로운 인벤토리 슬롯에 남는 자리가 있는 경우
        if( nextInventoryInfo.isSlotEnough(this) )
        {
            inventoryInfo.RemoveItem(this);                     // 이전 인벤토리에서 아이템을 제거해야 합니다.

            UpdateInventoryInfo(nextInventoryTr);               // 인벤토리 참조 정보를 호출자의 인벤토리로 업데이트 합니다.
            inventoryInfo.AddItem(this);                        // 업데이트 된 인벤토라에 아이템을 추가합니다. 
            inventoryInfo.SetItemSlotIdxBothToNearstSlot(this); // 슬롯 인덱스 정보를 가장 가까운 슬롯으로 업데이트합니다. 

            UpdatePositionInSlotList();                         // 업데이트 된 인덱스를 바탕으로 새로운 슬롯 위치로 이동시킵니다.
                                                                        
            prevDropEventCallerTr = nextSlotTr;                 // 성공했다면 이전 드랍이벤트 호출자를 최신화합니다. 
            return true;                                        // 성공을 반환합니다.
        }
        // 새로운 인벤토리 슬롯에 남는 자리가 없는 경우
        else
        {
            UpdatePositionInSlotList();     // 원위치로 되돌리고 실패를 반환합니다.
            return false;
        }
    }






    /// <summary>
    /// 아이템의 2D 모습을 중단하거나 다시 활성화시키는 메서드입니다.<br/>
    /// isWorldPositioned를 기반으로 최신화합니다. (2D로의 모습을 끄거나 킵니다)<br/><br/>
    /// ** isWorldPositioned가 변경되었을 경우 반드시 호출해야 합니다. **<br/>
    /// </summary>
    private void SwitchAppearAs2D(bool isWorldPositioned)
    {        
        // 월드 상에 아이템이 있다면, UI 이벤트를 더 이상 받지 않으며, 2D이미지를 투명처리합니다.
        itemCG.blocksRaycasts = !isWorldPositioned;
        itemCG.alpha = isWorldPositioned ? 0f:1f;
    }
           
    /// <summary>
    /// 2D와 3D 오브젝트의 부모와 자식관계를 변경하는 메서드입니다. 내부적으로 사용됩니다. <br/>
    /// isWorldPositioned를 기반으로 자동으로 최신화합니다. (2D 오브젝트로 변경하거나 3D오브젝트로 변경합니다.)<br/><br/>
    /// 전달인자로 부모로 삼을 오브젝트의 Transform을 전달해야 합니다. (2D는 슬롯정보, 3D는 월드의 위치정보를 전달합니다.) <br/>
    /// null인자 전달 시 부모가 없는 상태 즉, 계층 최상위에 배정됩니다.<br/>
    /// </summary>
    private void ChangeHierarchy( Transform parentTr=null )
    {
        if(isWorldPositioned)
        {
            itemTr.SetParent( parentTr );           // 3D오브젝트의 부모를 인벤토리에서 최상위 씬으로 변경
            itemRectTr.SetParent(itemTr);           // 2D오브젝트의 부모를 3D오브젝트로 변경   
            itemTr.gameObject.SetActive(true);      // 3D오브젝트를 활성화            
        }
        else
        {
            itemRectTr.SetParent( parentTr );           // 2D 오브젝트의 부모를 슬롯으로 설정
            itemTr.SetParent( itemRectTr );             // 3D 오브젝트의 부모를 2D 오브젝트로 설정
            itemTr.gameObject.SetActive( false );       // 3D 오브젝트 비활성화
        }
    }


    /// <summary>
    /// 아이템을 2D UI에서 3D 월드로 위치정보를 주어서 계층구조를 변경하여 이동을 해주는 메서드입니다.<br/>
    /// 상위 부모를 설정할 수 있습니다.
    /// </summary>
    private void Locate2DToWorld(Transform worldTr, bool isSetParent=false)
    {   
        // 좌표 입력이 들어오지 않은경우, 예외처리 
        if(worldTr==null)
            throw new Exception("아이템을 이동 시킬 월드 좌표를 입력해주세요.");

        // World상태 변수 활성화
        isWorldPositioned = true;  
                
        // 아이템의 2D모습을 가립니다.
        SwitchAppearAs2D(isWorldPositioned);
                    
        // 계층구조 전환                 
        if( isSetParent )              
            ChangeHierarchy(worldTr);   // 3D오브젝트의 부모를 인벤토리에서 worldTr로 변경   
        else
            ChangeHierarchy(null);      // 3D오브젝트의 부모를 최상위 씬으로 설정

        // 3D오브젝트 포지션 설정
        itemTr.position = worldTr.position;
        itemTr.rotation = worldTr.rotation;                        
    }

    /// <summary>
    /// 아이템을 2D UI에서 3D 월드로 위치정보를 주어서 계층구조를 변경하여 이동을 해주는 메서드입니다.<br/>
    /// 상위부모를 설정할 수 없습니다.
    /// </summary>
    private void Locate2DToWorld(Vector3 worldPos, Quaternion worldRot )
    {
        // World상태 변수 활성화
        isWorldPositioned = true;
                
        // 아이템의 2D모습을 가립니다.
        SwitchAppearAs2D( isWorldPositioned );

        // 계층구조 전환
        ChangeHierarchy(null);
        
        // 3D오브젝트 포지션 설정
        itemTr.position = worldPos;
        itemTr.rotation = worldRot;
    }
    




    

    /// <summary>
    /// 월드의 아이템을 습득하는 경우<br/>
    /// 즉, 월드의 아이템이 특정 인벤토리에 들어가야 하는 경우 호출해야 할 메서드입니다.<br/><br/>
    /// *** 인벤토리 정보를 전달하지 않으면 예외가 발생합니다. ***
    /// </summary>
    /// <param name="newInventoryInfo"></param>
    /// <returns>해당 인벤토리의 슬롯에 빈 자리가 없다면 false를 반환, 성공 시 true를 반환</returns>
    public bool OnItemGain(InventoryInfo newInventoryInfo)
    {        
        // 인자 미전달 시 예외처리
        if(newInventoryInfo == null)
            throw new Exception("인벤토리 정보가 전달되지 않았습니다. 확인하여 주세요.");
                
        return Locate3DToInventory(newInventoryInfo);  
    }


    /// <summary>
    /// 아이템을 3D 월드에서 슬롯으로 계층구조를 변경하여 이동을 해주는 메서드입니다.
    /// </summary>
    /// <returns>이동에 성공한 경우는 true를 슬롯이 꽉차 이동할 수 없다면 false를 반환합니다.</returns>
    public bool Locate3DToInventory(InventoryInfo inventoryInfo)
    {
        // 해당 종류의 아이템이 들어갈 슬롯이 없다면 실패를 반환합니다.
        if( !inventoryInfo.isSlotEnough(item.Type) )
            return false;

        isWorldPositioned = false;              // 월드위치 상태여부를 비활성화 합니다.
        
        // 아이템의 2D모습을 활성화 합니다.
        SwitchAppearAs2D( isWorldPositioned );
                
        // 계층구조 전환
        ChangeHierarchy(null); 



        // 인벤토리 정보를 인자로 전달받은 새로운 인벤토리로 업데이트 합니다.
        UpdateInventoryInfo(inventoryInfo.transform);

        // 해당 인벤토리의 활성화 탭 정보를 최신화 합니다.
        UpdateActiveTabInfo(interactive);



        // 아이템의 슬롯 인덱스 정보를 가장 가까운 슬롯으로 입력합니다.        
        inventoryInfo.SetItemSlotIdxBothToNearstSlot(this);
        
        // 아이템을 새로운 인벤토리에 집어넣습니다.
        inventoryInfo.AddItem(this);



        // 아이템의 위치정보를 반영합니다.
        UpdatePositionInSlotList();
        
        return true;
    }






}