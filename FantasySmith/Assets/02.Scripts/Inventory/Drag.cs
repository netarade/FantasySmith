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
 */


/// <summary>
/// Drag��ũ��Ʈ�� ItemInfo ��ũ��Ʈ�� �Բ� ������ ������Ʈ�� ������Ʈ�� ������ �־�� �մϴ�.<br/>
/// ������ ������Ʈ�� �巡�׸� �����ϰ� ���ݴϴ�.
/// </summary>
public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform itemTr;                       // ������ �ݺ� ȣ�� �ؾ� �ϹǷ� �ڽ� transform ĳ��ó��
    public static GameObject draggingItem;          // ���� �巡�� ���� ������ ������Ʈ�� �������� ���ϴ�.
    private CanvasGroup canvasGroup;                // blockRaycast�� ���� ĵ���� �׷� ���

    private Transform inventoryTr;                  // ���԰� �������� ����ִ� �����θ� �κ��丮
    public static Transform prevParentTrByDrag;     // �ڽ��� �ű�� ������ �θ� ����

    void Start()
    {
        itemTr=this.transform;
        draggingItem = null;
        inventoryTr = GameObject.Find("Inventory").transform;                 

        if( inventoryTr==null )
            Debug.Log("Drag��ũ��Ʈ null ������ Ȯ�����ּ���.");
    }

    public void OnDrag( PointerEventData eventData )
    {
        itemTr.position=Input.mousePosition;                // �������� ��ġ�� ���콺�� ��ġ�� ��ġ��Ų��.

    }

    public void OnBeginDrag( PointerEventData eventData )
    {

        draggingItem=this.gameObject;                       // �巡�װ� ���۵Ǹ� �巡�� ���� ������ ������ �־�� �Ѵ�

        prevParentTrByDrag=itemTr.parent;                         // ���� �θ� ��ġ�� �����صд�.
        itemTr.SetParent( inventoryTr.transform );          // �θ� ���������� ��Ƽ� �̹��� �켱������ ���δ�.
        itemTr.GetComponent<Image>().raycastTarget=false;   // �巡�� �̺�Ʈ �̿ܿ��� ���� �ʴ´�.
    }

    public void OnEndDrag( PointerEventData eventData )
    {
        draggingItem=null;                                  // �巡�� ���� ������ ������ null�� �����.
        itemTr.GetComponent<Image>().raycastTarget=true;    // �巡�װ� ������ �ٽ� �̺�Ʈ�� �ް� �����.

        if( itemTr.parent==inventoryTr )                    //�������� �巡�� ���� �ʾ��� ��
        {
            itemTr.SetParent( prevParentTrByDrag );               // ������ �θ�� ������.
            itemTr.localPosition=Vector3.zero;              // ������ ���߾ӿ� �;��Ѵ�.
        }

    }

}
