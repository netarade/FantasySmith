using ItemData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * <v1.0 - 2024_0105_최원준>
 * 1- 배열형식으로 내부 딕셔너리 새롭게 구성, 생성자에서 메서드를 통해 딕셔너리 초기화
 * 
 * <v2.0 - 2024_0110_최원준>
 * 1- InitDic메서드를 만들고 인자로 ItemType을 넣으면 해당 타입의 사전메서드를 호출해주도록 변경
 * 
 * <v2.1 - 2024_0118_최원준>
 * 1- InitDic메서드 내부에 빌딩아이템의 사전을 추가로 반환하도록 설정
 * 2- 주석 보완
 * 
 * <v2.2 - 2024_0125_최원준>
 * 1- itemDicSub를 변수에서 배열로 변경
 * 
 * 2- 잡화사전 InitDic_BuildingMisc와 InitDic_Food를 초기화
 * 
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
        /// 월드 사전 배열의 길이
        /// </summary>
        public readonly int worldDicLen;

        public WorldItem()
        {
            // 사전길이 설정
            worldDicLen = (int)ItemType.None;

            // 사전과 사전 종류 초기화
            worldDic = new Dictionary<string, Item>[worldDicLen];
            dicType = new ItemType[worldDicLen];
                       
            for(int i=0; i<worldDicLen; i++)
            {
                worldDic[i] = InitDic( (ItemType)i );
                dicType[i] = (ItemType)i;
            }
        }


        


        /// <summary>
        /// ItemType에 해당하는 모든 아이템 정보가 담긴 월드 아이템 딕셔너리를 반환합니다.<br/>
        /// </summary>
        /// <returns>ItemType에 해당하는 월드 아이템 딕셔너리를 반환</returns>
        private Dictionary<string, Item> InitDic(ItemType itemType)
        {            
            Dictionary<string, Item> itemDic;       // 반환할 클래스의 사전
            Dictionary<string, Item>[] itemDicSub = new Dictionary<string, Item>[2];    // 자식 클래스 사전
                        

            switch(itemType)
            {
                case ItemType.Misc:
                    itemDic = InitDic_Misc();
                    itemDicSub[0] = InitDic_BuildingMisc();    //자식사전을 추가로 할당받습니다.
                    itemDicSub[1] = InitDic_Food();

                    for( int i = 0; i<itemDicSub.Length; i++ )
                    {
                        // 자식 사전의 아이템을 하나씩 꺼내어 저장합니다.
                        foreach( KeyValuePair<string, Item> item in itemDicSub[i] )
                            itemDic.Add( item.Key, item.Value );
                    }
                    
                    break;

                case ItemType.Weapon:                
                    itemDic = InitDic_Weapon();
                    break;

                case ItemType.Quest:
                    itemDic = InitDic_Quest();
                    break;

                case ItemType.Building:
                    itemDic = InitDic_Building();
                    break;

                default:
                    throw new System.Exception("해당 사전이 존재하지 않습니다.");
            }

            return itemDic;
        }



    }




}