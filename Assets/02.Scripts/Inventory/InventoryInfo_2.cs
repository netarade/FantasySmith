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
 * <v1.1 - 2024_0105_�ֿ���>
 * 1- AddItem�޼��忡�� ���� �κ��丮�� �߰��ϰ� �ε����� �Է��ϴ� �Ϳ���
 * �ε����� ���� �Է��ϰ� �κ��丮�� �߰��ϴ� ������ �����Ͽ���.
 * ������ �̸� �߰��� �������� �ε��� ������ �ε����� ���ϴ� �������� �о���̱� ����.
 * 
 * <v1.2 -2024_0108_�ֿ���>
 * 1- ItemInfo�� ���� �����Ͽ� �κ��丮 ��Ͽ��� �������ִ� RemoveItem �����ε� �޼��� �߰�
 * ItemSelect ��ũ��Ʈ���� �κ��丮���� �ܺη� �������� ����Ͽ��� �� �ش� �������� ���� �����ؾ��ϱ� ����
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
    /// ���� �� �ٷ� �ı��Ϸ��� �ι� ° ���ڸ� true�� �����մϴ�. (�⺻������ �ı����� �ʽ��ϴ�.)<br/><br/>
    /// ���� �� �������� �ڵ����� World�� InventoryInfoŬ������ playerDropTr�� �����ص� ���� ����Ʈ���ݴϴ�.<br/><br/>
    /// ����� ���� �������� �ٸ� �κ��丮�� �ֱ� ���ؼ��� �ٸ� InventoryInfo������ AddItem�޼��带 ����� �ٽ� ItemInfo�� �����ؾ� �մϴ�.<br/><br/>
    /// *** �κ��丮�� �ش� �̸��� �������� ������ ���ܸ� �߻���ŵ�ϴ�. ***<br/>
    /// </summary>
    public ItemInfo RemoveItem(string itemName, bool isDestroy=false)
    {
        // �κ��丮 ��Ͽ��� �����ϰ� ��ȯ �� �������� �����մϴ�.
        ItemInfo targetItemInfo = inventory.RemoveItem(itemName);   
        
        if( targetItemInfo==null )
            throw new Exception( "�ش� �̸��� �������� �������� �ʽ��ϴ�." );       
        
        // �ı� �ɼ��� �ɷ��ִٸ�, �������� �ı��ϰ� null�� ��ȯ�մϴ�.
        else if( isDestroy )
        {
            Destroy( targetItemInfo.gameObject );       
            return null;
        }
        
        // �������� 3D���·� ��ȯ�մϴ�.
        targetItemInfo.DimensionShift(true);

        return targetItemInfo;
    }


    /// <summary>
    /// �ش� �������� �κ��丮�� ��Ͽ��� ���� �����Ŀ� ��Ͽ��� ������ �������� ItemInfo �������� ��ȯ�մϴ�.<br/>
    /// ���� �� �ٷ� �ı��Ϸ��� �ι� ° ���ڸ� true�� �����մϴ�. (�⺻������ �ı����� �ʽ��ϴ�.)<br/><br/>
    /// ���� �� �������� �ڵ����� World�� InventoryInfoŬ������ playerDropTr�� �����ص� ���� ����Ʈ���ݴϴ�.<br/><br/>
    /// ����� ���� �������� �ٸ� �κ��丮�� �ֱ� ���ؼ��� �ٸ� InventoryInfo������ AddItem�޼��带 ����� �ٽ� ItemInfo�� �����ؾ� �մϴ�.<br/><br/>
    /// *** �κ��丮�� �ش� �̸��� �������� ������ ���ܸ� �߻���ŵ�ϴ�. ***<br/>
    /// </summary>
    public ItemInfo RemoveItem(ItemInfo itemInfo, bool isDestroy=false)
    {
        // �κ��丮 ��Ͽ��� �����ϰ� ��ȯ �� �������� �����մϴ�.
        ItemInfo targetItemInfo = inventory.RemoveItem(itemInfo);   
        
        if( targetItemInfo==null )
            throw new Exception( "�ش� �̸��� �������� �������� �ʽ��ϴ�." );       
        
        // �ı� �ɼ��� �ɷ��ִٸ�, �������� �ı��ϰ� null�� ��ȯ�մϴ�.
        else if( isDestroy )
        {
            Destroy( targetItemInfo.gameObject );       
            return null;
        }
        // �������� 3D���·� ��ȯ�մϴ�.
        targetItemInfo.DimensionShift(true);

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
        // ItemInfo�� ���޹��� ���� ���
        if( itemInfo==null )
            throw new Exception( "���� ���� ������ ������ ���� ���� �������� �ʽ��ϴ�." );

        // ���Կ� ���ڸ��� ������ �������� �ʽ��ϴ�.
        if( !IsSlotEnough( itemInfo ) )
            return false;

        // �������� ����� �����ϴ� ���¶��, 2D�� ���������� �����մϴ�.
        if( itemInfo.IsWorldPositioned )
            itemInfo.DimensionShift( false );
                
        // �κ��丮 ������ ���ο� �κ��丮�� ������Ʈ �մϴ�.
        itemInfo.UpdateInventoryInfo(this);
        
        // ������������ ���� �κ��丮�� ����� �������� �Է��մϴ�.        
        SetItemSlotIdxBothToNearstSlot(itemInfo);  

        // ���� �κ��丮�� �������� �߰��մϴ�.
        inventory.AddItem(itemInfo);  

        
        // �������� �ֽ� ������ �ݿ��մϴ� 
        if(itemInfo.Item.Type == ItemType.Misc)
            UpdateAllItemInfo();    // ��ȭ�������� ��� ���� �����ۿ� ������ ���� ���� �����Ƿ� ��� ���� ������Ʈ
        else
            itemInfo.UpdatePositionInSlotList(); // ���� �������� �ش� �����۸� ������Ʈ �մϴ�.

        // ������ �߰� ������ ��ȯ�մϴ�.
        return true;
        
            
    }

    


    /// <summary>
    /// �κ��丮�� ��Ͽ� *����* �������� ���Ӱ� �����ϰ� �κ��丮 ��Ͽ� �߰��մϴ�.<br/>
    /// ������Ʈ 1���� �����ϹǷ�, ���� �������� ������Ű�� ���� �� �ߺ��Ͽ� ȣ���ؾ� �մϴ�.<br/>
    /// ��ȭ�������� ��� ��ø������ ������ �� �ֽ��ϴ�. ����ȭ �������� ���� �����մϴ�.(�⺻��:1)<br/>
    /// </summary>
    /// <returns>�κ��丮�� ���Կ� ������ 1���� ������ ������ ������� �ʴٸ� false�� ��ȯ, ������ ��� true�� ��ȯ</returns>
    public bool AddItem(string itemName, int overlapCount=1)
    {
        Debug.Log("���� ���� " + IsSlotEnough(itemName, overlapCount));

        if( !IsSlotEnough(itemName, overlapCount) )
            return false;
                
        // createManager���� ������û���Ͽ� ������Ʈ �ϳ��� ����ϴ�.
        ItemInfo itemInfo = createManager.CreateWorldItem(itemName, overlapCount);

        // �ش� �������� �κ��丮�� �ֽ��ϴ�.
        AddItem(itemInfo);

        return true;
    }

    /// <summary>
    /// �κ��丮�� ��Ͽ� *����* �������� ���Ӱ� �����ϰ� �κ��丮 ��Ͽ� �߰��մϴ�.<br/>
    /// ���ڷ� ������ �̸��� ��ø ���� ���� ����ü�� ���޹޽��ϴ�.<br/><br/>
    /// ������Ʈ ������ ���� �������� ������ �� �ֽ��ϴ�.<br/>
    /// ����ȭ�������� ��� ItemPair�� overlapcount�� ���õ˴ϴ�.<br/>
    /// </summary>
    /// <returns>�κ��丮�� ���Կ� ������ ���� ���� ������ ������ ������� �ʴٸ� false�� ��ȯ, ������ ��� true�� ��ȯ</returns>
    public bool AddItem(ItemPair[] itemPairs)
    {
        return true;
    }









}
