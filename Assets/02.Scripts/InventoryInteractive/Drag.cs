using Unity.VisualScripting;
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
 */


/// <summary>
/// Drag��ũ��Ʈ�� ItemInfo ��ũ��Ʈ�� �Բ� ������ 2D ������Ʈ�� ������Ʈ�� ������ �־�� �մϴ�.<br/>
/// ������ ������Ʈ�� �巡�׸� �����ϰ� ���ݴϴ�.
/// </summary>
public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform itemRectTr;               // ������ �ݺ� ȣ�� �ؾ� �ϹǷ� �ڽ� transform ĳ��ó��
    private Transform itemTr;                       // �������� ���� �ֻ��� �θ� ������Ʈ�� Transform

    public static GameObject draggingObj2D;         // ���� �巡�� ���� ������ 2D ������Ʈ�� �������� ���ϴ�.
    public static GameObject draggingObj3D;         // ���� �巡�� ���� ������ 3D ������Ʈ�� �������� ���ϴ�.

    private Transform inventoryTr;                  // ���԰� �������� ����ִ� �����θ� �κ��丮
    public static Transform prevParentTrByDrag;     // �ڽ��� �ű�� ������ �θ� ����

    void Start()
    {
        itemRectTr = transform.GetComponent<RectTransform>(); // �������� �������� ������ RectTransform ���� ����
        itemTr = transform.parent;                            // �������� �������� �ֻ��� ������Ʈ Transform ���� ����

        draggingObj2D = null;
        draggingObj3D = null;

        Transform canvasTr = GameObject.FindWithTag("CANVAS_CHARACTER").transform;
        inventoryTr = canvasTr.GetChild(0);                     // �κ��丮�� ĳ���� ĵ������ 0��° �ڽ�             
    }

    public void OnDrag( PointerEventData eventData )
    {
        itemRectTr.position=Input.mousePosition;                // ������ 2D�� ��ġ�� ���콺�� ��ġ�� ��ġ��Ų��.

    }

    public void OnBeginDrag( PointerEventData eventData )
    {

        draggingObj2D = transform.gameObject;                   // �巡�װ� ���۵Ǹ� �巡�� ���� ������ ������ �־�� �Ѵ�
        draggingObj3D = transform.parent.gameObject;
                                                                

        prevParentTrByDrag = itemTr.parent;                     // ���� �θ� ��ġ(���� ����)�� �����صд�.
        itemTr.SetParent( inventoryTr.transform );              // �θ� ���������� ��Ƽ� �̹��� �켱������ ���δ�.
        itemRectTr.GetComponent<Image>().raycastTarget=false;   // �巡�� �̺�Ʈ �̿ܿ��� ���� �ʴ´�.
    }

    public void OnEndDrag( PointerEventData eventData )
    {
        draggingObj2D = null;                                   // �巡�� ���� ������ ������ null�� �����.
        draggingObj3D = null;

        itemRectTr.GetComponent<Image>().raycastTarget=true;    // �巡�װ� ������ �ٽ� �̺�Ʈ�� �ް� �����.
       
        if( itemTr.parent == inventoryTr )                      // �������� �巡�� ���� �ʾ��� ��
        {
            itemTr.SetParent( prevParentTrByDrag );             // ������ �θ�� ������.
            itemRectTr.localPosition = Vector3.zero;            // ������ ���߾ӿ� �;��Ѵ�.
        }

    }

}
