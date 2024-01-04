using ItemData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * <v1.0 - 2024_0105_최원준>
 * 1- 배열형식으로 내부 딕셔너리 새롭게 구성, 생성자에서 메서드를 통해 딕셔너리 초기화
 * 
 */

namespace WorldItemData
{
    /// <summary>
    /// 모든 월드 아이템 목록을 보유하고 있는 딕셔너리 집합입니다. Monobehaviour를 상속하지 않으므로 생성해서 정보를 받아야 합니다.
    /// </summary>
    public partial class WorldItem
    {
        /// <summary>
        /// 월드 아이템이 담긴 사전 배열
        /// </summary>
        public readonly Dictionary<string, Item>[] worldDic;

        /// <summary>
        /// 사전 별 보관하는 아이템 종류
        /// </summary>
        public readonly ItemType[] dicType;
        
        /// <summary>
        /// 사전 배열의 길이
        /// </summary>
        public readonly int worldDicLen;

        public WorldItem()
        {
            // 사전길이 설정
            worldDicLen = 2;

            // 사전과 사전 종류 초기화
            worldDic = new Dictionary<string, Item>[worldDicLen];
            dicType = new ItemType[worldDicLen];
                       
            worldDic[0] = InitDic_Misc();
            dicType[0] = ItemType.Misc;

            worldDic[1] = InitDic_Weapon(); 
            dicType[1] = ItemType.Weapon;
        }
    }
}