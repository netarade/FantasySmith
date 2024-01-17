using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
 * 
 * <v2.0 - 2024_0117_�ֿ���>
 * 1- EquipmentTransformŬ���� ��� ��Ʈ����Ʈ ����
 * 2- ������ weaponTypes���� equipWeaponType�� ����
 * 3- Start������ ���̸� �� ���Կ� ���Ƶδ� ���� emptyList�� ���Ƶδ� ������ ����
 * ������ �������� ������ �� ���̰� ���Կ� �ڸ��ؾ��ϱ� ����
 * 
 * <v2.1 - 2024_0117_�ֿ���>
 * 1- ���̸� ��� �����ϴ� ��Ŀ��� �ϳ��� �����س��� �������� �����ϴ� ������� ����(��ũ��Ʈ �ٿ��� ���̸� ����� ����)
 * 
 * 2- isItemEquipped�迭�� �����ϰ� ���� �ϳ��� ����
 * (������ �� ���������� ���� 6���� ������ �߿��� �� 1���� ���������ϵ��� �Ұ��̹Ƿ�)
 * 
 * 3- � ������ �������� �����Ǿ������� ��Ÿ���� ���� equpSlotIndex�߰�
 * 
 * 4- �޼���� OnSlotDropped�� OnQuickSlotDrop���� ����, �Ű������� �����ε������� ���� Ʈ���������� ����
 * 
 * 5- SlotActivate�� isEquip �� equipSlotIndex�� ���� �˻� ���� �߰�
 * �Ű����� slotNo�� ���ο��� slotIndex�� ��ȯ�ϴ� �ڵ� �߰�
 * 
 * 6- Start������ ���� �� �ε尡 �̷������ �� ���� isItemPlace[]�� slotItemInfo[]�� �ʱ�ȭ �ϵ��� ����
 * 
 * 7- SlotActivate���� slotItemInfo�� ���� ���ϴ� �� ���� (������ ��� �� �ε�� �ڵ����� �����ϹǷ� ���Ӱ� �о���� �ʿ䰡 ����)
 * 
 * 8- equipWeaponType.Length != initializer.dicTypes.Length �˻繮 �ּ�ó��
 * (����Ÿ���� ����Ÿ���̱⿡ �ϳ��� ��ųʸ��� ���� �������Ⱑ ���� ����)
 * 
 * (��������)
 * ������ slotIndex�� -1�� �����, �ε� �� slotIndex�� -1�� �� �ٽ� �����ϰ� �����
 * (������ �������� �ϳ��ۿ� �����Ƿ� ���������� ���Ŀ� ���� ������ ������ �ش� ������ ���� ���������� ���� �����ؾ���)
 * �ε� �� isEquipped ������ true�� ������ֱ�
 * 
 * <v3.0 - 2024_0118_�ֿ���>
 * 1- slotLen�� equipWeaponType�� ���̿��� initializer.dicTypes[0]�� ���̷� ����
 * ������ ������ ��Ȯ�� ���缭�ؾ� IsItemPlaced[]�� �� ���¸� ���Ժ��� ������ �� ����
 * 
 * 2- dummyTr�� dummyRectTr�� ����, cell����� �����ϱ� ����. 
 * (�̹��� ����� ������ ������� �°� ��ҽ��Ѿ� �ϹǷ�,)
 * 
 */


[Serializable]
public class EquipmentTransform
{
    [Header("2-����")]
    public Transform axeTr;

    [Header("3-���")]
    public Transform pickaxTr;
    
    [Header("4-â")]
    public Transform spearTr;
        
    [Header("5-Ȱ")]
    public Transform bowTr;        
}




public class QuickSlot : InventoryInfo
{
    
    [Header("���Ժ� ���� ���� ����")]
    [SerializeField] WeaponType[] equipWeaponType;  // ���� ���� ����

    
    [Header("�������� ������ ��ġ����")]
    [SerializeField] EquipmentTransform equipTr;    // ��� ������ Ʈ������ ����


    RectTransform[] dummyRectTr;    // ���̿� ������Ʈ�� RectTransform
    Image[] dummyImg;               // ���̿� �̹���
    ItemInfo[] slotItemInfo;        // ���Կ� �ڸ��� �������� ����

    /// <summary>
    /// ������ ���̸� ��ȯ�մϴ�.
    /// </summary>
    public int slotLen;

    /// <summary>
    /// �������� �ش� �ε����� ���Կ� �ڸ��ߴ��� ���θ� ��ȯ�մϴ�.
    /// </summary>
    bool[] isItemPlaced;

    bool isItemEquipped;
    int equipSlotIndex;


    /// <summary>
    /// �ش� ������ �������� �������� ������ ��ȯ�մϴ�.
    /// </summary>
    public bool IsItemEquipped { get { return isItemEquipped; } }



    InventoryInfo playerInventory;  // �÷��̾� �κ��丮




    protected override void Awake()
    {
        base.Awake();

        saveFileName = inventoryTr.parent.parent.name + "_QuickSlot";
    }

    protected override void Start()
    {
        base.Start();

        if( initializer.dicTypes.Length != 1 )
            throw new Exception("�ϳ��� ���� ��ųʸ��� ���ð����մϴ�.");

        if( initializer.dicTypes[0].itemType != ItemType.Weapon )
            throw new Exception("���� �������� ���� ������ �ƴմϴ�.");
        
        //if( equipWeaponType.Length != initializer.dicTypes.Length )
        //    throw new Exception("�ν����� â���� ���� ���⸦ ��� �������� �ʾҽ��ϴ�.");
        
        
        // ���� ���� ���� (��ųʸ��� �������� ��)
        slotLen = initializer.dicTypes[0].slotLimit;

        // ������ ���̸�ŭ �迭�� �����մϴ�.
        dummyRectTr = new RectTransform[slotLen];
        dummyImg = new Image[slotLen];
        isItemPlaced = new bool[slotLen];
        slotItemInfo = new ItemInfo[slotLen];


        // �̸� ������� ���� �ϳ��� Transform�� �����մϴ�.
        dummyRectTr[0] = emptyListTr.GetChild(0).GetComponent<RectTransform>();

        for( int i = 0; i<slotLen; i++ )
        {
            // ���̿�����Ʈ�� �����Ͽ� emptyList�� ��ġ�ϰ�, �������� �����մϴ�.
            dummyRectTr[i] = Instantiate( dummyRectTr[0].gameObject, emptyListTr ).GetComponent<RectTransform>();            
            dummyImg[i] = dummyRectTr[i].GetComponent<Image>();            
        }       
        
        // ������ ���� ���� �ʱ�ȭ
        isItemEquipped = false;

        // �׷��� ����ĳ������ �����ϵ��� �÷��̾� �κ��丮�� �������·� �����մϴ�.
        playerInventory = inventoryTr.parent.GetChild(0).GetComponent<InventoryInfo>();
        this.RegisterInventoryLink(playerInventory);

        // â�� �׻� ����Ӵϴ�.
        InitOpenState(true);


        // �������� �ε�� ���Կ� �������� �����ϴ��� Ȯ��
        for( int i = 0; i<slotLen; i++ )
        {
            if( slotListTr.GetChild( i ).childCount>0 )
            {
                isItemPlaced[i]=true;
                slotItemInfo[i]=slotListTr.GetChild( i ).GetChild( 0 ).GetComponent<ItemInfo>();
            }
        }
    }






    /// <summary>
    /// ���Կ� ��� ��� �����ϰ� �����մϴ�.<br/>
    /// ������ ����� - �������� ���Կ� �ڸ��ϰ� ���� �ʰų�, �̹� �����ϰ� �ְų�,<br/>
    /// ������ ���������� - ������ ���°� �ƴϰų� ���� ������ ���а� ��ȯ�� �� �ֽ��ϴ�.<br/><br/>
    /// ���� 1~5�������� ������ ���� �Ǿ� ������, 1�� ��������, �������� ���뵿���� �ϰԵ˴ϴ�.<br/>
    /// *** ���εǾ��ִ� ���̿��� �Է��� ������ ���ܸ� �����ϴ�. ***
    /// </summary>
    /// <returns>������ ���� �� ���� ���� ���� �� true��, ���� �� false�� ��ȯ</returns>
    public bool ItemEquipSwitch(int keyPadNum)
    {
        if(keyPadNum<=0 || keyPadNum>=6 )
            throw new Exception("���� �Է°� ������ �Ǿ����� �ʽ��ϴ�.");

        if(keyPadNum==1)
            return SlotDeactivate();
        else
            return SlotActivate(keyPadNum-1);
    }





    /// <summary>
    /// ������ �۵��� �� ȣ��������� �޼����Դϴ�.<br/>
    /// ��� ��������ܰ� ���ÿ� ���� �̹����� ���Կ� �����༭ 
    /// ���� ���Կ� �������� �״�� �ִ� �� ó�� ���̰� �մϴ�.<br/>
    /// </summary>
    /// <returns>������ ���� ���� �� true, ���� �� false�� ��ȯ</returns>
    private bool SlotActivate(int slotIndex)
    {
        // �������� �ڸ����� �ʾҰų�, ������ �������� �������̶�� �� �̻� �������� �ʽ��ϴ�.
        if( !isItemPlaced[slotIndex] )
            return false;
        
        // �������� �������� ���¶��,
        if( isItemEquipped )
        {
            // ������ �������� �������̶�� �۵����� �ʽ��ϴ�
            if(equipSlotIndex == slotIndex)
                return false;
            
            // �ٸ� �������� �������̶�� ������������ �����մϴ�.
            SlotDeactivate();
        }

        // �������� ������ ���� ������ �޾ƿɴϴ�.
        Transform equipTr = GetEquipTr(slotItemInfo[slotIndex]);

        // �������� �����մϴ�.
        slotItemInfo[slotIndex].OnItemEquip(equipTr);

        // ���� ���Գѹ��� �����մϴ�.
        equipSlotIndex = slotIndex;


        
        // ���� �̹����� �����մϴ�.
        dummyImg[slotIndex].sprite = slotItemInfo[slotIndex].innerSprite;

        // ���� ������Ʈ�� �ش� �������� �����ϴ�.
        dummyRectTr[slotIndex].SetParent( slotListTr.GetChild(slotIndex), false );

        // ���̿�����Ʈ�� ũ�⸦ ���Ը���Ʈ�� cellũ��� �����ϰ� ����ϴ�.(������ ũ��� �����ϰ� ����ϴ�.)
        dummyRectTr[slotIndex].sizeDelta = slotListTr.GetComponent<GridLayoutGroup>().cellSize;

        
        // ������ �������� Ȱ��ȭ
        isItemEquipped = true;
                
        return true;
    }



    /// <summary>
    /// ������ �۵������� �� ȣ��������� �޼����Դϴ�.<br/>
    /// ���Կ� �������� �ִ� ���� �̹����� �󸮽�Ʈ�� ���� ��,
    /// ����Ǿ��ִ� ��� �����Ͽ� ���Կ� �����ִ� ������ �մϴ�.<br/>
    /// </summary>
    /// <returns>������ ���� ���� ���� �� true, ���� �� false�� ��ȯ</returns>
    private bool SlotDeactivate()
    {
        // �������� ���������� ���� ���¶�� �������� �ʽ��ϴ�.
        if( !isItemEquipped )
            return false;
        
        // �ڸ��� ���̸� �ٽ� �󸮽�Ʈ�� �����ϴ�
        dummyRectTr[equipSlotIndex].SetParent(emptyListTr, false);

        // ������ �������� ���� ���Կ� �ٽ� �߰��մϴ�.
        AddItemToSlot( slotItemInfo[equipSlotIndex], equipSlotIndex, interactive.IsActiveTabAll );
                
        // ������ �������� ����
        isItemEquipped = false;

        return true;
    }






    public Transform GetEquipTr(ItemInfo itemInfo)
    { 
        if(itemInfo == null)
            throw new Exception("������ ������ ���޵��� �ʾҽ��ϴ�.");

        ItemEquip itemEquip = itemInfo.Item as ItemEquip;
        if( itemEquip==null )
            throw new Exception("���� ������ ��� �ƴմϴ�.");


        ItemType itemType = itemInfo.Item.Type;

        if( itemType==ItemType.Weapon )
        {
            WeaponType weaponType = ( (ItemWeapon)itemInfo.Item ).WeaponType;
            
            switch(weaponType)
            {
                case WeaponType.Axe:
                    return equipTr.axeTr;

                case WeaponType.Pickax:
                    return equipTr.pickaxTr;

                case WeaponType.Spear:
                    return equipTr.spearTr;

                case WeaponType.Bow:
                    return equipTr.bowTr;                    
            }
        }
        throw new Exception("������ġ ������ ���ε��� �ʾҽ��ϴ�.");
    }




    /// <summary>
    /// ���� ����̺�Ʈ�� �Ͼ �� ����� �߻���Ű�� �ʿ��� �߰������� ȣ������� �� �޼����Դϴ�.<br/>
    /// ����� �Ͼ�� �����۰� ������ ������ �����ؾ� �մϴ�.
    /// </summary>
    public void OnQuickSlotDrop( ItemInfo droppedItemInfo, Transform slotTr )
    {
        int slotIndex = slotTr.GetSiblingIndex();

        // ���� �ڸ����� Ȱ��ȭ
        isItemPlaced[slotIndex] = true;

        // ���� ������ ���� Ȱ��ȭ
        slotItemInfo[slotIndex] = droppedItemInfo;
    }

    /// <summary>
    /// ���� �������� ����Ʈ �� �� ���� �ʿ��� ȣ������� �� �޼����Դϴ�.<br/>
    /// ����� �Ͼ�� ���������� ��������� �մϴ�.
    /// </summary>
    public void OnDummySelected(Transform slotTr)
    {       
        int slotIndex = slotTr.GetSiblingIndex();

        // �������� ���������� �ʰų�, ����������No�� ��ġ���� ������ �����մϴ�.
        if( !isItemEquipped || equipSlotIndex!=slotIndex )
            throw new Exception("���̸� ������ �� ���� �����Դϴ�.");

        // ������ ������ �����մϴ�.
        SlotDeactivate();

        // ���Ծ����� ���� ����
        slotItemInfo[slotIndex] = null;
    }




}


