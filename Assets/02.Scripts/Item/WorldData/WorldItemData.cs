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
        /// ���� �迭�� ����
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


        
        private Dictionary<string, Item> InitDic(ItemType itemType)
        {
            switch(itemType)
            {
                case ItemType.Misc:
                    return InitDic_Misc();
                case ItemType.Weapon:
                    return InitDic_Weapon();
                case ItemType.Quest:
                    return InitDic_Quest();
                default:
                    throw new System.Exception("�ش� ������ �������� �ʽ��ϴ�.");
            }
        }



    }




}