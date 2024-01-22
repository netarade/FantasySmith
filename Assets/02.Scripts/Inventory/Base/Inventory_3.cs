using ItemData;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/*
 * <v1.0 - 2023_1231_�ֿ���>
 * 1- FindNearstSlotIdx itemName�� itemType�� ���ڷ� �޴� �����ε� �޼��� ����
 * 
 * <v1.1 - 2024_0102_�ֿ���>
 * 1- ������ ���� �˻���� 
 * IsEnoughOverlapCount �޼��� �ۼ�
 * 
 * 2- ������ ���� ���� �� ��Ͽ��� ���� ��� 
 * IsEnoughOverlapCount �޼��忡 �������Ҹ�� �߰� �� SetOverlapCount �޼��� �ۼ�
 * 
 * (���� ���� ����_0102)
 * 1- �̸��� ������ ����ü ���·� ���� �迭�� �����ϴ� �����ε� �޼��� �����ϱ�
 * 
 * 
 * <v2.0 - 2024_0103_�ֿ���>
 * 1- IsEnoughOverlapCount�޼��� �����������ڿ� ���� ����ó�� �߰� 
 *  
 * 2- IsExist �߰�
 * ����ȭ �������� �����ϴ��� ���θ� ��ȯ�ϴ� �޼���, �������� ����
 * 
 * 3- ItemPair ����ü �߰�
 * ���ڷ� �̸��� ���� ���� ������ �� �ֵ��� ����
 * 
 * 4- ItemPair�� ItemPair�迭�� ���ڷ� �޴� IsEnough�޼��� ����
 * ���ڷ� ������ �������� �ִ���, �������� ��������� �˻��ϰ� �������� �Ǵ� ������ ���ֵ��� ����
 * 
 * (�̽�)
 * 1- IsEnough, IsEnoughOverlapCount, IsExist �޼����� ��ȯ���� ItemInfo�� �����ؾ��Ѵ�
 * ������ ������ 0�̵� ������ ������Ʈ�� InfoŬ�������� ����ó���ؾ� �ϱ� ����
 * 
 * <v2.1 - 2024_0103_�ֿ���>
 * (�̽�_0103) 
 * 1- SetOverlapCount�޼��忡��
 * ������Ʈ����Ʈ���� �ϳ��� �������� �о�ͼ� ���� ������ �����Ű�� �ִµ�,
 * �κ��丮 ���� �� ������Ʈ ����Ʈ ������ �Ͼ�� ������ �ִ�.
 * => ������ �������� ��Ƽ� �ѹ��� ����Ʈ���� �����ϴ� ���·� �����ؾ�
 * 
 * 
 * (�̽�_0104)
 * 1- ������ �� �����ؼ� �����ؾ� �ϴµ� ���� �� ������Ʈ ������ �о���̰� �ִ�.
 * ������, ���� ���ӿ����� slotIndex������ �����ؾ� �ϹǷ�, slotIndex�� ������� �����ؼ� ��������� �Ѵ�.
 * 
 * <v3.0 - 2024_0104_�ֿ���>
 * 1- InventoryInfo�� ������ �����͸� �����ϱ� ���� passItemInfoList�� �����ϰ�
 * SetOverlapCount�޼��忡�� ���������� ������ �������� �Ͻ����� ��Ƽ� �����ϴ� �������� �����Ͽ���.
 * 
 * �޼��� ���ο��� �������� �ʰ� InfoŬ������ �����ϴ� ������ ���������� InventoryŬ������ ���������, 
 * ������Ʈ ������ InventoryInfoŬ�������� �Ѱž� �ϱ� �����̸�,
 * ������ �ٽ� �ǵ����� �۾��� �ʿ��� ���� �ֱ� ���� 
 * (ex. ��ȭ������ 50���� �Ծ��µ� ������ ������� �ʴٸ�, ������ �����ۿ� �� �������� �˾ƾ� �Ѵ�.)
 * 
 * 2- �������� ������ isLatestRemove, Reduce���� isLatestModify�� ����
 * 
 * 3- IsAbleToAddMisc�޼��� �߰�
 * IsEnough, IsExist ���� ���ҳ� ���Ÿ� �������� ������ 
 * �� �޼���� �߰� �ϱ����� ���� ������ ���� �������, �׸��� ���ο� �������� �����ϱ� ���� ���԰����� �ִ��� ���θ� ��ȯ�ޱ� ����.
 * 
 * 
 * <v3.1 - 2024_0104_�ֿ���>
 * 1- IsAbleToAddMisc�޼��忡�� ���ɿ��θ� ��ȯ�ؾ��ϴµ� ������Ʈ����Ʈ�� ���� �����ϰ��־��� �� ����
 * 2- ���ı��� �޼��� ������ CompareByIndex�� CompareBySlotIndexEach�� ����
 * 3- SetOverlapCount�޼��� while���� ���� ��������, ���������� �����ϴ� ������ ���� �޼��带 ���� for�� ȣ��� ����
 * 4- SetOverlapCount �޼����� �Ű����� passList�� removeList�� �����ϰ� ������ ���ڷ� ����
 * 
 * 
 * <v3.2 - 2024_0105_�ֿ���>
 * 1- FindNearstSlotIdx�ڵ� ���� ����
 * a. �������� �ƹ��͵� ������� ������ �ʱⰪ�� 0���� �ְ��Ͽ���.
 * b. for�� ���� break�� �߰� (Ÿ���ε����� ã���� ���̻� ���������� �ؾ���)
 * c. �ε��� ����Ʈ�� �������� ���������鼭 ������ ���� �����ִ� ���, ���� �ε����� Ÿ������ �����ϴ� ���� �߰�
 * 
 * <v3.3 - 2024_0108_�ֿ���>
 * 1- IsAbleToAddMisc�޼��� ���� �������� ���� �����ѵ��� �Ұ����� ��ȯ�ϴ� �ڵ带 ����
 * reamainCount>0�϶� ���԰����� �����Ҷ� false�� ��ȯ�ϰ�, remainCount<=0 ������ �� ������ true�� ��ȯ�Ͽ��� ������ false�� ��ȯ�Ͽ��� �� ����
 * �߰��� remainCount�� Ž�� ���� �� 0���Ϸ� �������ٸ� �ٷ� true�� ��ȯ�ϵ��� ����
 * 
 * <v3.4 - 2024_0110_�ֿ���>
 * 1- IsExist�� �κ��丮 ���ſɼ�(isReduce)�� �����ϰ� ������Ʈ ����(itemObjCount) �������� �߰�, �ּ��� �׿��°� ����
 * ������ ������ ȥ�ո޼��带 ���̰� ������Ʈ �˻��̶�� ������ ������ ������ �� ���߱� ����.
 * 
 * <v3.5 - 2024_0111_�ֿ���>
 * 1- ����Ʈ ������ Ŭ���� �߰��� ���� 
 * FindNearstSlotIdx�޼��忡 �������� �о���� ���� ���̸� ����
 * 
 * <v3.6 - 2024_0112_�ֿ���>
 * 1- Inventory�� ������������ �����迭�� ������ ������ ����� FindNearstSlotIdx�޼����� ����Ʈ������ ���� ����
 * 
 * <v4.0 - 2024_0114_�ֿ���>
 * 1- FindNearstSlotIdx�޼��忡�� �ε�������Ʈ�� ���ϴ� �κ��� GetSlotIndexList �޼���� ����� ����
 * 
 * 2- FindNearstSlotIdx�޼��忡�� dicLen�� (int)ItemType.None�� ���ϴ� ���� Inventory�� dicLen���� ���� ��
 * itemDic�� ���� �� (ItemType)i�� �̿��ؼ� ���ϴ� ���� dicType[i]�� ����
 * 
 * 3- IsRemainSlot�޼��� ����
 * ������ �ε����� ������ �����ִ��� ���θ� ��ȯ�ϴ� �޼���� GetSlotIndexList�޼��带 ���������� ���
 * 
 * 4- ReadSlotIdxToIndexList �޼������ ReadSlotIndexByItemDic���� ����
 * FindNearstSlotIdx�޼������ GetLatestSlotIndex�� ����
 * ������ findSlotIdx�� latestSlotIndex�� ����
 * 
 * 
 * (�̽�)
 * 1- IsRemainSlot���� ���߿� ��Ÿ���� ���ڷ� �޾ƾ���.
 * (������ �ǻ󿡼��� ������ġ�� �ǹ��ϱ� ����)
 * => ���� ���� ���� ����
 * 
 * <v4.1 - 2024_0115_�ֿ���>
 * 1- GetDicIndex�޼��带 Inventoy.cs�� �ű�.(GetDicIdxByItemType�� �ߺ����)
 * 
 * <v4.2 - 2024_0115_�ֿ���>
 * 1- GetSlotIndexList�޼��带 
 * a. ���� ������ ������ SlotIndex�� Index�� Add��Ű�� �κ��� Tab�� �ش��ϴ� ��� ������ ������ Index�� Add��ų �� �ֵ��� ����
 * b. ItemType.None �������ڸ� �����ϰ� ����� ���ڷ� ��ȯ
 * 
 * 
 * 2- ReadSlotIndexByItemDic�޼��� ���� ReadSlotIndexFromItemDic���� ����
 * 
 * 3- GetLatestSlotIndex(string itemName, bool isIndexAll) �����ε� �޼��� ����
 * (ItemType�� ���ڷ� ���޹޴� �޼��常 �ַ� ����ϹǷ�)
 * 
 * 4- GetLatestSlotIndex�޼����� ���� GetSlotCountLimitTab�� �°� ����
 * 
 * 5- ������ ItemType�� slotIndex�� ���ڷ� �޴� IsRemainSlot �޼��带 IsRemainSlotCertain���� �����ϰ� 
 * ItemType�� ���ڷ� �޴� IsRemainSlotNearst�޼��� �ۼ�
 * (���� ���� �ڸ��� �ִ��� ���θ� ��ȯ�ϴ� �޼���� �ƹ� ���� �ڸ��� �ִ��� ���θ� ��ȯ�ϴ� �޼���� ��������)
 * 
 * 6- �ϰ����� ���� GetLatestSlotIndex�޼������ GetItemSlotIndexNearst�� ����
 * 
 * <v5.0 - 2024_0116_�ֿ���>
 * 1- IsRemainSlotCertain, GetSlotIndexList, GetItemSlotIndexNearst �޼��忡 
 * ���� �� ���� isActiveTabAll�� �޵��� ����, ItemType.None ���� �� ���ܸ� ó���Ͽ���
 * (������ �� ���¿����� ������ �޶����� �ϱ� �����̸�, ���º����� �ϳ� �� �޾Ƽ� �ڵ忡 �ϰ����� �ֱ� ����)
 * 
 * 2- �޼���� ���� IsRemainSlotNearst, IsRemainSlotCertain, GetItemSlotIndexNearst
 * -> IsRemainSlotIndirect, IsRemainSlotDirect, GetItemSlotIndexIndirect
 * 
 * 3- GetSlotIndexList�޼��忡�� isActiveAll�� ���� tabType�� ��ü������ ���ؾ�������
 * ����ؼ� ���������� �������μ� �ε�������Ʈ�� ���� �ε����� �޾ƿ��� �� ����
 * 
 * <v5.1 - 2024_0116_�ֿ���>
 * 1- GetSlotIndexList�޼��忡�� GetItemDic���� null�˻繮 �߰�
 * 2- IsRemainSlotDirect�޼��� ���� �ٸ� �޼���� ����ó���� �߰�
 * 3- IsRemainSlotDirect�޼��忡�� �����ε����� �������Ѽ��� �Ѿ�� ��� ���ܸ� ó���ϴ� �κ���
 * false�� ��ȯ�ϵ��� ����
 * ( �ε����� ���� �� �̻����� �Ҵ��Ϸ��ϴ� ��� ����ó�� )
 * 
 * <v5.2 - 2024_0118_�ֿ���>
 * 1- GetItemSlotIndexIndirect�޼��� ���� printDebugInfo�ּ�ó��
 * 
 * <v5.3- 2024_0122_�ֿ���>
 * 1- IsEnough�޼��忡�� ó�� ������ ���翩�θ� �˻��� �� ���ڷ� ���� count�� �־ IsExist�� ȣ���ϰ� �־��� ������
 * ��ȭ�������� ��� ����� �˻簡 ���� �ʾҴ� ������ ����
 * (=> ���� count�� ���� �ʰ� �����ϴ��� �˻� �� �Ŀ� 
 * ��ȭ�������� ��� IsEnoughOverlaCount��, ����ȭ�� ��� IsExist�� count�־ �� �� �� �˻������ ����)
 * 
 * <v5.4 - 2024_0123_�ֿ���>
 * 1- IsEnough, IsEnoughOverlapCount, SetOverlapCount����
 * GetItemTypeInExists�޼��带 GetItemTypeIgnoreExists�� ����
 * 
 * �̸��� ������� Ÿ���� ã�� ���� InExists�� �ʿ���µ� �̴�
 * �������� Add�� �� ���� �������� �ִ��� ���ΰ� �߿����� ������,
 * �ش� �̸��� ������ ������ ���� ���ǰ˻繮�� ���� ��찡 ���� ����.
 * 
 * 2- IsEnoughOverlapCount���� itemObjList�� null�� �˻縦 ���ܷ� ó���ϰ� �־��µ�
 * �̴� ������������ ����ó�� ������ �ƴϾ���. ���� falseó��
 * (�ش� �̸��� �������� ���� ���¿��� �� �޼��� �ߵ� �� ���ܰ� �߻����� ���ɼ��� ŭ)
 * 
 */


namespace InventoryManagement
{

    /// <summary>
    /// ������ �̸��� ������ ��Ÿ���� ����ü�Դϴ�.<br/>
    /// �κ��丮 �޼����� �������ڷ� ���Ǹ�,<br/>
    /// �ش� �������� �̸��� �κ��丮�� �����ϴ� ��, ������ ������ ����� �� Ȯ���ϴ� �뵵�� ���˴ϴ�.
    /// </summary>
    [Serializable]
    public struct ItemPair
    {
        public string itemName;
        public int overlapCount;

        public ItemPair(string itemName, int overlapCount)
        {
            this.itemName = itemName;
            this.overlapCount = overlapCount;
        }

    }




    public partial class Inventory
    {
        /// <summary>
        /// InventoryInfoŬ������ �ʿ� �� ������ ������ �����ϱ� ���� ����ϴ� ItemInfo�� �ӽ������� ��� ����Ʈ�Դϴ�.<br/>
        /// ���� ���۸���Ʈ �� ���� �� ��ü���ڷ� SetOverlapCount�޼��忡�� ���������� ���˴ϴ�.
        /// </summary>
        List<ItemInfo> tempInfoList = new List<ItemInfo>();



        /// <summary>
        /// �ش� ������ �������� �� �ƹ� ������ ������� ���θ� �Ǵ��մϴ�.<br/>
        /// ���ڷ� � ������ ���������� ���޹޽��ϴ�.<br/><br/>
        /// *** ItemType.None�� �����ϸ� ���ܸ� �����ϴ�. ***
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public bool IsRemainSlotIndirect( ItemType itemType )
        {
            // ��ü �ǿ� �� �ڸ��� �־ Ư�� �ǿ� �� �ڸ��� ������ �ȵǱ� ����
            if(itemType == ItemType.None)
                throw new Exception("�ش� ������ ���ڸ� ������ �� �����ϴ�.");

            if( GetCurRemainSlotCount(itemType)>0 )
                return true;

            return false;
        }



        
        /// <summary>
        /// �ش� ������ �������� �� Ư�� ������ ������� ���θ� ��ȯ�մϴ�.<br/>
        /// ���ڷ� � ������ ����������, �ش��ϴ� ������ �ε������� ��ü �� ���¸� ���� �޽��ϴ�.<br/>
        /// *** ItemType.None�� ���޵Ǹ� ���ܰ� �߻��մϴ�. ***
        /// </summary>
        /// <returns>�ش� ���� ���Կ� �������� �� ������ �ִٸ� true, ���ٸ� false�� ��ȯ</returns>
        public bool IsRemainSlotDirect(ItemType itemType, int slotIndex, bool isActiveTabAll)
        {
            if(itemType==ItemType.None)
                throw new Exception("��Ȯ�� ������ Ÿ���� �ʿ��մϴ�.");
            if(slotIndex<0)
                throw new Exception("���� �ε����� ��Ȯ���� �ʽ��ϴ�. 0���ϰ� ���޵Ǿ����ϴ�.");
            if(indexList==null)
                throw new Exception("���� ����Ʈ�� �������� �ʽ��ϴ�.");  

            // ���� ���� �ε����� ���Ѽ��� ������ ��� �ڸ��� �����Ƿ� false�� ��ȯ
            if(slotIndex >= GetSlotCountLimitTab(itemType, isActiveTabAll))
                return false;


            // �ε��� ����Ʈ�� �����Ͽ�, �ش��ϴ� ������ ������ ���� �ε��� ����Ʈ�� ���մϴ�.
            GetSlotIndexList( ref indexList, itemType, isActiveTabAll );
                        
            // ���� �ε��� ����Ʈ�� �ƹ��͵� ���� ��� �ڸ��� �����Ƿ� �ٷ� true�� ��ȯ�մϴ�
            if( indexList.Count==0 )    
                return true;

            // ������������ �����մϴ�.
            indexList.Sort();

            int idx;

            // �ε��� ����Ʈ�� ��� �о�鿩 slotIndex�� �����ϴ� �� ã���ϴ�.
            for( idx = 0; idx<indexList.Count; idx++ )
            {
                // ���� �������� ���ĵ� �ε��� ����Ʈ�� ���� ���� ���� �ε������� ũ�ٸ� (���ڸ��� �����Ƿ�) true�� ��ȯ�մϴ�.
                if( indexList[idx] > slotIndex )
                    return true;

                // �ε��� ����Ʈ�� �ش��ϴ� ���� �ε����� �����Ѵٸ� (������ �������� �ڸ��� ���� �ϰ� �����Ƿ�) false�� ��ȯ�մϴ�.
                if( indexList[idx] == slotIndex)
                    return false;
            }

            // idx�� �������Ѽ��� �����ߴٸ� false�� ��ȯ�մϴ�.
            if( idx == GetSlotCountLimitTab(itemType, isActiveTabAll) )
                return false;
            // �ε��� ����Ʈ���� slotIndex�� ã�� ���ߴٸ� (�������ѿ� �������� ���������̱⿡ ���ڸ��� �����Ƿ�) true�� ��ȯ�մϴ�.
            else
                return true;
        }







        /// <summary>
        /// ������ ������ ������ ���� ������ �����۵��� ��ųʸ��� �о�鿩 
        /// ������ ������ indexList�� �������� ���� �ε����� ����ϴ�.<br/><br/>
        /// ** �޼��� ȣ�� �� �ε��� ����Ʈ�� �ʱ�ȭ�Ǹ�, ���ĵ��� ���� ���·� ��ȯ�˴ϴ�. **<br/>
        /// ** �ι�° ���ڰ� ItemType.None���� ���޵Ǿ��ٸ� ���ܰ� �߻��մϴ�. **
        /// </summary>
        /// <param name="indexList"></param>
        /// <param name="itemType"></param>
        private void GetSlotIndexList( ref List<int> indexList, ItemType itemType, bool isActiveTabAll )
        {
            if(itemType == ItemType.None )
                throw new Exception("��Ȯ�� ������ Ÿ���� �ʿ��մϴ�.");
            if(indexList==null)
                throw new Exception("���� ����Ʈ�� �������� �ʽ��ϴ�."); 

            // ������ �ε��� ����Ʈ�� �ʱ�ȭ�մϴ�.
            indexList.Clear();   

            // �������� ItemType�� ������� �ش��ϴ� ��ųʸ��� ���մϴ�
            Dictionary<string, List<GameObject>> itemDic;


            TabType tabType;  

            // ��ü���� Ȱ��ȭ�Ǿ����� �ʴٸ�, ���ڷ� ���޹��� ItemType�� �ش��ϴ� TabType�� ���� ���մϴ�.
            if( !isActiveTabAll )
                tabType = ConvertItemTypeToTabType( itemType );            
            // ��ü ���� Ȱ��ȭ�Ǿ� �ִٸ� tabType�� ��ü�� �����մϴ�.
            else
                tabType = TabType.All;
            
            
            // �ش� TabType�� ���ϴ� ItemType ����Ʈ�� ����Ʈ�� ���̸� ��ȯ�޽��ϴ�.
            int tabKindLen = ConvertTabTypeToItemTypeList( ref tabKindList, tabType );


            // ����Ʈ�� ���̸�ŭ ��ȸ�մϴ�
            for(int i=0; i<tabKindLen; i++)
            {
                // ������ ���� �� ������ �ϳ��� �����ɴϴ�.
                itemDic = GetItemDic( tabKindList[i] );
                
                // ��ųʸ��� �ƹ� �������� ������� �ʴٸ� �������� �Ѿ�ϴ�.
                if(itemDic==null || itemDic.Count==0)
                    continue;

                // ������Ʈ�� �ϳ��� �о�鿩�� ������ �ε����� �ε��� ����Ʈ�� ����ֽ��ϴ�.
                ReadSlotIndexFromItemDic(ref indexList, itemDic, isActiveTabAll );
            }
        }


        
        /// <summary>
        /// ���� �ε����� �ش� ��ųʸ��κ��� �ϳ��� �о�鿩 �����մϴ�.<br/>
        /// ���ڷ� �ε��� ����Ʈ �������� � ��ųʸ��� �߰��� ������, ���� �� ���°� ��ü������ ���θ� �����ؾ� �մϴ�.
        /// </summary>
        private void ReadSlotIndexFromItemDic(ref List<int> indexList, Dictionary<string, List<GameObject>> itemDic, bool isActiveTabAll )
        {   
            if( indexList==null || itemDic==null )
                throw new Exception("����Ʈ Ȥ�� ��ųʸ� �������� �������� �ʽ��ϴ�.");
                

            // �ش� �������� ������ ������ �ϳ��� �о�ɴϴ�.
            foreach(List<GameObject> itemObjList in itemDic.Values)     
            {
                foreach(GameObject itemObj in itemObjList)
                {
                    Item item = itemObj.GetComponent<ItemInfo>().Item;   

                    if( isActiveTabAll )  
                        indexList.Add(item.SlotIndexAll);   // ��ü ���� �ε��� ������� ���ϴ� ���
                    else                
                        indexList.Add(item.SlotIndexEach);  // ���� ���� �ε��� ������� ���ϴ� ���
                } 
            }
        }
        



















        /// <summary>
        /// ���� ����� ������ �ε����� ���մϴ�.<br/>
        /// � ������ �������� ���� �������� ���� �� ���¸� ���ڷ� �����Ͽ��� �մϴ�.<br/>
        /// ��ü�ǻ��¶�� ��ü ���� �ε�����, ������ ���¶�� ���� ���� �ε����� ��ȯ�޽��ϴ�.<br/><br/>
        /// *** ItemType.None�� ������ ��� ���ܰ� �߻��մϴ�. ***
        /// </summary>
        /// <returns> �������� �� �ǿ����� ���� �ε����� ��ȯ, ���Կ� �ڸ��� ���ٸ� -1�� ��ȯ</returns>
        public int GetItemSlotIndexIndirect(ItemType itemType, bool isActiveTabAll)
        {
            // �ε�������Ʈ�� �����Ͽ� ������ ����ִ� ��� �������� �ε����� �о���Դϴ�.
            GetSlotIndexList(ref indexList, itemType, isActiveTabAll);

            indexList.Sort();   // �ε��� ����Ʈ�� ������������ �����մϴ�.

            // �ε��� ����Ʈ�� �������� �ƹ��͵� ���� ��� 0�� ��ȯ
            if( indexList.Count==0 )            
                return 0;
                        
            int latestSlotIndex = -1;    // ã�� ���� �ε����� �����ϰ� �ʱⰪ�� -1���� �����մϴ�.

            // ���ڷ� ������ �������� ������ �ش��ϴ� ���� ������ ĭ ���� ���� ���մϴ�.
            int slotCountLimitTab = GetSlotCountLimitTab(itemType, isActiveTabAll); 

            //string debugInfo= "";
            //debugInfo += string.Format($"������ ���� : {itemType}\n");
            //debugInfo += string.Format($"��ü �� ���� : {isActiveTabAll}\n");
            //debugInfo += string.Format($"�� ���� �� : {slotCountLimitTab}\n");
            //Debug.Log(debugInfo);

            // ���� �ε��� ���� 
            // 0, 1, 2, 3, 4
            // 0, 1, 4, 6, 9
            

            // �ε��� ���ڱ��� i�� 0���� ������Ű�鼭 �� ������ ã���ϴ� 
            for(int i=0; i<slotCountLimitTab; i++)
            {
                // i��° �ε�������Ʈ�� ����� �ε����� i�� ��ġ���� �ʴ´ٸ� ������ �� ������ �Ǵ�
                if( indexList[i]!=i )
                {
                    latestSlotIndex = i;
                    break;
                }
                //�ε��� ����Ʈ�� �������� ���������鼭 ������ ���� �����ִ� ���
                else if( i==indexList.Count-1 && i!=slotCountLimitTab-1 ) 
                {
                    latestSlotIndex = i+1; // ���� �ε����� Ÿ������ ����
                    break;
                }
            }

            //ã�� ���� �ε����� ��ȯ�մϴ�.
            return latestSlotIndex;
        }

















        /// <summary>
        /// ����Ʈ�� Sort�޼��忡 �ε��� ���ı����� �����ϱ� ���� ���� �޼���<br/>
        /// ��ȭ�������� ���� ���� �ε����� �������� �ؼ� ������������ ���ĵ˴ϴ�.<br/>
        /// </summary>
        public static int CompareBySlotIndexEach(GameObject itemObj1, GameObject itemObj2)
        {
            ItemMisc itemMisc1 = (ItemMisc)itemObj1.GetComponent<ItemInfo>().Item;
            ItemMisc itemMisc2 = (ItemMisc)itemObj2.GetComponent<ItemInfo>().Item;
            return itemMisc1.SlotIndexEach.CompareTo(itemMisc2.SlotIndexEach);
        }


        /// <summary>
        /// �κ��丮�� �����ϴ� �ش� �̸��� ������ ������ ������Ű�ų� ���ҽ�ŵ�ϴ�.<br/>
        /// ���ڷ� �ش� ������ �̸��� ����, ���� ��� �������� �������� ���� ���� ����Ʈ, ��������� �����ؾ� �մϴ�.<br/><br/>
        /// 
        /// ������ ������ ���ҽ�Ű���� ���� ���ڷ� ������ ����, ������Ű���� ����� �����մϴ�.<br/>
        /// �� �������� �ּ�, �ִ� ������ �����ϸ� ���� �������� ������ ������ �����մϴ�.<br/><br/>
        /// 
        /// ���� �������� ������ ���ҷ� ���� 0 �̵Ǹ� �������� �κ��丮 ��Ͽ��� �����ϰ� ���� ����Ʈ�� �������� �߰��մϴ�.<br/>
        /// ���� ���۸���Ʈ�� null�� �����ϸ�, �������� �������� �ʰ� ������Ʈ�� ��� �����մϴ�.(�⺻��:null)<br/>
        /// ���� ������ ���� �κ��丮 ��Ͽ��� �����ϰų�, ���۸���Ʈ�� �߰����� �ʽ��ϴ�.<br/><br/>
        /// 
        /// ��� ���� �������� �� �̻� ������ �������� ���ϴ� ���� ������ �ʰ� ������ ��ȯ�մϴ�.<br/>
        /// �ʰ� �������� ���� ������Ʈ ���ο� ������ ���� ���ҷ� ���� ���� ������Ʈ ������ �޼��� ȣ���ڿ��� ������ �ֽ��ϴ�.<br/><br/>
        /// �ֽ� ��, ������ ������ ���� ���� ����� ������ ���ֽ��ϴ�. (�⺻��: �ֽż�)<br/><br/>
        /// ** ������ �̸��� �ش� �κ��丮�� �������� �ʰų�, ��ȭ�������� �ƴ� ��� ���ܸ� �߻���ŵ�ϴ�. **
        /// </summary>
        /// <returns>������ ������Ʈ�� ���� Ȥ�� �߰��ϰ� ���� �ʰ� ����, ��� ���� �����ۿ� �ش� ������ ���ٸ� 0�� ��ȯ</returns>
        public int SetOverlapCount(string itemName, int inCount, bool isLatestModify=true, List<ItemInfo> removeList=null)
        {
            List<GameObject> itemObjList = GetItemObjectList(itemName);     // �κ��丮�� ������ ������Ʈ ����Ʈ ����
            ItemType itemType = GetItemTypeIgnoreExists(itemName);              // ������ ���� ����
                        
            // �ش� �̸��� ������ ������Ʈ�� �������� �ʴ� ���, �������� ������ ��ȭ�������� �ƴ� ��� ����ó��
            if(itemObjList==null)
                throw new Exception("�������� �� �κ��丮�� �������� �ʽ��ϴ�.");
            if(itemType != ItemType.Misc)
                throw new Exception("��ȭ �������� �ƴմϴ�.");
            

            bool isInstantRemove = false;       // ��� �������� �Ǵ� ����

            // ���ڷ� ���޹��� ���� ����Ʈ�� null�̶�� ��� ���� Ȱ��ȭ
            if(removeList==null)
            {
                isInstantRemove = true;       
                removeList = this.tempInfoList;   // �ӽø���Ʈ�� ����
            }

            int remainCount = inCount;          // ���� �Ǵ� �����ϰ� ���� ���� (�ʱⰪ : ���� ��������)
            
            // ���� ���� �ε����� ���������Ͽ� ������������ �����մϴ�.
            itemObjList.Sort(CompareBySlotIndexEach);


            // �ֽż����� ������ �� ������ ������ ������ ���� �Ǵ��Ͽ� ���� �޼��带 �ݺ�ȣ���մϴ�.            
            if(isLatestModify)  
            {
                for(int i=itemObjList.Count-1; i>=0; i--)
                    if( SetCountInLoopByOrder(i) ) { break; }   // ȣ�� ������ Ȱ��ȭ�Ǹ� �ݺ����� �����մϴ�.
            }
            else                
            {
                for(int i=0; i<=itemObjList.Count-1; i++)
                    if( SetCountInLoopByOrder(i) ) { break; }   // ȣ�� ������ Ȱ��ȭ�Ǹ� �ݺ����� �����մϴ�.
            }


            // ���� ����Ʈ�� �ִ� �������� �������� ��ȸ�մϴ�. 
            for( int i=tempInfoList.Count-1; i>=0; i--)
            {
                // �κ��丮 ����Ʈ���� ����
                RemoveItem(tempInfoList[i]);
                                
                // ��� ������ Ȱ��ȭ ���ִٸ� ������Ʈ ����
                if(isInstantRemove)
                    GameObject.Destroy(tempInfoList[i].gameObject);
            }

            // ��� ������ ��� ���۸���Ʈ �ʱ�ȭ
            if(isInstantRemove)
                tempInfoList.Clear();   

            // ȣ���ϰ� ���� ������ ��ȯ�մϴ�.
            return remainCount; 
            



            // �������� ��ȸ ������ ���� �ϳ��� ������ ���� ������ �����ϰ�,
            // ������ 0�̵� �������� tempInfoList�� ����ִ� ���� �޼����Դϴ�.
            bool SetCountInLoopByOrder(int idx)
            {
                // �ݺ����� �� ������ ������ ���� ������ ���� ����
                ItemInfo itemInfo=itemObjList[idx].GetComponent<ItemInfo>();
                ItemMisc itemMisc=(ItemMisc)itemInfo.Item;

                // ���������� �־ ���ο� ���� ������ ��ȯ�޽��ϴ�.
                remainCount=itemMisc.AccumulateOverlapCount( remainCount );

                // ������ ���� ������Ʈ�� ��ø �ؽ�Ʈ�� �����մϴ�.
                itemInfo.UpdateTextInfo();

                // ������ �������� ������ 0�� �� ��쿡�� ���� ����Ʈ�� ����ϴ�.
                // �ٷ� �������� �ʰ� ���� ��� ������ �߰��� ����Ʈ�� Count ������ ����� �����Դϴ�.
                if( itemMisc.OverlapCount==0 )
                    tempInfoList.Add( itemInfo );

                // ���� ������ 0�̵� ��� �ܺ� ȣ�� ������ Ȱ��ȭ�մϴ�.
                if(remainCount==0)  
                    return true;
                else
                    return false;
            }

        }

        

        /// <summary>
        /// �κ��丮�� �ش� �̸��� �������� ��ø������ ��������� Ȯ���ϴ� �޼����Դϴ�.<br/>
        /// ���ڷ� ������ �̸��� ������ �ʿ��մϴ�. (���� ������ �� �⺻ ������ 1���Դϴ�.)<br/><br/>
        /// ���� ° ���ڷ� ���� ���Ҹ�带 �����ϸ� �������� ��ø������ ����ϴٸ� ���ڷ� ���� ������ŭ ���ҽ�Ű��, <br/>
        /// 0�� ������ ��� �������� �κ��丮���� �����ϰ� �ı� ��ŵ�ϴ�.<br/>
        /// �ֽ� ��, ������ ������ ���ҿ��θ� ������ ���ֽ��ϴ�. (�⺻��: �ֽż�)<br/><br/>
        /// ** �ش� �̸��� �������� �������� �ʰų�, ��ȭ �������� �ƴϰų�, ������ �߸� �����ߴٸ� ���ܸ� �߻���ŵ�ϴ�. **
        /// </summary>
        /// <returns>������ ��ø������ ����ϸ� true��, ������� ������ false�� ��ȯ</returns>
        public bool IsEnoughOverlapCount(string itemName, int overlapCount=1, bool isReduce=false, bool isLatestModify=true)
        {           
            ItemType itemType = GetItemTypeIgnoreExists(itemName);              // �̸��� ���� �������� ���� ����
            
            
            if( itemType != ItemType.Misc)
                throw new Exception("�ش� �������� ��ȭ�������� �ƴմϴ�.");
            else if( overlapCount<=0 )
                throw new Exception("���� �������ڴ� 1�̻��̾�� �մϴ�.");
            

            List<GameObject> itemObjList = GetItemObjectList(itemName);     // �̸��� ���� ������ ������Ʈ ����Ʈ ����

            // �ش� ������ �̸����� ������Ʈ ����Ʈ�� �������� �ʰų�, Ű���� ������ ����ٸ� ����ó��
            if( itemObjList == null || itemObjList.Count==0 )
                return false;
                        
                                    
            int totalCount = 0;                         // ��ø������ ���� ��Ű�� ���� ���� ����
            bool isTotalEnough = false;                 // ��ø������ ������� Ȯ���ϱ� ���� ����

            foreach(GameObject itemObj in itemObjList)  // �������� �ϳ��� ������ ������ �н��ϴ�.
            {
                ItemMisc itemMisc = (ItemMisc)itemObj.GetComponent<ItemInfo>().Item;
                totalCount += itemMisc.OverlapCount;    // ��ø������ ������ŵ�ϴ�.
                   
                // �հ� ���� ������ ���� ������ ���ڷ� ��� �� �������� ū�� Ȯ���մϴ�.
                if(totalCount >= overlapCount)  
                {
                    isTotalEnough = true;    // isTotalEnough�� true�� ����� ���������ϴ�.
                    break;
                }
            }

            if(isTotalEnough)       // �հ� ���� ������ ���ڷ� ���� ������ �ʰ��ϴ� ���
            {
                if( isReduce )      // ���� �����, ��� ���θ� ��ȯ�ϱ� ���� �ش� ������ŭ ���ҽ�ŵ�ϴ�.
                    SetOverlapCount(itemName, -overlapCount, isLatestModify, null);

                    return true;
            }
            // �հ� ���� ������ ���ڷ� ���� ������ �ʰ����� ���ϴ� ��� ���и� ��ȯ�մϴ�.
            else
                return false;
        }



        /// <summary>
        /// �������� �κ��丮�� �����ϴ��� ���θ� ��ȯ�մϴ�.<br/>
        /// �������� ������ ������� ������Ʈ ������ �����ϴ��� ���θ� ��ȯ�մϴ�.<br/>
        /// ���������� �⺻ ���� 1�Դϴ�.<br/><br/>
        /// 
        /// *** �������ڰ� 0���Ϸ� ���޵� ��� ���ܸ� �߻���ŵ�ϴ�. *** <br/>
        /// </summary>
        /// <returns>�������� �����ϴ� ��� true��, �������� �ʰų� ������ ������� ������ false�� ��ȯ</returns>
        public bool IsExist(string itemName, int itemObjCount=1)
        {
            if(itemObjCount<=0)
                throw new Exception("������ 0���Ϸ� ���޵Ǿ����ϴ�. Ȯ���Ͽ� �ּ���.");


            List<GameObject> itemObjList = GetItemObjectList(itemName);     // �κ��丮�� ������ ������Ʈ ����Ʈ ����

            // �κ��丮�� ������Ʈ ����Ʈ�� �������� �ʴ� ��� false��ȯ
            if( itemObjList==null )
            {
                Debug.Log( "������Ʈ ����Ʈ�� �����ϴ�." );
                return false;
            }
            // ������Ʈ ����Ʈ�� �����ϸ鼭 ����ִ� ������Ʈ ������ ����ϴٸ�,
            else if( itemObjList.Count-itemObjCount>=0 )
            {
                Debug.Log( "������ ����մϴ�." );
                return true;
            }
            // ������Ʈ ������ ������� �ʴٸ�,
            else
            {
                Debug.Log("������ ��������ʽ���.");
                return false;
            }
        }


            
        


        /// <summary>
        /// �������� ������ ������� �������� �ش� ���� ��ŭ �κ��丮�� �����ϴ��� ���θ� ��ȯ�մϴ�.<br/>
        /// ������ �̸��� ������ ���ڷ� �޽��ϴ�.<br/><br/>
        /// �Ϲ� �������� ������Ʈ�� ������ �ǹ��ϸ�, ��ȭ �������� ��ø������ �ǹ��մϴ�.<br/>   
        /// �ش� ������ŭ ���� �� �ı��ɼ��� ������ �� �ֽ��ϴ�. (�⺻��: ���� 1, ���� ���� �� �ı� ����, �ֽż� ���� �� �ı�)<br/><br/>
        /// *** ���� ���ڰ� 0���϶�� ���ܸ� �߻���ŵ�ϴ�. ***
        /// </summary>
        /// <returns>�������� �����ϸ� ������ ����� ��� true��, �������� �ʰų� ������ ������� �ʴٸ� false�� ��ȯ</returns>
        public bool IsEnough(string itemName, int count=1, bool isReduceAndDestroy = false, bool isLatestModify=true)
        {
            // �������� �ϳ��� �����Ѵٸ�
            if( IsExist( itemName ) )
            {
                // ������ ������ Ȯ���մϴ�.
                ItemType itemType = GetItemTypeIgnoreExists( itemName );

                // ��ȭ �������̶��
                if( itemType==ItemType.Misc )
                {
                    // ��ø������ �˻��մϴ�.
                    if( IsEnoughOverlapCount( itemName, count, false, isLatestModify ) )
                    {
                        // �������� �� �ı��ɼ��� �ɷ��ִٸ�, �������� �� ���� 0���� �ı�
                        if( isReduceAndDestroy )
                            SetOverlapCount( itemName, -count, isLatestModify, null );

                        return true;    // �ɼǿ��ο� ������� true ��ȯ
                    }
                    // ������ ������� �ʴٸ�,
                    else
                        return false;   // �ɼǿ��ο� ������� false ��ȯ 
                }
                // ��ȭ �������� �ƴ϶��
                else
                {
                    // ������Ʈ ������ �˻��մϴ�.
                    if( IsExist( itemName, count ) )
                    {
                        // count������ŭ �����մϴ�.
                        if( isReduceAndDestroy )
                        {
                            for( int i = 0; i<count; i++ )
                            {
                                ItemInfo rItemInfo = RemoveItem( itemName, isLatestModify );
                                GameObject.Destroy( rItemInfo.gameObject );
                            }
                        }

                        return true;        // �ɼǿ��ο� ������� true ��ȯ
                    }
                    else
                        return false;
                }

            }
            // �������� �������� �ʴ´ٸ�,
            else 
                return false;
        }


        /// <summary>
        /// �������� ������ ������� �������� �ش� ���� ��ŭ �κ��丮�� �����ϴ��� ���θ� ��ȯ�մϴ�.<br/>
        /// ������ �̸��� �������� �̷���� ����ü �迭�� ���ڷ� �޽��ϴ�.<br/><br/>
        /// �Ϲ� �������� ������Ʈ�� ������ �ǹ��ϸ�, ��ȭ �������� ��ø������ �ǹ��մϴ�.<br/>   
        /// �ش� ������ŭ ���� �� �ı��ɼ��� ������ �� �ֽ��ϴ�. (�⺻��: ���� 1, ���� ���� �� �ı� ����, �ֽż� ���� �� �ı�)<br/><br/>
        /// *** ���� ���ڰ� 0���϶�� ���ܸ� �߻���ŵ�ϴ�. ***
        /// </summary>
        /// <returns>�������� �����ϸ� ������ ����� ��� true��, �������� �ʰų� ������ ������� �ʴٸ� false�� ��ȯ</returns>
        public bool IsEnough( ItemPair[] pairs, bool isReduceAndDestroy=false, bool isLatestModify=true)
        {
            int allEnough=0;
                        
            // ��� �������� ������ �����ϴ� �� Ȯ��
            foreach(ItemPair pair in pairs )
            {
                if( IsEnough(pair.itemName, pair.overlapCount, false, isLatestModify) )
                    allEnough++;
            }
                                    

            // ��� ������ �����ϴ� ���
            if(allEnough==pairs.Length)
            {
                // ���Ҹ���� ���
                if(isReduceAndDestroy)
                {
                    foreach(ItemPair pair in pairs )
                        IsEnough(pair.itemName, pair.overlapCount, true, isLatestModify);
                }

                return true;
            }
            // �ϳ��� ������ �������� �ʴ� ��� ���и� ��ȯ
            else
                return false;
        }

        


        /// <summary>
        /// ��ȭ �������� �ش� ���� ��ŭ �����Ѵٰ� ������ ��, �κ��丮�� ���� ��������� ��ȯ���ִ� �޼����Դϴ�.<br/>
        /// ������ �̸��� ������ ������ �־�� �մϴ�.<br/><br/>
        /// ���� ������ ������Ʈ�� �ִ� ���� GetCurRemainSlotCount�� ȣ���ϴ� ���� ���������� �����ϴ�.<br/><br/>
        /// *** ��ȭ �������� �ƴϰų�, �����������ڰ� 0���ϸ� ���ܸ� �����ϴ�. ***
        /// </summary>
        /// <returns>�������� �����ϱ� ���� ������ ����ϴٸ� true��, �����ϴٸ� false�� ��ȯ</returns>
        public bool IsAbleToAddMisc( string itemName, int overlapCount )
        {
            ItemType itemType = GetItemTypeIgnoreExists( itemName );

            // �ش� �������� ������ ��ȭ�������� �ƴ� ���� ������ �߸� ���� �� ��� ����ó��
            if( itemType!=ItemType.Misc )
                throw new Exception( "�ش� �̸� �������� ������ ��ȭ �������� �ƴմϴ�." );
            if( overlapCount<=0 )
                throw new Exception( "���� �������ڴ� 1�̻��̾�� �մϴ�." );

            // ���� ������ ���ʿ� ���� �������� ����
            int remainCount = overlapCount;

            // ������ �� �ִ���� ����
            int maxOverlapCount = GetItemMaxOverlapCount( itemName );

            // ���� ������ �������� �� �ִ� ���� ����
            int curRemainSlotCnt = GetCurRemainSlotCount( itemType );


            // ������Ʈ ����Ʈ ������ �����մϴ�.
            List<GameObject> itemObjList = GetItemObjectList( itemName );

            // ���� ������Ʈ ����Ʈ�� �ִ� ����
            if( itemObjList!=null )
            { 
                // �������� �ϳ��� ������ remainCount�� �����մϴ�.
                foreach( GameObject itemObj in itemObjList )
                {
                    ItemMisc itemMisc = (ItemMisc)itemObj.GetComponent<ItemInfo>().Item;

                    // �ش� �������� �ִ�������� ����� ���� ������ �� ���Ҵ� ���� ����մϴ�.
                    remainCount-=( maxOverlapCount-itemMisc.OverlapCount );

                    // ���� ������ ���� �����ۿ� ���԰��� �ϴٸ�, 
                    // ������Ʈ�� ���� ������ �ʿ� �����Ƿ� �� �̻� Ž���� �ߴ��ϰ� ������ ��ȯ�մϴ�.
                    if(remainCount<=0)
                        return true;
                }
            }
            

            
            // ������Ʈ ����Ʈ�� �ƿ� ���°���� �״���� ���� ���� �Ǵ�
            // ���� ������Ʈ ����Ʈ���� ���� ��Ų ��ŭ�� ���� ������ 0�̻��̶��,


            // ���� �� ������Ʈ ���� ���� : ���������� ���� �������� ������Ʈ 1���� �ִ�������� ���� �� (������ �������� ���)
            int createCnt = remainCount / maxOverlapCount;
            int remainder = remainCount % maxOverlapCount;

            // ������ �������� �ʾ�, ���� �Ҽ����� ���� ���� ������Ʈ�� �ϳ� �� �ʿ��ϹǷ� ���� ������ �ø��ϴ�. 
            if(remainder>0)
                createCnt++;


            // ���� ���� ������ �����ؾ� �� ������Ʈ �������� ���ų� ���ٸ� �������� ��ȯ
            if( curRemainSlotCnt >= createCnt )
                 return true; 
            // ���� ���� ������ �����ϴٸ�, ���� �Ұ��� ��ȯ
            else
                return false;
        }

                






    }

    

}