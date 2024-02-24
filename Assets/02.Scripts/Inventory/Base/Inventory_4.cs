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
 * <v1.4 - 2024_0126_최원준>
 * 1- GetItemDic메서드에서 itemDic이 null인 경우(사전이 아직 할당되지 않은 경우)
 * GetItemDic메서드가 null값을 반환하도록 추가 조건을 적음.
 * (세이브로드가 이루어지기 전에 InventoryInfo의 Awake문에서 UpdateAllItemAppearAs2D메서드의 호출이 이루어질 때
 *  해당 GetItemDic이 호출되는데 사전이 할당되어있지 않아 null값을 반환하기 전에 예외가 발생)
 * 
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
        /// 아이템 종류에 해당하는 사전의 인덱스를 반환합니다.<br/>
        /// 일치하는 종류의 사전이 없다면 -1을 반환합니다.<br/><br/>
        /// *** ItemType.None을 전달하면 예외가 발생합니다. ***
        /// </summary>
        /// <returns>일치하는 종류의 사전이 있다면 해당 사전의 인덱스 값을 반환, 없다면 -1을 반환</returns>
        public int GetDicIndex(ItemType itemType)
        {
            if(itemType==ItemType.None )
                throw new Exception("아이템 종류가 잘못되었습니다. 해당 종류의 사전을 구할 수 없습니다.");

            for(int i=0; i<dicLen; i++)
            {
                if( dicType[i]==itemType) 
                    return i;
            }

            return -1;
        }


        /// <summary>
        /// 아이템 종류에 해당하는 텝의 인덱스를 반환합니다.<br/>
        /// ItemType.None이 전달되었다면 전체 탭 인덱스 (TabType.All)을 구합니다.<br/>
        /// *** 아이템 종류에 해당하는 탭이없다면 예외가 발생합니다. ***
        /// </summary>
        /// <returns>아이템 종류에 해당하는 탭이 있다면 해당 탭의 인덱스를 반환, 없다면 -1을 반환</returns>
        public int GetTabIndex(ItemType itemType)
        {
            return (int)ConvertItemTypeToTabType(itemType);            
        }



        /// <summary>
        /// 아이템 이름을 기반으로 해당 아이템의 ItemType 값을 반환합니다<br/>
        /// 아이템이 현재 인벤토리의 목록에 존재하지 않아도 ItemType 값을 얻을 수 있습니다.<br/><br/>
        /// ** 해당하는 이름의 아이템이 월드 아이템 목록에 존재하지 않는 경우 예외 발생 **
        /// </summary>
        /// <returns>이름에 해당하는 아이템 타입을 반환</returns>
        public ItemType GetItemTypeIgnoreExists(string itemName)
        {            
            return createManager.GetWorldItemType(itemName);
        }
                

        /// <summary>
        /// 아이템 이름을 입력하여 해당 아이템의 종류 enum을 int형으로 반환받습니다. (인벤토리에 존재하지 않아도 됩니다.) <br/>
        /// *** 아이템이 월드 사전에 존재하지 않는다면 예외가 발생***
        /// </summary>
        /// <returns>해당 아이템의 ItemType의 int형 반환 값</returns>
        public int GetItemTypeIndexIgnoreExists(string itemName)
        {   
            return (int)GetItemTypeIgnoreExists(itemName);
        }


        

        
        /// <summary>
        /// 아이템의 이름을 기반으로 해당 아이템의 딕셔너리 참조값을 반환합니다<br/>
        /// 아이템 이름에 해당하는 종류의 아이템 사전이 존재하지 않는다면 null을 반환합니다.<br/>
        /// ** 해당하는 이름의 아이템이 월드 아이템 목록에 존재하지 않는 경우 예외 발생 **
        /// </summary>
        /// <returns>해당 아이템 종류의 사전을 반환</returns>
        public Dictionary<string, List<ItemInfo>> GetItemDic(string itemName)
        {            
            ItemType itemType = createManager.GetWorldItemType(itemName);

            return GetItemDic(itemType);
        }


        /// <summary>
        /// 아이템의 타입을 기반으로 해당 아이템의 딕셔너리 참조값을 반환합니다<br/>
        /// 해당 종류의 아이템 사전이 존재하지 않는다면 null을 반환합니다.<br/>
        /// *** ItemType.None으로 전달 된 경우 예외를 던집니다 *** 
        /// </summary>
        /// <returns>해당 아이템 종류의 사전을 반환</returns>
        public Dictionary<string, List<ItemInfo>> GetItemDic(ItemType itemType)
        {
            if(itemType == ItemType.None)
                throw new Exception("정확한 종류의 아이템을 전달해야 합니다.");

            int dicIdx = GetDicIndex(itemType);

            if( dicIdx < 0 || itemDic==null )
                return null;
            else
                return itemDic[dicIdx];
        }
        




        

        
        /// <summary>
        /// 해당 아이템의 이름을 인자로 넣어 아이템의 중첩 최대 갯수를 반환 받습니다.<br/>
        /// 아이템 별로 동일한 최대 중첩 가능 갯수를 알기 위한 메서드입니다.<br/><br/>
        /// 현재 IsAbleToAddMisc메서드에서 내부적으로 사용됩니다.<br/><br/>
        /// *** 해당하는 이름의 아이템이 잡화아이템이 아니거나 없는 경우 예외가 발생합니다.<br/>
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns>아이템의 최대 갯수를 반환합니다.</returns>
        public int GetItemMaxOverlapCount(string itemName)
        {
            Dictionary<string, Item> worldDic = createManager.GetWorldDic(itemName);

            ItemMisc itemMisc = worldDic[itemName] as ItemMisc;
            
            if(itemMisc==null)
                throw new Exception("해당 아이템 이름이 잡화아이템이 아닙니다.");

            return itemMisc.MaxOverlapCount;
        }




        /// <summary>
        /// 아이템 이름에 해당하는 ItemInfo 리스트 참조값을 반환합니다<br/>
        /// 옵션을 통해 리스트가 없다면 새롭게 생성할 수 있습니다.<br/><br/>
        /// *** 해당 아이템 이름이 월드사전에 매칭되지 않는 경우 예외가 발생 ***<br/>
        /// *** 해당 아이템 이름의 종류에 해당하는 사전을 보관하고 있지 않다면 예외가 발생 ***
        /// </summary>
        /// <returns>
        ///  1. 동일한 이름의 아이템이 하나라도 들어있다면 해당 ItemInfo 리스트를 반환합니다.<br/>
        ///  2. 동일한 이름의 아이템이 하나라도 들어있지 않을 때 (새로 생성하기 옵션을 주지 않은 경우) null값을 반환합니다.(일반적 사용, IsExist를 대체가능)<br/>
        ///  3. 동일한 이름의 아이템이 하나라도 들어있지 않을 때 (새로 생성하기 옵션을 준 경우) 새로운 ItemInfo 리스트를 반환합니다.(AddItem시 사용)
        /// </returns>
        public List<ItemInfo> GetItemInfoList(string itemName, bool isNewIfNotExist=false)
        {
            // 아이템 이름에 해당하는 사전 인덱스를 구합니다.
            int dicIdx = GetDicIndex( GetItemTypeIgnoreExists(itemName) );

            // 사전인덱스 값이 0이하라면 예외를 던집니다.
            if(dicIdx<0)
                throw new Exception("해당 아이템 이름에 해당하는 사전이 존재하지 않습니다.");

            // 아이템이 하나라도 들어있다면 바로 해당 ItemInfo 리스트를 반환합니다.
            if(itemDic[dicIdx].ContainsKey(itemName))
                return itemDic[dicIdx][itemName];
            
            // 아이템이 들어있지 않을때, 새로 생성하기 옵션이 있는 경우
            // ItemInfo 리스트를 새로 생성 및 등록하여 반환합니다.
            else if(isNewIfNotExist)     
            {                    
                List<ItemInfo> itemInfoList = new List<ItemInfo>();  // ItemInfo 리스트를 새로 만듭니다.
                itemDic[dicIdx].Add(itemName, itemInfoList);         // 인벤토리 사전에 ItemInfo 리스트를 집어넣습니다.
                return itemInfoList;                                 // 생성된 ItemInfo 리스트 참조를 반환합니다.
            }
            
            // 아이템이 들어있지 않을때, 새로 생성하기 옵션이 없는 경우 null을 반환합니다.
            else                    
                return null;           
        }
















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