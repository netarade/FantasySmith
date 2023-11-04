
using UnityEngine;
using UnityEngine.EventSystems;

/* [작업 사항]
 * <v1.0 - 2023_1105_최원준>
 * 1- d초기 로직 작성, 아이템 스위칭 구현
 * 
 */





public class Drop : MonoBehaviour, IDropHandler
{
    Transform slotTr;   // 슬롯 자신의 트랜스폼
    void Start()
    {
        slotTr = GetComponent<Transform>();     
    }

    /// <summary>
    /// OnDrop이벤트 다음에 OnDragEnd 이벤트가 호출되므로, 아이템 스위칭 처리가 가능합니다.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop( PointerEventData eventData )
    {
        if( transform.childCount==0 )   // 슬롯이 비어있다면, 부모를 변경하고
        {
            Drag.draggingItem.transform.SetParent( transform );         // 해당슬롯으로 부모 변경
            Drag.draggingItem.transform.localPosition = Vector3.zero;   // 정중앙 위치
        }
        else if( transform.childCount==1 ) // 슬롯에 아이템이 이미 있다면, 각 아이템의 위치를 교환한다.
        {
            Drag.draggingItem.transform.SetParent( slotTr );            // 드래그 중인 아이템은 해당 슬롯으로 위치
            Drag.draggingItem.transform.localPosition = Vector3.zero;   // 정중앙 위치 

            Transform switchingItemTr = slotTr.GetChild(0);          // 바꿀 아이템
            switchingItemTr.SetParent( Drag.prevParentTrByDrag );    // 이미지 우선순위 문제로 부모를 일시적으로 바꾸므로, 이전 부모를 받아온다.            
            switchingItemTr.localPosition = Vector3.zero;            // 정중앙 위치
        }

    }

}
