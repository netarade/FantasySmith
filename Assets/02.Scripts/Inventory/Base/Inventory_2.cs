using ItemData;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using WorldItemData;

/*
 * [�۾� ����]  
 * <v1.0 - 2023_1121_�ֿ���>
 * 1- �ʱ� ���� Ŭ���� ����
 * AddItem�޼��� �߰� - CreateManager�� CreateItemToNearstSlot�޼��� ȣ�� ���
 * 
 * <v1.1 -2023_1121_�ֿ���>
 * 1- ����Ŭ������ ����鼭 MonoBehaviour�� ������ �ʾ� new Ű���� ��� �߻��Ͽ� ����.
 * 
 * <v1.2 - 2023_1122_�ֿ���>
 * 1- �κ��丮�� �ε�Ǿ��� �� ������ ������Ʈ�� ����ġ�� ��ų �� �ֵ��� UpdateAllItem�޼��� �߰�
 * 
 * <v2.0 - 2023_1226_�ֿ���>
 * 1- UpdateAllItem�޼��带 UpdateAllItemInfo�޼���� ����
 * 2- LoadAllItem �ּ�ó�� 
 * - ���� ������ ����ȭ���� �� �� �ҷ������ٸ� �ش� �޼��� ���� ����
 * 
 * <v2.1 - 2023_1227_�ֿ���>
 * 1- Item ������ �������� �������� ����(3D ������Ʈ ���� 2D������Ʈ) 
 * UpdateAllItemInfo�޼����� ItemInfo�� �����ϴ� GetComponent�޼��带 GetComponentInChildren���� ����
 * 
 * <v2.2 - 2023_1229_�ֿ���>
 * 1- ���ӽ����̽��� CraftData���� InventoryManagement�� ����
 * 2- ���ϸ� ���� Inventory_p2-> Inventory_2
 * 
 * <v2.3 - 2023_1230_�ֿ���>
 * 1- CreateItem�޼��� �߰�
 * AddItem�޼����� ������ �ű�.
 * 
 * <v3.0 - 2023_1231_�ֿ���>
 * 1- CreateItem�޼��� ���� �ϰ� AddItem�޼��忡�� ItemInfo �Ǵ� GameObject�� ���ڷ� ���޹޾Ƽ� �ش� ��ųʸ��� �ֵ��� ����
 * 
 * 2- AddItem, RemoveItem, SetOverlapCount ���ں� �����ε� ����
 * 
 * 3- �پ��� ������ ���� �޼��� ����
 * IsContainsItemName - ������ �̸����� �ش� �κ��丮�� �����ϴ��� 
 * 
 * GetItemDicInExists - ���� �κ��丮 ��Ͽ� �̹� �����ϴ� �������� ���� ��ųʸ��� ��ȯ
 * GetItemDicIgnoreExists - ���� �κ��丮�� ��Ͽ� �������� �ʴ� �������� ���� ��ųʸ��� ��ȯ
 * 
 * GetItemTypeInExists - ��ųʸ��� �����ϴ� �������� Ÿ�Թ�ȯ
 * GetItemTypeIgnoreExists - ���� ��ųʸ��� �����ϴ� �������� Ÿ�� ��ȯ
 * GetItemObjectList - ��ųʸ����� ������Ʈ ����Ʈ�� ��ȯ
 * 
 * 
 * <v3.1 - 2024_0102_�ֿ���>
 * 1- AddItem�޼��� string, ItemŬ���� ��� �����ε� �޼��� ����
 * ������ �̸� �����Ǿ��ִ� ������Ʈ�� �ƴ� ��� ������ ���� ������Ʈ �޼��� ȣ���� ����ϱ� ������
 * �ᱹ InventoryInfo�޼��忡�� �ϼ��ؾ� �� ����̹Ƿ� �̹� �����Ǿ��ִ� ������Ʈ�� ��ũ��Ʈ�� �ִ� �޼��常 �ִ°��� ���ٰ� �Ǵ�.
 * 
 * (Remove�޼����� �����ε��� ���δ� ������ string������� �˻��� �����̱� ������, ������ �����ε��� InfoŬ�������� �״�� ���� ����)
 * 
 * 2- AddItem GameObject���ڸ� �޴� �޼��� ����ó�� �ۼ�
 * 
 * 3- SetOverlapCount�޼��� ���� �� ���� ��� Inventory_3.cs�� ����
 * 
 * 4- AddItem �� RemoveItem�� ����ó�� ���� �߰�
 * 5- RemoveItem GameObject�� ItemInfo ���� �޼��带 Item���� �޼��� ȣ�⿡�� �ٷ� string �޼���� ȣ��ǵ��� ���� 
 * 
 * <v3.2 - 2024_0102_�ֿ���>
 * 1- ItemType enum�� int������ ��ȯ�޴� �޼����� GetItemTypeIndexIgnoreExists �޼��带 �߰�
 * 
 * 
 * 
 * 
 * [�߰� ����]
 * 1- AddItem �޼���� �������� ������ ������ ������Ʈ�� �ܺ� InventoryInfo��ũ��Ʈ���� ����� ��.
 * (������ ������Ʈ�� ����Ȱ��ȭ �Ǳ������� �س��� �Ѵ�.)
 * 
 * 2- AddItem �޼���� �κ��丮�� ���� ��, ���� ���� �ε����� ��ü ���� �ε����� �Ѵ� ã�Ƽ� �־���� �Ѵ�.
 * (CreateManager�� �����ʿ�)
 * 
 * 
 * <v4.0 - 2024_0103_�ֿ���>
 * 1- RemoveItem�޼����� ��ȯ���� bool�� �ƴ϶� GameObject�� ����
 * ������ InventoryŬ���������� ������Ʈ�� �ı� ó���� ���ϱ� ������ InfoŬ������ �������� �Ѱܾ� �ϱ� ����
 * 
 * 2- GameObject, Item Ŭ������ ���ڷ� �޴� �����ε� �޼��� ����
 * �ڵ尡 �������� ������� ������ �ʿ� �� �߰��� ����
 * 
 * 3- AddItem�޼��� �ּ� ����
 * 
 * <v4.1 - 2024_0103_�ֿ���>
 * 1- RemoveItem���� ItemInfo�� �޴� �޼���, isLatest�� �޴� ���ڸ� �����ϰ�, �˻� ���� ����� �ƴ� �������� ������� ����
 * ������ ���� �������� �ƹ����̳� �����ϴ� ���� �ƴ϶� �������� ������ ������ �� Ư���������� ��Ȯ�� ��Ͽ��� �����ؾ� �ϴ� ��찡 ����� ���� 
 * 
 * 2- GetItemObjectList�޼��� ���ο� isNewIfNotExist ���� �ɼ� �߰�
 * ������ ������Ʈ����Ʈ�� �κ��丮 ���� ������ �������� �ʾƵ� ���Ӱ� ���� �� �߰��Ͽ� ��ȯ�� �����ϵ��� ���� 
 * 
 * 3- UpdateAllItemInfo �޼��带 InventoryInfoŬ������ �ű�.
 * InventoryInfo Ŭ�������� �κ��丮 �ε�� ����Ͽ�����, ����ó������ �ܺ�ó���� ������
 * �κ��丮�� �����ϴ� ��� �������� ������� OnItemCreated�޼��带 ȣ���Ͽ��� �ϳ� �������ڷ� InventoryInfo ������ �����ؾ� �ϱ� ����
 * 
 * 4- GetItemMaxOverlapCount�޼��� ����
 * ���� ���������κ��� ������ �޾Ƽ� ������ �� ������ �ִ� ��ø��� ������ ��ȯ�ϴ� �뵵 
 * (�ű� ������ �߰� �� ��������� ������Ʈ�� �߰��ؾ��ϴ� �� �˱� ���� ������Ʈ�� �ƿ� ���� ��쿡 ������ ���ϰ� �ǹǷ� �ʿ�)
 * 
 * 
 * <v4.2 - 2024_0104_�ֿ���>
 * 1- AddItemToDic ���ο� SetCurItemObjCount�޼��幮�� ����
 * 
 * <v4.3 - 2024_0105_�ֿ���>
 * 1- GetItemTypeIndexIgnoreExists�޼��� 
 * createManager���� �޼��带 �ҷ����� ItemType.None���� ��ȯ�Ǿ� ����ó�� �ȵǰ� ���� ��ȯ�Ǵ� �� ����
 * 
 * 2- GetItemDicIgnoreExsists �̸� ���ڸ� �޴� �����ε� �޼���
 * ���ܸ� ������ �κп��� null ��ȯ���� ����
 * 
 * 3- ��Ÿ string �̸��� ���ڷ� �޴� �޼���鿡�Լ� �̸��� ��ġ���� ������ null�̳� None��ȯ�Ͽ��µ�, ���ܸ� �������� ����
 * 
 * 4- AddItemToDic�޼��带 private���� ����
 * 
 * <v4.4 - 2024_0110_�ֿ���>
 * 1- RemoveItem�޼��忡�� itemObjList�� null�϶��� ���ǰ˻繮 �߰� 
 * (��ųʸ� null�δ� ������ ������ �������� ��� �ݺ�ȣ�Ⱑ�ɼ��� ����)
 * 
 * 2- RemoveItem�޼��忡�� itemObjList�� itemObjList.Count==0�϶��� ���� �˻繮 �߰�
 * ���� ������Ʈ����Ʈ�� count�� 0�̵Ǿ �ѹ� �����Ǹ� ���� ���·� ����
 * (�̴� 1��¥�� ������Ʈ�� ������ ������ �ݺ��Ǵ� ��찡 �ֱ� ������ ����Ʈ�� ������ ������ �ʱ� ����)
 * 
 * <V5.0 -2024_0111_�ֿ���>
 * 1- ����Ʈ ������ Ŭ������ �߰��ϸ鼭 ���� �޼��带 ����
 * 
 * <v5.1 -2024_0111_�ֿ���>
 * 1- �κ��丮�� ��ųʸ� �迭�� ��ȯ�ϸ鼭 ���� �޼��� �����ϰ� ����ȭ
 * 
 * <v5.2 - 2024_0114_�ֿ���>
 * 1- GetDicIndex�޼��� �߰�
 * DicType�� ��ġ�ϴ� �ε����� ���Ͽ� slotCountLimit �迭�� �����ϱ� ����
 * 
 * 2- GetSlotCountLimit �޼��� �߰�
 * ItemType�� ���ڷ� �־� �ش� ������ slotCountLimit�� ������ ���ϱ� ����
 * 
 * <v5.3 - 2024_0115_�ֿ���>
 * 1- InventoryInfoŬ�������� SetItemSlotIndexBothLatest�޼��带 �Űܿ�
 * ������ ���� �ε����� InventoryŬ�������� �ۿ� ���� �� ������, 
 * AddItem�ÿ� �� �������� �̷������ �������谡 �������� �ʱ� ����
 * 
 * 2- GetSlotCountLimit�޼��� ����
 * Inventory.cs�� GetSlotCountLimitDic�� GetSlotCountLimitTab���� �����Ͽ� ���Ӱ� �ۼ�
 * 
 * <v6.0 - 2024_0115_�ֿ���>
 * 1- SetItemSlotIndexBothLatest�޼������ SetItemSlotIndexNearst�� ����
 * SetItemSlotIndex�޼������ SetItemSlotIndexCertain�� ������ �ۼ��Ϸ�
 * (�ֽŽ��� �ε����� �������ִ� �޼���� ���������� �������ִ� �޼���� ����)
 * 
 * 2- AddItem�޼��忡 �������Կ� �߰��ϴ� �������� �߰�
 * 
 * 3- SetItemSlotIndexNearst�޼��� ������ GetItemSlotIndexNearst�޼��� �Ű������� �����Ͽ� ȣ��
 * 
 * 4- SetItemSlotIndexCertain�޼��� ������ GetItemSlotIndexNearst�޼��� �Ű����� �����Ͽ� ȣ��
 * 
 * 5- �޼���� ���� SetItemSlotIndexNearst, SetItemSlotIndexCertain -> SetItemSlotIndexIndirect, SetItemSlotIndexDirect
 * 
 * 6- SetItemSlotIndexIndirect�޼��忡�� IsRemainSlotIndirect ���ǿ� !�� �Ⱥٿ��� �� ����
 * 
 * 7- GetItemDicIgnoreExsists�޼��� �ּ� ����
 * (������ ������ �ش��ϴ� ������ ���� ���, ȣ�� �� ���ܰ� �߻��ϴ� �޼����̹Ƿ�)
 * 
 * 8- GetItemDicIgnoreExists�޼������ GetItemDic���� �����Ͽ�����, GetItemDicInExists�޼���, IsContainsItemName�޼��带 �����Ͽ���.
 * ������ ���� ������ ������ ��ųʸ��� �����ϴ� ������� ����Ǿ��� ������ ���� ��ųʸ��� �����ͼ��� �ȵǰ�,
 * GetItemDicInExists�޼����� ���� ��ųʸ��� �������� ����־�߸� ��ȯ�� ������ �޼���� ������ �ʰ� GetItemObjList�� �� ���̾��Եȴ�.
 * �������� ������ IsContainsItemName�޼��嵵 ��ųʸ��� ������ �̸��� �ִ��� �˻��ϴ� �ͺ��� �ٷ� ObjList�� ��ȯ�޴� ���� ������.
 * 
 * 9- GetItemDic���� ��ųʸ��� ã�� ���� ��� ���ܸ� ������ �κп��� null���� ��ȯ�ϵ��� �����Ͽ���. (�ش�κ� �ּ�����)
 * ������ Serialize�޼��忡�� �ݵ�� ȣ����Ѿ� �ϱ� ����
 * 
 * <v6.1 - 2024_0122_�ֿ���>
 * 1- GetItemTypeInExists�޼��带 ����
 * 
 * �̸��� ������� Ÿ���� ã�� ���� InExists�� �ʿ���µ� �̴�
 * �������� Add�� �� ���� �������� �ִ��� ���ΰ� �߿����� ������,
 * �ش� �̸��� ������ ������ ���� ���ǰ˻繮�� ���� ��찡 ���� ����.
 * 
 * 2- GetItemDic�޼��带 Inventory.cs�� GetDicIndex�޼��带 �̿��ϴ� ���·� ����
 * (������������ ���̱� ����)
 *
 * 3- GetItemObjectList�޼����� �ڵ带 �ٷ� ��ųʸ��� �̸����� �����ؼ� �˻� �� ��ȯ�ϴ� ��Ŀ��� 
 * �̸��� ������� GetDicIndex�� ���� ���ؼ� �˻��ϴ� ������� ����
 * 
 * (������ �ش��̸��� ������ ������ �ƿ� ���� �� ���ܸ� ���� �Ǽ��� ���� �� �� �ֱ� ����,
 * ���� �����ϱ� �ɼ��� �ڵ忡���� ������ �� �ֱ� ����)
 * 
 * 4- RemoveItem�� string itemName�� ���ڷ� �޴� �޼��忡��
 * itemObjList�� null�� �� �����Ͽ� null�� ��ȯ�ϴ� ���� ����ó���ϵ��� ����
 * ItemInfo�����ε� �޼����� ����ó���� ��ġ��Ŵ
 * (������ IsItemEnough�� �˻����� �ʰ� RemoveItemȣ���� �� �ٸ� �޼��带 �����ϴ� ���� �����ϱ� ����)
 * 
 * <v7.0 - 2024_0124_�ֿ���>
 * 1- ��ųʸ� ���������� GameObject��ݿ��� ItemInfo�� �����ϸ鼭 ���� �޼��� ����
 *(AddItemToDic, FillExistItemOverlapCount, RemoveItem(ItemInfo), RemoveItem(string), 
 *GetItemDic(ItemType), GetItemDic(string), GetItemInfoList )
 * 
 * <v7.1 - 2024_0126_�ֿ���>
 * 1- ��ƿ��Ƽ ���� ������ �޼��带 Invetory_4.cs�� �̵�
 * (GetItemTypeIgnoreExists GetItemDic GetItemMaxOverlapCount GetItemInfoList)
 * 
 * 2- �����۸� �ش��ϴ� ������ �˷��ִ� �޼��� HowManyCount�� �ۼ�
 * (ũ�����ÿ��� ���� ������ �������� ������ ������ ��Ȯ�ϰ� ���ʿ䰡 ����)
 * 
 * <v7.2 - 2024_0127_�ֿ���>
 * 1- ReduceItem�� ItemInfo�� ���� �޴� �����ε� �޼��� ����
 * 
 * 2- RemoveItem ItemInfo�� �޴� �޼��忡�� CalculateItemObjCount�� ȣ������ �ʰ� �ִ� �� ����
 * 
 * 
 * <v7.3 - 2024_0128_�ֿ���>
 * 1- AddItemMisc�޼��忡�� GetCurRemainSlotCount�� ���� �˻��ϰ� �ִ� �κ��� �����ϰ� IsAbleToAddMisc���� ���ǰ˻��ϵ��� ����
 * ��ȭ�������� ��� ������ ��� ��ø�� �� �ִٸ� �� �� �־�� �ϹǷ�
 * 
 * 
 * <v7.4 - 2024_0129_�ֿ���>
 * 1- �κ��丮�� isShareAll(��ü ���� ����) �ɼǿ� ���� �ε����� �Ҵ��ϵ��� ���� 
 * SetItemSlotIndexDirect�޼��� - �����ɼ��� �� (������ ������ �ƹ� �ε����� ���� �ʰ�,) ��ü �����ε����� �������� �ε����� ���� ���ڷ� ��ġ��Ŵ
 * SetItemSlotIndexIndirect�޼��� - �����ɼ��� �� (��ü�� �ε���, ������ �ε��� ���� 2�� ������ �ʰ�) ��ü�� �ε��� �ѹ��� ���Ͽ� ������ �ε����� ��ġ��Ŵ
 * 
 * 2- AddItem�� �׹�° ���� �ɼ����� IsAbleToOverlap �ɼ��� �⺻������ true�� �����Ͽ���.
 * �̴� ��ø�������� ��� ���� �Ҹ�� ���� ��ø�� ������ �����̸�, 
 * ���̺��� �� ��ø���� �ʰ� �����س��ٰ�, �ε��� �� ������ ��ø�Ǿ� ������Ʈ�� ������� ó�� ���̴� �̽��� �ֱ� ����.
 * �̸� �ذ��ϱ� ���� DeserializeItemListToDic�޼��忡�� AddItem�� ��ȭ�������̶� ��ø��Ű�� �ʰ� ������ ��ġ�� �ٷ� �߰��ϵ��� �Ͽ���.
 * 
 * <v7.5 - 2024_0131_�ֿ���>
 * 1- AddItemDirectly�޼��带 ����� ItemInfo�� �����ϸ� �ش� ������� ������ �ٷ� �߰��� �� �ְ� �Ͽ���.
 * �̴� �ε� �� �������� �ش� ������� �״�� �߰��ؾ� �ϱ� ����
 * 
 */

namespace InventoryManagement
{    
    public partial class Inventory
    {
        
        /// <summary>
        /// �ش� �������� �ε��� ������ ���� ����� �ֽŽ������� �������ִ� �޼����Դϴ�. <br/>
        /// Inventory�� AddItem���� �������� �߰��ϱ� �� ���������� �Է� �ޱ����� ���˴ϴ�.<br/>
        /// *** ���Կ� ���ڸ��� ���ų�, ������ ������ ���ٸ� ���ܸ� �߻���ŵ�ϴ�. ***
        /// </summary>
        public void SetItemSlotIndexIndirect( ItemInfo itemInfo )
        {        
            // ���� ���� ���� ó��
            if( itemInfo==null )
                throw new Exception("ItemInfo ��ũ��Ʈ�� �������� �ʴ� �������Դϴ�.");
            
            // ������ ������ ������ �����մϴ�.
            Item item = itemInfo.Item;
            ItemType itemType = item.Type;

            // ���Կ� ������� ���� ��
            if( !IsRemainSlotIndirect(itemType) )     
                throw new Exception("�������� �� �ڸ��� ������� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");


            // ��ü ���� ���� �ɼ��� Ȱ��ȭ�Ǿ� �ִٸ�,
            if( isShareAll )
            {
                // ����Ʈ �������� ��� ���� ���� �ε����� �Ҵ��մϴ�.
                if( itemType==ItemType.Quest )
                    item.SlotIndexEach=GetItemSlotIndexIndirect( itemType, false );
                // ����Ʈ �������� �ƴѰ�� ��ü ���� �ε����� �Ҵ��� ����, ���� �����ε����� ��ġ��ŵ�ϴ�.
                else
                {
                    item.SlotIndexAll=GetItemSlotIndexIndirect( itemType, true );
                    item.SlotIndexEach=item.SlotIndexAll;
                }
            }
            // ��ü ���� ���� �ɼ��� ��Ȱ��ȭ�Ǿ� �ִٸ�,
            else
            {
                // ���� ���� �ε��� ������ �Է��մϴ�.
                item.SlotIndexEach=GetItemSlotIndexIndirect( itemType, false );

                // (����Ʈ���� �ƴ� ���) ��ü ���� �ε��� ������ �Է��մϴ�.
                if( itemType!=ItemType.Quest )
                    item.SlotIndexAll=GetItemSlotIndexIndirect( itemType, true );
            }
        }
        

        /// <summary>
        /// �ش� �������� �ε��� ������ ���� ���� �ε����� �������ִ� �޼����Դϴ�.<br/>
        /// ���ڷ� �������� ������, � ������ �ε�������, ��ü���� Ȱ��ȭ�Ǿ��ִ��� ���θ� ���޹޽��ϴ�.<br/>
        /// *** ���Կ� ���ڸ��� ���ų�, ������ ������ ���ٸ� ���ܸ� �߻���ŵ�ϴ�. ***
        /// </summary>
        public void SetItemSlotIndexDirect(ItemInfo itemInfo, int slotIndex, bool isActiveTabAll)
        {
            // ���� ���� ���� ó��
            if( itemInfo==null )
                throw new Exception("ItemInfo ��ũ��Ʈ�� �������� �ʴ� �������Դϴ�.");
                        
            // ������ ������ ������ �����մϴ�.
            Item item = itemInfo.Item;
            ItemType itemType = item.Type;

            // Ư�� ���Կ� ������� ���ٸ� ���ܸ� �߻���ŵ�ϴ�
            if( !IsRemainSlotDirect(itemType, slotIndex, isActiveTabAll) )
                throw new Exception("���Կ� �ڸ��� ������� �ʽ��ϴ�.");

            // ��ü �� ���� �ɼ��� Ȱ��ȭ �Ǿ� �ִ� ���
            if( isShareAll )
            {                
                item.SlotIndexAll = slotIndex;      // ��ü ���� �ε��� �� ���� ���� �ε����� ���ڷ� ��ġ��ŵ�ϴ�.
                item.SlotIndexEach = slotIndex;
            }
            // ��ü �� ���� �ɼ��� ��Ȱ��ȭ �Ǿ��ִ� ���
            else
            {
                // ��ü ���� Ȱ��ȭ�Ǿ� �ִ� ���
                if( isActiveTabAll )
                {
                    item.SlotIndexAll=slotIndex;                                  // ��ü ���� �ε����� ���� ����
                    item.SlotIndexEach=GetItemSlotIndexIndirect( itemType, false ); // ���� ���� �ε����� ����� �ƹ��� ����
                }
                // ���� ���� Ȱ��ȭ�Ǿ� �ִ� ���
                else
                {
                    item.SlotIndexAll=GetItemSlotIndexIndirect( itemType, true );   // ��ü ���� �ε����� ����� �ƹ��� ����
                    item.SlotIndexEach=slotIndex;                                 // ���� ���� �ε����� ���� ����
                }
            }
        }







        /// <summary>
        /// ItemInfo ������Ʈ�� ���ڷ� ���޹޾� �ش� ������ ������Ʈ�� �ش��ϴ� ������ ã�Ƽ� 
        /// �������� �־��ִ� �޼����Դϴ�.<br/>
        /// ���� ���ڷ� Ư�� ���� �ε����� �����ǻ��¸� �����ϸ�,
        /// �ֽ� �����ε����� �ƴ϶� ���� ���� �ε����� �������� �߰��մϴ�.<br/><br/>
        /// <br/>
        /// *** ���ڷ� ���� ������Ʈ �������� ���ٸ� ���ܸ� �߻���ŵ�ϴ�. ***<br/>
        /// </summary>
        /// <returns>������ �߰� ������ true��, ���� �κ��丮�� �� ������ �����ϴٸ� false�� ��ȯ�մϴ�</returns>
        public bool AddItem(ItemInfo itemInfo, int slotIndex=-1, bool isActiveTabAll=false, bool isAbleToOverlap=true)
        {   
            if(itemInfo==null)
                throw new Exception("������ ��ũ��Ʈ�� �������� �ʴ� ������Ʈ�Դϴ�. Ȯ���Ͽ� �ּ���.");

            // ������ Ÿ���� Ȯ���մϴ�.
            ItemType itemType = itemInfo.Item.Type;

            // ��ȭ �����۰� ����ȭ ���������� �����Ͽ� �޼��带 ȣ���մϴ�.
            switch(itemType)
            {
                // Ÿ���� �Һи��� ��� ����ó��
                case ItemType.None:
                    throw new Exception("�ش� �������� ������ ��Ȯ���� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");

                case ItemType.Misc:
                    if(isAbleToOverlap)
                        return AddItemMisc(itemInfo, false ,slotIndex, isActiveTabAll);   // ������ �߰� �޼��带 ȣ���մϴ�.
                    else
                        return AddItemToDic(itemInfo, slotIndex, isActiveTabAll);

                default:
                    return AddItemToDic(itemInfo, slotIndex, isActiveTabAll);           // �ٷ� ��ųʸ��� �߰��մϴ�.
            }            
        }
                
  
        /// <summary>
        /// �߰� �� ItemInfo ������Ʈ�� ���ڷ� ���޹޾� �ش� ������ ������ ������Ʈ�� �־��ִ� �޼����Դϴ�.<br/>
        /// �ش� ������ ������ 1���� �� �� ������ �־�� �մϴ�.<br/><br/>
        /// ���� ���ڷ� Ư�� ���� �ε����� �����ǻ��¸� �����ϸ�,
        /// �ֽ� �����ε����� �ƴ϶� ���� ���� �ε����� �������� �߰��մϴ�.<br/><br/>
        /// </summary>
        /// <returns>�ش� ������ ������ �������� �� �� ������ ���ٸ� false, ������ �߰��� ���� �� true�� ��ȯ�մϴ�.</returns>
        private bool AddItemToDic(ItemInfo itemInfo, int slotIndex=-1, bool isActiveTabAll=false)
        {          
            // ������ �̸��� ������ �����մϴ�.
            string itemName = itemInfo.Item.Name;
            ItemType itemType = itemInfo.Item.Type;
                        
            // ���� �ε��� ���� ������ ���޵Ǿ��ٸ�,(���ڸ� ���� ���� �ʾҴٸ�)
            if( slotIndex<0 )
            {
                // ��ó�� ���� ������ ���ٸ� ���и� ��ȯ�մϴ�.
                if( !IsRemainSlotIndirect(itemType) )            
                    return false;
                
                // ������ ������ ���� ����� ������ �ε����� �����մϴ�.
                SetItemSlotIndexIndirect( itemInfo );
            }         
            // ���� �ε����� ���� ����� ���޵Ǿ��ٸ�
            else
            {             
                // ���� ���Կ� ���ڸ��� ���ٸ� ���и� ��ȯ�մϴ�.
                if( !IsRemainSlotDirect( itemType, slotIndex, isActiveTabAll ) )
                    return false;

                // ������ ������ ���� ���� �ε����� �����մϴ�.
                SetItemSlotIndexDirect(itemInfo, slotIndex, isActiveTabAll);
            }
                        
            // ���ǰ˻翡 ����Ͽ����Ƿ� �������� ������ �ٷ� �߰��մϴ�.
            AddItemDirectly(itemInfo);         

            // ������ ��ȯ�մϴ�.
            return true;                                    
        }


        /// <summary>
        /// �������� ������ ��� ���� �˻絵 ���� �ʰ� �ٷ� �߰��մϴ�.
        /// </summary>
        public void AddItemDirectly(ItemInfo itemInfo)
        {            
            // ������ �̸��� ������ �����մϴ�.
            string itemName = itemInfo.Item.Name;
            ItemType itemType = itemInfo.Item.Type;

            // ������ �̸��� ������� �ش� ������ ItemInfo ����Ʈ�� �����մϴ�. (�������� �ʴ´ٸ� ���Ӱ� �����Ͽ� ��ȯ�մϴ�.)
            List<ItemInfo> itemInfoList = GetItemInfoList(itemName, true);

            // �ش� ������ ������ �߰��մϴ�.
            itemInfoList.Add(itemInfo);          
                  
            // �ش� ������ ������ ���� ������Ʈ�� ������ ������ŵ�ϴ�.
            CalculateItemObjCount(itemType, 1);
        }





        

        /// <summary>
        /// ��ȭ ������ ������Ʈ�� ������ �κ��丮�� �߰��Ͽ� �ݴϴ�.<br/>
        /// ������ ������ ��ȭ�������� �ִ� ������ ä������ �ʾҴٸ�, ���� �������� ������ ���ҽ��� ���� �������� ������ ä��ϴ�.
        /// <br/><br/>
        /// 
        /// ���� �������� ������ 0�� �Ǿ��ٸ� ������Ʈ�� �ı��ϰ�,<br/> 
        /// 0�̵��� �ʾҴٸ� �ش� �������� ���Ӱ� �κ��丮 ���Կ� �߰��մϴ�.<br/><br/>
        /// �⺻������ ���Կ��� �ռ� ������ ä������ �̸� ������ �� �ֽ��ϴ�. (�⺻��: �����ȼ�)<br/><br/>
        /// 
        /// ���� ���ڷ� Ư�� ���� �ε����� �����ǻ��¸� �����ϸ�,
        /// �ֽ� �����ε����� �ƴ϶� ���� ���� �ε����� �������� �߰��մϴ�.<br/><br/>
        /// </summary>
        /// <returns>��ȭ �������� �� ������ ���ٸ� false��, ������ �߰��� ������ ��� true�� ��ȯ�մϴ�.</returns>
        private bool AddItemMisc(ItemInfo itemInfo, bool isLatestModify=false, int slotIndex=-1, bool isActiveTabAll=false)
        {
            // �������� �� ������ ���� ��� ���и� ��ȯ�մϴ�.
            if( !IsAbleToAddMisc(itemInfo.Name, itemInfo.OverlapCount) )
                return false;

            // ������ ������ �����Ͽ� ���������ۿ� ä��⸦ �����ϰ�, ���� ������ ��ȯ�޽��ϴ�.
            int afterCount = FillExistItemOverlapCount(itemInfo, isLatestModify);

            // ����ä��Ⱑ ����� �� 
            if( afterCount==0 )
                GameObject.Destroy( itemInfo.gameObject );      // ���� ������ 0�� �Ǿ��ٸ�, ���ڷ� ���޹��� ������Ʈ ����
            else
                AddItemToDic(itemInfo, slotIndex, isActiveTabAll);  // ���� ������ �����Ѵٸ�, �ش� �������� ������ �߰�

            return true;
        }

        







        /// <summary>
        /// ��ȭ �������� �κ��丮�� �߰��ϱ� ����,<br/>
        /// ������ �̸��� ���� ��ȭ �����ۿ� ������ ä���ְ�, ���� ������ ��ȯ�մϴ�.<br/>
        /// ���� ��ȭ�������� ���ٸ� ä���� ���ϰ� ���� ������ �״�� ��ȯ�մϴ�.<br/><br/>
        /// ���ڷ� ������ ItemInfo�� ������ ä�ŭ ���ҵǾ� ������,<br/>
        /// ������ 0�� �ɶ� ���� ä������ ������ ������ ���� �ʽ��ϴ�.<br/><br/>
        /// ���� ���� �� �ռ� �����ۺ��� ä���, ������ �� �ֽ��ϴ�.(�⺻��: �����ȼ�)<br/><br/>
        /// *** ������ �������� ��ȭ�������� �ƴ� ��� ���ܸ� �߻���ŵ�ϴ�. ***<br/>
        /// </summary>
        /// <returns>���� �����ۿ� ��� ������ �� ä�� ��� 0�� ��ȯ, �� ä���� ���� ��� ���� ������ ��ȯ</returns>
        public int FillExistItemOverlapCount(ItemInfo itemInfo, bool isLatestModify=false)
        {            
            if(itemInfo.Item.Type != ItemType.Misc)
                throw new Exception("���� ���� �������� ��ȭ �������� �ƴմϴ�. Ȯ���Ͽ� �ּ���.");

            ItemMisc newItemMisc = (ItemMisc)itemInfo.Item;

            // ������Ʈ ����Ʈ ������ �����մϴ�.
            List<ItemInfo> itemInfoList = GetItemInfoList( newItemMisc.Name );                       

            // ���� �����ۿ� ����ä��� ���� �� �ĸ� ���� ������ ���� �������� �������� �����մϴ�.
            int beforeCount = newItemMisc.OverlapCount; 
            int afterCount = newItemMisc.OverlapCount;

            // ���� ������Ʈ ����Ʈ�� �ִ� ����
            if( itemInfoList!=null )
            { 
                // �������� ������ �����մϴ�.
                itemInfoList.Sort(CompareBySlotIndexEach);

                // ���� ������ ���� �� �������� ���� ���ؿ� ���� �ϳ��� ������ ���� �����ۿ� ����ä��⸦ �����մϴ�.
                if( isLatestModify )
                {
                    for( int i = itemInfoList.Count-1; i>=0; i-- )
                        if( FillCountInLoopByOrder(i) ) { break; } // ���������� true�� ��ȯ�ϸ� ���������ϴ�.                                     
                }
                else
                {
                    for( int i = 0; i<=itemInfoList.Count-1; i++ )   
                        if( FillCountInLoopByOrder(i) ) { break; } // ���������� true�� ��ȯ�ϸ� ���������ϴ�.  
                }
            }

            // ä���ְ� ���� ������ ��ȯ�մϴ�.
            return afterCount;

            



            // ���� �����ۿ� ���� ������ �ٸ��� �Ͽ� ä��� ���� �ݺ����� ���� �޼����Դϴ�.
            bool FillCountInLoopByOrder(int idx)
            {
                // ���� �����ۿ� �ε����� ���� ������ �Ͽ� ������ �ҷ��ɴϴ�.
                ItemMisc oldItemMisc = (ItemMisc)itemInfoList[idx].GetComponent<ItemInfo>().Item;
                    
                // ���� �����ۿ� ���� ���������� �ִ� ���ġ���� ä��� ���� ������ ��ȯ�޽��ϴ�.
                afterCount = oldItemMisc.AccumulateOverlapCount(beforeCount);

                // �پ�� ������ŭ �űԾ������� ������ ���ҽ��� �ݴϴ�.
                newItemMisc.AccumulateOverlapCount(beforeCount-afterCount);
                    
                // ���� �ݺ����� ����ϱ� ���Ͽ� ��������� ��ġ���� �ݴϴ�. 
                beforeCount = afterCount;


                // ���� ������ 0�̵Ǹ� �ܺ� ȣ�� ������ Ȱ��ȭ�մϴ�.
                if(afterCount==0)
                    return true;
                else
                    return false;
            }
        }





        
        
        /// <summary>
        /// Ư�� �������� ItemInfo ������Ʈ�� ���ڷ� �޾Ƽ� �κ��丮�� ��ųʸ� ��Ͽ��� �������ִ� �޼��� �Դϴ�.<br/>
        /// �˻� ����� �ƴ϶� ���� �����Ͽ� ��Ͽ��� �����ϹǷ�, Ư�� �������� ���� �����ϴ� �뵵�� ����մϴ�.<br/>
        /// *** ���� ���� �������� ������ ���ܸ� ��ȯ�մϴ�. ***
        /// </summary>
        /// <returns>��ųʸ� ����� ���ſ� ������ ��� �ش� �������� ItemInfo ��������, ��Ͽ� ���� �������� ��� null�� ��ȯ�մϴ�.</returns>
        public ItemInfo RemoveItem(ItemInfo itemInfo)
        {
            // ���������� ����ó��
            if(itemInfo==null)
                throw new Exception("������ ��ũ��Ʈ�� �������� �ʴ� ������Ʈ�Դϴ�. Ȯ���Ͽ� �ּ���.");
            
            // �̸��� ������� �ش� ��ųʸ� ������ �޽��ϴ�.
            List<ItemInfo> itemInfoList = GetItemInfoList(itemInfo.Item.Name);            

            // �κ��丮 ��Ͽ��� ���� ������ ����ó��
            if( itemInfoList==null || itemInfoList.Count==0 )
                throw new Exception("�ش� �������� �κ��丮 ���ο� �������� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");

            // ItemInfo����Ʈ�� ���� ItemInfo�� �����Ͽ� ��Ͽ��� �����մϴ�.
            itemInfoList.Remove(itemInfo);

            // �ش� ������ ������ ���� ������Ʈ�� ������ ���ҽ�ŵ�ϴ�.
            CalculateItemObjCount(itemInfo.Type, -1);    

            // ��Ͽ��� ������ �������� �ٽ� ��ȯ�մϴ�. 
            return itemInfo;
        }


        
        /// <summary>
        /// �������� �̸��� ���ڷ� �޾Ƽ� �ش� �������� �˻��Ͽ� �κ��丮�� ��ųʸ� ��Ͽ��� �������ִ� �޼��� �Դϴ�.<br/>
        /// ���ڸ� ���� �ֽż����� ���� �� ������, ������ ������ ������ �������� ������ �� �ֽ��ϴ�. �⺻�� �ֽż��Դϴ�.<br/>
        /// </summary>
        /// <returns>��ųʸ� ����� ���ſ� ������ ��� �ش� �������� ItemInfo ��������, ��Ͽ� ���� �������� ��� null�� ��ȯ�մϴ�.</returns>
        public ItemInfo RemoveItem(string itemName, bool isLatest=true)
        {            
            // ��ųʸ����� ItemInfo ����Ʈ�� �޽��ϴ�
            List<ItemInfo> itemInfoList = GetItemInfoList(itemName); 

            // ������Ʈ ����Ʈ�� �������� �ʰų�, ������Ʈ ����Ʈ�� ������ 0�̶�� ������ ���ϹǷ� ����ó��
            if(itemInfoList==null || itemInfoList.Count==0)
                throw new Exception("�ش� �������� �κ��丮 ���ο� �������� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");


            // �ش� ItemInfo ����Ʈ�� ù��° �׸��� �����Ͽ� �������� Ÿ���� ����ϴ�
            ItemType itemType = itemInfoList[0].Item.Type;            
            
            ItemInfo targetItemInfo = null;  // ��ȯ�� �����۰� ������ �ε����� �����մϴ�.
            int targetIdx;

            if( isLatest )
                targetIdx = itemInfoList.Count-1;  // �ֽ� ��
            else
                targetIdx = 0;                    //������ ��

            targetItemInfo =itemInfoList[targetIdx];    // ��ȯ �� ������ �������� �����մϴ�.
            itemInfoList.RemoveAt(targetIdx);           // �ش� �ε����� �������� ����Ʈ���� �����մϴ�            
        
            CalculateItemObjCount(itemType, -1);        // �ش� ������ ������ ���� ������Ʈ�� ������ ���ҽ�ŵ�ϴ�.      
                        
            return targetItemInfo;                      // ��Ͽ��� ������ �������� ��ȯ�մϴ�.     
        }




        /// <summary>
        /// ���ڷ� ���� �� �̸��� �ش� �ϴ� �������� ������ �������� �˷��ݴϴ�.
        /// </summary>
        /// <returns>��ȭ �������� ��쿡�� ��ø ������, ����ȭ �������� ��쿡�� ������Ʈ ������ ��ȯ</returns>
        public int HowManyCount(string itemName)
        {
            ItemType itemType = GetItemTypeIgnoreExists(itemName);

            List<ItemInfo> itemInfoList= GetItemInfoList(itemName);

                        
            int totalCount = 0;

            // ��ȭ �������� ��� ��ø������ ������ŵ�ϴ�.
            if(itemType == ItemType.Misc)
            {
                foreach(ItemInfo itemInfo in itemInfoList)
                    totalCount += itemInfo.OverlapCount;
            }
            // ����ȭ �������� ��� ������Ʈ ���ڸ� ������ŵ�ϴ�. 
            else
                totalCount = itemInfoList.Count;
            
            return totalCount;
        }




        /// <summary>
        /// ������ �������� ���� �����Ͽ� �ش� �������� ���� ������ŭ ���ҽ�ŵ�ϴ�.<br/><br/>
        /// *** ������ ���� ������ ���� �ʾҰų�, ���� ���� ������ �߸��� ��� ���ܰ� �߻� ***<br/>
        /// *** ��ȭ�������� �ƴ� ��� ���ܰ� �߻� ***
        /// </summary>
        /// <returns>������ ������� ������ false��, ������ ����Ͽ� ���ҿ� �����ϸ� true�� ��ȯ</returns>
        public bool ReduceItem(ItemInfo itemInfo, int overlapCount)
        {
            if(itemInfo==null)
                throw new Exception("������ ������ ���޵��� �ʾҽ��ϴ�.");
            
            if(overlapCount<=0)
                throw new Exception("�������� ������ �߸��Ǿ����ϴ�.");


            ItemMisc itemMisc = itemInfo.Item as ItemMisc;
            
            if(itemMisc == null)
                throw new Exception( "����ȭ �������� ��� ������ ���� �� �����ϴ�. RemoveItem�޼��带 �������ּ���.");

            // �������� ������ ������ ���� �������� ���ٸ� ���и� ��ȯ�մϴ�.
            if( itemMisc.OverlapCount < overlapCount )
                return false;
            else
                itemMisc.AccumulateOverlapCount(-overlapCount);

            // ������ 0�̵Ǹ� �ı��ϰ�
            if( itemMisc.OverlapCount==0 )
            {
                RemoveItem(itemInfo);                       // �κ��丮 ��Ͽ��� �����մϴ�.
                GameObject.Destroy( itemInfo.gameObject );  // �ֻ��� ������Ʈ(2D������Ʈ)�� �����մϴ�.
                itemInfo.StatusWindowInteractive.OnItemPointerExit();   // ����â�� ��������ݴϴ�.
            }
            // �ƴ϶�� ���������� ������Ʈ �մϴ�.
            else
                itemInfo.UpdateTextInfo();

            return true;
        }

        
        










        
        


    }
}
