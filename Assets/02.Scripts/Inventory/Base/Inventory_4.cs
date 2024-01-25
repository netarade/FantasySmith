using ItemData;
using System;
using System.Collections.Generic;

/*
 * <v1.0 - 2024_0115_최원준>
 * 1- 탭을 정의하는 열거형과 메서드를 Interactive클래스에서 옮겨왔으며, 메서드를 static 처리하여 접근성을 높임.
 * 이유는 Inventory클래스 내부적으로 어떤 탭에 따라서 AddItem 및 SlotIndex를 계산하는 것이 달라질 수 있기 때문. 
 * 
 * 2- TabType에 None을 추가
 * 이유는 마지막 길이로 인식하여 계산하기 위함.
 * 
 * 3- ConvertTabTypeToItemTypeList메서드 내부 TabType.None 전달에 대한 예외처리 추가 
 * 
 * <v1.1 - 2024_0118_최원준>
 * 1- ConvertItemTypeToTabType메서드내부의 모든 아이템 종류 검사를 ==으로 대체
 * (is문을 사용했으나 열거형은 폴리모핑이 안되므로)
 * 
 * <v1.3 - 2024_0125_최원준>
 * 1- ItemBuilding 클래스의 상속관계를 ItemMisc에서 Item으로 변경함으로 인해 
 * ConvertItemTypeToTabType의 탭타입을 All로 설정
 * 
 */




namespace InventoryManagement
{
    /// <summary>
    /// 탭의 종류입니다.
    /// </summary>
    public enum TabType { All, Quest, Misc, Equip, None }



    public partial class Inventory
    {        
        
        /// <summary>
        /// 아이템 타입을 해당하는 탭 타입으로 변환해주는 메서드입니다.<br/>
        /// ItemType.None을 전달 시 TabType.All을 반환합니다.<br/><br/>
        /// *** 아이템 종류에 해당하는 탭이 없다면 예외가 발생합니다. ***
        /// </summary>
        /// <returns>인자로 들어온 아이템 종류에 해당하는 탭 타입</returns>
        public static TabType ConvertItemTypeToTabType(ItemType itemType)
        {
            if(itemType == ItemType.None )
                return TabType.All;
            else if(itemType == ItemType.Quest)
                return TabType.Quest;
            else if(itemType == ItemType.Misc)          // 건설재료, 요리재료등 모두 잡화로 인식
                return TabType.Misc;
            else if(itemType == ItemType.Weapon)
                return TabType.Equip;
            else if( itemType==ItemType.Building )      // 실외 건설아이템의 경우 모든 탭으로 인식
                return TabType.All;
            else
                throw new Exception("해당 아이템 종류로 설정된 탭타입이 존재하지 않습니다.");        
        }



        /// <summary>
        /// 탭 타입에 해당하는 모든 아이템 타입을 찾아주는 메서드입니다.<br/>
        /// 아이템 타입을 담을 수 있는 리스트를 전달해야 합니다.<br/><br/>
        /// *** 리스트 미전달 시 예외가 발생하며, 리스트 전달 시 리스트를 초기화하므로 주의해주세요. ***
        /// </summary>
        /// <returns>해당하는 탭의 아이템 타입의 갯수(리스트에 담긴 갯수)를 반환</returns>
        public static int ConvertTabTypeToItemTypeList(ref List<ItemType> itemTypeList, TabType curTabType)
        {        
            if(itemTypeList==null)
                throw new Exception("아이템 종류를 담을 리스트를 전달해주세요.");
            if(curTabType == TabType.None)
                throw new Exception("탭 종류를 잘못 전달하셨습니다.");


            // 리스트 초기화 메서드 호출
            itemTypeList.Clear();

            // 현재 활성화 탭이 전체 탭이라면,
            if( curTabType==TabType.All )
            {
                // 퀘스트 아이템을 제외한 모든 종류의 아이템 타입을 담습니다.
                for( int i = 0; i<(int)ItemType.None; i++ )
                {
                    if( i==(int)ItemType.Quest )
                        continue;

                    itemTypeList.Add( (ItemType)i );
                }
            }
            // 현재 활성화 탭이 개별 탭이라면,
            else
            {
                // 현재 탭에 해당하는 아이템 종류를 찾기 위해 개별 아이템 종류를 하나씩 순회합니다.
                for( int i = 0; i<(int)ItemType.None; i++ )
                {
                    // 순차 인덱스에 해당하는 아이템 타입변수를 확인합니다.
                    ItemType findItemType = (ItemType)i;

                    // 현재활성화 된 탭과 동일한 종류의 아이템 타입을 찾습니다.
                    if( curTabType==ConvertItemTypeToTabType( findItemType ) )
                        itemTypeList.Add( findItemType );
                }
            }
        
            // 리스트에 담긴 갯수를 반환합니다.
            return itemTypeList.Count;
        }




    }

}