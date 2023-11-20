using ItemData;
using UnityEngine;
using UnityEngine.EventSystems;

/* [�۾� ����]
 * <v1.0 - 2023_1105_�ֿ���>
 * 1- d�ʱ� ���� �ۼ�, ������ ����Ī ����
 * 
 * <v1.1 - 2023_1106_�ֿ���>
 * ��� �� ������ ��ġ�� �����ϵ��� ����
 * 
 * <v2.0 - 2023_1109_�ֿ���>
 * 1- ��ȭ��, �Ӽ���, ���μ��� ���� ���� ����
 * 2- ���� ĳ��ó���Ͽ� �ߺ� ��������
 * 
 * 
 * <v2.1 - 2023_1119_�ֿ���>
 * 1- ItemInfo��ũ��Ʈ�� item�� ���� �����ϰ� �ִ� ���� Item(������Ƽ)���� �������� ����
 */


public class Drop : MonoBehaviour, IDropHandler
{
    Transform slotTr;   // ���� �ڽ��� Ʈ������
    void Start()
    {
        slotTr = GetComponent<Transform>();     
    }

    /// <summary>
    /// OnDrop�̺�Ʈ ������ OnDragEnd �̺�Ʈ�� ȣ��ǹǷ�, ������ ����Ī ó���� �����մϴ�.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop( PointerEventData eventData )
    {
        GameObject dragObj = Drag.draggingItem;             
        Item draggingItem = dragObj.GetComponent<ItemInfo>().Item;   // ���� ����ϹǷ� ĳ��ó��



        // �巡�� ���� �������� �Ӽ����̳� ��ȭ��, ���μ��̶��, �����ϴ� ������.
        if( draggingItem.Type == ItemType.Misc )
        {
            ItemMisc draggingMisc = ((ItemMisc)draggingItem); 

            if( draggingMisc.EnumMiscType==MiscType.Engraving || draggingMisc.EnumMiscType==MiscType.Enhancement )
            {                
                Item applyItem = slotTr.GetChild(0).gameObject.GetComponent<ItemInfo>().Item;   //��ȭ�� ������ �������� ������ ���ϴ�.
                if(applyItem.Type == ItemType.Weapon)                 // ���� ����� ������ ��ȭ ������ �����ϰ� �ƴ϶�� ����Ī ������ �����մϴ�.
                {
                   
                    switch (draggingItem.Name)
                    {
                        case "��ȭ��" :
                            ((ItemWeapon)applyItem).BasePerformance *= 1.15f;  // �⺻ ������ 15% ������ŵ�ϴ�.
                            break;
                        case "�Ӽ���" :
                            if( ((ItemWeapon)applyItem).IsAttrUnlocked )           // �Ӽ��� ����ִٸ� 
                            {
                                ((ItemWeapon)applyItem).BasePerformance *= 1.50f;  // �⺻ ������ 50% ������ŵ�ϴ�.
                                ((ItemWeapon)applyItem).IsAttrUnlocked = false;    // �Ӽ��� �����մϴ�.
                            }
                            else        // �Ӽ��� �̹� ����Ǿ� �ִٸ�
                                return; // ������ ���������̳� ���ҷ����� �������� �ʽ��ϴ�.

                            break;
                        case "���μ�" :
                            ((ItemWeapon)applyItem).BasePerformance *= draggingMisc.EngraveInfo.PerformMult;  // �⺻ ������ ������ ���� ������ŭ ������ŵ�ϴ�.
                            ((ItemWeapon)applyItem).Engrave(draggingItem.Name); // �巡��������� �����Ͽ� ���������� �ݿ��մϴ�.
                            break;
                    }


                    if( draggingMisc.InventoryCount == 1 )    // ������ ������ 1�����,
                    {
                        PlayerInven.instance.miscList.Remove(dragObj);   // ��ȭ���� �÷��̾� �κ��丮�� ��ȭ��Ͽ��� �����մϴ�.
                        Destroy(dragObj, 0.5f);                           // 0.5���� ���� ��ŵ�ϴ�.
                        dragObj.SetActive(false);                         // �������� �ٷ� disable ��ŵ�ϴ�.
                    }
                    else //2�� �̻��̶��,
                    {
                        draggingMisc.InventoryCount -= 1;       // ���� ������ ����.
                        dragObj.GetComponent<ItemInfo>().UpdateCountTxt();  // ��ø �ؽ�Ʈ�� �����Ѵ�.
                    }


                    return;     // ����ǰ� ����Ī�� ���� �ʴ´�.
                }
            }            
        }
        



        if ( transform.childCount==0 )   // ������ ����ִٸ�, �θ� �����ϰ�
        {
            dragObj.transform.SetParent( transform );         // �ش罽������ �θ� ����
            dragObj.transform.localPosition = Vector3.zero;   // ���߾� ��ġ
            draggingItem.SlotIndex = transform.GetSiblingIndex();    //�ٲ� ������ ��ġ�� �����Ѵ�.
        }
        else if( transform.childCount==1 ) // ���Կ� �������� �̹� �ִٸ�, �� �������� ��ġ�� ��ȯ�Ѵ�.
        {
            int prevIdx = Drag.prevParentTrByDrag.GetSiblingIndex(); //�巡�� ���� �������� ���� �θ�(����)�� �ε����� ����
            
            draggingItem.SlotIndex = transform.GetSiblingIndex();    //�巡������ �������� �ٲ� ���� ��ġ�� �����Ѵ�.
            dragObj.transform.SetParent( slotTr );                              // �巡�� ���� �������� �ش� �������� ��ġ
            dragObj.transform.localPosition = Vector3.zero;                     // ���߾� ��ġ

            Transform switchingItemTr = slotTr.GetChild(0);                     // �ٲ� ������ (���� �ڽ��� 2���� ��ø�Ǿ� �ִ�. �� �� 1��° ������ �ڽ�)
            switchingItemTr.GetComponent<ItemInfo>().Item.SlotIndex = prevIdx;  //����Ī�� �������� ���� ��ȣ�� ����ص� ��ġ�� ���� 
            switchingItemTr.SetParent( Drag.prevParentTrByDrag );               // �̹��� �켱���� ������ �θ� �Ͻ������� �ٲٹǷ�, ���� �θ� �޾ƿ´�.            
            switchingItemTr.localPosition = Vector3.zero;                       // ���߾� ��ġ
        }

    }

}
