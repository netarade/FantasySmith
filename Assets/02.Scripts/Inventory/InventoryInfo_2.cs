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
 * <v3.5 - 2024_0123_�ֿ���>
 * 1- UpdateTextInfoSameItemName���� GetItemObjectList�޼��带 ����ϴ� ���� ���ǰ˻� �߰�
 * 
 * <v3.6 - 2024_0124_�ֿ���>
 * 1- InventoryŬ������ ��ųʸ� ���������� GameObject��ݿ��� ItemInfo�� �����ϸ鼭 ���� �޼��� ����
 * (UpdateTextInfoSameItemName)
 * 
 * <v4.0 - 2024_0126_�ֿ���>
 * 1- �κ��丮 �˻� �� ������� �޼��� (SetItemOverlapCount, IsItemEnough, IsSlotEnough, IsSlotEmpty)�� InventoryInfo.cs���� �Űܿ�
 * 
 * 2- �����۸� �ش��ϴ� ������ �˷��ִ� �޼��� HowManyCount�� �ۼ�
 *  (ũ�����ÿ��� ���� ������ �������� ������ ������ ��Ȯ�ϰ� ���ʿ䰡 ����)
 * 
 * <v4.1 - 2024_0127_�ֿ���>
 * 1- ReduceItem�� ItemInfo�� ���� �޴� �����ε� �޼��� ����
 * 
 * <v4.2 - 2024_0220_�ֿ���>
 * 1- AddItemMisc, AddItemToSlot�޼��忡 isAbleToOverlap�ɼ��� �߰��Ͽ� ��ȭ�������̴��� ��ġ�� �ʰ� ������Ʈ�� �״�� �߰��� �� �ֵ��� �Ͽ���
 * 
 * 2- AddItemMisc�� ��ȯ���� void���� bool�� �����Ͽ� ���ڷ� ���� �������� �ı����� ���θ� ��ȯ�ϵ��� ����
 * 
 * 3- IsSlotEmpty, AddItemToSlot, AddItemMisc, AddItemBasic�޼����� ������ ��ü�� ���º����� ���ڷ� �޴� ���� �����ϰ�, Ȱ�� ���� �ε����� ���޹޵��� ����
 * ������ ��ü���ε����� �κ��丮���� ��ü������ ���ͷ�Ƽ�� ��ũ��Ʈ ������ ���� �Ǵ��� �� �ֱ� ����
 * 
 * 4- AddItemToSlot�޼����� Ȱ�� �����ε����� ��Ȱ�� �����ε��� ��θ� ���ڷ� ���޹޴� �����ε� �޼��� �߰�
 * ���� �޼���� Ȱ�������ε����� �޾Ƽ� ������ ��Ȱ������ �ε����� ����� �ε����� �����Ҵ�޾�����,
 * Ư�� ���Կ� �߰��� �� (����Ī�ϴ� �������� �ڸ��ߴ� �ε��� ��θ� �̾�޾ƾ� �ϴ� ���) ������ ��Ȱ������ �ε��� ���� �����ؾ� �ϴ� ��찡 ����� ����
 * 
 * 5- IsSlotEmpty�� Ȱ��ȭ ������ Transform�� ���ڷ� ���޹޴� �����ε� �޼��� �߰�
 * ������ ����� �̷��� �� �ش� Ȱ�� ������ Transform�� �ٷ� �����Ͽ� �߰��� �� ���� �� �Ǵ��ϱ� ����
 * 
 * <v4.3 - 2024_0221_�ֿ���>
 * 1- RemoveItem�� isDestroy�ɼ��� �����ϰ� isUnregister�� ��ü
 * �������� ���ſ� ���ÿ� �ı��ϴ� ��찡 �� ������ �ʰ�, ���޾Ƽ� ȣ���Ͽ� ��ü�����ϱ� �����̸�,
 * isUnregister�ɼ��� 2D���� �״�� ��Ͽ��� �����Ͽ� Ÿ�κ��丮 �̵��� DimensionShift�� �߻���Ű�� �ʱ� ����.
 * (�����ۼ����� ��ũ��Ʈ���� �������� ĵ�����׷��� �����ϴµ� ������ �Ͼ�� ����)
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
    /// �κ��丮�� �����ϴ� �ش� �̸��� *����* ��ȭ ������ ������ ������Ű�ų� ���ҽ�ŵ�ϴ�.<br/>
    /// ���ڷ� �ش� ������ �̸��� ������ �����ؾ� �մϴ�.<br/><br/>
    /// ������ ������ ���ҽ�Ű���� ���� ���ڷ� ������ �����Ͽ��� �ϸ�,<br/>
    /// ���� ������ ���ҷ� ���� 0�̵Ǹ� �������� �κ��丮 ��Ͽ��� ���ŵǸ�, �ı��˴ϴ�.<br/><br/>
    /// ������ ������ ������Ű���� ���� ���ڷ� ����� �����Ͽ��� �ϸ�,<br/>
    /// ������ �ִ� ���� �������� ���� �� �̻� ������ ������Ű�� ���ϴ� ���� ������ �ʰ� ������ ��ȯ�մϴ�.<br/><br/>
    /// �ֽ� ��, ������ ������ ���ҿ��θ� ������ ���ֽ��ϴ�. (�⺻��: �ֽż�)<br/><br/>
    /// ** ������ �̸��� �ش� �κ��丮�� �������� �ʰų�, ��ȭ�������� �ƴ� ��� ���ܸ� �߻���ŵ�ϴ�. **
    /// </summary>
    /// <returns></returns>
    public int SetItemOverlapCount( string itemName, int inCount, bool isLatestModify = true )
    {
        return inventory.SetOverlapCount(itemName, inCount, isLatestModify, null);
    }


    
    /// <summary>
    /// �������� ������ ������� �������� �ش� ���� ��ŭ �κ��丮�� �����ϴ��� ���θ� ��ȯ�մϴ�.<br/>
    /// ������ �̸��� �������� �̷���� ����ü �迭�� ���ڷ� �޽��ϴ�.<br/><br/>
    /// �Ϲ� �������� ������Ʈ�� ������ �ǹ��ϸ�, ��ȭ �������� ��ø������ �ǹ��մϴ�.<br/>   
    /// �ش� ������ŭ ���� �� �ı��ɼ��� ������ �� �ֽ��ϴ�. (�⺻��: ���� 1, ���� ���� �� �ı� ����, �ֽż� ���� �� �ı�)<br/><br/>
    /// *** ���� ���ڰ� 0���϶�� ���ܸ� �߻���ŵ�ϴ�. ***
    /// </summary>
    /// <returns>�������� �����ϸ� ������ ����� ��� true��, �������� �ʰų� ������ ������� �ʴٸ� false�� ��ȯ</returns>
    public bool IsItemEnough( ItemPair[] pairs, bool isReduceAndDestroy = false, bool isLatestModify = true )
    {
        return inventory.IsEnough(pairs, isReduceAndDestroy, isLatestModify);
    }



    
    /// <summary>
    /// �������� ������ ������� �������� �ش� ���� ��ŭ �κ��丮�� �����ϴ��� ���θ� ��ȯ�մϴ�.<br/>
    /// ������ �̸��� ������ ���ڷ� �޽��ϴ�.<br/><br/>
    /// �Ϲ� �������� ������Ʈ�� ������ �ǹ��ϸ�, ��ȭ �������� ��ø������ �ǹ��մϴ�.<br/>   
    /// �ش� ������ŭ ���� �� �ı��ɼ��� ������ �� �ֽ��ϴ�. (�⺻��: ���� 1, ���� ���� �� �ı� ����, �ֽż� ���� �� �ı�)<br/><br/>
    /// *** ���� ���ڰ� 0���϶�� ���ܸ� �߻���ŵ�ϴ�. ***
    /// </summary>
    /// <returns>�������� �����ϸ� ������ ����� ��� true��, �������� �ʰų� ������ ������� �ʴٸ� false�� ��ȯ</returns>
    public bool IsItemEnough( string itemName, int count=1, bool isReduceAndDestroy = false, bool isLatestModify=true )
    {
        return inventory.IsEnough(itemName, count, isReduceAndDestroy, isLatestModify);      
    }






    












    /// <summary>
    /// �� �κ��丮�� ���Կ� �������� ���ϴ� ������ŭ �������� �� �ƹ� ���Կ� �� �� �ڸ��� �ִ��� ���θ� ��ȯ�մϴ�.<br/>
    /// ���ڷ� ������ �̸��� �������ڸ� �����ؾ� �մϴ�. (���������� �⺻���� 1�Դϴ�.)<br/><br/>
    /// 
    /// ����ȭ�������� ��� �������ڸ�ŭ ������Ʈ�� �����ϴ�. (��, �������ڰ� ������Ʈ�� ������ ���մϴ�.)<br/>
    /// ��ȭ �������� ��� �������ڸ� �־ �ִ� ��ø������ �����ϱ� �������� ������Ʈ�� 1���� �����մϴ�. (��, �������ڴ� ��ø������ ���մϴ�.)<br/><br/>
    /// </summary>
    /// <returns>�������� ���������ϴٸ� true��, �Ұ����ϴٸ� false�� ��ȯ�մϴ�.</returns>
    public bool IsSlotEnough(string itemName, int overlapCount=1)
    {
        ItemType itemType = createManager.GetWorldItemType(itemName);

        if( itemType==ItemType.Misc )
            return inventory.IsAbleToAddMisc( itemName, overlapCount ); 
        else
            return inventory.GetCurRemainSlotCount(itemType) >= overlapCount; // ���� ���� ĭ ���� overlapCount�̻�
    }

    /// <summary>
    /// �ش� �������� �ƹ� ���Կ� �� �� �ڸ��� �ִ��� ���θ� ��ȯ�մϴ�.<br/>
    /// �⺻ �������� ��� ���Կ��ο� ���� ����, ���и� ��ȯ�ϸ�,<br/>
    /// ��ȭ �������� ��� ��ø�Ǿ ������ �ʿ���� ��� ���Կ� ���ڸ��� ��� ������ ��ȯ�� �� �ֽ��ϴ�.<br/>
    /// ���� ������ ������ �����ؾ� �մϴ�.
    /// </summary>
    /// <returns>�������� ���������ϴٸ� true��, �Ұ����ϴٸ� false�� ��ȯ�մϴ�.</returns>
    public bool IsSlotEnough(ItemInfo itemInfo)
    {
        ItemType itemType = itemInfo.Item.Type;

        if( itemType == ItemType.Misc )
        {
            ItemMisc itemMisc = (ItemMisc)itemInfo.Item;
            return inventory.IsAbleToAddMisc(itemMisc.Name, itemMisc.OverlapCount);
        }
        else
            return inventory.IsRemainSlotIndirect(itemType);  // �ƹ��ڸ��� ���� ���� ĭ���� �ִ���
    }




    /// <summary>
    /// �ش� �������� ���� ���� Ư�� ���Կ� �� �� ���� �� ���θ� ��ȯ�մϴ�.<br/>
    /// ������ ������ Ȱ��ȭ ������ �ε����� �����ؾ� �մϴ�.
    /// </summary>
    /// <returns>���Կ� ���ڸ��� ���� ��� false��, ���ڸ��� �ִ� ��� true�� ��ȯ</returns>
    public bool IsSlotEmpty(ItemInfo itemInfo, int activeSlotIndex)
    {
        return inventory.IsRemainSlotDirect(itemInfo.Item.Type, activeSlotIndex, interactive.IsActiveTabAll);         
    }

    /// <summary>
    /// �ش� �������� ���� ���� Ư�� ���Կ� �� �� ���� �� ���θ� ��ȯ�մϴ�.<br/>
    /// ������ ������ Ȱ��ȭ ������ Transform �������� �����ؾ� �մϴ�.
    /// </summary>
    /// <returns>���Կ� ���ڸ��� ���� ��� false��, ���ڸ��� �ִ� ��� true�� ��ȯ</returns>
    public bool IsSlotEmpty(ItemInfo itemInfo, Transform activeSlotTr)
    {
        return inventory.IsRemainSlotDirect(itemInfo.Item.Type, activeSlotTr.GetSiblingIndex(), interactive.IsActiveTabAll);
    }
















    
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
    /// ������ �������� ���� �����Ͽ� �ش� �������� ���� ������ŭ ���ҽ�ŵ�ϴ�.<br/><br/>
    /// *** ������ ���� ������ ���� �ʾҰų�, ���� ���� ������ �߸��� ��� ���ܰ� �߻� ***<br/>
    /// *** ��ȭ�������� �ƴ� ��� ���ܰ� �߻� ***
    /// </summary>
    /// <returns>������ ������� ������ false��, ������ ����Ͽ� ���ҿ� �����ϸ� true�� ��ȯ</returns>
    public bool ReduceItem( ItemInfo itemInfo, int overlapCount )
    {
        return inventory.ReduceItem(itemInfo, overlapCount);
    }






    /// <summary>
    /// �ش� �̸��� �������� �κ��丮�� ��Ͽ��� �����Ŀ� ��Ͽ��� ������ �������� ItemInfo �������� ��ȯ�մϴ�.<br/>
    /// �⺻������ �������� 3D���·� ����� ������ ��ȯ�� ������,<br/>
    /// ���� ���ڷ� isUnregister �ɼ��� Ȱ��ȭ��Ű�� 2D ���� �״�� ��ȯ�մϴ�. (Ÿ �κ��丮���� �߰� �� Ȱ���մϴ�) <br/><br/>
    /// *** �κ��丮�� �ش� �̸��� �������� ������ ���ܸ� �߻���ŵ�ϴ�. ***<br/>
    /// </summary>
    public ItemInfo RemoveItem(string itemName, bool isUnregister=false)
    {
        // �κ��丮 ��Ͽ��� �����ϰ� ��ȯ �� �������� �����մϴ�.
        ItemInfo itemInfo = inventory.RemoveItem(itemName);   
        
        if( itemInfo==null )
            throw new Exception( "�ش� �̸��� �������� �������� �ʽ��ϴ�." );       
        
        // ������� �ɼ��� �ɷ����� �ʴٸ�,
        if( !isUnregister )       
            itemInfo.DimensionShift(true);  // �������� 3D���·� ��ȯ�մϴ�.

        // �κ��丮 ������ �����մϴ�.
        itemInfo.UpdateInventoryInfo(null);

        return itemInfo;
    }


    /// <summary>
    /// �ش� �������� �κ��丮�� ��Ͽ��� ���� �����մϴ�.<br/>
    /// �����Ŀ� ��Ͽ��� ������ �������� ItemInfo �������� ��ȯ�Ͽ� �ٸ� �޼��带 ȣ���� �� �ֽ��ϴ�.<br/><br/>
    /// �⺻������ �������� 3D���·� ����� ������ ��ȯ�� ������,<br/>
    /// ���� ���ڷ� isUnregister �ɼ��� Ȱ��ȭ��Ű�� 2D ���� �״�� ��ȯ�մϴ�. (Ÿ �κ��丮���� �߰� �� Ȱ���մϴ�) <br/><br/>
    /// *** ���ڰ� ���޵��� �ʰų�, �κ��丮�� �ش� �̸��� �������� ������ ���ܰ� �߻��մϴ�. ***<br/>
    /// </summary>
    public ItemInfo RemoveItem(ItemInfo itemInfo, bool isUnregister=false)
    {
        if(itemInfo==null)
            throw new Exception("������ ������ ���޵��� �ʾҽ��ϴ�.");
        
        if( inventory.RemoveItem(itemInfo) == null )
            throw new Exception("�ش� �κ��丮�� ������ ������ �������� �ʽ��ϴ�.");
                    
        // ������� �ɼ��� �ɷ����� �ʴٸ�,
        if( !isUnregister )       
            itemInfo.DimensionShift(true);  // �������� 3D���·� ��ȯ�մϴ�.

        // �κ��丮 ������ �����մϴ�.
        itemInfo.UpdateInventoryInfo(null);  
        
        return itemInfo;
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
    /// Ȱ��ȭ �����ε��� �������ڸ� �����Ͽ� Ư�� ���Կ� �߰��� �� ���θ� ������ �� �ֽ��ϴ�. (�⺻��: ����� �ε��� �Ҵ�)<br/>
    /// </summary>
    protected void AddItemBasic(ItemInfo itemInfo, int activeSlotIndex=-1)
    {
        if( itemInfo.Item.Type == ItemType.Misc )
            throw new Exception("�Ϲ� �������� �ƴմϴ�. ��ȭ ������ ���� �޼��带 ȣ���ؾ��մϴ�.");

        inventory.AddItem( itemInfo, activeSlotIndex, interactive.IsActiveTabAll ); // �������� ���� �κ��丮�� �߰��մϴ�.
        itemInfo.OnItemAdded(this);                                                 // �����ۿ� �ֽ� ������ �ݿ��մϴ�.
    }


    /// <summary>
    /// ��ȭ �������� ��� ������ �߰� �� �ؾ� �� ������ ��Ƴ��� �޼����Դϴ�.<br/>
    /// ���ļ����� ���Ͽ� ���� ������ ����ٸ� ������ �̸��� ��ȭ�������� �ؽ�Ʈ�� ��� ������Ʈ�ϰ�, 
    /// �ش� �������� ������ ������Ʈ�� �ݿ��մϴ�.<br/>
    /// Ȱ��ȭ �����ε��� �������ڸ� �����Ͽ� Ư�� ���Կ� ���� �߰��� �� ���θ� ������ �� �ֽ��ϴ�. (�⺻��: ����� �ε��� �Ҵ�)<br/>
    /// ��ȭ �������� ��� ���� �̸��� �����ۿ� ��ġ�� �ʰ� �߰��� �� �ִ� �ɼ��� ���� �����մϴ�. (�⺻��: ���ļ� �߰�)
    /// </summary>
    /// <returns>�������� ��ø�Ǿ� �ı��Ǿ��ٸ� true��, �ı����� �ʾҴٸ� false�� ��ȯ�մϴ�.</returns>
    protected bool AddItemMisc(ItemInfo itemInfo, int activeSlotIndex=-1, bool isAbleToOverlap=true)
    {
        if( itemInfo.Item.Type != ItemType.Misc )
            throw new Exception("�ش� �������� ��ȭ�������� �ƴմϴ�.");

        ItemMisc itemMisc = (ItemMisc)itemInfo.Item;

        // �߰� ������ ���� ���
        int beforeOverlapCount = itemMisc.OverlapCount;

        // �������� ���� �κ��丮�� �߰��մϴ�.
        inventory.AddItem( itemInfo, activeSlotIndex, interactive.IsActiveTabAll, isAbleToOverlap );

        // �߰� ������ �������
        int afterOverlapCount = itemMisc.OverlapCount;

        // ���������� ������ �ִٸ�, ���� �����۰� ������ �̸��� �������� ���������� ������Ʈ�մϴ�.
        if( beforeOverlapCount!=afterOverlapCount )
            UpdateTextInfoSameItemName( itemMisc.Name );

        // ������ ������ (������ ���ų�, ������ �־) ���� ���¶��, 
        if( afterOverlapCount!=0 )
        {            
            itemInfo.OnItemAdded( this );   // �����ۿ� �ֽ� ������ �ݿ��մϴ�. 
            return false;                   // �������� ������ ��ȯ�մϴ�.
        }
        // ������ ������ 0�̶��,
        else
            return true;                    // �������� �ı��� ��ȯ�մϴ�.
    }
     

    /// <summary>
    /// ���ڷ� ������ �̸��� ������ ���� �������� ������ ��� ������Ʈ ���ִ� �޼����Դϴ�<br/>
    /// AddItem���� ���������� ���˴ϴ�. 
    /// </summary>
    /// <param name="itemName"></param>
    protected void UpdateTextInfoSameItemName(string itemName)
    {
        List<ItemInfo> itemInfoList = inventory.GetItemInfoList(itemName);

        if(itemInfoList == null || itemInfoList.Count==0 )
            return;

        foreach(ItemInfo itemInfo in itemInfoList )
            itemInfo.UpdateTextInfo();

    }



    /// <summary>
    /// �κ��丮�� ��Ͽ� ������ �������� *Ȱ��ȭ ����*�� �߰��մϴ�.<br/>
    /// ������ ������ Ȱ��ȭ ������ Transform �������� �����ؾ� �մϴ�.<br/>
    /// ��ȭ �������� ��� �ɼ����� ���ļ� �߰����� ���θ� ������ �� �ֽ��ϴ�. (�⺻��: ��ġ�� �ʰ� �߰�)<br/><br/>
    /// *** ���ڷ� ���� ������Ʈ �������� null�̰ų�, ���� �ε����� �߸��Ǿ��ٸ� ���ܸ� �߻���ŵ�ϴ�. ***<br/>
    /// </summary>
    /// <returns>������Ʈ�� ���� �� �� ������ �����ϴٸ� false��, ������ ���� ���� �� true�� ��ȯ</returns>
    public bool AddItemToSlot(ItemInfo itemInfo, Transform activeSlotTr, bool isAbleToOverlap=false)
    {
        if( itemInfo==null || activeSlotTr==null )
            throw new Exception( "���� ���� ���� ���� �������� �ʽ��ϴ�." );
                        
        return AddItemToSlot(itemInfo, activeSlotTr.GetSiblingIndex(), isAbleToOverlap);
    }



    /// <summary>
    /// �κ��丮�� ��Ͽ� ������ �������� *Ȱ��ȭ ����*�� �߰��մϴ�.<br/>
    /// ������ ������ Ȱ��ȭ ���� �ε����� �����ؾ� �մϴ�.<br/>
    /// ��ȭ �������� ��� �ɼ����� ���ļ� �߰����� ���θ� ������ �� �ֽ��ϴ�. (�⺻��: ��ġ�� �ʰ� �߰�)<br/><br/>
    /// *** ���ڷ� ���� ������Ʈ �������� null�̰ų�, ���� �ε����� �߸��Ǿ��ٸ� ���ܸ� �߻���ŵ�ϴ�. ***<br/>
    /// </summary>
    /// <returns>������Ʈ�� ���� �� �� ������ �����ϴٸ� false��, ������ ���� ���� �� true�� ��ȯ</returns>
    public bool AddItemToSlot(ItemInfo itemInfo, int activeSlotIndex, bool isAbleToOverlap=false)
    {
        if( itemInfo==null )
            throw new Exception( "���� ���� ������ ���� ���� �������� �ʽ��ϴ�." );

        // �ش� ���Կ� �ڸ��� ���ٸ�, ���и� ��ȯ
        if( !IsSlotEmpty(itemInfo, activeSlotIndex) )
            return false;
        
        // ������ ������ ��ȭ���������� Ȯ��
        if(itemInfo.Item.Type == ItemType.Misc )
            AddItemMisc(itemInfo, activeSlotIndex, isAbleToOverlap);
        else
            AddItemBasic(itemInfo, activeSlotIndex);

        

        return true;
    }


    /// <summary>
    /// �κ��丮�� ��Ͽ� ������ �������� *�ٸ� �������� ����ϴ� Ư�� ���Կ� �״��* �߰��մϴ�.<br/>
    /// ������ ������ Ȱ��ȭ ���� �ε����� ��Ȱ��ȭ ���� �ε����� �����ؾ� �մϴ�.<br/>
    /// ��ȭ �������� ��� �ɼ����� ���ļ� �߰����� ���θ� ������ �� �ֽ��ϴ�. (�⺻��: ��ġ�� �ʰ� �߰�)<br/><br/>
    /// *** ���ڷ� ���� ������Ʈ �������� null�̰ų�, ���� �ε����� �߸��Ǿ��ٸ� ���ܸ� �߻���ŵ�ϴ�. ***<br/>
    /// </summary>
    /// <returns>������Ʈ�� ���� �� �� ������ �����ϴٸ� false��, ������ ���� ���� �� true�� ��ȯ</returns>
    public bool AddItemToSlot(ItemInfo itemInfo, int activeSlotIndex, int inactiveSlotIndex, bool isAbleToOverlap=false)
    {
        if( itemInfo==null )
            throw new Exception( "���� ���� ������ ���� ���� �������� �ʽ��ϴ�." );

        // �ش� ���Կ� �ڸ��� ���ٸ�, ���и� ��ȯ
        if( !IsSlotEmpty(itemInfo, activeSlotIndex) )
            return false;

        // ������ ������ ��ȭ���������� Ȯ��
        if( itemInfo.Item.Type == ItemType.Misc )
        {
            // ��ø���¸� �ݿ��Ͽ� ��ȭ�������� �߰��� �����ϰ� �ı��Ǿ��ٸ�,������Ʈ�� �������� �����Ƿ� �ٷ� ������ ��ȯ
            if( AddItemMisc( itemInfo, activeSlotIndex, isAbleToOverlap ) )
                return true;
        }
        else
            AddItemBasic( itemInfo, activeSlotIndex );

        // ������Ʈ�� �����ִ� ��� ������ ���� �ε����� �����մϴ�.
        if(interactive.IsActiveTabAll)
            itemInfo.SlotIndexEach = inactiveSlotIndex;
        else
            itemInfo.SlotIndexAll = inactiveSlotIndex;

        // ������ �߰��� �Ϸ�Ǿ����Ƿ� ������ ��ȯ�մϴ�.
        return true;
    }










        
    /// <summary>
    /// ���ڷ� ���� �� �̸��� �ش� �ϴ� �������� ������ �������� �˷��ݴϴ�.
    /// </summary>
    /// <returns>��ȭ �������� ��쿡�� ��ø ������, ����ȭ �������� ��쿡�� ������Ʈ ������ ��ȯ</returns>
    public int HowManyCount(string itemName)
    {
        return inventory.HowManyCount(itemName);
    }



    










}
