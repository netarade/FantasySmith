
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
        
    IReadOnlyList<InventoryInfo> clientList;    // 서버 인벤토리의 연결된 클라이언트 인벤토리참조
    PointerEventData pEventData;                // 그래픽 레이캐스팅 시 전달 할 포인터 이벤트
    List<RaycastResult> raycastResults;         // 그래픽 레이캐스팅 결과를 받을 리스트
    bool isInventoryConnect;                    // 셀렉팅 중인 아이템의 인벤토리가 연결 상태인 지 여부

    InventoryInteractive interactive;           // 전체 아이템 셀렉팅 여부 참조를 위한 인터렉티브 스크립트 참조
    bool isMyItemSelecting = false;             // 현재 아이템이 선택 중인지 여부
    bool isFirstSelectDelay = false;            // 처음 셀렉팅 후 딜레이 시간이 지났는지 여부
    Button itemSelectBtn;                       // 버튼의 셀렉트를 해제하기 위한 참조
    string strItemDropSpace = "ItemDropSpace";  // 아이템을 드롭할 수 있는 태그 설정(슬롯의 태그)


    void Start()
    {
        itemRectTr = GetComponent<RectTransform>();     
        itemCG = GetComponent<CanvasGroup>();     
        itemInfo = GetComponent<ItemInfo>();
        itemSelectBtn = GetComponent<Button>(); 
              
        pEventData=new PointerEventData( EventSystem.current ); // 이벤트 시스템을 설정합니다.
        raycastResults = new List<RaycastResult>();  
    }

    
    // 셀렉트 진행 중의 처리
    public void OnUpdateSelected( BaseEventData eventData )
    {
        // 내 아이템 선택상태가 아니라면 실행하지 않습니다. (아이템 1개만 실행합니다.)
        if( !isMyItemSelecting )
            return;
            

        // 아이템 위치를 마우스 위치와 일치시면 원점이 마우스 위치로 끌려오므로, 다시 원점위치로 보내서 해당 위치를 기준으로 움직이도록 해줍니다.
        itemRectTr.position = Input.mousePosition + moveVecToCenter;

        // 선택 중에 마우스 버튼을 클릭한 경우 - 반드시 선택 종료 (첫클릭에는 적용하지 않음)
        if( Input.GetMouseButton( 0 ) && isFirstSelectDelay )
        {
            itemSelectBtn.OnDeselect( eventData );              // 버튼을 Deselect상태로 만듭니다.
            EventSystem.current.SetSelectedGameObject( null );  // 이벤트 시스템의 셀렉트 상태를 null로 만듭니다.

        }
    }



    // 셀렉트 시작 시
    public void OnSelect( BaseEventData eventData )
    {        
        // Select를 시작하면 현재 아이템의 인벤토리 정보를 최신화하여 가져옵니다. 
        interactive = itemInfo.InventoryInfo.gameObject.GetComponent<InventoryInteractive>();

        // 아이템 셀렉팅이 하나라도 활성화되어 있다면 다른 아이템의 셀렉팅을 완전히 차단합니다.
        // 자신의 셀렉팅이 진행중이거나, 셀렉팅이 완료되지 않았다면 실행하지 않습니다.
        if( interactive.IsItemSelecting && isMyItemSelecting )
            return;
                
        interactive.IsItemSelecting = true;     // 전체 아이템에 적용하는 선택 상태를 활성화 합니다.
        isMyItemSelecting = true;               // 내 아이템 선택 진행 상태를 활성화 합니다.
                
        StartCoroutine( FirstSelectDelayTime(0.1f) );   // OnUpdateSelected 동시 호출을 방지하기 위해 딜레이를 줘서 상태변수를 활성화시킵니다.
                
        
        // 이전 부모 위치 (슬롯리스트와 개별 슬롯)을 저장합니다.
        prevParentTr=itemRectTr.parent;
        movingParentTr = interactive.gameObject.transform.parent;      // 인벤토리의 부모 캔버스 참조

        itemRectTr.SetParent( movingParentTr );                 // 부모를 일시적으로 인벤토리의 부모인 캔버스로 잡아서 이미지 우선순위를 높입니다.
        itemCG.blocksRaycasts=false;                            // 드래그 이벤트 이외에는 받지 않습니다.

        // 원점 이동 벡터를 구합니다.
        // (현재 인벤토리 원점 위치-마우스 이벤트가 발생한 위치 => 마우스 이벤트 위치에서 인벤토리 원점으로 이동할 수 있는 이동벡터)
        moveVecToCenter=itemRectTr.position-Input.mousePosition;
                






        // 연결된 모든 인벤토리의 그래픽 레이캐스터를 얻기 위한 인벤토리 참조를 서버 인벤토리를 통해 얻습니다.
        clientList = itemInfo.InventoryInfo.ServerInfo.ClientInfo;

        // 현재 아이템이 속한 인벤토리가 연결상태인지를 확인합니다.
        isInventoryConnect = itemInfo.InventoryInfo.IsConnect;

        // 레이캐스트 결과리스트를 초기화합니다.
        raycastResults.Clear();
    }



    // 셀렉트 종료 시
    public void OnDeselect( BaseEventData eventData )
    {        
        isMyItemSelecting=false;            // 내 아이템 선택 상태를 바로 비활성화합니다.
        isFirstSelectDelay=false;           // 처음 클릭 상태를 비활성화 합니다.

        // 이벤트가 일어날 포지션을 마우스를 다시 클릭했을 때의 지점으로 설정합니다.
        pEventData.position=Input.mousePosition;

        // 연결된 모든 인벤토리에게 그래픽 레이캐스팅을 시전하고 결과를 받습니다.
        for(int i=0; i<clientList.Count; i++)
            clientList[i].gRaycaster.Raycast( pEventData, raycastResults );


        // 레이캐스팅에 성공한 경우(검출한 오브젝트가 있는 경우)
        if( raycastResults.Count>0 )
        {
            //string objNames = "";
            //for( int i = 0; i<raycastResults.Count; i++ )
            //    objNames+=raycastResults[i].gameObject.name+" ";
            //print( "[검출되었습니다!]"+objNames );


            // 모든 레이캐스팅 결과를 하나씩 열어봅니다.
            foreach(RaycastResult raycastResult in raycastResults)
            {
                Transform resultTr = raycastResult.gameObject.transform;
                
                // 검출한 오브젝트의 태그가 슬롯이라면,
                if(resultTr.tag == strItemDropSpace)
                {
                    itemInfo.OnItemSlotDrop( resultTr );
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
            //print( "[검출되지 않았습니다]" );


            // 현재 인벤토리가 연결상태라면
            if( isInventoryConnect )
                itemInfo.UpdatePositionInSlotList();    // 원위치로 돌립니다.
            // 현재 인벤토리가 연결상태가 아니라면
            else                
                itemInfo.OnItemWorldDrop();             // 인벤토리에서 아이템 드롭을 허용합니다.
                        
            // 아이템 셀렉팅 상태를 딜레이 시간을 주지 않고 비활성화시킵니다.
            StartCoroutine( SelectDoneDelayTime( 0f ) );
        }
        else
            throw new System.Exception( "드랍 이상이 발생하였습니다. 확인하여 주세요." );

    }

    
    /// <summary>
    /// 처음 셀렉트하기 위해 클릭할 때 바로 셀렉트 종료의 호출이 일어나는 것을 막기 위한 딜레이 메서드
    /// </summary>
    IEnumerator FirstSelectDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        isFirstSelectDelay = true;
    }

    /// <summary>
    /// 셀렉팅이 끝났을 때 딜레이를 줘서 초기화 해줘야할 참조 값을 모아놓은 메서드
    /// </summary>
    IEnumerator SelectDoneDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        interactive.IsItemSelecting = false;
        itemCG.blocksRaycasts = true;       // 드래그가 끝나면 다시 이벤트를 받게 합니다.
    }
    






}
