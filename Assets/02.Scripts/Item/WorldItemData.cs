using ItemData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * <v1.0 - 2024_0105_�ֿ���>
 * 1- �迭�������� ���� ��ųʸ� ���Ӱ� ����, �����ڿ��� �޼��带 ���� ��ųʸ� �ʱ�ȭ
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
            worldDicLen = 2;

            // ������ ���� ���� �ʱ�ȭ
            worldDic = new Dictionary<string, Item>[worldDicLen];
            dicType = new ItemType[worldDicLen];
                       
            worldDic[0] = InitDic_Misc();
            dicType[0] = ItemType.Misc;

            worldDic[1] = InitDic_Weapon(); 
            dicType[1] = ItemType.Weapon;
        }
    }
}