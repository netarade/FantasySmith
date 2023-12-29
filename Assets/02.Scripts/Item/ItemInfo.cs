using UnityEngine;
using UnityEngine.UI;
using ItemData;
using InventoryManagement;

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
 *<v10.0 - 2023_1229_�ֿ���>
 *1- ���� �� Ŭ������ ���� ItemInfo -> Item
 *������ �߽ɱ���� ��ũ��Ʈ���� �̷������ �ϸ�, �������� DtItem �ν��Ͻ��� ����ؾ� �ϱ� ����
 *
 *<v10.1 - 2023-1229_�ֿ���>
 *1- UpdateSlotPosition���� �������ڸ� �߰��Ͽ� ���Ը���Ʈ ������ �ִ� ��쿡�� �ش� ���Ը���Ʈ�� �ε����� �����Ǿ�����Ʈ�� �ϸ�,
 *���Ը���Ʈ ������ ���� ���� ��쿡�� �������� ���� ����ִ� ������ �������� ���Ը���Ʈ�� �����ؼ� �ε����� ���� ������ ������Ʈ�� �ϵ��� ����
 *
 *2-
 *
 *
 *
 *
 *[���� �����ؾ��� ��] 
 * 1- UpdateInventoryPosition�� ���� �ڽ� �κ��丮 �������� �����ϰ� ������,
 * ���߿� UpdatePosition�� �� �� �������� ������ ������ �ε��� �Ӹ� �ƴ϶� ��� �����Կ� ����ִ����� ������ �־�� �Ѵ�.
 *
 *
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
        
    public ItemImageCollection[] iicArr;      // �ν����� �� �󿡼� ����� ������ �̹��� ���� �迭
    public enum eIIC { MiscBase,MiscAdd,MiscOther,Sword,Bow,Axe }    // �̹��� ���� �迭�� �ε��� ����
    private readonly int iicNum = 6;                             // �̹��� ���� �迭�� ����


    private RectTransform itemRectTr;   // �ڱ��ڽ� 2D Ʈ������ ����(�ʱ� ���� - ���� �θ�)
    private Transform itemTr;           // �ڱ��ڽ� 3D Ʈ������ ����(�ʱ� ���� - ���� ������ �ڽ�)


    private CanvasGroup itemCG;             // �������� ĵ���� �׷� ������Ʈ (�������� ����� ������ �� 2D�̺�Ʈ�� �������� �뵵) 

    private bool isWorldPositioned;         // �������� ���忡 �����ִ��� ����
    private InventoryInfo inventoryInfo;    // �������� ����ִ� �κ��丮 ������Ʈ�� ��ũ��Ʈ�� �����մϴ�.
    private Transform slotListTr;           // �������� ����ִ� ���Ը���Ʈ Ʈ������ ����

    
    /// <summary>
    /// �������� ���忡 �����ִ��� (3D ������Ʈ ����) ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool IsWorldPoisitioned { get { return isWorldPositioned; } }
    
    /// <summary>
    /// ���� �������� ��� �κ��丮�� �����Դϴ�.
    /// </summary>
    public InventoryInfo InventoryInfo { get { return inventoryInfo; } set; }

    /// <summary>
    /// ���� �������� ����ִ� ���Ը���Ʈ�� Transform�� ��ȯ�մϴ�.<br/>
    /// ������Ƽ ȣ�� �� �������� ���������� �������� ���Ӱ� ������ �����մϴ�.<br/>
    /// ���� ������ ������ ���� ������ �� �ֽ��ϴ�.<br/> 
    /// </summary>
    public Transform SlotListTr { get { return slotListTr; } }

    



    /// <summary>
    /// �������� ����ִ� ���� ������ ���� �����ϰų� ��ȯ�޽��ϴ�.<br/>
    /// Ŭ�� �� Item �ν��Ͻ��� �����ϰ�, ���� �Ǿ��ִ� �ν��Ͻ��� �ҷ��� �� �ֽ��ϴ�.<br/>
    /// </summary>
    public Item Item                
    {
        set
        {
            item=value;
        }
        get { return item; }
    }


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
    }
    

    /// <summary>
    /// �������� ������ ��� ������ �ֽ�ȭ�մϴ�.(�������� ���������� ������Ʈ�� �ݿ��մϴ�.)<br/>
    /// ������Ʈ�� item�� ������ �̷�����ٸ� item�� ������ �ִ� �̹����� �ݿ��ϰ� ��ȭ�������� ��� ��ø Ƚ������ �ֽ�ȭ �մϴ�.<br/>
    /// **** �ܺ� ��ũ��Ʈ���� �� ��ũ��Ʈ�� �޼��带 ������ �ʰ� ������Ƽ�� �޾� ������ ���� ������ ���� �������� �� �ֽ� ���� �ݿ��� ���Ͽ� ���� ȣ���ؾ� �մϴ�. ****<br/>
    /// </summary>
    /// <param name="SlotListTr">�������� ��� ���� ����Ʈ�� Ʈ������</param>
    public void OnItemChanged(Transform SlotListTr=null)
    {
        UpdateImage();
        UpdateCountTxt();
        UpdatePositionInSlotList(slotListTr);
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
    /// </summary>
    public void UpdatePositionInSlotList(Transform slotListTr = null)
    {
        if( isWorldPositioned )   // �������� ����� �����ִٸ� �������� �ʽ��ϴ�.
        {
            Debug.Log( "�������� ���忡 �����ִ� �����̹Ƿ� ��ġ������ ������Ʈ �� �� �����ϴ�." );
            return;
        }

        // ���ڷ� ���Ը���Ʈ Ʈ�������� �������� �ʾҴٸ�, ���� �������� ����ִ� ������ �θ� Ȯ���Ͽ� �����մϴ�.
        if(slotListTr == null )
            slotListTr = itemRectTr.parent.parent;  

        // ���� ����Ʈ�� ������ �����Ǿ����� �ʴٸ� ���������� �������� �ʽ��ϴ�.
        if( slotListTr.childCount==0 )
        {
            Debug.Log( "���� ������ �������� ���� �����Դϴ�." );
            return;
        }

        // ������Ʈ�� �� �θ� ������ ������ ����Ǿ��ִ� ���� ��ġ�� ���� �� �� ������ġ�� ���Կ� ����ϴ�.
        itemRectTr.SetParent( slotListTr.GetChild(item.SlotIndex) );  
        itemRectTr.localPosition = Vector3.zero;                      
    }


    /// <summary>
    /// �����ִ� ���� �߿��� ���� ���� �ε����� ��ȯ�մϴ�. ������ �� ����� -1�� ��ȯ�մϴ�.
    /// </summary>
    public int FindNearstRemainSlotIdx( Transform slotListTr = null ) 
    {
        // ���Ը���Ʈ ������ �������� �ʾҴٸ�, ���� �������� ��� ������ ������� ���Ը���Ʈ�� �����մϴ�.
        if(slotListTr==null)
            slotListTr = itemRectTr.parent.parent;

        int findIdx = -1;

        for( int i = 0; i<slotListTr.childCount; i++ )
        {
            if( slotListTr.GetChild(i).childCount!=0 )  // �ش� ���Ը���Ʈ�� �ڽ��� �ִٸ� ���� ���Ը���Ʈ�� �Ѿ�ϴ�.
                continue;

            findIdx = i;
            break;
        }

        // findIdx�� �������� �ʾҴٸ� -1�� ��ȯ�մϴ�. �����Ǿ��ٸ� 0�̻��� �ε������� ��ȯ�մϴ�.
        return findIdx;     
    }



    /// <summary>
    /// �������� 2D ����� �ߴ��ϰų� �ٽ� Ȱ��ȭ��Ű�� �޼����Դϴ�.<br/>
    /// ���ڷ� ���� ���忡 ������ ���� ���θ� �����Ͽ��� �մϴ�.
    /// </summary>
    /// <param name="isWorldPositioned"></param>
    private void TurnOnOffOperationAs2D(bool isWorldPositioned)
    {        
        // ���� �� �������� �ִٸ�, UI �̺�Ʈ�� �� �̻� ���� ������, 2D�̹����� ����ó���մϴ�.
        itemCG.blocksRaycasts = !isWorldPositioned;
        itemCG.alpha = isWorldPositioned ? 0f:1f;
    }



    /// <summary>
    /// �������� 2D UI���� 3D ����� ��ġ������ �־ ���������� �����Ͽ� �̵��� ���ִ� �޼����Դϴ�.<br/>
    /// ���� �θ� ������ �� �ֽ��ϴ�.
    /// </summary>
    /// <returns>�̵��� ������ ���� true��, ��ġ������ �߸��Ǿ� �̵��� �� ���ٸ� false�� ��ȯ�մϴ�.</returns>
    public bool Locate2DToWorld(Transform worldTr, bool setParentMode=false)
    {   
        // World���� ���� Ȱ��ȭ
        isWorldPositioned = true;
                
        // 3D������Ʈ ������ ����
        itemTr.position=worldTr.position;
        itemTr.rotation=worldTr.rotation;

        // �������� ��ȯ                  
        itemTr.SetParent(null);             // 3D������Ʈ�� �θ� �κ��丮���� �ֻ��� ������ ����
        itemTr.gameObject.SetActive(true);  // 3D������Ʈ�� Ȱ��ȭ
        itemRectTr.SetParent(itemTr);       // 2D������Ʈ�� �θ� 3D������Ʈ�� ����

        // Mode���� ���� �� �θ� �缳��
        if( setParentMode )                 
            itemTr.SetParent(worldTr);
                
        // �������� 2D����� �ߴ��մϴ�.
        TurnOnOffOperationAs2D( isWorldPositioned );
        return true;
    }

    /// <summary>
    /// �������� 2D UI���� 3D ����� ��ġ������ �־ ���������� �����Ͽ� �̵��� ���ִ� �޼����Դϴ�.<br/>
    /// �����θ� ������ �� �����ϴ�.
    /// </summary>
    /// <returns>�̵��� ������ ���� true��, ��ġ ������ �߸��Ǿ� �̵��� �� ���ٸ� false�� ��ȯ�մϴ�.</returns>
    public bool Locate2DToWorld(Vector3 worldPos, Quaternion worldRot )
    {
        // World���� ���� Ȱ��ȭ
        isWorldPositioned = true;

        // 3D������Ʈ ������ ����
        itemTr.position = worldPos;
        itemTr.rotation = worldRot;
        
        // �������� ��ȯ
        itemTr.SetParent(null);             // 3D������Ʈ�� �θ� �κ��丮���� �ֻ��� ������ ����
        itemTr.gameObject.SetActive(true);  // 3D������Ʈ�� Ȱ��ȭ
        itemRectTr.SetParent(itemTr);       // 2D������Ʈ�� �θ� 3D������Ʈ�� ����
                
        // �������� 2D����� �ߴ��մϴ�.
        TurnOnOffOperationAs2D( isWorldPositioned );

        return true;
    }
    
    /// <summary>
    /// �������� 3D ���忡�� �������� ���������� �����Ͽ� �̵��� ���ִ� �޼����Դϴ�. 
    /// </summary>
    /// <returns>�̵��� ������ ���� true�� ������ ���� �̵��� �� ���ٸ� false�� ��ȯ�մϴ�.</returns>
    public bool Locate3DToSlot(Transform slotTr)
    {
        // ���� �������� ���忡 �����ִ� ���¶��,
        if( isWorldPositioned )  
        {
            isWorldPositioned = false;              // ������ġ ���¿��θ� ��Ȱ��ȭ �մϴ�.
            itemRectTr.SetParent( slotTr );         // 2D ������Ʈ�� �θ� �������� ����
            itemTr.SetParent( itemRectTr );         // 3D ������Ʈ�� �θ� 2D ������Ʈ�� ����
            itemTr.gameObject.SetActive( false );   // 3D ������Ʈ ��Ȱ��ȭ
        }
                
        // �������� 2D����� Ȱ��ȭ�մϴ�.
        TurnOnOffOperationAs2D( isWorldPositioned );

        // �������� �ε����� ���޹��� ������ ������� �������ݴϴ�.
        item.SlotIndex = slotTr.GetSiblingIndex();  

        // �������� ��ġ������ ������Ʈ ���ݴϴ�.
        UpdatePositionInSlotList();   

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
            slotIndex = FindNearstRemainSlotIdx(slotListTr);

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

}