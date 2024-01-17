
using UnityEngine.EventSystems;

public class DummyItemSelect : ItemSelect
    , IUpdateSelectedHandler, ISelectHandler, IDeselectHandler      // 셀렉트 방식
{

    public override void OnSelect( BaseEventData eventData )
    {
        // 이전 코드를 그대로 실행합니다.
        base.OnSelect( eventData );

        // 더미아이템의 셀렉팅이 일어나면 바로 셀렉팅을 종료하여 OnDeslect를 호출합니다.
        FinishSelecting(eventData);
    }

    public override void OnDeselect( BaseEventData eventData )
    {
        InitFirstOnDeselect();  // 부모 코드를 실행합니다.
                          
        // 더미아이템의 퀵슬롯 정보를 참조합니다.
        QuickSlot quickSlot = (QuickSlot)(itemInfo.InventoryInfo);
                                                                  
        // 더미아이템의 셀렉팅 되기 이전 슬롯정보를 전달하여 더미아이템의 셀렉팅을 종료시킵니다.
        quickSlot.OnDummySelected(prevParentSlotTr);        


        // 연결된 모든 인벤토리에 레이캐스팅을 시전합니다.
        RaycastAllToConnectedInventory();
        
        // 레이캐스팅 결과를 봅니다.
        PrintDebugInfo(raycastResults);
        

        InitLastOnDeselect();   
    }

}
