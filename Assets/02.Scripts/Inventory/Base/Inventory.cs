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
        public Dictionary<string, List<GameObject>>[] itemDic;
        public ItemType[] dicType;        
        public int[] slotCountLimitDic;



        /**** �ε� �ÿ��� �ҷ��� �� ���� �߿� ���������� ���� �� �����ؾ� �� �Ӽ� ****/            
       
        /// <summary>
        /// ���� �κ��丮�� ������ ����ִ� ������  ������ ������Ʈ�� ������ ��ȯ�ްų� �����մϴ�.
        /// </summary>
        public int[] CurDicItemObjCount { get; set; }
        

        /**** �ڵ����� �����Ǵ� ���� �Ӽ��� ****/        
        /// <summary>
        /// �÷��̾� �κ��丮�� �� �ǿ� �������� �־����� �ʱ� ĭ�� ���� ���Դϴ�.
        /// </summary>
        public int InitialCountLimit { get{ return 10; } }        


        /// <summary>
        /// �÷��̾� �κ��丮�� ��ü ���� ĭ�� ���� �� �Դϴ�. ���� �߿� ���׷��̵� ������ ���� ������ �� �ֽ��ϴ�.
        /// </summary>
        public int SlotCountLimitAll {
            get{
                    int totalCnt = 0;
                    for(int i=0; i<itemDic.Length; i++)
                        totalCnt += slotCountLimitDic[i]; 
                    return totalCnt;
                } }        
        

        /// <summary>
        /// �κ��丮�� ���� �����ϰ� �ִ� ������ ���̸� ��ȯ�մϴ�
        /// </summary>
        public int dicLen { get { return dicType.Length; } }


        
        /**** ���������θ� ����ϴ� �Ӽ� ****/
        List<int> indexList = new List<int>();
        CreateManager createManager;





        

        /// <summary>
        /// ���޵� ������ ������ �ش��ϴ� ��ųʸ� �ε����� �ִ��� ã�ƺ��� ��ȯ�մϴ�.<br/>
        /// ��ġ�ϴ� ������ ������ ������ -1�� ��ȯ�մϴ�.<br/>
        /// *** ItemType.None�� �����ϰų�, ��ġ�ϴ� ������ ������ ���ٸ� ���ܰ� �߻��մϴ�.
        /// </summary>
        /// <returns>��ġ�ϴ� ������ ������ �ִٸ� �ش� ������ �ε��� ��</returns>
        public int GetDicIdxByItemType(ItemType itemType)
        {
            if(itemType==ItemType.None )
                throw new Exception("������ ������ �߸��Ǿ����ϴ�. �ش� ������ ������ ���� �� �����ϴ�.");

            for( int i = 0; i<dicType.Length; i++ )
            {
                if( itemType==dicType[i] )
                    return i;                    
            }

            // ��ġ�ϴ� ������ ������ ���� ���
            throw new Exception("�� �κ��丮�� ��ġ�ϴ� ������ ������ ������ �����ϴ�.");
        }



        
        
        /// <summary>
        /// ���� �κ��丮�� ����ִ� ������ ������Ʈ�� ������ �����Ͽ� ��ȯ�մϴ�.<br/>
        /// �ʱ� �ε� �� �κ��丮�� �ε� �� ������Ʈ�� ���� �� CurItemCount�� �����ϱ� ���� �ʿ��մϴ�.<br/><br/>
        /// ���ڷ� ItemType�� �����Ͽ� �ش� ItemType�� ��ųʸ����� ������Ʈ ������ ī�����մϴ�.<br/><br/>
        /// *** �ش� ������ ������ ������ ������ ���ܸ� �߻���ŵ�ϴ�. ***
        /// </summary>
        /// <returns>������ Ÿ�԰� ��ġ�ϴ� �κ��丮�� ���� �Ǿ��ִ� ������Ʈ�� ������ ��ȯ�մϴ�.</returns>
        public int GetCurDicItemObjCount(ItemType itemType) 
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
        /// ���� �κ��丮�� ����ִ� ������ ������Ʈ�� ��ü ������ �����Ͽ� ��ȯ�մϴ�
        /// </summary>
        /// <returns>���� �κ��丮�� �����ϴ� ��� �������� �� �����Դϴ�.</returns>
        public int GetCurDicItemObjCountAll()
        {
            int count = 0;
            int dicLen = (int)ItemType.None;

            for(int i=0; i<dicLen; i++)
                count += CurDicItemObjCount[i];

            return count;
        }

        
        /// <summary>
        /// ItemType�� ������� � ������ curItemCount�� ���� �Ǵ� ���� ��ų �� �����ϴ� �޼����Դϴ�.<br/>
        /// inCount�� ������ �����ϸ� �������� ���� ������ ���ҽ�ŵ�ϴ�.<br/>
        /// AddItem�޼��忡�� ���������� ���ǰ� �ֽ��ϴ�.<br/><br/>
        /// *** ���ڷ� ItemType.None�� ������ ��� ���ܸ� �߻���ŵ�ϴ�. ***
        /// </summary>
        public void SetCurDicItemObjCount(ItemType itemType, int inCount)
        {
            if(itemType == ItemType.None )
                throw new Exception("���ڷ� ���� �� ������ ������ �ش� �κ��丮�� ���� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");

            for(int i=0; i<dicLen; i++)
                if( dicType[i] == itemType )
                    CurDicItemObjCount[i] += inCount; 
        }






        /// <summary>
        /// ������ ������ ���� ������ ���� ���� ��ȯ�մϴ�. <br/>
        /// ItemType�� ���ڷ� ���޹޽��ϴ�.<br/>
        /// *** ���ڸ� �� ���� �Ǵ� ItemType.None ���� �� ��ü ���� �ε����� �������� ��ȯ�մϴ�.(�⺻ ��) *** <br/>
        /// </summary>
        /// <returns>���ڷ� ���� �� ������ �ִ� ���� ���� ��ȯ�մϴ�</returns>
        public int GetItemSlotCountLimit(ItemType itemType=ItemType.None)
        {          
            if( itemType==ItemType.None )
                return SlotCountLimitAll;
            else
                return slotCountLimitDic[GetDicIdxByItemType(itemType)];
        }



        /// <summary>
        /// ���� �κ��丮�� ������ ������ ���� �����ִ� ���� ������ ��ȯ�մϴ�.<br/>
        /// *** �������� ������ ItemType.None�� ���޵Ǿ��ٸ� ���ܸ� �߻���ŵ�ϴ�. ***
        /// </summary>
        /// <returns>�����ִ� ������ ������ 0, �����ִ� ������ �ִٸ� �ش� ������ ����</returns>
        public int GetCurRemainSlotCount(ItemType itemType)
        {
            if(itemType==ItemType.None )
                throw new Exception("�߸��� ������ ������ �����Դϴ�.");

            return GetItemSlotCountLimit(itemType) - CurDicItemObjCount[(int)itemType];
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

            // ���� �� ������ ��ϵ� �������� ������ ���Ͽ� �����մϴ�.
            for(int i=0; i<dicLen; i++)
                CurDicItemObjCount[i] = GetCurDicItemObjCount((ItemType)i);
        }



        /// <summary>
        /// �⺻ �κ��丮 �������Դϴ�. ����ȭ ���� �� ���˴ϴ�.<br/>
        /// ���ڸ� �������� ���� �� ��ü ������ ������ ��ųʸ��� �����մϴ�.<br/>
        /// </summary>
        public Inventory()
        {
            // ��� ������ ��������
            int dicLen = (int)ItemType.None;

            // �迭 ���� �Ҵ�
            itemDic = new Dictionary<string, List<GameObject>>[dicLen];
            dicType = new ItemType[dicLen];
            slotCountLimitDic = new int[dicLen];
            CurDicItemObjCount = new int[dicLen];

            // �迭�� ��� ��� �Ҵ�
            for( int i = 0; i<dicLen; i++ )
            {
                itemDic[i] = new Dictionary<string, List<GameObject>>();
                dicType[i] = (ItemType)i;
                slotCountLimitDic[i] = InitialCountLimit;
            }
        }
                
        /// <summary>
        /// �⺻ �κ��丮 �����ڿ��� ���� ���� �߿� �ʿ��� CreateManager �������� �޾� �ʱ�ȭ�� �����մϴ�.<br/>
        /// ������ȭ �ε� �� ���˴ϴ�.
        /// </summary>
        public Inventory(CreateManager createManager) : this() //����Ʈ ������ ȣ��
        {
            this.createManager = createManager;
        }



        /// <summary>
        /// �κ��丮 Ŭ������ ����ȭ���� �����ϱ� ���� �ʿ��� �������Դϴ�.<br/>
        /// InventoryInitializer�� �Է��� DicType[]�� �������� ���� �ʱ�ȭ�� �����մϴ�.
        /// </summary>
        public Inventory( InventoryInitializer initializer )
        {
            // ���޹��� �̴ϼȶ������� DicType �迭�� �����մϴ�.
            DicType[] dicTypes = initializer.dicTypes;

            // ���� ���̴� ���� ������ �����Դϴ�
            int dicLen = dicTypes.Length;
            
            // �迭 ���� �Ҵ�
            itemDic = new Dictionary<string, List<GameObject>>[dicLen];
            dicType = new ItemType[dicLen];
            slotCountLimitDic = new int[dicLen];
            CurDicItemObjCount = new int[dicLen];
           
            // �迭�� ��� ��Ҹ� ���� ���ڸ� ���� �Ҵ��մϴ�
            for( int i = 0; i<dicLen; i++ )
            {
                itemDic[i] = new Dictionary<string, List<GameObject>>();
                dicType[i] = dicTypes[i].itemType;
                slotCountLimitDic[i] = dicTypes[i].slotLimit;
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
