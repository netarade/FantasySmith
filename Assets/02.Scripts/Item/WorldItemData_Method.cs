using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WorldItemData
{
    public partial class WorldItem
    {
        /// <summary>
        /// ������ �̸��� ������� �ش� �������� ItemType�� ��ȯ�մϴ�
        /// </summary>
        public ItemType GetItemType(string itemName)
        {
            if(weapDic.ContainsKey(itemName))
                return ItemType.Weapon;
            else if(miscDic.ContainsKey(itemName))
                return ItemType.Misc;
            else
                return ItemType.None;
        }
        
        /// <summary>
        /// ������ �̸��� ������� �ش� �������� ���� ��ųʸ� �������� ��ȯ�մϴ�
        /// </summary>
        public Dictionary<string, Item> GetWorldDic(string itemName)
        {
            if(weapDic.ContainsKey(itemName))
                return weapDic;
            else if(miscDic.ContainsKey(itemName))
                return miscDic;
            else
                throw new Exception("�ش��ϴ� �̸��� �������� �����ϴ�. �̸��� Ȯ���Ͽ� �ּ���.");
        }

        /// <summary>
        /// �ش� �������̸��� ������� �������� ���� ��ųʸ� ��Ͽ��� �����ϴ��� ���θ� ��ȯ�մϴ�
        /// </summary>
        public bool IsContainsItemName(string itemName)
        {
            if(weapDic.ContainsKey(itemName))
                return true;
            else if(miscDic.ContainsKey(itemName))
                return true;
            else
                return false;
        }



    }
}
