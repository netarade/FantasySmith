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
 * 
 * 
 * 
 * [추후 예정] 
 * 1- prevParentTrByDrag를 static변수로 두면 곤란한 문제가 추후에 발생 예정
 * 플레이어 마다 Drag스크립트를 가지고 있으므로 인벤토리별 분리가 필요. InventoryManagement클래스에서 해당 정보를 가지고 있을 필요성
 * 2- inventoryTr을 Canvas 참조 기준으로 설정하고 있는점.=>아이템의 상위,상위부모로 간접참조를 해야함
 * 
 * 
 */


/// <summary>
/// Drag스크립트는 ItemInfo 스크립트와 함께 아이템 2D 오브젝트가 컴포넌트로 가지고 있어야 합니다.<br/>
/// 아이템 오브젝트의 드래그를 가능하게 해줍니다.
/// </summary>
public class ItemDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform itemRectTr;               // 여러번 반복 호출 해야 하므로 자신 transform 캐싱처리
    private Transform itemTr;                       // 아이템의 계층 최상위 부모 오브젝트의 Transform

    public static GameObject draggingObj2D;         // 현재 드래그 중인 아이템 2D 오브젝트의 참조값이 들어갑니다.
    public static GameObject draggingObj3D;         // 현재 드래그 중인 아이템 3D 오브젝트의 참조값이 들어갑니다.

    private Transform inventoryTr;                  // 슬롯과 아이템이 담겨있는 상위부모 인벤토리
    public static Transform prevParentTrByDrag;     // 자식을 옮기기 이전의 부모 슬롯

    void Start()
    {
        itemRectTr = transform.GetComponent<RectTransform>(); // 2D 오브젝트 RectTransform은 현재 드래그하고 있는 오브젝트의 트랜스폼이며,
        itemTr = transform.GetChild(transform.childCount-1);  // 3D 오브젝트 Transform은 2D오브젝트의 최하위 자식의 트랜스폼이다.

        draggingObj2D = null;

        Transform canvasTr = GameObject.FindWithTag("CANVAS_CHARACTER").transform;
        inventoryTr = canvasTr.GetChild(0);                     // 인벤토리는 캐릭터 캔버스의 0번째 자식             
    }

    public void OnDrag( PointerEventData eventData )
    {
        itemRectTr.position=Input.mousePosition;                // 아이템 2D의 위치를 마우스의 위치와 일치시킨다.
        
    }

    public void OnBeginDrag( PointerEventData eventData )
    {

        draggingObj2D = transform.gameObject;                   // 드래그가 시작되면 드래그 중인 아이템 정보가 있어야 한다
        //draggingObj3D = transform.parent.gameObject;
                                                                

        prevParentTrByDrag = itemRectTr.parent;                 // 이전 부모 위치(개별 슬롯)를 저장해둔다.
        itemRectTr.SetParent( inventoryTr.transform );          // 부모를 계층하위로 잡아서 이미지 우선순위를 높인다.
        itemRectTr.GetComponent<Image>().raycastTarget=false;   // 드래그 이벤트 이외에는 받지 않는다.
    }

    public void OnEndDrag( PointerEventData eventData )
    {
        // 마지막 레이케스트가 UI 영역에 닿지 않았다면,
        if( eventData.pointerCurrentRaycast.gameObject == null ) 
        {
            // itemRectTr.SetParent(null);   // 계층 최상위로 변경한다. (월드로 내보낸다.)
            // 인벤토리에서 제거.
            // 2D, 3D 전환

        }




        draggingObj2D = null;                                   // 드래그 중인 아이템 정보를 null로 만든다.
        //draggingObj3D = null;

        itemRectTr.GetComponent<Image>().raycastTarget=true;    // 드래그가 끝나면 다시 이벤트를 받게 만든다.
       
        if( itemRectTr.parent == inventoryTr )                  // 슬롯으로 드래그 하지 않았을 때
        {
            itemRectTr.SetParent( prevParentTrByDrag );         // 원래의 부모로 돌린다.
            itemRectTr.localPosition = Vector3.zero;            // 슬롯의 정중앙에 와야한다.
        }

    }

}
