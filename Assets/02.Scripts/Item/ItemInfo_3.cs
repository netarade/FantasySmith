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





    STransform prevEquipLocalTr = new STransform();   // �������� ���� ������ ������ �ִ� ��ȯ���� 

    /// <summary>
    /// �������� ������ �� ȣ������� �ϴ� �޼����Դϴ�.<br/>
    /// ���ڷ� ���޵� Transform�� �������� �ڽ����μ� ���ϰ� �Ǹ�,<br/>
    /// �������� ���������� �����ϰ� �ִ� STransform ������ ����ȭ �մϴ�.<br/><
    /// *** ��� �������� �ƴ϶�� ���ܰ� �߻��մϴ�. ***
    /// </summary>
    public bool OnItemEquip(bool isLoad=false)
    {
        ItemEquip itemEquip = item as ItemEquip;

        if( itemEquip==null )
        {
            Debug.Log( "�������� ���������� Ÿ���� �ƴմϴ�." );
            return false;
        }

         // �ε尡 �ƴ� ���¿��� ������ �ΰ�� 
        if( itemEquip.isEquip && !isLoad )
        {
            Debug.Log( "�� �������� �̹� ���� �����̹Ƿ� ������ �� �� �����ϴ�." );
            return false;
        }


        // �������¸� Ȱ��ȭ�մϴ�.
        itemEquip.isEquip = true;


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

        // ���� �̹����� Ȱ��ȭ�մϴ�.
        dummyInfo.DummyImg.enabled = true;
        // ���� ������Ʈ�� �������� ������Ʈ�մϴ�.
        dummyInfo.UpdatePositionInfo();

        // ���� �׼��� �����Ͽ� ����Ǿ��ִ� ��� �޼��忡 ���¸� �ݿ��մϴ�.
        if(EquipAction!=null)
            EquipAction(true);

        // ������ ��ȯ�մϴ�.
        return true;        
    }




   






    /// <summary>
    /// ������ ������ ������ �� ȣ������� �ϴ� �޼����Դϴ�.<br/>
    /// ���� ���� �������� ��ȯ ������ ���� ȸ���մϴ�.<br/><br/>
    /// </summary>
    public bool OnItemUnequip()
    {
        ItemEquip itemEquip = item as ItemEquip;

        if( !itemEquip.isEquip )
        {
            Debug.Log( "���� ���°� �ƴϹǷ� ������ ������ �� �����ϴ�." );
            return false;
        }
        


        // ���� ���¸� ��Ȱ��ȭ �մϴ�.
        itemEquip.isEquip = false;
                
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

        // ���� �̹����� ��Ȱ��ȭ �մϴ�.
        dummyInfo.DummyImg.enabled = false;
        // ���̿�����Ʈ�� �������� ������Ʈ�մϴ�.
        dummyInfo.UpdatePositionInfo();

        // ���� ���� �׼��� �����Ͽ� ����Ǿ��ִ� ��� �޼��忡 ���¸� �ݿ��մϴ�.
        if(EquipAction!=null)
            EquipAction(false);

        // ������ ��ȯ�մϴ�.
        return true;
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
        worldInventoryInfo.inventory.AddItem(this);

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
