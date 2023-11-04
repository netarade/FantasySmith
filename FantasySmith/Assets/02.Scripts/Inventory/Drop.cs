
using UnityEngine;
using UnityEngine.EventSystems;

/* [�۾� ����]
 * <v1.0 - 2023_1105_�ֿ���>
 * 1- d�ʱ� ���� �ۼ�, ������ ����Ī ����
 * 
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
        if( transform.childCount==0 )   // ������ ����ִٸ�, �θ� �����ϰ�
        {
            Drag.draggingItem.transform.SetParent( transform );         // �ش罽������ �θ� ����
            Drag.draggingItem.transform.localPosition = Vector3.zero;   // ���߾� ��ġ
        }
        else if( transform.childCount==1 ) // ���Կ� �������� �̹� �ִٸ�, �� �������� ��ġ�� ��ȯ�Ѵ�.
        {
            Drag.draggingItem.transform.SetParent( slotTr );            // �巡�� ���� �������� �ش� �������� ��ġ
            Drag.draggingItem.transform.localPosition = Vector3.zero;   // ���߾� ��ġ 

            Transform switchingItemTr = slotTr.GetChild(0);          // �ٲ� ������
            switchingItemTr.SetParent( Drag.prevParentTrByDrag );    // �̹��� �켱���� ������ �θ� �Ͻ������� �ٲٹǷ�, ���� �θ� �޾ƿ´�.            
            switchingItemTr.localPosition = Vector3.zero;            // ���߾� ��ġ
        }

    }

}
