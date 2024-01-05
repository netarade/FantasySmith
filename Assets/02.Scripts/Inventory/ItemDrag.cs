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
 */


/// <summary>
/// Drag스크립트는 ItemInfo 스크립트와 함께 아이템 2D 오브젝트가 컴포넌트로 가지고 있어야 합니다.<br/>
/// 아이템 오브젝트의 드래그를 가능하게 해줍니다.
/// </summary>
public class ItemDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform itemRectTr;      // 자신 transform 캐싱
    private CanvasGroup itemCG;            // 자신의 캔버스 그룹 캐싱
    private ItemInfo itemInfo;             // 자신의 ItemInfo 캐싱
    
    public Transform prevSlotListTr;                // 자식을 옮기기 이전의 아이템 오브젝트가 속한 슬롯리스트
    public Transform prevSlotTr;                    // 자식을 옮기기 이전의 부모 슬롯                       (Drop스크립트에서 참조)

    void Start()
    {
        itemRectTr = GetComponent<RectTransform>();     
        itemCG = GetComponent<CanvasGroup>();     
        itemInfo = GetComponent<ItemInfo>();
    }


    public void OnBeginDrag( PointerEventData eventData )
    {                                                                
        prevSlotTr = itemRectTr.parent;             // 이전 슬롯과 슬롯리스트를 기억한다.
        prevSlotListTr = prevSlotTr.parent;

        // 이전 부모 위치(개별 슬롯)를 저장해둔다.
        itemRectTr.SetParent( prevSlotListTr );     // 부모를 계층하위로 잡아서 이미지 우선순위를 높인다.
        itemCG.blocksRaycasts=false;                // 드래그 이벤트 이외에는 받지 않는다.
    }
        
    public void OnDrag( PointerEventData eventData )
    {
        itemRectTr.position = Input.mousePosition;                // 아이템 2D의 위치를 마우스의 위치와 일치시킨다.        
    }

    public void OnEndDrag( PointerEventData eventData )
    {                                    

        // 마지막 레이케스트가 UI 영역에 닿지 않았다면, (인벤토리 밖으로 빼냈다면,)
        if( eventData.pointerCurrentRaycast.gameObject == null ) 
        {
            Debug.Log("아이템을 밖으로 빼내시겠습니까?");
            //itemInfo.InventoryInfo.RemoveItem(itemInfo.name);
        }    
        
        if( itemRectTr.parent == prevSlotListTr )  // 슬롯의 드랍에 실패했을 때
        {
            itemRectTr.SetParent( prevSlotTr );         // 원래의 부모 슬롯으로 돌린다.
            itemRectTr.localPosition = Vector3.zero;    // 슬롯의 정중앙에 와야한다.
        }

        itemCG.blocksRaycasts=true;                     // 드래그가 끝나면 다시 이벤트를 받게 만든다.

    }

}
