using UnityEngine;
using InventoryManagement;
using DataManagement;
using ItemData;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 * [작업 사항]  
 * <v1.0 - 2024_0103_최원준>
 * 1- AddItem, RemoveItem메서드 오버로딩 대부분을 삭제하고 필요한 메서드만 구현
 * 
 * 
 * (이슈)
 * 1- 잡화 아이템의 경우 AddItem할 때 기존 아이템이 있는 경우 기존 아이템에 들어갈 수량이 충분하다면, 
 * 인자로 들어온 오브젝트를 파괴시켜야 한다.
 * 
 * 2- Split메서드 구현
 * 인벤토리 내부에서 특정키(ctrl등) 클릭 시 Split 기능 
 * 혹은 월드에 드랍할 때 잡화아이템의 경우 드랍할 수량을 결정하여 Split할 수 있게 해야한다.
 * 수량만 다른 새로운 오브젝트를 생성시키고 수량 정보를 조절해야 한다. 내부에서 split시 목록에 Add까지 해야 하고,
 * 외부드랍을 통한 Split시에는 인벤토리 목록에서 제거하면 안된다.
 * 
 * 
 * <v1.1 - 2024_0105_최원준>
 * 1- AddItem메서드에서 먼저 인벤토리에 추가하고 인덱스를 입력하는 것에서
 * 인덱스를 먼저 입력하고 인벤토리에 추가하는 구조로 변경하였음.
 * 이유는 미리 추가한 아이템의 인덱스 정보를 인덱스를 구하는 과정에서 읽어들이기 때문.
 * 
 * <v1.2 -2024_0108_최원준>
 * 1- ItemInfo를 직접 전달하여 인벤토리 목록에서 제거해주는 RemoveItem 오버로딩 메서드 추가
 * ItemSelect 스크립트에서 인벤토리에서 외부로 아이템을 드랍하였을 때 해당 아이템을 직접 제거해야하기 때문
 * 
 * <v2.0 - 2024_0109_최원준>
 * 1- AddItem(ItemInfo) 메서드에서 잡화아이템이 들어온 경우와 일반아이템인 경우를 구분하여 로직 작성
 * AddItem을 통해 내부에서 게임오브젝트가 파괴되더라고 ItemInfo는 바로 파괴되지 않고 참조값이 존재하기 때문에
 * 전 후 수량을 파악하여 필요한 처리를 하도록하였음.
 * 
 * <v2.1 - 2024_0111_최원준>
 * 1- RemoveItem메서드의 변수명 targetItemInfo->rItemInfo로 변경 
 * 2- 이름을 인자로 받는 RemoveItem메서드 내 rItemInfo.UpdateInventoryInfo(null); 추가
 * 
 * <v3.0 -2024_0114_최원준>
 * 1- UpdateAllOverlapCountInExist메서드 주석 추가
 * 2- 상속스크립트 QuickSlot을 정의하고 관련속성을 상속받기위해 private 변수와 메서드를 protected처리
 * 
 * <v3.1 - 2024_0115_최원준>
 * 1- AddItem메서드 내부의 FindSlotIdxToNearstSlot을 내부적으로 호출 하던 것을 삭제
 * Inventory클래스에서 밖에 Index를 구할 수 없으므로, Inventory클래스의 AddItem에서 내부적으로 통합하였기 때문
 * 
 * <V3.2 - 2024_0116_최원준>
 * 1- 기존 AddItem 메서드 내부의 잡화 아이템, 비잡화아이템을 나눠서 추가하던 로직을 메서드화 하여
 * AddItemBasic과 AddItemMisc 메서드로 나누었으며, AddItem 내부에 포함 (protected처리)
 * 
 * 2- AddItem의 ItemPairs[]를 인자로 받는 오버로딩 메서드 작성
 * 
 * 3- AddItemToSlot메서드 작성
 * (특정 슬롯에 넣기 위해 슬롯 인덱스와 탭상태를 받는 메서드)
 * 
 * <v3.3 - 2024_0116_최원준>
 * 1- AddItemToSlot메서드에서 slotIndex를 예외처리하던 부분삭제
 * (IsSlotEnough에서 진행하므로 필요없음)
 * 
 * <v3.4 - 2024_0122_최원준>
 * 1- 메서드명 UpdateAllOVerlapCountInExist를 UpdateTextInfoSameItemName으로 변경
 * 
 * <v3.5 - 2024_0123_최원준>
 * 1- UpdateTextInfoSameItemName에서 GetItemObjectList메서드를 사용하는 구문 조건검사 추가
 * 
 * <v3.6 - 2024_0124_최원준>
 * 1- Inventory클래스의 딕셔너리 저장형식을 GameObject기반에서 ItemInfo로 변경하면서 관련 메서드 수정
 * (UpdateTextInfoSameItemName)
 * 
 * <v4.0 - 2024_0126_최원준>
 * 1- 인벤토리 검색 및 연산관련 메서드 (SetItemOverlapCount, IsItemEnough, IsSlotEnough, IsSlotEmpty)를 InventoryInfo.cs에서 옮겨옴
 * 
 * 2- 아이템명에 해당하는 수량을 알려주는 메서드 HowManyCount를 작성
 *  (크래프팅에서 같은 종류의 아이템이 수량이 얼마인지 정확하게 알필요가 있음)
 * 
 * <v4.1 - 2024_0127_최원준>
 * 1- ReduceItem의 ItemInfo를 직접 받는 오버로딩 메서드 구현
 * 
 * <v4.2 - 2024_0220_최원준>
 * 1- AddItemMisc, AddItemToSlot메서드에 isAbleToOverlap옵션을 추가하여 잡화아이템이더라도 겹치지 않고 오브젝트를 그대로 추가할 수 있도록 하였음
 * 
 * 2- AddItemMisc의 반환형을 void에서 bool로 설정하여 인자로 넣은 아이템의 파괴상태 여부를 반환하도록 설정
 * 
 * 3- IsSlotEmpty, AddItemToSlot, AddItemMisc, AddItemBasic메서드의 기존에 전체탭 상태변수를 인자로 받던 것을 삭제하고, 활성 슬롯 인덱스만 전달받도록 설정
 * 이유는 전체탭인덱스는 인벤토리에서 자체적으로 인터렉티브 스크립트 참조를 통해 판단할 수 있기 때문
 * 
 * 4- AddItemToSlot메서드의 활성 슬롯인덱스와 비활성 슬롯인덱스 모두를 인자로 전달받는 오버로딩 메서드 추가
 * 기존 메서드는 활성슬롯인덱스만 받아서 나머지 비활성슬롯 인덱스는 가까운 인덱스를 간접할당받았지만,
 * 특정 슬롯에 추가할 때 (스위칭하는 아이템이 자리했던 인덱스 모두를 이어받아야 하는 경우) 나머지 비활성슬롯 인덱스 또한 지정해야 하는 경우가 생기기 때문
 * 
 * 5- IsSlotEmpty의 활성화 슬롯의 Transform을 인자로 전달받는 오버로딩 메서드 추가
 * 아이템 드롭이 이뤄질 때 해당 활성 슬롯의 Transform을 바로 전달하여 추가할 수 있을 지 판단하기 위함
 * 
 * <v4.3 - 2024_0221_최원준>
 * 1- RemoveItem의 isDestroy옵션을 삭제하고 isUnregister로 대체
 * 아이템을 제거와 동시에 파괴하는 경우가 잘 쓰이지 않고, 연달아서 호출하여 대체가능하기 때문이며,
 * isUnregister옵션은 2D상태 그대로 목록에서 제거하여 타인벤토리 이동시 DimensionShift를 발생시키지 않기 위함.
 * (아이템셀렉팅 스크립트에서 아이템의 캔버스그룹을 조절하는데 간섭이 일어나기 때문)
 * 
 */

public partial class InventoryInfo : MonoBehaviour
{
    /// <summary>
    /// Inventory클래스에서 필요 시 아이템 정보를 전송받기 위해 사용하는 ItemInfo를 임시적으로 담는 리스트입니다.<br/>
    /// AddItem메서드에서 
    /// </summary>
    protected List<ItemInfo> tempInfoList = new List<ItemInfo>();




    

    /// <summary>
    /// 인벤토리에 존재하는 해당 이름의 *기존* 잡화 아이템 수량을 증가시키거나 감소시킵니다.<br/>
    /// 인자로 해당 아이템 이름과 수량을 지정해야 합니다.<br/><br/>
    /// 아이템 수량을 감소시키려면 수량 인자로 음수를 전달하여야 하며,<br/>
    /// 기존 수량이 감소로 인해 0이되면 아이템이 인벤토리 목록에서 제거되며, 파괴됩니다.<br/><br/>
    /// 아이템 수량을 증가시키려면 수량 인자로 양수를 전달하여야 하며,<br/>
    /// 아이템 최대 수량 제한으로 인해 더 이상 수량을 증가시키기 못하는 경우는 나머지 초과 수량을 반환합니다.<br/><br/>
    /// 최신 순, 오래된 순으로 감소여부를 결정할 수있습니다. (기본값: 최신순)<br/><br/>
    /// ** 아이템 이름이 해당 인벤토리에 존재하지 않거나, 잡화아이템이 아닌 경우 예외를 발생시킵니다. **
    /// </summary>
    /// <returns></returns>
    public int SetItemOverlapCount( string itemName, int inCount, bool isLatestModify = true )
    {
        return inventory.SetOverlapCount(itemName, inCount, isLatestModify, null);
    }


    
    /// <summary>
    /// 아이템의 종류와 상관없이 아이템이 해당 수량 만큼 인벤토리에 존재하는지 여부를 반환합니다.<br/>
    /// 아이템 이름과 수량으로 이루어진 구조체 배열을 인자로 받습니다.<br/><br/>
    /// 일반 아이템은 오브젝트의 갯수를 의미하며, 잡화 아이템은 중첩수량을 의미합니다.<br/>   
    /// 해당 수량만큼 감소 및 파괴옵션을 지정할 수 있습니다. (기본값: 수량 1, 수량 감소 및 파괴 안함, 최신순 감소 및 파괴)<br/><br/>
    /// *** 수량 인자가 0이하라면 예외를 발생시킵니다. ***
    /// </summary>
    /// <returns>아이템이 존재하며 수량이 충분한 경우 true를, 존재하지 않거나 수량이 충분하지 않다면 false를 반환</returns>
    public bool IsItemEnough( ItemPair[] pairs, bool isReduceAndDestroy = false, bool isLatestModify = true )
    {
        return inventory.IsEnough(pairs, isReduceAndDestroy, isLatestModify);
    }



    
    /// <summary>
    /// 아이템의 종류와 상관없이 아이템이 해당 수량 만큼 인벤토리에 존재하는지 여부를 반환합니다.<br/>
    /// 아이템 이름과 수량을 인자로 받습니다.<br/><br/>
    /// 일반 아이템은 오브젝트의 갯수를 의미하며, 잡화 아이템은 중첩수량을 의미합니다.<br/>   
    /// 해당 수량만큼 감소 및 파괴옵션을 지정할 수 있습니다. (기본값: 수량 1, 수량 감소 및 파괴 안함, 최신순 감소 및 파괴)<br/><br/>
    /// *** 수량 인자가 0이하라면 예외를 발생시킵니다. ***
    /// </summary>
    /// <returns>아이템이 존재하며 수량이 충분한 경우 true를, 존재하지 않거나 수량이 충분하지 않다면 false를 반환</returns>
    public bool IsItemEnough( string itemName, int count=1, bool isReduceAndDestroy = false, bool isLatestModify=true )
    {
        return inventory.IsEnough(itemName, count, isReduceAndDestroy, isLatestModify);      
    }






    












    /// <summary>
    /// 이 인벤토리의 슬롯에 아이템을 원하는 수량만큼 생성했을 때 아무 슬롯에 들어갈 빈 자리가 있는지 여부를 반환합니다.<br/>
    /// 인자로 아이템 이름과 수량인자를 전달해야 합니다. (수량인자의 기본값은 1입니다.)<br/><br/>
    /// 
    /// 비잡화아이템의 경우 수량인자만큼 오브젝트를 갖습니다. (즉, 수량인자가 오브젝트의 개수를 말합니다.)<br/>
    /// 잡화 아이템의 경우 수량인자를 넣어도 최대 중첩수량에 도달하기 전까지는 오브젝트를 1개만 형성합니다. (즉, 수량인자는 중첩수량을 말합니다.)<br/><br/>
    /// </summary>
    /// <returns>아이템을 생성가능하다면 true를, 불가능하다면 false를 반환합니다.</returns>
    public bool IsSlotEnough(string itemName, int overlapCount=1)
    {
        ItemType itemType = createManager.GetWorldItemType(itemName);

        if( itemType==ItemType.Misc )
            return inventory.IsAbleToAddMisc( itemName, overlapCount ); 
        else
            return inventory.GetCurRemainSlotCount(itemType) >= overlapCount; // 남은 슬롯 칸 수가 overlapCount이상
    }

    /// <summary>
    /// 해당 아이템이 아무 슬롯에 들어갈 빈 자리가 있는지 여부를 반환합니다.<br/>
    /// 기본 아이템의 경우 슬롯여부에 따라 성공, 실패를 반환하며,<br/>
    /// 잡화 아이템의 경우 중첩되어서 슬롯이 필요없는 경우 슬롯에 빈자리가 없어도 성공을 반환할 수 있습니다.<br/>
    /// 기존 아이템 정보가 존재해야 합니다.
    /// </summary>
    /// <returns>아이템을 생성가능하다면 true를, 불가능하다면 false를 반환합니다.</returns>
    public bool IsSlotEnough(ItemInfo itemInfo)
    {
        ItemType itemType = itemInfo.Item.Type;

        if( itemType == ItemType.Misc )
        {
            ItemMisc itemMisc = (ItemMisc)itemInfo.Item;
            return inventory.IsAbleToAddMisc(itemMisc.Name, itemMisc.OverlapCount);
        }
        else
            return inventory.IsRemainSlotIndirect(itemType);  // 아무자리에 남은 슬롯 칸수가 있는지
    }




    /// <summary>
    /// 해당 아이템이 현재 탭의 특정 슬롯에 들어갈 수 있을 지 여부를 반환합니다.<br/>
    /// 아이템 정보와 활성화 슬롯의 인덱스를 전달해야 합니다.
    /// </summary>
    /// <returns>슬롯에 빈자리가 없는 경우 false를, 빈자리가 있는 경우 true를 반환</returns>
    public bool IsSlotEmpty(ItemInfo itemInfo, int activeSlotIndex)
    {
        return inventory.IsRemainSlotDirect(itemInfo.Item.Type, activeSlotIndex, interactive.IsActiveTabAll);         
    }

    /// <summary>
    /// 해당 아이템이 현재 탭의 특정 슬롯에 들어갈 수 있을 지 여부를 반환합니다.<br/>
    /// 아이템 정보와 활성화 슬롯의 Transform 참조값을 전달해야 합니다.
    /// </summary>
    /// <returns>슬롯에 빈자리가 없는 경우 false를, 빈자리가 있는 경우 true를 반환</returns>
    public bool IsSlotEmpty(ItemInfo itemInfo, Transform activeSlotTr)
    {
        return inventory.IsRemainSlotDirect(itemInfo.Item.Type, activeSlotTr.GetSiblingIndex(), interactive.IsActiveTabAll);
    }
















    
    /// <summary>
    /// 아이템의 이름과 수량을 인자로 받아 해당 아이템을 수량만큼 감소시켜줍니다.<br/>
    /// 아이템의 종류와 상관없이 해당 수량 만큼 인벤토리에서 감소시킨 후 성공 여부를 반환합니다.<br/>
    /// 잡화아이템의 경우 중첩수량을 감소시키며, 일반 아이템의 경우 오브젝트 갯수를 감소시킵니다.<br/><br/>
    /// *** 수량 인자가 0이하라면 예외를 발생시킵니다. ***
    /// </summary>
    /// <returns>아이템 수량 감소에 성공한 경우 true를, 존재하지 않거나 수량이 충분하지 않다면 false를 반환</returns>
    public bool ReduceItem(string itemName, int count)
    {
        return IsItemEnough(itemName, count, true);
    }
    
    /// <summary>
    /// 아이템의 이름과 수량을 가지는 구조체 배열을 인자로 받아 해당 아이템을 수량만큼 감소시켜줍니다.<br/>
    /// 아이템의 종류와 상관없이 해당 수량 만큼 인벤토리에서 감소시킨 후 성공 여부를 반환합니다.<br/>
    /// 잡화아이템의 경우 중첩수량을 감소시키며, 일반 아이템의 경우 오브젝트 갯수를 감소시킵니다.<br/><br/>
    /// *** 수량 인자가 0이하라면 예외를 발생시킵니다. ***
    /// </summary>
    /// <returns>아이템 수량 감소에 성공한 경우 true를, 존재하지 않거나 수량이 충분하지 않다면 false를 반환</returns>
    public bool ReduceItem( ItemPair[] itemPairs )
    {
        return IsItemEnough(itemPairs, true);
    }

    
    /// <summary>
    /// 아이템 참조값을 직접 전달하여 해당 아이템의 전달 수량만큼 감소시킵니다.<br/><br/>
    /// *** 아이템 정보 전달이 되지 않았거나, 수량 인자 전달이 잘못된 경우 예외가 발생 ***<br/>
    /// *** 잡화아이템이 아닌 경우 예외가 발생 ***
    /// </summary>
    /// <returns>수량이 충분하지 않으면 false를, 수량이 충분하여 감소에 성공하면 true를 반환</returns>
    public bool ReduceItem( ItemInfo itemInfo, int overlapCount )
    {
        return inventory.ReduceItem(itemInfo, overlapCount);
    }






    /// <summary>
    /// 해당 이름의 아이템을 인벤토리의 목록에서 제거후에 목록에서 제거한 아이템의 ItemInfo 참조값을 반환합니다.<br/>
    /// 기본적으로 아이템을 3D상태로 만들어 참조값 반환을 하지만,<br/>
    /// 선택 인자로 isUnregister 옵션을 활성화시키면 2D 상태 그대로 반환합니다. (타 인벤토리로의 추가 시 활용합니다) <br/><br/>
    /// *** 인벤토리에 해당 이름의 아이템이 없으면 예외를 발생시킵니다. ***<br/>
    /// </summary>
    public ItemInfo RemoveItem(string itemName, bool isUnregister=false)
    {
        // 인벤토리 목록에서 제거하고 반환 할 참조값을 저장합니다.
        ItemInfo itemInfo = inventory.RemoveItem(itemName);   
        
        if( itemInfo==null )
            throw new Exception( "해당 이름의 아이템이 존재하지 않습니다." );       
        
        // 등록해제 옵션이 걸려있지 않다면,
        if( !isUnregister )       
            itemInfo.DimensionShift(true);  // 아이템을 3D상태로 전환합니다.

        // 인벤토리 정보를 제거합니다.
        itemInfo.UpdateInventoryInfo(null);

        return itemInfo;
    }


    /// <summary>
    /// 해당 아이템을 인벤토리의 목록에서 직접 제거합니다.<br/>
    /// 제거후에 목록에서 제거한 아이템의 ItemInfo 참조값을 반환하여 다른 메서드를 호출할 수 있습니다.<br/><br/>
    /// 기본적으로 아이템을 3D상태로 만들어 참조값 반환을 하지만,<br/>
    /// 선택 인자로 isUnregister 옵션을 활성화시키면 2D 상태 그대로 반환합니다. (타 인벤토리로의 추가 시 활용합니다) <br/><br/>
    /// *** 인자가 전달되지 않거나, 인벤토리에 해당 이름의 아이템이 없으면 예외가 발생합니다. ***<br/>
    /// </summary>
    public ItemInfo RemoveItem(ItemInfo itemInfo, bool isUnregister=false)
    {
        if(itemInfo==null)
            throw new Exception("아이템 정보가 전달되지 않았습니다.");
        
        if( inventory.RemoveItem(itemInfo) == null )
            throw new Exception("해당 인벤토리에 아이템 정보가 존재하지 않습니다.");
                    
        // 등록해제 옵션이 걸려있지 않다면,
        if( !isUnregister )       
            itemInfo.DimensionShift(true);  // 아이템을 3D상태로 전환합니다.

        // 인벤토리 정보를 제거합니다.
        itemInfo.UpdateInventoryInfo(null);  
        
        return itemInfo;
    }







    /// <summary>
    /// 인벤토리의 목록에 없는 아이템을 새롭게 생성하고 가까운 슬롯에 추가합니다.<br/>
    /// 오브젝트 1개만 생성하므로, 여러 아이템을 생성시키고 싶을 때 중복하여 호출해야 합니다.<br/>
    /// 잡화아이템의 경우 중첩수량을 설정할 수 있습니다. 비잡화 아이템의 경우는 무시합니다.(기본값:1)<br/>
    /// </summary>
    /// <returns>인벤토리의 슬롯에 아이템 1개를 생성할 공간이 충분하지 않다면 false를 반환, 성공한 경우 true를 반환</returns>
    public bool AddItem(string itemName, int overlapCount=1)
    {
        if( !IsSlotEnough(itemName, overlapCount) )
            return false;
        
        // createManager에게 생성요청을하여 오브젝트 하나를 만듭니다.
        ItemInfo itemInfo = createManager.CreateWorldItem(itemName, overlapCount);

        // 해당 아이템을 인벤토리에 넣습니다.
        AddItem(itemInfo);

        return true;
    }

    
    /// <summary>
    /// 인벤토리의 목록에 *없는* 아이템을 새롭게 생성하고 가까운 슬롯에 추가합니다.<br/>
    /// 인자로 아이템 이름과 중첩 수량 묶음 구조체를 전달받습니다.<br/><br/>
    /// 오브젝트 단위로 여러 아이템을 생성할 수 있습니다.<br/>
    /// 비잡화아이템의 경우 ItemPair의 overlapcount는 무시됩니다.<br/>
    /// </summary>
    /// <returns>인벤토리의 슬롯에 아이템 여러 개를 생성할 공간이 충분하지 않다면 false를 반환, 성공한 경우 true를 반환</returns>
    public bool AddItem(ItemPair[] itemPairs)
    {
        for( int i = 0; i<itemPairs.Length; i++ )
        {            
            // 해당 이름의 아이템을 해당 수량만큼 생성했을 때 슬롯이 충분하다면 반복문이 끝날때까지 계속 검사합니다
            if( IsSlotEnough( itemPairs[i].itemName, itemPairs[i].overlapCount ) )
                continue;
            
            // 한번이라도 부족하다면 false를 반환합니다.
            return false;
        }

        // 모든 아이템을 추가합니다
        for(int i=0; i<itemPairs.Length; i++)
            AddItem( itemPairs[i].itemName, itemPairs[i].overlapCount );
        
        return true;
    }






    /// <summary>
    /// 인벤토리의 목록에 *월드*에 존재하는 기존의 아이템을 가까운 슬롯에 추가합니다.<br/>
    /// 다른 인벤토리에 존재하는 아이템은 OnItemSlotDrop메서드 호출을 통해 다른 인벤토리로 전이되어야 합니다.<br/><br/>
    /// *** 인자로 들어온 컴포넌트 참조값이 null이라면 예외를 발생시킵니다. ***<br/>
    /// </summary>
    /// <returns>오브젝트가 생성 될 빈 공간이 부족하다면 false를, 아이템 생성 성공 시 true를 반환</returns>
    public bool AddItem(ItemInfo itemInfo)
    {     
        // ItemInfo를 전달받지 못한 경우
        if( itemInfo==null )
            throw new Exception( "전달 받은 아이템 정보의 참조 값이 존재하지 않습니다." );

        // 슬롯에 빈자리가 없으면 실행하지 않습니다.
        if( !IsSlotEnough( itemInfo ) )
            return false;


        // 아이템의 타입이 잡화종류인 경우 전후수량을 비교하여 처리합니다.
        if( itemInfo.Item.Type is ItemType.Misc )
            AddItemMisc(itemInfo);
        // 일반 아이템인 경우 바로 추가합니다.
        else
            AddItemBasic(itemInfo);

        // 아이템 추가의 성공을 반환합니다.
        return true;
    }

    

    /// <summary>
    /// 일반 아이템(비잡화 아이템)의 경우에 아이템 추가 시 해야할 로직을 모아놓은 메서드입니다.<br/>
    /// 아이템 추가 후 해당 아이템의 정보를 오브젝트에 반영합니다.<br/>
    /// 활성화 슬롯인덱스 선택인자를 전달하여 특정 슬롯에 추가할 지 여부를 선택할 수 있습니다. (기본값: 가까운 인덱스 할당)<br/>
    /// </summary>
    protected void AddItemBasic(ItemInfo itemInfo, int activeSlotIndex=-1)
    {
        if( itemInfo.Item.Type == ItemType.Misc )
            throw new Exception("일반 아이템이 아닙니다. 잡화 아이템 전용 메서드를 호출해야합니다.");

        inventory.AddItem( itemInfo, activeSlotIndex, interactive.IsActiveTabAll ); // 아이템을 내부 인벤토리에 추가합니다.
        itemInfo.OnItemAdded(this);                                                 // 아이템에 최신 정보를 반영합니다.
    }


    /// <summary>
    /// 잡화 아이템의 경우 아이템 추가 시 해야 할 로직을 모아놓은 메서드입니다.<br/>
    /// 전후수량을 비교하여 수량 변동이 생겼다면 동일한 이름의 잡화아이템의 텍스트를 모두 업데이트하고, 
    /// 해당 아이템의 정보를 오브젝트에 반영합니다.<br/>
    /// 활성화 슬롯인덱스 선택인자를 전달하여 특정 슬롯에 직접 추가할 지 여부를 선택할 수 있습니다. (기본값: 가까운 인덱스 할당)<br/>
    /// 잡화 아이템의 경우 동일 이름의 아이템에 겹치지 않고 추가할 수 있는 옵션이 따로 존재합니다. (기본값: 겹쳐서 추가)
    /// </summary>
    /// <returns>아이템이 중첩되어 파괴되었다면 true를, 파괴되지 않았다면 false를 반환합니다.</returns>
    protected bool AddItemMisc(ItemInfo itemInfo, int activeSlotIndex=-1, bool isAbleToOverlap=true)
    {
        if( itemInfo.Item.Type != ItemType.Misc )
            throw new Exception("해당 아이템이 잡화아이템이 아닙니다.");

        ItemMisc itemMisc = (ItemMisc)itemInfo.Item;

        // 추가 이전의 수량 계산
        int beforeOverlapCount = itemMisc.OverlapCount;

        // 아이템을 내부 인벤토리에 추가합니다.
        inventory.AddItem( itemInfo, activeSlotIndex, interactive.IsActiveTabAll, isAbleToOverlap );

        // 추가 이후의 수량계산
        int afterOverlapCount = itemMisc.OverlapCount;

        // 수량정보가 변동이 있다면, 기존 아이템과 동일한 이름의 아이템의 수량정보를 업데이트합니다.
        if( beforeOverlapCount!=afterOverlapCount )
            UpdateTextInfoSameItemName( itemMisc.Name );

        // 아이템 수량이 (변동이 없거나, 변동이 있어도) 남은 상태라면, 
        if( afterOverlapCount!=0 )
        {            
            itemInfo.OnItemAdded( this );   // 아이템에 최신 정보를 반영합니다. 
            return false;                   // 아이템의 생존을 반환합니다.
        }
        // 아이템 수량이 0이라면,
        else
            return true;                    // 아이템의 파괴를 반환합니다.
    }
     

    /// <summary>
    /// 인자로 전달한 이름과 동일한 기존 아이템의 수량을 모두 업데이트 해주는 메서드입니다<br/>
    /// AddItem에서 내부적으로 사용됩니다. 
    /// </summary>
    /// <param name="itemName"></param>
    protected void UpdateTextInfoSameItemName(string itemName)
    {
        List<ItemInfo> itemInfoList = inventory.GetItemInfoList(itemName);

        if(itemInfoList == null || itemInfoList.Count==0 )
            return;

        foreach(ItemInfo itemInfo in itemInfoList )
            itemInfo.UpdateTextInfo();

    }



    /// <summary>
    /// 인벤토리의 목록에 기존의 아이템을 *활성화 슬롯*에 추가합니다.<br/>
    /// 아이템 정보와 활성화 슬롯의 Transform 참조값을 전달해야 합니다.<br/>
    /// 잡화 아이템의 경우 옵션으로 겹쳐서 추가할지 여부를 선택할 수 있습니다. (기본값: 겹치지 않고 추가)<br/><br/>
    /// *** 인자로 들어온 컴포넌트 참조값이 null이거나, 슬롯 인덱스가 잘못되었다면 예외를 발생시킵니다. ***<br/>
    /// </summary>
    /// <returns>오브젝트가 생성 될 빈 공간이 부족하다면 false를, 아이템 생성 성공 시 true를 반환</returns>
    public bool AddItemToSlot(ItemInfo itemInfo, Transform activeSlotTr, bool isAbleToOverlap=false)
    {
        if( itemInfo==null || activeSlotTr==null )
            throw new Exception( "전달 받은 참조 값이 존재하지 않습니다." );
                        
        return AddItemToSlot(itemInfo, activeSlotTr.GetSiblingIndex(), isAbleToOverlap);
    }



    /// <summary>
    /// 인벤토리의 목록에 기존의 아이템을 *활성화 슬롯*에 추가합니다.<br/>
    /// 아이템 정보와 활성화 슬롯 인덱스를 전달해야 합니다.<br/>
    /// 잡화 아이템의 경우 옵션으로 겹쳐서 추가할지 여부를 선택할 수 있습니다. (기본값: 겹치지 않고 추가)<br/><br/>
    /// *** 인자로 들어온 컴포넌트 참조값이 null이거나, 슬롯 인덱스가 잘못되었다면 예외를 발생시킵니다. ***<br/>
    /// </summary>
    /// <returns>오브젝트가 생성 될 빈 공간이 부족하다면 false를, 아이템 생성 성공 시 true를 반환</returns>
    public bool AddItemToSlot(ItemInfo itemInfo, int activeSlotIndex, bool isAbleToOverlap=false)
    {
        if( itemInfo==null )
            throw new Exception( "전달 받은 아이템 참조 값이 존재하지 않습니다." );

        // 해당 슬롯에 자리가 없다면, 실패를 반환
        if( !IsSlotEmpty(itemInfo, activeSlotIndex) )
            return false;
        
        // 아이템 종류가 잡화아이템인지 확인
        if(itemInfo.Item.Type == ItemType.Misc )
            AddItemMisc(itemInfo, activeSlotIndex, isAbleToOverlap);
        else
            AddItemBasic(itemInfo, activeSlotIndex);

        

        return true;
    }


    /// <summary>
    /// 인벤토리의 목록에 기존의 아이템을 *다른 아이템이 사용하던 특정 슬롯에 그대로* 추가합니다.<br/>
    /// 아이템 정보와 활성화 슬롯 인덱스와 비활성화 슬롯 인덱스를 전달해야 합니다.<br/>
    /// 잡화 아이템의 경우 옵션으로 겹쳐서 추가할지 여부를 선택할 수 있습니다. (기본값: 겹치지 않고 추가)<br/><br/>
    /// *** 인자로 들어온 컴포넌트 참조값이 null이거나, 슬롯 인덱스가 잘못되었다면 예외를 발생시킵니다. ***<br/>
    /// </summary>
    /// <returns>오브젝트가 생성 될 빈 공간이 부족하다면 false를, 아이템 생성 성공 시 true를 반환</returns>
    public bool AddItemToSlot(ItemInfo itemInfo, int activeSlotIndex, int inactiveSlotIndex, bool isAbleToOverlap=false)
    {
        if( itemInfo==null )
            throw new Exception( "전달 받은 아이템 참조 값이 존재하지 않습니다." );

        // 해당 슬롯에 자리가 없다면, 실패를 반환
        if( !IsSlotEmpty(itemInfo, activeSlotIndex) )
            return false;

        // 아이템 종류가 잡화아이템인지 확인
        if( itemInfo.Item.Type == ItemType.Misc )
        {
            // 중첩상태를 반영하여 잡화아이템의 추가를 진행하고 파괴되었다면,오브젝트가 남아있지 않으므로 바로 성공을 반환
            if( AddItemMisc( itemInfo, activeSlotIndex, isAbleToOverlap ) )
                return true;
        }
        else
            AddItemBasic( itemInfo, activeSlotIndex );

        // 오브젝트가 남아있는 경우 나머지 슬롯 인덱스를 설정합니다.
        if(interactive.IsActiveTabAll)
            itemInfo.SlotIndexEach = inactiveSlotIndex;
        else
            itemInfo.SlotIndexAll = inactiveSlotIndex;

        // 아이템 추가가 완료되었으므로 성공을 반환합니다.
        return true;
    }










        
    /// <summary>
    /// 인자로 전달 한 이름에 해당 하는 아이템의 수량이 얼마인지를 알려줍니다.
    /// </summary>
    /// <returns>잡화 아이템의 경우에는 중첩 수량을, 비잡화 아이템의 경우에는 오브젝트 갯수를 반환</returns>
    public int HowManyCount(string itemName)
    {
        return inventory.HowManyCount(itemName);
    }



    










}
