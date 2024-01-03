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
    /// �ش� �̸��� �������� �κ��丮�� ��Ͽ��� �����Ŀ� ��Ͽ��� ������ �������� ItemInfo �������� ��ȯ�մϴ�.<br/>
    /// ���� �� �ٷ� �ı��Ϸ��� �ι� ° ���ڸ� true�� ������ �մϴ�. (�⺻������ ��Ͽ��� ���Ÿ� �� �� �ı����� �ʽ��ϴ�.)<br/><br/>
    /// ��ȯ ���� �������� ���忡 �������� ���ؼ��� �ش� ItemInfo������ OnItemWroldDrop�޼��带 ����ؾ� �ϸ�,<br/>
    /// �ٸ� �κ��丮�� �ֱ� ���ؼ��� �ٸ� InventoryInfo������ AddItem�޼��带 ����� ItemInfo�� �����ؾ� �մϴ�.<br/><br/>
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
    /// �κ��丮�� ��Ͽ� ���忡 �����ϴ� �������� �߰��ϰų�<br/>
    /// �ٸ� �κ��丮�� �����ϴ� �������� �߰��մϴ�.<br/>
    /// *** ���ڷ� ���� ������Ʈ �������� ������ ���ܸ� �߻���ŵ�ϴ�. ***<br/>
    /// </summary>
    public bool AddItem(ItemInfo itemInfo)
    {        
        if( itemInfo==null )
            throw new Exception( "���� ���� ������ ������ ���� ���� �������� �ʽ��ϴ�." );  

        ItemType inItemType = itemInfo.Item.Type;
        string inItemName = itemInfo.Item.Name;

        // ��ȭ ������ ���ο� ���� �κ��丮�� �ش� ��ȭ�����۰� ������ ��ȭ�������� �ִ��� �˻�
        if( inItemType == ItemType.Misc && inventory.IsExist(inItemName) )
        {            
            ItemMisc inItemMisc = (ItemMisc)itemInfo.Item;
            int inCount = inItemMisc.OverlapCount; 
            
            List<GameObject> existItemObjList = inventory.GetItemObjectList(inItemName);
            


            inventory.SetOverlapCount(inItemName, inCount);


        }


        // ����ȭ �������̰ų�, ������ �̸��� ��ȭ�������� ���� ���
        return inventory.AddItem( itemInfo );
    }



    /// <summary>
    /// �κ��丮�� ��Ͽ� ���� �������� ���Ӱ� ������ ��û�մϴ�.
    /// </summary>
    public bool CreateItemToNearstSlot()
    {
        return true;
    }





}
