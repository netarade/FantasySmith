using CreateManagement;
using ItemData;
using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * [���� ����]
 * �÷��̾� ���ۿ� �ʿ��� �������̸�, ����ȭ�Ǿ� Save �� Load �Ǿ�� �� ����
 * 
 * [�κ��丮�� ���]
 * 1- ����ȭ ������ȭ
 * 2- �ش� ������ ���� �� ������Ͽ��� ����, ���� ����, ��ġ �ݿ�, 
 * 
 * 
 * [�۾� ����]
 * <v1.0 - 2023_1113_�ֿ���>
 * 1- �з� ����� Ivnentory Ŭ������ Inventory.cs���Ϸ� �и��Ͽ��� (namespace�� �״��)
 * 
 * <v2.0 - 2023_1114_�ֿ���>
 * 1- �κ��丮 Ŭ���� ����� ��� �� ��ȭ������ ���ӿ�����Ʈ ����Ʈ�� ��ųʸ�-���ӿ�����Ʈ����Ʈ�� �����ϱ�� ��ȹ
 * => �� ������ ���� ���� ������ �Ͼ�� ����Ʈ�� ���������� ���� �ϳ��� ������ ����� ��ȿ���� �߻��ϰ� �ֱ� �����̸�,
 * Ư�� ������ ������ �������� ��Ͽ��� �����ϱ� ���� �Ź� ����Ʈ�� �ϳ��� ���������ϸ� ���� �ڿ��Ҹ� ���� �߻��ϱ� ����
 * 
 * <v3.0 - 2023_1119_�ֿ���>
 * 1- �޼��� �̸� ���� - GetItemList -> ConvertDicToItemList
 * 2- �޼��� �̸� ���� �� �Ű����� ���� - SetObjectList(ItemType, List<Item>) -> ConvertItemListToDic(List<Item>)
 * 3- ������Ƽ �̸� ���� - TotalMaxCount -> AllCountLimit, TotalCurCount->TotalCount
 * 4- InitialCount -> InitialCountLimit���� ���� �� static �������� ������Ƽ�� ����. (�޸� ���� ���� ����)
 * 
 * 5- ���ο� ������� �߰� - weapCountLimit, miscCountLimit 
 * �κ��丮 �������� ĭ���� ������ �ִ� ������� �����ؾ� �ϱ� ����
 * ĭ���� change�Ѵٰ� �� �� ��ü ĭ���� �ø��� ���߿� ���� ĭ���� �������� ���ϱ� �����̸�, 
 * item�� add�� ���� ���� ������ �־�� ���� ĭ���� ���Ͽ� add��ų �� ������,
 * ���� ���� ĭ ���� �������� TotalCountLimit�� �ڵ����� ������ �� �ֱ� ����
 *  
 * 6- �⺻ ������ ����
 * �κ��丮 �ʱ� ������, �����ʱ�ȭ�� ���� ���� ĭ �� �ʱ�ȭ�� �̷����.
 * 
 * <v4.0 - 2023_1120_�ֿ���>
 * 1- �� ��ũ��Ʈ�� Inventory Ŭ������ partial Ŭ������ ��ȯ
 * ������ �߰�, �˻�, ���� ���� ������ ���� ���Ͽ��� �� ����
 * 2- ���� ����Ʈ�� ����� �߰�
 * 3- Inventory Ŭ������ TotalCountLimit �������� AllCountLimit���� ����
 * 4- Inventory Ŭ������ SerializableInventory Ŭ������ weapCountLimit, miscCountLimit ������Ƽȭ �� ������ �ոӸ� �빮�ڷ� ����
 * 5- Serializable Ŭ������ �����ڿ�  WeapCountLimit, MiscCountLimit�� �ʱ�ȭ�� �����ִ� �� ����
 * 
 * <v4.1 - 2023_1122_�ֿ���>
 * 1- ����, ��Ÿ ������ �ּ� ����
 * 
 * <v4.2 - 2023_1221_�ֿ���>
 * 1- ����Ŭ������ Inventory_p2.cs ���� �߰�
 * AddItem�޼��带 �߰��Ͽ���.
 * 
 * <v5.0 - 2023_1221_�ֿ���>
 * 1- WeapCount�� MiscCount�� ���� miscDic�� weapDic�� Values�� �ٲ㼭 �����ϴ� �� ����
 * 
 * 2- ���ο� ������ �����ϴ� ��� DataManager�� Load�޼���� GameData�� ������ ������
 * ����Ʈ �����ڰ� �ƴ� Inventory( SerializableInventory savedInventory ) �����ڰ� ȣ������� ����
 * ��ųʸ��� �ʱ�ȭ���� �ʾ� null���۷����� �߻��Ͽ����ϴ�.
 * �̸� ���ֱ� ���� �ش� �����ڿ� ��ųʸ��� null�� �˻縦 �߰��Ͽ� ���ο� ��ųʸ��� �����ϵ��� �Ͽ����ϴ�.
 * 
 * <v5.1 - 2023_1224_�ֿ���>
 * 1- ������ ������ ConvertItemListToDic �޼��忡��
 * The given key 'ö' was not present in the dictionary. ������ �߻��Ͽ�
 * miscDic[item.Name]==null�� �˻��ϴ� ������ miscDic.ContainsKey(item.Name) �������� ����
 * 
 * <v6.0 - 2023_1226_�ֿ���>
 * 1- SerializableInventory�� weapList, miscList�� Item Ÿ�Կ��� ���� Ÿ���� ItemWeapon, ItemMiscŸ������ ����
 * 2- ConvertDicToItemList�� ConvertItemListToDic �޼��� ���������� ������ ���� �ߺ��������� �ۼ��Ǿ��ִ� ���� �Ϲ�ȭ�޼���� ����
 * 
 * <v7.0 - 2023_1226_�ֿ���>
 * 1- DeserializeItemListToDic�� ����� ��¿� �޼������ ����
 * 
 * 2- SerializableInventory�� WeapCountLimit, MiscCountLimit���� �ڵ����� ������Ƽ�� �ƴ� public������ ����
 * 
 * 3- SerializableInventory�� Ŭ���� ���� SInventory�� �����ϰ� Serialize�޼���� Deserialize�޼��带 �����Ͽ���.
 * ������ STransform�� ��뿡 �ϰ����� �־� ����� �ʰ� �ϱ� ����
 * 
 * 4- InventoryŬ������ SerialzableInventory�� ���ڷ� �޴� ������ ����
 * SInventory�� Deserialize�޼��忡�� ���Ӱ� �κ��丮�� �����ؼ� �ϳ��� ���� �־��༭ �κ��丮Ŭ������ ��ȯ�ϰԲ� ����
 * 
 * 5- WeapCountLimit, MiscCountLimit ������Ƽ set��� �߰�
 * 
 * <v7.1 - 2023_1227_�ֿ���>
 * 1- Item�������� �������� �������� ���� SerializeDicToItemList�޼��忡�� GetComponent�� ItemInfo�� �����ϴ� ���� GetComponentInChildren���� ����
 * 
 * <v7.2 - 2023_1229_�ֿ���>
 * 1- ���ӽ����̽��� CraftData���� InventoryManagement�� ����
 * 2- SInventoryŬ���� SaveData_Inventory�� �̵�
 * 
 * <v7.3 - 2023_1230_�ֿ���>
 * 1- ���� ��������� ��ųʸ��� privateó��
 * ������ ���̺귯�� ����ڰ� �κ��丮�� ���� ���� �� �Ǽ��� �����ϱ� ����.
 * 
 * <v8.0 - 2023_1231_�ֿ���>
 * 1- WeapCountLimit,MiscCountLimit�� -> WeapSlotCountLimit, MiscSlotCountLimit���� �̸� ����
 * ���� ���� �� (������ ĭ ���� ��)��� �ǹ̸� �и��ϰ� �Ͽ���
 * 
 * 2- curWeapItemCount, curMiscItemCount ���� �߰�
 * ���� ����ִ� ������Ʈ ������ �����ϴ� ������ ����
 * 
 * 3- TotalCount�� TotalCurItemCount�� ���� 
 * curWeapItemCount, curMiscItemCount �������� ���
 * 
 * 4- ������ WeapCount, MiscCount ������Ƽ�� �����ϰ� GetCurItemCount, SetCurItemCount, GetItemCountLimit �޼��带 �߰�
 * 
 * �ε� �� curWeapItemCount�� curMiscItemCount�� �ʱ�ȭ �ϰų�,
 * AddItem, RemoveItem���� ���������� ������ ī��Ʈ�� ������ ���� �� �̿��ϵ��� ����
 *  
 * 
 * 5- weapDic, miscDic�� JsonProperty�� �ٿ����Ҵ� �� ����
 * 
 * 6- DeserializeItemListToDic ����ȭ,
 * Inventory_2�� ���ǵǾ��ִ� �⺻ �޼��带 ������� ��������
 * 
 * <v8.1 - 2023_1231_�ֿ���>
 * 1- ���� �ε����� �ӽ÷� ��� ���� indexList ������ ����
 * 2- ������ ���� 
 * AllCountLimit -> SlotCountLimitAll
 * WeapSlotCountLimit -> SlotCountLimitWeap
 * MiscSlotCountLimit -> SlotCountLimitMisc
 * 
 * 3- GetCurItemCountLimit�޼��� ����, GetItemSlotCountLimit�� ���Ӱ� ����,
 * ���ڸ� �� ���� �� ��ü ������ �ִ� ���� ���� ��ȯ�ϵ��� �����Ͽ���,
 * ���ڸ� ���� �� ItemType�� ���� �ִ� ���� ���� ���� ��ȯ�ϵ��� ����.
 * 
 * 4- GetCurItemCount�޼��� ���� ��ųʸ� count==0���� Ȯ���ϴ� �ڵ� ����
 * 
 * 
 * <v8.2 - 2024_0102_�ֿ���>
 * 1- ��ųʸ��� ���̸� ��ȯ�ϴ� ������Ƽ �߰�
 * 2- CurItemCount, CurItemWeapCout, CurItemMiscCount�� CurItemObjCount�� �̸�����
 * 
 * 
 * [�̽�_0102]
 * 1- ��ųʸ� �������� �Ϲ�ȭ ��ų �� �־����.
 * 2- curDicLen ������Ƽ�� ���� ��ųʸ��� ����� ���θ� ��ȯ�ؾ� �ϴµ� �̸� ���� enum������ ���� ������ �־�� �Ѵ�.
 * 
 * 
 * 
 * 
 * 
 * <v8.3 - 2024_0103_�ֿ���>
 * 1- GetCurItem�޼��� ����ó���� �߰�
 * 
 * 
 * <v8.4 - 2024_0104_�ֿ���>
 * 1- ���� �����ִ� ������ ������ ��ȯ�ϴ� GetCurRemainSlotCount �޼��� �߰�
 * AddItem�� IsAbleToAddMisc�޼��� ��� �������� �߰��� ������ �ֳ� ���캸�� ���� ���
 * 
 * <v8.5 - 2024_0105_�ֿ���>
 * 1- ���� ���������� createManager�� �ΰ�, ������ ȣ�� �� �ش� �������� �ʱ�ȭ�ϵ��� �Ͽ���.
 * 
 * 2- DeserializeItemListToDic���ο� �ڵ� ���� 
 * (ItemŬ������ ���� Add�ϴ� �޼��带 ������ ����� createManager���� ȣ���Ͽ� �������� �޵��� ����)
 * 
 * <v8.6 - 2024_0110_�ֿ���>
 * 1- CurDicLen �� ���� ItemType.None-1�̴� ���� ItemType.None���μ���
 * (�������� 0���� �����ϹǷ�)
 * 
 * 
 * <V9.0 -2024_0111_�ֿ���>
 * 1- ����Ʈ ������ Ŭ������ �߰��ϸ鼭 ���� �޼��带 ����
 * 
 * 2- ������ �� SetCurItemObjCount�� GetItemSlotCountLimit�޼��忡 ����Ʈ������ ���� �ڵ带 �߰�
 * 
 * 
 * 3- ��ųʸ� �Ϲ�ȭ�� �������̸�, ���� ��ųʸ��� ��ųʸ� �迭�� ��ȯ�ϰ�, (itemDic[])
 * �ش� ��ųʸ��� ������ Ÿ�� ������ ���Ӱ� �����Ͽ���.(dicType[])
 * 
 * 4- �κ��丮 ������, ����ȭ, ������ȭ �޼��� �迭������� ����
 * 
 * 5- ������ ���� ������ �ε��� �˻��� ���� GetDicIdxByItemType�޼����
 * ������ Ŭ������ ������ ������ ��Ī�����ִ� TypeMatchingItemClassToItemType�޼��带 �߰�
 * 
 * <v9.1 - 2024_0111_�ֿ���>
 * 1- �޼���� ���� (Get,Set)CurItemCount(,All)-> GetCurDicItemObjCountAll, GetCurDicItemObjCount, SetCurDicItemObjCount
 * 2- GetCurDicItemObjCountAll�޼���� SetCurDicItemObjCount�޼��带 GetCurDicItemObjCount����� �ƴ϶�, CurDicItemObjCount�� ���ϴ� ������ ����
 * 3- dicType���� public���� ����
 * 4- Serialize, Deserialize �޼��� null�� ���� ���ǰ˻繮 �߰�(����ڰ� ������ ������ �� �ֱ� ����)
 * 
 * <v9.2 - 2024_0112_�ֿ���>
 * 1- CurDicItemObjCount int�迭 ������Ƽ�� �����ڿ��� �Ҵ�����ְ� �ִ����� ����
 * 
 * 
 * <v10.0 - 2024_0115_�ֿ���>
 * 1- Initializer�κ��� TabType ���� ������ �޾� �ʱ�ȭ �ϵ��� ����      
 * ������ Add�� � ��Ÿ���� �������� ���� �ε����� ������ ����ؾ��ϱ� ����
 * => (���) TabType�� �̴ϼȶ������� ���� �ʰ� �׻� �����ϵ��� ����
 * 
 * 2- slotCountLimitTab���� �߰��ϰ� �����ڿ��� �ʱ�ȭ
 * �� ���� ��������� ������ ���Ҵ� �� ����� �ʿ䰡 �ֱ� ����
 * 
 * 3- InitSlotCountLimitTab�޼��� �߰��Ͽ�, slotCountLimitTab�� �� ����� �޼���ȣ��� �ڵ���� 
 * 
 * 4- �⺻ �κ��丮 ������ ���� 
 * Initializer�� �̿��� �����ڸ� ȣ���� ���̱� ����
 * 
 * 5- InitialCountLimit������Ƽ ����, Initializer�� �̿��ؼ� ���� ���� ���� �ʱ�ȭ�ϱ� ����
 * 
 * 6- CurDicItemObjCount������Ƽ�� �Ϲ� �迭������ ����, ������ dicItemObjCount�� ����
 * 
 * 7- GetItemSlotCountLimit�޼���� GetSlotCountLimitDic���� ����, GetSlotCountLimitTab �޼��� �߰�
 * (��ųʸ��� ���� ���Ѽ��� �� �� ���� ���Ѽ��� �ݵ�� �����ؼ� ���ؾ��ϱ� ����)
 * 
 * 
 * 8 - tabItemObjCount���� ���Ӱ� ����
 * �ǿ� ��� �������� ������ �˷��ִ� �Ӽ�
 * 
 * 9- GetDicIdxByItemType�޼��� ����(GetDicIndex�� �ߺ��ǹǷ�)
 * 
 * 10- GetTabIndex�޼��� �ۼ�
 * ���� �ε����� ��ȯ�ϴ� ���
 * 
 * 11 - �޼���� SetCurDicItemObjCount�� AccumulateItemObjCount�� ����
 * ������ ������ ������ ���� ��ųʸ� ������Ʈ ���� �Բ� �� ������Ʈ ���� ���ÿ� �������Ѿ� �ϱ� ����
 * 
 * 12- GetCurDicItemObjCountAll�޼��� ����
 * tabItemObjCount���� ��ü���� ��쿡 ���� ����ϱ� ����
 * 
 * 13 - SlotCountLimitAll���� slotCountLimitTab���� ��ü�Ǻ� ���� ���� ����ϰ� �ֱ� ����
 * 
 * 14- isShareSlot���� ���� �� �����ڿ��� Initializer������ isShareSlot �ɼ��� ���ڷ� �Ѱܹ޾� �ʱ�ȭ 
 * 
 * 15- GetCurDicItemObjCount�� InitCurDicItemObjCount�� ����, privat�޼���� ����
 * GetCurDicItemObjCountAll�޼��� ����
 * 
 * 16- GetCurRemainSlotCount�޼��� ���������� ���Ѽ� ��ݿ��� �����Ѽ� ������� ����
 * �������� �������� �������Ѿ� �ϹǷ�,
 * 
 * <v10.1 - 2024_0115_�ֿ���>
 * 1- slotCountLimitDic �� dicItemObjCount ���� ����, GetSlotCountLimitDic�޼��� ����
 * �׻� ������ ���� ���� �� ���� ������� �����ؾ� �ϹǷ� �����ϰų� ������ �ʿ伺�� ����.
 * 
 * 2- InitSlotCountLimitTab�޼��� ����
 * �ǽ��������� �̹� �����ϹǷ� ���Ӱ� ����� �ʿ䰡 ������
 * 
 * <v10.2 - 2024_0115_�ֿ���>
 * 1- GetCurRemainSlotCount�޼��� �ּ� �Ϻ� ���� 
 * (ItemType.None���޽� ����-> ��ü�ǹ�ȯ)
 * 
 * 
 */




namespace InventoryManagement
{
    /// <summary>
    /// �κ��丮�� �����ϴ� Ŭ����, ��ųʸ��� �̸������� ���Ͽ� �����ϸ�
    /// ���ο� GameObject�� �����ϴ� weapList�� miscList�� �ֽ��ϴ�.<br/> 
    /// ******Save, Load�� �κ��丮 Ŭ������ �����ؾ� �մϴ�.******
    /// </summary>
    [Serializable]
    public partial class Inventory
    {
        /**** ���̺� �ε� �� �����ؾ��� �Ӽ��� ****/
        public Dictionary<string, List<GameObject>>[] itemDic;  // ������ ��ųʸ�
        public ItemType[] dicType;                              // ��ųʸ��� ����

        public int[] slotCountLimitTab;                         // �Ǻ� ���� ���� ��

        /*** �ε�ÿ��� �����Ǵ� �Ӽ��� ***/
        public TabType[] tabType;                           // ���� ����


        /**** �ε� �ÿ��� �ҷ��� �� ���� �߿� ���������� ���� �� �����ؾ� �� �Ӽ� ****/ 
        /// <summary>
        /// �� ���� �����ϰ� �ִ� ������ ������Ʈ�� ���� ��ȯ�ްų� �����մϴ�.
        /// </summary>
        public int[] tabItemObjCount; 





        /**** �ڵ����� �����Ǵ� ���� �Ӽ��� ****/    

        /// <summary>
        /// �κ��丮�� ���� �����ϰ� �ִ� ������ ���̸� ��ȯ�մϴ�
        /// </summary>
        public int dicLen { get { return dicType.Length; } }


        /// <summary>
        /// �κ��丮�� ���� �����ϰ� �ִ� ���� ���̸� ��ȯ�մϴ�
        /// </summary>
        public int tabLen { get { return tabType.Length;} }


        
        /**** ���������θ� ����ϴ� �Ӽ� ****/
        List<int> indexList = new List<int>();      // ��ųʸ��� ��� �������� �ε����� �о�鿩 ��� ����Ʈ

        CreateManager createManager;                // ���� ������ ������ ������ �Ŵ��� �ν��Ͻ�

        List<ItemType> tabKindList = new List<ItemType>();  // �������� ��ġ�ϴ� ������Ÿ���� ��� ����Ʈ





        
        /// <summary>
        /// ������ ������ �ش��ϴ� ������ �ε����� ��ȯ�մϴ�.<br/>
        /// ��ġ�ϴ� ������ ������ ���ٸ� -1�� ��ȯ�մϴ�.<br/><br/>
        /// *** ItemType.None�� �����ϸ� ���ܰ� �߻��մϴ�. ***
        /// </summary>
        /// <returns>��ġ�ϴ� ������ ������ �ִٸ� �ش� ������ �ε��� ���� ��ȯ, ���ٸ� -1�� ��ȯ</returns>
        public int GetDicIndex(ItemType itemType)
        {
            if(itemType==ItemType.None )
                throw new Exception("������ ������ �߸��Ǿ����ϴ�. �ش� ������ ������ ���� �� �����ϴ�.");

            for(int i=0; i<dicLen; i++)
            {
                if( dicType[i]==itemType) 
                    return i;
            }

            return -1;
        }


        /// <summary>
        /// ������ ������ �ش��ϴ� ���� �ε����� ��ȯ�մϴ�.<br/>
        /// ItemType.None�� ���޵Ǿ��ٸ� ��ü �� �ε��� (TabType.All)�� ���մϴ�.<br/>
        /// *** ������ ������ �ش��ϴ� ���̾��ٸ� ���ܰ� �߻��մϴ�. ***
        /// </summary>
        /// <returns>������ ������ �ش��ϴ� ���� �ִٸ� �ش� ���� �ε����� ��ȯ, ���ٸ� -1�� ��ȯ</returns>
        public int GetTabIndex(ItemType itemType)
        {
            return (int)ConvertItemTypeToTabType(itemType);            
        }








        
        
        /// <summary>
        /// ���� �κ��丮�� ����ִ� ��ųʸ� �� ������ ������Ʈ�� ���� �����Ͽ� ��ȯ�մϴ�.<br/>
        /// �ʱ� �ε� �� �κ��丮�� �ε� �� ������Ʈ�� ���� �� dicItemObjCount�� �����ϱ� ���� �ʿ��մϴ�.<br/><br/>
        /// ���ڷ� ItemType�� �����Ͽ� �ش� ItemType�� ��ųʸ����� ������Ʈ ������ ī�����մϴ�.<br/><br/>
        /// *** �ش� ������ ������ ������ ������ ���ܸ� �߻���ŵ�ϴ�. ***
        /// </summary>
        /// <returns>������ Ÿ�԰� ��ġ�ϴ� �κ��丮�� ���� �Ǿ��ִ� ������Ʈ�� ������ ��ȯ�մϴ�.</returns>
        private int InitCurDicItemObjCount(ItemType itemType) 
        { 
            // ����� ������Ʈ�� �� ������ ����� �ʱ�ȭ�մϴ�
            int toatlCount = 0;

            // ������ Ÿ���� ������� ��ųʸ��� ���մϴ�.
            Dictionary<string, List<GameObject>> itemDic = GetItemDicIgnoreExsists(itemType);

            // �ش� �κ��丮 ��ųʸ��� ����ִ� ����Ʈ�� �ϳ��� ���ٸ�, �ٷ� 0�� ��ȯ�մϴ�.
            if(itemDic.Count==0)        
                return 0;

            // �ش� ��ųʸ����� ���ӿ�����Ʈ ����Ʈ�� ������ ����Ʈ�� Count ������Ƽ�� ���� ���ӿ�����Ʈ ���ڸ� ������ŵ�ϴ�.
            foreach(List<GameObject> objList in itemDic.Values)
                toatlCount += objList.Count;   
            
            return toatlCount;  
        }

        /// <summary>
        /// ���� �κ��丮�� ����ִ� �� �� ������ ������Ʈ�� ���� �����Ͽ� ��ȯ�մϴ�.<br/>
        /// (��ųʸ��� ������ ������Ʈ�� ���� ������� �����մϴ�.)<br/><br/>
        /// �ʱ� �ε� �� �κ��丮�� �ε� �� ������Ʈ�� ���� �� tabItemObjCount�� �����ϱ� ���� �ʿ��մϴ�.<br/><br/>
        /// ���ڷ� TabType�� �����Ͽ� �ش� ���� ItemType�� ��� ���Ͽ� ItemType�� ��ųʸ� ������Ʈ ���� ��� ī�����մϴ�.<br/><br/>
        /// *** �ش� ������ ���� ������ ���ܸ� �߻���ŵ�ϴ�. ***
        /// </summary>
        /// <returns>������ Ÿ�԰� ��ġ�ϴ� �κ��丮�� ���� �Ǿ��ִ� ������Ʈ�� ������ ��ȯ�մϴ�.</returns>
        private int InitCurTabItemObjCount(TabType tapType)
        {
            int totalCount = 0;

            // �������� �ش��ϴ� ��� ������ ������ ��� ����Ʈ�� ���޹޽��ϴ�.
            int tabKindLen = ConvertTabTypeToItemTypeList(ref tabKindList, tapType);

            // �ǿ� �ش��ϴ� ������ ������ �ϳ��� ��ȸ�մϴ�.
            for(int i=0; i<tabKindLen; i++)
            {
                // ������ ������ ���� ������Ʈ ���� ���� ���� �ش� ���� ������Ʈ ���� ������ŵ�ϴ�.
                totalCount += InitCurDicItemObjCount(tabKindList[i]);
            }
            
            return totalCount;
        }








        /// <summary>
        /// ItemType�� ������� � ������ dicItemCount�� ���� �Ǵ� ���� ��ų �� �����ϴ� �޼����Դϴ�.<br/>
        /// inCount�� ������ �����ϸ� �������� ���� ������ ���ҽ�ŵ�ϴ�.<br/>
        /// AddItem�޼���, RemoveItem�޼��忡�� ���������� ���ǰ� �ֽ��ϴ�.<br/><br/>
        /// *** ���ڷ� ItemType.None�� ������ ��� ���ܸ� �߻���ŵ�ϴ�. ***
        /// </summary>
        public void AccumulateItemObjCount(ItemType itemType, int inCount)
        {
            if(itemType == ItemType.None )
                throw new Exception("���ڷ� ���� �� ������ ������ �ش� �κ��丮�� ���� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");

            // ������ ������ �ش��ϴ� ��ųʸ��� ���� �ε����� ���մϴ�.
            int eachTabIdx = GetTabIndex(itemType);
            int allTabIdx = GetTabIndex(ItemType.None);

            // ������ ������ �ش��ϴ� �� �� ������Ʈ ���� ������ŵ�ϴ�
            tabItemObjCount[eachTabIdx] += inCount;

            // ����Ʈ �������� �ƴ� ��쿡�� ��ü�� ī��Ʈ�� ������ŵ�ϴ�
            if(itemType != ItemType.Quest)
                tabItemObjCount[allTabIdx] += inCount;
        }







        /// <summary>
        /// ������ ������ �ش��ϴ� �Ǻ� ������ ���� ���� ��ȯ�մϴ�.<br/>
        /// ItemType�� ���ڷ� ���޹�����, 
        /// ItemType.None ���� �� ��ü���� ���� ���� ���� ��ȯ�մϴ�<br/>
        /// </summary>
        /// <returns></returns>
        public int GetSlotCountLimitTab(ItemType itemType)
        {
            return slotCountLimitTab[GetTabIndex(itemType)];
        }




        /// <summary>
        /// ���� �κ��丮�� ������ ������ �ش� �ϴ� ���� �����ִ� ���� ������ ��ȯ�մϴ�.<br/>
        /// �������� ������ ItemType.None�� ���޵Ǿ��ٸ� ��ü���� ���� ���� ���� ��ȯ�մϴ�.
        /// </summary>
        /// <returns>�����ִ� ������ ������ 0, �����ִ� ������ �ִٸ� �ش� ������ ����</returns>
        public int GetCurRemainSlotCount(ItemType itemType)
        {
            if(itemType==ItemType.None )
                throw new Exception("�߸��� ������ ������ �����Դϴ�.");

            // ������ ������ �ش��ϴ� ���� (�ִ� �������Ѽ�-���� ������Ʈ��)
            return GetSlotCountLimitTab(itemType) - tabItemObjCount[GetTabIndex(itemType)];
        }











        /// <summary>
        /// ���� �κ��丮�� �����ϰ� �ִ� ���� �߿��� ���ϴ� ������ ������ �ν��Ͻ� ����Ʈ�� ����� �ݴϴ�.<br/>
        /// ������Ʈ�� ������ �������� ������ ������ �о���� �ʿ䰡 ���� ��,<br/>
        /// ���� ������Ʈ ����Ʈ�� ����ȭ �� ��, ���� �Ѿ�� ���� �����ؾ��ϴ� �뵵�� ����մϴ�.
        /// </summary>
        /// <param name="itemType"> GameObjectŸ�Կ��� ItemŸ������ ����Ʈȭ �� ��������� ������ �����ּ���. Weapon, Misc���� �ֽ��ϴ�. </param>
        public List<T> SerializeDicToItemList<T>() where T : Item
        {
            List<T> itemList = new List<T>();       // ���ο� T���� �����۸���Ʈ ����


            // ���� �κ��丮���� � ������ ���� �� ������ T�� �������� �����մϴ�.
            Dictionary<string, List<GameObject>> itemDic = null;
            ItemType itemType = ItemType.None;

            // ���� ������ Ŭ���� T���ڸ� ItemType�� �°� ��ȯ�մϴ�.
            itemType = TypeMatchingItemClassToItemType<T>();

            // ��ȯ�� itemType�� �������� � ��ųʸ��� ������ �� �����մϴ�.
            itemDic = GetItemDicIgnoreExsists(itemType);

            // ������ ��ųʸ��� �Ҵ�Ǿ����� �ʰų� ������ 0�̶��, null�� ��ȯ�մϴ�.
            if( itemDic==null || itemDic.Count==0 )
                return null;


            // �ش� ������ �������� �ϳ��� �ҷ��ͼ� ���� Ŭ���� ����(T����) ���� �����մϴ�.
            foreach( List<GameObject> objList in itemDic.Values )                // �ش� �������� ���ӿ�����Ʈ ����Ʈ�� �ϳ��� ������
            {
                // ������Ʈ ����Ʈ�� �Ҵ�Ǿ������� �������� �������� �ʴ°�� ���� ������Ʈ����Ʈ�� ã���ϴ�.
                if(objList.Count==0)
                    continue;

                for( int i = 0; i<objList.Count; i++ )                               // ����Ʈ�� ���ӿ�����Ʈ�� ��� �����ɴϴ�.
                    itemList.Add( (T)objList[i].GetComponentInChildren<ItemInfo>().Item ); // item ��ũ��Ʈ�� �ϳ��� ������ T�������� �����մϴ�.
            }

            // item ������ �ϳ��� ����Ǿ��ִ� itemList�� ��ȯ�մϴ�.
            return itemList;
        }


        /// <summary>
        /// ���ǵ� �� ������ Ŭ������ �κ��丮�� �����ϴ� ItemType���� ��Ī�����ִ� �޼����Դϴ�.
        /// </summary>
        /// <returns>�ش� ������ Ŭ������ �ν��� �� �ִ� ItemType ��</returns>
        public ItemType TypeMatchingItemClassToItemType<T>() where T : Item
        {
            ItemType itemType = ItemType.None;

            if( typeof( T )==typeof( ItemWeapon ) )
                itemType = ItemType.Weapon;
            else if( typeof( T )==typeof( ItemMisc ) )
                itemType = ItemType.Misc;
            else if( typeof( T )==typeof( ItemQuest ) )
                itemType = ItemType.Quest;
            else
                throw new Exception("������ Ŭ������ �´� ������ Ÿ���� ���ǵǾ����� �ʽ��ϴ�.");

            return itemType;
        }








        /// <summary>
        /// ���� �� ������ ������ ����ִ� ����Ʈ�� ����ְ� �޼��带 ȣ���ϸ�, �� �κ��丮 ������ ��ųʸ��� ��ȯ���� �����մϴ�.<br/>
        /// �ε� �Ҷ��� ���� ��ȯ���� �� ������ ����Ʈ�� ������ȭ �Ͽ� �ٽ� �κ��丮 ������� ��ȯ�ؾ��ϴ� �뵵�� ����մϴ�.
        /// </summary>
        public void DeserializeItemListToDic<T>( List<T> itemList ) where T : Item
        {               
            if(itemList==null)  // ����Ʈ�� null ���޵Ǿ��ٸ� �����մϴ�.
                return;

            foreach(Item item in itemList) // ������ ����Ʈ���� ���� ������ ������ �ϳ��� �����ɴϴ�.
            { 
                // ���忡 ������Ʈ�� ����� ItemInfo �������� �޽��ϴ�.
                ItemInfo itemInfo = createManager.CreateWorldItem(item);
                                
                // �κ��丮 ���ο� �߰��մϴ�.
                AddItem(itemInfo);
            }





            /**** �ε� ���� �߰��� ����ؼ� �ʱ�ȭ�ؾ� �� (�������� �ʴ�) �Ӽ��� *****/
            /**** ����� �Ӽ��� ���� �ʱ�ȭ ���� �޶����� ������ �� ��� ****/            

            // �� ������ ������Ʈ �� ����
            for(int i=0; i<tabLen; i++)
                tabItemObjCount[i] = InitCurTabItemObjCount( tabType[i] );
        }



        










        /// <summary>
        /// �κ��丮 Ŭ������ ����ȭ���� �����ϱ� ���� �ʿ��� �������Դϴ�.<br/>
        /// InventoryInitializer�� �Է��� DicType[]�� �������� ���� �ʱ�ȭ�� �����մϴ�.
        /// </summary>
        public Inventory( InventoryInitializer initializer )
        {
            // ��� ��Ÿ���� �Ҵ��մϴ�.
            // ������ ������ ���������� ��Ÿ�Ը� �Ҵ��ؾ������� ��ųʸ� ���� �� ���ε����� �߰��� �ƴ�
            // ���� ������ �Ͼ ������ �ս��� �Ͼ �� �����Ƿ� ��� ���� �Ҵ�
            int tabLen = (int)TabType.None;

            tabType = new TabType[tabLen];

            for(int i=0; i<tabLen; i++)
                tabType[i] = (TabType)i;

            slotCountLimitTab = new int[tabLen];    
            tabItemObjCount = new int[tabLen];    





            // �̴ϼȶ��������� ������ ��ųʸ� Ÿ���� �����Ͽ� ���̸� �����մϴ�.
            DicType[] dicTypes = initializer.dicTypes;
            int dicLen = dicTypes.Length;

            // ��ü �迭 ��� �� �Ҵ��մϴ�
            itemDic=new Dictionary<string, List<GameObject>>[dicLen];
            dicType=new ItemType[dicLen];

            // �迭�� ���� ��Ҹ� �Ҵ��մϴ�.
            for( int i = 0; i<dicLen; i++ )
            {
                itemDic[i] = new Dictionary<string, List<GameObject>>();    // ���� ��ųʸ� �Ҵ�
                dicType[i] = dicTypes[i].itemType;                          // ���� Ÿ���� �̴ϼȶ������� Ÿ������ ����
                

                // ������ ������ �ش��ϴ� ��Ÿ�� �ε����� ���մϴ�.
                int tabIdx = GetTabIndex(dicType[i]);

                // ���� ��ųʸ� ���Ѽ��� �ش� ���� ���Ѽ��� ������ŵ�ϴ�.
                slotCountLimitTab[tabIdx] += dicTypes[i].slotLimit;
            }

        }



        /// <summary>
        /// ���� �߿� �ʿ��� ������ȭ ��ų �κ��丮 Ŭ���� �������Դϴ�.<br/>
        /// ���޹��� InventoryInitializer�� createManager�� ���� �κ��丮�� �����ϰ� �ʱ�ȭ�մϴ�.<br/>
        /// </summary>
        public Inventory( InventoryInitializer initializer, CreateManager createManager ) : this(initializer)            
        {            
            // CreateManager �������� ����
            this.createManager = createManager;
        }


               
    }




    

}
