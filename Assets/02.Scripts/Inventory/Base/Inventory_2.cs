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
                          
            // ���� ���� �ε��� ������ �Է��մϴ�.
            item.SlotIndexEach = GetItemSlotIndexIndirect(itemType, false);
            
            // (����Ʈ���� �ƴ� ���) ��ü ���� �ε��� ������ �Է��մϴ�.
            if(itemType!=ItemType.Quest)
                item.SlotIndexAll = GetItemSlotIndexIndirect(itemType, true);
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

            // ��ü ���� Ȱ��ȭ�Ǿ� �ִ� ���
            if(isActiveTabAll)
            {
                item.SlotIndexAll = slotIndex;                                  // ��ü ���� �ε����� ���� ����
                item.SlotIndexEach = GetItemSlotIndexIndirect(itemType, false); // ���� ���� �ε����� ����� �ƹ��� ����
            }
            // ���� ���� Ȱ��ȭ�Ǿ� �ִ� ���
            else
            {
                item.SlotIndexAll = GetItemSlotIndexIndirect(itemType, true);   // ��ü ���� �ε����� ����� �ƹ��� ����
                item.SlotIndexEach = slotIndex;                                 // ���� ���� �ε����� ���� ����
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
        public bool AddItem(ItemInfo itemInfo, int slotIndex=-1, bool isActiveTabAll=false)
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
                    return AddItemMisc(itemInfo, false ,slotIndex, isActiveTabAll);   // ������ �߰� �޼��带 ȣ���մϴ�.

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


            // ������ �̸��� ������� ������Ʈ ����Ʈ�� �����մϴ�. (�������� �ʴ´ٸ� ���Ӱ� �����Ͽ� ��ȯ�մϴ�.)
            List<GameObject> itemObjList = GetItemObjectList(itemName, true);

            // �ش� ������ ������Ʈ�� �߰��մϴ�.
            itemObjList.Add(itemInfo.gameObject);          
                  
            // �ش� ������ ������ ���� ������Ʈ�� ������ ������ŵ�ϴ�.
            CalculateItemObjCount(itemType, 1);          

            // ������ ��ȯ�մϴ�.
            return true;                                    
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
            if( GetCurRemainSlotCount(itemInfo.Item.Type) == 0 )
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
            List<GameObject> itemObjList = GetItemObjectList( newItemMisc.Name );                       

            // ���� �����ۿ� ����ä��� ���� �� �ĸ� ���� ������ ���� �������� �������� �����մϴ�.
            int beforeCount = newItemMisc.OverlapCount; 
            int afterCount = newItemMisc.OverlapCount;

            // ���� ������Ʈ ����Ʈ�� �ִ� ����
            if( itemObjList!=null )
            { 
                // �������� ������ �����մϴ�.
                itemObjList.Sort(CompareBySlotIndexEach);

                // ���� ������ ���� �� �������� ���� ���ؿ� ���� �ϳ��� ������ ���� �����ۿ� ����ä��⸦ �����մϴ�.
                if( isLatestModify )
                {
                    for( int i = itemObjList.Count-1; i>=0; i-- )
                        if( FillCountInLoopByOrder(i) ) { break; } // ���������� true�� ��ȯ�ϸ� ���������ϴ�.                                     
                }
                else
                {
                    for( int i = 0; i<=itemObjList.Count-1; i++ )   
                        if( FillCountInLoopByOrder(i) ) { break; } // ���������� true�� ��ȯ�ϸ� ���������ϴ�.  
                }
            }

            // ä���ְ� ���� ������ ��ȯ�մϴ�.
            return afterCount;

            



            // ���� �����ۿ� ���� ������ �ٸ��� �Ͽ� ä��� ���� �ݺ����� ���� �޼����Դϴ�.
            bool FillCountInLoopByOrder(int idx)
            {
                // ���� �����ۿ� �ε����� ���� ������ �Ͽ� ������ �ҷ��ɴϴ�.
                ItemMisc oldItemMisc = (ItemMisc)itemObjList[idx].GetComponent<ItemInfo>().Item;
                    
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
            List<GameObject> itemObjList = GetItemObjectList(itemInfo.Item.Name);            

            // �κ��丮 ��Ͽ��� ���� ������ ����ó��
            if( itemObjList==null || itemObjList.Count==0 )
                throw new Exception("�ش� �������� �κ��丮 ���ο� �������� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");

            // List<GameObject>�� ���� GameObject �ν��Ͻ��� �����Ͽ� ��Ͽ��� �����մϴ�.
            itemObjList.Remove(itemInfo.gameObject);

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
            // ��ųʸ����� ������Ʈ ����Ʈ�� �޽��ϴ�
            List<GameObject> itemObjList = GetItemObjectList(itemName); 

            // ������Ʈ ����Ʈ�� �������� �ʰų�, ������Ʈ ����Ʈ�� ������ 0�̶�� ������ ���ϹǷ� ����ó��
            if(itemObjList==null || itemObjList.Count==0)
                throw new Exception("�ش� �������� �κ��丮 ���ο� �������� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");


            // �ش� ������Ʈ ����Ʈ�� �����Ͽ� �������� Ÿ���� �̸� ����ϴ�
            ItemType itemType = itemObjList[0].GetComponent<ItemInfo>().Item.Type;            
            
            ItemInfo targetItemInfo = null;  // ��ȯ�� �����۰� ������ �ε����� �����մϴ�.
            int targetIdx;

            if( isLatest )
                targetIdx = itemObjList.Count-1;  // �ֽ� ��
            else
                targetIdx = 0;                    //������ ��

            targetItemInfo =itemObjList[targetIdx].GetComponent<ItemInfo>();  // ��ȯ �� ������ �������� ����ϴ�.
            itemObjList.RemoveAt(targetIdx);        // �ش� �ε����� �������� �����մϴ�            
        
            CalculateItemObjCount(itemType, -1);       // �ش� ������ ������ ���� ������Ʈ�� ������ ���ҽ�ŵ�ϴ�.      
                        
            return targetItemInfo;            // ��Ͽ��� ������ �������� ��ȯ�մϴ�.     
        }








        
        /// <summary>
        /// ������ �̸��� ������� �ش� �������� ItemType ���� ��ȯ�մϴ�<br/>
        /// �������� ���� �κ��丮�� ��Ͽ� �������� �ʾƵ� ItemType ���� ���� �� �ֽ��ϴ�.<br/><br/>
        /// ** �ش��ϴ� �̸��� �������� ���� ������ ��Ͽ� �������� �ʴ� ��� ���� �߻� **
        /// </summary>
        /// <returns>�̸��� �ش��ϴ� ������ Ÿ���� ��ȯ</returns>
        public ItemType GetItemTypeIgnoreExists(string itemName)
        {            
            return createManager.GetWorldItemType(itemName);
        }

        
        /// <summary>
        /// �������� �̸��� ������� �ش� �������� ��ųʸ� �������� ��ȯ�մϴ�<br/>
        /// ������ �̸��� �ش��ϴ� ������ ������ ������ �������� �ʴ´ٸ� null�� ��ȯ�մϴ�.<br/>
        /// ** �ش��ϴ� �̸��� �������� ���� ������ ��Ͽ� �������� �ʴ� ��� ���� �߻� **
        /// </summary>
        /// <returns>�ش� ������ ������ ������ ��ȯ</returns>
        public Dictionary<string, List<GameObject>> GetItemDic(string itemName)
        {            
            ItemType itemType = createManager.GetWorldItemType(itemName);

            return GetItemDic(itemType);
        }


        /// <summary>
        /// �������� Ÿ���� ������� �ش� �������� ��ųʸ� �������� ��ȯ�մϴ�<br/>
        /// �ش� ������ ������ ������ �������� �ʴ´ٸ� null�� ��ȯ�մϴ�.<br/>
        /// *** ItemType.None���� ���� �� ��� ���ܸ� �����ϴ� *** 
        /// </summary>
        /// <returns>�ش� ������ ������ ������ ��ȯ</returns>
        public Dictionary<string, List<GameObject>> GetItemDic(ItemType itemType)
        {
            if(itemType == ItemType.None)
                throw new Exception("��Ȯ�� ������ �������� �����ؾ� �մϴ�.");

            int dicIdx = GetDicIndex(itemType);

            if( dicIdx < 0 )
                return null;
            else
                return itemDic[dicIdx];
        }
        

        /// <summary>
        /// ������ �̸��� �ش��ϴ� ������Ʈ ����Ʈ �������� ��ȯ�մϴ�<br/>
        /// �ɼ��� ���� ������Ʈ ����Ʈ�� ���ٸ� ���Ӱ� ������ �� �ֽ��ϴ�.<br/><br/>
        /// *** �ش� ������ �̸��� ��������� ��Ī���� �ʴ� ��� ���ܰ� �߻� ***<br/>
        /// *** �ش� ������ �̸��� ������ �ش��ϴ� ������ �����ϰ� ���� �ʴٸ� ���ܰ� �߻� ***
        /// </summary>
        /// <returns>
        ///  1. ������ �̸��� �������� �ϳ��� ����ִٸ� �ش� ������Ʈ����Ʈ�� ��ȯ�մϴ�.<br/>
        ///  2. ������ �̸��� �������� �ϳ��� ������� ���� �� (���� �����ϱ� �ɼ��� ���� ���� ���) null���� ��ȯ�մϴ�.(�Ϲ��� ���, IsExist�� ��ü����)<br/>
        ///  3. ������ �̸��� �������� �ϳ��� ������� ���� �� (���� �����ϱ� �ɼ��� �� ���) ���ο� ������Ʈ ����Ʈ�� ��ȯ�մϴ�.(AddItem�� ���)
        /// </returns>
        public List<GameObject> GetItemObjectList(string itemName, bool isNewIfNotExist=false)
        {
            // ������ �̸��� �ش��ϴ� ���� �ε����� ���մϴ�.
            int dicIdx = GetDicIndex( GetItemTypeIgnoreExists(itemName) );

            // �����ε��� ���� 0���϶�� ���ܸ� �����ϴ�.
            if(dicIdx<0)
                throw new Exception("�ش� ������ �̸��� �ش��ϴ� ������ �������� �ʽ��ϴ�.");

            // �������� �ϳ��� ����ִٸ� �ٷ� �ش� ������Ʈ ����Ʈ�� ��ȯ�մϴ�.
            if(itemDic[dicIdx].ContainsKey(itemName))
                return itemDic[dicIdx][itemName];
            
            // �������� ������� ������, ���� �����ϱ� �ɼ��� �ִ� ���
            // ������Ʈ ����Ʈ�� ���� ���� �� ����Ͽ� ��ȯ�մϴ�.
            else if(isNewIfNotExist)     
            {                    
                List<GameObject> itemObjList = new List<GameObject>();  // ������Ʈ ����Ʈ�� ���� ����ϴ�.
                itemDic[dicIdx].Add(itemName, itemObjList);             // �κ��丮 ������ ������Ʈ ����Ʈ�� ����ֽ��ϴ�.
                return itemObjList;                                     // ������ ������Ʈ ����Ʈ ������ ��ȯ�մϴ�.
            }
            
            // �������� ������� ������, ���� �����ϱ� �ɼ��� ���� ��� null�� ��ȯ�մϴ�.
            else                    
                return null;           
        }


        /// <summary>
        /// ������ �̸��� �Է��Ͽ� �ش� �������� ���� enum�� int������ ��ȯ�޽��ϴ�. (�κ��丮�� �������� �ʾƵ� �˴ϴ�.) <br/>
        /// *** �������� ���� ������ �������� �ʴ´ٸ� ���ܰ� �߻�***
        /// </summary>
        /// <returns>�ش� �������� ItemType�� int�� ��ȯ ��</returns>
        public int GetItemTypeIndexIgnoreExists(string itemName)
        {   
            return (int)GetItemTypeIgnoreExists(itemName);
        }



        /// <summary>
        /// �ش� �������� �̸��� ���ڷ� �־� �������� ��ø �ִ� ������ ��ȯ �޽��ϴ�.<br/>
        /// ������ ���� ������ �ִ� ��ø ���� ������ �˱� ���� �޼����Դϴ�.<br/><br/>
        /// ���� IsAbleToAddMisc�޼��忡�� ���������� ���˴ϴ�.<br/><br/>
        /// *** �ش��ϴ� �̸��� �������� ��ȭ�������� �ƴϰų� ���� ��� ���ܰ� �߻��մϴ�.<br/>
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns>�������� �ִ� ������ ��ȯ�մϴ�.</returns>
        public int GetItemMaxOverlapCount(string itemName)
        {
            Dictionary<string, Item> worldDic = createManager.GetWorldDic(itemName);

            ItemMisc itemMisc = worldDic[itemName] as ItemMisc;
            
            if(itemMisc==null)
                throw new Exception("�ش� ������ �̸��� ��ȭ�������� �ƴմϴ�.");

            return itemMisc.MaxOverlapCount;
        }









        
        


    }
}
