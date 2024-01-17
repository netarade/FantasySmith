
using UnityEngine.EventSystems;

public class DummyItemSelect : ItemSelect
    , IUpdateSelectedHandler, ISelectHandler, IDeselectHandler      // ����Ʈ ���
{

    public override void OnSelect( BaseEventData eventData )
    {
        // ���� �ڵ带 �״�� �����մϴ�.
        base.OnSelect( eventData );

        // ���̾������� �������� �Ͼ�� �ٷ� �������� �����Ͽ� OnDeslect�� ȣ���մϴ�.
        FinishSelecting(eventData);
    }

    public override void OnDeselect( BaseEventData eventData )
    {
        InitFirstOnDeselect();  // �θ� �ڵ带 �����մϴ�.
                          
        // ���̾������� ������ ������ �����մϴ�.
        QuickSlot quickSlot = (QuickSlot)(itemInfo.InventoryInfo);
                                                                  
        // ���̾������� ������ �Ǳ� ���� ���������� �����Ͽ� ���̾������� �������� �����ŵ�ϴ�.
        quickSlot.OnDummySelected(prevParentSlotTr);        


        // ����� ��� �κ��丮�� ����ĳ������ �����մϴ�.
        RaycastAllToConnectedInventory();
        
        // ����ĳ���� ����� ���ϴ�.
        PrintDebugInfo(raycastResults);
        

        InitLastOnDeselect();   
    }

}
