
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
 * 
 */


/// <summary>
/// Drag스크립트는 ItemInfo 스크립트와 함께 아이템 2D 오브젝트가 컴포넌트로 가지고 있어야 합니다.<br/>
/// 아이템 오브젝트의 드래그를 가능하게 해줍니다.
/// </summary>
public class ItemSelect : MonoBehaviour
    //, IDragHandler, IBeginDragHandler, IEndDragHandler            // 드래그 방식
    , IUpdateSelectedHandler, ISelectHandler, IDeselectHandler      // 셀렉트 방식
{
    private RectTransform itemRectTr;      // 자신 transform 캐싱
    private CanvasGroup itemCG;            // 자신의 캔버스 그룹 캐싱
    private ItemInfo itemInfo;             // 자신의 ItemInfo 캐싱
    
    public Transform prevParentTr;          // 아이템을 옮기기 이전의 부모 (슬롯)
    public Transform movingParentTr;        // 아이템을 옮기는 도중의 부모 (캔버스)

    public Vector3 moveVecToCenter;
        
    GraphicRaycaster gRaycaster;                // 인벤토리 캔버스의 그래픽레이캐스터
    PointerEventData pEventData;                // 그래픽 레이캐스팅 시 전달 할 포인터 이벤트
    List<RaycastResult> raycastResults;         // 그래픽 레이캐스팅 결과를 받을 리스트
    

    InventoryInteractive inventoryInteractive;  // 전체 아이템 셀렉팅 여부 참조를 위한 인터렉티브 스크립트 참조
    bool isMyItemSelecting = false;             // 현재 아이템이 선택 중인지 여부
    bool isFirstSelectDelay = false;            // 처음 셀렉팅 후 딜레이 시간이 지났는지 여부
    Button itemSelectBtn;                       // 버튼의 셀렉트를 해제하기 위한 참조
    string strItemDropSpace = "ItemDropSpace";


    void Start()
    {
        itemRectTr = GetComponent<RectTransform>();     
        itemCG = GetComponent<CanvasGroup>();     
        itemInfo = GetComponent<ItemInfo>();
        itemSelectBtn = GetComponent<Button>(); 

        raycastResults = new List<RaycastResult>();
        pEventData = new PointerEventData(EventSystem.current);
    }



    public void OnUpdateSelected( BaseEventData eventData )
    {
        // 내 아이템 선택상태가 아니라면 실행하지 않습니다. (아이템 1개만 실행합니다.)
        if( !isMyItemSelecting )
            return;
            

        // 아이템 위치를 마우스 위치와 일치시면 원점이 마우스 위치로 끌려오므로, 다시 원점위치로 보내서 해당 위치를 기준으로 움직이도록 해줍니다.
        itemRectTr.position = Input.mousePosition + moveVecToCenter;  

        // 선택 중에 마우스 버튼을 클릭한 경우 - 반드시 선택 종료
        if( Input.GetMouseButton(0) && isFirstSelectDelay )
        {
            print("버튼을 누릅니다.");
            itemSelectBtn.OnDeselect( eventData );              // 버튼을 Deselect상태로 만듭니다.
            EventSystem.current.SetSelectedGameObject( null );  // 이벤트 시스템의 셀렉트 상태를 null로 만듭니다.
            
            isMyItemSelecting = false;                            // 내 아이템 선택 상태를 바로 비활성화합니다.
            isFirstSelectDelay = false;                              // 처음 클릭 상태를 비활성화 합니다.


            // 이벤트가 일어날 포지션을 마우스를 다시 클릭했을 때의 지점으로 설정합니다.
            pEventData.position=Input.mousePosition;

            // 그래픽 레이캐스트를 통해 결과를 받습니다.
            gRaycaster.Raycast( pEventData, raycastResults );

            print("레이캐스팅을 시작합니다.");


            // 레이캐스팅에 성공한 경우(검출한 오브젝트가 있는 경우)
            if( raycastResults.Count>0 )
            {
                for( int i = 0; i<raycastResults.Count; i++ )
                {
                    // 검출한 오브젝트의 태그가 슬롯이라면,
                    if( raycastResults[i].gameObject.tag==strItemDropSpace )
                    {
                        print("드랍 메서드를 호출합니다.");
                        itemInfo.OnItemSlotDrop( raycastResults[i].gameObject.transform );
                    }
                    // 검출한 오브젝트의 태그가 슬롯이 아니라면, 다시 원위치로 돌려줍니다.
                    else
                        itemInfo.UpdatePositionInSlotList();    
                }
                
                // 아이템 셀렉팅 상태를 딜레이 시간을 줘서 비활성화시킵니다.
                StartCoroutine( SelectDoneDelayTime( 0.03f ) );
            }
            // 레이캐스팅의 검출이 없으면서, 부모의 이동이 발생하지 않았다면,(슬롯의 드랍에 실패했다면)
            else if( raycastResults.Count==0 && itemRectTr.parent==movingParentTr )          
            {
                // 아이템 셀렉팅 상태를 딜레이 시간을 주지 않고 비활성화시킵니다.
                StartCoroutine( SelectDoneDelayTime( 0f ) );
                
                // 인벤토리에서 아이템 드롭
                itemInfo.OnItemWorldDrop();
                

                
            }
            else
                throw new System.Exception("드랍 이상이 발생하였습니다. 확인하여 주세요.");

        }
        else if( Input.GetMouseButton( 0 ) )
        {
            print( "아직 활성화되지 않았습니다" );
            itemRectTr.localPosition = Vector3.zero;
        }




    }



    public void OnSelect( BaseEventData eventData )
    {        
        // Select를 시작하면 현재 아이템의 인벤토리 정보를 최신화하여 가져옵니다. 
        inventoryInteractive = itemInfo.InventoryInfo.gameObject.GetComponent<InventoryInteractive>();

        // 아이템 셀렉팅이 하나라도 활성화되어 있다면 다른 아이템의 셀렉팅을 완전히 차단합니다.
        // 자신의 셀렉팅이 진행중이거나, 셀렉팅이 완료되지 않았다면 실행하지 않습니다.
        if( inventoryInteractive.IsItemSelecting && isMyItemSelecting )
            return;
                
        inventoryInteractive.IsItemSelecting = true;    // 전체 아이템에 적용하는 선택 상태를 활성화 합니다.
        isMyItemSelecting = true;                         // 내 아이템 선택 진행 상태를 활성화 합니다.
                
        StartCoroutine( FirstSelectDelayTime(0.1f) );   // OnUpdateSelected 동시 호출을 방지하기 위해 딜레이를 줘서 상태변수를 활성화시킵니다.
                
        
        print("클릭을 시작합니다.");

        // 이전 부모 위치 (슬롯리스트와 개별 슬롯)을 저장합니다.
        prevParentTr=itemRectTr.parent;
        movingParentTr = inventoryInteractive.gameObject.transform.parent;      // 인벤토리의 부모 캔버스 참조

        itemRectTr.SetParent( movingParentTr );                 // 부모를 일시적으로 인벤토리의 부모인 캔버스로 잡아서 이미지 우선순위를 높입니다.
        itemCG.blocksRaycasts=false;                            // 드래그 이벤트 이외에는 받지 않습니다.

        // 원점 이동 벡터를 구합니다.
        // (현재 인벤토리 원점 위치-마우스 이벤트가 발생한 위치 => 마우스 이벤트 위치에서 인벤토리 원점으로 이동할 수 있는 이동벡터)
        moveVecToCenter=itemRectTr.position-Input.mousePosition;
                
        // 아이템정보에 있는 해당 인벤토리를 불러온 다음 부모 Canvas에 접근하여 그래픽레이캐스터를 참조합니다.
        gRaycaster=itemInfo.InventoryInfo.transform.parent.GetComponent<GraphicRaycaster>();

        // 레이캐스트 결과리스트를 초기화합니다.
        raycastResults.Clear();
    }


    public void OnDeselect( BaseEventData eventData )
    {        
        
    }

    
    IEnumerator FirstSelectDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        isFirstSelectDelay = true;
    }

    IEnumerator SelectDoneDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        inventoryInteractive.IsItemSelecting = false;
        itemCG.blocksRaycasts=true;                         // 드래그가 끝나면 다시 이벤트를 받게 만든다.
        print("전체 셀렉팅 종료 호출");
    }
    










    
    //public void OnBeginDrag( PointerEventData eventData )
    //{                                                                
    //    prevSlotTr = itemRectTr.parent;             // 이전 슬롯과 슬롯리스트를 기억한다.
    //    prevSlotListTr = prevSlotTr.parent;

    //    // 이전 부모 위치(개별 슬롯)를 저장해둔다.
    //    itemRectTr.SetParent( prevSlotListTr );     // 부모를 계층하위로 잡아서 이미지 우선순위를 높인다.
    //    itemCG.blocksRaycasts=false;                // 드래그 이벤트 이외에는 받지 않는다.
    //}
        
    //public void OnDrag( PointerEventData eventData )
    //{
    //    itemRectTr.position = Input.mousePosition;                // 아이템 2D의 위치를 마우스의 위치와 일치시킨다.        
    //}

    //public void OnEndDrag( PointerEventData eventData )
    //{                                    

    //    // 마지막 레이케스트가 UI 영역에 닿지 않았다면, (인벤토리 밖으로 빼냈다면,)
    //    if( eventData.pointerCurrentRaycast.gameObject == null ) 
    //    {
    //        Debug.Log("아이템을 밖으로 빼내시겠습니까?");
    //        //itemInfo.InventoryInfo.RemoveItem(itemInfo.name);
    //    }    
        
    //    if( itemRectTr.parent == prevSlotListTr )  // 슬롯의 드랍에 실패했을 때
    //    {
    //        itemRectTr.SetParent( prevSlotTr );         // 원래의 부모 슬롯으로 돌린다.
    //        itemRectTr.localPosition = Vector3.zero;    // 슬롯의 정중앙에 와야한다.
    //    }

    //    itemCG.blocksRaycasts=true;                     // 드래그가 끝나면 다시 이벤트를 받게 만든다.

    //}



}
