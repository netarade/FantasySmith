//using UnityEngine;
//using UnityEngine.EventSystems; // 마우스 클릭, 드래그 드랍, 이벤트 관련 로직

///* [작업 사항]
// * <v1.0 - 2023_1103_최원준>
// * 초기 로직 작성
// * 
// */


///// <summary>
///// Drag스크립트는 ItemInfo 스크립트와 함께 아이템 오브젝트가 컴포넌트로 가지고 있어야 합니다.<br/>
///// 아이템 오브젝트의 드래그를 가능하게 해줍니다.
///// </summary>
//public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
//{
//    private Transform itemTr;                       // 여러번 반복 호출 해야 하므로 자신 transform 캐싱처리
//    private Transform inventoryTr;                  // 판넬-인벤토리의 transform을 참조합니다. 하위에 슬롯 리스트가 있어야 합니다.
//    public  GameObject draggingItem = null;         // 현재 드래그 중인 아이템 오브젝트의 참조값이 들어갑니다.
    
//    //2
//    private Transform slotListTr;                   // 슬롯 리스트의 transform을 참조합니다.
//    private CanvasGroup canvasGroup;                // 

//    void Start()
//    {
//        itemTr = this.transform;   
//        inventoryTr = GameObject.Find("Inventory").transform;


//        //2
//        slotListTr = GameObject.Find("ItemList").transform;
//        canvasGroup = GetComponent<CanvasGroup>();
//    }

//    // 이벤트 데이터만 받는 클래스. 
//    // 기억력 성격 강한 컨트롤,핸들러 클래스가있고
//    // 데이터 담당하는 엔터티 클래스가 있다.
//    // 이벤트 정보만 받아온다.
//    public void OnDrag( PointerEventData eventData )
//    { 
//        itemTr.position = Input.mousePosition;
//    }

//    public void OnBeginDrag( PointerEventData eventData )
//    {                 
//        draggingItem = this.gameObject; // 드래그가 시작되면 드래그 중인 아이템 정보가 있어야 한다
//        canvasGroup.blocksRaycasts = true;
//    }

//    public void OnEndDrag( PointerEventData eventData )
//    {
//        draggingItem = null;            // 드래그 중인 아이템 정보를 null로 만든다.

//        //슬롯으로 드래그 하지 않았을 때 원래대로 ItemList로 돌린다.
//        if(itemTr.parent == inventoryTr)
//        {
//            itemTr.SetParent(slotListTr.transform, false);

//            //2023-1010
//            // GameManager.instance.RemoveItem(GetComponent<Item>());
//            GameManager.instance.RemoveItem(GetComponent<ItemInfo>().itemData);

//        }
//        //부모가 인벤토리인데 끌다가 띄면 다시 돌아가야 한다.

//    }
//}
