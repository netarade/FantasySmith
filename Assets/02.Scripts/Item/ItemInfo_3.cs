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













    public void PrintDebugInfo()
    {
        string debugInfo = "";
        
        debugInfo += string.Format($"�̸� : {item.Name}\n" );
        debugInfo += string.Format($"���� : {item.Type}\n" );
        debugInfo += string.Format($"��ü �� �ε��� : {item.SlotIndexAll}\n" );
        debugInfo += string.Format($"���� �� �ε��� : {item.SlotIndexEach}\n" );

        print(debugInfo);
    }






}
