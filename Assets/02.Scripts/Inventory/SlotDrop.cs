using ItemData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/* [�۾� ����]
 * <v1.0 - 2023_1105_�ֿ���>
 * 1- �ʱ� ���� �ۼ�, ������ ����Ī ����
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
 * 
 * <v3.0 - 2023_1216_�ֿ���>
 * 1- �Ӽ����� 5���� �������� ������ �̸��� ���� �Ӽ����� ���� �б� ó��, ������ �̷����� ������ ���� �����ǵ��� ����
 * 2- ������ ���� 1�϶� ��ȭ ����Ʈ���� �����ϴ� ������ ��ȭ ��ųʸ��� �����Ͽ� �����ϴ� �������� ����
 * 
 * <v3.1 - 2023_1217_�ֿ���>
 * 1- miscDic�� PlayerInvenŬ�������� �̱��濡 �����ؼ� �ʱ�ȭ �ϴ� ���� �ν��Ͻ� �����Ͽ� �ʱ�ȭ �ϴ� ������ ����
 * (�̱����� ����ȯ�� �ʱ�ȭ�� ������ ����)
 * 2- switch���� ��ȭ���� ���η����� �ִ����� ����
 * 3- Drop��ũ��Ʈ���� �Ӽ����̳� ���μ��� ������ �� ���� BasePerformance�� �����ϴ� ���� ����
 * 
 * <v3.2 - 2023_1222_�ֿ���>
 * 1- playerInvenInfo�� �߸��� ���� ����
 * �ܼ� GetComponent<InventoryInfo>()���� GameObject.FindWithTag("Player").GetComponent<InventoryInfo>()���� ����
 * 
 * <v4.0 - 2023_1224_�ֿ���>
 * 1- dragObj�� null ���۷����� ���� return�� �߰��Ͽ�
 * �巡�� ���� ������ ������Ʈ�� ���� �� Drop �̺�Ʈ�� ���� ������ ����
 * 
 * <v5.0 - 2023_1227_�ֿ���>
 * 1- ������ �������� �������� ����(3D������Ʈ���� 2D������Ʈ �δ� ���) �����ϴ� dragObj�� dragObj3D�� dragObj2D ������ ����.
 * 2- OnDrop���� �����ϴ� �������� Drop Ŭ���� ������ ������ġ�� ����
 * 
 * <v5.1 - 2023_1228_�ֿ���>
 * 1- �ٽ� ������ �������� �������� ����(3D������Ʈ, 2D������Ʈ ����Ī ���) �ڵ带 2D�������� ����
 * 
 * <v5.2 - 2023_1229_�ֿ���>
 * 1- Ŭ������ ���ϸ��� Drop->SlotDrop 
 * 2- �ʿ���� ���� ���� ����
 * 
 * <v5.3 - 2023_1231_�ֿ���>
 * 1- ��������κ��� OverlapCount�� ���� �����ϴ� �κп��� SetOverlapCount�޼���� ��ü
 * 2- playerInvenInfo -> invenInfo�� ������ ����, Find�� �����ϴ� �κ��� �������� ������ ����
 * 
 * <v6.0 -2024_0104_�ֿ���>
 * 1- ��� �̺�Ʈ�� �Ͼ �� �������� ��ġ�� �Ű��ִ� ���� ������ �ּ�ó���ϰ�, 
 * �������ʿ��� �κ��丮�� ��û�ؼ� �ε����� �ο��޴� ���·� ������ ������. 
 * ������ �����ʿ����� Ȱ��ȭ �ǿ� ���� �ε����� �ο��� �� �ֱ� �����ε�, 
 * �̴� ���� �κ��丮 ��������� ��������� �ٸ� �κ��丮�� ���Կ��� �Űܿ� ���� �ݵ�� �ѹ��� ��ü���ε����� �ο��޾ƾ� �Ѵ�.
 * �̴� �κ��丮�ʿ��� �� ������ ��û�ؾ� �ϹǷ�, ������ �κ��丮���� �����ۿ� �־��ִ� �ͺ��� �������� �ٷ� ������ �θ��� �κ��丮�� ��û�ؼ� �ٷ� ���޹ް�
 * �ڽ��� �����Ǿ�����Ʈ�� �ϴ� ���� ���ݴ� ������ ������ ����.
 * => ���� ����� �Ͼ �� �ش� �������� OnItemSlotDrop�� �ڽ��� ���� �������� ���ڸ� �����ϸ� �ǵ��� �����ϰ� �Ǿ���.
 * 
 * 
 * [���� �����ؾ��� �̽�����]
 * 1- playerInvenInfo�� �±� ����� �ƴ϶� ��������������� ���� (��Ƽ�÷��̾� ����)
 * 2- ����1���� �Ǿ����� ������Ʈ�� �ٷ��ı����� �ʰ� 0.5���� �ı��ߴ� �ڵ带 ���� ������ ����� �ȳ�. �ٷ��ı��ϴ� �������� �ٲ㺼 ��
 * 
 * 3- ��� �� Ȱ��ȭ ���� �ǿ� ���� �����ε����� �����ؾ� �Ѵ�. 
 * 
 */


public class SlotDrop : MonoBehaviour, IDropHandler
{
    Transform slotTr;                              // ���� �ڽ��� Ʈ������

    Dictionary<string, List<GameObject>> miscDic;  // �÷��̾� �κ��丮�� ��ȭ ����

    Item applyItem;                                // ��ȭ�� ������ ������
    RectTransform switchingItemRectTr;             // ������� �� ��ġ�� �ٲ� �������� ���� 2D Transform


    void Start()
    {
        slotTr = GetComponent<Transform>();

        //�÷��̾� �κ��丮 �������� ��ȭ ��ųʸ��� �����մϴ�
        InventoryInfo invenInfo = slotTr.parent.parent.parent.parent.GetComponent<InventoryInfo>();

        miscDic = invenInfo.inventory.miscDic;
    }

    /// <summary>
    /// OnDrop�̺�Ʈ ������ OnDragEnd �̺�Ʈ�� ȣ��ǹǷ�, ������ ����Ī ó���� �����մϴ�.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop( PointerEventData eventData )
    {
        GameObject dropItemObj = eventData.pointerCurrentRaycast.gameObject;   // �̺�Ʈ�� ���޹��� 2D ������Ʈ ����

        if(dropItemObj == null)                       // �巡�� ���� ������Ʈ�� ���� ��� ���� ������ �������� ���� 
            return;

        ItemInfo dropItemInfo = dropItemObj.GetComponent<ItemInfo>();    // ��� �� ������ ���� ��ũ��Ʈ
        Item droItem = dropItemObj.GetComponent<ItemInfo>().Item;       // ��� �� ������ ����




        dropItemInfo.OnItemSlotDrop(slotTr);
        























        //// �巡�� ���� �������� �Ӽ����̳� ��ȭ��, ���μ��̶��, �����ϴ� ������.
        //if( droItem.Type==ItemType.Misc )
        //{
        //    ItemMisc dropItemMisc = ( (ItemMisc)droItem );      // ���� �巡�� ���� ��ȭ �������� ����

        //    if( dropItemMisc.MiscType==MiscType.Engraving||
        //        dropItemMisc.MiscType==MiscType.Enhancement||
        //        dropItemMisc.MiscType==MiscType.Attribute )
        //    {
        //        applyItem = slotTr.GetChild(0).gameObject.GetComponent<ItemInfo>().Item;   //��ȭ�� ������ �������� ������ ���ϴ�.
                
        //        if( applyItem.Type==ItemType.Weapon )                 // ���� ����� ������ ��ȭ ������ �����ϰ� �ƴ϶�� ����Ī ������ �����մϴ�.
        //        {
        //            switch( droItem.Name )
        //            {
        //                case "��ȭ��":
        //                    break;
        //                case "�Ӽ���-��":
        //                    ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Water ); // �� �Ӽ��� �����մϴ�. 
        //                    break;
        //                case "�Ӽ���-��":
        //                    ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Gold );  // �� �Ӽ��� �����մϴ�. 
        //                    break;
        //                case "�Ӽ���-��":
        //                    ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Earth ); // �� �Ӽ��� �����մϴ�. 
        //                    break;
        //                case "�Ӽ���-ȭ":
        //                    ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Fire );  // ȭ �Ӽ��� �����մϴ�. 
        //                    break;
        //                case "�Ӽ���-ǳ":
        //                    ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Wind );  // ǳ �Ӽ��� �����մϴ�. 
        //                    break;
        //                // �Ӽ����� ���濩�ο� ������� �Ҹ�Ǿ�� �մϴ�
        //                case "���μ�":
        //                    ( (ItemWeapon)applyItem ).Engrave( droItem.Name ); // �巡��������� �����Ͽ� ���������� �ݿ��մϴ�.
        //                    break;
        //            }


        //            if( dropItemMisc.OverlapCount==1 )    // ������ ������ 1�����,
        //            {
        //                miscDic[droItem.Name].Remove( dropItemObj );   // �������� �÷��̾� �κ��丮�� ��ȭ��Ͽ��� �����մϴ�.
        //                Destroy( dropItemObj, 0.5f );                       // 0.5���� ���� ��ŵ�ϴ�.
        //                dropItemObj.SetActive( false );                     // �������� �ٷ� disable ��ŵ�ϴ�.
        //            }
        //            else //2�� �̻��̶��,
        //            {
        //                dropItemMisc.SetOverlapCount(-1);       // ���� ������ ����.
        //                dropItemObj.GetComponent<ItemInfo>().UpdateCountTxt();  // ��ø �ؽ�Ʈ�� �����Ѵ�.
        //            }


        //            return;     // ����ǰ� ����Ī�� ���� �ʴ´�.
        //        }
        //    }
        //}




        //if( slotTr.childCount==0 )   // ������ ����ִٸ�, �θ� �����ϰ�
        //{
        //    dropItemObj.transform.SetParent( slotTr );                    // �ش罽������ �θ� ����
        //    dropItemObj.transform.localPosition = Vector3.zero;           // ���߾� ��ġ
        //    droItem.SlotIndex = slotTr.GetSiblingIndex();          // �ٲ� ������ ��ġ�� �����Ѵ�.
        //}
        //else if( slotTr.childCount==1 ) // ���Կ� �������� �̹� �ִٸ�, �� �������� ��ġ�� ��ȯ�Ѵ�.
        //{
        //    int prevIdx = dropItemObj.GetComponent<ItemDrag>().prevSlotTr.GetSiblingIndex(); // �巡�� ���� �������� ���� �θ�(����)�� �ε����� ����

        //    droItem.SlotIndex = slotTr.GetSiblingIndex();      // �巡������ �������� �ٲ� ���� ��ġ�� �����Ѵ�.
        //    dropItemObj.transform.SetParent( slotTr );                // �巡�� ���� �������� �ش� �������� ��ġ
        //    dropItemObj.transform.localPosition = Vector3.zero;       // ���߾� ��ġ


        //    // �ٲ� ������ ����               
        //    switchingItemRectTr = slotTr.GetChild(0).GetComponent<RectTransform>();

        //    switchingItemRectTr.GetComponent<ItemInfo>().Item.SlotIndex=prevIdx;  // ����Ī�� �������� ���� ��ȣ�� ����ص� ��ġ�� ���� 
        //    switchingItemRectTr.SetParent( ItemDrag.prevSlotTr );             // �̹��� �켱���� ������ �θ� �Ͻ������� �ٲٹǷ�, ���� �θ� �޾ƿ´�.            
        //    switchingItemRectTr.localPosition=Vector3.zero;                       // ���߾� ��ġ
        //}

    }

}
