using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ItemData;
using UnityEngine.UI;
using Unity.VisualScripting;
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
 */



/// <summary>
/// �� ��ũ��Ʈ�� �ݵ�� ������ ������Ʈ(������)�� ��ġ�ؾ� �մϴ�. �������� �̺�Ʈ�� ������ �޾ƾ� �ϱ� �����Դϴ�.
/// </summary>
public class ItemPointerStatusWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{    
    private GameObject statusWindow;    // ����â ������Ʈ
    private Image itemStatusImage;      // ����â�� ������ �̹���
    private Text txtEnhancement;        // ����â�� ������ ��ȭ �ؽ�Ʈ
    private Text txtName;               // ����â�� ������ �̸�
    private Text txtDesc;               // ����â�� ������ ����
    private Text txtSpec;               // ����â�� ������ ����

    private ItemInfo itemInfo;                  // �������� ������ �ִ� ���� ����
    private Item item;                          // ���� �������� �����ϱ� ���� ����
    private ItemImageCollection iicMiscOther;   // ���μ� �̹����� ������ ����ȭ ��ũ��Ʈ ����

    [SerializeField] private readonly int AnyEngraveMaxNum = 3;     // ������ ������� ���� ���� �ִ� (�ǳ� �ִ� ����)
    [SerializeField] private GameObject[] PanelEngraveArr;          // ���� �ǳ�
    [SerializeField] private Transform[] PanelEngraveTrArr;         // ���� �ǳ�
    [SerializeField] private Image[] imageEngraveArr;               // ���� �̹���
    [SerializeField] private Text[] txtNameArr;                     // ���� �̸�     
    [SerializeField] private Text[] txtDescArr;                     // ���� ����

    RectTransform statusRectTr; // ����â�� ��Ʈ Ʈ������
    RectTransform itemRectTr;   // �������� ��Ʈ Ʈ������

    void Start()
    {        
        // ���� ������Ʈ�� ��� ������ ĳ���� Canvas�� 1��° �ڽ��� ����â ������Ʈ�� ������ ������� �մϴ�.
        statusWindow= GameObject.FindWithTag("CANVAS_CHARACTER").transform.GetChild(1).gameObject;

        if(statusWindow == null)
            throw new Exception("����â�� ������ Ȯ���Ͽ� �ּ���.");

        itemStatusImage = statusWindow.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        txtEnhancement = statusWindow.transform.GetChild(1).GetComponent<Text>();
        txtName = statusWindow.transform.GetChild(2).GetComponent<Text>();
        txtDesc = statusWindow.transform.GetChild(3).GetComponent<Text>();
        txtSpec = statusWindow.transform.GetChild(4).GetComponent<Text>();

        itemInfo = GetComponent<ItemInfo>();
        item = itemInfo.Item;

        PanelEngraveArr = new GameObject[AnyEngraveMaxNum];
        imageEngraveArr = new Image[AnyEngraveMaxNum];
        txtNameArr = new Text[AnyEngraveMaxNum];
        txtDescArr = new Text[AnyEngraveMaxNum];

        for(int i=0; i<AnyEngraveMaxNum; i++)
        {
            PanelEngraveArr[i] = statusWindow.transform.GetChild(5+i).gameObject;
            imageEngraveArr[i] = PanelEngraveArr[i].transform.GetChild(0).GetChild(0).GetComponent<Image>();
            txtNameArr[i] = PanelEngraveArr[i].transform.GetChild(1).GetComponent<Text>();
            txtDescArr[i] = PanelEngraveArr[i].transform.GetChild(2).GetComponent<Text>();
        }
        statusRectTr = statusWindow.GetComponent<RectTransform>();
        itemRectTr = this.gameObject.GetComponent<RectTransform>();
        statusRectTr.SetAsLastSibling();  // ����â�� ĵ������ ������ �ڽ����� ��ġ�Ͽ� �̹��� ǥ�� �켱������ ���� �Ѵ�.
                                          
        // ���� ���� �� ����â�� ���д�
        statusWindow.SetActive(false);        

        iicMiscOther=CreateManager.instance.transform.GetChild(0).GetChild(2).gameObject.GetComponent<ItemImageCollection>();
        if(iicMiscOther == null)
            throw new Exception("iicMiscOther�� ������ Ȯ���Ͽ� �ּ���.");

    }
    

    /// <summary>
    /// �����ۿ� Ŀ���� ���� ��� ���� �������� ������ �� �� �ֽ��ϴ�.
    /// </summary>
    public void OnPointerEnter( PointerEventData eventData )
    {
        if( item==null )                            // ������ ������ ����ٸ� �������� �ʴ´�.
            return;


        /*** ������ ���� ������� ���� ���� ***/
        statusWindow.SetActive(true);               // ����â Ȱ��ȭ

        statusRectTr.position = itemRectTr.position 
            + Vector3.right*(statusRectTr.sizeDelta.x/2 + 10f);  
        //����â�� ��ġ�� ������ ��ġ�κ��� ����â ũ��, ������ũ�⸦ ����Ͽ� �������� ���� 10��ŭ ������ �Ÿ��̴�.
    
        


        itemStatusImage.sprite = itemInfo.statusImage.sprite;   // �̹����� ����� statusSprite �̹����� �����ش�.
        txtName.text = item.Name;                               // �̸� �ؽ�Ʈ�� ������ �̸��� �����ش�.
        txtDesc.text = item.Name;                               // ���� �ؽ�Ʈ�� ������ �̸��� �ӽ������� �����ش�.

         for(int i=0; i<AnyEngraveMaxNum; i++)                  // ��� ���� �ǳ��� off�Ѵ�.
                PanelEngraveArr[i].SetActive(false);



        /*** ������ ������ ���� ***/
        if( item.Type == ItemType.Misc )
        { 
            txtEnhancement.enabled = false;         // ��ȭ �ؽ�Ʈ�� ��Ȱ��ȭ
            txtSpec.enabled = false;                // �� ���� �ؽ�Ʈ�� ��Ȱ��ȭ
            txtDesc.text += " " + ((ItemMisc)item).OverlapCount + "��";        // ��ø Ƚ���� ǥ��
        }
        else if(item.Type == ItemType.Weapon)
        {
            txtEnhancement.enabled = true;          // ��ȭ �ؽ�Ʈ�� Ȱ��ȭ
            txtSpec.enabled = true;                 // �� ���� �ؽ�Ʈ�� Ȱ��ȭ

            ItemWeapon itemWeap = (ItemWeapon)item;                     // �������� ���� �ڷ������� ����ȯ

            if(itemWeap.EnhanceNum>0)  // ���� ��ȭ �ܰ谡 1�ܰ� �̻��� ��� ��ȭ �ؽ�Ʈ �Է�
                txtEnhancement.text = "+" + itemWeap.EnhanceNum.ToString(); 
            else
                txtEnhancement.text = "";

            string strGrade;                    // ������ ��� �ѱ� ���ڿ�
            string strType;                     // ������ ���� �ѱ� ���ڿ�
            string strAttr;                     // ������ �Ӽ� �ѱ� ���ڿ�
            
            switch(itemWeap.LastGrade)          // ���� ����� �ѱ� ���ڿ��� ����, �̸��� �÷���
            {
                case Rarity.Normal :
                    strGrade="�븻";
                    txtName.color = new Color(0f, 0f, 0f, 1f);
                    break;
                case Rarity.Magic :
                    strGrade="����";
                    txtName.color = new Color(0/255f, 13/255f, 235/255f, 0.66f);
                    break;        
                case Rarity.Rare :
                    strGrade="����";
                    txtName.color = new Color(138/255f, 81/255f, 192/255f, 0.66f);
                    break;
                case Rarity.Epic :
                    strGrade="����";
                    txtName.color = new Color(78/255f, 0f, 108/255f, 1f);
                    break;
                case Rarity.Unique :
                    strGrade="����ũ";
                    txtName.color = new Color(249/255f, 130/255f, 40/255f, 1f);
                    break;
                case Rarity.Legend :
                    strGrade="������";
                    txtName.color = new Color(1f, 1f, 85/255f, 1f);
                    break;
                default :
                    strGrade="����";
                    break;
            }

            switch(itemWeap.EnumWeaponType)     // ���� ������ �ѱ� ���ڿ��� ����
            {
                case WeaponType.Sword :
                    strType="��";
                    break;
                case WeaponType.Bow :
                    strType="Ȱ";
                    break;
                default :
                    strType="����";
                    break;
            }
                        
            switch(itemWeap.CurAttribute)       // ���� �Ӽ��� �ѱ� ���ڿ��� ����
            {
                case AttributeType.Water :
                    strAttr="��(�)";
                    break;
                case AttributeType.Wind :
                    strAttr="ǳ(��)";
                    break;
                case AttributeType.Earth :
                    strAttr="��(�)";
                    break;
                case AttributeType.Fire :
                    strAttr="ȭ(��)";
                    break;
                case AttributeType.Gold :
                    strAttr="��(��)";
                    break;
                case AttributeType.None :
                    strAttr="��(��)";
                    break;
                default :
                    strAttr="����";
                    break;
            }                                
                        
            txtSpec.text = string.Format(               // ������ �� ���� �ؽ�Ʈ
                $"���: {strGrade}\n" +
                $"���� : {strType}\n" +
                $"���ݷ� : {itemWeap.Power}\n" +
                $"������ : {itemWeap.Durability}\n" +
                $"���ݼӵ� : {itemWeap.Speed:0.00}\n" +
                $"���� : {itemWeap.Weight}\n" +
                $"�Ӽ� : {strAttr}");


            /******** ���� ���� ************/          
            if(itemWeap.RemainEngraveNum > 0 ) // ������ �ϳ��� �����Ǿ� �ִٸ�
            {
                ItemEngraving[] engraveArr = itemWeap.EquipEngraveArrInfo;      // ���� ����ü �迭�� �޾ƿ´�. 
                int curEngraveNum = itemWeap.EquipEngraveNum;                   // ���� ���� ���� ����
                
                for(int i=0; i<curEngraveNum; i++)      // ���� ���� ���� ���� ������ŭ
                { 
                    // ���� �ǳ��� ���ش�.
                    PanelEngraveArr[i].SetActive(true);                         
                    
                    // ����������� ���μ� ���� ����ü���� �ε����� �޾Ƽ� IIC ����ȭ ��ũ��Ʈ���� �̹����� ���� �����Ѵ�.
                    imageEngraveArr[i].sprite = iicMiscOther.icArrImg[engraveArr[i].StatusImageIdx].statusSprite;
                    
                    // �߰� ������ ������ �ݿ��Ѵ�.
                    txtNameArr[i].text = engraveArr[i].Name.ToString();     
                    txtDescArr[i].text = engraveArr[i].Desc;     
                 }
            }

        }
        
    }


    /// <summary>
    /// �����ۿ��� Ŀ���� ���� ���� ������ �������ͽ� â�� ������ϴ�.
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerExit( PointerEventData eventData )
    {
        statusWindow.SetActive(false);      // ����â ��Ȱ��ȭ
    }


}
