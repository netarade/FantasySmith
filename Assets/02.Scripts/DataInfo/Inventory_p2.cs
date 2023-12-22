using ItemData;
using System.Collections.Generic;
using UnityEngine;

/*
 * [작업 사항]  
 * <v1.0 - 2023_1121_최원준>
 * 1- 초기 분할 클래스 정의
 * AddItem메서드 추가 - CreateManager의 CreateItemToNearstSlot메서드 호출 기능
 * 
 * <v1.1 -2023_1121_최원준>
 * 1- 분할클래스를 만들면서 MonoBehaviour를 지우지 않아 new 키워드 경고가 발생하여 제거.
 * 
 * <v1.2 - 2023_1122_최원준>
 * 1- 인벤토리가 로드되었을 때 아이템 오브젝트가 원위치를 시킬 수 있도록 UpdateAllItem메서드 추가
 * 
 */

namespace CraftData
{    
    public partial class Inventory
    {
        public void AddItem(string itemName, int count=1)
        {
            CreateManager.instance.CreateItemToNearstSlot(this, itemName, count);
        }

        public void RemoveItem(string itemName)
        {

        }


        public void UpdateAllItem()
        {
            foreach( List<GameObject> objList in weapDic.Values )          // 무기사전에서 게임오브젝트 리스트를 하나씩 꺼내어
            {
                for(int i=0; i<objList.Count; i++)                         // 리스트의 게임오브젝트를 모두 가져옵니다.
                    objList[i].GetComponent<ItemInfo>().OnItemChanged();   // item 스크립트를 하나씩 꺼내어 OnItemChnaged메서드를 호출합니다.
            }

            foreach( List<GameObject> objList in miscDic.Values ) // 잡화사전에서 게임오브젝트 리스트를 하나씩 꺼내어
            {
                for(int i=0; i<objList.Count; i++)                         // 리스트의 게임오브젝트를 모두 가져옵니다.
                    objList[i].GetComponent<ItemInfo>().OnItemChanged();   // item 스크립트를 하나씩 꺼내어 OnItemChnaged메서드를 호출합니다.
            }      
        }


    }
}
