using Unity.VisualScripting;
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
 */


/// <summary>
/// Drag스크립트는 ItemInfo 스크립트와 함께 아이템 오브젝트가 컴포넌트로 가지고 있어야 합니다.<br/>
/// 아이템 오브젝트의 드래그를 가능하게 해줍니다.
/// </summary>
public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform itemTr;                       // 여러번 반복 호출 해야 하므로 자신 transform 캐싱처리
    public static GameObject draggingItem;          // 현재 드래그 중인 아이템 오브젝트의 참조값이 들어갑니다.
    private CanvasGroup canvasGroup;                // blockRaycast를 위한 캔버스 그룹 등록

    private Transform inventoryTr;                  // 슬롯과 아이템이 담겨있는 상위부모 인벤토리
    public static Transform prevParentTrByDrag;     // 자식을 옮기기 이전의 부모 슬롯

    void Start()
    {
        itemTr=this.transform;
        draggingItem = null;
        inventoryTr = GameObject.Find("Inventory").transform;                 

        if( inventoryTr==null )
            Debug.Log("Drag스크립트 null 참조를 확인해주세요.");
    }

    public void OnDrag( PointerEventData eventData )
    {
        itemTr.position=Input.mousePosition;                // 아이템의 위치를 마우스의 위치와 일치시킨다.

    }

    public void OnBeginDrag( PointerEventData eventData )
    {

        draggingItem=this.gameObject;                       // 드래그가 시작되면 드래그 중인 아이템 정보가 있어야 한다

        prevParentTrByDrag=itemTr.parent;                         // 이전 부모 위치를 저장해둔다.
        itemTr.SetParent( inventoryTr.transform );          // 부모를 계층하위로 잡아서 이미지 우선순위를 높인다.
        itemTr.GetComponent<Image>().raycastTarget=false;   // 드래그 이벤트 이외에는 받지 않는다.
    }

    public void OnEndDrag( PointerEventData eventData )
    {
        draggingItem=null;                                  // 드래그 중인 아이템 정보를 null로 만든다.
        itemTr.GetComponent<Image>().raycastTarget=true;    // 드래그가 끝나면 다시 이벤트를 받게 만든다.

        if( itemTr.parent==inventoryTr )                    //슬롯으로 드래그 하지 않았을 때
        {
            itemTr.SetParent( prevParentTrByDrag );               // 원래의 부모로 돌린다.
            itemTr.localPosition=Vector3.zero;              // 슬롯의 정중앙에 와야한다.
        }

    }

}
