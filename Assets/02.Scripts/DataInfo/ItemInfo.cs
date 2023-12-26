using UnityEngine;
using UnityEngine.UI;
using ItemData;

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
    private Item item;              // ��� ������ Ŭ������ ���� ������ ����
    private Image itemImage;        // �������� �κ��丮���� 2D�󿡼� ������ �̹���     

    public Sprite innerSprite;      // �������� �κ��丮���� ������ �̹��� ��������Ʈ
    public Sprite statusSprite;     // �������� ����â���� ������ �̹��� ��������Ʈ

    public Text countTxt;           // ��ȭ �������� ������ �ݿ��� �ؽ�Ʈ
    public Transform slotListTr;    // �������� ���̰� �� ���� ���� �θ��� ���Ը���Ʈ Ʈ������ ����

    public ItemImageCollection[] iicArr;      // �ν����� �� �󿡼� ����� ������ �̹��� ���� �迭
    public enum eIIC { MiscBase,MiscAdd,MiscOther,Sword,Bow }    // �̹��� ���� �迭�� �ε��� ����
    private readonly int iicNum = 5;                             // �̹��� ���� �迭�� ����

    

    /// <summary>
    /// Ŭ�� �� Item �ν��Ͻ��� �����ϰ�, ���� �Ǿ��ִ� �ν��Ͻ��� �ҷ��� �� �ֽ��ϴ�.
    /// </summary>
    public Item Item                // �ܺο��� ����������� ������ų �� ȣ���ؾ��� ������Ƽ
    {
        get; set;
        //set {
        //        item =  value;
        //    }
        //get {return item;}
    }


    /// <summary>
    /// �̹��� ������Ʈ�� ��� �켱������ ���̱� ���� OnEnable ���
    /// </summary>
    private void OnEnable()
    {
        itemImage = GetComponent<Image>();
        countTxt = GetComponentInChildren<Text>();

        Transform canvasTr = GameObject.FindWithTag("CANVAS_CHARACTER").transform;
        slotListTr = canvasTr.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        
        // �ν����ͺ� �󿡼� �޾Ƴ��� ��������Ʈ �̹��� ������ �����մϴ�.
        Transform imageCollectionsTr = GameObject.FindAnyObjectByType<CreateManager>().transform.GetChild(0);

        // �迭�� �ش� ������ŭ �������ݴϴ�.
        iicArr = new ItemImageCollection[iicNum];

        // �� iicArr�� imageCollectionsTr�� ���� �ڽĿ�����Ʈ�μ� ItemImageCollection ��ũ��Ʈ�� ������Ʈ�� ������ �ֽ��ϴ�
        for( int i = 0; i<iicNum; i++)
            iicArr[i] = imageCollectionsTr.GetChild(i).GetComponent<ItemImageCollection>();
        
        // ������ ������Ʈ�� ���� �����Ǹ� ������ ������Ʈ�ϴ� �޼��带ȣ���Ͽ��� �մϴ�.
        //OnItemChanged(); 
    }

    //public void Start()
    //{

    //}

    /// <summary>
    /// ������Ʈ�� item�� ������ �̷�����ٸ� item�� ������ �ִ� �̹����� �ݿ��ϰ� ��ȭ�������� ��� ��ø Ƚ������ �ֽ�ȭ �մϴ�.<br/>
    /// ������Ʈ�� ������ ���� �Է��� ���� ���� �ڵ� ȣ���� �̷������ �޼����̱⿡ ������ ���� ������ ���� ���� ���� ȣ���Ͻø� �˴ϴ�.
    /// </summary>
    public void OnItemChanged()
    {
        UpdateImage();
        UpdateCountTxt();
        UpdatePosition();
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
                   
        //Debug.Log(Item.Type);
        switch( Item.Type )         // �������� ����Ÿ���� �����մϴ�.
        {

            case ItemType.Weapon:
            //Item.ItemDeubgInfo();
                ItemWeapon weapItem = (ItemWeapon)Item;
                WeaponType weaponType = weapItem.eWeaponType;  // �������� ����Ÿ���� �����մϴ�.

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
            //Item.ItemDeubgInfo();

                ItemMisc miscItem = (ItemMisc)Item; 
                MiscType miscType = miscItem.eMiscType;

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
             
        innerSprite = iicArr[imgIdx].icArrImg[Item.sImageRefIndex.innerImgIdx].innerSprite;
        statusSprite = iicArr[imgIdx].icArrImg[Item.sImageRefIndex.statusImgIdx].statusSprite;

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

    
    // �������� ��ġ���� �ݿ�  (���� ��� ������ ��ġ�� �����ؼ� ���Կ� �־��ְ� �ִ°�? => CreateManager���� Instantiate�Ҷ� �̸� ���δ�.)
    public void UpdatePosition()
    {
        if( slotListTr.childCount==0 )
        {
            Debug.Log( "���� ������ �������� ���� �����Դϴ�." );
            return;
        }
        transform.SetParent( slotListTr.GetChild(Item.SlotIndex) );  // ������Ʈ�� �� �θ� ������ �������� ���� ��ġ�� �����Ѵ�.
        transform.localPosition = Vector3.zero;                    // ������ġ�� �θ�������κ��� 0,0,0���� �����.
    }


}