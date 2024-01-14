
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
 * => 
 * ���� ��ġ ���� �� ���� Select�� UpdateSelected ȣ���� �����ؾ� �ϸ�,
 * OnUpdateSelected���� Ŭ����ư�� ������ �ݵ�� Select�� �����Ű�� EventSystem�� Select���¸� null�� ������
 * �ٸ� �������� Select�� �ߵ���ų �� �ִ�.
 * (�̴� ���� �� ��� �ִ� ���� �������� �� ���� ������ �Ǵµ� �̰��� �ٽ� ����Ʈ�� �̾����� ����)
 * 
 * <v6.1 - 2024_0112_�ֿ���>
 * 1- OnSelectUpdated�� �ִ� ����Ʈ�� ����� ��쿡 ó���ϴ� ������ Deselect�� �ű�.
 * 
 * <v6.2 - 2024_0114_�ֿ���>
 * 1- gRaycaster�� �����ϰ� �б����� InventoryInfo����Ʈ ������ clientList�� �����Ͽ�
 * �������� ���� �� �ش� clieentList�� �ִ� ��� �׷��ȷ���ĳ������ ����ĳ������ ȣ���ϵ��� �Ͽ���
 * 
 * 2- isInventoryConnect���� �����Ͽ� �������Ҷ����� Ȯ���Ͽ� 
 * ���� ������ �� �����±װ� ������� �ʴ´ٸ� ������ ���� ����� ����
 * 
 * 3- IEnumerator �ּ��߰� �� OnBeginDrag �ּ� ����
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
        
    IReadOnlyList<InventoryInfo> clientList;    // ���� �κ��丮�� ����� Ŭ���̾�Ʈ �κ��丮����
    PointerEventData pEventData;                // �׷��� ����ĳ���� �� ���� �� ������ �̺�Ʈ
    List<RaycastResult> raycastResults;         // �׷��� ����ĳ���� ����� ���� ����Ʈ
    bool isInventoryConnect;                    // ������ ���� �������� �κ��丮�� ���� ������ �� ����

    InventoryInteractive interactive;           // ��ü ������ ������ ���� ������ ���� ���ͷ�Ƽ�� ��ũ��Ʈ ����
    bool isMyItemSelecting = false;             // ���� �������� ���� ������ ����
    bool isFirstSelectDelay = false;            // ó�� ������ �� ������ �ð��� �������� ����
    Button itemSelectBtn;                       // ��ư�� ����Ʈ�� �����ϱ� ���� ����
    string strItemDropSpace = "ItemDropSpace";  // �������� ����� �� �ִ� �±� ����(������ �±�)


    void Start()
    {
        itemRectTr = GetComponent<RectTransform>();     
        itemCG = GetComponent<CanvasGroup>();     
        itemInfo = GetComponent<ItemInfo>();
        itemSelectBtn = GetComponent<Button>(); 
              
        pEventData=new PointerEventData( EventSystem.current ); // �̺�Ʈ �ý����� �����մϴ�.
        raycastResults = new List<RaycastResult>();  
    }

    
    // ����Ʈ ���� ���� ó��
    public void OnUpdateSelected( BaseEventData eventData )
    {
        // �� ������ ���û��°� �ƴ϶�� �������� �ʽ��ϴ�. (������ 1���� �����մϴ�.)
        if( !isMyItemSelecting )
            return;
            

        // ������ ��ġ�� ���콺 ��ġ�� ��ġ�ø� ������ ���콺 ��ġ�� �������Ƿ�, �ٽ� ������ġ�� ������ �ش� ��ġ�� �������� �����̵��� ���ݴϴ�.
        itemRectTr.position = Input.mousePosition + moveVecToCenter;

        // ���� �߿� ���콺 ��ư�� Ŭ���� ��� - �ݵ�� ���� ���� (ùŬ������ �������� ����)
        if( Input.GetMouseButton( 0 ) && isFirstSelectDelay )
        {
            itemSelectBtn.OnDeselect( eventData );              // ��ư�� Deselect���·� ����ϴ�.
            EventSystem.current.SetSelectedGameObject( null );  // �̺�Ʈ �ý����� ����Ʈ ���¸� null�� ����ϴ�.

        }
    }



    // ����Ʈ ���� ��
    public void OnSelect( BaseEventData eventData )
    {        
        // Select�� �����ϸ� ���� �������� �κ��丮 ������ �ֽ�ȭ�Ͽ� �����ɴϴ�. 
        interactive = itemInfo.InventoryInfo.gameObject.GetComponent<InventoryInteractive>();

        // ������ �������� �ϳ��� Ȱ��ȭ�Ǿ� �ִٸ� �ٸ� �������� �������� ������ �����մϴ�.
        // �ڽ��� �������� �������̰ų�, �������� �Ϸ���� �ʾҴٸ� �������� �ʽ��ϴ�.
        if( interactive.IsItemSelecting && isMyItemSelecting )
            return;
                
        interactive.IsItemSelecting = true;     // ��ü �����ۿ� �����ϴ� ���� ���¸� Ȱ��ȭ �մϴ�.
        isMyItemSelecting = true;               // �� ������ ���� ���� ���¸� Ȱ��ȭ �մϴ�.
                
        StartCoroutine( FirstSelectDelayTime(0.1f) );   // OnUpdateSelected ���� ȣ���� �����ϱ� ���� �����̸� �༭ ���º����� Ȱ��ȭ��ŵ�ϴ�.
                
        
        // ���� �θ� ��ġ (���Ը���Ʈ�� ���� ����)�� �����մϴ�.
        prevParentTr=itemRectTr.parent;
        movingParentTr = interactive.gameObject.transform.parent;      // �κ��丮�� �θ� ĵ���� ����

        itemRectTr.SetParent( movingParentTr );                 // �θ� �Ͻ������� �κ��丮�� �θ��� ĵ������ ��Ƽ� �̹��� �켱������ ���Դϴ�.
        itemCG.blocksRaycasts=false;                            // �巡�� �̺�Ʈ �̿ܿ��� ���� �ʽ��ϴ�.

        // ���� �̵� ���͸� ���մϴ�.
        // (���� �κ��丮 ���� ��ġ-���콺 �̺�Ʈ�� �߻��� ��ġ => ���콺 �̺�Ʈ ��ġ���� �κ��丮 �������� �̵��� �� �ִ� �̵�����)
        moveVecToCenter=itemRectTr.position-Input.mousePosition;
                






        // ����� ��� �κ��丮�� �׷��� ����ĳ���͸� ��� ���� �κ��丮 ������ ���� �κ��丮�� ���� ����ϴ�.
        clientList = itemInfo.InventoryInfo.ServerInfo.ClientInfo;

        // ���� �������� ���� �κ��丮�� ������������� Ȯ���մϴ�.
        isInventoryConnect = itemInfo.InventoryInfo.IsConnect;

        // ����ĳ��Ʈ �������Ʈ�� �ʱ�ȭ�մϴ�.
        raycastResults.Clear();
    }



    // ����Ʈ ���� ��
    public void OnDeselect( BaseEventData eventData )
    {        
        isMyItemSelecting=false;            // �� ������ ���� ���¸� �ٷ� ��Ȱ��ȭ�մϴ�.
        isFirstSelectDelay=false;           // ó�� Ŭ�� ���¸� ��Ȱ��ȭ �մϴ�.

        // �̺�Ʈ�� �Ͼ �������� ���콺�� �ٽ� Ŭ������ ���� �������� �����մϴ�.
        pEventData.position=Input.mousePosition;

        // ����� ��� �κ��丮���� �׷��� ����ĳ������ �����ϰ� ����� �޽��ϴ�.
        for(int i=0; i<clientList.Count; i++)
            clientList[i].gRaycaster.Raycast( pEventData, raycastResults );


        // ����ĳ���ÿ� ������ ���(������ ������Ʈ�� �ִ� ���)
        if( raycastResults.Count>0 )
        {
            //string objNames = "";
            //for( int i = 0; i<raycastResults.Count; i++ )
            //    objNames+=raycastResults[i].gameObject.name+" ";
            //print( "[����Ǿ����ϴ�!]"+objNames );


            // ��� ����ĳ���� ����� �ϳ��� ����ϴ�.
            foreach(RaycastResult raycastResult in raycastResults)
            {
                Transform resultTr = raycastResult.gameObject.transform;
                
                // ������ ������Ʈ�� �±װ� �����̶��,
                if(resultTr.tag == strItemDropSpace)
                {
                    itemInfo.OnItemSlotDrop( resultTr );
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
            //print( "[������� �ʾҽ��ϴ�]" );


            // ���� �κ��丮�� ������¶��
            if( isInventoryConnect )
                itemInfo.UpdatePositionInSlotList();    // ����ġ�� �����ϴ�.
            // ���� �κ��丮�� ������°� �ƴ϶��
            else                
                itemInfo.OnItemWorldDrop();             // �κ��丮���� ������ ����� ����մϴ�.
                        
            // ������ ������ ���¸� ������ �ð��� ���� �ʰ� ��Ȱ��ȭ��ŵ�ϴ�.
            StartCoroutine( SelectDoneDelayTime( 0f ) );
        }
        else
            throw new System.Exception( "��� �̻��� �߻��Ͽ����ϴ�. Ȯ���Ͽ� �ּ���." );

    }

    
    /// <summary>
    /// ó�� ����Ʈ�ϱ� ���� Ŭ���� �� �ٷ� ����Ʈ ������ ȣ���� �Ͼ�� ���� ���� ���� ������ �޼���
    /// </summary>
    IEnumerator FirstSelectDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        isFirstSelectDelay = true;
    }

    /// <summary>
    /// �������� ������ �� �����̸� �༭ �ʱ�ȭ ������� ���� ���� ��Ƴ��� �޼���
    /// </summary>
    IEnumerator SelectDoneDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        interactive.IsItemSelecting = false;
        itemCG.blocksRaycasts = true;       // �巡�װ� ������ �ٽ� �̺�Ʈ�� �ް� �մϴ�.
    }
    






}
