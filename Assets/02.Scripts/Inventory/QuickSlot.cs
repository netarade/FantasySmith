using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
 * <v3.1 - 2024_0123_�ֿ���>
 * 1- ������ ���� �� ���� �� 3D������Ʈ �ֻ������ִ� �⺻ �ݶ��̴��� ��Ȱ��ȭ Ȱ��ȭ�ϵ��� ����
 * 
 * <v3.2 - 2024_0124_�ֿ���>
 * 1- Start���ο� playerInventory������ ��� �κ���  GetChild(0) ������������ GetComponentInChildren���� ����
 * ������ �κ��丮 ���ο� ũ������ UIâ�� �����ؾ� �ϴ� ��� ����â���� ������ �ռ��� �ϹǷ� 
 * ĵ���� ���� �κ��丮 ������Ʈ���� ���� ��ġ�ؾ��ϱ� ����
 * 
 * <v3.3 - 2024_0125_�ֿ���>
 * 1- Awake���� saveFileName�� �ٸ��� �������̵��ϴ� �κ� ����
 * �θ� �κ��丮 ��ũ��Ʈ���� �������� �̸��� �ڵ����� �����ǰ� �Ͽ����Ƿ� 
 * 
 * 2- Start������ �������ο� ownerId�� �÷��̾� �κ��丮�� Ʋ���� �˻��ؼ� ����ó���ϴ� �κ� �߰�
 * 
 * <v3.4 - 2024_0126_�ֿ���>
 * 1- SlotActivate �� Deactivate�� itemInfo�� ItemCol�� �� ��ũ��Ʈ���� �������ִ� �κ���
 * ItemInfo�� OnItemEquip, OnItemUnequip�޼���� �ű�
 * 
 * <v3.5 - 2024_0127_�ֿ���>
 * 1- SlotDeactivate�޼��忡�� AddItemToSlot�޼����� ȣ���� �ش� �������� OnItemUnequip���� ȣ���ϵ��� �ñ�
 * ������ �̹��� �������� ���� ������ �־ ȣ�� ������ �ѹ��� �����ϱ� ����.
 * 
 * <v3.6 - 2024_0129_�ֿ���>
 * 1- ���������� ���� �� ������� �̽��� �־ OnApplicationQuit �������̵��Ͽ� �������� �������� ����� ������ �����ϵ��� �Ͽ���.
 * 
 * <v4.0 - 2024_0130_�ֿ���>
 * 1- OnApplicationQuit �������̵� ����
 * ItemInfo���� WorldDrop�� �κ��丮�� ��ϵ� ���·� ������ �� �ִ� �ɼ��� ����� �ξ��� ������ ���� ������ ������ �� �ʿ䰡 ����.
 * 
 * 2- ������ ���� �̹��� ���� �Ӽ� �� ���� �ڵ带 ��� �����Ͽ���. (���� �����ڵ嵵 ����)
 * ������ �� ������ ���� Item2D ������Ʈ ������ ���� ������Ʈ�� �������� ������ �����Ͽ�����, ItemInfo���� �̸� ������ �� �ֱ� ����.
 * 
 * 3- SlotActivate �� SlotDeactivate �޼��� ����ȭ
 * �ش� ������ �������� OnItemEquip�� OnItemUnEquip�޼��带 ȣ�����ֱ⸸ �ϸ��.
 * 
 * <v4.1 - 2024_0130_�ֿ���>
 * 1- �����Կ� ������ �ִ� �������� �̹��� �迭 backgroundIcon ������ �߰��ϰ�,
 * ���� ������ŭ �ʱ�ȭ �� ���� �� ������ �̹��� OnOff����Ī�� �̷���� �� �ֵ��� ��. �ε�ÿ��� ����  
 * 
 * 
 * <v4.2 - 2024_0214_�ֿ���>
 * 1- SlotActivate �� SlotDeactivate�޼��� ���ο� OnItemEquip�� OnItemUnequip�޼��带 ȣ���ϴ� �ڵ带 EquipSwitch�޼��� ȣ��� ����
 * (�������� ���¸� �����ϰ� �ֱ� ������ Ư���� �����Ͽ� ȣ���� �ʿ䰡 ���� ������ �ϳ��� �޼���� ����)
 * 
 * <v4.3 - 2024_0214_�ֿ���>
 * 1- OnDummySelected�޼��� ����
 * ���� �������� �����õɶ��� ���� ������ ����Ʈ ��ũ��Ʈ���� ���������� ȣ�����ְ�, ���� ������ �������� ����ָ� �Ǳ� ����
 * 
 * 2- OnQuickSlotSelect�޼��� ���ο� isItemPlaced�� slotItemInfo�������� �ʱ�ȭ�ϴ� �κ� �߰�
 * 
 * <v4.4 - 2024_0216_�ֿ���>
 * 1- SlotDeactivate�޼��忡 �������ڷ� isForceEquip�ɼ��� �־ ������ ���������Ͽ� ���� ������ �� �� �ְ� �Ͽ���
 * (=> ���� �۵� ������ɿ��� ������ ������ ���� �����ϴ� ����� �߰��� �־�� �ϱ� ����) 
 * 
 * 2- OnQuickSlotSelect�޼��� ���ο� ���� ���� �������� �������ߴٸ�, ������ ������ ������ �� �ְ� 
 * SlotDeactivate���� SlotDeactivate(true) �޼��带 ȣ�� �ϴ��ڵ�� ����
 * 
 * 3- OnQuickSlotDrop�޼��� ���ο��� �������� ���� ���̶�� ������ ������ ������ �� �ְ� 
 * SlotDeactivate���� SlotDeactivate(true)�޼��带 ȣ�� �ϴ��ڵ�� ����
 * 
 * <v4.5 - 2024_0222_�ֿ���>
 * 1- InitOpenState ȣ�� ��ġ�� Start�� ���������� ����
 * 
 * 2- ������ �ε�� ��� �������� ������ �ǵ帮�� �ʰ� 2d��ɸ� ������ ���·� �����ϱ� ����
 * InitOpenState ȣ�� ���� AllItemOperateSwitchAs2d�޼��� ���౸���� �߰�
 * 
 * <v4.6 - 2024_0223_�ֿ���>
 * 1- OnQuickSlotDrop�޼��忡�� �������� ���Կ� �ڸ��� ��� �������� ������ �������� �������ִ� �ڵ带 �߰��Ͽ���.
 * ������ �������� ��� ItemSelect��ũ��Ʈ���� SlotDrop
 * 
 * 2- IsAbleToQuickSlotDrop�޼��带 �űԷ� �ۼ��Ͽ� OnQuickSlotDrop�� ���ǰ˻��ڵ带 ��� �ű��,
 * OnQuickSlotDrop���� ���� �ʱ�ȭ �ڵ常 ���δ� �������� ����
 * => ������ ������ ���ǰ˻�� �����ʱ�ȭ�� ���ÿ� ����Ǹ� ���ǰ˻簡 �������� �� ������ ����������ϱ� �����̸�,
 * �������� ��ӽ� ���и� ���� �ʾұ� ������ ���ÿ� �ص� �����������, ���� ���� �� �������ǵ� �߰��� �˻� �� 
 * ����� �������� ���ϱ� ������ ���� �и��ؾ� ��.
 * 
 * <v4.7 - 2024_0224_�ֿ���>
 * 1- GetSlotItemInfo �ű� �޼��� �ۼ�
 * ���Կ� �ڸ��� �������� ������ ��ȯ�� �� �ְ� �Ͽ���
 * ������ �ٸ� ��ũ��Ʈ���� Ư�� �������� 
 * 
 * 2- IsAbleToQuickSlotDrop ���� �޼��带 IsAbleToSlotDrop �������̵� �޼���� �����Ͽ� 
 * �θ��� ���� ��� ������ ���� �����ϰ�, �߰��� �������� ���� ������ǵ� �����ؼ� ��ȯ�� �� �ֵ��� ����
 * => ������ ���� ��ӽ� ���ǰ˻縦 �����ϴµ� �κ��丮 ��� ��ũ��Ʈ�� �߰��� ������ 
 * �ٸ� �κ��丮���� ���ο� ���� ���� �˻縦 ����ؼ� �߰��ؾ� �ϱ� ������ �̸� �����ϰ� �ϳ��� �����ϱ� ����.
 * 
 * 
 */






public class QuickSlot : InventoryInfo
{
    
    [Header("���Ժ� ���� ���� ����")]
    [SerializeField] WeaponType[] equipWeaponType;  // ���� ���� ����

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



    InventoryInfo playerInventory;  // �����԰� ����Ǿ� ������ �Ǵ� �÷��̾��� �κ��丮

    Image[] backgroundIcon;         // �����Կ� ������ �ִ� ��������



    protected override void Awake()
    {
        base.Awake();

        playerInventory = GetComponent<InventoryInfo>();

        if(ownerId != playerInventory.OwnerId)
            throw new Exception("���� ������ �κ��丮 ������ �ĺ���ȣ�� �����ؾ��մϴ�.");

        if(isServer)
            throw new Exception("�߽� �κ��丮�� �� �� �����ϴ�.");

    }

    protected override void Start()
    {
        base.Start();



        if( initializer.dicTypes.Length!=1 )
            throw new Exception( "�ϳ��� ���� ��ųʸ��� ���ð����մϴ�." );

        if( initializer.dicTypes[0].itemType!=ItemType.Weapon )
            throw new Exception( "���� �������� ���� ������ �ƴմϴ�." );



        // ���� ���� ���� (��ųʸ��� �������� ��)
        slotLen=initializer.dicTypes[0].slotLimit;

        // �迭 ��� �� �ʱ�ȭ        
        isItemPlaced=new bool[slotLen];
        slotItemInfo=new ItemInfo[slotLen];
        backgroundIcon = new Image[slotLen];


        for(int i=0; i<slotLen; i++)
            backgroundIcon[i] = inventoryTr.parent.GetChild(3).GetChild(0).GetChild(i).GetComponent<Image>();
        

        // ������ ���� ���� �ʱ�ȭ
        isItemEquipped=false;

        // �׷��� ����ĳ������ �����ϵ��� �÷��̾� �κ��丮�� �������·� �����մϴ�.
        playerInventory=inventoryTr.parent.GetComponentInChildren<InventoryInfo>();
        this.RegisterInventoryLink( playerInventory );
                

        // �������� �ε�� ���Կ� �������� �����ϴ��� Ȯ��
        for( int i = 0; i<slotLen; i++ )
        {
            if( slotListTr.GetChild( i ).childCount>0 )
            {
                isItemPlaced[i]=true;
                backgroundIcon[i].enabled = false; // �������� �̹����� ��Ȱ��ȭ�մϴ�.
                slotItemInfo[i] = slotListTr.GetChild( i ).GetChild( 0 ).GetComponent<ItemInfo>();

                if( slotItemInfo[i]==null )
                {
                    slotItemInfo[i]=slotListTr.GetChild( i ).GetChild( 0 ).GetComponent<DummyInfo>().EquipItemInfo;

                    // ���� ������ �ִ� ��� 
                    if( slotItemInfo[i]!=null )
                    {
                        isItemEquipped = true;      // �������� ���� ���·� ����
                        equipSlotIndex = i;         // ���� ������ ���� �ε����� �ش� �ε����� ����
                    }
                }            



            }
        }



        // â�� �׻� ����Ӵϴ�.
        InitOpenState( true );

        // �������� 2d ����� ������ ���·� �����մϴ�.
        AllItemOperateSwitchAs2d(false, false);
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

        // ������ ���� ������ �����Ͽ� �ش� ������ �������� �����մϴ�.
        slotItemInfo[slotIndex].EquipSwitch();
        

        // ���� ���Գѹ��� �����մϴ�.
        equipSlotIndex = slotIndex;
                
        // ������ �������� Ȱ��ȭ
        isItemEquipped = true;
                
        return true;
    }



    /// <summary>
    /// ������ �۵������� �� ȣ��������� �޼����Դϴ�.<br/>
    /// ���Կ� �������� �ִ� ���� �̹����� �󸮽�Ʈ�� ���� ��,
    /// ����Ǿ��ִ� ��� �����Ͽ� ���Կ� �����ִ� ������ �մϴ�.<br/><br/>
    /// �������ڷ� ���� ���� �ɼ��� Ȱ��ȭ ��Ű�� �������� ������ ���� �����Ͽ� ���� ���¸� �����մϴ�. (�⺻ ��: false)
    /// </summary>
    /// <returns>������ ���� ���� ���� �� true, ���� �� false�� ��ȯ</returns>
    private bool SlotDeactivate(bool isForceEquip=false)
    {
        // �������� ���������� ���� ���¶�� �������� �ʽ��ϴ�.
        if( !isItemEquipped )
            return false;
                
        // ���� �������ο� ���� �������� ������ �����մϴ�.
        slotItemInfo[equipSlotIndex].EquipSwitch(isForceEquip,false);

        isItemEquipped = false;                     // ������ ���� ���¸� �����մϴ�.
        equipSlotIndex = -1;                        // ���� ���� �ε����� �ʱ�ȭ�մϴ�.

        return true;
    }





    
    /// <summary>
    /// ���ڷ� ���� �� �������� ���� ���������� ��� ������ �� ���θ� ��ȯ�մϴ�.<br/>
    /// �θ� �κ��丮�� ���� ��� ���ǿ� �߰��ؼ� ��� ��ũ��Ʈ�� �������� ��������� �˻��ϰ� ����� ��ȯ�մϴ�.<br/>
    /// ����� ������ ������ ����� ������ Transform ���� ���� �����ؾ� �մϴ�.<br/>
    /// </summary>
    /// <returns>�����Կ� ����� �����ϸ� true��, �Ұ����ϸ� false�� ��ȯ</returns>
    public override bool IsAbleToSlotDrop( ItemInfo dropItemInfo, Transform dropSlotTr )
    {
        // �θ� ������ �����ϰ� ��ȯ�޽��ϴ�.
        bool bBaseCondition = base.IsAbleToSlotDrop(dropItemInfo, dropSlotTr);

        // �ڽ� ������ �����մϴ�. (�⺻��: ���)
        bool bQuickSlotCondition = true;

        // ��� �� ������ �ε����� �о���Դϴ�.
        int slotIndex = dropSlotTr.GetSiblingIndex();

        // ��� �� ���Կ� �������� �̹� �ڸ��� ���¶��, �ش� �������� ����Ʈ������ �Ͻ������� �����մϴ�
        if( isItemPlaced[slotIndex] )
            slotItemInfo[slotIndex].ItemSelect.SelectPreventTemporary();

        // ������ �ε����� �������� �迭�� ������ �ε����� �ʰ��Ѵٸ� ������������ ó���մϴ�.
        if(slotIndex >= equipWeaponType.Length)
            bQuickSlotCondition = false;

        // ����� �������� ����Ÿ���� ��ġ���� ������ ������������ ó���մϴ�.
        if(dropItemInfo.WeaponType !=equipWeaponType[slotIndex] )
            bQuickSlotCondition = false;

        // �θ�� �ڽ� ���� ��θ� �����ϴ� �� ���θ� ��ȯ�մϴ�. 
        return (bBaseCondition && bQuickSlotCondition);
    }



    /// <summary>
    /// ���� ����̺�Ʈ�� �Ͼ�� ���� �����Կ� ����� �����ۿ� ���� ������ �ֽ�ȭ���ֱ� ���� �޼����Դϴ�.<br/>
    /// ����� ������ ������ ����� ������ Transform ���� ���� �����ؾ� �մϴ�.<br/>
    /// *** ���� �˻縦 ���� �������� �ʽ��ϴ�. IsAbleToQuickSlotDrop�� ���� �˻������� ����� ���¿��� ����ؾ� �մϴ�. ***
    /// </summary>
    public void OnQuickSlotDrop( ItemInfo dropItemInfo, Transform dropSlotTr )
    {
        // ��� �� ������ �ε����� �о���Դϴ�.
        int slotIndex = dropSlotTr.GetSiblingIndex();

        // �������� ���� ���� �����̶�� ���� �������� ������ �����մϴ�.
        if(equipSlotIndex==slotIndex)
            SlotDeactivate(true);

        backgroundIcon[slotIndex].enabled = false;   // �������� �̹����� ��Ȱ��ȭ �մϴ�.
        isItemPlaced[slotIndex] = true;              // ������ �ڸ� ���¸� Ȱ��ȭ�մϴ�.
        slotItemInfo[slotIndex] = dropItemInfo;      // ���� ������ ������ �����մϴ�.
    }









    /// <summary>
    /// �� ���Կ��� �������� �������� �Ͼ �� ȣ��������ϴ� �޼����Դϴ�.<br/>
    /// ��� �������� �ٽ� ���� �������ִ� ����� ������ �ֽ��ϴ�.<br/>
    /// ������ ������ �����ؾ� �մϴ�.
    /// </summary>
    public void OnQuickSlotSelect(ItemInfo itemInfo)
    {   
        int slotIdxTab = itemInfo.SlotIndexActiveTab;     // ������ ������ �����Ͽ� ���� �ε����� Ȯ���մϴ�.

        if( equipSlotIndex==slotIdxTab )            // ���� ���� �������� �������ߴٸ�, ������ ������ �����մϴ�.
            SlotDeactivate(true);

        backgroundIcon[slotIdxTab].enabled = true;  // �������� �̹����� Ȱ��ȭ �մϴ�.
        isItemPlaced[slotIdxTab] = false;           // ������ �ڸ� ���¸� �����մϴ�.
        slotItemInfo[slotIdxTab] = null;            // ���� ������ ������ �ʱ�ȭ�մϴ�.
    }


    /// <summary>
    /// �����Կ� �ڸ��ϰ� �ִ� ���� �ε����� �ش��ϴ� ������ ������ ��ȯ�մϴ�.<br/>
    /// ���� ���� ������ ���ο� ������� ItemInfo ���� ���� �״�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>�ش� ���Կ� �������� �����Ѵٸ� ������ ������ ��ȯ, �������� �ʴ´ٸ� null ��ȯ</returns>
    public override ItemInfo GetSlotItemInfo(Transform slotTr)
    {
        // ������ �ڽ� �ε����� ���� �ε����� �����մϴ�.
        int slotIndex = slotTr.GetSiblingIndex();

        // ������ �ε����� �������� �迭�� ������ �ε����� �ʰ��Ѵٸ� ���������� ��ȯ�մϴ�.
        if(slotIndex >= equipWeaponType.Length)
            return null;
        
        // ���Կ� �������� �ڸ����� �ʾҴٸ� ���������� ��ȯ�մϴ�.
        if( !isItemPlaced[slotIndex] )
            return null;

        // �ش� ���� �ڸ��� ������ ������ ��ȯ�մϴ�.
        return slotItemInfo[slotIndex];
    }





}


