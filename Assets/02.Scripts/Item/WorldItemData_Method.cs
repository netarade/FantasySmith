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
        /// 아이템 이름을 기반으로 해당 아이템의 ItemType을 반환합니다
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
        /// 아이템 이름을 기반으로 해당 아이템의 월드 딕셔너리 참조값을 반환합니다
        /// </summary>
        public Dictionary<string, Item> GetWorldDic(string itemName)
        {
            if(weapDic.ContainsKey(itemName))
                return weapDic;
            else if(miscDic.ContainsKey(itemName))
                return miscDic;
            else
                throw new Exception("해당하는 이름의 아이템이 없습니다. 이름을 확인하여 주세요.");
        }

        /// <summary>
        /// 해당 아이템이름을 기반으로 아이템이 월드 딕셔너리 목록에서 존재하는지 여부를 반환합니다
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
