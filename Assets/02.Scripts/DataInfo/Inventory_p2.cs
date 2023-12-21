using ItemData;
using UnityEngine;

/*
 * [작업 사항]  
 * <v1.0 - 2023_1121_최원준>
 * 1- 초기 분할 클래스 정의
 * AddItem메서드 추가 - CreateManager의 CreateItemToNearstSlot메서드 호출 기능
 * 
 * <v1.1 -2023_1121_최원준>
 * 1- 분할클래스를 만들면서 MonoBehaviour를 지우지 않아 new 키워드 경고가 발생하여 제거.
 */

namespace CraftData
{    
    public partial class Inventory
    {
        public void AddItem(string itemName, int count=1)
        {
            CreateManager.instance.CreateItemToNearstSlot(this, itemName, count);
        }

    }
}
