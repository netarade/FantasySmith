using CreateManagement;
using DataManagement;
using ItemData;
using System;
using UnityEngine;
using UnityEngine.UIElements;

/*
* [�۾� ����]
* <v1.0 - 2024_0112_�ֿ���>
* 1- ������ ���� ó���� ���� ���� Ŭ���� �ۼ� 
* 
* <v1.1 - 2024_0114_�ֿ���>
* 1- OnItemEquip �޼��� �ۼ�
* OnItemWorldDrop�� setParent���·� �̷������ �Ǹ�, 
* �ٽ� �ڱ��������� ��ȯ�Ͽ� �޼��带 ����� ���� ���ٿ� �����ϱ� ������ ����
* 
* <v1.2 -2024_0116_�ֿ���>
* 1- PrintDebugInfo�޼��� �ۼ�
* 
* <v2.0 - 2024_0118_�ֿ���>
* 1- CheckToDestroy�޼��带 �ۼ�
* InfoŬ�������� �ܺ� ����ڰ� ������ ������ ������Ű�� ���� �����̹Ƿ�,
* ������ ���������� �ٲ� ������ �ı����θ� üũ�ؼ� �ڵ����� �ı����ѹ������� �ϱ�����.
* 
* <v2.1 - 2024_0118_�ֿ���>
* 1- CheckDestroyInfo�޼��忡�� 2D ������ ���� 2D ������Ʈ�� �ı��ؾ��ϰ�, 3D �����϶��� 3D������Ʈ�� �ı��ϵ��� ����
* 
* <v2.2 - 2024_0125_�ֿ���>
* 1- RegisterToWorld �ۼ�
* �κ��丮 �������� Transform ������ �޾Ƽ� ������id�� ���� �� ���� �κ��丮�� �����ϴ� ����
* 
* <v2.3 - 2024_0125_�ֿ���>
* 1- RegisterToWorld�޼��� ���ڸ� InventoryInfo�� ������� �����ε�, ���� �޼���� �����ε� �޼��带 ��Ȱ��, �ּ� ����
* 
* <v3.0 - 2024_0126_�ֿ���>
* 1- �÷��̾ UserInfo ��ũ��Ʈ���� Id�� �Ҵ�ް� �����Կ� ����, UserInfo�� �Ű������� ���� �ؾ� �ϹǷ�,
* RegisterToWorld �����ε� �޼���(InventoryInfo �� Transform)�� �����ϰ� UserInfo�� ���޹���.
* 
* 2- RegisterToWorld�޼������ RegisterWorldInvetory�� ����
* 
* 3- SetPrivateId�޼��� �����Ͽ� �������� ���� �ĺ� ��ȣ(Id)�� �ܺηκ��� �ο����� �� �ְ� �Ͽ���.
* 
* 4- isRegistered �Ӽ��� �����Ͽ� �����κ��丮�� ��ϵǾ����� ���θ� ����ϰ�, �ش� ������ ���� SetPrivateId�� �߰�ȣ�� ���ɼ� ���θ� �Ǵ�.
* 
* <v3.1 - 2024_0126_�ֿ���>
* 1- OnItemEquip�޼��带 �⺻������ ���ڷ� ���޹��� Transform�� ���� �������� ���ϰ� ���ְ� 
* ��ġ������ �޾ư��� �ʵ��� �Ͽ���. �����ۿ� ���������� ����Ǿ��ִ� STransform�� �����Ͽ� ��ȯ������ ����.
* 
* + �ּ� ����
* 
* 2- OnItemEquip �޼��尡 �������� ���� ������ ��ȯ ������ ����ϵ��� STransform equipPrevTr ���� �߰�
* 
* 3- OnItemUnEquip�޼��� �ۼ� - �ٽ� ������ ��ȯ������ ���ư��� �ϴ� ���
* 
* 4- �������� ������ �� ���� 2d������Ʈ�� ������ ���� �θ� 3d������ ���� ���� ���ϹǷ� �ٽ� ������� �����ִ� ��� �߰�
* 
* <v3.2 - 2024_0129_�ֿ���>
* 1- ������ ������ ������ٵ��� �߷��� ����, ���� ���� �� �߷��� �ٽ� Ű���� ���� (���Ⱑ �������Ƿ�)
* 
* <v3.3 - 2024_0130_�ֿ���>
* 1- ������ equipPrevTr�� prevEquipLocalTr�� ����
* 
* 2- OnItemEquip OnItemUnequip�޼��� ���� ����
* a.�Ű������� equipTr�� equipParentTr�� ����
* b.���� ������ ������Ʈ �ϴ� �ڵ� �߰�
* c.��ȯ ������ void���� bool�� �����Ͽ�, ����ó���� ���з� �ٲٰ�, ���п��� ȣ��� �� �ֵ��� ����
* d.���� �� �����ۿ� ����� �����׼� �븮�ڸ� ȣ���ϵ��� �Ͽ���.
* e.�ݶ��̴��� ������ٵ� null �˻繮 �߰�
* 
* 
* <v4.0 - 2024_0214_�ֿ���>
* 1- �޼���� OnItemEquip�� OnItemUnequip�� EquipTransfer�� UnEquipTransfer�� �ٲ� �� 
* ���������� public���� private�� ����, �׸��� EquipSwitch �ű� �޼��忡 ���Խ�Ŵ.
* 
* ������ ���� ����� 3D ������Ʈ�� ���۽��Ѽ� �����ϴ� ������ ������ 3D������Ʈ�� �÷��̾ �̸� �־�ΰ� Ȱ��ȭ�� �����ִ� ��� 2������ �����ϱ� ����
* 
* 2- EquipTransfer�� UnequipTransfer�޼��忡�� EquipAction �븮�� ���� �ڵ�� isLoad���� �ڵ带 �����Ͽ���
* 
* ������ ������ ���� �� �÷��̾� ���� �߰� �׼��� �������ְų�, �ε� �� ���¸� �������ֱ� ���� �뵵�� ����ϰ� �־��µ�,
* 
* ���� �޼��带 2������ �����ϸ鼭 
* ���� ���������� �븮�� �׼��� ���� ����� �ʿ���� �÷��̾��� equipInfo�� �޼��带 ȣ���ؾ��ϰ�,
* ���� ���������� �ε� �ÿ��� �븮�� �׼��� ȣ�����ָ� �Ǵµ� �̸� ���� ���� �߿� ��� ��� �ְų�, ���� ���� �޼��忡�� �����ϰ� �ڵ�� �б��ϱ⺸��
* 
* ���� ���� �޼��忡���� �������� ���븸 ������ְ� ���� ���� �޼��忡�� �б��ϸ�,
* �÷��̾� ���´� �ε��� ���� ���������� �ٸ� ��ũ��Ʈ�� ���� �޼��带 ȣ�����ְų�, �κ��丮���� �ε��ϸ鼭 ���� ȣ�����ִ� ���� ȿ�����̱� ����
* 
* 3- EquipTransfer, UnequipTransfer�޼��带 ȣ�� �� IsEquip ������ ������ ���������ִ� �ڵ带 �����ϰ�, 
* EquipTransferSwitch�޼��带 �ۼ��Ͽ� �ش� �޼��带 �׻� ȣ���� �� �ֵ��� �Ͽ�, ���⼭ IsEquip ���¸� �����ϵ����Ͽ���.
* ������ isLoad�ɼ��� �־ �ε� �� ���� ��ȭ�� ���� �ʰ� ���븸 �����ֵ��� �ϱ� ����. 
* (�������� ���� �޼��常 ȣ���ص� ���°� �����Ǿ������ ������ ȣ�� �� ���¸� �������� ������ �����Ͽ� ȣ���� �ʿ䰡 ����)
* 
* 4- EquipTransfer, UnequipTransfer�� void������ �����ϰ� ���� ���п��� ��ȯ �ڵ� ����
* ������ EquipSwitch�޼��忡�� ���� ������ ���¸� ���� �����ϸ�, 
* �ε� �ÿ��� ���¹����� �õ����� �ʱ� ������, ������ ���¿��� ȣ������ �� ���� ���¿��� �˻� ���ǿ� �ɷ��� �����ϸ� �ȵǱ� ����
* 
* 5- EquipSwitch ���� �޼��� ȣ���� ���� ����Ÿ���� �б��ؼ� ���� ���� �޼���� ���� ���� �޼��带 �б��ؼ� ȣ�����ְ�,
* ���� ������¸� �����ϸ�, �ε� �ɼ��� �������ڷ� �־ ������¸� �ݿ����� �ʰ� ������ �����ų �� �ְԲ� ����
* 
* <v4.1 - 2024_0214_�ֿ���>
* 1- EquipTransfer, UnequipTransfer�޼��� ���ο� dummyInfo ���� �޼��带 ���������ϴ� �Ϳ��� OnItemEquipSwitch ���� �޼��� ȣ��� ��ü
* 
* <v4.2 - 2024_0216_�ֿ���>
* 1- EquipSwitch�޼����� ���� isLoad �������ڸ��� isRemainEquipState�� �����ϰ�,
* isForceUserState �������ڸ� �߰��Ͽ� ���� ���� ���� �ݿ� �ɼǰ� ���� ���� ���� �ɼ��� �����Ͽ� ȣ���� �� �ְ� ���־���
* 
* ������ �ε� �� ���� ���� �� ���� �ϴ� ���� ���� ���� ��(������ ������ ��) ���� ���� �� �����ϴ� ��찡 ���еǱ� ����
* 
* 
*/


public partial class ItemInfo : MonoBehaviour
{    
    
    /// <summary>
    /// �������� ���� �������� ���� �ı����θ� üũ�մϴ�.<br/>
    /// ������ ���ǿ� ���� �ı�, ��ø������ �����ı� 
    /// </summary>
    public void CheckDestroyInfo()
    {
        bool isRemove = false;
        ItemWeapon itemWeapon = item as ItemWeapon;
        ItemBuilding itemBuilding = item as ItemBuilding;
        ItemMisc itemMisc = item as ItemMisc;
        
        // ����� �Ǽ����������� ��� �������� üũ�մϴ�.
        if( itemWeapon!=null )
        {
            if( itemWeapon.Durability<=0 )
                isRemove=true;
        }
        else if( itemBuilding!=null )
        {
            if( itemBuilding.Durability<=0 )
                isRemove=true;
        }
        
        // ��ȭ�������� ��� ��ø������ üũ�մϴ�.
        if( itemMisc!=null )
        {
            if( itemMisc.OverlapCount<=0 )
                isRemove = true;
        }

        // �ı����ΰ� Ȱ��ȭ�� ���
        if( isRemove )
        {
            // �κ��丮�� �ִٸ� ��Ͽ��� ����
            if( inventoryInfo!=null )
                inventoryInfo.RemoveItem( this );   
            
            /*** ���� ���¿� ���� ������ �ı� ***/
            if(isWorldPositioned)
                Destroy( transform.parent.gameObject );     // �θ� 3D ������ ������Ʈ�� �ı��մϴ�.  
            else
                Destroy( this.gameObject );                 // 2D ������ ������Ʈ�� �ı��մϴ�.
        }
    }




    /// <summary>
    /// ItemStatus����ü�� ���޹޾� �ش� ��ġ��ŭ ������ ��ȭ�� �ִ� �޼����� �븮��
    /// </summary>
    public delegate void StatusChangeAction(ItemStatus status);

    /// <summary>
    /// �������� ������ ItemFood�� ��쿡 ȣ�Ⱑ���� �޼����Դϴ�.<br/>
    /// �������� �����Ͽ� ĳ������ �������ͽ��� ������ �ݴϴ�.<br/><br/>
    /// </summary>
    /// <returns>������ ���뿡 �����ϸ� true��, �����ϸ� false�� ��ȯ</returns>
    public bool OnItemEat(StatusChangeAction StatusChange)
    {        
        ItemFood itemFood = item as ItemFood;

        if( itemFood==null )
            throw new Exception("�������� ������ ������ �ƴմϴ�.");
        
        if(StatusChange == null)
            throw new Exception("�������ͽ��� �����Ű�� �޼��尡 ���޵��� �ʾҽ��ϴ�.");

        // �������ҿ� �����ϸ� �������ͽ� ��ȭ �޼��带 ȣ���ϰ� true�� ��ȯ
        if( inventoryInfo.ReduceItem( this, 1 ) )
        {
            StatusChange( StatusInfo );
            return true;
        }
        // �������ҿ� �����ϸ� false�� ��ȯ
        else
            return false;
    }


    /// <summary>
    /// �������� ���� ��ȯ�ϴ� �޼����Դϴ�.<br/>
    /// ���� ������ ���� �ٸ� ������� ������ �����մϴ�.<br/>
    /// �����ۿ� ���� �÷��̾� ������ ������ �������� �����ϴ� ��İ� 3D ������Ʈ�� �����Ͽ� �����ϴ� ������� �����մϴ�.<br/><br/>
    /// ���� ���ڷ� ���� ���� ���� �ݿ� �ɼǰ� ���� ���� ���� �ɼ��� ���������մϴ�.<br/>
    /// ���⸦ ������ ���� �Ǵ� ���� �ϴ� ��� �������¸� ���� �ݿ� �ɼ��� Ȱ��ȭ �ؾ� �ϸ�,<br/>
    /// ���� ���� ���� �ɼ��� ��� �ε� �ÿ� ���� ���¸� ������ ä�� ���� �� ������ �ؾ� �մϴ�.
    /// </summary>
    /// <returns>������ȯ ���� �� true��, ���� �� false�� ��ȯ</returns>
    public void EquipSwitch( bool isForceUserState = false, bool isRemainEquipState = false )
    {
        // ���� ���� ���� �ɼ��� �ɷ����� ���� ��쿡�� ������ ���� ���¸� ������ŵ�ϴ�.
        if( !isRemainEquipState )
            IsEquip=!IsEquip;

        
        // ����Ʈ �������̸鼭 �Ӹ��� �����ϴ� �������� ���
        if( EquipType==EquipType.Helmet && Type==ItemType.Quest )
        {
            // ���� ���� ���¸� �������� equipInfo�� �������� ����޼��带 ȣ���մϴ�.
            equipInfo.EquipHoodSwitch( IsEquip );
        }


        // ����� �����ϴ� �������� ���
        else if( EquipType==EquipType.Weapon )
        {
            // ���� ������¸� �������� ���۹���� ����޼��带 ȣ���մϴ�.
            if( IsEquip )
                EquipTransfer();    // �������� �����Ͽ� �����մϴ�.            
            else
                UnequipTransfer();  // �������� �����Ͽ� ���� �����մϴ�.


            //  ���� ���� ���� �ݿ� �ɼ��� �ɷ��ִ� ��� (�����ڵ�)
            if( isForceUserState )
            {
                PlayerWeapon playerWeapon = ownerTr.GetComponent<PlayerWeapon>();

                if( playerWeapon!=null )
                {
                    if(IsEquip)
                        playerWeapon.ChangeWeaponDirectly( this.WeaponType );
                    else
                        playerWeapon.ChangeWeaponDirectly( WeaponType.None );
                }
            }


        }



        else
            throw new Exception( "���� EquipType�� ���� �޼��尡 ���ε��� �ʾҽ��ϴ�." );
    }




    STransform prevEquipLocalTr = new STransform();   // �������� ���� ������ ������ �ִ� ��ȯ���� 

    /// <summary>
    /// �������� ������ �� ȣ������� �ϴ� �޼����Դϴ�.<br/>
    /// ���ڷ� ���޵� Transform�� �������� �ڽ����μ� ���ϰ� �Ǹ�,<br/>
    /// �������� ���������� �����ϰ� �ִ� STransform ������ ����ȭ �մϴ�.<br/><br/>
    /// *** ��� �������� �ƴ϶�� ���ܰ� �߻��մϴ�. ***
    /// </summary>
    private void EquipTransfer()
    {
        ItemEquip itemEquip = item as ItemEquip;

        if( itemEquip==null )
            throw new Exception( "�������� ���������� Ÿ���� �ƴմϴ�." );


        // �������� ���������� ����Ǳ� ���� ���� ũ�⸦ ����մϴ�.
        Vector3 prevItem2dLossyScale = itemRectTr.lossyScale;

        // �������� ���������� ���ϱ� ��(2D���¿���) ���� ���� ��ȯ������ ����մϴ�.
        prevEquipLocalTr.Serialize(Item3dTr, true);        
                

        // ���� ������ ��ȯ�޽��ϴ�.
        Transform equipParentTr = equipInfo.GetEquipParentTr(this);

        // �κ��丮�� ��ϵ� ���·� ���� ������ �������� �����ϰ� ������ ���ӽ�ŵ�ϴ�.
        OnItemWorldDrop(equipParentTr, TrApplyRange.Pos, true, true);
        
        // �������� ���� ������ ���������� ���� STransform ���� Item3dTr�� ���������� ��������ݴϴ�.
        itemEquip.EquipLocalTr.Deserialize(Item3dTr, true);
        
        
        // 2d�������� ���� �ڽĻ��¿��� ������ �Ͼ�Ƿ� ������ ������ ���߾��ݴϴ�.
        // �׷��� ������ �ٽ� ���Կ� ���ư� �� �������� ������ ���°� �ǹǷ� �̹��� ����� ���ϰ� �� ���Դϴ�
        STransform.SetLossyScale(itemRectTr, prevItem2dLossyScale);
        
        // �������� �⺻ �ݶ��̴��� ��Ȱ��ȭ �մϴ�.
        if(itemCol!=null)
            itemCol.enabled = false;
        
        // �������� �߷��� �����մϴ�.
        if(itemRb!=null)
            itemRb.useGravity = false;

        // ������ ������ ���� ���� ������ ������Ʈ�մϴ�.
        dummyInfo.OnItemEquip(true); 
    }



    /// <summary>
    /// ������ ������ ������ �� ȣ������� �ϴ� �޼����Դϴ�.<br/>
    /// ���� ���� �������� ��ȯ ������ ���� ȸ���մϴ�.<br/><br/>
    /// *** ��� �������� �ƴ϶�� ���ܰ� �߻��մϴ�. ***
    /// </summary>
    private void UnequipTransfer()
    {
        ItemEquip itemEquip = item as ItemEquip;
                
        if( itemEquip==null )
            throw new Exception( "�������� ���������� Ÿ���� �ƴմϴ�." );
                        
        // ������ ������ ������ 2D���·� �����մϴ�.
        DimensionShift(false);

        // �������� 2D �������� ������Ʈ�մϴ�.
        UpdatePositionInfo();

        // �������� �����ϰ� �ִ� ���� ���� ��ȯ������ Item3DTr�� �Է��Ͽ� ���󺹱� �����ݴϴ�.
        prevEquipLocalTr.Deserialize( Item3dTr, true );

        // �������� �⺻ �ݶ��̴��� Ȱ��ȭ �մϴ�.
        if( itemCol!=null )
            itemCol.enabled = true;

        // �������� �߷��� Ȱ��ȭ�մϴ�.
        if( itemRb!=null )
            itemRb.useGravity = true;
        
        // ������ ������ ���� ���� ������ ������Ʈ�մϴ�.
        dummyInfo.OnItemEquip(false);

    }














    bool isRegisterdToWorld = false;    // ���� �κ��丮�� ��ϵǾ����� ����

    /// <summary>
    /// �������� ��� ���µ� ��ȯ ���� �ʰ� ���� �κ��丮�� ��ϸ� �մϴ�.<br/>
    /// ��, 3D������ �������� ���� �κ��丮�� �߰��Ǿ 2D���·� ������� �ʽ��ϴ�.<br/><br/>
    /// ���ڷ� �������� ������ ������ �����ؾ� �մϴ�.<br/><br/>
    /// *** �� �޼��带 ���� �������� ���� �κ��丮�� ��ϵǾ �����ĺ���ȣ�� ���� �Ҵ� ���� �ʽ��ϴ�. ***
    /// </summary>
    /// <returns>�ڱ��ڽ��� �������� �״�� ��ȯ�մϴ�. �߰��� �ٸ� �޼��带 ȣ���� �� �ֽ��ϴ�</returns>
    public ItemInfo RegisterWorldInvetory(UserInfo ownerInfo)
    {
        // ������ ������ �ĺ���ȣ�� ������ �ĺ���ȣ�� �����մϴ�.
        item.OwnerId = ownerInfo.UserId;    

        // ������ ������ ���� ������ ������ ������ �����մϴ�.
        item.OwnerName = ownerInfo.UserPrefabName;

        // ������ ������ ��ġ�� �����մϴ�.
        ownerTr = ownerInfo.transform;

        // ���� �κ��丮�� �����մϴ�.
        worldInventoryInfo.Inventory.AddItem(this);

        // ��ϵǾ����� Ȱ��ȭ�մϴ�.
        isRegisterdToWorld = true;

        return this;
    }

    /// <summary>
    /// �������� ���� Id�� �Ҵ�޽��ϴ�.<br/>
    /// CreateManager�� �ο��� ���� ID�� �����ؾ��մϴ�.<br/><br/>
    /// *** �������� ���忡 ��ϵ� ���°� �ƴ϶�� ���ܰ� �߻��մϴ�. ***
    /// </summary>
    public void SetPrivateId(int id)
    {
        // ���� �κ��丮�� ��ϵ��� �ʾҴٸ� ����ó��
        if( !isRegisterdToWorld )
            throw new Exception("���� �κ��丮�� ��ϵ� ���°� �ƴմϴ�.");

        item.Id = id;
    }








    /// <summary>
    /// ����� ��¿� �޼��� - ������ ������ Ȯ���մϴ�.
    /// </summary>
    public void PrintDebugInfo()
    {
        item.ItemDeubgInfo();
    }






}
