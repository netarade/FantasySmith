using UnityEngine;
using UnityEngine.EventSystems;
using ItemData;
using UnityEngine.UI;
using System;

/* [�۾� ����]
 * <v1.0 - 2023_1105_�ֿ���>
 * 1- �ʱ� ���� �ۼ�
 * Ŀ���� ���ٴ�� ���� ������ �������ͽ� â�� �� �� ������
 * ���� ���� �������ͽ� â�� �������� �Ͽ���.
 * �������ͽ� â�� ������ ������ �ݿ�
 * 
 * <v1.1 - 2023_1106_�ֿ���>
 * 1- txtDesc�� txtSpec ������ ����
 * 2- ����â�� ����� ��ȭ�������� ������ �ݿ��ϵ��� ����
 * 
 * <v1.2 - 2023_1106_�ֿ���>
 * 1- ��ȭ�������� ����â ���� �̸��� ������ ǥ�õǵ��� ����
 * 2- ����â ������ �����Ҵ����δ� ����� ������ �ʾ� ���� ���� �� �����ϴ� ������ �̸� ��Ƴ����� InventoryManagement�� static ���� ������ ����.
 * 
 * <v2.0 - 2023_1109_�ֿ���>
 * 1- ���� �ǳ� �߰� �� ���� Ȱ��ȭ ����, ���� �ݿ�
 * 
 * <v2.1 - 2023_1112_�ֿ���>
 * 1- ����â ��ġ�� ������ �ٷ� ������ ���� �� �ְ� ����
 * 2- ����â�� �ٸ� �̹����� ��ġ�� ��� ���� �տ� ǥ�õ� �� �ֵ��� ����
 * 
 * <v3.0 - 2023_1216_�ֿ���
 * 1- statusWindow�� ������ PlayerInven���� �����ϴ� ���� ���� ĳ���� ĵ������ �±׷� ã�� �����ϵ��� ����
 * 2- ����â�� ���� �� �������� �ణ ����
 * 
 * <v3.1 -2023_1217_�ֿ���>
 * 1- ���μ� ���� ����ü���� �ε����� �޾Ƽ� IIC ����ȭ ��ũ��Ʈ���� �̹����� �����Ͽ� ���μ� �̹����� �ݿ��ϵ��� ���� 
 * 2- iicMiscOther�� ������ CreateManager�� �̱����� ���� ������ ����
 * 
 * <v4.0 - 2023_1223_�ֿ���>
 * 1- ����â ��ġ ����
 * �κ��丮 �������� ��� ���� ���� ���� �ϴ����� ������ �����ְ�, �ϴܿ� ���� ���� ���� ������� �÷��� �����ְ� ����
 * 
 * <v4.1 - 2023_1226_�ֿ���>
 * 1- ���Ը���Ʈ ������ �����ۿ�����Ʈ �������� �����ִ� �� ����
 * �������� ���Ը���Ʈ �ܺο� �����Ǿ����� ��� ���Ը���Ʈ�� �������� ���ϰ� �Ǳ� ����
 * 
 * 2- ������ SlotListTr�� SlotListRectTr�� ����
 * 
 * 3- GameObject������ statusWindow�� Transform������ statusWindowTr�� ����
 * 
 * <v4.2 - 2023_1227_�ֿ���>
 * 1- statusWindowTr�� ������ ���ο� �ӽú��� Transform canvasTr������� ����
 * 
 * <v4.3 - 2023_1229_�ֿ���>
 * 1- ����â �������� ���� (�κ��丮 ���� ������ �ڽ��ε���)���� ���� ���� ����
 * 
 * <v4.4 - 2024_0105_�ֿ���>
 * 1- iicMiscOther�� ������ CreateManager�� �̱����������� FindWithTag������ �ӽ� ���� (���� �������ɼ� ����)
 * 
 * <v5.0 - 2024_0106_�ֿ���>
 * 1- Ŭ������ ���ϸ� ItemPointerStatusWindow���� StatusWindowInteractive�� ����
 * �����ۿ� �����Ͽ��� ��ũ��Ʈ�� �κ��丮�� �����ϱ�� ����
 * ������ �������� �Ź� �κ��丮�� �Űܴٴϱ� ������ ����â�� ������ �Ź� �ٸ��� �޾Ƽ� ����â�� ����� �ϸ�,
 * ��� �������� ����â �ڵ带 ������ �ִ� �ͺ��� ����â�� �޼��带 �ΰ� ������ �̺�Ʈ �߻� �� �ش� �޼��带 ȣ���ϱ⸸ �ϸ� �Ǳ� ����
 * 
 * 
 * 2- �ʿ���� ���� ���� �� ������ ����
 * ������ ���� itemStatusImage->statusImage
 * ������ ���� statusWIndorTr (statusRectTr�� ��ġ�� ����)
 * ������� itemInfo, item���� (����â�� ��� �� ItemInfo�� �޸��ؼ� �ޱ� ����)
 * ������� slotListRectTr ���� (����â�� �����ġ�� �Ǵ� �����̵Ǵ� �����̳� inventoryRectTr�� ��ü)
 * 
 * <5.1 - 2024_0107_�ֿ���>
 * 1- InventoryInteractive�� IsItemSelecting ���¿� ���� ����â�� ����� �ʵ��� ����
 * 
 */



/// <summary>
/// �� ��ũ��Ʈ�� �ݵ�� ������ ������Ʈ(������)�� ��ġ�ؾ� �մϴ�. �������� �̺�Ʈ�� ������ �޾ƾ� �ϱ� �����Դϴ�.
/// </summary>
public class StatusWindowInteractive : MonoBehaviour
{    
    RectTransform statusRectTr; // ����â�� ��Ʈ Ʈ������
    Image statusImage;          // ����â�� ������ �̹���
    Text txtEnhancement;        // ����â�� ������ ��ȭ �ؽ�Ʈ
    Text txtName;               // ����â�� ������ �̸�
    Text txtDesc;               // ����â�� ������ ����
    Text txtSpec;               // ����â�� ������ ����

    readonly int AnyEngraveMaxNum = 3;     // ������ ������� ���� ���� �ִ� (�ǳ� �ִ� ����)
    Transform[] PanelEngraveTrArr;         // ���� �ǳ�
    Image[] imageEngraveArr;               // ���� �̹���
    Text[] txtNameArr;                     // ���� �̸�     
    Text[] txtDescArr;                     // ���� ����
        
    private ItemImageCollection iicMiscOther;   // ���μ� �̹����� ������ ����ȭ ��ũ��Ʈ ����
    RectTransform inventoryRectTr;
    InventoryInteractive inventoryInteractive;



    void Start()
    {        
        // ���� ������Ʈ�� ��� ������ 0��° �ڽ��� �κ��丮 ������Ʈ�� ������ �ڽ��� ����â ������Ʈ ������ ������� �մϴ�.
        inventoryRectTr = GetComponent<RectTransform>();
        inventoryInteractive = GetComponent<InventoryInteractive>();
        statusRectTr = inventoryRectTr.GetChild(inventoryRectTr.childCount-1).GetComponent<RectTransform>();

        // ����â �� ���� ������Ʈ ���� ����
        statusImage = statusRectTr.GetChild(0).GetChild(0).GetComponent<Image>();
        txtEnhancement = statusRectTr.GetChild(1).GetComponent<Text>();
        txtName = statusRectTr.GetChild(2).GetComponent<Text>();
        txtDesc = statusRectTr.GetChild(3).GetComponent<Text>();
        txtSpec = statusRectTr.GetChild(4).GetComponent<Text>();

        // ���� �����迭 ��� �� ũ�� �Ҵ�
        PanelEngraveTrArr = new Transform[AnyEngraveMaxNum];
        imageEngraveArr = new Image[AnyEngraveMaxNum];
        txtNameArr = new Text[AnyEngraveMaxNum];
        txtDescArr = new Text[AnyEngraveMaxNum];

        // ���� �ǳ� �� ���� ������Ʈ ���� ����
        for(int i=0; i<AnyEngraveMaxNum; i++)
        {
            PanelEngraveTrArr[i] = statusRectTr.GetChild(5+i);  //�����ǳ��� ����â�� 5���ڽĺ��� ����
            imageEngraveArr[i] = PanelEngraveTrArr[i].GetChild(0).GetChild(0).GetComponent<Image>();
            txtNameArr[i] = PanelEngraveTrArr[i].GetChild(1).GetComponent<Text>();
            txtDescArr[i] = PanelEngraveTrArr[i].GetChild(2).GetComponent<Text>();
        }
              
        // ���� ���� �� ����â�� ���Ӵϴ�.
        statusRectTr.gameObject.SetActive(false); 
                                                                    

        // ���μ� �̹����� �������� ���� ��� ���� �����Ƿ�, ���μ� �̹����� ������ ��θ� �޾ƿɴϴ�.
        Transform controllerTr = GameObject.FindWithTag("GameController").transform;
        iicMiscOther = controllerTr.GetChild(0).GetChild(2).gameObject.GetComponent<ItemImageCollection>();

        if(iicMiscOther == null)
            throw new Exception("iicMiscOther�� ������ Ȯ���Ͽ� �ּ���.");

    }
    

    /// <summary>
    /// �����ۿ� Ŀ���� ���� ��� ���� �������� ������ �� �� �ֽ��ϴ�.
    /// </summary>
    public void OnItemPointerEnter( ItemInfo itemInfo )
    {
        if(itemInfo == null)
            throw new Exception("�ش� �������� ������ ���޵��� �ʾҽ��ϴ�. Ȯ���Ͽ� �ּ���.");
        
        // �������� ������ ���¶�� �������� �ʽ��ϴ�.
        if( inventoryInteractive.IsItemSelecting )
            return;


        RectTransform itemRectTr = itemInfo.gameObject.GetComponent<RectTransform>();
        Item item = itemInfo.Item;


        /*** ������ ���� ������� ���� ���� ***/

        statusRectTr.gameObject.SetActive( true );        // ����â Ȱ��ȭ


        float delta = inventoryRectTr.position.y-itemRectTr.position.y;         // ����â�� ��� ��ġ �Ǵܱ���(�κ��丮�� �������� y��ġ ����)
        Vector3 rightPadding = Vector3.right*( statusRectTr.sizeDelta.x/2+30f );  // ����â ������ ����
        Vector3 upPadding = Vector3.up*( statusRectTr.sizeDelta.y/2 );              // ����â ��� ����


        if( delta<statusRectTr.sizeDelta.y/3 )          // �κ��丮 ��ܿ��� 1/3�̸� �������ִٸ�,
            statusRectTr.position=itemRectTr.position+rightPadding-upPadding;
        else if( delta<statusRectTr.sizeDelta.y*2/3 )   // �κ��丮 ��ܿ��� 2/3�̸� ������ �ִٸ�,
            statusRectTr.position=itemRectTr.position+rightPadding;
        else                                             // �κ��丮 ��ܿ��� 2/3�̻� ������ �ִٸ�,
            statusRectTr.position=itemRectTr.position+rightPadding+upPadding;

        statusImage.sprite=itemInfo.statusSprite;         // �̹����� ����� statusSprite �̹����� �����ش�.
        txtName.text=item.Name;                           // �̸� �ؽ�Ʈ�� ������ �̸��� �����ش�.
        txtDesc.text=item.Name;                           // ���� �ؽ�Ʈ�� ������ �̸��� �ӽ������� �����ش�.


        // ��� ���� �ǳ��� off�Ѵ�.
        for( int i = 0; i<AnyEngraveMaxNum; i++ )
            PanelEngraveTrArr[i].gameObject.SetActive( false );






        /*** ������ ������ ���� ***/
        if( item.Type==ItemType.Misc )
        {
            txtEnhancement.enabled=false;         // ��ȭ �ؽ�Ʈ�� ��Ȱ��ȭ
            txtSpec.enabled=false;                // �� ���� �ؽ�Ʈ�� ��Ȱ��ȭ
            txtDesc.text+=" "+( (ItemMisc)item ).OverlapCount+"��";        // ��ø Ƚ���� ǥ��
        }
        else if( item.Type==ItemType.Weapon )
        {
            txtEnhancement.enabled=true;          // ��ȭ �ؽ�Ʈ�� Ȱ��ȭ
            txtSpec.enabled=true;                 // �� ���� �ؽ�Ʈ�� Ȱ��ȭ

            ItemWeapon itemWeap = (ItemWeapon)item;                     // �������� ���� �ڷ������� ����ȯ

            if( itemWeap.EnhanceNum>0 )  // ���� ��ȭ �ܰ谡 1�ܰ� �̻��� ��� ��ȭ �ؽ�Ʈ �Է�
                txtEnhancement.text="+"+itemWeap.EnhanceNum.ToString();
            else
                txtEnhancement.text="";

            string strGrade;                    // ������ ��� �ѱ� ���ڿ�
            string strType;                     // ������ ���� �ѱ� ���ڿ�
            string strAttr;                     // ������ �Ӽ� �ѱ� ���ڿ�

            switch( itemWeap.LastGrade )          // ���� ����� �ѱ� ���ڿ��� ����, �̸��� �÷���
            {
                case Rarity.Normal:
                    strGrade="�븻";
                    txtName.color=new Color( 0f, 0f, 0f, 1f );
                    break;
                case Rarity.Magic:
                    strGrade="����";
                    txtName.color=new Color( 0/255f, 13/255f, 235/255f, 0.66f );
                    break;
                case Rarity.Rare:
                    strGrade="����";
                    txtName.color=new Color( 138/255f, 81/255f, 192/255f, 0.66f );
                    break;
                case Rarity.Epic:
                    strGrade="����";
                    txtName.color=new Color( 78/255f, 0f, 108/255f, 1f );
                    break;
                case Rarity.Unique:
                    strGrade="����ũ";
                    txtName.color=new Color( 249/255f, 130/255f, 40/255f, 1f );
                    break;
                case Rarity.Legend:
                    strGrade="������";
                    txtName.color=new Color( 1f, 1f, 85/255f, 1f );
                    break;
                default:
                    strGrade="����";
                    break;
            }

            switch( itemWeap.WeaponType )     // ���� ������ �ѱ� ���ڿ��� ����
            {
                case WeaponType.Sword:
                    strType="��";
                    break;
                case WeaponType.Bow:
                    strType="Ȱ";
                    break;
                default:
                    strType="����";
                    break;
            }

            switch( itemWeap.CurrentAttribute )       // ���� �Ӽ��� �ѱ� ���ڿ��� ����
            {
                case AttributeType.Water:
                    strAttr="��(�)";
                    break;
                case AttributeType.Wind:
                    strAttr="ǳ(��)";
                    break;
                case AttributeType.Earth:
                    strAttr="��(�)";
                    break;
                case AttributeType.Fire:
                    strAttr="ȭ(��)";
                    break;
                case AttributeType.Gold:
                    strAttr="��(��)";
                    break;
                case AttributeType.None:
                    strAttr="��(��)";
                    break;
                default:
                    strAttr="����";
                    break;
            }

            txtSpec.text=string.Format(               // ������ �� ���� �ؽ�Ʈ
                $"���: {strGrade}\n"+
                $"���� : {strType}\n"+
                $"���ݷ� : {itemWeap.Power}\n"+
                $"������ : {itemWeap.Durability}\n"+
                $"���ݼӵ� : {itemWeap.Speed:0.00}\n"+
                $"���� : {itemWeap.Weight}\n"+
                $"�Ӽ� : {strAttr}" );


            /******** ���� ���� ************/
            if( itemWeap.RemainEngraveNum>0 ) // ������ �ϳ��� �����Ǿ� �ִٸ�
            {
                ItemEngraving[] engraveArr = itemWeap.EquipEngraveArrInfo;      // ���� ����ü �迭�� �޾ƿ´�. 
                int curEngraveNum = itemWeap.EquipEngraveNum;                   // ���� ���� ���� ����

                for( int i = 0; i<curEngraveNum; i++ )      // ���� ���� ���� ���� ������ŭ
                {
                    // ���� �ǳ��� ���ش�.
                    PanelEngraveTrArr[i].gameObject.SetActive( true );

                    // ����������� ���μ� ���� ����ü���� �ε����� �޾Ƽ� IIC ����ȭ ��ũ��Ʈ���� �̹����� ���� �����Ѵ�.
                    imageEngraveArr[i].sprite=iicMiscOther.icArrImg[engraveArr[i].StatusImageIdx].statusSprite;

                    // �߰� ������ ������ �ݿ��Ѵ�.
                    txtNameArr[i].text=engraveArr[i].Name.ToString();
                    txtDescArr[i].text=engraveArr[i].Desc;
                }
            }

        }

    }


    /// <summary>
    /// �����ۿ��� Ŀ���� ���� ���� ������ �������ͽ� â�� ������ϴ�.
    /// </summary>
    public void OnItemPointerExit()
    {
        statusRectTr.gameObject.SetActive( false );      // ����â ��Ȱ��ȭ
    }


}
