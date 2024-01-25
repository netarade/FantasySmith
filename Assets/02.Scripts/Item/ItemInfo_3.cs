using CreateManagement;
using DataManagement;
using ItemData;
using System;
using UnityEngine;

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







    public void OnItemUse()
    {
            
    }



    public void OnItemDrink()
    {

    }


    /// <summary>
    /// ��� �������� �����մϴ�.<br/>
    /// ���ڷ� ���޵� Transform�� ������ ��ġ�� ȸ�������� ������, �������� �ڽ����μ� ���ϰ� �˴ϴ�.<br/><br/>
    /// *** ��� �������� �ƴ϶�� ���ܰ� �߻��մϴ�. ***
    /// </summary>
    /// <returns>�ش� �������� ItemInfo ���� �ٽ� ��ȯ</returns>
    public ItemInfo OnItemEquip(Transform equipTr)
    {
        if( !(item is ItemEquip) )
            throw new Exception("�������� ���������� Ÿ���� �ƴմϴ�.");

        OnItemWorldDrop(equipTr, true);
        return this;
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
