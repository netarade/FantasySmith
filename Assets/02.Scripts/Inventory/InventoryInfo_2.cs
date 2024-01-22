using UnityEngine;
using InventoryManagement;
using DataManagement;
using ItemData;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

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
 * <v2.0 - 2024_0109_�ֿ���>
 * 1- AddItem(ItemInfo) �޼��忡�� ��ȭ�������� ���� ���� �Ϲݾ������� ��츦 �����Ͽ� ���� �ۼ�
 * AddItem�� ���� ���ο��� ���ӿ�����Ʈ�� �ı��Ǵ���� ItemInfo�� �ٷ� �ı����� �ʰ� �������� �����ϱ� ������
 * �� �� ������ �ľ��Ͽ� �ʿ��� ó���� �ϵ����Ͽ���.
 * 
 * <v2.1 - 2024_0111_�ֿ���>
 * 1- RemoveItem�޼����� ������ targetItemInfo->rItemInfo�� ���� 
 * 2- �̸��� ���ڷ� �޴� RemoveItem�޼��� �� rItemInfo.UpdateInventoryInfo(null); �߰�
 * 
 * <v3.0 -2024_0114_�ֿ���>
 * 1- UpdateAllOverlapCountInExist�޼��� �ּ� �߰�
 * 2- ��ӽ�ũ��Ʈ QuickSlot�� �����ϰ� ���üӼ��� ��ӹޱ����� private ������ �޼��带 protectedó��
 * 
 * <v3.1 - 2024_0115_�ֿ���>
 * 1- AddItem�޼��� ������ FindSlotIdxToNearstSlot�� ���������� ȣ�� �ϴ� ���� ����
 * InventoryŬ�������� �ۿ� Index�� ���� �� �����Ƿ�, InventoryŬ������ AddItem���� ���������� �����Ͽ��� ����
 * 
 * <V3.2 - 2024_0116_�ֿ���>
 * 1- ���� AddItem �޼��� ������ ��ȭ ������, ����ȭ�������� ������ �߰��ϴ� ������ �޼���ȭ �Ͽ�
 * AddItemBasic�� AddItemMisc �޼���� ����������, AddItem ���ο� ���� (protectedó��)
 * 
 * 2- AddItem�� ItemPairs[]�� ���ڷ� �޴� �����ε� �޼��� �ۼ�
 * 
 * 3- AddItemToSlot�޼��� �ۼ�
 * (Ư�� ���Կ� �ֱ� ���� ���� �ε����� �ǻ��¸� �޴� �޼���)
 * 
 * <v3.3 - 2024_0116_�ֿ���>
 * 1- AddItemToSlot�޼��忡�� slotIndex�� ����ó���ϴ� �κл���
 * (IsSlotEnough���� �����ϹǷ� �ʿ����)
 * 
 * <v3.4 - 2024_0122_�ֿ���>
 * 1- �޼���� UpdateAllOVerlapCountInExist�� UpdateTextInfoSameItemName���� ����
 * 
 */

public partial class InventoryInfo : MonoBehaviour
{
    /// <summary>
    /// InventoryŬ�������� �ʿ� �� ������ ������ ���۹ޱ� ���� ����ϴ� ItemInfo�� �ӽ������� ��� ����Ʈ�Դϴ�.<br/>
    /// AddItem�޼��忡�� 
    /// </summary>
    protected List<ItemInfo> tempInfoList = new List<ItemInfo>();



    
    /// <summary>
    /// �������� �̸��� ������ ���ڷ� �޾� �ش� �������� ������ŭ ���ҽ����ݴϴ�.<br/>
    /// �������� ������ ������� �ش� ���� ��ŭ �κ��丮���� ���ҽ�Ų �� ���� ���θ� ��ȯ�մϴ�.<br/>
    /// ��ȭ�������� ��� ��ø������ ���ҽ�Ű��, �Ϲ� �������� ��� ������Ʈ ������ ���ҽ�ŵ�ϴ�.<br/><br/>
    /// *** ���� ���ڰ� 0���϶�� ���ܸ� �߻���ŵ�ϴ�. ***
    /// </summary>
    /// <returns>������ ���� ���ҿ� ������ ��� true��, �������� �ʰų� ������ ������� �ʴٸ� false�� ��ȯ</returns>
    public bool ReduceItem(string itemName, int count)
    {
        return IsItemEnough(itemName, count, true);
    }
    
    /// <summary>
    /// �������� �̸��� ������ ������ ����ü �迭�� ���ڷ� �޾� �ش� �������� ������ŭ ���ҽ����ݴϴ�.<br/>
    /// �������� ������ ������� �ش� ���� ��ŭ �κ��丮���� ���ҽ�Ų �� ���� ���θ� ��ȯ�մϴ�.<br/>
    /// ��ȭ�������� ��� ��ø������ ���ҽ�Ű��, �Ϲ� �������� ��� ������Ʈ ������ ���ҽ�ŵ�ϴ�.<br/><br/>
    /// *** ���� ���ڰ� 0���϶�� ���ܸ� �߻���ŵ�ϴ�. ***
    /// </summary>
    /// <returns>������ ���� ���ҿ� ������ ��� true��, �������� �ʰų� ������ ������� �ʴٸ� false�� ��ȯ</returns>
    public bool ReduceItem( ItemPair[] itemPairs )
    {
        return IsItemEnough(itemPairs, true);
    }









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
        ItemInfo rItemInfo = inventory.RemoveItem(itemName);   
        
        if( rItemInfo==null )
            throw new Exception( "�ش� �̸��� �������� �������� �ʽ��ϴ�." );       
        
        // �ı� �ɼ��� �ɷ��ִٸ�, �������� �ı��ϰ� null�� ��ȯ�մϴ�.
        else if( isDestroy )
        {
            Destroy( rItemInfo.gameObject );       
            return null;
        }
        
        // �������� 3D���·� ��ȯ�մϴ�.
        rItemInfo.DimensionShift(true);
        rItemInfo.UpdateInventoryInfo(null);

        return rItemInfo;
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
        ItemInfo rItemInfo = inventory.RemoveItem(itemInfo);   
        
        if( rItemInfo==null )
            throw new Exception( "�ش� �̸��� �������� �������� �ʽ��ϴ�." );       
        
        // �ı� �ɼ��� �ɷ��ִٸ�, �������� �ı��ϰ� null�� ��ȯ�մϴ�.
        else if( isDestroy )
        {
            Destroy( rItemInfo.gameObject );       
            return null;
        }
        // �������� 3D���·� ��ȯ�մϴ�.
        rItemInfo.DimensionShift(true);
        rItemInfo.UpdateInventoryInfo(null);

        return rItemInfo;
    }







    /// <summary>
    /// �κ��丮�� ��Ͽ� ���� �������� ���Ӱ� �����ϰ� ����� ���Կ� �߰��մϴ�.<br/>
    /// ������Ʈ 1���� �����ϹǷ�, ���� �������� ������Ű�� ���� �� �ߺ��Ͽ� ȣ���ؾ� �մϴ�.<br/>
    /// ��ȭ�������� ��� ��ø������ ������ �� �ֽ��ϴ�. ����ȭ �������� ���� �����մϴ�.(�⺻��:1)<br/>
    /// </summary>
    /// <returns>�κ��丮�� ���Կ� ������ 1���� ������ ������ ������� �ʴٸ� false�� ��ȯ, ������ ��� true�� ��ȯ</returns>
    public bool AddItem(string itemName, int overlapCount=1)
    {
        if( !IsSlotEnough(itemName, overlapCount) )
            return false;
                
        // createManager���� ������û���Ͽ� ������Ʈ �ϳ��� ����ϴ�.
        ItemInfo itemInfo = createManager.CreateWorldItem(itemName, overlapCount);

        // �ش� �������� �κ��丮�� �ֽ��ϴ�.
        AddItem(itemInfo);

        return true;
    }

    
    /// <summary>
    /// �κ��丮�� ��Ͽ� *����* �������� ���Ӱ� �����ϰ� ����� ���Կ� �߰��մϴ�.<br/>
    /// ���ڷ� ������ �̸��� ��ø ���� ���� ����ü�� ���޹޽��ϴ�.<br/><br/>
    /// ������Ʈ ������ ���� �������� ������ �� �ֽ��ϴ�.<br/>
    /// ����ȭ�������� ��� ItemPair�� overlapcount�� ���õ˴ϴ�.<br/>
    /// </summary>
    /// <returns>�κ��丮�� ���Կ� ������ ���� ���� ������ ������ ������� �ʴٸ� false�� ��ȯ, ������ ��� true�� ��ȯ</returns>
    public bool AddItem(ItemPair[] itemPairs)
    {
        for( int i = 0; i<itemPairs.Length; i++ )
        {            
            // �ش� �̸��� �������� �ش� ������ŭ �������� �� ������ ����ϴٸ� �ݺ����� ���������� ��� �˻��մϴ�
            if( IsSlotEnough( itemPairs[i].itemName, itemPairs[i].overlapCount ) )
                continue;
            
            // �ѹ��̶� �����ϴٸ� false�� ��ȯ�մϴ�.
            return false;
        }

        // ��� �������� �߰��մϴ�
        for(int i=0; i<itemPairs.Length; i++)
            AddItem( itemPairs[i].itemName, itemPairs[i].overlapCount );
        
        return true;
    }






    /// <summary>
    /// �κ��丮�� ��Ͽ� *����*�� �����ϴ� ������ �������� ����� ���Կ� �߰��մϴ�.<br/>
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


        // �������� Ÿ���� ��ȭ������ ��� ���ļ����� ���Ͽ� ó���մϴ�.
        if( itemInfo.Item.Type is ItemType.Misc )
            AddItemMisc(itemInfo);
        // �Ϲ� �������� ��� �ٷ� �߰��մϴ�.
        else
            AddItemBasic(itemInfo);


        // ������ �߰��� ������ ��ȯ�մϴ�.
        return true;
    }

    

    /// <summary>
    /// �Ϲ� ������(����ȭ ������)�� ��쿡 ������ �߰� �� �ؾ��� ������ ��Ƴ��� �޼����Դϴ�.<br/>
    /// ������ �߰� �� �ش� �������� ������ ������Ʈ�� �ݿ��մϴ�.<br/>
    /// �������ڸ� �����Ͽ� Ư�� ���Կ� �߰��� �� ���θ� ������ �� �ֽ��ϴ�. ���� �ǻ��°� �ʿ��մϴ�.<br/>
    /// </summary>
    protected void AddItemBasic(ItemInfo itemInfo, int slotIndex=-1, bool isActiveTabAll=false)
    {
        if( itemInfo.Item.Type == ItemType.Misc )
            throw new Exception("�Ϲ� �������� �ƴմϴ�. ��ȭ ������ ���� �޼��带 ȣ���ؾ��մϴ�.");

        inventory.AddItem( itemInfo, slotIndex, isActiveTabAll );  // �������� ���� �κ��丮�� �߰��մϴ�.
        itemInfo.OnItemAdded(this);                                // �����ۿ� �ֽ� ������ �ݿ��մϴ�.
    }


    /// <summary>
    /// ��ȭ �������� ��� ������ �߰� �� �ؾ� �� ������ ��Ƴ��� �޼����Դϴ�.<br/>
    /// ���ļ����� ���Ͽ� ���� ������ ����ٸ� ������ �̸��� ��ȭ�������� �ؽ�Ʈ�� ��� ������Ʈ�ϰ�, 
    /// �ش� �������� ������ ������Ʈ�� �ݿ��մϴ�.<br/>
    /// �������ڸ� �����Ͽ� Ư�� ���Կ� �߰��� �� ���θ� ������ �� �ֽ��ϴ�. ���� �ǻ��°� �ʿ��մϴ�.<br/>
    /// </summary>
    protected void AddItemMisc(ItemInfo itemInfo, int slotIndex=-1, bool isActiveTabAll=false)
    {
        if( itemInfo.Item.Type != ItemType.Misc )
            throw new Exception("�ش� �������� ��ȭ�������� �ƴմϴ�.");

        ItemMisc itemMisc = (ItemMisc)itemInfo.Item;

        // �߰� ������ ���� ���
        int beforeOverlapCount = itemMisc.OverlapCount;

        // �������� ���� �κ��丮�� �߰��մϴ�.
        inventory.AddItem( itemInfo, slotIndex, isActiveTabAll );

        // �߰� ������ �������
        int afterOverlapCount = itemMisc.OverlapCount;

        // ���������� ������ �ִٸ�, ���� �����۰� ������ �̸��� �������� ���������� ������Ʈ�մϴ�.
        if( beforeOverlapCount!=afterOverlapCount )
            UpdateTextInfoSameItemName( itemMisc.Name );

        // ������ ������ ������ ���ų�, ���� ���¶��, �����ۿ� �ֽ� ������ �ݿ��մϴ�. 
        if( afterOverlapCount!=0 )                              
            itemInfo.OnItemAdded( this );
    }
     

    /// <summary>
    /// ���ڷ� ������ �̸��� ������ ���� �������� ������ ��� ������Ʈ ���ִ� �޼���<br/>
    /// AddItem���� ���������� ���˴ϴ�. 
    /// </summary>
    /// <param name="itemName"></param>
    protected void UpdateTextInfoSameItemName(string itemName)
    {
        List<GameObject> itemObjList = inventory.GetItemObjectList(itemName);

        foreach(GameObject itemObj in itemObjList )
        {
            itemObj.GetComponent<ItemInfo>().UpdateTextInfo();
        }

    }



    /// <summary>
    /// �κ��丮�� ��Ͽ� ������ �������� *Ư�� ����*�� �߰��մϴ�.<br/>
    /// *** ���ڷ� ���� ������Ʈ �������� null�̰ų�, ���� �ε����� �߸��Ǿ��ٸ� ���ܸ� �߻���ŵ�ϴ�. ***<br/>
    /// </summary>
    /// <returns>������Ʈ�� ���� �� �� ������ �����ϴٸ� false��, ������ ���� ���� �� true�� ��ȯ</returns>
    public bool AddItemToSlot(ItemInfo itemInfo, Transform slotTr, bool isActiveTabAll)
    {
        if( itemInfo==null || slotTr==null )
            throw new Exception( "���� ���� ���� ���� �������� �ʽ��ϴ�." );
                        
        return AddItemToSlot(itemInfo, slotTr.GetSiblingIndex(), isActiveTabAll);
    }



    /// <summary>
    /// �κ��丮�� ��Ͽ� ������ �������� *Ư�� ����*�� �߰��մϴ�.<br/>
    /// *** ���ڷ� ���� ������Ʈ �������� null�̰ų�, ���� �ε����� �߸��Ǿ��ٸ� ���ܸ� �߻���ŵ�ϴ�. ***<br/>
    /// </summary>
    /// <returns>������Ʈ�� ���� �� �� ������ �����ϴٸ� false��, ������ ���� ���� �� true�� ��ȯ</returns>
    public bool AddItemToSlot(ItemInfo itemInfo, int slotIndex, bool isActiveTabAll)
    {
        if( itemInfo==null )
            throw new Exception( "���� ���� ������ ���� ���� �������� �ʽ��ϴ�." );

        // �ش� ���Կ� �ڸ��� ���ٸ�, ���и� ��ȯ
        if( !IsSlotEmpty(itemInfo, slotIndex, isActiveTabAll) )
            return false;
        
        if(itemInfo.Item.Type is ItemType.Misc )
            AddItemMisc(itemInfo, slotIndex, isActiveTabAll);
        else
            AddItemBasic(itemInfo, slotIndex, isActiveTabAll);

        return true;
    }


    //public ItemInfo GetItemInfo(string itemName, bool isLatest=true)
    //{

    //}





    










}
