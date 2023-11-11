using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems; // 마우스 클릭, 드래그 드랍, 이벤트 관련 로직
using UnityEngine.UI;


/* [작업 사항]
 * <v1.0 - 2023_1108_최원준>
 * 1- 초기 로직 작성
 * 
 */


/// <summary>
/// InventoryDrag스크립트는 Inventory 오브젝트가 컴포넌트로 가지고 있어야 합니다.<br/>
/// 인벤토리 오브젝트의 드래그를 가능하게 해줍니다.
/// </summary>
public class InventoryDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform tr;                               // 여러번 반복 호출 해야 하므로 자신 transform 캐싱처리
    private CanvasGroup cg;
    
    void Start()
    {
        tr= transform;
        cg = GetComponent<CanvasGroup>();
    }
    
    public void OnBeginDrag( PointerEventData eventData )
    {
        cg.blocksRaycasts = false;       // 다른 UI 이벤트를 받지 않습니다.
    }
    public void OnDrag( PointerEventData eventData )
    {
        tr.position = Input.mousePosition;                // 아이템의 위치를 마우스의 위치와 일치시킨다.
    }


    public void OnEndDrag( PointerEventData eventData )
    {
        cg.blocksRaycasts = true;          // 다른 UI 이벤트를 받습니다.
    }
}
