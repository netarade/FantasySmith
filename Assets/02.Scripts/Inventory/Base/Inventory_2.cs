using ItemData;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using WorldItemData;

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
 * <v2.0 - 2023_1226_최원준>
 * 1- UpdateAllItem메서드를 UpdateAllItemInfo메서드로 변경
 * 2- LoadAllItem 주석처리 
 * - 개별 종류로 직렬화했을 때 잘 불러와진다면 해당 메서드 삭제 예정
 * 
 * <v2.1 - 2023_1227_최원준>
 * 1- Item 프리팹 계층구조 변경으로 인해(3D 오브젝트 하위 2D오브젝트) 
 * UpdateAllItemInfo메서드의 ItemInfo를 참조하는 GetComponent메서드를 GetComponentInChildren으로 변경
 * 
 * <v2.2 - 2023_1229_최원준>
 * 1- 네임스페이스명 CraftData에서 InventoryManagement로 변경
 * 2- 파일명 변경 Inventory_p2-> Inventory_2
 * 
 * <v2.3 - 2023_1230_최원준>
 * 1- CreateItem메서드 추가
 * AddItem메서드의 내용을 옮김.
 * 
 * <v3.0 - 2023_1231_최원준>
 * 1- CreateItem메서드 삭제 하고 AddItem메서드에서 ItemInfo 또는 GameObject를 인자로 전달받아서 해당 딕셔너리에 넣도록 구현
 * 
 * 2- AddItem, RemoveItem, SetOverlapCount 인자별 오버로딩 정의
 * 
 * 3- 다양한 참조를 돕는 메서드 정의
 * IsContainsItemName - 아이템 이름으로 해당 인벤토리에 존재하는지 
 * 
 * GetItemDicInExists - 현재 인벤토리 목록에 이미 존재하는 아이템을 담을 딕셔너리를 반환
 * GetItemDicIgnoreExists - 현재 인벤토리에 목록에 존재하지 않는 아이템을 담을 딕셔너리를 반환
 * 
 * GetItemTypeInExists - 딕셔너리에 존재하는 아이템의 타입반환
 * GetItemTypeIgnoreExists - 월드 딕셔너리에 존재하는 아이템의 타입 반환
 * GetItemObjectList - 딕셔너리에서 오브젝트 리스트를 반환
 * 
 * 
 * <v3.1 - 2024_0102_최원준>
 * 1- AddItem메서드 string, Item클래스 기반 오버로딩 메서드 삭제
 * 이유는 미리 생성되어있는 오브젝트가 아닌 경우 포지션 정보 업데이트 메서드 호출이 곤란하기 때문에
 * 결국 InventoryInfo메서드에서 완성해야 할 기능이므로 이미 생성되어있는 오브젝트나 스크립트를 넣는 메서드만 있는것이 낫다고 판단.
 * 
 * (Remove메서드의 오버로딩을 놔두는 이유는 string기반으로 검색이 메인이기 때문에, 나머지 오버로딩은 Info클래스에서 그대로 재사용 가능)
 * 
 * 2- AddItem GameObject인자를 받는 메서드 예외처리 작성
 * 
 * 3- SetOverlapCount메서드 삭제 및 관련 기능 Inventory_3.cs로 구현
 * 
 * 4- AddItem 및 RemoveItem에 예외처리 문장 추가
 * 5- RemoveItem GameObject와 ItemInfo 인자 메서드를 Item인자 메서드 호출에서 바로 string 메서드로 호출되도록 변경 
 * 
 * <v3.2 - 2024_0102_최원준>
 * 1- ItemType enum을 int형으로 반환받는 메서드인 GetItemTypeIndexIgnoreExists 메서드를 추가
 * 
 * 
 * 
 * 
 * [추가 사항]
 * 1- AddItem 메서드로 아이템을 넣지만 포지션 업데이트를 외부 InventoryInfo스크립트에서 해줘야 함.
 * (포지션 업데이트를 현재활성화 탭기준으로 해놔야 한다.)
 * 
 * 2- AddItem 메서드로 인벤토리에 넣을 때, 개별 슬롯 인덱스와 전체 슬롯 인덱스를 둘다 찾아서 넣어줘야 한다.
 * (CreateManager도 수정필요)
 * 
 * 
 * <v4.0 - 2024_0103_최원준>
 * 1- RemoveItem메서드의 반환값을 bool이 아니라 GameObject로 변경
 * 이유는 Inventory클래스에서는 오브젝트의 파괴 처리를 못하기 때문에 Info클래스로 참조값을 넘겨야 하기 때문
 * 
 * 2- GameObject, Item 클래스를 인자로 받는 오버로딩 메서드 제거
 * 코드가 쓸데없이 길어지기 때문에 필요 시 추가할 예정
 * 
 * 3- AddItem메서드 주석 수정
 * 
 * <v4.1 - 2024_0103_최원준>
 * 1- RemoveItem에서 ItemInfo를 받는 메서드, isLatest를 받는 인자를 삭제하고, 검색 삭제 방식이 아닌 참조삭제 방식으로 구현
 * 이유는 동일 아이템을 아무것이나 삭제하는 것이 아니라 수량등의 정보를 조절을 한 특정아이템을 정확히 목록에서 삭제해야 하는 경우가 생기기 때문 
 * 
 * 2- GetItemObjectList메서드 내부에 isNewIfNotExist 인자 옵션 추가
 * 아이템 오브젝트리스트가 인벤토리 내부 사전에 존재하지 않아도 새롭게 생성 및 추가하여 반환이 가능하도록 구현 
 * 
 * 3- UpdateAllItemInfo 메서드를 InventoryInfo클래스로 옮김.
 * InventoryInfo 클래스에서 인벤토리 로드시 사용하였으나, 내부처리보다 외부처리에 가깝고
 * 인벤토리에 존재하는 모든 아이템을 대상으로 OnItemCreated메서드를 호출하여야 하나 전달인자로 InventoryInfo 참조를 전달해야 하기 때문
 * 
 * 4- GetItemMaxOverlapCount메서드 구현
 * 월드 아이템으로부터 정보를 받아서 아이템 별 동일한 최대 충첩허용 갯수를 반환하는 용도 
 * (신규 아이템 추가 시 어느정도의 오브젝트를 추가해야하는 지 알기 위해 오브젝트가 아예 없는 경우에 참조를 못하게 되므로 필요)
 * 
 * 
 * <v4.2 - 2024_0104_최원준>
 * 1- AddItemToDic 내부에 SetCurItemObjCount메서드문장 삽입
 * 
 * <v4.3 - 2024_0105_최원준>
 * 1- GetItemTypeIndexIgnoreExists메서드 
 * createManager에서 메서드를 불러오면 ItemType.None으로 반환되어 예외처리 안되고 값이 반환되던 점 수정
 * 
 * 2- GetItemDicIgnoreExsists 이름 인자를 받는 오버로딩 메서드
 * 예외를 던지던 부분에서 null 반환으로 수정
 * 
 * 3- 기타 string 이름을 인자로 받는 메서드들에게서 이름이 일치하지 않으면 null이나 None반환하였는데, 예외를 던지도록 수정
 * 
 * 4- AddItemToDic메서드를 private으로 수정
 * 
 * <v4.4 - 2024_0110_최원준>
 * 1- RemoveItem메서드에서 itemObjList가 null일때의 조건검사문 추가 
 * (딕셔너리 null로는 완전히 삭제된 아이템인 경우 반복호출가능성이 있음)
 * 
 * 2- RemoveItem메서드에서 itemObjList가 itemObjList.Count==0일때의 조건 검사문 추가
 * 현재 오브젝트리스트의 count가 0이되어도 한번 생성되면 남는 상태로 존재
 * (이는 1개짜리 오브젝트가 삭제와 생성이 반복되는 경우가 있기 때문에 리스트를 완전히 지우지 않기 때문)
 * 
 * <V5.0 -2024_0111_최원준>
 * 1- 퀘스트 아이템 클래스를 추가하면서 관련 메서드를 수정
 * 
 * <v5.1 -2024_0111_최원준>
 * 1- 인벤토리를 딕셔너리 배열로 전환하면서 관련 메서드 수정하고 최적화
 * 
 * <v5.2 - 2024_0114_최원준>
 * 1- GetDicIndex메서드 추가
 * DicType과 일치하는 인덱스를 구하여 slotCountLimit 배열에 접근하기 위함
 * 
 * 2- GetSlotCountLimit 메서드 추가
 * ItemType을 인자로 넣어 해당 종류의 slotCountLimit을 빠르게 구하기 위함
 * 
 * <v5.3 - 2024_0115_최원준>
 * 1- InventoryInfo클래스에서 SetItemSlotIndexBothLatest메서드를 옮겨옴
 * 이유는 슬롯 인덱스를 Inventory클래스에서 밖에 구할 수 없으며, 
 * AddItem시에 한 동작으로 이루어져야 순서관계가 무너지지 않기 때문
 * 
 * 2- GetSlotCountLimit메서드 삭제
 * Inventory.cs에 GetSlotCountLimitDic과 GetSlotCountLimitTab으로 구분하여 새롭게 작성
 * 
 * <v6.0 - 2024_0115_최원준>
 * 1- SetItemSlotIndexBothLatest메서드명을 SetItemSlotIndexNearst로 변경
 * SetItemSlotIndex메서드명을 SetItemSlotIndexCertain로 변경후 작성완료
 * (최신슬롯 인덱스를 설정해주는 메서드와 지정슬롯을 설정해주는 메서드로 구분)
 * 
 * 2- AddItem메서드에 지정슬롯에 추가하는 선택인자 추가
 * 
 * 3- SetItemSlotIndexNearst메서드 내부의 GetItemSlotIndexNearst메서드 매개변수를 수정하여 호출
 * 
 * 4- SetItemSlotIndexCertain메서드 내부의 GetItemSlotIndexNearst메서드 매개변수 수정하여 호출
 * 
 * 5- 메서드명 변경 SetItemSlotIndexNearst, SetItemSlotIndexCertain -> SetItemSlotIndexIndirect, SetItemSlotIndexDirect
 * 
 * 6- SetItemSlotIndexIndirect메서드에서 IsRemainSlotIndirect 조건에 !를 안붙였던 점 수정
 * 
 * 7- GetItemDicIgnoreExsists메서드 주석 수정
 * (아이템 종류에 해당하는 사전이 없는 경우, 호출 시 예외가 발생하는 메서드이므로)
 * 
 * 8- GetItemDicIgnoreExists메서드명을 GetItemDic으로 변경하였으며, GetItemDicInExists메서드, IsContainsItemName메서드를 삭제하였음.
 * 이유는 이제 선택한 종류의 딕셔너리만 보관하는 방식으로 변경되었기 때문에 없는 딕셔너리를 가져와서는 안되고,
 * GetItemDicInExists메서드의 경우는 딕셔너리에 아이템이 들어있어야만 반환이 가능한 메서드라 쓰이지 않고 GetItemObjList를 더 많이쓰게된다.
 * 마찬가지 이유로 IsContainsItemName메서드도 딕셔너리에 아이템 이름이 있는지 검사하는 것보다 바로 ObjList를 반환받는 것이 빠르다.
 * 
 * 9- GetItemDic에서 딕셔너리를 찾지 못한 경우 예외를 던지던 부분에서 null값을 반환하도록 수정하였음. (해당부분 주석보완)
 * 이유는 Serialize메서드에서 반드시 호출시켜야 하기 때문
 * 
 * <v6.1 - 2024_0122_최원준>
 * 1- GetItemTypeInExists메서드를 삭제
 * 
 * 이름을 기반으로 타입을 찾을 때는 InExists가 필요없는데 이는
 * 아이템을 Add할 때 기존 아이템이 있는지 여부가 중요하지 않으며,
 * 해당 이름의 아이템 종류에 따라 조건검사문을 들어가는 경우가 많기 때문.
 * 
 * 2- GetItemDic메서드를 Inventory.cs의 GetDicIndex메서드를 이용하는 형태로 구현
 * (유지보수성을 높이기 위해)
 *
 * 3- GetItemObjectList메서드의 코드를 바로 딕셔너리에 이름으로 접근해서 검사 후 반환하는 방식에서 
 * 이름을 기반으로 GetDicIndex를 먼저 구해서 검사하는 방식으로 변경
 * 
 * (이유는 해당이름의 아이템 사전이 아예 없을 때 예외를 던져 실수를 방지 할 수 있기 때문,
 * 새로 생성하기 옵션의 코드에서도 재사용할 수 있기 때문)
 * 
 * 4- RemoveItem의 string itemName을 인자로 받는 메서드에서
 * itemObjList가 null일 때 실패하여 null을 반환하던 것을 예외처리하도록 변경
 * ItemInfo오버로딩 메서드의 예외처리와 일치시킴
 * (이유는 IsItemEnough를 검사하지 않고 RemoveItem호출한 후 다른 메서드를 연계하는 것을 방지하기 위해)
 * 
 * <v7.0 - 2024_0124_최원준>
 * 1- 딕셔너리 저장형식을 GameObject기반에서 ItemInfo로 변경하면서 관련 메서드 수정
 *(AddItemToDic, FillExistItemOverlapCount, RemoveItem(ItemInfo), RemoveItem(string), 
 *GetItemDic(ItemType), GetItemDic(string), GetItemInfoList )
 * 
 * <v7.1 - 2024_0126_최원준>
 * 1- 유틸리티 적인 성격의 메서드를 Invetory_4.cs로 이동
 * (GetItemTypeIgnoreExists GetItemDic GetItemMaxOverlapCount GetItemInfoList)
 * 
 * 2- 아이템명에 해당하는 수량을 알려주는 메서드 HowManyCount를 작성
 * (크래프팅에서 같은 종류의 아이템이 수량이 얼마인지 정확하게 알필요가 있음)
 * 
 * <v7.2 - 2024_0127_최원준>
 * 1- ReduceItem의 ItemInfo를 직접 받는 오버로딩 메서드 구현
 * 
 * 2- RemoveItem ItemInfo를 받는 메서드에서 CalculateItemObjCount를 호출하지 않고 있던 점 수정
 * 
 * 
 * <v7.3 - 2024_0128_최원준>
 * 1- AddItemMisc메서드에서 GetCurRemainSlotCount로 조건 검사하고 있던 부분을 삭제하고 IsAbleToAddMisc으로 조건검사하도록 변경
 * 잡화아이템의 경우 슬롯이 없어도 중첩될 수 있다면 들어갈 수 있어야 하므로
 * 
 * 
 * <v7.4 - 2024_0129_최원준>
 * 1- 인벤토리의 isShareAll(전체 슬롯 공유) 옵션에 따라서 인덱스를 할당하도록 수정 
 * SetItemSlotIndexDirect메서드 - 공유옵션일 때 (나머지 한쪽을 아무 인덱스로 잡지 않고,) 전체 슬롯인덱스와 개별슬롯 인덱스를 들어온 인자로 일치시킴
 * SetItemSlotIndexIndirect메서드 - 공유옵션일 때 (전체탭 인덱스, 개별탭 인덱스 따로 2번 구하지 않고) 전체탭 인덱스 한번만 구하여 개별탭 인덱스와 일치시킴
 * 
 * 2- AddItem의 네번째 인자 옵션으로 IsAbleToOverlap 옵션을 기본적으로 true로 설정하였음.
 * 이는 중첩아이템의 경우 수량 소모로 인해 중첩이 가능한 상태이면, 
 * 세이브할 때 중첩하지 않고 저장해놨다가, 로드할 때 강제로 중첩되어 오브젝트가 사라진것 처럼 보이는 이슈가 있기 때문.
 * 이를 해결하기 위해 DeserializeItemListToDic메서드에서 AddItem시 잡화아이템이라도 중첩시키지 않고 슬롯의 위치에 바로 추가하도록 하였음.
 * 
 * <v7.5 - 2024_0131_최원준>
 * 1- AddItemDirectly메서드를 만들어 ItemInfo를 전달하면 해당 정보대로 사전에 바로 추가할 수 있게 하였음.
 * 이는 로드 시 아이템을 해당 정보대로 그대로 추가해야 하기 때문
 * 
 */

namespace InventoryManagement
{    
    public partial class Inventory
    {
        
        /// <summary>
        /// 해당 아이템의 인덱스 정보를 가장 가까운 최신슬롯으로 지정해주는 메서드입니다. <br/>
        /// Inventory의 AddItem에서 아이템을 추가하기 전 슬롯정보를 입력 받기위해 사용됩니다.<br/>
        /// *** 슬롯에 빈자리가 없거나, 아이템 정보가 없다면 예외를 발생시킵니다. ***
        /// </summary>
        public void SetItemSlotIndexIndirect( ItemInfo itemInfo )
        {        
            // 인자 전달 오류 처리
            if( itemInfo==null )
                throw new Exception("ItemInfo 스크립트가 존재하지 않는 아이템입니다.");
            
            // 아이템 정보와 종류를 저장합니다.
            Item item = itemInfo.Item;
            ItemType itemType = item.Type;

            // 슬롯에 빈공간이 없을 때
            if( !IsRemainSlotIndirect(itemType) )     
                throw new Exception("아이템이 들어갈 자리가 충분하지 않습니다. 확인하여 주세요.");


            // 전체 슬롯 공유 옵션이 활성화되어 있다면,
            if( isShareAll )
            {
                // 퀘스트 아이템의 경우 개별 슬롯 인덱스만 할당합니다.
                if( itemType==ItemType.Quest )
                    item.SlotIndexEach=GetItemSlotIndexIndirect( itemType, false );
                // 퀘스트 아이템이 아닌경우 전체 슬롯 인덱스를 할당한 다음, 개별 슬롯인덱스와 일치시킵니다.
                else
                {
                    item.SlotIndexAll=GetItemSlotIndexIndirect( itemType, true );
                    item.SlotIndexEach=item.SlotIndexAll;
                }
            }
            // 전체 슬롯 공유 옵션이 비활성화되어 있다면,
            else
            {
                // 개별 슬롯 인덱스 정보를 입력합니다.
                item.SlotIndexEach=GetItemSlotIndexIndirect( itemType, false );

                // (퀘스트탭이 아닌 경우) 전체 슬롯 인덱스 정보를 입력합니다.
                if( itemType!=ItemType.Quest )
                    item.SlotIndexAll=GetItemSlotIndexIndirect( itemType, true );
            }
        }
        

        /// <summary>
        /// 해당 아이템의 인덱스 정보를 지정 슬롯 인덱스로 설정해주는 메서드입니다.<br/>
        /// 인자로 아이템의 정보와, 어떤 슬롯의 인덱스인지, 전체탭이 활성화되어있는지 여부를 전달받습니다.<br/>
        /// *** 슬롯에 빈자리가 없거나, 아이템 정보가 없다면 예외를 발생시킵니다. ***
        /// </summary>
        public void SetItemSlotIndexDirect(ItemInfo itemInfo, int slotIndex, bool isActiveTabAll)
        {
            // 인자 전달 오류 처리
            if( itemInfo==null )
                throw new Exception("ItemInfo 스크립트가 존재하지 않는 아이템입니다.");
                        
            // 아이템 정보와 종류를 저장합니다.
            Item item = itemInfo.Item;
            ItemType itemType = item.Type;

            // 특정 슬롯에 빈공간이 없다면 예외를 발생시킵니다
            if( !IsRemainSlotDirect(itemType, slotIndex, isActiveTabAll) )
                throw new Exception("슬롯에 자리가 충분하지 않습니다.");

            // 전체 탭 공유 옵션이 활성화 되어 있는 경우
            if( isShareAll )
            {                
                item.SlotIndexAll = slotIndex;      // 전체 슬롯 인덱스 및 개별 슬롯 인덱스를 인자로 일치시킵니다.
                item.SlotIndexEach = slotIndex;
            }
            // 전체 탭 공유 옵션이 비활성화 되어있는 경우
            else
            {
                // 전체 탭이 활성화되어 있는 경우
                if( isActiveTabAll )
                {
                    item.SlotIndexAll=slotIndex;                                  // 전체 슬롯 인덱스를 직접 지정
                    item.SlotIndexEach=GetItemSlotIndexIndirect( itemType, false ); // 개별 슬롯 인덱스를 가까운 아무곳 지정
                }
                // 개별 탭이 활성화되어 있는 경우
                else
                {
                    item.SlotIndexAll=GetItemSlotIndexIndirect( itemType, true );   // 전체 슬롯 인덱스는 가까운 아무곳 지정
                    item.SlotIndexEach=slotIndex;                                 // 개별 슬롯 인덱스를 직접 지정
                }
            }
        }







        /// <summary>
        /// ItemInfo 컴포넌트를 인자로 전달받아 해당 아이템 오브젝트에 해당하는 사전을 찾아서 
        /// 아이템을 넣어주는 메서드입니다.<br/>
        /// 선택 인자로 특정 슬롯 인덱스와 현재탭상태를 전달하면,
        /// 최신 슬롯인덱스가 아니라 지정 슬롯 인덱스에 아이템을 추가합니다.<br/><br/>
        /// <br/>
        /// *** 인자로 들어온 컴포넌트 참조값이 없다면 예외를 발생시킵니다. ***<br/>
        /// </summary>
        /// <returns>아이템 추가 성공시 true를, 현재 인벤토리에 들어갈 공간이 부족하다면 false를 반환합니다</returns>
        public bool AddItem(ItemInfo itemInfo, int slotIndex=-1, bool isActiveTabAll=false, bool isAbleToOverlap=true)
        {   
            if(itemInfo==null)
                throw new Exception("아이템 스크립트가 존재하지 않는 오브젝트입니다. 확인하여 주세요.");

            // 아이템 타입을 확인합니다.
            ItemType itemType = itemInfo.Item.Type;

            // 잡화 아이템과 비잡화 아이템으로 구분하여 메서드를 호출합니다.
            switch(itemType)
            {
                // 타입이 불분명한 경우 예외처리
                case ItemType.None:
                    throw new Exception("해당 아이템의 종류가 명확하지 않습니다. 확인하여 주세요.");

                case ItemType.Misc:
                    if(isAbleToOverlap)
                        return AddItemMisc(itemInfo, false ,slotIndex, isActiveTabAll);   // 별도의 추가 메서드를 호출합니다.
                    else
                        return AddItemToDic(itemInfo, slotIndex, isActiveTabAll);

                default:
                    return AddItemToDic(itemInfo, slotIndex, isActiveTabAll);           // 바로 딕셔너리에 추가합니다.
            }            
        }
                
  
        /// <summary>
        /// 추가 할 ItemInfo 컴포넌트를 인자로 전달받아 해당 사전에 아이템 오브젝트를 넣어주는 메서드입니다.<br/>
        /// 해당 종류의 아이템 1개가 들어갈 빈 슬롯이 있어야 합니다.<br/><br/>
        /// 선택 인자로 특정 슬롯 인덱스와 현재탭상태를 전달하면,
        /// 최신 슬롯인덱스가 아니라 지정 슬롯 인덱스에 아이템을 추가합니다.<br/><br/>
        /// </summary>
        /// <returns>해당 아이템 종류의 아이템이 들어갈 빈 슬롯이 없다면 false, 아이템 추가에 성공 시 true를 반환합니다.</returns>
        private bool AddItemToDic(ItemInfo itemInfo, int slotIndex=-1, bool isActiveTabAll=false)
        {          
            // 아이템 이름과 종류를 참조합니다.
            string itemName = itemInfo.Item.Name;
            ItemType itemType = itemInfo.Item.Type;
                        
            // 슬롯 인덱스 값이 음수로 전달되었다면,(인자를 따로 주지 않았다면)
            if( slotIndex<0 )
            {
                // 근처의 남은 슬롯이 없다면 실패를 반환합니다.
                if( !IsRemainSlotIndirect(itemType) )            
                    return false;
                
                // 아이템 정보를 가장 가까운 슬롯의 인덱스로 설정합니다.
                SetItemSlotIndexIndirect( itemInfo );
            }         
            // 슬롯 인덱스의 값이 양수로 전달되었다면
            else
            {             
                // 지정 슬롯에 빈자리가 없다면 실패를 반환합니다.
                if( !IsRemainSlotDirect( itemType, slotIndex, isActiveTabAll ) )
                    return false;

                // 아이템 정보를 지정 슬롯 인덱스로 설정합니다.
                SetItemSlotIndexDirect(itemInfo, slotIndex, isActiveTabAll);
            }
                        
            // 조건검사에 통과하였으므로 아이템을 사전에 바로 추가합니다.
            AddItemDirectly(itemInfo);         

            // 성공을 반환합니다.
            return true;                                    
        }


        /// <summary>
        /// 아이템을 사전에 어떠한 조건 검사도 하지 않고 바로 추가합니다.
        /// </summary>
        public void AddItemDirectly(ItemInfo itemInfo)
        {            
            // 아이템 이름과 종류를 참조합니다.
            string itemName = itemInfo.Item.Name;
            ItemType itemType = itemInfo.Item.Type;

            // 아이템 이름을 기반으로 해당 사전의 ItemInfo 리스트를 결정합니다. (존재하지 않는다면 새롭게 생성하여 반환합니다.)
            List<ItemInfo> itemInfoList = GetItemInfoList(itemName, true);

            // 해당 아이템 정보를 추가합니다.
            itemInfoList.Add(itemInfo);          
                  
            // 해당 아이템 종류의 현재 오브젝트의 갯수를 증가시킵니다.
            CalculateItemObjCount(itemType, 1);
        }





        

        /// <summary>
        /// 잡화 아이템 오브젝트를 기존의 인벤토리에 추가하여 줍니다.<br/>
        /// 기존의 동일한 잡화아이템이 최대 수량이 채워지지 않았다면, 들어온 아이템의 수량을 감소시켜 기존 아이템의 수량을 채웁니다.
        /// <br/><br/>
        /// 
        /// 들어온 아이템의 수량이 0이 되었다면 오브젝트를 파괴하고,<br/> 
        /// 0이되지 않았다면 해당 아이템을 새롭게 인벤토리 슬롯에 추가합니다.<br/><br/>
        /// 기본적으로 슬롯에서 앞선 순서로 채워지며 이를 변경할 수 있습니다. (기본값: 오래된순)<br/><br/>
        /// 
        /// 선택 인자로 특정 슬롯 인덱스와 현재탭상태를 전달하면,
        /// 최신 슬롯인덱스가 아니라 지정 슬롯 인덱스에 아이템을 추가합니다.<br/><br/>
        /// </summary>
        /// <returns>잡화 아이템이 들어갈 공간이 없다면 false를, 아이템 추가에 성공한 경우 true를 반환합니다.</returns>
        private bool AddItemMisc(ItemInfo itemInfo, bool isLatestModify=false, int slotIndex=-1, bool isActiveTabAll=false)
        {
            // 아이템이 들어갈 공간이 없는 경우 실패를 반환합니다.
            if( !IsAbleToAddMisc(itemInfo.Name, itemInfo.OverlapCount) )
                return false;

            // 아이템 정보를 전달하여 기존아이템에 채우기를 실행하고, 남은 수량을 반환받습니다.
            int afterCount = FillExistItemOverlapCount(itemInfo, isLatestModify);

            // 수량채우기가 종료된 후 
            if( afterCount==0 )
                GameObject.Destroy( itemInfo.gameObject );      // 남은 수량이 0이 되었다면, 인자로 전달받은 오브젝트 삭제
            else
                AddItemToDic(itemInfo, slotIndex, isActiveTabAll);  // 남은 수량이 존재한다면, 해당 아이템을 사전에 추가

            return true;
        }

        







        /// <summary>
        /// 잡화 아이템을 인벤토리에 추가하기 전에,<br/>
        /// 동일한 이름의 기존 잡화 아이템에 수량을 채워주고, 남은 수량을 반환합니다.<br/>
        /// 기존 잡화아이템이 없다면 채우지 못하고 남은 수량을 그대로 반환합니다.<br/><br/>
        /// 인자로 전달한 ItemInfo의 수량은 채운만큼 감소되어 있으며,<br/>
        /// 수량이 0이 될때 까지 채우지만 삭제는 별도로 하지 않습니다.<br/><br/>
        /// 슬롯 순서 상 앞선 아이템부터 채우며, 변경할 수 있습니다.(기본값: 오래된순)<br/><br/>
        /// *** 전달한 아이템이 잡화아이템이 아닌 경우 예외를 발생시킵니다. ***<br/>
        /// </summary>
        /// <returns>기존 아이템에 모든 수량을 다 채운 경우 0을 반환, 다 채우지 못한 경우 남은 수량을 반환</returns>
        public int FillExistItemOverlapCount(ItemInfo itemInfo, bool isLatestModify=false)
        {            
            if(itemInfo.Item.Type != ItemType.Misc)
                throw new Exception("전달 받은 아이템이 잡화 아이템이 아닙니다. 확인하여 주세요.");

            ItemMisc newItemMisc = (ItemMisc)itemInfo.Item;

            // 오브젝트 리스트 참조를 설정합니다.
            List<ItemInfo> itemInfoList = GetItemInfoList( newItemMisc.Name );                       

            // 기존 아이템에 수량채우기 실행 전 후를 비교할 수량을 현재 아이템의 수량으로 설정합니다.
            int beforeCount = newItemMisc.OverlapCount; 
            int afterCount = newItemMisc.OverlapCount;

            // 기존 오브젝트 리스트가 있는 경우는
            if( itemInfoList!=null )
            { 
                // 오름차순 정렬을 시행합니다.
                itemInfoList.Sort(CompareBySlotIndexEach);

                // 슬롯 순서로 정렬 된 아이템을 접근 기준에 따라 하나씩 꺼내어 기존 아이템에 수량채우기를 실행합니다.
                if( isLatestModify )
                {
                    for( int i = itemInfoList.Count-1; i>=0; i-- )
                        if( FillCountInLoopByOrder(i) ) { break; } // 내부적으로 true를 반환하면 빠져나갑니다.                                     
                }
                else
                {
                    for( int i = 0; i<=itemInfoList.Count-1; i++ )   
                        if( FillCountInLoopByOrder(i) ) { break; } // 내부적으로 true를 반환하면 빠져나갑니다.  
                }
            }

            // 채워주고 남은 수량을 반환합니다.
            return afterCount;

            



            // 기존 아이템에 접근 순서를 다르게 하여 채우기 위한 반복문용 내부 메서드입니다.
            bool FillCountInLoopByOrder(int idx)
            {
                // 기존 아이템에 인덱스를 통한 접근을 하여 정보를 불러옵니다.
                ItemMisc oldItemMisc = (ItemMisc)itemInfoList[idx].GetComponent<ItemInfo>().Item;
                    
                // 기존 아이템에 현재 남은수량을 최대 허용치까지 채우고 남은 수량을 반환받습니다.
                afterCount = oldItemMisc.AccumulateOverlapCount(beforeCount);

                // 줄어든 수량만큼 신규아이템의 수량을 감소시켜 줍니다.
                newItemMisc.AccumulateOverlapCount(beforeCount-afterCount);
                    
                // 다음 반복문에 사용하기 위하여 현재수량과 일치시켜 줍니다. 
                beforeCount = afterCount;


                // 남은 수량이 0이되면 외부 호출 금지를 활성화합니다.
                if(afterCount==0)
                    return true;
                else
                    return false;
            }
        }





        
        
        /// <summary>
        /// 특정 아이템의 ItemInfo 컴포넌트를 인자로 받아서 인벤토리의 딕셔너리 목록에서 제거해주는 메서드 입니다.<br/>
        /// 검색 방식이 아니라 직접 참조하여 목록에서 제거하므로, 특정 아이템을 직접 제거하는 용도로 사용합니다.<br/>
        /// *** 참조 값이 존재하지 않으면 예외를 반환합니다. ***
        /// </summary>
        /// <returns>딕셔너리 목록의 제거에 성공한 경우 해당 아이템의 ItemInfo 참조값을, 목록에 없는 아이템인 경우 null을 반환합니다.</returns>
        public ItemInfo RemoveItem(ItemInfo itemInfo)
        {
            // 전달인자의 예외처리
            if(itemInfo==null)
                throw new Exception("아이템 스크립트가 존재하지 않는 오브젝트입니다. 확인하여 주세요.");
            
            // 이름을 기반으로 해당 딕셔너리 참조를 받습니다.
            List<ItemInfo> itemInfoList = GetItemInfoList(itemInfo.Item.Name);            

            // 인벤토리 목록에서 없는 아이템 예외처리
            if( itemInfoList==null || itemInfoList.Count==0 )
                throw new Exception("해당 아이템이 인벤토리 내부에 존재하지 않습니다. 확인하여 주세요.");

            // ItemInfo리스트에 직접 ItemInfo를 전달하여 목록에서 제거합니다.
            itemInfoList.Remove(itemInfo);

            // 해당 아이템 종류의 현재 오브젝트의 갯수를 감소시킵니다.
            CalculateItemObjCount(itemInfo.Type, -1);    

            // 목록에서 제거한 참조값을 다시 반환합니다. 
            return itemInfo;
        }


        
        /// <summary>
        /// 아이템의 이름을 인자로 받아서 해당 아이템을 검색하여 인벤토리의 딕셔너리 목록에서 제거해주는 메서드 입니다.<br/>
        /// 인자를 통해 최신순으로 제거 할 것인지, 오래된 순으로 제거할 것인지를 결정할 수 있습니다. 기본은 최신순입니다.<br/>
        /// </summary>
        /// <returns>딕셔너리 목록의 제거에 성공한 경우 해당 아이템의 ItemInfo 참조값을, 목록에 없는 아이템인 경우 null을 반환합니다.</returns>
        public ItemInfo RemoveItem(string itemName, bool isLatest=true)
        {            
            // 딕셔너리에서 ItemInfo 리스트를 받습니다
            List<ItemInfo> itemInfoList = GetItemInfoList(itemName); 

            // 오브젝트 리스트가 존재하지 않거나, 오브젝트 리스트의 갯수가 0이라면 삭제를 못하므로 예외처리
            if(itemInfoList==null || itemInfoList.Count==0)
                throw new Exception("해당 아이템이 인벤토리 내부에 존재하지 않습니다. 확인하여 주세요.");


            // 해당 ItemInfo 리스트의 첫번째 항목을 참조하여 아이템의 타입을 얻습니다
            ItemType itemType = itemInfoList[0].Item.Type;            
            
            ItemInfo targetItemInfo = null;  // 반환할 아이템과 참조할 인덱스를 설정합니다.
            int targetIdx;

            if( isLatest )
                targetIdx = itemInfoList.Count-1;  // 최신 순
            else
                targetIdx = 0;                    //오래된 순

            targetItemInfo =itemInfoList[targetIdx];    // 반환 할 아이템 참조값을 저장합니다.
            itemInfoList.RemoveAt(targetIdx);           // 해당 인덱스의 아이템을 리스트에서 제거합니다            
        
            CalculateItemObjCount(itemType, -1);        // 해당 아이템 종류의 현재 오브젝트의 갯수를 감소시킵니다.      
                        
            return targetItemInfo;                      // 목록에서 제거한 아이템을 반환합니다.     
        }




        /// <summary>
        /// 인자로 전달 한 이름에 해당 하는 아이템의 수량이 얼마인지를 알려줍니다.
        /// </summary>
        /// <returns>잡화 아이템의 경우에는 중첩 수량을, 비잡화 아이템의 경우에는 오브젝트 갯수를 반환</returns>
        public int HowManyCount(string itemName)
        {
            ItemType itemType = GetItemTypeIgnoreExists(itemName);

            List<ItemInfo> itemInfoList= GetItemInfoList(itemName);

                        
            int totalCount = 0;

            // 잡화 아이템의 경우 중첩수량을 누적시킵니다.
            if(itemType == ItemType.Misc)
            {
                foreach(ItemInfo itemInfo in itemInfoList)
                    totalCount += itemInfo.OverlapCount;
            }
            // 비잡화 아이템의 경우 오브젝트 숫자를 누적시킵니다. 
            else
                totalCount = itemInfoList.Count;
            
            return totalCount;
        }




        /// <summary>
        /// 아이템 참조값을 직접 전달하여 해당 아이템의 전달 수량만큼 감소시킵니다.<br/><br/>
        /// *** 아이템 정보 전달이 되지 않았거나, 수량 인자 전달이 잘못된 경우 예외가 발생 ***<br/>
        /// *** 잡화아이템이 아닌 경우 예외가 발생 ***
        /// </summary>
        /// <returns>수량이 충분하지 않으면 false를, 수량이 충분하여 감소에 성공하면 true를 반환</returns>
        public bool ReduceItem(ItemInfo itemInfo, int overlapCount)
        {
            if(itemInfo==null)
                throw new Exception("아이템 정보가 전달되지 않았습니다.");
            
            if(overlapCount<=0)
                throw new Exception("수량인자 전달이 잘못되었습니다.");


            ItemMisc itemMisc = itemInfo.Item as ItemMisc;
            
            if(itemMisc == null)
                throw new Exception( "비잡화 아이템의 경우 수량을 줄일 수 없습니다. RemoveItem메서드를 실행해주세요.");

            // 아이템이 보유한 수량이 들어온 수량보다 적다면 실패를 반환합니다.
            if( itemMisc.OverlapCount < overlapCount )
                return false;
            else
                itemMisc.AccumulateOverlapCount(-overlapCount);

            // 수량이 0이되면 파괴하고
            if( itemMisc.OverlapCount==0 )
            {
                RemoveItem(itemInfo);                       // 인벤토리 목록에서 제거합니다.
                GameObject.Destroy( itemInfo.gameObject );  // 최상위 오브젝트(2D오브젝트)를 제거합니다.
                itemInfo.StatusWindowInteractive.OnItemPointerExit();   // 상태창을 종료시켜줍니다.
            }
            // 아니라면 수량정보를 업데이트 합니다.
            else
                itemInfo.UpdateTextInfo();

            return true;
        }

        
        










        
        


    }
}
