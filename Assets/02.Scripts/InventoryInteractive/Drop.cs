using ItemData;
using System.Collections.Generic;
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
 */


public class Drop : MonoBehaviour, IDropHandler
{
    Transform slotTr;   // ���� �ڽ��� Ʈ������
    Dictionary<string, List<GameObject>> miscDic;
    void Start()
    {
        slotTr=GetComponent<Transform>();
        //�÷��̾� �κ��丮 �������� ��ȭ ��ųʸ��� �����մϴ�
        InventoryInfo playerInvenInfo = GameObject.FindWithTag("Player").GetComponent<InventoryInfo>();

        miscDic=playerInvenInfo.inventory.miscDic;
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
        if( draggingItem.Type==ItemType.Misc )
        {
            ItemMisc draggingMisc = ( (ItemMisc)draggingItem );

            if( draggingMisc.eMiscType==MiscType.Engraving||
                draggingMisc.eMiscType==MiscType.Enhancement||
                draggingMisc.eMiscType==MiscType.Attribute )
            {
                Item applyItem = slotTr.GetChild( 0 ).gameObject.GetComponent<ItemInfo>().Item;   //��ȭ�� ������ �������� ������ ���ϴ�.
                if( applyItem.Type==ItemType.Weapon )                 // ���� ����� ������ ��ȭ ������ �����ϰ� �ƴ϶�� ����Ī ������ �����մϴ�.
                {

                    switch( draggingItem.Name )
                    {
                        case "��ȭ��":
                            break;
                        case "�Ӽ���-��":
                            ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Water ); // �� �Ӽ��� �����մϴ�. 
                            break;
                        case "�Ӽ���-��":
                            ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Gold );  // �� �Ӽ��� �����մϴ�. 
                            break;
                        case "�Ӽ���-��":
                            ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Earth ); // �� �Ӽ��� �����մϴ�. 
                            break;
                        case "�Ӽ���-ȭ":
                            ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Fire );  // ȭ �Ӽ��� �����մϴ�. 
                            break;
                        case "�Ӽ���-ǳ":
                            ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Wind );  // ǳ �Ӽ��� �����մϴ�. 
                            break;
                        // �Ӽ����� ���濩�ο� ������� �Ҹ�Ǿ�� �մϴ�
                        case "���μ�":
                            ( (ItemWeapon)applyItem ).Engrave( draggingItem.Name ); // �巡��������� �����Ͽ� ���������� �ݿ��մϴ�.
                            break;
                    }


                    if( draggingMisc.OverlapCount==1 )    // ������ ������ 1�����,
                    {
                        miscDic[draggingItem.Name].Remove( dragObj );   // �������� �÷��̾� �κ��丮�� ��ȭ��Ͽ��� �����մϴ�.
                        Destroy( dragObj, 0.5f );                       // 0.5���� ���� ��ŵ�ϴ�.
                        dragObj.SetActive( false );                     // �������� �ٷ� disable ��ŵ�ϴ�.
                    }
                    else //2�� �̻��̶��,
                    {
                        draggingMisc.OverlapCount-=1;       // ���� ������ ����.
                        dragObj.GetComponent<ItemInfo>().UpdateCountTxt();  // ��ø �ؽ�Ʈ�� �����Ѵ�.
                    }


                    return;     // ����ǰ� ����Ī�� ���� �ʴ´�.
                }
            }
        }




        if( transform.childCount==0 )   // ������ ����ִٸ�, �θ� �����ϰ�
        {
            dragObj.transform.SetParent( transform );         // �ش罽������ �θ� ����
            dragObj.transform.localPosition=Vector3.zero;   // ���߾� ��ġ
            draggingItem.SlotIndex=transform.GetSiblingIndex();    //�ٲ� ������ ��ġ�� �����Ѵ�.
        }
        else if( transform.childCount==1 ) // ���Կ� �������� �̹� �ִٸ�, �� �������� ��ġ�� ��ȯ�Ѵ�.
        {
            int prevIdx = Drag.prevParentTrByDrag.GetSiblingIndex(); //�巡�� ���� �������� ���� �θ�(����)�� �ε����� ����

            draggingItem.SlotIndex=transform.GetSiblingIndex();    //�巡������ �������� �ٲ� ���� ��ġ�� �����Ѵ�.
            dragObj.transform.SetParent( slotTr );                              // �巡�� ���� �������� �ش� �������� ��ġ
            dragObj.transform.localPosition=Vector3.zero;                     // ���߾� ��ġ

            Transform switchingItemTr = slotTr.GetChild( 0 );                     // �ٲ� ������ (���� �ڽ��� 2���� ��ø�Ǿ� �ִ�. �� �� 1��° ������ �ڽ�)
            switchingItemTr.GetComponent<ItemInfo>().Item.SlotIndex=prevIdx;  //����Ī�� �������� ���� ��ȣ�� ����ص� ��ġ�� ���� 
            switchingItemTr.SetParent( Drag.prevParentTrByDrag );               // �̹��� �켱���� ������ �θ� �Ͻ������� �ٲٹǷ�, ���� �θ� �޾ƿ´�.            
            switchingItemTr.localPosition=Vector3.zero;                       // ���߾� ��ġ
        }

    }

}
