using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * [�۾� ����]  
 * <v1.0 - 2024_0114_�ֿ���>
 * 1- ���� ������ ����ڰ� ������ �� �ֵ��� ��� ���� ����
 * 
 * 2- Ư�� ������ �������� ���� ȣ���� �޼��� ���� (SlotActivate�޼���)
 * 
 * 3- ������ ���� �� �ӽ� �̹����� ������ ���� ������Ʈ�� �̹��� ���� ����.
 * 
 * 4- ���Կ� ���� ������ �˷��ִ� slotItemInfo�迭, slotLen, isItemPlaced�迭, isItemEquipped�迭 ������ �ʱ�ȭ
 * 
 * <v1.1 - 2024_0114_�ֿ���>
 * 1- SlotActivate�޼��� ��ȯ �����߰� (������ ������ �ȵ��ְų�, �̹� �������̶��)
 * 
 * 2- SlotActivate�޼��忡�� 
 * (������ ���� �� AddItem�� �ڵ����� �̷���� ���¿���) ���� �� RemoveItem�� �Ͽ� �÷��̾��� Equip��ҷ� �����鼭
 * ���̸� emptyList���� ���������� ������ �ش� �������� 2D �̹����� �����ִ� ������� ����
 * 
 * 3- SlotActivate�޼��忡�� HandŬ�� �� Uequip�� �̷�����鼭 ���̸� �ٽ� emptyList�� ������,
 * �÷��̾� Equip��ҿ��� Quick�������� AddItem��Ű���� ����
 * 
 * 
 * [�̽�]
 * ���Ե�� Ʃ�� - isItemPlaced true ����
 * 
 */


[Serializable]
public class EquipmentTransform
{
    public Transform axeTr;
    public Transform bluntTr;
    public Transform spearTr;
    public Transform swordTr;
    public Transform bowTr;        
}




public class QuickSlot : InventoryInfo
{
    
    [SerializeField] WeaponType[] weaponTypes;      // ���� ���� ����
    [SerializeField] EquipmentTransform equipTr;    // ��� ������ Ʈ������ ����


    Transform[] dummyTr;        // ���̿� ������Ʈ�� Transform
    Image[] dummyImg;           // ���̿� �̹���
    ItemInfo[] slotItemInfo;    // ���Կ� �ڸ��� �������� ����

    /// <summary>
    /// ������ ���̸� ��ȯ�մϴ�.
    /// </summary>
    public int slotLen;

    /// <summary>
    /// �������� �ش� �ε����� ���Կ� �ڸ��ߴ��� ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool[] isItemPlaced;

    /// <summary>
    /// �ش� ������ �������� �������� ������ ��ȯ�մϴ�.
    /// </summary>
    public bool[] isItemEquipped;

    void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        base.Start();

        if( initializer.dicTypes.Length != 1 )
            throw new Exception("�ϳ��� ���� ��ųʸ��� ���ð����մϴ�.");

        if( initializer.dicTypes[0].itemType != ItemType.Weapon )
            throw new Exception("���� �������� ���� ������ �ƴմϴ�.");
        
        if( weaponTypes.Length != initializer.dicTypes.Length )
            throw new Exception("�ν����� â���� ���� ���⸦ ��� �������� �ʾҽ��ϴ�.");
        
        // ���� ���� ����
        slotLen = weaponTypes.Length;

        // ������ ���̸�ŭ �迭�� �����մϴ�.
        dummyTr = new Transform[slotLen];
        dummyImg = new Image[slotLen];
        isItemPlaced = new bool[slotLen];
        isItemEquipped = new bool[slotLen];
        slotItemInfo = new ItemInfo[slotLen];
        
        for( int i = 0; i<slotLen; i++ )
        {
            // ���̿�����Ʈ�� ����� �̹��� ������Ʈ�� ���� �� ���Կ� ��ġ�մϴ�.
            dummyTr[i] = new GameObject( "dummy"+ i.ToString() ).transform;
            dummyTr[i].SetParent( slotListTr.GetChild( i ) );
            dummyImg[i] = dummyTr[i].gameObject.AddComponent<Image>();

            // ���� ���� ���� �� ������ ���� ���� �ʱ�ȭ
            isItemPlaced[i] = false;
            isItemEquipped[i] = false;
        }
        

    }

    public void SlotActivate(int slotNo)
    {
        // �������� �ڸ����� �ʾҰų�, �������� �������̶�� �� �̻� �������� �ʽ��ϴ�.
        if( !isItemPlaced[slotNo] || isItemEquipped[slotNo] )
            return;

        // �ش� ������ ������ ������ �����մϴ�.
        slotItemInfo[slotNo] = slotListTr.GetChild(slotNo-1).GetComponent<ItemInfo>();

        // �������� ������ ������ �����մϴ�.
        Transform equipTr = GetEquipTr(slotItemInfo[slotNo]);

        // �������� �����մϴ�.
        slotItemInfo[slotNo].OnItemEquip(equipTr);

        // ���� ������Ʈ�� �ش� �������� �����ϴ�.
        dummyTr[slotNo].SetParent( slotListTr.GetChild(slotNo), false );

        // ���� �̹����� �����ݴϴ�.
        dummyImg[slotNo].sprite = slotItemInfo[slotNo].innerSprite;
        
        // ������ �������� ����
        isItemEquipped[slotNo] = !isItemEquipped[slotNo];
    }




    public void SlotDeactivate(int slotNo)
    {
        // �������� ���������� ���� ���¶�� �������� �ʽ��ϴ�.
        if( !isItemEquipped[slotNo] )
            return;

        // ���� ���Կ� �߰��մϴ�.
        AddItem(slotItemInfo[slotNo]);


        // ������ �������� ����
        isItemEquipped[slotNo] = false;
    }






    public Transform GetEquipTr(ItemInfo itemInfo)
    {
        if( ! (itemInfo.Item is ItemEquip) )
            throw new Exception("���� ������ ��� �ƴմϴ�.");


        ItemType itemType = itemInfo.Item.Type;

        if( itemType==ItemType.Weapon )
        {
            WeaponType weaponType = ( (ItemWeapon)itemInfo.Item ).WeaponType;

            switch(weaponType)
            {
                case WeaponType.Axe:
                    return equipTr.axeTr;

                case WeaponType.Blunt:
                    return equipTr.bluntTr;

                case WeaponType.Spear:
                    return equipTr.spearTr;

                case WeaponType.Sword:
                    return equipTr.swordTr;

                case WeaponType.Bow:
                    return equipTr.bowTr;                    
            }
        }

        throw new Exception("������ġ ������ ���ε��� �ʾҽ��ϴ�.");
    }






}


