using ItemData;
using System;
using System.Collections.Generic;

/*
 * <v1.0 - 2024_0115_�ֿ���>
 * 1- ���� �����ϴ� �������� �޼��带 InteractiveŬ�������� �Űܿ�����, �޼��带 static ó���Ͽ� ���ټ��� ����.
 * ������ InventoryŬ���� ���������� � �ǿ� ���� AddItem �� SlotIndex�� ����ϴ� ���� �޶��� �� �ֱ� ����. 
 * 
 * 2- TabType�� None�� �߰�
 * ������ ������ ���̷� �ν��Ͽ� ����ϱ� ����.
 * 
 * 3- ConvertTabTypeToItemTypeList�޼��� ���� TabType.None ���޿� ���� ����ó�� �߰� 
 * 
 * <v1.1 - 2024_0118_�ֿ���>
 * 1- ConvertItemTypeToTabType�޼��峻���� ��� ������ ���� �˻縦 ==���� ��ü
 * (is���� ��������� �������� ���������� �ȵǹǷ�)
 * 
 * <v1.3 - 2024_0125_�ֿ���>
 * 1- ItemBuilding Ŭ������ ��Ӱ��踦 ItemMisc���� Item���� ���������� ���� 
 * ConvertItemTypeToTabType�� ��Ÿ���� All�� ����
 * 
 * <v1.4 - 2024_0126_�ֿ���>
 * 1- GetItemDic�޼��忡�� itemDic�� null�� ���(������ ���� �Ҵ���� ���� ���)
 * GetItemDic�޼��尡 null���� ��ȯ�ϵ��� �߰� ������ ����.
 * (���̺�ε尡 �̷������ ���� InventoryInfo�� Awake������ UpdateAllItemAppearAs2D�޼����� ȣ���� �̷���� ��
 *  �ش� GetItemDic�� ȣ��Ǵµ� ������ �Ҵ�Ǿ����� �ʾ� null���� ��ȯ�ϱ� ���� ���ܰ� �߻�)
 * 
 * 
 */




namespace InventoryManagement
{
    /// <summary>
    /// ���� �����Դϴ�.
    /// </summary>
    public enum TabType { All, Quest, Misc, Equip, None }



    public partial class Inventory
    {        
        
        
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
        /// ������ �̸��� �Է��Ͽ� �ش� �������� ���� enum�� int������ ��ȯ�޽��ϴ�. (�κ��丮�� �������� �ʾƵ� �˴ϴ�.) <br/>
        /// *** �������� ���� ������ �������� �ʴ´ٸ� ���ܰ� �߻�***
        /// </summary>
        /// <returns>�ش� �������� ItemType�� int�� ��ȯ ��</returns>
        public int GetItemTypeIndexIgnoreExists(string itemName)
        {   
            return (int)GetItemTypeIgnoreExists(itemName);
        }


        

        
        /// <summary>
        /// �������� �̸��� ������� �ش� �������� ��ųʸ� �������� ��ȯ�մϴ�<br/>
        /// ������ �̸��� �ش��ϴ� ������ ������ ������ �������� �ʴ´ٸ� null�� ��ȯ�մϴ�.<br/>
        /// ** �ش��ϴ� �̸��� �������� ���� ������ ��Ͽ� �������� �ʴ� ��� ���� �߻� **
        /// </summary>
        /// <returns>�ش� ������ ������ ������ ��ȯ</returns>
        public Dictionary<string, List<ItemInfo>> GetItemDic(string itemName)
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
        public Dictionary<string, List<ItemInfo>> GetItemDic(ItemType itemType)
        {
            if(itemType == ItemType.None)
                throw new Exception("��Ȯ�� ������ �������� �����ؾ� �մϴ�.");

            int dicIdx = GetDicIndex(itemType);

            if( dicIdx < 0 || itemDic==null )
                return null;
            else
                return itemDic[dicIdx];
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




        /// <summary>
        /// ������ �̸��� �ش��ϴ� ItemInfo ����Ʈ �������� ��ȯ�մϴ�<br/>
        /// �ɼ��� ���� ����Ʈ�� ���ٸ� ���Ӱ� ������ �� �ֽ��ϴ�.<br/><br/>
        /// *** �ش� ������ �̸��� ��������� ��Ī���� �ʴ� ��� ���ܰ� �߻� ***<br/>
        /// *** �ش� ������ �̸��� ������ �ش��ϴ� ������ �����ϰ� ���� �ʴٸ� ���ܰ� �߻� ***
        /// </summary>
        /// <returns>
        ///  1. ������ �̸��� �������� �ϳ��� ����ִٸ� �ش� ItemInfo ����Ʈ�� ��ȯ�մϴ�.<br/>
        ///  2. ������ �̸��� �������� �ϳ��� ������� ���� �� (���� �����ϱ� �ɼ��� ���� ���� ���) null���� ��ȯ�մϴ�.(�Ϲ��� ���, IsExist�� ��ü����)<br/>
        ///  3. ������ �̸��� �������� �ϳ��� ������� ���� �� (���� �����ϱ� �ɼ��� �� ���) ���ο� ItemInfo ����Ʈ�� ��ȯ�մϴ�.(AddItem�� ���)
        /// </returns>
        public List<ItemInfo> GetItemInfoList(string itemName, bool isNewIfNotExist=false)
        {
            // ������ �̸��� �ش��ϴ� ���� �ε����� ���մϴ�.
            int dicIdx = GetDicIndex( GetItemTypeIgnoreExists(itemName) );

            // �����ε��� ���� 0���϶�� ���ܸ� �����ϴ�.
            if(dicIdx<0)
                throw new Exception("�ش� ������ �̸��� �ش��ϴ� ������ �������� �ʽ��ϴ�.");

            // �������� �ϳ��� ����ִٸ� �ٷ� �ش� ItemInfo ����Ʈ�� ��ȯ�մϴ�.
            if(itemDic[dicIdx].ContainsKey(itemName))
                return itemDic[dicIdx][itemName];
            
            // �������� ������� ������, ���� �����ϱ� �ɼ��� �ִ� ���
            // ItemInfo ����Ʈ�� ���� ���� �� ����Ͽ� ��ȯ�մϴ�.
            else if(isNewIfNotExist)     
            {                    
                List<ItemInfo> itemInfoList = new List<ItemInfo>();  // ItemInfo ����Ʈ�� ���� ����ϴ�.
                itemDic[dicIdx].Add(itemName, itemInfoList);         // �κ��丮 ������ ItemInfo ����Ʈ�� ����ֽ��ϴ�.
                return itemInfoList;                                 // ������ ItemInfo ����Ʈ ������ ��ȯ�մϴ�.
            }
            
            // �������� ������� ������, ���� �����ϱ� �ɼ��� ���� ��� null�� ��ȯ�մϴ�.
            else                    
                return null;           
        }
















        /// <summary>
        /// ������ Ÿ���� �ش��ϴ� �� Ÿ������ ��ȯ���ִ� �޼����Դϴ�.<br/>
        /// ItemType.None�� ���� �� TabType.All�� ��ȯ�մϴ�.<br/><br/>
        /// *** ������ ������ �ش��ϴ� ���� ���ٸ� ���ܰ� �߻��մϴ�. ***
        /// </summary>
        /// <returns>���ڷ� ���� ������ ������ �ش��ϴ� �� Ÿ��</returns>
        public static TabType ConvertItemTypeToTabType(ItemType itemType)
        {
            if(itemType == ItemType.None )
                return TabType.All;
            else if(itemType == ItemType.Quest)
                return TabType.Quest;
            else if(itemType == ItemType.Misc)          // �Ǽ����, �丮���� ��� ��ȭ�� �ν�
                return TabType.Misc;
            else if(itemType == ItemType.Weapon)
                return TabType.Equip;
            else if( itemType==ItemType.Building )      // �ǿ� �Ǽ��������� ��� ��� ������ �ν�
                return TabType.All;
            else
                throw new Exception("�ش� ������ ������ ������ ��Ÿ���� �������� �ʽ��ϴ�.");        
        }



        /// <summary>
        /// �� Ÿ�Կ� �ش��ϴ� ��� ������ Ÿ���� ã���ִ� �޼����Դϴ�.<br/>
        /// ������ Ÿ���� ���� �� �ִ� ����Ʈ�� �����ؾ� �մϴ�.<br/><br/>
        /// *** ����Ʈ ������ �� ���ܰ� �߻��ϸ�, ����Ʈ ���� �� ����Ʈ�� �ʱ�ȭ�ϹǷ� �������ּ���. ***
        /// </summary>
        /// <returns>�ش��ϴ� ���� ������ Ÿ���� ����(����Ʈ�� ��� ����)�� ��ȯ</returns>
        public static int ConvertTabTypeToItemTypeList(ref List<ItemType> itemTypeList, TabType curTabType)
        {        
            if(itemTypeList==null)
                throw new Exception("������ ������ ���� ����Ʈ�� �������ּ���.");
            if(curTabType == TabType.None)
                throw new Exception("�� ������ �߸� �����ϼ̽��ϴ�.");


            // ����Ʈ �ʱ�ȭ �޼��� ȣ��
            itemTypeList.Clear();

            // ���� Ȱ��ȭ ���� ��ü ���̶��,
            if( curTabType==TabType.All )
            {
                // ����Ʈ �������� ������ ��� ������ ������ Ÿ���� ����ϴ�.
                for( int i = 0; i<(int)ItemType.None; i++ )
                {
                    if( i==(int)ItemType.Quest )
                        continue;

                    itemTypeList.Add( (ItemType)i );
                }
            }
            // ���� Ȱ��ȭ ���� ���� ���̶��,
            else
            {
                // ���� �ǿ� �ش��ϴ� ������ ������ ã�� ���� ���� ������ ������ �ϳ��� ��ȸ�մϴ�.
                for( int i = 0; i<(int)ItemType.None; i++ )
                {
                    // ���� �ε����� �ش��ϴ� ������ Ÿ�Ժ����� Ȯ���մϴ�.
                    ItemType findItemType = (ItemType)i;

                    // ����Ȱ��ȭ �� �ǰ� ������ ������ ������ Ÿ���� ã���ϴ�.
                    if( curTabType==ConvertItemTypeToTabType( findItemType ) )
                        itemTypeList.Add( findItemType );
                }
            }
        
            // ����Ʈ�� ��� ������ ��ȯ�մϴ�.
            return itemTypeList.Count;
        }



        






    }

}