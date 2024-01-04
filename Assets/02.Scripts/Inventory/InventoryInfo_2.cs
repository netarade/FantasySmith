using UnityEngine;
using InventoryManagement;
using DataManagement;
using ItemData;
using System;
using System.Collections.Generic;

/*
 * [�۾� ����]  
 * <v1.0 - 2024_0103_�ֿ���>
 * 1- AddItem, RemoveItem�޼��� �����ε� ��κ��� �����ϰ� �ʿ��� �޼��常 ����
 * 
 * 
 * (�̽�)
 * 1- ��ȭ �������� ��� AddItem�� �� ���� �������� �ִ� ��� ���� �����ۿ� �� ������ ����ϴٸ�, 
 * ���ڷ� ���� ������Ʈ�� �ı����Ѿ� �Ѵ�.
 * 
 * 2- Split�޼��� ����
 * �κ��丮 ���ο��� Ư��Ű(ctrl��) Ŭ�� �� Split ��� 
 * Ȥ�� ���忡 ����� �� ��ȭ�������� ��� ����� ������ �����Ͽ� Split�� �� �ְ� �ؾ��Ѵ�.
 * ������ �ٸ� ���ο� ������Ʈ�� ������Ű�� ���� ������ �����ؾ� �Ѵ�. ���ο��� split�� ��Ͽ� Add���� �ؾ� �ϰ�,
 * �ܺε���� ���� Split�ÿ��� �κ��丮 ��Ͽ��� �����ϸ� �ȵȴ�.
 * 
 * 
 */


public partial class InventoryInfo : MonoBehaviour
{
    /// <summary>
    /// InventoryŬ�������� �ʿ� �� ������ ������ ���۹ޱ� ���� ����ϴ� ItemInfo�� �ӽ������� ��� ����Ʈ�Դϴ�.<br/>
    /// AddItem�޼��忡�� 
    /// </summary>
    List<ItemInfo> tempInfoList = new List<ItemInfo>();






    /// <summary>
    /// �ش� �̸��� �������� �κ��丮�� ��Ͽ��� �����Ŀ� ��Ͽ��� ������ �������� ItemInfo �������� ��ȯ�մϴ�.<br/>
    /// ���� �� �ٷ� �ı��Ϸ��� �ι� ° ���ڸ� true�� ������ �մϴ�. (�⺻������ ��Ͽ��� ���Ÿ� �� �� �ı����� �ʽ��ϴ�.)<br/><br/>
    /// ��ȯ ���� �������� ���忡 �������� ���ؼ��� �ش� ItemInfo������ OnItemWroldDrop�޼��带 ����ؾ� �ϸ�,<br/>
    /// ����� ���� �������� �ٸ� �κ��丮�� �ֱ� ���ؼ��� �ٸ� InventoryInfo������ AddItem�޼��带 ����� ItemInfo�� �����ؾ� �մϴ�.<br/><br/>
    /// *** �κ��丮�� �ش� �̸��� �������� ������ ���ܸ� �߻���ŵ�ϴ�. ***<br/>
    /// </summary>
    public ItemInfo RemoveItem(string itemName, bool isDestroy=false)
    {
        ItemInfo targetItemInfo = inventory.RemoveItem(itemName);   // �κ��丮 ��Ͽ��� ����
        
        if( targetItemInfo==null )
            throw new Exception( "�ش� �̸��� �������� �������� �ʽ��ϴ�." );        
        else if( isDestroy )
        {
            Destroy( targetItemInfo.gameObject );  // �ı� �ɼ��� �ɷ��ִٸ�, �������� �ı��ϰ� null�� ��ȯ�մϴ�.     
            return null;
        }

        return targetItemInfo;
    }

    /// <summary>
    /// �κ��丮�� ��Ͽ� *����*�� �����ϴ� ������ �������� �߰��մϴ�.<br/>
    /// �ٸ� �κ��丮�� �����ϴ� �������� OnItemSlotDrop�޼��� ȣ���� ���� �ٸ� �κ��丮�� ���̵Ǿ�� �մϴ�.<br/><br/>
    /// *** ���ڷ� ���� ������Ʈ �������� null�̶�� ���ܸ� �߻���ŵ�ϴ�. ***<br/>
    /// </summary>
    /// <returns>������Ʈ�� ���� �� �� ������ �����ϴٸ� false��, ������ ���� ���� �� true�� ��ȯ</returns>
    public bool AddItem(ItemInfo itemInfo)
    {        
        if( itemInfo==null )
            throw new Exception( "���� ���� ������ ������ ���� ���� �������� �ʽ��ϴ�." );  
                        
        inventory.AddItem( itemInfo );

        // �κ��丮 ������ ���ڷ� ���޹��� ���ο� �κ��丮�� ������Ʈ �մϴ�.
        itemInfo.UpdateInventoryInfo(this);

        // �������� ���� �ε��� ������ ���� ����� �������� �Է��մϴ�.        
        SetItemSlotIdxBothToNearstSlot(itemInfo);
                
        // �������� ��ġ������ �ݿ��մϴ�.
        itemInfo.UpdatePositionInSlotList();


        
    }

    


    /// <summary>
    /// �κ��丮�� ��Ͽ� ���� �������� ���Ӱ� �����ϰ� �κ��丮 ��Ͽ� �߰��մϴ�.<br/>
    /// ������Ʈ 1���� �����ϹǷ�, ���� �������� ������Ű�� ���� �� �ߺ��Ͽ� ȣ���ؾ� �մϴ�.<br/>
    /// ��ȭ�������� ��� ��ø������ ������ �� �ֽ��ϴ�. ����ȭ �������� ���� �����մϴ�.(�⺻��:1)<br/>
    /// </summary>
    /// <returns>�κ��丮�� ���Կ� ������ 1���� ������ ������ ������� �ʴٸ� false�� ��ȯ, ������ ��� true�� ��ȯ</returns>
    public bool CreateItem(string itemName, int miscOverlapCount=1)
    {
        return true;
    }

    /// <summary>
    /// �κ��丮�� ��Ͽ� ���� �������� ���Ӱ� �����ϰ� �κ��丮 ��Ͽ� �߰��մϴ�.<br/>
    /// ���ڷ� ������ �̸��� ��ø ���� ���� ����ü�� ���޹޽��ϴ�.<br/><br/>
    /// ������Ʈ ������ ���� �������� ������ �� �ֽ��ϴ�.<br/>
    /// ����ȭ�������� ��� ItemPair�� overlapcount�� ���õ˴ϴ�.<br/>
    /// </summary>
    /// <returns>�κ��丮�� ���Կ� ������ ���� ���� ������ ������ ������� �ʴٸ� false�� ��ȯ, ������ ��� true�� ��ȯ</returns>
    public bool CreateItem(ItemPair[] itemPairs)
    {
        return true;
    }





}
