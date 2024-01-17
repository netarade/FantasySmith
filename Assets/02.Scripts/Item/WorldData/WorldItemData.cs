using ItemData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * <v1.0 - 2024_0105_�ֿ���>
 * 1- �迭�������� ���� ��ųʸ� ���Ӱ� ����, �����ڿ��� �޼��带 ���� ��ųʸ� �ʱ�ȭ
 * 
 * <v2.0 - 2024_0110_�ֿ���>
 * 1- InitDic�޼��带 ����� ���ڷ� ItemType�� ������ �ش� Ÿ���� �����޼��带 ȣ�����ֵ��� ����
 * 
 * <v2.1 - 2024_0118_�ֿ���>
 * 1- InitDic�޼��� ���ο� ������������ ������ �߰��� ��ȯ�ϵ��� ����
 * 2- �ּ� ����
 * 
 */

namespace WorldItemData
{
    /// <summary>
    /// ��� ���� ������ ����� �����ϰ� �ִ� ��ųʸ� �����Դϴ�. Monobehaviour�� ������� �����Ƿ� �����ؼ� ������ �޾ƾ� �մϴ�.
    /// </summary>
    public partial class WorldItem
    {
        /// <summary>
        /// ���� �������� ��� ���� �迭
        /// </summary>
        public readonly Dictionary<string, Item>[] worldDic;

        /// <summary>
        /// ���� �� �����ϴ� ������ ����
        /// </summary>
        public readonly ItemType[] dicType;
        
        /// <summary>
        /// ���� ���� �迭�� ����
        /// </summary>
        public readonly int worldDicLen;

        public WorldItem()
        {
            // �������� ����
            worldDicLen = (int)ItemType.None;

            // ������ ���� ���� �ʱ�ȭ
            worldDic = new Dictionary<string, Item>[worldDicLen];
            dicType = new ItemType[worldDicLen];
                       
            for(int i=0; i<worldDicLen; i++)
            {
                worldDic[i] = InitDic( (ItemType)i );
                dicType[i] = (ItemType)i;
            }
        }


        


        /// <summary>
        /// ItemType�� �ش��ϴ� ��� ������ ������ ��� ���� ������ ��ųʸ��� ��ȯ�մϴ�.<br/>
        /// </summary>
        /// <returns>ItemType�� �ش��ϴ� ���� ������ ��ųʸ��� ��ȯ</returns>
        private Dictionary<string, Item> InitDic(ItemType itemType)
        {            
            Dictionary<string, Item> itemDic;       // ��ȯ�� Ŭ������ ����
            Dictionary<string, Item> itemDicSub;    // �ڽ� Ŭ���� ����
            
            switch(itemType)
            {
                case ItemType.Misc:
                    itemDic = InitDic_Misc();
                    itemDicSub = InitDic_Building();    //�ڽĻ����� �߰��� �Ҵ�޽��ϴ�.

                    // �ڽ� ������ �������� �ϳ��� ������ �����մϴ�.
                    foreach(KeyValuePair<string, Item> item in itemDicSub)
                        itemDic.Add(item.Key, item.Value);
                                        
                    break;

                case ItemType.Weapon:                
                    itemDic = InitDic_Weapon();
                    break;

                case ItemType.Quest:
                    itemDic = InitDic_Quest();
                    break;

                default:
                    throw new System.Exception("�ش� ������ �������� �ʽ��ϴ�.");
            }

            return itemDic;
        }



    }




}