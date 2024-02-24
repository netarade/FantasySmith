using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.EventSystems; // 마우스 클릭, 드래그 드랍, 이벤트 관련 로직
using UnityEngine.UI;


/* [작업 사항]
 * <v1.0 - 2023_1103_최원준>
 * 1- 초기 로직 작성
 * 
 * <v2.0 - 2023_1005_최원준>
 * 1- 드래그 시 이미지가 보이지 않는 현상 - 캔버스 우선순위 문제 해결
 * 아이템의 부모 계층을 일시적으로 하위로 두게 만듬.
 * 
 * 2- 계층을 하위로 두었을 때 드랍이벤트 발생하지 않는 문제 해결
 * 아이템의 rayCastTarget을 드래그 시작하면 off시켜서
 * 아이템 자체의 드랍이벤트를 막아야 뒤에 있는 슬롯에게로 연결될 수 있음.
 * 
 * <v3.0 - 2023_1224_최원준>
 * 1- dragginItem을 draggingObj로 이름변경
 * 
 * <v4.0 - 2023_1227_최원준>
 * 1- 아이템 프리팹 계층구조 변경(Transform 컴포넌트 최상위, RectTransform컴포넌트 하위자식)으로 인해
 * itemTr의 이름을 itemRectTr로 변경, dragginObj를 draggingObj2D로 변경하고 참조 조정
 * 
 * <v4.1 - 2023_1228_최원준>
 * 1- 아이템 프리팹 계층구조 변경(3D오브젝트, 2D오브젝트 전환구조)으로 인해
 * 코드를 2D 트랜스폼기준으로 다시 수정
 * 
 * 
 * <v4.2 - 2023_1229_최원준>
 * 1- 클래스 및 파일명변경 Drag->ItemDrag
 * 
 * <v5.0 - 2023_1230_최원준>
 * 1- prevParentTrByDrag를 prevSlotTr로 변수명 변경 및 static변수에서 일반변수로 수정
 * 
 * 2- static 변수인 draggingObj2D, draggingObj3D 삭제 
 * - Drop에서 사용하고 있었으나 멀티 유저에서 동시참조가 발생할것이기 때문에 Drop에서 발생한 eventData를 통해 받도록 변경
 * 
 * 3- inventoryTr을 삭제하고, prevSlotListTr을 선언
 * 
 * 4- OnEndDrag 메서드 간소화
 * 
 * <v6.0 - 2024_0107_최원준>
 * 1- 아이템 드래그 방식에서 셀렉트 방식으로 변경하였으며, 그에따라 파일명을 ItemDrag에서 ItemSelect로 변경하고, 스크립트를 대폭수정하였습니다.
 * 
 * 2- 변수명 변경
 * prevSlotTr -> prevParentTr 
 * prevSlotListTr -> movingParentTr
 * 
 * 3- 하나의 아이템만 선택하기 위하여 isMyItemSelecting상태변수와, interactive클래스에 isItemSelecting 공유 상태 변수를 두었으며,
 * OnSelect의 중복호출을 방지하기 위하여 isFirstSelectDelay변수에 DelayTime 두어서 활성화 시기를 늦추고, 
 * OnUpdateSelected와 OnSelect의 중복호출을 방지하기 위하여 isMyItemSelecting에 DelayTime을 두어서 비활성화시기를 늦추었음.
 * 
 * => 
 * 서로 위치 스왑 시 동시 Select및 UpdateSelected 호출을 방지해야 하며,
 * OnUpdateSelected에서 클릭버튼을 누르면 반드시 Select를 종료시키고 EventSystem의 Select상태를 null로 만들어야
 * 다른 아이템의 Select를 발동시킬 수 있다.
 * (이는 스왑 시 들고 있는 같은 아이템을 한 번더 누르게 되는데 이것이 다시 셀렉트로 이어지기 때문)
 * 
 * <v6.1 - 2024_0112_최원준>
 * 1- OnSelectUpdated에 있는 셀렉트가 종료된 경우에 처리하던 로직을 Deselect로 옮김.
 * 
 * <v6.2 - 2024_0114_최원준>
 * 1- gRaycaster를 삭제하고 읽기전용 InventoryInfo리스트 변수인 clientList를 선언하여
 * 셀렉팅이 끝날 때 해당 clieentList에 있는 모든 그래픽레이캐스터의 레이캐스팅을 호출하도록 하였음
 * 
 * 2- isInventoryConnect변수 선언하여 셀렉팅할때마다 확인하여 
 * 연결 상태일 때 슬롯태그가 검출되지 않는다면 아이템 월드 드롭을 방지
 * 
 * 3- IEnumerator 주석추가 및 OnBeginDrag 주석 삭제
 * 
 * <v7.0 - 2024_0117_최원준>
 * 1- OnDeselect에서 주석처리되어있던 디버그용 코드를 PrintDebugInfo메서드를 만들어 넣어두었음
 * 
 * 2- DummyItemSelect스크립트 파일의 상속과 오버라이딩을 위해 protected 및 virtual 처리
 * 
 * 3- 내부 코드를 기능 단위별로 메서드화하여 상속 후 재사용성을 도모
 * 
 * 4- 변수명 prevParentTr을 prevParentSlotTr로 변경, movingParentTr을 selectingParentTr로 변경
 * 
 * <v7.1 - 2024_0118_최원준>
 * 1- DropItemWithRaycastResults메서드에서  
 * 슬롯의 계층 상위 부모 인벤토리가 퀵슬롯이라면 퀵슬롯의 드롭메서드까지 호출해주도록 변경
 * 
 * <v7.2 - 2024_0122_최원준>
 * 1- 그래픽 레이캐스팅을 ItemSelect스크립트에서 직접하는 것이 아니라 아이템이 속한 InventoryInfo를 매번 참조하여 캐스팅하도록 변경
 * 
 * a- 그래픽레이캐스팅 관련 변수를 삭제
 * 관련 메서드 (RaycastAllToConnectedInventory, PrintDebugInfo) 및 
 * 관련 변수(clientList, raycastResults, isInventoryConnect 등)을 삭제 
 * 
 * b- DropItemWithRaycastResults메서드 DropItemWithRaycast으로 이름 변경
 * 내부에 inventoryInfo의 레이캐스팅을 호출하여 결과를 받도록 코드변경
 * 
 * <v7.3 - 2024_0123_최원준>
 * 1- FinishSelecting 매개변수 없는 오버로딩 메서드 추가선언
 * ItemInfo에서 호출하여 슬롯 드롭시 강제로 셀렉팅 종료를 하기 위함 
 * 
 * <v7.4 - 2024_0123_최원준>
 * 1- 타 인벤토리 이동 시 셀렉팅을 막기 위해 슬롯 드랍이후 
 * itemInfo의 최신 inventoryInfo와 interactive에 접근하여 중간에 상태를 반영시키고
 * SelectDoneDelayTime 메서드에서도 과거 인벤토리와 최신 인벤토리의 상태를 동시에 해제하도록 변경
 * 
 * <v7.5 - 2024_0126_최원준>
 * 1- 퀵슬롯 드롭에 실패한 경우 원위치로 돌려주는 코드 추가
 * 
 * <v7.6 - 2024_0130_최원준>
 * 1- InitOnSelect메서드에서 퀵슬롯에게 셀렉팅이 일어남을 알리는 OnQuickSlotSlect 메서드를 호출
 * 
 * 
 * 
 * <v8.0 - 2024_0209_최원준>
 * 1- 주석에 몇가지 설명 누락된 부분있어서 주석에 추가
 * a- v7.3에서 FinishSelecting 디폴트 매개변수 메서드를 추가 선언했다고 했는데 해당 스크립트에서 보이지 않음.
 * b- itemCG의 레이캐스팅을 차단하는 이유는 아이템 드롭 하자마자 버튼 이벤트가 발생하여 다시 셀렉팅이 일어나기 때문 (중복셀렉트 방지)
 * c- 스위칭 할 아이템의 inventoryCG를 차단하는 이유
 * 
 * 2- DropItemWithRaycast메서드 내부에 QuestCheck 및 Craft_UIManager 메서드 호출 코드 null검사 구문 추가
 * 
 * 3- interactive변수명을 interactiveBeforeDrop으로 변경
 * 이유는 드롭 이전의 인벤토리의 interactive의 상태를 변경시키는데, 드롭이 성공한 후 인벤토리가 바뀌므로 상태를 되돌려 놓아야 함.
 * 
 * 4- InitOnSelect메서드 내부의 interactiveBeforeDrop의 참조값 잡는 부분을 새롭게 찾지 않고 itemInfo의 기존 참조값을 받도록 수정
 * 
 * <v8.1 - 2024_0216_최원준>
 * 1- 변수명 isFirstSelectDelay을 bInstantFinishDelay로 변경, 메서드명 FirstSelectDelayTime을 InstantSelectFinishDelayTime로 변경
 * 
 * <v8.2 - 2024_0221_최원준>
 * 1- 아이템의 재선택을 가능하게 만들어주는 isReselect변수를 추가하고, 
 * ReselectUntilDeselect메서드 호출을 통해 외부에서 플래그를 활성화 시키면
 * 드롭이 이루어지고 난 이후 Deselect가 마무리 될때 SelectDoneDelayTime 코루틴 호출 시 마지막에 다시 셀렉팅을 강제로 진행하게 해주었음
 * 
 * 2- DropItemWithRaycast메서드에서 레이캐스팅 결과를 foreach문으로 돌리면서 태그가 슬롯이 아닐때마다 UpdatePositionInfo를 호출하던 부분을 수정
 * => 슬롯 태그가 하나라도 검출된다면 UpdatePositionInfo를 돌리지 말아야 함 (내부 메서드 호출로 인해 자체적으로 돌릴 것이므로) 
 * 
 * <8.3 - 2024_0222_최원준>
 * 1- OnSelectFailCondition메서드에서 실패조건에 IsItemSelecting과 isMyItemSelecting을 && 조건으로 처리하던 점을 수정
 * isMyItemSelecting조건 삭제
 * => 이유는 자기 아이템 셀렉팅이 중요한 것이 아니라, 
 * 인벤토리 전체에서 셀렉팅이 이루어지고 있다면 다른 아이템 셀렉팅을 못해야 하므로
 * 
 * (이슈)
 * 아이템의 버튼이벤트가 발생하지 못하도록 레이캐스트블록만 차단하면 따로 아이템과 인벤토리 셀렉팅 상태를 관리할 필요가 없음.
 * 
 * 
 * <v8.4 - 2024_0223_최원준>
 * 1- 셀렉팅 속성관련 코드를 모두 제거하였음.
 * 더 이상 셀렉팅 상태관리를 할 필요없이 itemCG의 blocksRaycasts만 해제하면 셀렉팅 할 수 없기 때문
 * 
 * a- OnSelectFailCondition및 OnUpdateSelectedFailCondition 메서드를 삭제
 * b- 타 인벤토리 드롭시 상태전이 하는 코드를 제거
 * c- isMyItemSelecting변수 및 interactiveBeforeDrop 참조변수 삭제
 * 
 * 2- IEnumerator를 모두 내부 메서드로 만들고, WaitForSeconds를 캐싱변수 처리
 * 
 * 3- SelectPreventTemporary메서드를 추가
 * 아이템 스위칭 시 상대 아이템의 셀렉팅을 일시적으로 막을 수 있게 메서드화하였음
 * 
 *  
 * 
 * (이슈)
 * 1- 아이템의 버튼 셀렉팅을 방지하고자 할 때
 * itemCG의 blocksRaycasts 혹은 interactable 둘중 하나만 막아도 되지만,
 * interactable은 버튼의 하이라이팅을 차단하는 효과를,
 * blocksRaycasts는 이미지의 포인터 이벤트를 차단하는 효과를 추가적으로 가질 수 있음.
 * => 따라서 SelectPreventTemporary메서드에서는 드롭시 상대 아이템의 포인터 이벤트를 그대로 살리기 위해
 * interactable만 일시적으로 차단하고, 원본아이템의 경우는 블록레이캐스트 차단을 통해 포인터 이벤트를 셀렉팅하는 도중 막아주는것이 낫다고 판단
 * 
 * <v8.5 - 2024_0223_최원준>
 * 
 * 1- DropItemWithRaycast메서드에서 한번이라도 슬롯 검출이 되었다면 break문을 통해 빠져나가는 코드를 추가하였음.
 * => (이슈) 동일 캔버스내에 있는 인벤토리가 등록되어있는 경우 (퀵슬롯) 동일 캔버스 레이캐스팅이 중복해서 일어나게 됨.
 * 
 * 2- SelectPreventTemporary메서드에서 상대 아이템 캔버스그룹의 interactable과 raycastsBlocks을 동시에 막도록 설정
 * => (이슈) 동일한 아이템을 중첩시킬려고 할 때, 유니티 자체적인 특성으로 캔버스 그룹 속성 중 한쪽만 막으면 
 * 상대 아이템 셀렉팅이 일어났다가 취소되는 현상이 생기는 것을 발견, 타 아이템은 셀렉팅이 일어나지 않음 
 * 
 * 3.
 * (이슈) 동일 이름의 잡화 아이템을 빠르게 중첩 및 스왑하다보면 동시성 이슈가 발생해서,
 * 상대 아이템의 셀렉팅을 캔버스 그룹을 통해 막는 코드를 넣더라도, 
 * 중첩할 대상 아이템의 셀렉팅이 일어난 이후 자동으로 셀렉팅 종료를 유니티에서 실행하게되고, 
 * 종료시 bInstantFinishDelay를 false로 넣더라도, 시작 상태의 코루틴이 돌아서 true 인상태로 존재하게 됨으로서,
 * 다음 셀렉팅이 안되는 문제 발생
 * => 종료 코루틴이 돌 때 bInstantFinishDelay를 false로 초기화 해놓는 코드를 추가해야 함.
 * 
 * 4- 슬롯 태그 검출 시 퀵슬롯인지 먼저 검사해서 OnQuickSlotDrop조건 검사후 OnItemSlotDrop을 실행하던 부분을 OnItemSlotDrop을 바로 실행하는 것으로 변경
 * => 이유는 현스크립트에서 스왑시 자기 인벤토리의 아이템이 퀵슬롯인지를 검사할 수 없기 때문에
 * OnItemSlotDrop내부에서 자체적으로 검사하는 것으로 변경
 * 
 * (이슈)
 * 1- 퀵슬롯에서 아이템 스왑을 진행할 때, 상대 아이템의 셀렉팅을 막기전에 셀렉팅이 먼저 진행되는 경우가 생겨서
 * 드롭실패시 원래 아이템은 자리 돌아가지만, 상대 무기가 셀렉팅되버리는 경우가 있음.
 * => (해결완료) OnItemSlotDrop에서 SelectPreventTemporary메서드를 호출하지만, 조건 분기에 따라 실행하지 못하는 경우가 생겼기 떄문
 * 
 * 
 */


/// <summary>
/// Drag스크립트는 ItemInfo 스크립트와 함께 아이템 2D 오브젝트가 컴포넌트로 가지고 있어야 합니다.<br/>
/// 아이템 오브젝트의 드래그를 가능하게 해줍니다.
/// </summary>
public class ItemSelect : MonoBehaviour
    , IUpdateSelectedHandler, ISelectHandler, IDeselectHandler      // 셀렉트 방식
{
    protected RectTransform itemRectTr;     // 자신 transform 캐싱
    protected CanvasGroup itemCG;           // 자신의 캔버스 그룹 캐싱
    protected ItemInfo itemInfo;            // 자신의 ItemInfo 캐싱
    
    protected Transform prevParentSlotTr;   // 아이템을 옮기기 이전의 부모 (슬롯)
    protected Transform selectingParentTr;  // 아이템을 옮기는 도중의 부모 (캔버스)

    protected Vector3 moveVecToCenter;      // 아이템 셀렉팅을 시작할 때 인벤토리를 원점으로 되돌릴 수 있는 벡터
        

    protected InventoryInfo inventoryInfo;                // 아이템이 속한 인벤토리 참조 (동적으로 변경)
    
    protected bool bInstantFinishDelay = false;           // 처음 셀렉팅 후 딜레이 시간이 지났는지 여부
    protected Button itemSelectBtn;                       // 버튼의 셀렉트를 해제하기 위한 참조
    protected string strItemDropSpace = "ItemDropSpace";  // 아이템을 드롭할 수 있는 태그 설정(슬롯의 태그)

    protected bool isReselect = false;                    // 아이템의 드롭이 이루어지고 난 이후 재선택을 가능하게 만드는 상태변수

    WaitForSeconds selectDelayTime = new WaitForSeconds(0.1f); // 아이템 재셀렉팅 방지를 위한 딜레이 시간

    bool isOnSelectDone = false;                          // OnSelect와 OnUpdateSelected의 호출순서 고정을 위한 상태변수


    // +++ 2024_0128_협업 코드 (신혜성)
    public QuestCheck check;
    public Craft_UIManager craft_UIManager;


    protected virtual void Start()
    {
        itemRectTr = GetComponent<RectTransform>();     
        itemCG = GetComponent<CanvasGroup>();     
        itemInfo = GetComponent<ItemInfo>();
        itemSelectBtn = GetComponent<Button>();  

        // +++ 2024_0128_협업 코드 (신혜성)
        check = GameObject.Find("Canvas-Quest").GetComponent<QuestCheck>();
        craft_UIManager = GameObject.Find("CraftingSystem").GetComponent<Craft_UIManager>();
    }



    

    

    // 셀렉트 시작 시
    public virtual void OnSelect( BaseEventData eventData )
    {                        
        InitOnSelect();                 // 셀렉팅 실행 전 값의 초기화를 진행합니다.
    }
           


    // 셀렉트 진행 중의 처리
    public virtual void OnUpdateSelected( BaseEventData eventData )
    {
        MatchItemPositionWithCursor();          // 마우스 위치를 커서클릭 지점으로 맞춥니다.

        // 이미 선택되어있는 상태에서 한번 더 선택을 못하도록 종료시킵니다. (셀렉팅이 이루어지고 있는 상태에서 마우스 클릭 감지 코드 검사문으로 인해 바로 셀렉팅이 종료되는 것을 방지합니다.)
        if( Input.GetMouseButton( 0 ) && bInstantFinishDelay )
            FinishSelecting(eventData);
    }

    

    // 셀렉트 종료 시
    public virtual void OnDeselect( BaseEventData eventData )
    {
        InitFirstOnDeselect();              // 셀릭팅이 끝난 후 초기화를 진행합니다.

        DropItemWithRaycast();       // 레이캐스팅된 결과를 토대로 아이템을 이동시킵니다.      

        InitLastOnDeselect();               // 레이캐스팅이 끝난 후 초기화를 진행합니다.
    }









    


    /// <summary>
    /// 아이템 위치를 마우스가 클릭한 지점 맞춰주는 메서드입니다.<br/>
    /// 원점 앵커를 다시 이전 원점위치로 돌려보내 클릭지점을 맞추는 기능을 가지고 있습니다.
    /// </summary>
    protected void MatchItemPositionWithCursor()
    {
        // 아이템 위치를 마우스 위치로 해버리면 원점이 마우스 위치로 끌려오므로,
        // 다시 원점위치로 보내서 해당 위치를 기준으로 움직이도록 해줍니다.
        itemRectTr.position = Input.mousePosition + moveVecToCenter;               
    }

    
    /// <summary>
    /// 아이템 셀렉팅을 강제로 종료합니다. 
    /// </summary>
    protected void FinishSelecting(BaseEventData eventData)
    {
        itemSelectBtn.OnDeselect( eventData );              // 버튼을 Deselect상태로 만듭니다.
        EventSystem.current.SetSelectedGameObject( null );  // 이벤트 시스템의 셀렉트 상태를 null로 만듭니다.
    }






    
    /// <summary>
    /// OnSelect에서 아이템과 관련 된 속성들을 초기화하기 위해 호출해주는 메서드입니다.
    /// </summary>
    protected void InitOnSelect()
    {
        // 중복셀렉트 방지 활성화 (드롭 하자마자 셀렉트가 일어나지 않게 드롭 후 딜레이를 줘서 킵니다.)
        itemCG.blocksRaycasts=false; 

        // 재선택 플래그를 비활성화 합니다.
        isReselect = false;                            
        
        // 아이템이 속한 인벤토리 정보를 새롭게 참조합니다. (변경가능성이 있으므로)
        inventoryInfo = itemInfo.InventoryInfo;

        
        prevParentSlotTr=itemRectTr.parent;                 // 이전 부모 위치 (슬롯리스트와 개별 슬롯)을 저장합니다.
        selectingParentTr=inventoryInfo.transform.parent;   // 인벤토리의 부모 캔버스 참조

        itemRectTr.SetParent( selectingParentTr );          // 부모를 일시적으로 인벤토리의 부모인 캔버스로 잡아서 이미지 우선순위를 높입니다.     
        

        // 원점 이동 벡터를 구합니다.
        // (현재 인벤토리 원점 위치-마우스 이벤트가 발생한 위치 => 마우스 이벤트 위치에서 인벤토리 원점으로 이동할 수 있는 이동벡터)
        moveVecToCenter=itemRectTr.position-Input.mousePosition;

        
               



        // OnUpdateSelected 동시 호출을 방지하기 위해 딜레이를 줘서 상태변수를 활성화시킵니다.
        StartCoroutine( InstantSelectFinishDelayTime() );

        // 처음 셀렉트하기 위해 클릭할 때 바로 셀렉트 종료의 호출이 일어나는 것을 막기 위한 딜레이시간 내부 메서드
        IEnumerator InstantSelectFinishDelayTime()
        {
            yield return selectDelayTime;
            bInstantFinishDelay = true;
        }
    }

    




    /// <summary>
    /// OnDeselect 동작이 일어날 때 최초로 초기화를 해줘야 하는 메서드입니다.
    /// </summary>
    protected void InitFirstOnDeselect()
    {
        bInstantFinishDelay=false;       // 처음 클릭 상태를 비활성화 합니다.
        //Debug.Log( "transformID: " + transform.GetInstanceID() + "클릭상태 초기화 완료");
    }

    
    /// <summary>
    /// OnDeselect 동작이 일어날 때 (중간 동작 이후) 마지막에 초기화 해 줘야 하는 메서드입니다.<br/>
    /// OnDeselect에서 중간 레이캐스팅이 끝난 후 호출해주고 있습니다.
    /// </summary>
    protected void InitLastOnDeselect()
    {
        // 아이템 셀렉팅 상태를 딜레이 시간을 줘서 비활성화시킵니다.
        StartCoroutine( SelectDoneDelayTime() );


        // 아이템의 셀렉팅이 끝나고 난 이후 마지막으로 초기화해줘야 할 속성들을 모아놓은 메서드
        IEnumerator SelectDoneDelayTime()
        {
            yield return selectDelayTime;
        
            // 인벤토리 변경 전 상태를 초기화합니다.
            // 중복셀렉트 방지 해제(드롭 하자마자 셀렉트가 일어나지 않게 드롭 후 딜레이를 줘서 킵니다.)
            itemCG.blocksRaycasts=true; 

            bInstantFinishDelay = false;     // 처음 클릭 상태를 비활성화 합니다.


            // 재선택 플래그가 활성화되어 있다면, 아이템의 셀렉팅을 처음부터 다시 시작합니다.
            if( isReselect )
            {
                //Debug.Log( "transformID: " + transform.GetInstanceID() + "리셀렉팅합니다.");
                EventSystem.current.SetSelectedGameObject( this.gameObject );
            }

        }
    }

    
    
    







    /// <summary>
    /// 레이캐스팅 결과의 조건에 따라 아이템에게 일어나는 동작을 모아둔 메서드입니다. 
    /// </summary>
    protected void DropItemWithRaycast()
    {        
        // 인벤토리에 그래픽 레이케스팅을 요청하고 결과를 반환받습니다.
        IReadOnlyList<RaycastResult> raycastResults = inventoryInfo.RaycastAllToConnectedInventory();

        // 레이캐스팅에 성공한 경우(검출한 오브젝트가 있는 경우)
        if( raycastResults.Count>0 )
        {
            // 레이캐스팅 결과에 대한 슬롯 탐색여부를 초기화합니다.
            bool isFoundSlot = false;

            // 모든 레이캐스팅 결과를 하나씩 열어봅니다.
            foreach( RaycastResult raycastResult in raycastResults )
            {
                Transform resultTr = raycastResult.gameObject.transform;

                // 검출한 오브젝트의 태그가 슬롯이라면,
                if( resultTr.tag==strItemDropSpace )
                {
                    // 슬롯 탐색 여부를 활성화합니다.
                    isFoundSlot = true;
                    
                    // 아이템의 슬롯 드랍을 실행합니다.
                    itemInfo.OnItemSlotDrop( resultTr );
                    

                    // 슬롯을 한번이라도 만나면 반복문을 종료합니다.
                    break;      
                }
            }

            
            // 검출한 오브젝트의 태그에 슬롯이 없다면(슬롯탐색 여부가 비활성화상태라면), 다시 원위치로 돌려줍니다.
            if(!isFoundSlot)                
                itemInfo.UpdatePositionInfo();
        }
        // 레이캐스팅의 검출이 없으면서, 부모의 이동이 발생하지 않았다면,(슬롯의 드랍에 실패했다면)
        else if( raycastResults.Count==0 && itemRectTr.parent==selectingParentTr )
        {
            //print( "[검출되지 않았습니다]" );

            // 현재 인벤토리가 연결상태라면
            if( inventoryInfo.IsConnect )
                itemInfo.UpdatePositionInfo();    // 원위치로 돌립니다.
            // 현재 인벤토리가 연결상태가 아니라면
            else
            {
                itemInfo.OnItemWorldDrop();       // 인벤토리에서 아이템 드롭을 허용합니다.


                // +++ 2024_0128_협업 코드 (신혜성)
                if(check!=null)
                    check.WorldQuestCheck();          // 드랍된 아이템이 발생할 경우 퀘스트 체크 최신화
                if(craft_UIManager != null)
                    craft_UIManager.CheckTab();       // 드랍된 아이템이 발생할 경우 제작에 필요한 재료 목록을 최신화 
            }
        }
        else
            throw new System.Exception( "드랍 이상이 발생하였습니다. 확인하여 주세요." );
    }








    
    /// <summary>
    /// 아이템의 재선택을 강제로 활성화하는 메서드입니다.<br/>
    /// 아이템의 Deselect가 완전히 끝나기 전까지 호출되면 마지막에 셀렉팅을 다시 시작해줍니다.
    /// </summary>
    public void ReselectUntilDeselect()
    {
        isReselect = true;
    }


    /// <summary>
    /// 아이템의 셀렉트를 일시적으로 방지해주기 위한 메서드입니다.<br/>
    /// 아이템의 상호작용 속성을 일시적으로 비활성화 후 활성화 시켜줍니다.<br/><br/>
    /// 아이템 스위칭 시 상대 아이템의 셀렉팅을 막기위한 용도로 사용됩니다.<br/>
    /// </summary>
    public void SelectPreventTemporary()
    {
        StartCoroutine( SelectPreventDelayTime() );


        //아이템의 상호작용 속성을 일시적 해제 이후 딜레이 시간 이후 원상복구해주는 메서드
        IEnumerator SelectPreventDelayTime()
        {
            // itemCG의 (착용중인 아이템의 경우 더미의) 상호작용을 일시적으로 비활성화
            if( itemInfo.IsEquip )
            {
                itemInfo.DummyInfo.DummyImg.raycastTarget = false;
                itemInfo.DummyInfo.DummyBtn.interactable = false;
            }
            else
            {
                itemCG.blocksRaycasts=false;
                itemCG.interactable=false;
            }
            yield return selectDelayTime;

            
            // itemCG의 (착용중인 아이템의 경우 더미의) 상호작용을 다시 활성화
            if( itemInfo.IsEquip )
            {
                itemInfo.DummyInfo.DummyImg.raycastTarget = true;
                itemInfo.DummyInfo.DummyBtn.interactable = true;
            }
            else
            {
                itemCG.blocksRaycasts = true;
                itemCG.interactable = true;
            }
        }
    }





}
