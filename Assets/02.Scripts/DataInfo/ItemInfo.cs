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
    public Image innerImage;        // �������� �κ��丮���� ������ �̹��� (������Ʈ�� �̹��� ������Ʈ�� ���Ѵ�.)
    public Image statusImage;       // �������� ����â���� ������ �̹��� (������Ʈ�� �̹��� ������Ʈ)
    public Text countTxt;           // ��ȭ �������� ������ �ݿ��� �ؽ�Ʈ
    public Transform slotListTr;    // �������� ���̰� �� ���� ���� �θ��� ���Ը���Ʈ Ʈ������ ����

    public ItemImageCollection[] iicArr;      // �ν����� �� �󿡼� ����� ������ �̹��� ���� �迭
    public enum eIIC { MiscBase,MiscAdd,MiscOther,Sword,Bow }    // �̹��� ���� �迭�� �ε��� ����
    public void Start()
    {
        countTxt = GetComponentInChildren<Text>();
        slotListTr = GameObject.FindWithTag("CANVAS_CHRACTER").transform.GetChild(0).GetChild(0);
        
        // �ν����ͺ� �󿡼� �޾Ƴ��� ��������Ʈ �̹��� ������ �����մϴ�.
        Transform imageCollectionsTr = CreateManager.instance.transform.GetChild(0);

        // �� iicArr�� imageCollectionsTr�� ���� �ڽĿ�����Ʈ�μ� ItemImageCollection ��ũ��Ʈ�� ������Ʈ�� ������ �ֽ��ϴ�
        for( int i = 0; i<5; i++)
            iicArr[i] = imageCollectionsTr.GetChild(i).GetComponent<ItemImageCollection>();
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

                if( weaponType==WeaponType.Sword )
                {
                    innerImage.sprite=iicArr[((int)eIIC.Sword)].icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;
                    statusImage.sprite=iicArr[(int)eIIC.Bow].icArrImg[item.sImageRefIndex.statusImgIdx].statusSprite;
                }
                else if( weaponType==WeaponType.Bow )
                {
                    innerImage.sprite=iicArr[(int)eIIC.Bow].icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;
                    statusImage.sprite=iicArr[(int)eIIC.Bow].icArrImg[item.sImageRefIndex.statusImgIdx].statusSprite;
                }
                break;
                // ������ ������Ʈ �̹����� �ν����ͺ信 ����ȭ�Ǿ� �ִ� ItemImageCollection Ŭ������
                // ���� ����ü �迭 ImageColection[]�� ����������� ���� ������ ������ �ִ� ImageReferenceIndex ����ü�� �ε����� �����ɴϴ�.                
                
            case ItemType.Misc:
                MiscType miscType = ((ItemMisc)item).eMiscType;

                if( miscType==MiscType.Basic )
                {
                    innerImage.sprite=iicArr[(int)eIIC.MiscBase].icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;
                    statusImage.sprite=iicArr[(int)eIIC.MiscBase].icArrImg[item.sImageRefIndex.statusImgIdx].statusSprite;
                }
                else if( miscType==MiscType.Additive )
                {
                    innerImage.sprite=iicArr[(int)eIIC.MiscAdd].icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;
                    statusImage.sprite=iicArr[(int)eIIC.MiscAdd].icArrImg[item.sImageRefIndex.statusImgIdx].statusSprite;
                }
                else
                {
                    innerImage.sprite=iicArr[(int)eIIC.MiscOther].icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;
                    statusImage.sprite=iicArr[(int)eIIC.MiscOther].icArrImg[item.sImageRefIndex.statusImgIdx].statusSprite;
                }
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
            countTxt.text = ((ItemMisc)item).OverlapCount.ToString();
        }
        else
            countTxt.enabled = false;                // ��ȭ�������� �ƴ϶�� ��ø �ؽ�Ʈ�� ��Ȱ��ȭ�մϴ�.
    }

    
    // �������� ��ġ���� �ݿ�  (���� ��� ������ ��ġ�� �����ؼ� ���Կ� �־��ְ� �ִ°�? => CreateManager���� Instantiate�Ҷ� �̸� ���δ�.)
    public void UpdatePosition()
    {
        transform.SetParent( slotListTr.GetChild(item.SlotIndex) );  // ������Ʈ�� �� �θ� ������ �������� ���� ��ġ�� �����Ѵ�.
        transform.localPosition = Vector3.zero;                    // ������ġ�� �θ�������κ��� 0,0,0���� �����.
    }


}