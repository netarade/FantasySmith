using ItemData;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class ItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /*
     * [�۾� ����]
     * 
     * <v1.0 - 2023_1102_�ֿ���>
     * 1- �������� SetOverlapCount, Remove, IsEnoughOverlapCount�޼��� ItemInfoŬ�������� �Űܿ�
     * ������ ������ ������Ű�ų�, ������Ű�ų�, ������ ���ȯ�� Ȯ���ϴ� ���
     * 
     * 2- �������� SetOverlapCount, IsEnoughOverlapCount �޼��� ����
     * ������ ������ ��ü���� ������, �����˻� ����� �ְԵǸ�, �κ��丮�� �ִ� ���¿� ���»��¸� �����ؼ� �ڵ带 �־�� �ϱ� �����̰�,
     * �κ��丮�� ������ ������ ������ ����ȭ ������ �߻��� ���ɼ��� ũ�� ����
     * 
     * 3- Remove�޼��嵵 ����
     * Inventory ���������� ������ ItemInfo�� ��ȯ�ϸ� �״��� �����ϸ� �����̸� �� �ʿ䰡 ���⶧�� 
     * 
     * 
     * <v2.0 - 2024_0104_�ֿ���>
     * 1- SlotDrop�� ���� ���� �޼������ ItemInfo.cs���� �Űܿ�
     * 
     * <v2.1 - 2024_0105_�ֿ���>
     * 1- MoveSlotToAnotherListSlot�޼��� ���� IsSlotEnough�� ItemType��� ȣ�⿡�� ItemInfo��� ȣ��� ����
     * => ��ȭ�������� ��� ������ �ʿ����� ���� ��찡 �ֱ� ����
     * 
     * 2- SlotDrop �̺�Ʈ �߻� �� MoveSlotInSameListSlot������ isActiveTabAll�� �� slotIndex�� ���� �ִ� ���� slotIndexAll�� ����
     * 
     * <2.2 - 2024_0106_�ֿ���
     * 1- ItemPointerStatusWindow�� �����ϴ� �ڵ带 �Ϻ� �Űܿ�.
     * �������� ����â �ڵ带 ��� �ִ����� ����â���� �ű�� ������ �̺�Ʈ �ÿ��� �ش� ����â�ڵ带 ȣ���ϴ� ������� ����
     * 
     * 2- Pointer Enter�� PointerExit�̺�Ʈ ��� �� �������� ������ ������ �Ͼ ������ ����â�� �޼��带 ȣ��
     * 
     * <v2.3 -2024_0112_�ֿ���>
     * 1- �׽�Ʈ�� �޼��� PrintDebugInfo ����
     * 
     * <v2.4 - 2024_0116_�ֿ���>
     * 1- MoveSlotToAnotherListSlot�޼��� ���ο�
     * ���� ��� ���� �˻� �޼��带 IsSlotEnough�� IsSlotEnoughCertain���� ����
     * 
     */

    string strItemDropSpace = "ItemDropSpace";
    
    /// <summary>
    /// �����ۿ��� Ŀ���� ��� ���� �ڵ����� ������ �������ͽ� â�� ����ݴϴ�.
    /// </summary>
    public void OnPointerEnter( PointerEventData eventData )
    {
        statusInteractive.OnItemPointerEnter(this);
    }

    /// <summary>
    /// �����ۿ��� Ŀ���� ���� ���� �ڵ����� ������ �������ͽ� â�� ������ϴ�.
    /// </summary>
    public void OnPointerExit( PointerEventData eventData )
    {
        statusInteractive.OnItemPointerExit();
    }






    /// <summary>
    /// �������� ���� ����� �߻��� �� �������� �̵���Ű�� ������ �����ϱ� ���Ͽ� ȣ������� �ϴ� �޼����Դϴ�.<br/>
    /// ���� Ȥ�� Ÿ �κ��丮�� ���� �� ��� �߻��� ����մϴ�.<br/><br/>
    /// ����->���� �κ��丮 ���� : �������� �ʽ��ϴ�. ���� �������� �ִٸ� ��ġ�� ��ȯ�մϴ�.<br/>
    /// ����->�ٸ� �κ��丮 ���� : ���ڸ��� ���ٸ� �����մϴ�. ���� �������� �ִٸ� �����մϴ�.<br/>
    /// </summary>
    /// <returns>���� ��ӿ� ���� �� true�� ���� �� false�� ��ȯ�մϴ�.</returns>
    public bool OnItemSlotDrop( Transform callerSlotTr )
    {        
        // ȣ�� ���ڰ� ���޵��� �ʾҴ��� �˻�
        if( callerSlotTr==null)
            throw new Exception("������ ������ ���޵��� �ʾҽ��ϴ�. �ùٸ� ���� ����̺�Ʈ ȣ������ Ȯ���Ͽ� �ּ���.");

        bool isCallerSlot = (callerSlotTr.tag == strItemDropSpace);
        bool isPrevCallerSlot = (prevDropSlotTr.tag == strItemDropSpace);
        
        // ȣ���ڰ� �������� �˻�
        if( !isCallerSlot )
            throw new Exception("�������ڰ� ������ �ƴմϴ�. �ùٸ� ���� ����̺�Ʈ ȣ������ Ȯ���Ͽ� �ּ���.");
        // ���� ȣ���� ������ �������� �˻�
        else if( !isPrevCallerSlot )
            throw new Exception("����->������ ����̺�Ʈ�� �߻��Ͽ����ϴ�. �ùٸ� ���� ����̺�Ʈ ȣ������ Ȯ���Ͽ� �ּ���.");

        // ���� ���� ȣ���ڿ� ���� ����̺�Ʈ ȣ���ڰ� ���ٸ�,
        if(callerSlotTr==prevDropSlotTr)        
        {   
            print("���� �κ��丮 ���� ���� �� ��� �߻�");
            return MoveSlotInSameListSlot(callerSlotTr);            // ������ �κ��丮�� ������ ����->���������� �̵�
        }
        else
        {
            // ���� ����̺�Ʈ ȣ���ڿ� �θ� ���ٸ�(������ ���� ����Ʈ������ �̵��̶��)
            if( callerSlotTr.parent == prevDropSlotTr.parent)
            {
                print("���� �κ��丮 Ÿ ���� �� ��� �߻�");
                return MoveSlotInSameListSlot(callerSlotTr);       // ���� �κ��丮 Ÿ ���� �� �̵�
            }
            else
            {
                print("Ÿ �κ��丮 Ÿ ���� �� ��� �߻�");
                return MoveSlotToAnotherListSlot(callerSlotTr);    // Ÿ �κ��丮 ���������� �̵�
            }
        }   
    }







    /// <summary>
    /// �������� ���� ���Կ��� �ٸ� �������� �̵����� �ִ� �޼����Դϴ�.<br/>
    /// ������ �ڽ��ε����� �о� �鿩 �����ۿ� ������� �ְ�, ������ ������Ʈ�� ��ġ�� ������Ʈ �մϴ�.<br/>
    /// ���� ���Կ� �̹� �ٸ� ������ ������Ʈ�� �ִٸ� ������ �ε��� ������ ��ġ�� ��ȯ�մϴ�.<br/>
    /// </summary>
    /// <returns>����� ���������� �����Ƿ� true�� ��ȯ�մϴ�. (����Ī �Ұ��� ������ ���� ������ �����մϴ�.)</returns>
    private bool MoveSlotInSameListSlot(Transform nextSlotTr)
    {
        int nextSlotIdx = nextSlotTr.GetSiblingIndex();     // ���� ������ �ε��� �ش� ������ �ڽĳѹ��� �����մϴ�.
        
        if( nextSlotTr.childCount==0 )
        {
            // Ȱ��ȭ ���� �ǿ� ���� �� �������� ���� �ε��� ������ �����մϴ�.
            if(isActiveTabAll)
                item.SlotIndexAll = nextSlotIdx;
            else
                item.SlotIndexEach = nextSlotIdx;

            // �ش� ������ ��ġ������ ������Ʈ �մϴ�.
            UpdatePositionInSlotList();
        }
        else if(nextSlotTr.childCount==1)
        {             
            // ���Կ� ����ִ� �ٲ� �������� ������ �����ɴϴ�.
            ItemInfo switchItemInfo = nextSlotTr.GetChild(0).GetComponent<ItemInfo>(); 
            
            // Ȱ��ȭ ���� �ǿ� ���� �� �������� �ε��� ������ �����մϴ�.
            if(isActiveTabAll)
            {
                switchItemInfo.SlotIndexAll = item.SlotIndexAll;  // �ٲܾ������� �ε����� �� �������� ���� ������ �ε����� �־��ݴϴ�.      
                item.SlotIndexAll = nextSlotIdx;                  // �� �������� �ε����� �ٲ� ������ ��ġ�� �����մϴ�.
            }
            else
            {
                switchItemInfo.SlotIndex = item.SlotIndexEach;          
                item.SlotIndexEach = nextSlotIdx;
            }
            
            switchItemInfo.UpdatePositionInSlotList();      // �ٲ� �������� ��ġ ������ ������Ʈ �մϴ�.
            UpdatePositionInSlotList();                     // �� �������� ��ġ ������ ������Ʈ �մϴ�.
        }
        else  // ���Կ� �ڽ��� 2�� �̻��� ��� - ���� ó��
            throw new Exception("���Կ� �ڽ��� 2�� �̻� �����ֽ��ϴ�. Ȯ���Ͽ� �ּ���.");

        prevDropSlotTr = nextSlotTr;                 // �����ߴٸ� ���� ����̺�Ʈ ȣ���ڸ� �ֽ�ȭ�մϴ�. 

        return true;    // ����� ���������� �����Ƿ� true�� ��ȯ�մϴ�. (����Ī �Ұ��� ������ ���� ������ �����մϴ�.)
    }



    
    /// <summary>
    /// �������� ���� �κ��丮�� ���Կ��� �ٸ� �κ��丮�� �������� �̵������ִ� �޼����Դϴ�.<br/>
    /// �ش� �κ��丮�� ���� �ڸ��� �ִ��� �ű�� ���� Ȯ���Ͽ� �ڸ��� ����ϴٸ�<br/>
    /// ������ �κ��丮 ��Ͽ��� �� �������� �����ϰ�, �ű� �κ��丮�� ��� ������ �ֽ�ȭ�Ͽ� �ݴϴ�.<br/>
    /// �̴� ��ư ������ �������� �Űܾ� �� ��찡 �ֱ� �����Դϴ�.<br/>
    /// </summary>
    /// <returns>���ο� �κ��丮 ���Կ� ���� �ڸ��� �ִ°�� true��, �����ڸ��� ���ų� �ش� ���Կ� ������ �������� �ִٸ� false�� ��ȯ</returns>
    private bool MoveSlotToAnotherListSlot(Transform nextSlotTr)
    {
        if(nextSlotTr.childCount>=1)        // ���Կ� �������� ����ִٸ�,
        {
            UpdatePositionInSlotList();     // ����ġ�� �ǵ����� ���и� ��ȯ�մϴ�.
            return false;
        }

        // ���ڷ� ������ ������ ���������� ������� �κ��丮 ������ �����մϴ�
        Transform nextInventoryTr = nextSlotTr.parent.parent.parent.parent;
        InventoryInfo nextInventoryInfo = nextInventoryTr.GetComponent<InventoryInfo>();

        // ��ü�� ���ο� �ڽ� �ε����� ����
        bool isActiveTabAllNext = nextInventoryInfo.interactive.IsActiveTabAll;
        int childIdxNext = nextSlotTr.GetSiblingIndex();

        // ���ο� �κ��丮 ���Կ� ���� �ڸ��� �ִ� ���
        if( nextInventoryInfo.IsSlotEnoughCertain(this,childIdxNext,isActiveTabAllNext) )
        {
            inventoryInfo.RemoveItem(this);         // ���� �κ��丮���� �������� �����ؾ� �մϴ�.
            
            nextInventoryInfo.AddItem(this);        // ���� �������� ���ο� �����ۿ� �߰��մϴ�.
                                                    // (AddItem���������� UpdateInventoryInfo�� ���� ������ prevDropSlot ������ ��)

            return true;                            // ������ ��ȯ�մϴ�.
        }
        // ���ο� �κ��丮 ���Կ� ���� �ڸ��� ���� ���
        else
        {
            UpdatePositionInSlotList();     // ����ġ�� �ǵ����� ���и� ��ȯ�մϴ�.
            return false;
        }
    }


    

}
