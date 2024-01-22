using ItemData;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/*
 * <v1.0 - 2023_1231_최원준>
 * 1- FindNearstSlotIdx itemName과 itemType을 인자로 받는 오버로등 메서드 구현
 * 
 * <v1.1 - 2024_0102_최원준>
 * 1- 아이템 수량 검색기능 
 * IsEnoughOverlapCount 메서드 작성
 * 
 * 2- 아이템 수량 조절 및 목록에서 제거 기능 
 * IsEnoughOverlapCount 메서드에 수량감소모드 추가 및 SetOverlapCount 메서드 작성
 * 
 * (향후 구현 예정_0102)
 * 1- 이름과 수량을 구조체 형태로 만들어서 배열로 전달하는 오버로딩 메서드 구현하기
 * 
 * 
 * <v2.0 - 2024_0103_최원준>
 * 1- IsEnoughOverlapCount메서드 수량전달인자에 대한 예외처리 추가 
 *  
 * 2- IsExist 추가
 * 비잡화 아이템이 존재하는지 여부를 반환하는 메서드, 삭제까지 가능
 * 
 * 3- ItemPair 구조체 추가
 * 인자로 이름과 수량 쌍을 전달할 수 있도록 설계
 * 
 * 4- ItemPair과 ItemPair배열을 인자로 받는 IsEnough메서드 정의
 * 인자로 전달한 아이템이 있는지, 수량까지 충분한지를 검사하고 수량감소 또는 제거할 수있도록 구현
 * 
 * (이슈)
 * 1- IsEnough, IsEnoughOverlapCount, IsExist 메서드의 반환값을 ItemInfo로 변경해야한다
 * 이유는 수량이 0이된 아이템 오브젝트를 Info클래스에서 삭제처리해야 하기 때문
 * 
 * <v2.1 - 2024_0103_최원준>
 * (이슈_0103) 
 * 1- SetOverlapCount메서드에서
 * 오브젝트리스트에서 하나씩 아이템을 읽어와서 수량 정보를 변경시키고 있는데,
 * 인벤토리 제거 시 오브젝트 리스트 변동이 일어나는 문제가 있다.
 * => 삭제할 아이템을 모아서 한번에 리스트에서 제거하는 형태로 구현해야
 * 
 * 
 * (이슈_0104)
 * 1- 제거할 때 정렬해서 제거해야 하는데 현재 들어간 오브젝트 순서로 읽어들이고 있다.
 * 하지만, 실제 게임에서는 slotIndex순으로 제거해야 하므로, slotIndex를 기반으로 정렬해서 제거해줘야 한다.
 * 
 * <v3.0 - 2024_0104_최원준>
 * 1- InventoryInfo에 아이템 데이터를 전송하기 위한 passItemInfoList를 선언하고
 * SetOverlapCount메서드에서 내부적으로 삭제할 아이템을 일시적을 담아서 전달하는 형식으로 구현하였음.
 * 
 * 메서드 내부에서 삭제하지 않고 Info클래스로 전송하는 이유는 정보수정은 Inventory클래스가 담당하지만, 
 * 오브젝트 권한은 InventoryInfo클래스에게 넘거야 하기 때문이며,
 * 수량을 다시 되돌리는 작업이 필요할 때가 있기 때문 
 * (ex. 잡화아이템 50개를 먹었는데 슬롯이 비어있지 않다면, 기존의 아이템에 다 들어가는지를 알아야 한다.)
 * 
 * 2- 전달인자 변수명 isLatestRemove, Reduce등을 isLatestModify로 통일
 * 
 * 3- IsAbleToAddMisc메서드 추가
 * IsEnough, IsExist 등은 감소나 제거를 목적으로 하지만 
 * 이 메서드는 추가 하기위해 기존 수량에 들어가기 충분한지, 그리고 새로운 아이템을 생성하기 위한 슬롯공간이 있는지 여부를 반환받기 위함.
 * 
 * 
 * <v3.1 - 2024_0104_최원준>
 * 1- IsAbleToAddMisc메서드에서 가능여부만 반환해야하는데 오브젝트리스트를 새로 생성하고있었던 점 수정
 * 2- 정렬기준 메서드 변수명 CompareByIndex를 CompareBySlotIndexEach로 변경
 * 3- SetOverlapCount메서드 while문을 통해 오름차순, 내림차순을 수행하던 로직을 내부 메서드를 통한 for문 호출로 변경
 * 4- SetOverlapCount 메서드의 매개변수 passList를 removeList로 변경하고 마지막 인자로 변경
 * 
 * 
 * <v3.2 - 2024_0105_최원준>
 * 1- FindNearstSlotIdx코드 로직 수정
 * a. 아이템이 아무것도 들어있지 않을때 초기값을 0으로 주게하였음.
 * b. for문 내부 break문 추가 (타겟인덱스를 찾으면 더이상 수정없도록 해야함)
 * c. 인덱스 리스트의 마지막에 도달했으면서 슬롯은 아직 남아있는 경우, 다음 인덱스를 타겟으로 설정하는 조건 추가
 * 
 * <v3.3 - 2024_0108_최원준>
 * 1- IsAbleToAddMisc메서드 내부 로직에서 생성 가능한데도 불가능을 반환하던 코드를 수정
 * reamainCount>0일때 슬롯갯수가 부족할때 false를 반환하고, remainCount<=0 이하일 때 무조건 true를 반환하여야 했으나 false를 반환하였던 점 수정
 * 추가로 remainCount가 탐색 중일 때 0이하로 떨어진다면 바로 true를 반환하도록 수정
 * 
 * <v3.4 - 2024_0110_최원준>
 * 1- IsExist에 인벤토리 제거옵션(isReduce)을 삭제하고 오브젝트 갯수(itemObjCount) 검출기능을 추가, 주석도 그에맞게 수정
 * 이유는 복잡한 혼합메서드를 줄이고 오브젝트 검색이라는 본연의 목적에 초점을 더 맞추기 위함.
 * 
 * <v3.5 - 2024_0111_최원준>
 * 1- 퀘스트 아이템 클래스 추가로 인해 
 * FindNearstSlotIdx메서드에 아이템을 읽어오는 사전 길이를 수정
 * 
 * <v3.6 - 2024_0112_최원준>
 * 1- Inventory를 개별사전에서 사전배열로 구조를 변경한 관계로 FindNearstSlotIdx메서드의 퀘스트아이템 로직 수정
 * 
 * <v4.0 - 2024_0114_최원준>
 * 1- FindNearstSlotIdx메서드에서 인덱스리스트를 구하던 부분을 GetSlotIndexList 메서드로 만들어 재사용
 * 
 * 2- FindNearstSlotIdx메서드에서 dicLen을 (int)ItemType.None로 구하던 것을 Inventory의 dicLen으로 변경 후
 * itemDic을 구할 때 (ItemType)i를 이용해서 구하던 것을 dicType[i]로 변경
 * 
 * 3- IsRemainSlot메서드 정의
 * 지정한 인덱스의 슬롯이 남아있는지 여부를 반환하는 메서드로 GetSlotIndexList메서드를 내부적으로 사용
 * 
 * 4- ReadSlotIdxToIndexList 메서드명을 ReadSlotIndexByItemDic으로 변경
 * FindNearstSlotIdx메서드명을 GetLatestSlotIndex로 변경
 * 변수명 findSlotIdx를 latestSlotIndex로 변경
 * 
 * 
 * (이슈)
 * 1- IsRemainSlot에서 나중에 탭타입을 인자로 받아야함.
 * (이유는 탭상에서의 슬롯위치를 의미하기 때문)
 * => 공유 슬롯 구현 이후
 * 
 * <v4.1 - 2024_0115_최원준>
 * 1- GetDicIndex메서드를 Inventoy.cs로 옮김.(GetDicIdxByItemType과 중복기능)
 * 
 * <v4.2 - 2024_0115_최원준>
 * 1- GetSlotIndexList메서드를 
 * a. 단일 종류의 아이템 SlotIndex만 Index를 Add시키던 부분을 Tab에 해당하는 모든 아이템 종류의 Index를 Add시킬 수 있도록 수정
 * b. ItemType.None 선택인자를 삭제하고 명시적 인자로 전환
 * 
 * 
 * 2- ReadSlotIndexByItemDic메서드 명을 ReadSlotIndexFromItemDic으로 수정
 * 
 * 3- GetLatestSlotIndex(string itemName, bool isIndexAll) 오버로딩 메서드 삭제
 * (ItemType을 인자로 전달받는 메서드만 주로 사용하므로)
 * 
 * 4- GetLatestSlotIndex메서드의 내용 GetSlotCountLimitTab에 맞게 수정
 * 
 * 5- 기존의 ItemType과 slotIndex를 인자로 받는 IsRemainSlot 메서드를 IsRemainSlotCertain으로 변경하고 
 * ItemType만 인자로 받는 IsRemainSlotNearst메서드 작성
 * (지정 슬롯 자리가 있는지 여부를 반환하는 메서드와 아무 슬롯 자리가 있는지 여부를 반환하는 메서드로 나누었음)
 * 
 * 6- 일관성을 위해 GetLatestSlotIndex메서드명을 GetItemSlotIndexNearst로 변경
 * 
 * <v5.0 - 2024_0116_최원준>
 * 1- IsRemainSlotCertain, GetSlotIndexList, GetItemSlotIndexNearst 메서드에 
 * 현재 탭 상태 isActiveTabAll을 받도록 수정, ItemType.None 전달 시 예외를 처리하였음
 * (이유는 탭 상태에따라서 로직이 달라져야 하기 때문이며, 상태변수를 하나 더 받아서 코드에 일관성을 주기 위함)
 * 
 * 2- 메서드명 변경 IsRemainSlotNearst, IsRemainSlotCertain, GetItemSlotIndexNearst
 * -> IsRemainSlotIndirect, IsRemainSlotDirect, GetItemSlotIndexIndirect
 * 
 * 3- GetSlotIndexList메서드에서 isActiveAll에 따라서 tabType을 전체탭으로 구해야했으나
 * 계속해서 개별탭으로 구함으로서 인덱스리스트가 개별 인덱스만 받아오던 점 수정
 * 
 * <v5.1 - 2024_0116_최원준>
 * 1- GetSlotIndexList메서드에서 GetItemDic관련 null검사문 추가
 * 2- IsRemainSlotDirect메서드 내부 다른 메서드들 예외처리문 추가
 * 3- IsRemainSlotDirect메서드에서 슬롯인덱스가 슬롯제한수를 넘어가는 경우 예외를 처리하던 부분을
 * false를 반환하도록 수정
 * ( 인덱스를 제한 수 이상으로 할당하려하는 경우 실패처리 )
 * 
 * <v5.2 - 2024_0118_최원준>
 * 1- GetItemSlotIndexIndirect메서드 내부 printDebugInfo주석처리
 * 
 * <v5.3- 2024_0122_최원준>
 * 1- IsEnough메서드에서 처음 아이템 존재여부를 검사할 때 인자로 들어온 count를 넣어서 IsExist를 호출하고 있었기 때문에
 * 잡화아이템의 경우 제대로 검사가 되지 않았던 문제를 수정
 * (=> 먼저 count를 넣지 않고 존재하는지 검사 한 후에 
 * 잡화아이템의 경우 IsEnoughOverlaCount를, 비잡화의 경우 IsExist에 count넣어서 한 번 더 검사들어가도록 수정)
 * 
 * <v5.4 - 2024_0123_최원준>
 * 1- IsEnough, IsEnoughOverlapCount, SetOverlapCount에서
 * GetItemTypeInExists메서드를 GetItemTypeIgnoreExists로 변경
 * 
 * 이름을 기반으로 타입을 찾을 때는 InExists가 필요없는데 이는
 * 아이템을 Add할 때 기존 아이템이 있는지 여부가 중요하지 않으며,
 * 해당 이름의 아이템 종류에 따라 조건검사문을 들어가는 경우가 많기 때문.
 * 
 * 2- IsEnoughOverlapCount에서 itemObjList의 null값 검사를 예외로 처리하고 있었는데
 * 이는 실패조건이지 예외처리 조건이 아니었음. 따라서 false처리
 * (해당 이름의 아이템이 없는 상태에서 이 메서드 발동 시 예외가 발생했을 가능성이 큼)
 * 
 */


namespace InventoryManagement
{

    /// <summary>
    /// 아이템 이름과 수량을 나타내는 구조체입니다.<br/>
    /// 인벤토리 메서드의 전달인자로 사용되며,<br/>
    /// 해당 아이템의 이름이 인벤토리에 존재하는 지, 아이템 수량은 충분한 지 확인하는 용도로 사용됩니다.
    /// </summary>
    [Serializable]
    public struct ItemPair
    {
        public string itemName;
        public int overlapCount;

        public ItemPair(string itemName, int overlapCount)
        {
            this.itemName = itemName;
            this.overlapCount = overlapCount;
        }

    }




    public partial class Inventory
    {
        /// <summary>
        /// InventoryInfo클래스에 필요 시 아이템 정보를 전송하기 위해 사용하는 ItemInfo를 임시적으로 담는 리스트입니다.<br/>
        /// 현재 전송리스트 미 전달 시 대체인자로 SetOverlapCount메서드에서 내부적으로 사용됩니다.
        /// </summary>
        List<ItemInfo> tempInfoList = new List<ItemInfo>();



        /// <summary>
        /// 해당 종류의 아이템이 들어갈 아무 슬롯이 비었는지 여부를 판단합니다.<br/>
        /// 인자로 어떤 종류의 아이템인지 전달받습니다.<br/><br/>
        /// *** ItemType.None을 전달하면 예외를 던집니다. ***
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public bool IsRemainSlotIndirect( ItemType itemType )
        {
            // 전체 탭에 들어갈 자리가 있어도 특정 탭에 들어갈 자리가 없으면 안되기 때문
            if(itemType == ItemType.None)
                throw new Exception("해당 종류의 인자를 전달할 수 없습니다.");

            if( GetCurRemainSlotCount(itemType)>0 )
                return true;

            return false;
        }



        
        /// <summary>
        /// 해당 종류의 아이템이 들어갈 특정 슬롯이 비었는지 여부를 반환합니다.<br/>
        /// 인자로 어떤 종류의 아이템인지, 해당하는 슬롯의 인덱스값과 전체 탭 상태를 전달 받습니다.<br/>
        /// *** ItemType.None이 전달되면 예외가 발생합니다. ***
        /// </summary>
        /// <returns>해당 탭의 슬롯에 아이템이 들어갈 공간이 있다면 true, 없다면 false를 반환</returns>
        public bool IsRemainSlotDirect(ItemType itemType, int slotIndex, bool isActiveTabAll)
        {
            if(itemType==ItemType.None)
                throw new Exception("정확한 아이템 타입이 필요합니다.");
            if(slotIndex<0)
                throw new Exception("슬롯 인덱스가 정확하지 않습니다. 0이하가 전달되었습니다.");
            if(indexList==null)
                throw new Exception("참조 리스트가 존재하지 않습니다.");  

            // 넣을 슬롯 인덱스가 제한수에 도달한 경우 자리가 없으므로 false를 반환
            if(slotIndex >= GetSlotCountLimitTab(itemType, isActiveTabAll))
                return false;


            // 인덱스 리스트를 전달하여, 해당하는 아이템 종류의 슬롯 인덱스 리스트를 구합니다.
            GetSlotIndexList( ref indexList, itemType, isActiveTabAll );
                        
            // 구한 인덱스 리스트에 아무것도 없는 경우 자리가 있으므로 바로 true를 반환합니다
            if( indexList.Count==0 )    
                return true;

            // 오름차순으로 정렬합니다.
            indexList.Sort();

            int idx;

            // 인덱스 리스트를 모두 읽어들여 slotIndex가 존재하는 지 찾습니다.
            for( idx = 0; idx<indexList.Count; idx++ )
            {
                // 오름 차순으로 정렬된 인덱스 리스트의 값이 지정 슬롯 인덱스보다 크다면 (빈자리가 있으므로) true를 반환합니다.
                if( indexList[idx] > slotIndex )
                    return true;

                // 인덱스 리스트에 해당하는 슬롯 인덱스가 존재한다면 (기존의 아이템이 자리를 차지 하고 있으므로) false를 반환합니다.
                if( indexList[idx] == slotIndex)
                    return false;
            }

            // idx가 슬롯제한수에 도달했다면 false를 반환합니다.
            if( idx == GetSlotCountLimitTab(itemType, isActiveTabAll) )
                return false;
            // 인덱스 리스트에서 slotIndex를 찾지 못했다면 (슬롯제한에 도달하지 않은상태이기에 빈자리가 있으므로) true를 반환합니다.
            else
                return true;
        }







        /// <summary>
        /// 아이템 종류와 동일한 탭을 가지는 아이템들의 딕셔너리를 읽어들여 
        /// 참조로 전달한 indexList에 아이템의 슬롯 인덱스를 담습니다.<br/><br/>
        /// ** 메서드 호출 시 인덱스 리스트는 초기화되며, 정렬되지 않은 상태로 반환됩니다. **<br/>
        /// ** 두번째 인자가 ItemType.None으로 전달되었다면 예외가 발생합니다. **
        /// </summary>
        /// <param name="indexList"></param>
        /// <param name="itemType"></param>
        private void GetSlotIndexList( ref List<int> indexList, ItemType itemType, bool isActiveTabAll )
        {
            if(itemType == ItemType.None )
                throw new Exception("정확한 아이템 타입이 필요합니다.");
            if(indexList==null)
                throw new Exception("참조 리스트가 존재하지 않습니다."); 

            // 기존의 인덱스 리스트를 초기화합니다.
            indexList.Clear();   

            // 아이템의 ItemType을 기반으로 해당하는 딕셔너리를 구합니다
            Dictionary<string, List<GameObject>> itemDic;


            TabType tabType;  

            // 전체탭이 활성화되어있지 않다면, 인자로 전달받은 ItemType에 해당하는 TabType을 직접 구합니다.
            if( !isActiveTabAll )
                tabType = ConvertItemTypeToTabType( itemType );            
            // 전체 탭이 활성화되어 있다면 tabType을 전체로 설정합니다.
            else
                tabType = TabType.All;
            
            
            // 해당 TabType에 속하는 ItemType 리스트와 리스트의 길이를 반환받습니다.
            int tabKindLen = ConvertTabTypeToItemTypeList( ref tabKindList, tabType );


            // 리스트의 길이만큼 순회합니다
            for(int i=0; i<tabKindLen; i++)
            {
                // 아이템 종류 별 사전을 하나씩 가져옵니다.
                itemDic = GetItemDic( tabKindList[i] );
                
                // 딕셔너리에 아무 아이템이 들어있지 않다면 다음으로 넘어갑니다.
                if(itemDic==null || itemDic.Count==0)
                    continue;

                // 오브젝트를 하나씩 읽어들여서 아이템 인덱스를 인덱스 리스트에 집어넣습니다.
                ReadSlotIndexFromItemDic(ref indexList, itemDic, isActiveTabAll );
            }
        }


        
        /// <summary>
        /// 슬롯 인덱스를 해당 딕셔너리로부터 하나씩 읽어들여 저장합니다.<br/>
        /// 인자로 인덱스 리스트 참조값과 어떤 딕셔너리에 추가할 것인지, 현재 탭 상태가 전체탭인지 여부를 전달해야 합니다.
        /// </summary>
        private void ReadSlotIndexFromItemDic(ref List<int> indexList, Dictionary<string, List<GameObject>> itemDic, bool isActiveTabAll )
        {   
            if( indexList==null || itemDic==null )
                throw new Exception("리스트 혹은 딕셔너리 참조값이 존재하지 않습니다.");
                

            // 해당 사전에서 아이템 정보를 하나씩 읽어옵니다.
            foreach(List<GameObject> itemObjList in itemDic.Values)     
            {
                foreach(GameObject itemObj in itemObjList)
                {
                    Item item = itemObj.GetComponent<ItemInfo>().Item;   

                    if( isActiveTabAll )  
                        indexList.Add(item.SlotIndexAll);   // 전체 슬롯 인덱스 기반으로 구하는 경우
                    else                
                        indexList.Add(item.SlotIndexEach);  // 개별 슬롯 인덱스 기반으로 구하는 경우
                } 
            }
        }
        



















        /// <summary>
        /// 가장 가까운 슬롯의 인덱스를 구합니다.<br/>
        /// 어떤 종류의 아이템을 넣을 것인지와 현재 탭 상태를 인자로 전달하여야 합니다.<br/>
        /// 전체탭상태라면 전체 슬롯 인덱스를, 개별탭 상태라면 개별 슬롯 인덱스를 반환받습니다.<br/><br/>
        /// *** ItemType.None을 전달할 경우 예외가 발생합니다. ***
        /// </summary>
        /// <returns> 아이템이 들어갈 탭에서의 슬롯 인덱스를 반환, 슬롯에 자리가 없다면 -1을 반환</returns>
        public int GetItemSlotIndexIndirect(ItemType itemType, bool isActiveTabAll)
        {
            // 인덱스리스트를 전달하여 사전에 들어있는 모든 아이템의 인덱스를 읽어들입니다.
            GetSlotIndexList(ref indexList, itemType, isActiveTabAll);

            indexList.Sort();   // 인덱스 리스트를 오름차순으로 정렬합니다.

            // 인덱스 리스트에 아이템이 아무것도 없는 경우 0을 반환
            if( indexList.Count==0 )            
                return 0;
                        
            int latestSlotIndex = -1;    // 찾을 슬롯 인덱스를 선언하고 초기값을 -1으로 설정합니다.

            // 인자로 전달한 아이템의 종류에 해당하는 탭의 슬롯의 칸 제한 수를 구합니다.
            int slotCountLimitTab = GetSlotCountLimitTab(itemType, isActiveTabAll); 

            //string debugInfo= "";
            //debugInfo += string.Format($"아이템 종류 : {itemType}\n");
            //debugInfo += string.Format($"전체 탭 여부 : {isActiveTabAll}\n");
            //debugInfo += string.Format($"탭 제한 수 : {slotCountLimitTab}\n");
            //Debug.Log(debugInfo);

            // 정렬 인덱스 예시 
            // 0, 1, 2, 3, 4
            // 0, 1, 4, 6, 9
            

            // 인덱스 숫자까지 i를 0부터 증가시키면서 빈 슬롯을 찾습니다 
            for(int i=0; i<slotCountLimitTab; i++)
            {
                // i번째 인덱스리스트에 저장된 인덱스와 i가 일치하지 않는다면 슬롯이 빈 것으로 판단
                if( indexList[i]!=i )
                {
                    latestSlotIndex = i;
                    break;
                }
                //인덱스 리스트의 마지막에 도달했으면서 슬롯은 아직 남아있는 경우
                else if( i==indexList.Count-1 && i!=slotCountLimitTab-1 ) 
                {
                    latestSlotIndex = i+1; // 다음 인덱스를 타겟으로 설정
                    break;
                }
            }

            //찾은 슬롯 인덱스를 반환합니다.
            return latestSlotIndex;
        }

















        /// <summary>
        /// 리스트의 Sort메서드에 인덱스 정렬기준을 전달하기 위한 기준 메서드<br/>
        /// 잡화아이템의 개별 슬롯 인덱스를 기준으로 해서 오름차순으로 정렬됩니다.<br/>
        /// </summary>
        public static int CompareBySlotIndexEach(GameObject itemObj1, GameObject itemObj2)
        {
            ItemMisc itemMisc1 = (ItemMisc)itemObj1.GetComponent<ItemInfo>().Item;
            ItemMisc itemMisc2 = (ItemMisc)itemObj2.GetComponent<ItemInfo>().Item;
            return itemMisc1.SlotIndexEach.CompareTo(itemMisc2.SlotIndexEach);
        }


        /// <summary>
        /// 인벤토리에 존재하는 해당 이름의 아이템 수량을 증가시키거나 감소시킵니다.<br/>
        /// 인자로 해당 아이템 이름과 수량, 삭제 대상 아이템의 참조값을 받을 전송 리스트, 조정방식을 전달해야 합니다.<br/><br/>
        /// 
        /// 아이템 수량을 감소시키려면 수량 인자로 음수를 전달, 증가시키려면 양수를 전달합니다.<br/>
        /// 각 아이템의 최소, 최대 수량에 도달하면 다음 아이템의 나머지 수량을 조절합니다.<br/><br/>
        /// 
        /// 기존 아이템이 수량이 감소로 인해 0 이되면 아이템이 인벤토리 목록에서 제거하고 전송 리스트에 아이템을 추가합니다.<br/>
        /// 만약 전송리스트에 null을 전달하면, 아이템을 전송하지 않고 오브젝트를 즉시 삭제합니다.(기본값:null)<br/>
        /// 수량 증가의 경우는 인벤토리 목록에서 제거하거나, 전송리스트에 추가하지 않습니다.<br/><br/>
        /// 
        /// 모든 기존 아이템이 더 이상 수량을 조절하지 못하는 경우는 나머지 초과 수량을 반환합니다.<br/>
        /// 초과 수량으로 인한 오브젝트 새로운 생성과 수량 감소로 인한 기존 오브젝트 삭제는 메서드 호출자에게 권한이 있습니다.<br/><br/>
        /// 최신 순, 오래된 순으로 수량 조절 방식을 결정할 수있습니다. (기본값: 최신순)<br/><br/>
        /// ** 아이템 이름이 해당 인벤토리에 존재하지 않거나, 잡화아이템이 아닌 경우 예외를 발생시킵니다. **
        /// </summary>
        /// <returns>기존의 오브젝트에 감소 혹은 추가하고 남은 초과 수량, 모든 기존 아이템에 해당 수량이 들어갔다면 0을 반환</returns>
        public int SetOverlapCount(string itemName, int inCount, bool isLatestModify=true, List<ItemInfo> removeList=null)
        {
            List<GameObject> itemObjList = GetItemObjectList(itemName);     // 인벤토리의 아이템 오브젝트 리스트 참조
            ItemType itemType = GetItemTypeIgnoreExists(itemName);              // 아이템 종류 참조
                        
            // 해당 이름의 아이템 오브젝트가 존재하지 않는 경우, 아이템의 종류가 잡화아이템이 아닌 경우 예외처리
            if(itemObjList==null)
                throw new Exception("아이템이 이 인벤토리에 존재하지 않습니다.");
            if(itemType != ItemType.Misc)
                throw new Exception("잡화 아이템이 아닙니다.");
            

            bool isInstantRemove = false;       // 즉시 삭제여부 판단 변수

            // 인자로 전달받은 전송 리스트가 null이라면 즉시 삭제 활성화
            if(removeList==null)
            {
                isInstantRemove = true;       
                removeList = this.tempInfoList;   // 임시리스트로 설정
            }

            int remainCount = inCount;          // 가산 또는 감산하고 남은 수량 (초기값 : 수량 전달인자)
            
            // 개별 슬롯 인덱스를 기준으로하여 오름차순으로 정렬합니다.
            itemObjList.Sort(CompareBySlotIndexEach);


            // 최신순으로 수정할 지 오래된 순으로 수정할 지를 판단하여 내부 메서드를 반복호출합니다.            
            if(isLatestModify)  
            {
                for(int i=itemObjList.Count-1; i>=0; i--)
                    if( SetCountInLoopByOrder(i) ) { break; }   // 호출 금지가 활성화되면 반복문을 종료합니다.
            }
            else                
            {
                for(int i=0; i<=itemObjList.Count-1; i++)
                    if( SetCountInLoopByOrder(i) ) { break; }   // 호출 금지가 활성화되면 반복문을 종료합니다.
            }


            // 삭제 리스트에 있는 아이템을 역순으로 순회합니다. 
            for( int i=tempInfoList.Count-1; i>=0; i--)
            {
                // 인벤토리 리스트에서 제거
                RemoveItem(tempInfoList[i]);
                                
                // 즉시 삭제가 활성화 되있다면 오브젝트 삭제
                if(isInstantRemove)
                    GameObject.Destroy(tempInfoList[i].gameObject);
            }

            // 즉시 삭제의 경우 전송리스트 초기화
            if(isInstantRemove)
                tempInfoList.Clear();   

            // 호출하고 남은 수량을 반환합니다.
            return remainCount; 
            



            // 아이템을 조회 순서에 따라서 하나씩 꺼내어 수량 정보를 조절하고,
            // 수량이 0이된 아이템을 tempInfoList에 담아주는 내부 메서드입니다.
            bool SetCountInLoopByOrder(int idx)
            {
                // 반복문을 돌 때마다 아이템 정보 참조를 위한 설정
                ItemInfo itemInfo=itemObjList[idx].GetComponent<ItemInfo>();
                ItemMisc itemMisc=(ItemMisc)itemInfo.Item;

                // 남은수량을 넣어서 새로운 남은 수량을 반환받습니다.
                remainCount=itemMisc.AccumulateOverlapCount( remainCount );

                // 수정이 끝난 오브젝트의 중첩 텍스트를 수정합니다.
                itemInfo.UpdateTextInfo();

                // 변경한 아이템의 수량이 0이 된 경우에는 삭제 리스트에 담습니다.
                // 바로 제거하지 않고 따로 담는 이유는 중간에 리스트의 Count 변동이 생기기 때문입니다.
                if( itemMisc.OverlapCount==0 )
                    tempInfoList.Add( itemInfo );

                // 남은 수량이 0이된 경우 외부 호출 금지를 활성화합니다.
                if(remainCount==0)  
                    return true;
                else
                    return false;
            }

        }

        

        /// <summary>
        /// 인벤토리에 해당 이름의 아이템의 중첩수량이 충분한지를 확인하는 메서드입니다.<br/>
        /// 인자로 아이템 이름과 수량이 필요합니다. (수량 미전달 시 기본 수량은 1개입니다.)<br/><br/>
        /// 세번 째 인자로 수량 감소모드를 선택하면 아이템의 중첩수량이 충분하다면 인자로 들어온 수량만큼 감소시키며, <br/>
        /// 0에 도달한 경우 아이템을 인벤토리에서 제거하고 파괴 시킵니다.<br/>
        /// 최신 순, 오래된 순으로 감소여부를 결정할 수있습니다. (기본값: 최신순)<br/><br/>
        /// ** 해당 이름의 아이템이 존재하지 않거나, 잡화 아이템이 아니거나, 수량을 잘못 전달했다면 예외를 발생시킵니다. **
        /// </summary>
        /// <returns>아이템 중첩수량이 충분하면 true를, 충분하지 않으면 false를 반환</returns>
        public bool IsEnoughOverlapCount(string itemName, int overlapCount=1, bool isReduce=false, bool isLatestModify=true)
        {           
            ItemType itemType = GetItemTypeIgnoreExists(itemName);              // 이름을 통한 아이템의 종류 구분
            
            
            if( itemType != ItemType.Misc)
                throw new Exception("해당 아이템이 잡화아이템이 아닙니다.");
            else if( overlapCount<=0 )
                throw new Exception("수량 전달인자는 1이상이어야 합니다.");
            

            List<GameObject> itemObjList = GetItemObjectList(itemName);     // 이름을 통한 아이템 오브젝트 리스트 참조

            // 해당 아이템 이름으로 오브젝트 리스트가 존재하지 않거나, 키값이 있지만 비었다면 실패처리
            if( itemObjList == null || itemObjList.Count==0 )
                return false;
                        
                                    
            int totalCount = 0;                         // 중첩수량을 누적 시키기 위한 변수 설정
            bool isTotalEnough = false;                 // 중첩수량이 충분한지 확인하기 위한 변수

            foreach(GameObject itemObj in itemObjList)  // 아이템을 하나씩 꺼내어 정보를 읽습니다.
            {
                ItemMisc itemMisc = (ItemMisc)itemObj.GetComponent<ItemInfo>().Item;
                totalCount += itemMisc.OverlapCount;    // 중첩수량을 누적시킵니다.
                   
                // 합계 누적 수량을 구할 때마다 인자로 들어 온 수량보다 큰지 확인합니다.
                if(totalCount >= overlapCount)  
                {
                    isTotalEnough = true;    // isTotalEnough를 true로 만들고 빠져나갑니다.
                    break;
                }
            }

            if(isTotalEnough)       // 합계 누적 수량이 인자로 들어온 수량을 초과하는 경우
            {
                if( isReduce )      // 감소 모드라면, 충분 여부를 반환하기 전에 해당 수량만큼 감소시킵니다.
                    SetOverlapCount(itemName, -overlapCount, isLatestModify, null);

                    return true;
            }
            // 합계 누적 수량이 인자로 들어온 수량을 초과하지 못하는 경우 실패를 반환합니다.
            else
                return false;
        }



        /// <summary>
        /// 아이템이 인벤토리에 존재하는지 여부를 반환합니다.<br/>
        /// 아이템의 종류에 상관없이 오브젝트 단위로 존재하는지 여부만 반환합니다.<br/>
        /// 수량인자의 기본 값은 1입니다.<br/><br/>
        /// 
        /// *** 수량인자가 0이하로 전달된 경우 예외를 발생시킵니다. *** <br/>
        /// </summary>
        /// <returns>아이템이 존재하는 경우 true를, 존재하지 않거나 수량이 충분하지 않으면 false를 반환</returns>
        public bool IsExist(string itemName, int itemObjCount=1)
        {
            if(itemObjCount<=0)
                throw new Exception("수량이 0이하로 전달되었습니다. 확인하여 주세요.");


            List<GameObject> itemObjList = GetItemObjectList(itemName);     // 인벤토리의 아이템 오브젝트 리스트 참조

            // 인벤토리에 오브젝트 리스트가 존재하지 않는 경우 false반환
            if( itemObjList==null )
            {
                Debug.Log( "오브젝트 리스트가 없습니다." );
                return false;
            }
            // 오브젝트 리스트가 존재하면서 들어있는 오브젝트 수량이 충분하다면,
            else if( itemObjList.Count-itemObjCount>=0 )
            {
                Debug.Log( "수량이 충분합니다." );
                return true;
            }
            // 오브젝트 수량이 충분하지 않다면,
            else
            {
                Debug.Log("수량이 충분하지않습다.");
                return false;
            }
        }


            
        


        /// <summary>
        /// 아이템의 종류와 상관없이 아이템이 해당 수량 만큼 인벤토리에 존재하는지 여부를 반환합니다.<br/>
        /// 아이템 이름과 수량을 인자로 받습니다.<br/><br/>
        /// 일반 아이템은 오브젝트의 갯수를 의미하며, 잡화 아이템은 중첩수량을 의미합니다.<br/>   
        /// 해당 수량만큼 감소 및 파괴옵션을 지정할 수 있습니다. (기본값: 수량 1, 수량 감소 및 파괴 안함, 최신순 감소 및 파괴)<br/><br/>
        /// *** 수량 인자가 0이하라면 예외를 발생시킵니다. ***
        /// </summary>
        /// <returns>아이템이 존재하며 수량이 충분한 경우 true를, 존재하지 않거나 수량이 충분하지 않다면 false를 반환</returns>
        public bool IsEnough(string itemName, int count=1, bool isReduceAndDestroy = false, bool isLatestModify=true)
        {
            // 아이템이 하나라도 존재한다면
            if( IsExist( itemName ) )
            {
                // 아이템 종류를 확인합니다.
                ItemType itemType = GetItemTypeIgnoreExists( itemName );

                // 잡화 아이템이라면
                if( itemType==ItemType.Misc )
                {
                    // 중첩수량을 검사합니다.
                    if( IsEnoughOverlapCount( itemName, count, false, isLatestModify ) )
                    {
                        // 수량감산 및 파괴옵션이 걸려있다면, 수량감산 및 수량 0이하 파괴
                        if( isReduceAndDestroy )
                            SetOverlapCount( itemName, -count, isLatestModify, null );

                        return true;    // 옵션여부와 상관없이 true 반환
                    }
                    // 수량이 충분하지 않다면,
                    else
                        return false;   // 옵션여부와 상관없이 false 반환 
                }
                // 잡화 아이템이 아니라면
                else
                {
                    // 오브젝트 수량을 검사합니다.
                    if( IsExist( itemName, count ) )
                    {
                        // count갯수만큼 제거합니다.
                        if( isReduceAndDestroy )
                        {
                            for( int i = 0; i<count; i++ )
                            {
                                ItemInfo rItemInfo = RemoveItem( itemName, isLatestModify );
                                GameObject.Destroy( rItemInfo.gameObject );
                            }
                        }

                        return true;        // 옵션여부와 상관없이 true 반환
                    }
                    else
                        return false;
                }

            }
            // 아이템이 존재하지 않는다면,
            else 
                return false;
        }


        /// <summary>
        /// 아이템의 종류와 상관없이 아이템이 해당 수량 만큼 인벤토리에 존재하는지 여부를 반환합니다.<br/>
        /// 아이템 이름과 수량으로 이루어진 구조체 배열을 인자로 받습니다.<br/><br/>
        /// 일반 아이템은 오브젝트의 갯수를 의미하며, 잡화 아이템은 중첩수량을 의미합니다.<br/>   
        /// 해당 수량만큼 감소 및 파괴옵션을 지정할 수 있습니다. (기본값: 수량 1, 수량 감소 및 파괴 안함, 최신순 감소 및 파괴)<br/><br/>
        /// *** 수량 인자가 0이하라면 예외를 발생시킵니다. ***
        /// </summary>
        /// <returns>아이템이 존재하며 수량이 충분한 경우 true를, 존재하지 않거나 수량이 충분하지 않다면 false를 반환</returns>
        public bool IsEnough( ItemPair[] pairs, bool isReduceAndDestroy=false, bool isLatestModify=true)
        {
            int allEnough=0;
                        
            // 모든 아이템이 조건을 충족하는 지 확인
            foreach(ItemPair pair in pairs )
            {
                if( IsEnough(pair.itemName, pair.overlapCount, false, isLatestModify) )
                    allEnough++;
            }
                                    

            // 모두 조건을 충족하는 경우
            if(allEnough==pairs.Length)
            {
                // 감소모드인 경우
                if(isReduceAndDestroy)
                {
                    foreach(ItemPair pair in pairs )
                        IsEnough(pair.itemName, pair.overlapCount, true, isLatestModify);
                }

                return true;
            }
            // 하나라도 조건을 충족하지 않는 경우 실패를 반환
            else
                return false;
        }

        


        /// <summary>
        /// 잡화 아이템을 해당 수량 만큼 생성한다고 가정할 때, 인벤토리에 들어가기 충분한지를 반환해주는 메서드입니다.<br/>
        /// 아이템 이름과 생성할 수량을 넣어야 합니다.<br/><br/>
        /// 실제 아이템 오브젝트가 있는 경우는 GetCurRemainSlotCount를 호출하는 것이 성능적으로 빠릅니다.<br/><br/>
        /// *** 잡화 아이템이 아니거나, 수량전달인자가 0이하면 예외를 던집니다. ***
        /// </summary>
        /// <returns>아이템을 생성하기 위한 슬롯이 충분하다면 true를, 부족하다면 false를 반환</returns>
        public bool IsAbleToAddMisc( string itemName, int overlapCount )
        {
            ItemType itemType = GetItemTypeIgnoreExists( itemName );

            // 해당 아이템의 종류가 잡화아이템이 아닌 경우와 수량이 잘못 전달 된 경우 예외처리
            if( itemType!=ItemType.Misc )
                throw new Exception( "해당 이름 아이템의 종류가 잡화 아이템이 아닙니다." );
            if( overlapCount<=0 )
                throw new Exception( "수량 전달인자는 1이상이어야 합니다." );

            // 남은 수량을 최초에 들어온 수량으로 설정
            int remainCount = overlapCount;

            // 아이템 별 최대수량 참조
            int maxOverlapCount = GetItemMaxOverlapCount( itemName );

            // 현재 아이템 생성가능 한 최대 슬롯 갯수
            int curRemainSlotCnt = GetCurRemainSlotCount( itemType );


            // 오브젝트 리스트 참조를 설정합니다.
            List<GameObject> itemObjList = GetItemObjectList( itemName );

            // 기존 오브젝트 리스트가 있는 경우는
            if( itemObjList!=null )
            { 
                // 아이템을 하나씩 꺼내어 remainCount를 감산합니다.
                foreach( GameObject itemObj in itemObjList )
                {
                    ItemMisc itemMisc = (ItemMisc)itemObj.GetComponent<ItemInfo>().Item;

                    // 해당 아이템을 최대수량으로 만들기 위한 수량이 얼마 남았는 지를 계산합니다.
                    remainCount-=( maxOverlapCount-itemMisc.OverlapCount );

                    // 남은 수량이 기존 아이템에 포함가능 하다면, 
                    // 오브젝트를 새로 생성할 필요 없으므로 더 이상 탐색을 중단하고 성공을 반환합니다.
                    if(remainCount<=0)
                        return true;
                }
            }
            

            
            // 오브젝트 리스트가 아예 없는경우의 그대로인 남은 수량 또는
            // 기존 오브젝트 리스트에서 제외 시킨 만큼의 남은 수량이 0이상이라면,


            // 생성 할 오브젝트 갯수 설정 : 최종적으로 남은 수량에서 오브젝트 1개별 최대수량으로 나눈 몫 (나누어 떨어지는 경우)
            int createCnt = remainCount / maxOverlapCount;
            int remainder = remainCount % maxOverlapCount;

            // 나누어 떨어지지 않아, 남는 소수점이 생길 때는 오브젝트가 하나 더 필요하므로 생성 갯수를 올립니다. 
            if(remainder>0)
                createCnt++;


            // 남은 슬롯 갯수가 생성해야 할 오브젝트 갯수보다 많거나 같다면 생성가능 반환
            if( curRemainSlotCnt >= createCnt )
                 return true; 
            // 남은 슬롯 갯수가 부족하다면, 생성 불가능 반환
            else
                return false;
        }

                






    }

    

}