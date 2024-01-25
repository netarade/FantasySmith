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