using UnityEngine;
using InventoryManagement;
using DataManagement;
using ItemData;
using System;
using System.Collections.Generic;

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
 */


public partial class InventoryInfo : MonoBehaviour
{
    /// <summary>
    /// Inventory클래스에서 필요 시 아이템 정보를 전송받기 위해 사용하는 ItemInfo를 임시적으로 담는 리스트입니다.<br/>
    /// AddItem메서드에서 
    /// </summary>
    List<ItemInfo> tempInfoList = new List<ItemInfo>();






    /// <summary>
    /// 해당 이름의 아이템을 인벤토리의 목록에서 제거후에 목록에서 제거한 아이템의 ItemInfo 참조값을 반환합니다.<br/>
    /// 제거 후 바로 파괴하려면 두번 째 인자를 true로 만들어야 합니다. (기본적으로 목록에서 제거만 될 뿐 파괴되지 않습니다.)<br/><br/>
    /// 반환 받은 아이템을 월드에 내보내기 위해서는 해당 ItemInfo참조의 OnItemWroldDrop메서드를 사용해야 하며,<br/>
    /// 월드로 나간 아이템을 다른 인벤토리로 주기 위해서는 다른 InventoryInfo참조의 AddItem메서드를 사용해 ItemInfo를 전달해야 합니다.<br/><br/>
    /// *** 인벤토리에 해당 이름의 아이템이 없으면 예외를 발생시킵니다. ***<br/>
    /// </summary>
    public ItemInfo RemoveItem(string itemName, bool isDestroy=false)
    {
        ItemInfo targetItemInfo = inventory.RemoveItem(itemName);   // 인벤토리 목록에서 제거
        
        if( targetItemInfo==null )
            throw new Exception( "해당 이름의 아이템이 존재하지 않습니다." );        
        else if( isDestroy )
        {
            Destroy( targetItemInfo.gameObject );  // 파괴 옵션이 걸려있다면, 아이템을 파괴하고 null을 반환합니다.     
            return null;
        }

        return targetItemInfo;
    }

    /// <summary>
    /// 인벤토리의 목록에 *월드*에 존재하는 기존의 아이템을 추가합니다.<br/>
    /// 다른 인벤토리에 존재하는 아이템은 OnItemSlotDrop메서드 호출을 통해 다른 인벤토리로 전이되어야 합니다.<br/><br/>
    /// *** 인자로 들어온 컴포넌트 참조값이 null이라면 예외를 발생시킵니다. ***<br/>
    /// </summary>
    /// <returns>오브젝트가 생성 될 빈 공간이 부족하다면 false를, 아이템 생성 성공 시 true를 반환</returns>
    public bool AddItem(ItemInfo itemInfo)
    {     
        // ItemInfo를 전달받지 못한 경우
        if( itemInfo==null )
            throw new Exception( "전달 받은 아이템 정보의 참조 값이 존재하지 않습니다." ); 
        // 아이템이 월드상에 존재하는 상태라면, 2D로 계층구조를 변경합니다.
        else if( itemInfo.IsWorldPositioned )
            itemInfo.TransferWorldTo2D();

        // 내부 인벤토리에 아이템이 성공적으로 추가되었다면,
        if( inventory.AddItem(itemInfo) )
        {
            // 인벤토리 정보를 인자로 전달받은 새로운 인벤토리로 업데이트 합니다.
            itemInfo.UpdateInventoryInfo(this);

            // 아이템의 슬롯 인덱스 정보를 가장 가까운 슬롯으로 입력합니다.        
            SetItemSlotIdxBothToNearstSlot(itemInfo);  
            
            // 아이템의 위치정보를 반영합니다.
            itemInfo.UpdatePositionInSlotList();

            // 아이템 추가 성공을 반환합니다.
            return true;
        }
        // 추가되지 않았다면 실패를 반환합니다.
        else
            return false;        
    }

    


    /// <summary>
    /// 인벤토리의 목록에 없는 아이템을 새롭게 생성하고 인벤토리 목록에 추가합니다.<br/>
    /// 오브젝트 1개만 생성하므로, 여러 아이템을 생성시키고 싶을 때 중복하여 호출해야 합니다.<br/>
    /// 잡화아이템의 경우 중첩수량을 설정할 수 있습니다. 비잡화 아이템의 경우는 무시합니다.(기본값:1)<br/>
    /// </summary>
    /// <returns>인벤토리의 슬롯에 아이템 1개를 생성할 공간이 충분하지 않다면 false를 반환, 성공한 경우 true를 반환</returns>
    public bool CreateItem(string itemName, int miscOverlapCount=1)
    {
        return true;
    }

    /// <summary>
    /// 인벤토리의 목록에 없는 아이템을 새롭게 생성하고 인벤토리 목록에 추가합니다.<br/>
    /// 인자로 아이템 이름과 중첩 수량 묶음 구조체를 전달받습니다.<br/><br/>
    /// 오브젝트 단위로 여러 아이템을 생성할 수 있습니다.<br/>
    /// 비잡화아이템의 경우 ItemPair의 overlapcount는 무시됩니다.<br/>
    /// </summary>
    /// <returns>인벤토리의 슬롯에 아이템 여러 개를 생성할 공간이 충분하지 않다면 false를 반환, 성공한 경우 true를 반환</returns>
    public bool CreateItem(ItemPair[] itemPairs)
    {
        return true;
    }





}
