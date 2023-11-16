using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

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
    private Item item;               // ��� ������ Ŭ������ ���� ������ ����
    public Image innerImage;        // �������� �κ��丮���� ������ �̹��� (������Ʈ�� �̹��� ������Ʈ�� ���Ѵ�.)
    public Text countTxt;           // ��ȭ �������� ������ �ݿ��� �ؽ�Ʈ
    public Transform slotList;      // �������� ���̰� �� ���� ���� �θ��� ���Ը���Ʈ�� ����

    [SerializeField] ItemImageCollection iicMiscBase;           // �ν����� �� �󿡼� ����� ��ȭ �⺻ ������ �̹��� ����
    [SerializeField] ItemImageCollection iicMiscAdd;            // �ν����� �� �󿡼� ����� ��ȭ �߰� ������ �̹��� ����
    [SerializeField] ItemImageCollection iicMiscOther;          // �ν����� �� �󿡼� ����� ��ȭ ��Ÿ ������ �̹��� ����
    [SerializeField] ItemImageCollection iicWeaponSword;        // �ν����� �� �󿡼� ����� ���� �� ������ �̹��� ����
    [SerializeField] ItemImageCollection iicWeaponBow;          // �ν����� �� �󿡼� ����� ���� Ȱ ������ �̹��� ����

    public void Start()
    {
        countTxt = GetComponentInChildren<Text>();
        slotList = GameObject.Find("Inventory").transform.GetChild(0);
        
        // �ν����ͺ� �󿡼� �޾Ƴ��� ��������Ʈ �̹��� ������ �����Ѵ�.
        iicMiscBase=GameObject.Find( "ImageCollections" ).transform.GetChild( 0 ).GetComponent<ItemImageCollection>();
        iicMiscAdd=GameObject.Find( "ImageCollections" ).transform.GetChild( 1 ).GetComponent<ItemImageCollection>();
        iicMiscOther=GameObject.Find( "ImageCollections" ).transform.GetChild( 2 ).GetComponent<ItemImageCollection>();
        iicWeaponSword=GameObject.Find( "ImageCollections" ).transform.GetChild( 3 ).GetComponent<ItemImageCollection>();
        iicWeaponBow=GameObject.Find( "ImageCollections" ).transform.GetChild( 4 ).GetComponent<ItemImageCollection>();
    }


    /// <summary>
    /// Ŭ�� �� Item �ν��Ͻ��� �����ϰ�, ���� �Ǿ��ִ� �ν��Ͻ��� �ҷ��� �� �ֽ��ϴ�.<br/> 
    /// �������� ����� �� �ڵ����� OnItemChanged()�޼��带 ȣ���Ͽ� ������Ʈ ���� ������ �ݿ��մϴ�.
    /// </summary>
    public Item Item                // �ܺο��� ����������� ������ų �� ȣ���ؾ��� ������Ƽ
    {
        set {
                item =  value;
                OnItemChanged();      // ��������� �������� ������ ���ο��� �ڵ� ȣ�����ش�. 
            }
        get {return item;}
    }


    /// <summary>
    /// �̹��� ������Ʈ�� ��� �켱������ ���̱� ���� OnEnable ���
    /// </summary>
    private void OnEnable()
    {
        innerImage = GetComponent<Image>();
        countTxt = GetComponentInChildren<Text>();
        countTxt.enabled = false;
    }

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
        switch( item.Type ) // �������� ����Ÿ���� �����մϴ�.
        {
            case ItemType.Weapon:
                WeaponType weaponType = ((ItemWeapon)item).EnumWeaponType;  // �������� ����Ÿ���� �����մϴ�.

                if(weaponType==WeaponType.Sword)
                    innerImage.sprite = iicWeaponSword.icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;  
                else if(weaponType==WeaponType.Bow)
                    innerImage.sprite =iicWeaponBow.icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;
                break;

                // ������ ������Ʈ �̹����� �ν����ͺ信 ����ȭ�Ǿ� �ִ� ItemImageCollection Ŭ������
                // ���� ����ü �迭 ImageColection[]�� ����������� ���� ������ ������ �ִ� ImageReferenceIndex ����ü�� �ε����� �����ɴϴ�.                
                
            case ItemType.Misc:
                MiscType miscType = ((ItemMisc)item).EnumMiscType;

                if(miscType==MiscType.Basic)
                    innerImage.sprite = iicMiscBase.icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;
                else if(miscType==MiscType.Additive)
                    innerImage.sprite = iicMiscAdd.icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;
                else
                    innerImage.sprite = iicMiscOther.icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;
                break;
        }
    }


    /// <summary>
    /// ��ȭ �������� ��øȽ���� �������� �����մϴ�. ��ȭ �������� ������ ����� �� ���� ȣ���� �ֽʽÿ�.
    /// </summary>
    public void UpdateCountTxt()
    {
        if( item.Type==ItemType.Misc )                // ��ȭ �������� ��ø ������ ǥ���մϴ�.
        {
            countTxt.enabled=true;
            countTxt.text = ((ItemMisc)item).InventoryCount.ToString();
        }
        else
            countTxt.enabled = false;                // ��ȭ�������� �ƴ϶�� ��ø �ؽ�Ʈ�� ��Ȱ��ȭ�մϴ�.
    }


    // ���ο� ������ ���� ���� �� (Item�ν��Ͻ��� ������ ���ο� ������Ʈ ���� Item�ν��Ͻ��� �������� ��)


    // ���� �������� 0�� �Ǿ��� �� �ı� ����. (���� ��� �˻��ϰ�����? => CraftManagement���� CraftManager.instance.UpdateInventoryText(true); �� ȣ���ϰ� �ִ�)
    // ������. �Ͻ������� -���Ǿ��� �� �ı���ų ���ΰ�
    // ���Ŵ� ������Ʈ �Ӹ� �ƴ϶� �κ��丮�� ����Ʈ������ ������� �Ѵ�. ���ŵ� �������� ���������� �ȵȴ�.

    /// <summary>
    /// 
    /// </summary>
    /// <returns>��ȯ ���� �������� ���� �ѹ��Դϴ�.</returns>
    public int RemoveItemObject()
    {

    }


    // �������� ��ġ���� �ݿ�  (���� ��� ������ ��ġ�� �����ؼ� ���Կ� �־��ְ� �ִ°�? => CreateManager���� Instantiate�Ҷ� �̸� ���δ�.)
    public void UpdatePosition()
    {
        transform.SetParent( slotList.GetChild(item.SlotIndex) );  // ������Ʈ�� �� �θ� ������ �������� ���� ��ġ�� �����Ѵ�.
        transform.localPosition = Vector3.zero;                    // ������ġ�� �θ�������κ��� 0,0,0���� �����.
    }





    /// <summary>
    /// �������� �ı� �Ǳ��� ������ �Ѱ��ֱ� ���� �޼��� (���� �ӽ÷� �̸��� ������ �Ѱ��ְ� ���ο� �������� �����ϴ� �������� �Ǿ� �ִ�.)
    /// </summary>
    private void OnDestroy()
    {
        if( item.Type==ItemType.Misc )
        {
            CraftManager.instance.miscSaveDic.Add( item.Name, ( (ItemMisc)item ).InventoryCount );    //�̸��� ������ ����
            Debug.Log( item.Name );
        }
        else if( item.Type==ItemType.Weapon )
        {
            CraftManager.instance.weapSaveDic.Add( item.Name, 0 );    //�̸��� ����
            Debug.Log( item.Name );
        }
    }

}