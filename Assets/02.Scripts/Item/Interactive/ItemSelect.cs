using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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
 * <v7.5 - 2024_0126_�ֿ���>
 * 1- ������ ��ӿ� ������ ��� ����ġ�� �����ִ� �ڵ� �߰�
 * 
 * <v7.6 - 2024_0130_�ֿ���>
 * 1- InitOnSelect�޼��忡�� �����Կ��� �������� �Ͼ�� �˸��� OnQuickSlotSlect �޼��带 ȣ��
 * 
 * 
 * 
 * <v8.0 - 2024_0209_�ֿ���>
 * 1- �ּ��� ��� ���� ������ �κ��־ �ּ��� �߰�
 * a- v7.3���� FinishSelecting ����Ʈ �Ű����� �޼��带 �߰� �����ߴٰ� �ߴµ� �ش� ��ũ��Ʈ���� ������ ����.
 * b- itemCG�� ����ĳ������ �����ϴ� ������ ������ ��� ���ڸ��� ��ư �̺�Ʈ�� �߻��Ͽ� �ٽ� �������� �Ͼ�� ���� (�ߺ�����Ʈ ����)
 * c- ����Ī �� �������� inventoryCG�� �����ϴ� ����
 * 
 * 2- DropItemWithRaycast�޼��� ���ο� QuestCheck �� Craft_UIManager �޼��� ȣ�� �ڵ� null�˻� ���� �߰�
 * 
 * 3- interactive�������� interactiveBeforeDrop���� ����
 * ������ ��� ������ �κ��丮�� interactive�� ���¸� �����Ű�µ�, ����� ������ �� �κ��丮�� �ٲ�Ƿ� ���¸� �ǵ��� ���ƾ� ��.
 * 
 * 4- InitOnSelect�޼��� ������ interactiveBeforeDrop�� ������ ��� �κ��� ���Ӱ� ã�� �ʰ� itemInfo�� ���� �������� �޵��� ����
 * 
 * <v8.1 - 2024_0216_�ֿ���>
 * 1- ������ isFirstSelectDelay�� bInstantFinishDelay�� ����, �޼���� FirstSelectDelayTime�� InstantSelectFinishDelayTime�� ����
 * 
 * <v8.2 - 2024_0221_�ֿ���>
 * 1- �������� �缱���� �����ϰ� ������ִ� isReselect������ �߰��ϰ�, 
 * ReselectUntilDeselect�޼��� ȣ���� ���� �ܺο��� �÷��׸� Ȱ��ȭ ��Ű��
 * ����� �̷������ �� ���� Deselect�� ������ �ɶ� SelectDoneDelayTime �ڷ�ƾ ȣ�� �� �������� �ٽ� �������� ������ �����ϰ� ���־���
 * 
 * 2- DropItemWithRaycast�޼��忡�� ����ĳ���� ����� foreach������ �����鼭 �±װ� ������ �ƴҶ����� UpdatePositionInfo�� ȣ���ϴ� �κ��� ����
 * => ���� �±װ� �ϳ��� ����ȴٸ� UpdatePositionInfo�� ������ ���ƾ� �� (���� �޼��� ȣ��� ���� ��ü������ ���� ���̹Ƿ�) 
 * 
 * <8.3 - 2024_0222_�ֿ���>
 * 1- OnSelectFailCondition�޼��忡�� �������ǿ� IsItemSelecting�� isMyItemSelecting�� && �������� ó���ϴ� ���� ����
 * isMyItemSelecting���� ����
 * => ������ �ڱ� ������ �������� �߿��� ���� �ƴ϶�, 
 * �κ��丮 ��ü���� �������� �̷������ �ִٸ� �ٸ� ������ �������� ���ؾ� �ϹǷ�
 * 
 * (�̽�)
 * �������� ��ư�̺�Ʈ�� �߻����� ���ϵ��� ����ĳ��Ʈ��ϸ� �����ϸ� ���� �����۰� �κ��丮 ������ ���¸� ������ �ʿ䰡 ����.
 * 
 * 
 * <v8.4 - 2024_0223_�ֿ���>
 * 1- ������ �Ӽ����� �ڵ带 ��� �����Ͽ���.
 * �� �̻� ������ ���°����� �� �ʿ���� itemCG�� blocksRaycasts�� �����ϸ� ������ �� �� ���� ����
 * 
 * a- OnSelectFailCondition�� OnUpdateSelectedFailCondition �޼��带 ����
 * b- Ÿ �κ��丮 ��ӽ� �������� �ϴ� �ڵ带 ����
 * c- isMyItemSelecting���� �� interactiveBeforeDrop �������� ����
 * 
 * 2- IEnumerator�� ��� ���� �޼���� �����, WaitForSeconds�� ĳ�̺��� ó��
 * 
 * 3- SelectPreventTemporary�޼��带 �߰�
 * ������ ����Ī �� ��� �������� �������� �Ͻ������� ���� �� �ְ� �޼���ȭ�Ͽ���
 * 
 *  
 * 
 * (�̽�)
 * 1- �������� ��ư �������� �����ϰ��� �� ��
 * itemCG�� blocksRaycasts Ȥ�� interactable ���� �ϳ��� ���Ƶ� ������,
 * interactable�� ��ư�� ���̶������� �����ϴ� ȿ����,
 * blocksRaycasts�� �̹����� ������ �̺�Ʈ�� �����ϴ� ȿ���� �߰������� ���� �� ����.
 * => ���� SelectPreventTemporary�޼��忡���� ��ӽ� ��� �������� ������ �̺�Ʈ�� �״�� �츮�� ����
 * interactable�� �Ͻ������� �����ϰ�, ������������ ���� ��Ϸ���ĳ��Ʈ ������ ���� ������ �̺�Ʈ�� �������ϴ� ���� �����ִ°��� ���ٰ� �Ǵ�
 * 
 * <v8.5 - 2024_0223_�ֿ���>
 * 
 * 1- DropItemWithRaycast�޼��忡�� �ѹ��̶� ���� ������ �Ǿ��ٸ� break���� ���� ���������� �ڵ带 �߰��Ͽ���.
 * => (�̽�) ���� ĵ�������� �ִ� �κ��丮�� ��ϵǾ��ִ� ��� (������) ���� ĵ���� ����ĳ������ �ߺ��ؼ� �Ͼ�� ��.
 * 
 * 2- SelectPreventTemporary�޼��忡�� ��� ������ ĵ�����׷��� interactable�� raycastsBlocks�� ���ÿ� ������ ����
 * => (�̽�) ������ �������� ��ø��ų���� �� ��, ����Ƽ ��ü���� Ư������ ĵ���� �׷� �Ӽ� �� ���ʸ� ������ 
 * ��� ������ �������� �Ͼ�ٰ� ��ҵǴ� ������ ����� ���� �߰�, Ÿ �������� �������� �Ͼ�� ���� 
 * 
 * 3.
 * (�̽�) ���� �̸��� ��ȭ �������� ������ ��ø �� �����ϴٺ��� ���ü� �̽��� �߻��ؼ�,
 * ��� �������� �������� ĵ���� �׷��� ���� ���� �ڵ带 �ִ���, 
 * ��ø�� ��� �������� �������� �Ͼ ���� �ڵ����� ������ ���Ḧ ����Ƽ���� �����ϰԵǰ�, 
 * ����� bInstantFinishDelay�� false�� �ִ���, ���� ������ �ڷ�ƾ�� ���Ƽ� true �λ��·� �����ϰ� �����μ�,
 * ���� �������� �ȵǴ� ���� �߻�
 * => ���� �ڷ�ƾ�� �� �� bInstantFinishDelay�� false�� �ʱ�ȭ �س��� �ڵ带 �߰��ؾ� ��.
 * 
 * 4- ���� �±� ���� �� ���������� ���� �˻��ؼ� OnQuickSlotDrop���� �˻��� OnItemSlotDrop�� �����ϴ� �κ��� OnItemSlotDrop�� �ٷ� �����ϴ� ������ ����
 * => ������ ����ũ��Ʈ���� ���ҽ� �ڱ� �κ��丮�� �������� ������������ �˻��� �� ���� ������
 * OnItemSlotDrop���ο��� ��ü������ �˻��ϴ� ������ ����
 * 
 * (�̽�)
 * 1- �����Կ��� ������ ������ ������ ��, ��� �������� �������� �������� �������� ���� ����Ǵ� ��찡 ���ܼ�
 * ��ӽ��н� ���� �������� �ڸ� ���ư�����, ��� ���Ⱑ �����õǹ����� ��찡 ����.
 * => (�ذ�Ϸ�) OnItemSlotDrop���� SelectPreventTemporary�޼��带 ȣ��������, ���� �б⿡ ���� �������� ���ϴ� ��찡 ����� ����
 * 
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
    
    protected bool bInstantFinishDelay = false;           // ó�� ������ �� ������ �ð��� �������� ����
    protected Button itemSelectBtn;                       // ��ư�� ����Ʈ�� �����ϱ� ���� ����
    protected string strItemDropSpace = "ItemDropSpace";  // �������� ����� �� �ִ� �±� ����(������ �±�)

    protected bool isReselect = false;                    // �������� ����� �̷������ �� ���� �缱���� �����ϰ� ����� ���º���

    WaitForSeconds selectDelayTime = new WaitForSeconds(0.1f); // ������ �缿���� ������ ���� ������ �ð�

    bool isOnSelectDone = false;                          // OnSelect�� OnUpdateSelected�� ȣ����� ������ ���� ���º���


    // +++ 2024_0128_���� �ڵ� (������)
    public QuestCheck check;
    public Craft_UIManager craft_UIManager;


    protected virtual void Start()
    {
        itemRectTr = GetComponent<RectTransform>();     
        itemCG = GetComponent<CanvasGroup>();     
        itemInfo = GetComponent<ItemInfo>();
        itemSelectBtn = GetComponent<Button>();  

        // +++ 2024_0128_���� �ڵ� (������)
        check = GameObject.Find("Canvas-Quest").GetComponent<QuestCheck>();
        craft_UIManager = GameObject.Find("CraftingSystem").GetComponent<Craft_UIManager>();
    }



    

    

    // ����Ʈ ���� ��
    public virtual void OnSelect( BaseEventData eventData )
    {                        
        InitOnSelect();                 // ������ ���� �� ���� �ʱ�ȭ�� �����մϴ�.
    }
           


    // ����Ʈ ���� ���� ó��
    public virtual void OnUpdateSelected( BaseEventData eventData )
    {
        MatchItemPositionWithCursor();          // ���콺 ��ġ�� Ŀ��Ŭ�� �������� ����ϴ�.

        // �̹� ���õǾ��ִ� ���¿��� �ѹ� �� ������ ���ϵ��� �����ŵ�ϴ�. (�������� �̷������ �ִ� ���¿��� ���콺 Ŭ�� ���� �ڵ� �˻繮���� ���� �ٷ� �������� ����Ǵ� ���� �����մϴ�.)
        if( Input.GetMouseButton( 0 ) && bInstantFinishDelay )
            FinishSelecting(eventData);
    }

    

    // ����Ʈ ���� ��
    public virtual void OnDeselect( BaseEventData eventData )
    {
        InitFirstOnDeselect();              // �������� ���� �� �ʱ�ȭ�� �����մϴ�.

        DropItemWithRaycast();       // ����ĳ���õ� ����� ���� �������� �̵���ŵ�ϴ�.      

        InitLastOnDeselect();               // ����ĳ������ ���� �� �ʱ�ȭ�� �����մϴ�.
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






    
    /// <summary>
    /// OnSelect���� �����۰� ���� �� �Ӽ����� �ʱ�ȭ�ϱ� ���� ȣ�����ִ� �޼����Դϴ�.
    /// </summary>
    protected void InitOnSelect()
    {
        // �ߺ�����Ʈ ���� Ȱ��ȭ (��� ���ڸ��� ����Ʈ�� �Ͼ�� �ʰ� ��� �� �����̸� �༭ ŵ�ϴ�.)
        itemCG.blocksRaycasts=false; 

        // �缱�� �÷��׸� ��Ȱ��ȭ �մϴ�.
        isReselect = false;                            
        
        // �������� ���� �κ��丮 ������ ���Ӱ� �����մϴ�. (���氡�ɼ��� �����Ƿ�)
        inventoryInfo = itemInfo.InventoryInfo;

        
        prevParentSlotTr=itemRectTr.parent;                 // ���� �θ� ��ġ (���Ը���Ʈ�� ���� ����)�� �����մϴ�.
        selectingParentTr=inventoryInfo.transform.parent;   // �κ��丮�� �θ� ĵ���� ����

        itemRectTr.SetParent( selectingParentTr );          // �θ� �Ͻ������� �κ��丮�� �θ��� ĵ������ ��Ƽ� �̹��� �켱������ ���Դϴ�.     
        

        // ���� �̵� ���͸� ���մϴ�.
        // (���� �κ��丮 ���� ��ġ-���콺 �̺�Ʈ�� �߻��� ��ġ => ���콺 �̺�Ʈ ��ġ���� �κ��丮 �������� �̵��� �� �ִ� �̵�����)
        moveVecToCenter=itemRectTr.position-Input.mousePosition;

        
               



        // OnUpdateSelected ���� ȣ���� �����ϱ� ���� �����̸� �༭ ���º����� Ȱ��ȭ��ŵ�ϴ�.
        StartCoroutine( InstantSelectFinishDelayTime() );

        // ó�� ����Ʈ�ϱ� ���� Ŭ���� �� �ٷ� ����Ʈ ������ ȣ���� �Ͼ�� ���� ���� ���� �����̽ð� ���� �޼���
        IEnumerator InstantSelectFinishDelayTime()
        {
            yield return selectDelayTime;
            bInstantFinishDelay = true;
        }
    }

    




    /// <summary>
    /// OnDeselect ������ �Ͼ �� ���ʷ� �ʱ�ȭ�� ����� �ϴ� �޼����Դϴ�.
    /// </summary>
    protected void InitFirstOnDeselect()
    {
        bInstantFinishDelay=false;       // ó�� Ŭ�� ���¸� ��Ȱ��ȭ �մϴ�.
        //Debug.Log( "transformID: " + transform.GetInstanceID() + "Ŭ������ �ʱ�ȭ �Ϸ�");
    }

    
    /// <summary>
    /// OnDeselect ������ �Ͼ �� (�߰� ���� ����) �������� �ʱ�ȭ �� ��� �ϴ� �޼����Դϴ�.<br/>
    /// OnDeselect���� �߰� ����ĳ������ ���� �� ȣ�����ְ� �ֽ��ϴ�.
    /// </summary>
    protected void InitLastOnDeselect()
    {
        // ������ ������ ���¸� ������ �ð��� �༭ ��Ȱ��ȭ��ŵ�ϴ�.
        StartCoroutine( SelectDoneDelayTime() );


        // �������� �������� ������ �� ���� ���������� �ʱ�ȭ����� �� �Ӽ����� ��Ƴ��� �޼���
        IEnumerator SelectDoneDelayTime()
        {
            yield return selectDelayTime;
        
            // �κ��丮 ���� �� ���¸� �ʱ�ȭ�մϴ�.
            // �ߺ�����Ʈ ���� ����(��� ���ڸ��� ����Ʈ�� �Ͼ�� �ʰ� ��� �� �����̸� �༭ ŵ�ϴ�.)
            itemCG.blocksRaycasts=true; 

            bInstantFinishDelay = false;     // ó�� Ŭ�� ���¸� ��Ȱ��ȭ �մϴ�.


            // �缱�� �÷��װ� Ȱ��ȭ�Ǿ� �ִٸ�, �������� �������� ó������ �ٽ� �����մϴ�.
            if( isReselect )
            {
                //Debug.Log( "transformID: " + transform.GetInstanceID() + "���������մϴ�.");
                EventSystem.current.SetSelectedGameObject( this.gameObject );
            }

        }
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
            // ����ĳ���� ����� ���� ���� Ž�����θ� �ʱ�ȭ�մϴ�.
            bool isFoundSlot = false;

            // ��� ����ĳ���� ����� �ϳ��� ����ϴ�.
            foreach( RaycastResult raycastResult in raycastResults )
            {
                Transform resultTr = raycastResult.gameObject.transform;

                // ������ ������Ʈ�� �±װ� �����̶��,
                if( resultTr.tag==strItemDropSpace )
                {
                    // ���� Ž�� ���θ� Ȱ��ȭ�մϴ�.
                    isFoundSlot = true;
                    
                    // �������� ���� ����� �����մϴ�.
                    itemInfo.OnItemSlotDrop( resultTr );
                    

                    // ������ �ѹ��̶� ������ �ݺ����� �����մϴ�.
                    break;      
                }
            }

            
            // ������ ������Ʈ�� �±׿� ������ ���ٸ�(����Ž�� ���ΰ� ��Ȱ��ȭ���¶��), �ٽ� ����ġ�� �����ݴϴ�.
            if(!isFoundSlot)                
                itemInfo.UpdatePositionInfo();
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
            {
                itemInfo.OnItemWorldDrop();       // �κ��丮���� ������ ����� ����մϴ�.


                // +++ 2024_0128_���� �ڵ� (������)
                if(check!=null)
                    check.WorldQuestCheck();          // ����� �������� �߻��� ��� ����Ʈ üũ �ֽ�ȭ
                if(craft_UIManager != null)
                    craft_UIManager.CheckTab();       // ����� �������� �߻��� ��� ���ۿ� �ʿ��� ��� ����� �ֽ�ȭ 
            }
        }
        else
            throw new System.Exception( "��� �̻��� �߻��Ͽ����ϴ�. Ȯ���Ͽ� �ּ���." );
    }








    
    /// <summary>
    /// �������� �缱���� ������ Ȱ��ȭ�ϴ� �޼����Դϴ�.<br/>
    /// �������� Deselect�� ������ ������ ������ ȣ��Ǹ� �������� �������� �ٽ� �������ݴϴ�.
    /// </summary>
    public void ReselectUntilDeselect()
    {
        isReselect = true;
    }


    /// <summary>
    /// �������� ����Ʈ�� �Ͻ������� �������ֱ� ���� �޼����Դϴ�.<br/>
    /// �������� ��ȣ�ۿ� �Ӽ��� �Ͻ������� ��Ȱ��ȭ �� Ȱ��ȭ �����ݴϴ�.<br/><br/>
    /// ������ ����Ī �� ��� �������� �������� �������� �뵵�� ���˴ϴ�.<br/>
    /// </summary>
    public void SelectPreventTemporary()
    {
        StartCoroutine( SelectPreventDelayTime() );


        //�������� ��ȣ�ۿ� �Ӽ��� �Ͻ��� ���� ���� ������ �ð� ���� ���󺹱����ִ� �޼���
        IEnumerator SelectPreventDelayTime()
        {
            // itemCG�� (�������� �������� ��� ������) ��ȣ�ۿ��� �Ͻ������� ��Ȱ��ȭ
            if( itemInfo.IsEquip )
            {
                itemInfo.DummyInfo.DummyImg.raycastTarget = false;
                itemInfo.DummyInfo.DummyBtn.interactable = false;
            }
            else
            {
                itemCG.blocksRaycasts=false;
                itemCG.interactable=false;
            }
            yield return selectDelayTime;

            
            // itemCG�� (�������� �������� ��� ������) ��ȣ�ۿ��� �ٽ� Ȱ��ȭ
            if( itemInfo.IsEquip )
            {
                itemInfo.DummyInfo.DummyImg.raycastTarget = true;
                itemInfo.DummyInfo.DummyBtn.interactable = true;
            }
            else
            {
                itemCG.blocksRaycasts = true;
                itemCG.interactable = true;
            }
        }
    }





}
