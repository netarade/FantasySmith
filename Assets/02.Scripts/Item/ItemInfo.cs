using UnityEngine;
using UnityEngine.UI;
using ItemData;
using InventoryManagement;
using System;

/*
 * [�۾� ����]
 * 
 * <v1.0 - 2023_1102_�ֿ���>
 * 1- �����ۼ� �� �ּ�ó��
 * 
 * <v2.0 - 2023_1103_�ֿ���>
 * 1- �ּ� ����
 * 2- �̹��� ������Ʈ ��� ������ Start�޼��忡�� OnEnable�� ����
 * �ν��Ͻ��� �����Ǿ� �̹��� ������Ʈ�� ��� �����ϸ� OnItemAdded�� ȣ�� ������ ���ü��� ������ ��������Ʈ �̹����� ������� �ʴ´�.
 * 
 * <v3.0 - 2023-1105_�ֿ���>
 * 1- ��������� ������ item�� ������Ƽȭ ���Ѽ� set�� ȣ��Ǿ��� �� OnItemChanged()�� ȣ��ǵ��� ���� 
 * OnItemAdded�� privateó�� �� ���� ����ó�� ���� ����
 *
 *<v4.0 - 2023_1108_�ֿ���>
 *1- �������� �ı��� �� ������ �����ϵ��� �����Ͽ�����, ���������� ����� ������� �ʴ� �����߻�
 *=> �������ʿ��� �ı��ɋ����� ��ųʸ��� �����ؼ� CraftManager�ʿ��� �������� �ѹ� �̸� �������ֵ��� ����
 *
 *2- OnItemChanged �޼��� �ּ��߰�
 *
 *3- UpdateCountTxt �޼��� �߰�
 * ������ ������ ���� �ɶ� �������� �ؽ�Ʈ�� �������ֵ��� �Ͽ���.
 * item�ʿ��� �޼��带 ������ �������� �ؼ� ���� ���ټ� Ȯ��.
 *
 *<v5.0 - 2023_1112_�ֿ���>
 *1- OnItemAdded�޼��� �߰�����. (CreateManager�ʿ��� �ߺ��ڵ� ����ϰ� �ִ� �� ���� �� ����, �ּ� �߰�) 
 *
 *<v6.0 - 2023_1114_�ֿ���>
 *1- OnItemAdded�޼��带 OnItemChanged�� �̸�����
 *2- ItemInfo Ŭ���� ���� �ּ� �߰�
 *3- private �޼��� public �޼���� ����
 *4- ��� ���� item�� public�Ǿ��ִ� ���� private ó��. �ݵ�� ������Ƽ�� ���� �ʱ�ȭ�� ����
 *
 *<v7.0 - 2023_1116_�ֿ���>
 *1- ItemInfo Ŭ������ ItemImageCollection ��������� �����Ͽ� �ܺ��̹����� �����ϵ��� �����Ͽ����ϴ�. 
 *(CreateManager�� �ִ� ���� ������ �Űܿ�.)
 *
 *2- UpdateImage�޼��带 �����Ͽ����ϴ�.
 *������ ������ Ŭ������ ImageCollection ����ü ������ ����� �����ϰ� �ִ� ������ ImageReferenceIndex ����ü ������ ����� �����ϵ��� �ٲپ��� ������
 *item�� ImageReferenceIndex ��������� ���� �ε������� �޾ƿͼ� ImageCollection ������ �����Ͽ� ������Ʈ�� �̹����� �־��ֵ��� ����.
 *
 *<v7.1 - 2023_1119_�ֿ���>
 *1- OnDestroy()�޼��� �ּ�ó��. 
 *InventoryŬ������ ����ȭ �����ϰ� ������ �����̹Ƿ�
 *
 *2- RemoveItemObject �̱��� �޼��� ���� - inventoryŬ�������� ����
 *ItemInfo Ŭ������ ������Ʈ�� ������ �ֽ�ȭ �����ִ� ������ �ϰ� �ؾ��ϱ� �����̸�, 
 *ItemInfo���� item�� ���������� �����ϴ� �޼��带 �߰��ϱ� �����ϸ�, InventoryŬ���������� ����� �ߺ� ������ ���ɼ��� Ŀ��.
 *
 *<v8.0 - 2023_1216_�ֿ���>
 *1- �������� ����â �̹��� ���� statusImage �߰� �� UpdateImage�޼��� ���� ����
 *
 *2- ������ �ı��� ���� ���� �ּ�ó�� �Ǿ��ִ� �κ� ����
 *
 *3- slotList ������ slotListTr�� ����
 *
 *4- Transform imageCollectionsTr �ӽú��� ������ 
 * GameObject.Find( "ImageCollections" ) �ߺ� ȣ�� ���� ����
 *
 *<v8.1 - 2023_1217_�ֿ���>
 *1- ItemImageCollection �������� �ϳ��� �����ϴ� ���� �迭�� ���� ����
 *
 *<v8.2 - 2023_1221_�ֿ���>
 *1- GameObject.Find()�޼���� ������Ʈ�� �˻��ϴ� ���� ���������� ����
 *
 *<v9.0 - 2023_1222_�ֿ���>
 *1- �±����� ö�ڿ��� ���� (CANVAS_CHRACTER -> CANVAS_CHARACTER)
 *
 *2- ItemImageCollection[]�� �迭�� �������ϰ� ������ ���ؼ� �ߴ� �迭�� bounds���� ����
 *
 *3- �������� ���������� UpdateImage�� UpdatePosition�� ȣ���ϸ� ������ ������ �ʱ� ������ bounds������ �ߴµ�
 * OnItemChanged�޼��带 �������� �������� ȣ���� �ƴ϶�, �������� ���� ������ ȣ���ϵ��� �����Ͽ���.
 *
 *4 - SlotListTr�� ����Ʈ�� �����ִ� ���� ����
 *
 *<v9.1 - 2023_1224_�ֿ���>
 *1- Item������Ƽ�� �ּ��Ϻ� ����
 *2- ������Ʈ ���� ���� Start���� OnEanble�� �̵� �� ����
 *3- UpdataImage�޼��� ������ ������ ���� �ߺ����� ���� �� ����ȭ
 *
 *<v9.2 - 2023_1226_�ֿ���>
 *1- �Ϻ� ����� ��¸޼��� ����
 *2- UpdatePosition�� slotListTr�� childCount �˻籸�� �߰�
 *3- Item ������Ƽ �ٽ� �ּ����� 
 *
 *<v9.3 - 2023_1228_�ֿ���>
 *1- ������ ������ ���� ���� �������� ���� (3D������Ʈ ������ 2D ������Ʈ�� �δ� ����)
 *���� Transform, RectTransform itemTr�� itemRectTr ������ �����Ͽ� �ڱ�Ʈ������ ĳ��ó��
 *
 *2- UpdatePosition 2D������Ʈ�� �θ� �����ϴ� ���� �ֻ��� 3D������Ʈ�� �����ϵ��� ����
 *
 *3- UpdatePosition �������� UpdateInventoryPosition���� ����
 *
 *<v9.4 - 2023_1228_�ֿ���
 *1- ������ ������ �������� �纯������ ���� (3D������Ʈ, 2D������Ʈ ��ȯ���)
 *�ڵ带 2D�������� �ӽ� ���� (itemTr->itemRectTr)
 *
 *
 *<v10.0 - 2023-1229_�ֿ���>
 *1- UpdateSlotPosition���� �������ڸ� �߰��Ͽ� ���Ը���Ʈ ������ �ִ� ��쿡�� �ش� ���Ը���Ʈ�� �ε����� �����Ǿ�����Ʈ�� �ϸ�,
 *���Ը���Ʈ ������ ���� ���� ��쿡�� �������� ���� ����ִ� ������ �������� ���Ը���Ʈ�� �����ؼ� �ε����� ���� ������ ������Ʈ�� �ϵ��� ����
 *
 *<v10.1 - 2023_1230_�ֿ���>
 *1- UpdateSlotPoisition�޼������ UpdatePositionInSlotList�� ���� �ϰ�
 *���Ը���Ʈ�� ���ڷ� �޵��� ����. �������ڷ� ���ڰ� ���޵��� �ʾҴٸ� ���������� �����Ͽ� �ڵ�����ϵ��� ����
 *
 *2- OnItemChanged�޼��带 �ܺο��� ȣ���� �� slotList�� ���ڷ� �޾Ƽ� ȣ�Ⱑ���ϵ��� ����,
 *UpdatePositionInSlotList�� slotList���ڸ� �����Ͽ� ȣ��. ���������� �������ڷ� ȣ�Ⱑ��
 *
 *3- �������� ��ȯ�ڵ带 �����ϰ� ����
 *�̸� ItemTr ItemRectTr�� ��Ƴ��� �ʿ����
 *��ȯ�� �̷���� ���� �޼��带 ȣ���ϸ� 2D�� �����̿��� �Ҷ��� ���� �������ڽ� 
 *
 *<v10.2 - 2023_1231_�ֿ���>
 *1- Locate 2D, 3D �޼���
 *2- SetOverlapCount�޼���
 *3- FindNearstRemainSlotIdx �޼��� inventoryInfo �Ű����� �����ε�
 *
 *<v10.3 - 2024_0101_�ֿ���>
 *1- �������� ���� Ȱ��ȭ ������ ���� Ȯ���ϰ� �����Ǿ�����Ʈ�� �ؾ� �ϹǷ�, ���ͷ�Ƽ�� ��ũ��Ʈ���� Ȱ��ȭ�� ���� ������ �޾Ƶ��� �Ͽ���
 *2- prevDropEventCallerTr���� ���� �� OnItemDrop�޼��� �߰�
 *
 *3- FindNearstRemainSlotIdx �޼��� ��� ����
 *InventoryInfo�� �־�� �� ����̸�, �������� �κ��丮 ������ �����ϰ� �ֱ� ������ �κ��丮�� �����Ͽ� ȣ���ϱ⸸ �ϸ� �Ǳ� ����
 *
 *4- OnEnable���� ���Ը���Ʈ ���� �� canvsTr�� find �޼���� ã�Ƽ� ó���ϴ� �� �����ϰ�,
 *UpdateInventoryInfo�޼��� ȣ��� ����
 *
 *<2024_0102_�ֿ���>
 *1- OnItemDrop�޼��� �����Ϸ�
 *� �������� ��� �̺�Ʈ �߻��� �ܺ� ��ũ��Ʈ���� ȣ���ϵ��� ����
 *2- UpdateActiveTabInfo �޼��� �����ϰ� OnItemChanged ���ο� �߰�.
 *
 *
 *
 *
 *[���� �����ؾ��� ��] 
 * 1- UpdateInventoryPosition�� ���� �ڽ� �κ��丮 �������� �����ϰ� ������,
 * ���߿� UpdatePosition�� �� �� �������� ������ ������ �ε��� �Ӹ� �ƴ϶� ��� �����Կ� ����ִ����� ������ �־�� �Ѵ�.
 *
 *2- ���Ե���̺�Ʈ�� �߻��� �� �κ��丮 ������ �ٸ��ٸ� ���� �κ��丮���� �� ������ ����� �����ؾ��Ѵ�.
 * ���������� �Ͼ �� �κ��丮���� �� �������� ��Ͽ� �߰��ϰų� �����ؾ� �Ѵ�.
 * Drag���� �κ��丮 ������ ������ �� �κ��丮 ��Ͽ��� �� �������� �����ؾ� �Ѵ�.
 *
 *
 *
 *
 * [�̽�_0101] 
 * 1- ������ ����(�κ��丮 ����) ������Ʈ ����
 * a- Slot To Slot���� SlotDrop�� �Ͼ �� Slot�� ���� �޾ƾ� �Ѵ�.
 * b- ItemDrag�ؼ� �κ��丮 �ܺη� Drop�Ҷ� ItemDrag�� ���� �޾ƾ� �Ѵ�.
 * c- ItemInfo���� 2D to World(������������), 3D to Slot(�κ��丮��������) �Ҷ� ��ü������ Ȯ���ؾ� �Ѵ�.
 * => �ܺο��� ������Ʈ �޼��带 ȣ���� �� �ְ� ������Ѵ�.
 *
 * 2- Ÿ �κ��丮�� ������Ʈ�� �̷��� ���� 
 * slotIndexAll�� slotIndex ��θ� �޾߾� �Ѵ�.
 * ������ �ѹ� ���� ���¿��� �ٸ� �Ǻ����� �õ��ϸ� ��ġ ������ �ȸ±� ����
 *
 *
 *
 */


/// <summary>
/// ���� ���� ������ ������Ʈ�� �� Ŭ������ ������Ʈ�� �������մϴ�.<br/><br/>
/// 
/// ItemInfo ��ũ��Ʈ�� ������Ʈ�� ���� ������ ������Ʈ�� ��ü���� ����� ������ �����ϴ�.<br/>
/// (ItemInfo�� ���� ������ �ν��Ͻ��� item�� �Ҵ�� �� �ڵ����� �̷�����ϴ�.)<br/><br/>
/// 
/// 1.������Ʈ�� �̹����� ���� �������� ������ �����Ͽ� ä��ϴ�.<br/>
/// 2.��ȭ�������� ��� ��øȽ���� ������������ ���Ͽ� ǥ���Ͽ� �ݴϴ�. ����ȭ �������� ��� �ؽ�Ʈ�� ���ϴ�.<br/>
/// 3.�κ��丮 ���� ���� �������� ������ ������ �����Ͽ� �ش� ���Կ� ��ġ��ŵ�ϴ�.<br/><br/>
/// 
/// ����) �������� ���� ������ �ٲ� �� ���� �ֽ� ������ ������Ʈ�� �ݿ��ؾ� �մϴ�.<br/>
/// 1,2,3�� ��� �� �޼��带 ���� ȣ�� �� �� ������ ��� ���� �ѹ��� ȣ���ϴ�  OnItemChanged�޼��尡 �ֽ��ϴ�.<br/>
/// </summary>
public class ItemInfo : MonoBehaviour
{
    private Item item;             // �������� ���� ������ ��� ����

    private Image itemImage;       // �������� �κ��丮���� 2D�󿡼� ������ �̹��� ������Ʈ ����  
    public Sprite innerSprite;     // �������� �κ��丮���� ������ �̹��� ��������Ʈ
    public Sprite statusSprite;    // �������� ����â���� ������ �̹��� ��������Ʈ (����â ��ũ��Ʈ���� ������ �ϰ� �˴ϴ�.)
    private Text countTxt;         // ��ȭ �������� ������ �ݿ��� �ؽ�Ʈ
        
    public ItemImageCollection[] iicArr;                             // �ν����� �� �󿡼� ����� ������ �̹��� ���� �迭
    public enum eIIC { MiscBase,MiscAdd,MiscOther,Sword,Bow,Axe }    // �̹��� ���� �迭�� �ε��� ����
    private readonly int iicNum = 6;                                 // �̹��� ���� �迭�� ����


    private RectTransform itemRectTr;       // �ڱ��ڽ� 2D Ʈ������ ����(�ʱ� ���� - ���� �θ�)
    private Transform itemTr;               // �ڱ��ڽ� 3D Ʈ������ ����(�ʱ� ���� - ���� ������ �ڽ�)
    private CanvasGroup itemCG;             // �������� ĵ���� �׷� ������Ʈ (�������� ����� ������ �� 2D�̺�Ʈ�� �������� �뵵) 


    /*** Locate2DToWorld �Ǵ� Locate3DToWorld �޼��� ȣ�� �� ����***/
    private bool isWorldPositioned;         // �������� ���忡 �����ִ��� ����

    /**** InventoryInfoChange �޼��� ȣ�� �� ���� ****/
    private Transform inventoryTr;              // ���� �������� ����ִ� �κ��丮�� ���������� �����մϴ�.
    private Transform slotListTr;               // ���� �������� ����ִ� ���Ը���Ʈ Ʈ������ ����
    private InventoryInfo inventoryInfo;        // ���� �������� ���� �� �κ��丮���� ��ũ��Ʈ
    private InventoryInteractive interactive;   // ���� �������� ���� �� ���ͷ�Ƽ�� ��ũ��Ʈ
    private Transform playerTr;                 // ���� �������� �����ϰ� �ִ� �÷��̾� ĳ���� ���� ����
    private Transform playerDropTr;             // �÷��̾ �������� ��ӽ�ų �� �������� ������ ��ġ


    /**** InventoryInfoChange �޼��� ȣ��� ���� ****/
    /**** inveractive���� �����Ͼ� �� ������ ����*****/
    private bool isActiveTabAll;                // ���� �������� ����ִ� �κ��丮�� Ȱ��ȭ ���� ������ ��ü����, �������� ����

    /**** OnItemDrop�޼��� ȣ�� �� ���� ****/
    private Transform prevDropEventCallerTr;    // ����̺�Ʈ�� �߻��� �� ������ ����̺�Ʈ ȣ���ڸ� ����ϱ� ���� ���� ���� 





    
    /*** ���ο����� ���������� �б����� ������Ƽ***/

    /// <summary>
    /// �������� ���忡 �����ִ��� (3D ������Ʈ ����) ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool IsWorldPositioned { get {return isWorldPositioned;} }
    
    /// <summary>
    /// ���� �������� ��� �κ��丮�� �����Դϴ�.
    /// </summary>
    public InventoryInfo InventoryInfo { get {return inventoryInfo;} }

    /// <summary>
    /// ���� �������� ����ִ� ���Ը���Ʈ�� Transform�� ��ȯ�մϴ�.<br/>
    /// </summary>
    public Transform SlotListTr { get {return slotListTr;} }







    /*** ���� �������� �Ӽ��� �������ִ� ������Ƽ ***/

    /// <summary>
    /// ������ ������Ʈ�� �ش��ϴ� �ǿ��� ��� ° ���Կ� ����ִ� �� �ε����� ��ȯ�ϰų� �����մϴ�.
    /// </summary>
    public int SlotIndex { get{return item.SlotIndex;} set{ item.SlotIndex=value;} }

    
    /// <summary>
    /// ������ ������Ʈ�� ��ü �ǿ��� ��� ° ���Կ� ����ִ� �� �ε����� ��ȯ�ϰų� �����մϴ�.
    /// </summary>
    public int SlotIndexAll { get{return item.SlotIndexAll;} set{item.SlotIndexAll=value;} }
      
    /// <summary>
    /// �������� ����ִ� ���� ������ ���� �����ϰų� ��ȯ�޽��ϴ�.<br/>
    /// Ŭ�� �� Item �ν��Ͻ��� �����ϰ�, ���� �Ǿ��ִ� �ν��Ͻ��� �ҷ��� �� �ֽ��ϴ�.<br/>
    /// </summary>
    public Item Item { set{ item=value; } get { return item; } }







    /// <summary>
    /// �̹��� ������Ʈ�� ��� �켱������ ���̱� ���� OnEnable ���
    /// </summary>
    private void OnEnable()
    {
        itemRectTr = transform.GetComponent<RectTransform>();   // �ڱ��ڽ� 2d Ʈ������ ����(�ʱ� ���� - ���� �θ�)
        itemTr = itemRectTr.GetChild(itemRectTr.childCount-1);  // �ڱ��ڽ� 3d Ʈ������ ����(�ʱ� ���� - ���� ������ �ڽ�)

        itemImage = GetComponent<Image>();
        countTxt = GetComponentInChildren<Text>();

        Transform canvasTr = GameObject.FindWithTag("CANVAS_CHARACTER").transform;
        //slotListTr = canvasTr.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        
        // �ν����ͺ� �󿡼� �޾Ƴ��� ��������Ʈ �̹��� ������ �����մϴ�.
        Transform imageCollectionsTr = GameObject.FindAnyObjectByType<CreateManager>().transform.GetChild(0);

        // �迭�� �ش� ������ŭ �������ݴϴ�.
        iicArr = new ItemImageCollection[iicNum];

        // �� iicArr�� imageCollectionsTr�� ���� �ڽĿ�����Ʈ�μ� ItemImageCollection ��ũ��Ʈ�� ������Ʈ�� ������ �ֽ��ϴ�
        for( int i = 0; i<iicNum; i++)
            iicArr[i] = imageCollectionsTr.GetChild(i).GetComponent<ItemImageCollection>();
        
        isWorldPositioned = false;
        itemCG = GetComponent<CanvasGroup>();

        // �������� �κ��丮 ������ ���������� ������� �ʱ�ȭ���ݴϴ�.
        UpdateInventoryInfo(itemRectTr.parent.parent.parent.parent);
    }
    

    /// <summary>
    /// �������� ������ ��� ������ �ֽ�ȭ�մϴ�.(�������� ���������� ������Ʈ�� �ݿ��մϴ�.)<br/>
    /// ������Ʈ�� item�� ������ �̷�����ٸ� item�� ������ �ִ� �̹����� �ݿ��ϰ� ��ȭ�������� ��� ��ø Ƚ������ �ֽ�ȭ �մϴ�.<br/>
    /// **** �ܺ� ��ũ��Ʈ���� �� ��ũ��Ʈ�� �޼��带 ������ �ʰ� ������Ƽ�� �޾� ������ ���� ������ ���� �������� �� �ֽ� ���� �ݿ��� ���Ͽ� ���� ȣ���ؾ� �մϴ�. ****<br/>
    /// </summary>
    /// <param name="SlotListTr">�������� ��� ���� ����Ʈ�� Ʈ������</param>
    public void OnItemChanged()
    {
        UpdateImage();                  // �̹��� �ֽ�ȭ
        UpdateCountTxt();               // ��ø ���� �ֽ�ȭ
        UpdateActiveTabInfo();          // Ȱ��ȭ �� �ֽ�ȭ
        UpdatePositionInSlotList();     // ���� ��ġ �ֽ�ȭ
    }

    /// <summary>
    /// �������� �̹��� ������ �޾ƿͼ� ������Ʈ�� �ݿ��մϴ�.<br/>
    /// Item Ŭ������ ���ǵ� �� �ܺο��� ������ �̹��� �ε����� �����ϰ� �ֽ��ϴ�.<br/>
    /// �ش� �ε����� �����Ͽ� �ν����ͺ信 ��ϵ� �̹����� �����մϴ�.
    /// </summary>
    public void UpdateImage()
    {
        if(iicArr.Length == 0 )     // ������ ���� ������ iicArr�� �����ϴ� ���� �����Ͽ� �ݴϴ�.
            return;

        int imgIdx = -1;            // ������ �̹��� �ε��� ����
                   
        switch( Item.Type )         // �������� ����Ÿ���� �����մϴ�.
        {

            case ItemType.Weapon:
                ItemWeapon weapItem = (ItemWeapon)Item;
                WeaponType weaponType = weapItem.WeaponType;  // �������� ����Ÿ���� �����մϴ�.

                switch (weaponType)
                {
                    case WeaponType.Sword :             // ����Ÿ���� ���̶��,
                        imgIdx = (int)eIIC.Sword;
                        break;
                    case WeaponType.Bow :               // ����Ÿ���� Ȱ�̶��,
                        imgIdx = (int)eIIC.Bow;
                        break;
                }
                break;
                
            case ItemType.Misc:
                ItemMisc miscItem = (ItemMisc)Item; 
                MiscType miscType = miscItem.MiscType;

                switch (miscType)
                {
                    case MiscType.Basic :           // ����Ÿ���� �⺻ �����,
                        imgIdx = (int)eIIC.MiscBase;
                        break;
                    case MiscType.Additive :        // ����Ÿ���� �߰� �����,
                        imgIdx = (int)eIIC.MiscAdd;
                        break;
                    default :                       // ����Ÿ���� ��Ÿ �����,
                        imgIdx = (int)eIIC.MiscOther;
                        break;
                }
                break;
        }

        // ������ ������Ʈ �̹����� �ν����ͺ信 ����ȭ�Ǿ� �ִ� ItemImageCollection Ŭ������ ���� ����ü �迭 ImageColection[]��
        // ����������� ���� ������ ������ �ִ� ImageReferenceIndex ����ü�� �ε����� �����ͼ� �����մϴ�.                
             
        innerSprite = iicArr[imgIdx].icArrImg[Item.ImageRefIndex.innerImgIdx].innerSprite;
        statusSprite = iicArr[imgIdx].icArrImg[Item.ImageRefIndex.statusImgIdx].statusSprite;

        // ������ ��������Ʈ �̹����� ������� �������� ������ 2D�̹����� �����մϴ�.
        itemImage.sprite = innerSprite;
    }


    /// <summary>
    /// ��ȭ �������� ��øȽ���� �������� �����մϴ�. ��ȭ �������� ������ ����� �� ���� ȣ���� �ֽʽÿ�.
    /// </summary>
    public void UpdateCountTxt()
    {
        if( Item.Type==ItemType.Misc )                // ��ȭ �������� ��ø ������ ǥ���մϴ�.
        {
            countTxt.enabled=true;
            countTxt.text = ((ItemMisc)Item).OverlapCount.ToString();
        }
        else
            countTxt.enabled = false;                // ��ȭ�������� �ƴ϶�� ��ø �ؽ�Ʈ�� ��Ȱ��ȭ�մϴ�.
    }
        
    /// <summary>
    /// ���� �������� ���� ����Ʈ�� �����ִٸ� ��ġ�� ���� �ε����� �°� �ֽ�ȭ�����ݴϴ�.<br/>
    /// ���� �ε����� �߸��Ǿ� �ִٸ� �ٸ� ��ġ�� �̵� �� �� �ֽ��ϴ�.<br/><br/>
    /// ** �������� ���� �� �ְų� ���� ������ �� ���� ��� ���ܸ� �����ϴ�. **<br/>
    /// </summary>
    public void UpdatePositionInSlotList()
    {
        // �������� ����� �����ְų� ������ ������ �ʾҴٸ�, ���ܸ� �����ϴ�.
        if( isWorldPositioned && slotListTr == null )   
            throw new Exception("������ ������ ������Ʈ �� �� �ִ� ��Ȳ�� �ƴմϴ�. Ȯ���Ͽ� �ּ���.");

        // ���� ����Ʈ�� ������ �����Ǿ����� �ʴٸ� ���������� �������� �ʽ��ϴ�.
        if( slotListTr.childCount==0 )
        {
            Debug.Log( "���� ������ �������� ���� �����Դϴ�." );
            return;
        }
                 
        // ���� Ȱ��ȭ ���� ���� ������� � �ε����� �������� �����մϴ�.
        int activeIndex = isActiveTabAll? item.SlotIndexAll : item.SlotIndex;
        itemRectTr.SetParent( slotListTr.GetChild(activeIndex) );     // �������� �θ� �ش� �������� �����մϴ�.
        itemRectTr.localPosition = Vector3.zero;                      // ������ġ�� ���Կ� ����ϴ�.
    }

    public void UpdateActiveTabInfo()
    {
        isActiveTabAll = interactive.IsActiveTabAll;
    }










    /// <summary>
    /// �������� �κ��丮���� ����� ����� �߻��� �� �ش� �������� �̵���Ű�� ���Ͽ� ȣ������� �ϴ� �޼����Դϴ�.<br/>
    /// ���� ��ǥ�� �Է� �� �شٸ� �ش� ���� ��ǥ �� �������� ����Ʈ����, �⺻ ���� null�� �شٸ� �÷��̾� ���� ��ǥ�� ����մϴ�.<br/>
    /// isSetParent�ɼ��� �����ϸ� �ش� ������ǥ ������Ʈ�� ���� �ڽ����� ������ݴϴ�.<br/>
    /// *** ���� �κ��丮 ������ ���� ������ ���ܸ� �߻���ŵ�ϴ�. *** <br/>
    /// </summary>
    public void OnItemWorldDrop( Transform callerWorldTr=null, bool isSetParent=false )
    {
        if(callerWorldTr==prevDropEventCallerTr)
            throw new Exception("���� ���� ��ǥ�� ����̺�Ʈ�� �ߺ� �߻��Ͽ����ϴ�. Ȯ���Ͽ� �ּ���.");   
                      
        if( callerWorldTr == null ) 
            Transfer2DToWorld(null, isSetParent);           // ���� ��ǥ�� �������� ���� ���      
        else
            Transfer2DToWorld(callerWorldTr, isSetParent);  // ���� ��ǥ�� ������ �� ���
    }


    
    /// <summary>
    /// �������� ����� �߻��� �� �������� �̵���Ű�� ���Ͽ� ȣ������� �ϴ� �޼����Դϴ�.<br/>
    /// �κ��丮�� ���� �� �̵��̳�, �κ��丮->����, ����->������� ��� �߻��� ����մϴ�.<br/><br/>
    /// ����->���� �κ��丮 ���� : ���� �������� �ִٸ� ��ġ�� ��ȯ�մϴ�.<br/>
    /// ����->�ٸ� �κ��丮 ���� : ���ڸ��� ���ٸ� �����մϴ�. ���� �������� �ִٸ� �����մϴ�.<br/>
    /// ����->���� : �����ġ�� �����ϰ� ������ ����մϴ�. ���� ��ǥ���� ����� �ߺ������� �ʽ��ϴ�. <br/>
    /// �κ��丮->���� :���� ��ǥ�� �شٸ� �ش� ���� ��ǥ�� �������� ����Ʈ����, null�� �شٸ� �÷��̾� ��ó�� ����մϴ�.<br/>
    /// </summary>
    /// <returns>���� ��ӿ� ���� �� true�� ���� �� false�� ��ȯ�մϴ�.</returns>
    public bool OnItemSlotDrop( Transform callerSlotTr )
    {
        
        // ȣ���ڰ� �������� �˻�
        if( callerSlotTr==null)
            throw new Exception("������ ������ �����ϴ�. �ùٸ� ���� ����̺�Ʈ ȣ������ Ȯ���Ͽ� �ּ���.");

        // ȣ���ڰ� �������� �˻�
        bool isCallerSlot = callerSlotTr.GetComponent<SlotDrop>() != null;
        bool isPrevCallerSlot = prevDropEventCallerTr.GetComponent<SlotDrop>() != null;

        if( !isCallerSlot )
            throw new Exception("������ �ƴմϴ�. �ùٸ� ���� ����̺�Ʈ ȣ������ Ȯ���Ͽ� �ּ���.");
        else if( !isPrevCallerSlot )
            throw new Exception("����->������ ����̺�Ʈ�� �߻��Ͽ����ϴ�. �ùٸ� ���� ����̺�Ʈ ȣ������ Ȯ���Ͽ� �ּ���.");



        // ������ �κ��丮�� ������ ����->�������� �̵��� ���
        if(callerSlotTr==prevDropEventCallerTr)        
        {   
            return MoveSlotInSameListSlot(callerSlotTr);     
        }
        else
        {
            // ���� ����̺�Ʈ ȣ���ڿ� �θ� ���ٸ�(������ ���� ����Ʈ������ �̵��̶��)
            if( callerSlotTr.parent == prevDropEventCallerTr.parent) 
                return MoveSlotInSameListSlot(callerSlotTr);       // ���� ���� �� �̵�
            else
                return MoveSlotToAnotherListSlot(callerSlotTr);    // Ÿ �κ��丮 ���������� �̵�            
        }   
    }
    



    /// <summary>
    /// �������� 2D���� 3D�� ������ǥ�� �Է��Ͽ� �����ϴ� �޼����Դϴ�.<br/>
    /// null�� ���� �� �÷��̾��� �����ǥ�� �������� ��ӽ�Ű��<br/>
    /// ������ǥ�� �� ��� �ش� ��ġ�� �������� ���۽�ŵ�ϴ�.<br/>
    /// *** ������ �κ��丮 ������ ���� ��� ���ܸ� �߻���ŵ�ϴ�. ***<br/>
    /// </summary>
    /// <returns>����� ���� ������ ���� true�� ��ȯ�մϴ�.</returns>
    private bool Transfer2DToWorld(Transform worldTr, bool isSetParent=false)
    {
        if(inventoryInfo==null)
            throw new Exception("������ �κ��丮 ������ �����ϴ�. 2D->World �޼��� ȣ���� �´��� Ȯ���Ͽ� �ּ���.");

        if(worldTr==null)   // ������ǥ�� ���� ���� ���
        {
            Locate2DToWorld(playerDropTr, isSetParent); // �÷��̾��� �����ġ�� �������, ����� �������� �����մϴ�.
            UpdateInventoryInfo(null);                  // �κ��丮 ������ null������ �����Ͽ� �ֽ�ȭ �մϴ�.
        }
        else                // ������ǥ�� �� ���
        {
            inventoryInfo.RemoveItem(this);         // �� �������� ���� �κ��丮���� �����մϴ�.
            UpdateInventoryInfo(null);              // �κ��丮 ������ null������ �����Ͽ� �ֽ�ȭ �մϴ�.
            Locate2DToWorld(worldTr, isSetParent);  // ���� ��ǥ�� ����� �������� �����մϴ�.
        }
        
        prevDropEventCallerTr = null;           // �����̹Ƿ�, ���� ����̺�Ʈ ȣ���ڸ� null�� ����ϴ�.
        return true;
    }







    /// <summary>
    /// �������� �κ��丮 ������ ����� �� ���ڷ� ���� Transform ������ �������� �ٽ� �������ִ� �޼����Դϴ�.<br/>
    /// ���� �κ��丮 ��Ͽ��� �ش� �������� �����ϰ�, ���ο� �κ��丮�� �ش� �������� ����� �߰��Ͽ� �ݴϴ�.<br/>
    /// </summary>
    private void UpdateInventoryInfo(Transform newInventoryTr)
    {        
        // null���� ���޵� ���� ����� �����ٰ� �Ǵ��մϴ�.
        if(newInventoryTr == null)  
        {
            inventoryTr = null;
            slotListTr = null;
            inventoryInfo = null;
            interactive = null;
            playerTr = null;
            playerDropTr = null;
        }
        else // �ٸ� �κ��丮�� ���޵� ���
        {
            // �κ��丮 ���� ������ ������Ʈ �մϴ�.
            inventoryTr = newInventoryTr;
            slotListTr = inventoryTr.GetChild(0).GetChild(0).GetChild(0);
            inventoryInfo = inventoryTr.GetComponent<InventoryInfo>();
            interactive = inventoryTr.GetComponent<InventoryInteractive>();
            playerTr = inventoryTr.parent.parent;
            playerDropTr = playerTr;    // *****�÷��̾� ������� ���߿� ����*****            
        }      
    }







    /// <summary>
    /// �������� ���� ���Կ��� �ٸ� �������� �̵����� �ִ� �޼����Դϴ�.<br/>
    /// ������ �ڽ��ε����� �о� �鿩 �����ۿ� ������� �ְ�, ������ ������Ʈ�� ��ġ�� ������Ʈ �մϴ�.<br/>
    /// ���� ���Կ� �̹� �ٸ� ������ ������Ʈ�� �ִٸ� ������ �ε��� ������ ��ġ�� ��ȯ�մϴ�.<br/>
    /// </summary>
    /// <returns>����� ���������� �����Ƿ� true�� ��ȯ�մϴ�. (����Ī �Ұ��� ������ ���� ������ �����մϴ�.)</returns>
    private bool MoveSlotInSameListSlot(Transform nextSlotTr)
    {
        int nextSlotIdx = nextSlotTr.GetSiblingIndex();     // ���� ������ �ε��� �ش� ������ �ڽĳѹ��� �����մϴ�.
        
        if( nextSlotTr.childCount==0 )
        {
            // Ȱ��ȭ ���� �ǿ� ���� �� �������� ���� �ε��� ������ �����մϴ�.
            if(isActiveTabAll)
                item.SlotIndex = nextSlotIdx;
            else
                item.SlotIndexAll = nextSlotIdx;

            // �ش� ������ ��ġ������ ������Ʈ �մϴ�.
            UpdatePositionInSlotList();
        }
        else if(nextSlotTr.childCount==1)
        {             
            // ���Կ� ����ִ� �ٲ� �������� ������ �����ɴϴ�.
            ItemInfo switchItemInfo = nextSlotTr.GetChild(0).GetComponent<ItemInfo>(); 
            
            // Ȱ��ȭ ���� �ǿ� ���� �� �������� �ε��� ������ �����մϴ�.
            if(isActiveTabAll)
            {
                switchItemInfo.SlotIndexAll = item.SlotIndexAll;  // �ٲܾ������� �ε����� �� �������� ���� ������ �ε����� �־��ݴϴ�.      
                item.SlotIndexAll = nextSlotIdx;                  // �� �������� �ε����� �ٲ� ������ ��ġ�� �����մϴ�.
            }
            else
            {
                switchItemInfo.SlotIndex = item.SlotIndex;          
                item.SlotIndex = nextSlotIdx;
            }
            
            switchItemInfo.UpdatePositionInSlotList();      // �ٲ� �������� ��ġ ������ ������Ʈ �մϴ�.
            UpdatePositionInSlotList();                     // �� �������� ��ġ ������ ������Ʈ �մϴ�.
        }
        else  // ���Կ� �ڽ��� 2�� �̻��� ��� - ���� ó��
            throw new Exception("���Կ� �ڽ��� 2�� �̻� �����ֽ��ϴ�. Ȯ���Ͽ� �ּ���.");

        prevDropEventCallerTr = nextSlotTr;                 // �����ߴٸ� ���� ����̺�Ʈ ȣ���ڸ� �ֽ�ȭ�մϴ�. 

        return true;    // ����� ���������� �����Ƿ� true�� ��ȯ�մϴ�. (����Ī �Ұ��� ������ ���� ������ �����մϴ�.)
    }



    
    /// <summary>
    /// �������� ���� �κ��丮�� ���Կ��� �ٸ� �κ��丮�� �������� �̵������ִ� �޼����Դϴ�.<br/>
    /// �ش� �κ��丮�� ���� �ڸ��� �ִ��� �ű�� ���� Ȯ���Ͽ� �ڸ��� ����ϴٸ�<br/>
    /// ������ �κ��丮 ��Ͽ��� �� �������� �����ϰ�, �ű� �κ��丮�� ��� ������ �ֽ�ȭ�Ͽ� �ݴϴ�.<br/>
    /// �̴� ��ư ������ �������� �Űܾ� �� ��찡 �ֱ� �����Դϴ�.<br/>
    /// </summary>
    /// <returns>���ο� �κ��丮 ���Կ� ���� �ڸ��� �ִ°�� true��, �����ڸ��� ���ų� �ش� ���Կ� ������ �������� �ִٸ� false�� ��ȯ</returns>
    private bool MoveSlotToAnotherListSlot(Transform nextSlotTr)
    {
        if(nextSlotTr.childCount>=1)        // ���Կ� �������� ����ִٸ�,
        {
            UpdatePositionInSlotList();     // ����ġ�� �ǵ����� ���и� ��ȯ�մϴ�.
            return false;
        }

        Transform nextInventoryTr = nextSlotTr.parent.parent.parent.parent;
        InventoryInfo nextInventoryInfo = GetComponent<InventoryInfo>();

        // ���ο� �κ��丮 ���Կ� ���� �ڸ��� �ִ� ���
        if( nextInventoryInfo.isSlotEnough(this) )
        {
            inventoryInfo.RemoveItem(this);                     // ���� �κ��丮���� �������� �����ؾ� �մϴ�.
            UpdateInventoryInfo(nextInventoryTr);               // �κ��丮 ���� ������ ȣ������ �κ��丮�� ������Ʈ �մϴ�.

            inventoryInfo.AddItem(this);                        // ������Ʈ �� �κ���� �������� �߰��մϴ�. 
            inventoryInfo.SetItemSlotIdxBothToNearstSlot(this); // ���� �ε��� ������ ������Ʈ�մϴ�.    
            UpdatePositionInSlotList();                         // ������Ʈ �� �ε����� �������� ���ο� ���� ��ġ�� �̵���ŵ�ϴ�.
                                                                        
            prevDropEventCallerTr = nextSlotTr;                 // �����ߴٸ� ���� ����̺�Ʈ ȣ���ڸ� �ֽ�ȭ�մϴ�. 
            return true;                                        // ������ ��ȯ�մϴ�.
        }
        // ���ο� �κ��丮 ���Կ� ���� �ڸ��� ���� ���
        else
        {
            UpdatePositionInSlotList();     // ����ġ�� �ǵ����� ���и� ��ȯ�մϴ�.
            return false;
        }
    }






    /// <summary>
    /// �������� 2D ����� �ߴ��ϰų� �ٽ� Ȱ��ȭ��Ű�� �޼����Դϴ�.<br/>
    /// isWorldPositioned�� ������� �ֽ�ȭ�մϴ�.<br/><br/>
    /// ** isWorldPositioned�� ����Ǿ��� ��� �ݵ�� ȣ���ؾ� �մϴ�. **<br/>
    /// </summary>
    private void SwitchAppearAs2D(bool isWorldPositioned)
    {        
        // ���� �� �������� �ִٸ�, UI �̺�Ʈ�� �� �̻� ���� ������, 2D�̹����� ����ó���մϴ�.
        itemCG.blocksRaycasts = !isWorldPositioned;
        itemCG.alpha = isWorldPositioned ? 0f:1f;
    }
           
    /// <summary>
    /// 2D�� 3D ������Ʈ�� �θ�� �ڽİ��踦 �����ϴ� �޼����Դϴ�. ���������� ���˴ϴ�. <br/>
    /// �������ڷ� ������ Transform ������ �ش� ������ Transform ������ �����ؾ� �մϴ�. <br/>
    /// </summary>
    /// <param name="parentTr"></param>
    private void ChangeHierarchy( Transform parentTr )
    {
        if(isWorldPositioned)
        {
            itemTr.SetParent( parentTr );           // 3D������Ʈ�� �θ� �κ��丮���� �ֻ��� ������ ����
            itemRectTr.SetParent(itemTr);           // 2D������Ʈ�� �θ� 3D������Ʈ�� ����   
            itemTr.gameObject.SetActive(true);      // 3D������Ʈ�� Ȱ��ȭ            
        }
        else
        {
            itemRectTr.SetParent( parentTr );           // 2D ������Ʈ�� �θ� �������� ����
            itemTr.SetParent( itemRectTr );             // 3D ������Ʈ�� �θ� 2D ������Ʈ�� ����
            itemTr.gameObject.SetActive( false );       // 3D ������Ʈ ��Ȱ��ȭ
        }
    }


    /// <summary>
    /// �������� 2D UI���� 3D ����� ��ġ������ �־ ���������� �����Ͽ� �̵��� ���ִ� �޼����Դϴ�.<br/>
    /// ���� �θ� ������ �� �ֽ��ϴ�.
    /// </summary>
    private void Locate2DToWorld(Transform worldTr, bool isSetParent=false)
    {   
        // ��ǥ �Է��� ������ �������, ����ó�� 
        if(worldTr==null)
            throw new Exception("�������� �̵� ��ų ���� ��ǥ�� �Է����ּ���.");

        // World���� ���� Ȱ��ȭ
        isWorldPositioned = true;  
                
        // �������� 2D����� �����ϴ�.
        SwitchAppearAs2D(isWorldPositioned);
                    
        // �������� ��ȯ                 
        if( isSetParent )              
            ChangeHierarchy(worldTr);   // 3D������Ʈ�� �θ� �κ��丮���� worldTr�� ����   
        else
            ChangeHierarchy(null);      // 3D������Ʈ�� �θ� �ֻ��� ������ ����

        // 3D������Ʈ ������ ����
        itemTr.position = worldTr.position;
        itemTr.rotation = worldTr.rotation;                        
    }

    /// <summary>
    /// �������� 2D UI���� 3D ����� ��ġ������ �־ ���������� �����Ͽ� �̵��� ���ִ� �޼����Դϴ�.<br/>
    /// �����θ� ������ �� �����ϴ�.
    /// </summary>
    private void Locate2DToWorld(Vector3 worldPos, Quaternion worldRot )
    {
        // World���� ���� Ȱ��ȭ
        isWorldPositioned = true;
                
        // �������� 2D����� �����ϴ�.
        SwitchAppearAs2D( isWorldPositioned );

        // �������� ��ȯ
        ChangeHierarchy(null);
        
        // 3D������Ʈ ������ ����
        itemTr.position = worldPos;
        itemTr.rotation = worldRot;
    }
    




    /// <summary>
    /// �������� 3D ���忡�� �������� ���������� �����Ͽ� �̵��� ���ִ� �޼����Դϴ�. 
    /// </summary>
    /// <returns>�̵��� ������ ���� true�� ������ ���� �̵��� �� ���ٸ� false�� ��ȯ�մϴ�.</returns>
    public bool Locate3DToSlot(Transform slotTr)
    {
        isWorldPositioned = false;              // ������ġ ���¿��θ� ��Ȱ��ȭ �մϴ�.
        
        // �������� 2D����� Ȱ��ȭ �մϴ�.
        SwitchAppearAs2D( isWorldPositioned );
                
        // �������� ��ȯ
        ChangeHierarchy(slotTr);

        // �������� �ε����� ���޹��� ������ ������� �������ݴϴ�.
        item.SlotIndex = slotTr.GetSiblingIndex();  

        // �������� ��ġ������ ������Ʈ ���ݴϴ�.
        //UpdatePositionInSlotList(slotTr);   

        return true;
    }

    /// <summary>
    /// �������� 3D ���忡�� ���Ը���Ʈ�� ���������� �����Ͽ� �̵��� ���ִ� �޼����Դϴ�.<br/>
    /// ���ڷ� ���Ը���Ʈ�� Ʈ�������� �����ε����� �����ؾ��մϴ�.<br/>
    /// ���ڸ� �������� ���� ��� ������ ���� ����� ���� �������� ��ġ�����ݴϴ�.
    /// </summary>
    /// <returns>�̵��� ������ ���� true�� ������ ���� �̵��� �� ���ٸ� false�� ��ȯ�մϴ�.</returns>
    public bool Locate3DToSlotList(Transform slotListTr, int slotIndex=-1)
    {
        // ���ڷ� ���޹��� �ε����� -1���϶��, ���� ����� ������ ã���ϴ�.
        if(slotIndex<=-1)                                       
            slotIndex = FindNearstRemainActiveSlotIdx(slotListTr);

        // ã�ų� ���޹��� ���� �ε����� ������� �������� �־��� ������ �����մϴ�.
        Transform targetSlot = slotListTr.GetChild(slotIndex);

        // ���������� �����Ͽ� ���������� �����ϴ� �޼��带 ȣ���մϴ�.
        Locate3DToSlot(targetSlot); 
    }

    /// <summary>
    /// �������� 3D ���忡�� �κ��丮�� �������� ���������� �����Ͽ� �̵��� ���ִ� �޼����Դϴ�.<br/>
    /// ���޵� �κ��丮�� ������� ���� ����� ������ ã�Ƽ� �������� �־��ݴϴ�. <br/>
    /// </summary>
    /// <param name="inventoryInfo">�κ��丮 ��ũ��Ʈ</param>
    /// <returns>�̵��� ������ ���� true�� ������ ���� �̵��� �� ���ٸ� false�� ��ȯ�մϴ�.</returns>
    public bool Locate3DToInventory(InventoryInfo inventoryInfo)
    {
        return Locate3DToInventory( inventoryInfo.inventory );
    }

    
    /// <summary>
    /// �������� 3D ���忡�� �κ��丮�� �������� ���������� �����Ͽ� �̵��� ���ִ� �޼����Դϴ�.<br/>
    /// ���޵� �κ��丮�� ������� ���� ����� ������ ã�Ƽ� �������� �־��ݴϴ�. <br/>
    /// </summary>
    /// <param name="inventory">�κ��丮 ����</param>
    /// <returns>�̵��� ������ ���� true�� ������ ���� �̵��� �� ���ٸ� false�� ��ȯ�մϴ�.</returns>
    public bool Locate3DToInventory( Inventory inventory )
    {
        return true;
    }




    
    /// <summary>
    /// �ش� ��ȭ �������� ��ø������ �����Ͽ� �ݴϴ�.<br/><br/>
    /// ���� ���ڸ� ���� ���� ��, �ش� �������� �ִ� ��ø������ �ʰ��ϴ� ��� ������ �ʰ������� ��ȯ�մϴ�.<br/><br/>
    /// ���� ���ڸ� ���� ���� ��, �ش� �������� �ּ� ��ø����(0)�� �ʰ��ϴ� ��� 
    /// �������� �ı� �� �κ��丮 ��Ͽ��� �����ϰ�, ������ ������ ��ȯ�մϴ�.<br/><br/>
    /// ***** �ش� �������� ��ȭ�������� �ƴ϶�� ���ܸ� �����ϴ�. *****<br/>
    /// </summary>
    /// <param name="inCount"></param>
    /// <returns></returns>
    public int SetOverlapCount(int inCount)
    {
        if( item.Type!=ItemType.Misc )
            throw new Exception( "��ȭ�������� �ƴմϴ�. Ȯ���Ͽ��ּ���." );

        int remainCount=0;
        //ItemMisc itemMisc = (ItemMisc)item;

        //remainCount = itemMisc.SetOverlapCount(inCount);

        //if(remainCount<=0)  // ������ �ı� �� �κ��丮 ��Ͽ��� ����
        //{
        //    Destroy(this.gameObject, 0.0001f);       // return�� ȣ���� ���Ͽ� �ణ �ʰ� �ı��մϴ�.
        //    inventoryInfo.Remove
        //}

        return remainCount;
    }


}