//using UnityEngine;
//using UnityEngine.EventSystems; // ���콺 Ŭ��, �巡�� ���, �̺�Ʈ ���� ����

///* [�۾� ����]
// * <v1.0 - 2023_1103_�ֿ���>
// * �ʱ� ���� �ۼ�
// * 
// */


///// <summary>
///// Drag��ũ��Ʈ�� ItemInfo ��ũ��Ʈ�� �Բ� ������ ������Ʈ�� ������Ʈ�� ������ �־�� �մϴ�.<br/>
///// ������ ������Ʈ�� �巡�׸� �����ϰ� ���ݴϴ�.
///// </summary>
//public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
//{
//    private Transform itemTr;                       // ������ �ݺ� ȣ�� �ؾ� �ϹǷ� �ڽ� transform ĳ��ó��
//    private Transform inventoryTr;                  // �ǳ�-�κ��丮�� transform�� �����մϴ�. ������ ���� ����Ʈ�� �־�� �մϴ�.
//    public  GameObject draggingItem = null;         // ���� �巡�� ���� ������ ������Ʈ�� �������� ���ϴ�.
    
//    //2
//    private Transform slotListTr;                   // ���� ����Ʈ�� transform�� �����մϴ�.
//    private CanvasGroup canvasGroup;                // 

//    void Start()
//    {
//        itemTr = this.transform;   
//        inventoryTr = GameObject.Find("Inventory").transform;


//        //2
//        slotListTr = GameObject.Find("ItemList").transform;
//        canvasGroup = GetComponent<CanvasGroup>();
//    }

//    // �̺�Ʈ �����͸� �޴� Ŭ����. 
//    // ���� ���� ���� ��Ʈ��,�ڵ鷯 Ŭ�������ְ�
//    // ������ ����ϴ� ����Ƽ Ŭ������ �ִ�.
//    // �̺�Ʈ ������ �޾ƿ´�.
//    public void OnDrag( PointerEventData eventData )
//    { 
//        itemTr.position = Input.mousePosition;
//    }

//    public void OnBeginDrag( PointerEventData eventData )
//    {                 
//        draggingItem = this.gameObject; // �巡�װ� ���۵Ǹ� �巡�� ���� ������ ������ �־�� �Ѵ�
//        canvasGroup.blocksRaycasts = true;
//    }

//    public void OnEndDrag( PointerEventData eventData )
//    {
//        draggingItem = null;            // �巡�� ���� ������ ������ null�� �����.

//        //�������� �巡�� ���� �ʾ��� �� ������� ItemList�� ������.
//        if(itemTr.parent == inventoryTr)
//        {
//            itemTr.SetParent(slotListTr.transform, false);

//            //2023-1010
//            // GameManager.instance.RemoveItem(GetComponent<Item>());
//            GameManager.instance.RemoveItem(GetComponent<ItemInfo>().itemData);

//        }
//        //�θ� �κ��丮�ε� ���ٰ� ��� �ٽ� ���ư��� �Ѵ�.

//    }
//}
