using System.Collections;
using System.Collections.Generic;
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
 * <v7.5 - 2024_0123_최원준>
 * 1- 
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
    protected InventoryInteractive interactive;           // 전체 아이템 셀렉팅 여부 참조를 위한 인터렉티브 스크립트 참조
    protected bool isMyItemSelecting = false;             // 현재 아이템이 선택 중인지 여부
    protected bool isFirstSelectDelay = false;            // 처음 셀렉팅 후 딜레이 시간이 지났는지 여부
    protected Button itemSelectBtn;                       // 버튼의 셀렉트를 해제하기 위한 참조
    protected string strItemDropSpace = "ItemDropSpace";  // 아이템을 드롭할 수 있는 태그 설정(슬롯의 태그)


    protected virtual void Start()
    {
        itemRectTr = GetComponent<RectTransform>();     
        itemCG = GetComponent<CanvasGroup>();     
        itemInfo = GetComponent<ItemInfo>();
        itemSelectBtn = GetComponent<Button>();  
    }



    

    // 셀렉트 진행 중의 처리
    public virtual void OnUpdateSelected( BaseEventData eventData )
    {        
        if( OnUpdateSelectedFailCondition() )   // 실패조건이 성사되면 실행하지 않습니다.
            return;
        
        MatchItemPositionWithCursor();          // 마우스 위치를 커서클릭 지점으로 맞춥니다.


        // 이미 선택되어있는 상태에서 한번 더 선택을 못하도록 종료시킵니다.
        if( Input.GetMouseButton( 0 ) && isFirstSelectDelay )
            FinishSelecting(eventData);
    }

    

    // 셀렉트 시작 시
    public virtual void OnSelect( BaseEventData eventData )
    {        
        if( OnSelectFailCondition() )   // 실패조건이 성사되면 실행하지 않습니다.
            return;
                
        InitOnSelect();                 // 셀렉팅 실행 전 값의 초기화를 진행합니다.    
    }
           
    

    // 셀렉트 종료 시
    public virtual void OnDeselect( BaseEventData eventData )
    {
        InitFirstOnDeselect();              // 셀릭팅이 끝난 후 초기화를 진행합니다.

        DropItemWithRaycast();       // 레이캐스팅된 결과를 토대로 아이템을 이동시킵니다.      

        InitLastOnDeselect();               // 레이캐스팅이 끝난 후 초기화를 진행합니다.
    }









    /// <summary>
    /// OnUpdateSelected 콜백 이벤트가 실행되지 않기 위한 조건을 반환합니다.
    /// </summary>
    /// <returns>실패조건이 충족되었다면 true, 실행해야 한다면 false를 반환</returns>
    protected bool OnUpdateSelectedFailCondition()
    {
        // 내 아이템 선택상태가 아니라면 실행하지 않습니다. (아이템 1개만 실행합니다.)
        if( !isMyItemSelecting )
            return true;
        else
            return false;
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






    protected bool OnSelectFailCondition()
    {
        if( itemInfo==null )
            Debug.Log( $"아이템 정보가 없습니다.\n본인:{gameObject.name} 부모:{transform.parent.name}" );
        else if(itemInfo.InventoryInfo==null)
            Debug.Log( $"인벤토리 정보가 없습니다.\n본인:{gameObject.name} 부모:{transform.parent.name}" );
        else
            Debug.Log( $"다른 오류.\n본인:{gameObject.name} 부모:{transform.parent.name}" );



        // Select를 시작하면 현재 아이템의 인벤토리 정보를 최신화하여 가져옵니다. 
        interactive = itemInfo.InventoryInfo.gameObject.GetComponent<InventoryInteractive>();

        // 아이템 셀렉팅이 하나라도 활성화되어 있다면 다른 아이템의 셀렉팅을 완전히 차단합니다.
        // 자신의 셀렉팅이 진행중이거나, 셀렉팅이 완료되지 않았다면 실행하지 않습니다.
        if( interactive.IsItemSelecting && isMyItemSelecting )
            return true;
        else 
            return false;
    }


    protected void InitOnSelect()
    {
        interactive.IsItemSelecting=true;     // 전체 아이템에 적용하는 선택 상태를 활성화 합니다.
        isMyItemSelecting=true;               // 내 아이템 선택 진행 상태를 활성화 합니다.

        // OnUpdateSelected 동시 호출을 방지하기 위해 딜레이를 줘서 상태변수를 활성화시킵니다.
        StartCoroutine( FirstSelectDelayTime( 0.1f ) );


        // 이전 부모 위치 (슬롯리스트와 개별 슬롯)을 저장합니다.
        prevParentSlotTr=itemRectTr.parent;
        selectingParentTr=interactive.gameObject.transform.parent; // 인벤토리의 부모 캔버스 참조

        itemRectTr.SetParent( selectingParentTr );              // 부모를 일시적으로 인벤토리의 부모인 캔버스로 잡아서 이미지 우선순위를 높입니다.
        itemCG.blocksRaycasts=false;                            // 드래그 이벤트 이외에는 받지 않습니다.

        // 원점 이동 벡터를 구합니다.
        // (현재 인벤토리 원점 위치-마우스 이벤트가 발생한 위치 => 마우스 이벤트 위치에서 인벤토리 원점으로 이동할 수 있는 이동벡터)
        moveVecToCenter=itemRectTr.position-Input.mousePosition;

        
        // 아이템이 속한 인벤토리 정보를 새롭게 참조합니다. (변경가능성이 있으므로)
        inventoryInfo = itemInfo.InventoryInfo;
    }



    /// <summary>
    /// OnDeselect 동작이 일어날 때 최초로 초기화를 해줘야 하는 메서드입니다.
    /// </summary>
    protected void InitFirstOnDeselect()
    {
        isMyItemSelecting=false;        // 내 아이템 선택 상태를 바로 비활성화합니다.
        isFirstSelectDelay=false;       // 처음 클릭 상태를 비활성화 합니다.
    }

    
    /// <summary>
    /// OnDeselect 동작이 일어날 때 (중간 동작 이후) 마지막에 초기화 해 줘야 하는 메서드입니다.<br/>
    /// OnDeselect에서 중간 레이캐스팅이 끝난 후 호출해주고 있습니다.
    /// </summary>
    protected void InitLastOnDeselect()
    {        
        // 아이템 셀렉팅 상태를 딜레이 시간을 줘서 비활성화시킵니다.
        StartCoroutine( SelectDoneDelayTime( 0.15f ) );
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
            // 모든 레이캐스팅 결과를 하나씩 열어봅니다.
            foreach( RaycastResult raycastResult in raycastResults )
            {
                Transform resultTr = raycastResult.gameObject.transform;

                // 검출한 오브젝트의 태그가 슬롯이라면,
                if( resultTr.tag==strItemDropSpace )
                {
                    // 슬롯의 계층 상위 부모 인벤토리가 퀵슬롯이라면 퀵슬롯 드롭메서드 또한 호출해줍니다.
                    QuickSlot quickSlot = resultTr.parent.parent.parent.parent.GetComponent<QuickSlot>();

                    // 퀵슬롯이라면 퀵슬롯에 드롭이 가능한지 확인후 드롭을 진행합니다.
                    if( quickSlot!=null )
                    {
                        if( quickSlot.OnQuickSlotDrop( itemInfo, resultTr ) )
                        {
                            itemInfo.OnItemSlotDrop( resultTr );

                            // 타 인벤토리 전이 시 아이템 셀렉팅 상태가 끝나지 않았기 때문에
                            // 최신 인벤토리 정보의 상태를 반영합니다.
                            itemInfo.InventoryInteractive.IsItemSelecting = true;
                            itemInfo.InventoryInfo.inventoryCG.blocksRaycasts = false;
                        }

                    }
                    // 퀵슬롯이 아닌 경우 바로 드롭을 진행합니다.
                    else
                    {
                        itemInfo.OnItemSlotDrop( resultTr );
                        
                        // 타 인벤토리 전이 시 아이템 셀렉팅 상태가 끝나지 않았기 때문에
                        // 최신 인벤토리 정보의 상태를 반영합니다.
                        itemInfo.InventoryInteractive.IsItemSelecting = true;
                        itemInfo.InventoryInfo.inventoryCG.blocksRaycasts = false;
                    }
                }
                // 검출한 오브젝트의 태그가 슬롯이 아니라면, 다시 원위치로 돌려줍니다.
                else
                    itemInfo.UpdatePositionInfo();
            }

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
                itemInfo.OnItemWorldDrop();       // 인벤토리에서 아이템 드롭을 허용합니다.
        }
        else
            throw new System.Exception( "드랍 이상이 발생하였습니다. 확인하여 주세요." );
    }
















    /// <summary>
    /// 처음 셀렉트하기 위해 클릭할 때 바로 셀렉트 종료의 호출이 일어나는 것을 막기 위한 딜레이 메서드
    /// </summary>
    protected IEnumerator FirstSelectDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        isFirstSelectDelay = true;
    }

    /// <summary>
    /// 셀렉팅이 끝났을 때 딜레이를 줘서 초기화 해줘야할 참조 값을 모아놓은 메서드
    /// </summary>
    protected IEnumerator SelectDoneDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        
        // 인벤토리 변경 전 상태를 초기화합니다.
        interactive.IsItemSelecting = false;    
        itemCG.blocksRaycasts = true;           // 드래그가 끝나면 다시 이벤트를 받게 합니다.

        
        // 새로 변경된 인벤토리의 상태를 초기화합니다.
        if( !itemInfo.IsWorldPositioned )
        {
            itemInfo.InventoryInteractive.IsItemSelecting=false;
            itemInfo.InventoryInfo.inventoryCG.blocksRaycasts=true;
        }
    }
    


    





}
