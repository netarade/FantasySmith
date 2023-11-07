using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ItemData;
using UnityEngine.UI;
using Unity.VisualScripting;

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
 */



/// <summary>
/// �� ��ũ��Ʈ�� �ݵ�� ������ ������Ʈ(������)�� ��ġ�ؾ� �մϴ�. �������� �̺�Ʈ�� ������ �޾ƾ� �ϱ� �����Դϴ�.
/// </summary>
public class ItemPointerStatusWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{    
    private GameObject statusWindow;    // ����â ������Ʈ
    private Image imageItem;            // ����â�� ������ �̹���
    private Text txtEnhancement;        // ����â�� ������ ��ȭ �ؽ�Ʈ
    private Text txtName;               // ����â�� ������ �̸�
    private Text txtDesc;               // ����â�� ������ ����
    private Text txtSpec;               // ����â�� ������ ����

    private Item item;                  // ���� �������� �����ϱ� ���� ����


    
    /// <summary>
    /// ��� ������ �ν��Ͻ��� �������ͽ� â�� ������ ���� �����ؾ� �ϹǷ� �ݵ�� Awake�� �ξ�� �մϴ�.
    /// </summary>
    void Awake()
    {        
        statusWindow=InventoryManagement.statusWindow;
        imageItem = statusWindow.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        txtEnhancement = statusWindow.transform.GetChild(1).GetComponent<Text>();
        txtName = statusWindow.transform.GetChild(2).GetComponent<Text>();
        txtDesc = statusWindow.transform.GetChild(3).GetComponent<Text>();
        txtSpec = statusWindow.transform.GetChild(4).GetComponent<Text>();
    }
    

    void Start()
    {        
        item = GetComponent<ItemInfo>().item;
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
        statusWindow.transform.localPosition 
            = transform.position + Vector3.left*350f + Vector3.down*600f; //����â�� ��ġ
    
        imageItem.sprite = item.Image.statusSprite; // �̹����� ����� statusSprite �̹����� �����ش�.
        txtName.text = item.Name;                   // �̸� �ؽ�Ʈ�� ������ �̸��� �����ش�.
        txtDesc.text = item.Name;                   // ���� �ؽ�Ʈ�� ������ �̸��� �ӽ������� �����ش�.


        /*** ������ ������ ���� ***/
        if( item.Type == ItemType.Misc )
        { 
            txtEnhancement.enabled = false;         // ��ȭ �ؽ�Ʈ�� ��Ȱ��ȭ
            txtSpec.enabled = false;                // �� ���� �ؽ�Ʈ�� ��Ȱ��ȭ
            txtDesc.text += " " + ((ItemMisc)item).InventoryCount + "��";        // ��ø Ƚ���� ǥ��
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
            

            switch(itemWeap.LastGrade)          // ���� ����� �ѱ� ���ڿ��� ����
            {
                case Rarity.Normal :
                    strGrade="�븻";
                    break;
                case Rarity.Magic :
                    strGrade="����";
                    break;        
                case Rarity.Rare :
                    strGrade="����";
                    break;
                case Rarity.Epic :
                    strGrade="����";
                    break;
                case Rarity.Unique :
                    strGrade="����ũ";
                    break;
                case Rarity.Legend :
                    strGrade="������";
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
                case EnumAttribute.Water :
                    strAttr="��(�)";
                    break;
                case EnumAttribute.Wind :
                    strAttr="ǳ(��)";
                    break;
                case EnumAttribute.Earth :
                    strAttr="��(�)";
                    break;
                case EnumAttribute.Fire :
                    strAttr="ȭ(��)";
                    break;
                case EnumAttribute.Gold :
                    strAttr="��(��)";
                    break;
                case EnumAttribute.None :
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
