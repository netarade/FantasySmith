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
 * <v7.0 - 2024_0117_�ֿ���>
 * 1- OnDeselect���� �ּ�ó���Ǿ��ִ� ����׿� �ڵ带 PrintDebugInfo�޼��带 ����� �־�ξ���
 * 
 * 2- DummyItemSelect��ũ��Ʈ ������ ��Ӱ� �������̵��� ���� protected �� virtual ó��
 * 
 * 3- ���� �ڵ带 ��� �������� �޼���ȭ�Ͽ� ��� �� ���뼺�� ����
 * 
 * 4- ������ prevParentTr�� prevParentSlotTr�� ����, movingParentTr�� selectingParentTr�� ����
 * 
 * <v7.1 - 2024_0118_�ֿ���>
 * 1- DropItemWithRaycastResults�޼��忡��  
 * ������ ���� ���� �θ� �κ��丮�� �������̶�� �������� ��Ӹ޼������ ȣ�����ֵ��� ����
 * 
 * <v7.2 - 2024_0122_�ֿ���>
 * 1- �׷��� ����ĳ������ ItemSelect��ũ��Ʈ���� �����ϴ� ���� �ƴ϶� �������� ���� InventoryInfo�� �Ź� �����Ͽ� ĳ�����ϵ��� ����
 * 
 * a- �׷��ȷ���ĳ���� ���� ������ ����
 * ���� �޼��� (RaycastAllToConnectedInventory, PrintDebugInfo) �� 
 * ���� ����(clientList, raycastResults, isInventoryConnect ��)�� ���� 
 * 
 * b- DropItemWithRaycastResults�޼��� DropItemWithRaycast���� �̸� ����
 * ���ο� inventoryInfo�� ����ĳ������ ȣ���Ͽ� ����� �޵��� �ڵ庯��
 * 
 * <v7.3 - 2024_0123_�ֿ���>
 * 1- FinishSelecting �Ű����� ���� �����ε� �޼��� �߰�����
 * ItemInfo���� ȣ���Ͽ� ���� ��ӽ� ������ ������ ���Ḧ �ϱ� ���� 
 * 
 * <v7.4 - 2024_0123_�ֿ���>
 * 1- Ÿ �κ��丮 �̵� �� �������� ���� ���� ���� ������� 
 * itemInfo�� �ֽ� inventoryInfo�� interactive�� �����Ͽ� �߰��� ���¸� �ݿ���Ű��
 * SelectDoneDelayTime �޼��忡���� ���� �κ��丮�� �ֽ� �κ��丮�� ���¸� ���ÿ� �����ϵ��� ����
 * 
 * <v7.5 - 2024_0123_�ֿ���>
 * 1- 
 * 
 */


/// <summary>
/// Drag��ũ��Ʈ�� ItemInfo ��ũ��Ʈ�� �Բ� ������ 2D ������Ʈ�� ������Ʈ�� ������ �־�� �մϴ�.<br/>
/// ������ ������Ʈ�� �巡�׸� �����ϰ� ���ݴϴ�.
/// </summary>
public class ItemSelect : MonoBehaviour
    , IUpdateSelectedHandler, ISelectHandler, IDeselectHandler      // ����Ʈ ���
{
    protected RectTransform itemRectTr;     // �ڽ� transform ĳ��
    protected CanvasGroup itemCG;           // �ڽ��� ĵ���� �׷� ĳ��
    protected ItemInfo itemInfo;            // �ڽ��� ItemInfo ĳ��
    
    protected Transform prevParentSlotTr;   // �������� �ű�� ������ �θ� (����)
    protected Transform selectingParentTr;  // �������� �ű�� ������ �θ� (ĵ����)

    protected Vector3 moveVecToCenter;      // ������ �������� ������ �� �κ��丮�� �������� �ǵ��� �� �ִ� ����
        

    protected InventoryInfo inventoryInfo;                // �������� ���� �κ��丮 ���� (�������� ����)
    protected InventoryInteractive interactive;           // ��ü ������ ������ ���� ������ ���� ���ͷ�Ƽ�� ��ũ��Ʈ ����
    protected bool isMyItemSelecting = false;             // ���� �������� ���� ������ ����
    protected bool isFirstSelectDelay = false;            // ó�� ������ �� ������ �ð��� �������� ����
    protected Button itemSelectBtn;                       // ��ư�� ����Ʈ�� �����ϱ� ���� ����
    protected string strItemDropSpace = "ItemDropSpace";  // �������� ����� �� �ִ� �±� ����(������ �±�)


    protected virtual void Start()
    {
        itemRectTr = GetComponent<RectTransform>();     
        itemCG = GetComponent<CanvasGroup>();     
        itemInfo = GetComponent<ItemInfo>();
        itemSelectBtn = GetComponent<Button>();  
    }



    

    // ����Ʈ ���� ���� ó��
    public virtual void OnUpdateSelected( BaseEventData eventData )
    {        
        if( OnUpdateSelectedFailCondition() )   // ���������� ����Ǹ� �������� �ʽ��ϴ�.
            return;
        
        MatchItemPositionWithCursor();          // ���콺 ��ġ�� Ŀ��Ŭ�� �������� ����ϴ�.


        // �̹� ���õǾ��ִ� ���¿��� �ѹ� �� ������ ���ϵ��� �����ŵ�ϴ�.
        if( Input.GetMouseButton( 0 ) && isFirstSelectDelay )
            FinishSelecting(eventData);
    }

    

    // ����Ʈ ���� ��
    public virtual void OnSelect( BaseEventData eventData )
    {        
        if( OnSelectFailCondition() )   // ���������� ����Ǹ� �������� �ʽ��ϴ�.
            return;
                
        InitOnSelect();                 // ������ ���� �� ���� �ʱ�ȭ�� �����մϴ�.    
    }
           
    

    // ����Ʈ ���� ��
    public virtual void OnDeselect( BaseEventData eventData )
    {
        InitFirstOnDeselect();              // �������� ���� �� �ʱ�ȭ�� �����մϴ�.

        DropItemWithRaycast();       // ����ĳ���õ� ����� ���� �������� �̵���ŵ�ϴ�.      

        InitLastOnDeselect();               // ����ĳ������ ���� �� �ʱ�ȭ�� �����մϴ�.
    }









    /// <summary>
    /// OnUpdateSelected �ݹ� �̺�Ʈ�� ������� �ʱ� ���� ������ ��ȯ�մϴ�.
    /// </summary>
    /// <returns>���������� �����Ǿ��ٸ� true, �����ؾ� �Ѵٸ� false�� ��ȯ</returns>
    protected bool OnUpdateSelectedFailCondition()
    {
        // �� ������ ���û��°� �ƴ϶�� �������� �ʽ��ϴ�. (������ 1���� �����մϴ�.)
        if( !isMyItemSelecting )
            return true;
        else
            return false;
    }


    /// <summary>
    /// ������ ��ġ�� ���콺�� Ŭ���� ���� �����ִ� �޼����Դϴ�.<br/>
    /// ���� ��Ŀ�� �ٽ� ���� ������ġ�� �������� Ŭ�������� ���ߴ� ����� ������ �ֽ��ϴ�.
    /// </summary>
    protected void MatchItemPositionWithCursor()
    {
        // ������ ��ġ�� ���콺 ��ġ�� �ع����� ������ ���콺 ��ġ�� �������Ƿ�,
        // �ٽ� ������ġ�� ������ �ش� ��ġ�� �������� �����̵��� ���ݴϴ�.
        itemRectTr.position = Input.mousePosition + moveVecToCenter;               
    }

    
    /// <summary>
    /// ������ �������� ������ �����մϴ�. 
    /// </summary>
    protected void FinishSelecting(BaseEventData eventData)
    {
        itemSelectBtn.OnDeselect( eventData );              // ��ư�� Deselect���·� ����ϴ�.
        EventSystem.current.SetSelectedGameObject( null );  // �̺�Ʈ �ý����� ����Ʈ ���¸� null�� ����ϴ�.
    }






    protected bool OnSelectFailCondition()
    {
        if( itemInfo==null )
            Debug.Log( $"������ ������ �����ϴ�.\n����:{gameObject.name} �θ�:{transform.parent.name}" );
        else if(itemInfo.InventoryInfo==null)
            Debug.Log( $"�κ��丮 ������ �����ϴ�.\n����:{gameObject.name} �θ�:{transform.parent.name}" );
        else
            Debug.Log( $"�ٸ� ����.\n����:{gameObject.name} �θ�:{transform.parent.name}" );



        // Select�� �����ϸ� ���� �������� �κ��丮 ������ �ֽ�ȭ�Ͽ� �����ɴϴ�. 
        interactive = itemInfo.InventoryInfo.gameObject.GetComponent<InventoryInteractive>();

        // ������ �������� �ϳ��� Ȱ��ȭ�Ǿ� �ִٸ� �ٸ� �������� �������� ������ �����մϴ�.
        // �ڽ��� �������� �������̰ų�, �������� �Ϸ���� �ʾҴٸ� �������� �ʽ��ϴ�.
        if( interactive.IsItemSelecting && isMyItemSelecting )
            return true;
        else 
            return false;
    }


    protected void InitOnSelect()
    {
        interactive.IsItemSelecting=true;     // ��ü �����ۿ� �����ϴ� ���� ���¸� Ȱ��ȭ �մϴ�.
        isMyItemSelecting=true;               // �� ������ ���� ���� ���¸� Ȱ��ȭ �մϴ�.

        // OnUpdateSelected ���� ȣ���� �����ϱ� ���� �����̸� �༭ ���º����� Ȱ��ȭ��ŵ�ϴ�.
        StartCoroutine( FirstSelectDelayTime( 0.1f ) );


        // ���� �θ� ��ġ (���Ը���Ʈ�� ���� ����)�� �����մϴ�.
        prevParentSlotTr=itemRectTr.parent;
        selectingParentTr=interactive.gameObject.transform.parent; // �κ��丮�� �θ� ĵ���� ����

        itemRectTr.SetParent( selectingParentTr );              // �θ� �Ͻ������� �κ��丮�� �θ��� ĵ������ ��Ƽ� �̹��� �켱������ ���Դϴ�.
        itemCG.blocksRaycasts=false;                            // �巡�� �̺�Ʈ �̿ܿ��� ���� �ʽ��ϴ�.

        // ���� �̵� ���͸� ���մϴ�.
        // (���� �κ��丮 ���� ��ġ-���콺 �̺�Ʈ�� �߻��� ��ġ => ���콺 �̺�Ʈ ��ġ���� �κ��丮 �������� �̵��� �� �ִ� �̵�����)
        moveVecToCenter=itemRectTr.position-Input.mousePosition;

        
        // �������� ���� �κ��丮 ������ ���Ӱ� �����մϴ�. (���氡�ɼ��� �����Ƿ�)
        inventoryInfo = itemInfo.InventoryInfo;
    }



    /// <summary>
    /// OnDeselect ������ �Ͼ �� ���ʷ� �ʱ�ȭ�� ����� �ϴ� �޼����Դϴ�.
    /// </summary>
    protected void InitFirstOnDeselect()
    {
        isMyItemSelecting=false;        // �� ������ ���� ���¸� �ٷ� ��Ȱ��ȭ�մϴ�.
        isFirstSelectDelay=false;       // ó�� Ŭ�� ���¸� ��Ȱ��ȭ �մϴ�.
    }

    
    /// <summary>
    /// OnDeselect ������ �Ͼ �� (�߰� ���� ����) �������� �ʱ�ȭ �� ��� �ϴ� �޼����Դϴ�.<br/>
    /// OnDeselect���� �߰� ����ĳ������ ���� �� ȣ�����ְ� �ֽ��ϴ�.
    /// </summary>
    protected void InitLastOnDeselect()
    {        
        // ������ ������ ���¸� ������ �ð��� �༭ ��Ȱ��ȭ��ŵ�ϴ�.
        StartCoroutine( SelectDoneDelayTime( 0.15f ) );
    }




    /// <summary>
    /// ����ĳ���� ����� ���ǿ� ���� �����ۿ��� �Ͼ�� ������ ��Ƶ� �޼����Դϴ�. 
    /// </summary>
    protected void DropItemWithRaycast()
    {        
        // �κ��丮�� �׷��� �����ɽ����� ��û�ϰ� ����� ��ȯ�޽��ϴ�.
        IReadOnlyList<RaycastResult> raycastResults = inventoryInfo.RaycastAllToConnectedInventory();

        // ����ĳ���ÿ� ������ ���(������ ������Ʈ�� �ִ� ���)
        if( raycastResults.Count>0 )
        {
            // ��� ����ĳ���� ����� �ϳ��� ����ϴ�.
            foreach( RaycastResult raycastResult in raycastResults )
            {
                Transform resultTr = raycastResult.gameObject.transform;

                // ������ ������Ʈ�� �±װ� �����̶��,
                if( resultTr.tag==strItemDropSpace )
                {
                    // ������ ���� ���� �θ� �κ��丮�� �������̶�� ������ ��Ӹ޼��� ���� ȣ�����ݴϴ�.
                    QuickSlot quickSlot = resultTr.parent.parent.parent.parent.GetComponent<QuickSlot>();

                    // �������̶�� �����Կ� ����� �������� Ȯ���� ����� �����մϴ�.
                    if( quickSlot!=null )
                    {
                        if( quickSlot.OnQuickSlotDrop( itemInfo, resultTr ) )
                        {
                            itemInfo.OnItemSlotDrop( resultTr );

                            // Ÿ �κ��丮 ���� �� ������ ������ ���°� ������ �ʾұ� ������
                            // �ֽ� �κ��丮 ������ ���¸� �ݿ��մϴ�.
                            itemInfo.InventoryInteractive.IsItemSelecting = true;
                            itemInfo.InventoryInfo.inventoryCG.blocksRaycasts = false;
                        }

                    }
                    // �������� �ƴ� ��� �ٷ� ����� �����մϴ�.
                    else
                    {
                        itemInfo.OnItemSlotDrop( resultTr );
                        
                        // Ÿ �κ��丮 ���� �� ������ ������ ���°� ������ �ʾұ� ������
                        // �ֽ� �κ��丮 ������ ���¸� �ݿ��մϴ�.
                        itemInfo.InventoryInteractive.IsItemSelecting = true;
                        itemInfo.InventoryInfo.inventoryCG.blocksRaycasts = false;
                    }
                }
                // ������ ������Ʈ�� �±װ� ������ �ƴ϶��, �ٽ� ����ġ�� �����ݴϴ�.
                else
                    itemInfo.UpdatePositionInfo();
            }

        }
        // ����ĳ������ ������ �����鼭, �θ��� �̵��� �߻����� �ʾҴٸ�,(������ ����� �����ߴٸ�)
        else if( raycastResults.Count==0 && itemRectTr.parent==selectingParentTr )
        {
            //print( "[������� �ʾҽ��ϴ�]" );

            // ���� �κ��丮�� ������¶��
            if( inventoryInfo.IsConnect )
                itemInfo.UpdatePositionInfo();    // ����ġ�� �����ϴ�.
            // ���� �κ��丮�� ������°� �ƴ϶��
            else
                itemInfo.OnItemWorldDrop();       // �κ��丮���� ������ ����� ����մϴ�.
        }
        else
            throw new System.Exception( "��� �̻��� �߻��Ͽ����ϴ�. Ȯ���Ͽ� �ּ���." );
    }
















    /// <summary>
    /// ó�� ����Ʈ�ϱ� ���� Ŭ���� �� �ٷ� ����Ʈ ������ ȣ���� �Ͼ�� ���� ���� ���� ������ �޼���
    /// </summary>
    protected IEnumerator FirstSelectDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        isFirstSelectDelay = true;
    }

    /// <summary>
    /// �������� ������ �� �����̸� �༭ �ʱ�ȭ ������� ���� ���� ��Ƴ��� �޼���
    /// </summary>
    protected IEnumerator SelectDoneDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        
        // �κ��丮 ���� �� ���¸� �ʱ�ȭ�մϴ�.
        interactive.IsItemSelecting = false;    
        itemCG.blocksRaycasts = true;           // �巡�װ� ������ �ٽ� �̺�Ʈ�� �ް� �մϴ�.

        
        // ���� ����� �κ��丮�� ���¸� �ʱ�ȭ�մϴ�.
        if( !itemInfo.IsWorldPositioned )
        {
            itemInfo.InventoryInteractive.IsItemSelecting=false;
            itemInfo.InventoryInfo.inventoryCG.blocksRaycasts=true;
        }
    }
    


    





}
