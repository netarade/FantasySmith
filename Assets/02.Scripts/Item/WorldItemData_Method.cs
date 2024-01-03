using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * [�۾� ����]
 * 
 * <v1.0 - 2023_1231_�ֿ���>
 * 1- �ű� �ڵ� �ۼ� 
 * 
 * <v1.1 - 2024_0104_�ֿ���>
 * 1- ����ó���� ���� �ּ� �߰�
 * 
 * 
 */


namespace WorldItemData
{
    public partial class WorldItem
    {
        /// <summary>
        /// ������ �̸��� ������� �ش� �������� ItemType�� ��ȯ�մϴ�<br/><br/>
        /// *** �ش��ϴ� �̸��� �������� ���� ��� ItemType.None�� ��ȯ�մϴ�.***
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
        /// ������ �̸��� ������� �ش� �������� ���� ��ųʸ� �������� ��ȯ�մϴ�.<br/><br/>
        /// *** �ش��ϴ� �̸��� �������� ���� ��� ���ܸ� �߻���ŵ�ϴ�.***
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
        /// �ش� �������̸��� ������� �������� ���� ��ųʸ� ��Ͽ��� �����ϴ��� ���θ� ��ȯ�մϴ�<br/><br/>
        /// *** �ش��ϴ� �̸��� �������� ���� ��� false�� ��ȯ�մϴ�.***
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
