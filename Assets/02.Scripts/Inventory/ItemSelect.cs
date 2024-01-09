
using System.Collections;
using System.Collections.Generic;
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
 * <v6.0 - 2024_0107_�ֿ���>
 * 1- ������ �巡�� ��Ŀ��� ����Ʈ ������� �����Ͽ�����, �׿����� ���ϸ��� ItemDrag���� ItemSelect�� �����ϰ�, ��ũ��Ʈ�� ���������Ͽ����ϴ�.
 * 
 * 2- ������ ����
 * prevSlotTr -> prevParentTr 
 * prevSlotListTr -> movingParentTr
 * 
 * 3- �ϳ��� �����۸� �����ϱ� ���Ͽ� isMyItemSelecting���º�����, interactiveŬ������ isItemSelecting ���� ���� ������ �ξ�����,
 * OnSelect�� �ߺ�ȣ���� �����ϱ� ���Ͽ� isFirstSelectDelay������ DelayTime �ξ Ȱ��ȭ �ñ⸦ ���߰�, 
 * OnUpdateSelected�� OnSelect�� �ߺ�ȣ���� �����ϱ� ���Ͽ� isMyItemSelecting�� DelayTime�� �ξ ��Ȱ��ȭ�ñ⸦ ���߾���.
 * 
 * 
 */


/// <summary>
/// Drag��ũ��Ʈ�� ItemInfo ��ũ��Ʈ�� �Բ� ������ 2D ������Ʈ�� ������Ʈ�� ������ �־�� �մϴ�.<br/>
/// ������ ������Ʈ�� �巡�׸� �����ϰ� ���ݴϴ�.
/// </summary>
public class ItemSelect : MonoBehaviour
    //, IDragHandler, IBeginDragHandler, IEndDragHandler            // �巡�� ���
    , IUpdateSelectedHandler, ISelectHandler, IDeselectHandler      // ����Ʈ ���
{
    private RectTransform itemRectTr;      // �ڽ� transform ĳ��
    private CanvasGroup itemCG;            // �ڽ��� ĵ���� �׷� ĳ��
    private ItemInfo itemInfo;             // �ڽ��� ItemInfo ĳ��
    
    public Transform prevParentTr;          // �������� �ű�� ������ �θ� (����)
    public Transform movingParentTr;        // �������� �ű�� ������ �θ� (ĵ����)

    public Vector3 moveVecToCenter;
        
    GraphicRaycaster gRaycaster;                // �κ��丮 ĵ������ �׷��ȷ���ĳ����
    PointerEventData pEventData;                // �׷��� ����ĳ���� �� ���� �� ������ �̺�Ʈ
    List<RaycastResult> raycastResults;         // �׷��� ����ĳ���� ����� ���� ����Ʈ
    

    InventoryInteractive inventoryInteractive;  // ��ü ������ ������ ���� ������ ���� ���ͷ�Ƽ�� ��ũ��Ʈ ����
    bool isMyItemSelecting = false;             // ���� �������� ���� ������ ����
    bool isFirstSelectDelay = false;            // ó�� ������ �� ������ �ð��� �������� ����
    Button itemSelectBtn;                       // ��ư�� ����Ʈ�� �����ϱ� ���� ����
    string strItemDropSpace = "ItemDropSpace";


    void Start()
    {
        itemRectTr = GetComponent<RectTransform>();     
        itemCG = GetComponent<CanvasGroup>();     
        itemInfo = GetComponent<ItemInfo>();
        itemSelectBtn = GetComponent<Button>(); 

        raycastResults = new List<RaycastResult>();
        pEventData = new PointerEventData(EventSystem.current);
    }



    public void OnUpdateSelected( BaseEventData eventData )
    {
        // �� ������ ���û��°� �ƴ϶�� �������� �ʽ��ϴ�. (������ 1���� �����մϴ�.)
        if( !isMyItemSelecting )
            return;
            

        // ������ ��ġ�� ���콺 ��ġ�� ��ġ�ø� ������ ���콺 ��ġ�� �������Ƿ�, �ٽ� ������ġ�� ������ �ش� ��ġ�� �������� �����̵��� ���ݴϴ�.
        itemRectTr.position = Input.mousePosition + moveVecToCenter;  

        // ���� �߿� ���콺 ��ư�� Ŭ���� ��� - �ݵ�� ���� ����
        if( Input.GetMouseButton(0) && isFirstSelectDelay )
        {
            print("��ư�� �����ϴ�.");
            itemSelectBtn.OnDeselect( eventData );              // ��ư�� Deselect���·� ����ϴ�.
            EventSystem.current.SetSelectedGameObject( null );  // �̺�Ʈ �ý����� ����Ʈ ���¸� null�� ����ϴ�.
            
            isMyItemSelecting = false;                            // �� ������ ���� ���¸� �ٷ� ��Ȱ��ȭ�մϴ�.
            isFirstSelectDelay = false;                              // ó�� Ŭ�� ���¸� ��Ȱ��ȭ �մϴ�.


            // �̺�Ʈ�� �Ͼ �������� ���콺�� �ٽ� Ŭ������ ���� �������� �����մϴ�.
            pEventData.position=Input.mousePosition;

            // �׷��� ����ĳ��Ʈ�� ���� ����� �޽��ϴ�.
            gRaycaster.Raycast( pEventData, raycastResults );

            print("����ĳ������ �����մϴ�.");


            // ����ĳ���ÿ� ������ ���(������ ������Ʈ�� �ִ� ���)
            if( raycastResults.Count>0 )
            {
                for( int i = 0; i<raycastResults.Count; i++ )
                {
                    // ������ ������Ʈ�� �±װ� �����̶��,
                    if( raycastResults[i].gameObject.tag==strItemDropSpace )
                    {
                        print("��� �޼��带 ȣ���մϴ�.");
                        itemInfo.OnItemSlotDrop( raycastResults[i].gameObject.transform );
                    }
                    // ������ ������Ʈ�� �±װ� ������ �ƴ϶��, �ٽ� ����ġ�� �����ݴϴ�.
                    else
                        itemInfo.UpdatePositionInSlotList();    
                }
                
                // ������ ������ ���¸� ������ �ð��� �༭ ��Ȱ��ȭ��ŵ�ϴ�.
                StartCoroutine( SelectDoneDelayTime( 0.03f ) );
            }
            // ����ĳ������ ������ �����鼭, �θ��� �̵��� �߻����� �ʾҴٸ�,(������ ����� �����ߴٸ�)
            else if( raycastResults.Count==0 && itemRectTr.parent==movingParentTr )          
            {
                // ������ ������ ���¸� ������ �ð��� ���� �ʰ� ��Ȱ��ȭ��ŵ�ϴ�.
                StartCoroutine( SelectDoneDelayTime( 0f ) );
                
                // �κ��丮���� ������ ���
                itemInfo.OnItemWorldDrop();
                

                
            }
            else
                throw new System.Exception("��� �̻��� �߻��Ͽ����ϴ�. Ȯ���Ͽ� �ּ���.");

        }
        else if( Input.GetMouseButton( 0 ) )
        {
            print( "���� Ȱ��ȭ���� �ʾҽ��ϴ�" );
            itemRectTr.localPosition = Vector3.zero;
        }




    }



    public void OnSelect( BaseEventData eventData )
    {        
        // Select�� �����ϸ� ���� �������� �κ��丮 ������ �ֽ�ȭ�Ͽ� �����ɴϴ�. 
        inventoryInteractive = itemInfo.InventoryInfo.gameObject.GetComponent<InventoryInteractive>();

        // ������ �������� �ϳ��� Ȱ��ȭ�Ǿ� �ִٸ� �ٸ� �������� �������� ������ �����մϴ�.
        // �ڽ��� �������� �������̰ų�, �������� �Ϸ���� �ʾҴٸ� �������� �ʽ��ϴ�.
        if( inventoryInteractive.IsItemSelecting && isMyItemSelecting )
            return;
                
        inventoryInteractive.IsItemSelecting = true;    // ��ü �����ۿ� �����ϴ� ���� ���¸� Ȱ��ȭ �մϴ�.
        isMyItemSelecting = true;                         // �� ������ ���� ���� ���¸� Ȱ��ȭ �մϴ�.
                
        StartCoroutine( FirstSelectDelayTime(0.1f) );   // OnUpdateSelected ���� ȣ���� �����ϱ� ���� �����̸� �༭ ���º����� Ȱ��ȭ��ŵ�ϴ�.
                
        
        print("Ŭ���� �����մϴ�.");

        // ���� �θ� ��ġ (���Ը���Ʈ�� ���� ����)�� �����մϴ�.
        prevParentTr=itemRectTr.parent;
        movingParentTr = inventoryInteractive.gameObject.transform.parent;      // �κ��丮�� �θ� ĵ���� ����

        itemRectTr.SetParent( movingParentTr );                 // �θ� �Ͻ������� �κ��丮�� �θ��� ĵ������ ��Ƽ� �̹��� �켱������ ���Դϴ�.
        itemCG.blocksRaycasts=false;                            // �巡�� �̺�Ʈ �̿ܿ��� ���� �ʽ��ϴ�.

        // ���� �̵� ���͸� ���մϴ�.
        // (���� �κ��丮 ���� ��ġ-���콺 �̺�Ʈ�� �߻��� ��ġ => ���콺 �̺�Ʈ ��ġ���� �κ��丮 �������� �̵��� �� �ִ� �̵�����)
        moveVecToCenter=itemRectTr.position-Input.mousePosition;
                
        // ������������ �ִ� �ش� �κ��丮�� �ҷ��� ���� �θ� Canvas�� �����Ͽ� �׷��ȷ���ĳ���͸� �����մϴ�.
        gRaycaster=itemInfo.InventoryInfo.transform.parent.GetComponent<GraphicRaycaster>();

        // ����ĳ��Ʈ �������Ʈ�� �ʱ�ȭ�մϴ�.
        raycastResults.Clear();
    }


    public void OnDeselect( BaseEventData eventData )
    {        
        
    }

    
    IEnumerator FirstSelectDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        isFirstSelectDelay = true;
    }

    IEnumerator SelectDoneDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        inventoryInteractive.IsItemSelecting = false;
        itemCG.blocksRaycasts=true;                         // �巡�װ� ������ �ٽ� �̺�Ʈ�� �ް� �����.
        print("��ü ������ ���� ȣ��");
    }
    










    
    //public void OnBeginDrag( PointerEventData eventData )
    //{                                                                
    //    prevSlotTr = itemRectTr.parent;             // ���� ���԰� ���Ը���Ʈ�� ����Ѵ�.
    //    prevSlotListTr = prevSlotTr.parent;

    //    // ���� �θ� ��ġ(���� ����)�� �����صд�.
    //    itemRectTr.SetParent( prevSlotListTr );     // �θ� ���������� ��Ƽ� �̹��� �켱������ ���δ�.
    //    itemCG.blocksRaycasts=false;                // �巡�� �̺�Ʈ �̿ܿ��� ���� �ʴ´�.
    //}
        
    //public void OnDrag( PointerEventData eventData )
    //{
    //    itemRectTr.position = Input.mousePosition;                // ������ 2D�� ��ġ�� ���콺�� ��ġ�� ��ġ��Ų��.        
    //}

    //public void OnEndDrag( PointerEventData eventData )
    //{                                    

    //    // ������ �����ɽ�Ʈ�� UI ������ ���� �ʾҴٸ�, (�κ��丮 ������ ���´ٸ�,)
    //    if( eventData.pointerCurrentRaycast.gameObject == null ) 
    //    {
    //        Debug.Log("�������� ������ �����ðڽ��ϱ�?");
    //        //itemInfo.InventoryInfo.RemoveItem(itemInfo.name);
    //    }    
        
    //    if( itemRectTr.parent == prevSlotListTr )  // ������ ����� �������� ��
    //    {
    //        itemRectTr.SetParent( prevSlotTr );         // ������ �θ� �������� ������.
    //        itemRectTr.localPosition = Vector3.zero;    // ������ ���߾ӿ� �;��Ѵ�.
    //    }

    //    itemCG.blocksRaycasts=true;                     // �巡�װ� ������ �ٽ� �̺�Ʈ�� �ް� �����.

    //}



}
