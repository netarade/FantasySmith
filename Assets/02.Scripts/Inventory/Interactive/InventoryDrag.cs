using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems; // 마우스 클릭, 드래그 드랍, 이벤트 관련 로직
using UnityEngine.UI;


/* [작업 사항]
 * <v1.0 - 2023_1108_최원준>
 * 1- 초기 로직 작성
 * 
 * <v1.1 - 2024_0106_최원준>
 * 1- isStop 변수 추가하여 특정상황에서 동작하지 않도록 하였음.
 * 2- UpdateDragStopConditon메서드를 통해 외부에서 메서드 호출을 통해 isStop변수를 변경가능하도록 구현
 * 
 * <v1.2 - 2024_0107_최원준>
 * 1- isStop변수와 UpdateDragStopCondition메서드 삭제,
 * 아이템이 드래그가 일어나면 InventoryInteractive스크립트에 IsItemSelecting 상태를 활성화시키도록 하였고,
 * 이 상태변수의 활성화 여부를 읽어서 현 스크립트도 작동하지 않게끔 구현
 * 
 * 2- 변수명 tr을 inventoryTr로 변경
 * 
 * <v1.3 - 2024_0223_최원준>
 * 1- InventoryInteractive의 IsItemSlecting 상태 변수에 따라서 인벤토리 드래그를 막는 조건을 삭제
 * 이유는 아이템 셀렉팅을 더이상 상태변수로 관리하지 않고 셀렉팅 시 캔버스그룹의 속성을 해제함으로서 상호작용을 막기 때문
 * 
 */


/// <summary>
/// InventoryDrag스크립트는 Inventory 오브젝트가 컴포넌트로 가지고 있어야 합니다.<br/>
/// 인벤토리 오브젝트의 드래그를 가능하게 해줍니다.
/// </summary>
public class InventoryDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    RectTransform inventoryTr;  // 여러번 반복 호출 해야 하므로 자신 transform 캐싱처리
    CanvasGroup inventoryCG;    // 자신의 캔버스 그룹 컴포넌트 참조
    Vector3 moveVecToCenter;    // 인벤토리에 마우스 위치를 찍은다음 원점을 마우스 위치로 잡을 때 다시 원점으로 돌려보낼 벡터
                            

    void Start()
    {
        inventoryTr= GetComponent<RectTransform>();
        inventoryCG = GetComponent<CanvasGroup>();
    }
    
    public void OnBeginDrag( PointerEventData eventData )
    {                 
        // 현재 인벤토리 원점 위치 - 마우스 이벤트가 발생한 위치 => 마우스 이벤트 위치에서 인벤토리 원점으로 이동할 수 있는 이동벡터
        moveVecToCenter = inventoryTr.position - Input.mousePosition;

        // 다른 UI 이벤트를 받지 않습니다.
        inventoryCG.blocksRaycasts = false;       
    }
    public void OnDrag( PointerEventData eventData )
    {   
        // 마우스 위치와 인벤토리 위치를 일치시키면 원점이 마우스위치로 끌려오므로, 마우스위치에서 원점으로 보낸 포지션대로 움직이도록 해줍니다.
        inventoryTr.position = Input.mousePosition + moveVecToCenter;                
    }


    public void OnEndDrag( PointerEventData eventData )
    {   
        // 다른 UI 이벤트를 받습니다.
        inventoryCG.blocksRaycasts = true;          
    }
}
