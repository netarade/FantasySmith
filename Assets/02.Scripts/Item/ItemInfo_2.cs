using ItemData;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class ItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /*
     * [작업 사항]
     * 
     * <v1.0 - 2023_1102_최원준>
     * 1- 아이템의 SetOverlapCount, Remove, IsEnoughOverlapCount메서드 ItemInfo클래스에서 옮겨옴
     * 아이템 수량을 증감시키거나, 삭제시키거나, 수량이 충분환지 확인하는 기능
     * 
     * 2- 아이템의 SetOverlapCount, IsEnoughOverlapCount 메서드 삭제
     * 이유는 아이템 자체적인 삭제나, 정보검색 기능을 넣게되면, 인벤토리가 있는 상태와 없는상태를 구분해서 코드를 넣어야 하기 때문이고,
     * 인벤토리를 통하지 않으면 정보의 동기화 오류가 발생할 가능성이 크기 때문
     * 
     * 3- Remove메서드도 삭제
     * Inventory 내부적으로 제거후 ItemInfo를 반환하면 그다음 삭제하면 딜레이를 줄 필요가 없기때문 
     * 
     * 
     * <v2.0 - 2024_0104_최원준>
     * 1- SlotDrop에 관한 관련 메서드들을 ItemInfo.cs에서 옮겨옴
     * 
     * <v2.1 - 2024_0105_최원준>
     * 1- MoveSlotToAnotherListSlot메서드 내부 IsSlotEnough를 ItemType기반 호출에서 ItemInfo기반 호출로 변경
     * => 잡화아이템의 경우 슬롯이 필요하지 않은 경우가 있기 때문
     * 
     * 2- SlotDrop 이벤트 발생 시 MoveSlotInSameListSlot내에서 isActiveTabAll일 때 slotIndex에 값을 넣던 점을 slotIndexAll로 변경
     * 
     * <2.2 - 2024_0106_최원준
     * 1- ItemPointerStatusWindow에 존재하던 코드를 일부 옮겨옴.
     * 아이템이 상태창 코드를 들고 있던것을 상태창으로 옮기고 포인터 이벤트 시에만 해당 상태창코드를 호출하는 방식으로 변경
     * 
     * 2- Pointer Enter와 PointerExit이벤트 상속 후 아이템의 포인터 접근이 일어날 때마다 상태창의 메서드를 호출
     * 
     * <v2.3 -2024_0112_최원준>
     * 1- 테스트용 메서드 PrintDebugInfo 삭제
     * 
     */

    string strItemDropSpace = "ItemDropSpace";
    
    /// <summary>
    /// 아이템에서 커서를 대는 순간 자동으로 아이템 스테이터스 창을 띄워줍니다.
    /// </summary>
    public void OnPointerEnter( PointerEventData eventData )
    {
        statusInteractive.OnItemPointerEnter(this);
    }

    /// <summary>
    /// 아이템에서 커서를 떼는 순간 자동으로 아이템 스테이터스 창이 사라집니다.
    /// </summary>
    public void OnPointerExit( PointerEventData eventData )
    {
        statusInteractive.OnItemPointerExit();
    }






    /// <summary>
    /// 아이템의 슬롯 드롭이 발생할 때 아이템을 이동시키고 정보를 이전하기 위하여 호출해줘야 하는 메서드입니다.<br/>
    /// 동일 혹은 타 인벤토리의 슬롯 간 드랍 발생시 사용합니다.<br/><br/>
    /// 슬롯->같은 인벤토리 슬롯 : 실패하지 않습니다. 기존 아이템이 있다면 위치를 교환합니다.<br/>
    /// 슬롯->다른 인벤토리 슬롯 : 빈자리가 없다면 실패합니다. 기존 아이템이 있다면 실패합니다.<br/>
    /// </summary>
    /// <returns>슬롯 드롭에 성공 시 true를 실패 시 false를 반환합니다.</returns>
    public bool OnItemSlotDrop( Transform callerSlotTr )
    {        
        // 호출 인자가 전달되지 않았는지 검사
        if( callerSlotTr==null)
            throw new Exception("슬롯의 참조가 전달되지 않았습니다. 올바른 슬롯 드랍이벤트 호출인지 확인하여 주세요.");

        bool isCallerSlot = (callerSlotTr.tag == strItemDropSpace);
        bool isPrevCallerSlot = (prevDropSlotTr.tag == strItemDropSpace);
        
        // 호출자가 슬롯인지 검사
        if( !isCallerSlot )
            throw new Exception("전달인자가 슬롯이 아닙니다. 올바른 슬롯 드랍이벤트 호출인지 확인하여 주세요.");
        // 이전 호출자 정보가 슬롯인지 검사
        else if( !isPrevCallerSlot )
            throw new Exception("월드->슬롯의 드랍이벤트가 발생하였습니다. 올바른 슬롯 드랍이벤트 호출인지 확인하여 주세요.");

        // 현재 슬롯 호출자와 이전 드랍이벤트 호출자가 같다면,
        if(callerSlotTr==prevDropSlotTr)        
        {   
            print("동일 인벤토리 동일 슬롯 간 드롭 발생");
            return MoveSlotInSameListSlot(callerSlotTr);            // 동일한 인벤토리의 동일한 슬롯->슬롯으로의 이동
        }
        else
        {
            // 이전 드랍이벤트 호출자와 부모가 같다면(동일한 슬롯 리스트에서의 이동이라면)
            if( callerSlotTr.parent == prevDropSlotTr.parent)
            {
                print("동일 인벤토리 타 슬롯 간 드롭 발생");
                return MoveSlotInSameListSlot(callerSlotTr);       // 같은 인벤토리 타 슬롯 간 이동
            }
            else
            {
                print("타 인벤토리 타 슬롯 간 드롭 발생");
                return MoveSlotToAnotherListSlot(callerSlotTr);    // 타 인벤토리 슬롯으로의 이동
            }
        }   
    }







    /// <summary>
    /// 아이템을 기존 슬롯에서 다른 슬롯으로 이동시켜 주는 메서드입니다.<br/>
    /// 슬롯의 자식인덱스를 읽어 들여 아이템에 적용시켜 주고, 아이템 오브젝트의 위치를 업데이트 합니다.<br/>
    /// 만약 슬롯에 이미 다른 아이템 오브젝트가 있다면 서로의 인덱스 정보와 위치를 교환합니다.<br/>
    /// </summary>
    /// <returns>현재는 실패조건이 없으므로 true를 반환합니다. (스위칭 불가한 아이템 등이 나오면 조절합니다.)</returns>
    private bool MoveSlotInSameListSlot(Transform nextSlotTr)
    {
        int nextSlotIdx = nextSlotTr.GetSiblingIndex();     // 다음 슬롯의 인덱스 해당 슬롯의 자식넘버를 참조합니다.
        
        if( nextSlotTr.childCount==0 )
        {
            // 활성화 중인 탭에 따른 이 아이템의 슬롯 인덱스 정보를 수정합니다.
            if(isActiveTabAll)
                item.SlotIndexAll = nextSlotIdx;
            else
                item.SlotIndex = nextSlotIdx;

            // 해당 정보로 위치정보를 업데이트 합니다.
            UpdatePositionInSlotList();
        }
        else if(nextSlotTr.childCount==1)
        {             
            // 슬롯에 담겨있는 바꿀 아이템의 정보를 가져옵니다.
            ItemInfo switchItemInfo = nextSlotTr.GetChild(0).GetComponent<ItemInfo>(); 
            
            // 활성화 중인 탭에 따른 두 아이템의 인덱스 정보를 수정합니다.
            if(isActiveTabAll)
            {
                switchItemInfo.SlotIndexAll = item.SlotIndexAll;  // 바꿀아이템의 인덱스를 이 아이템의 이전 슬롯의 인덱스로 넣어줍니다.      
                item.SlotIndexAll = nextSlotIdx;                  // 이 아이템의 인덱스를 바뀔 슬롯의 위치로 수정합니다.
            }
            else
            {
                switchItemInfo.SlotIndex = item.SlotIndex;          
                item.SlotIndex = nextSlotIdx;
            }
            
            switchItemInfo.UpdatePositionInSlotList();      // 바꿀 아이템의 위치 정보를 업데이트 합니다.
            UpdatePositionInSlotList();                     // 이 아이템의 위치 정보를 업데이트 합니다.
        }
        else  // 슬롯에 자식이 2개 이상인 경우 - 예외 처리
            throw new Exception("슬롯에 자식이 2개 이상 겹쳐있습니다. 확인하여 주세요.");

        prevDropSlotTr = nextSlotTr;                 // 성공했다면 이전 드랍이벤트 호출자를 최신화합니다. 

        return true;    // 현재는 실패조건이 없으므로 true를 반환합니다. (스위칭 불가한 아이템 등이 나오면 조절합니다.)
    }



    
    /// <summary>
    /// 아이템을 기존 인벤토리의 슬롯에서 다른 인벤토리의 슬롯으로 이동시켜주는 메서드입니다.<br/>
    /// 해당 인벤토리에 남는 자리가 있는지 옮기기 전에 확인하여 자리가 충분하다면<br/>
    /// 이전의 인벤토리 목록에서 이 아이템을 제거하고, 옮길 인벤토리로 모든 정보를 최신화하여 줍니다.<br/>
    /// 이는 버튼 등으로 아이템을 옮겨야 할 경우가 있기 때문입니다.<br/>
    /// </summary>
    /// <returns>새로운 인벤토리 슬롯에 남는 자리가 있는경우 true를, 남는자리가 없거나 해당 슬롯에 기존의 아이템이 있다면 false를 반환</returns>
    private bool MoveSlotToAnotherListSlot(Transform nextSlotTr)
    {
        if(nextSlotTr.childCount>=1)        // 슬롯에 아이템이 담겨있다면,
        {
            UpdatePositionInSlotList();     // 원위치로 되돌리고 실패를 반환합니다.
            return false;
        }

        // 인자로 전달한 슬롯의 계층정보를 기반으로 인벤토리 참조를 설정합니다
        Transform nextInventoryTr = nextSlotTr.parent.parent.parent.parent;
        InventoryInfo nextInventoryInfo = nextInventoryTr.GetComponent<InventoryInfo>();

        // 새로운 인벤토리 슬롯에 남는 자리가 있는 경우
        if( nextInventoryInfo.IsSlotEnough(this) )
        {
            inventoryInfo.RemoveItem(item.Name);    // 이전 인벤토리에서 아이템을 제거해야 합니다.
            
            nextInventoryInfo.AddItem(this);        // 현재 아이템을 새로운 아이템에 추가합니다.
                                                    // (AddItem내부적으로 UpdateInventoryInfo가 들어가기 때문에 prevDropSlot 정보도 들어감)

            return true;                            // 성공을 반환합니다.
        }
        // 새로운 인벤토리 슬롯에 남는 자리가 없는 경우
        else
        {
            UpdatePositionInSlotList();     // 원위치로 되돌리고 실패를 반환합니다.
            return false;
        }
    }


    

}
