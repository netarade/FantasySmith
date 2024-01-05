using UnityEngine;
using UnityEngine.EventSystems; // ���콺 Ŭ��, �巡�� ���, �̺�Ʈ ���� ����
using UnityEngine.UI;


/* [�۾� ����]
 * <v1.0 - 2023_1103_�ֿ���>
 * 1- �ʱ� ���� �ۼ�
 * 
 * <v2.0 - 2023_1005_�ֿ���>
 * 1- �巡�� �� �̹����� ������ �ʴ� ���� - ĵ���� �켱���� ���� �ذ�
 * �������� �θ� ������ �Ͻ������� ������ �ΰ� ����.
 * 
 * 2- ������ ������ �ξ��� �� ����̺�Ʈ �߻����� �ʴ� ���� �ذ�
 * �������� rayCastTarget�� �巡�� �����ϸ� off���Ѽ�
 * ������ ��ü�� ����̺�Ʈ�� ���ƾ� �ڿ� �ִ� ���Կ��Է� ����� �� ����.
 * 
 * <v3.0 - 2023_1224_�ֿ���>
 * 1- dragginItem�� draggingObj�� �̸�����
 * 
 * <v4.0 - 2023_1227_�ֿ���>
 * 1- ������ ������ �������� ����(Transform ������Ʈ �ֻ���, RectTransform������Ʈ �����ڽ�)���� ����
 * itemTr�� �̸��� itemRectTr�� ����, dragginObj�� draggingObj2D�� �����ϰ� ���� ����
 * 
 * <v4.1 - 2023_1228_�ֿ���>
 * 1- ������ ������ �������� ����(3D������Ʈ, 2D������Ʈ ��ȯ����)���� ����
 * �ڵ带 2D Ʈ�������������� �ٽ� ����
 * 
 * 
 * <v4.2 - 2023_1229_�ֿ���>
 * 1- Ŭ���� �� ���ϸ��� Drag->ItemDrag
 * 
 * <v5.0 - 2023_1230_�ֿ���>
 * 1- prevParentTrByDrag�� prevSlotTr�� ������ ���� �� static�������� �Ϲݺ����� ����
 * 
 * 2- static ������ draggingObj2D, draggingObj3D ���� 
 * - Drop���� ����ϰ� �־����� ��Ƽ �������� ���������� �߻��Ұ��̱� ������ Drop���� �߻��� eventData�� ���� �޵��� ����
 * 
 * 3- inventoryTr�� �����ϰ�, prevSlotListTr�� ����
 * 
 * 4- OnEndDrag �޼��� ����ȭ
 * 
 */


/// <summary>
/// Drag��ũ��Ʈ�� ItemInfo ��ũ��Ʈ�� �Բ� ������ 2D ������Ʈ�� ������Ʈ�� ������ �־�� �մϴ�.<br/>
/// ������ ������Ʈ�� �巡�׸� �����ϰ� ���ݴϴ�.
/// </summary>
public class ItemDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform itemRectTr;      // �ڽ� transform ĳ��
    private CanvasGroup itemCG;            // �ڽ��� ĵ���� �׷� ĳ��
    private ItemInfo itemInfo;             // �ڽ��� ItemInfo ĳ��
    
    public Transform prevSlotListTr;                // �ڽ��� �ű�� ������ ������ ������Ʈ�� ���� ���Ը���Ʈ
    public Transform prevSlotTr;                    // �ڽ��� �ű�� ������ �θ� ����                       (Drop��ũ��Ʈ���� ����)

    void Start()
    {
        itemRectTr = GetComponent<RectTransform>();     
        itemCG = GetComponent<CanvasGroup>();     
        itemInfo = GetComponent<ItemInfo>();
    }


    public void OnBeginDrag( PointerEventData eventData )
    {                                                                
        prevSlotTr = itemRectTr.parent;             // ���� ���԰� ���Ը���Ʈ�� ����Ѵ�.
        prevSlotListTr = prevSlotTr.parent;

        // ���� �θ� ��ġ(���� ����)�� �����صд�.
        itemRectTr.SetParent( prevSlotListTr );     // �θ� ���������� ��Ƽ� �̹��� �켱������ ���δ�.
        itemCG.blocksRaycasts=false;                // �巡�� �̺�Ʈ �̿ܿ��� ���� �ʴ´�.
    }
        
    public void OnDrag( PointerEventData eventData )
    {
        itemRectTr.position = Input.mousePosition;                // ������ 2D�� ��ġ�� ���콺�� ��ġ�� ��ġ��Ų��.        
    }

    public void OnEndDrag( PointerEventData eventData )
    {                                    

        // ������ �����ɽ�Ʈ�� UI ������ ���� �ʾҴٸ�, (�κ��丮 ������ ���´ٸ�,)
        if( eventData.pointerCurrentRaycast.gameObject == null ) 
        {
            Debug.Log("�������� ������ �����ðڽ��ϱ�?");
            //itemInfo.InventoryInfo.RemoveItem(itemInfo.name);
        }    
        
        if( itemRectTr.parent == prevSlotListTr )  // ������ ����� �������� ��
        {
            itemRectTr.SetParent( prevSlotTr );         // ������ �θ� �������� ������.
            itemRectTr.localPosition = Vector3.zero;    // ������ ���߾ӿ� �;��Ѵ�.
        }

        itemCG.blocksRaycasts=true;                     // �巡�װ� ������ �ٽ� �̺�Ʈ�� �ް� �����.

    }

}
