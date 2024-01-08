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
 * <v1.1 - 2024_0105_최원준>
 * 1- AddItem메서드에서 먼저 인벤토리에 추가하고 인덱스를 입력하는 것에서
 * 인덱스를 먼저 입력하고 인벤토리에 추가하는 구조로 변경하였음.
 * 이유는 미리 추가한 아이템의 인덱스 정보를 인덱스를 구하는 과정에서 읽어들이기 때문.
 * 
 * <v1.2 -2024_0108_최원준>
 * 1- ItemInfo를 직접 전달하여 인벤토리 목록에서 제거해주는 RemoveItem 오버로딩 메서드 추가
 * ItemSelect 스크립트에서 인벤토리에서 외부로 아이템을 드랍하였을 때 해당 아이템을 직접 제거해야하기 때문
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
    /// 제거 후 바로 파괴하려면 두번 째 인자를 true로 설정합니다. (기본적으로 파괴되지 않습니다.)<br/><br/>
    /// 제거 한 아이템은 자동으로 World의 InventoryInfo클래스의 playerDropTr로 지정해둔 곳에 떨어트려줍니다.<br/><br/>
    /// 월드로 나간 아이템을 다른 인벤토리로 주기 위해서는 다른 InventoryInfo참조의 AddItem메서드를 사용해 다시 ItemInfo를 전달해야 합니다.<br/><br/>
    /// *** 인벤토리에 해당 이름의 아이템이 없으면 예외를 발생시킵니다. ***<br/>
    /// </summary>
    public ItemInfo RemoveItem(string itemName, bool isDestroy=false)
    {
        // 인벤토리 목록에서 제거하고 반환 할 참조값을 저장합니다.
        ItemInfo targetItemInfo = inventory.RemoveItem(itemName);   
        
        if( targetItemInfo==null )
            throw new Exception( "해당 이름의 아이템이 존재하지 않습니다." );       
        
        // 파괴 옵션이 걸려있다면, 아이템을 파괴하고 null을 반환합니다.
        else if( isDestroy )
        {
            Destroy( targetItemInfo.gameObject );       
            return null;
        }
        
        // 아이템을 3D상태로 전환합니다.
        targetItemInfo.DimensionShift(true);

        return targetItemInfo;
    }


    /// <summary>
    /// 해당 아이템을 인벤토리의 목록에서 직접 제거후에 목록에서 제거한 아이템의 ItemInfo 참조값을 반환합니다.<br/>
    /// 제거 후 바로 파괴하려면 두번 째 인자를 true로 설정합니다. (기본적으로 파괴되지 않습니다.)<br/><br/>
    /// 제거 한 아이템은 자동으로 World의 InventoryInfo클래스의 playerDropTr로 지정해둔 곳에 떨어트려줍니다.<br/><br/>
    /// 월드로 나간 아이템을 다른 인벤토리로 주기 위해서는 다른 InventoryInfo참조의 AddItem메서드를 사용해 다시 ItemInfo를 전달해야 합니다.<br/><br/>
    /// *** 인벤토리에 해당 이름의 아이템이 없으면 예외를 발생시킵니다. ***<br/>
    /// </summary>
    public ItemInfo RemoveItem(ItemInfo itemInfo, bool isDestroy=false)
    {
        // 인벤토리 목록에서 제거하고 반환 할 참조값을 저장합니다.
        ItemInfo targetItemInfo = inventory.RemoveItem(itemInfo);   
        
        if( targetItemInfo==null )
            throw new Exception( "해당 이름의 아이템이 존재하지 않습니다." );       
        
        // 파괴 옵션이 걸려있다면, 아이템을 파괴하고 null을 반환합니다.
        else if( isDestroy )
        {
            Destroy( targetItemInfo.gameObject );       
            return null;
        }
        // 아이템을 3D상태로 전환합니다.
        targetItemInfo.DimensionShift(true);

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

        // 슬롯에 빈자리가 없으면 실행하지 않습니다.
        if( !IsSlotEnough( itemInfo ) )
            return false;

        // 아이템이 월드상에 존재하는 상태라면, 2D로 계층구조를 변경합니다.
        if( itemInfo.IsWorldPositioned )
            itemInfo.DimensionShift( false );
                
        // 인벤토리 정보를 새로운 인벤토리로 업데이트 합니다.
        itemInfo.UpdateInventoryInfo(this);
        
        // 아이템정보를 현재 인벤토리의 가까운 슬롯으로 입력합니다.        
        SetItemSlotIdxBothToNearstSlot(itemInfo);  

        // 내부 인벤토리에 아이템을 추가합니다.
        inventory.AddItem(itemInfo);  

        
        // 아이템의 최신 정보를 반영합니다 
        if(itemInfo.Item.Type == ItemType.Misc)
            UpdateAllItemInfo();    // 잡화아이템의 경우 기존 아이템에 수정이 들어갔을 수도 있으므로 모든 정보 업데이트
        else
            itemInfo.UpdatePositionInSlotList(); // 기존 아이템은 해당 아이템만 업데이트 합니다.

        // 아이템 추가 성공을 반환합니다.
        return true;
        
            
    }

    


    /// <summary>
    /// 인벤토리의 목록에 *없는* 아이템을 새롭게 생성하고 인벤토리 목록에 추가합니다.<br/>
    /// 오브젝트 1개만 생성하므로, 여러 아이템을 생성시키고 싶을 때 중복하여 호출해야 합니다.<br/>
    /// 잡화아이템의 경우 중첩수량을 설정할 수 있습니다. 비잡화 아이템의 경우는 무시합니다.(기본값:1)<br/>
    /// </summary>
    /// <returns>인벤토리의 슬롯에 아이템 1개를 생성할 공간이 충분하지 않다면 false를 반환, 성공한 경우 true를 반환</returns>
    public bool AddItem(string itemName, int overlapCount=1)
    {
        Debug.Log("슬롯 여부 " + IsSlotEnough(itemName, overlapCount));

        if( !IsSlotEnough(itemName, overlapCount) )
            return false;
                
        // createManager에게 생성요청을하여 오브젝트 하나를 만듭니다.
        ItemInfo itemInfo = createManager.CreateWorldItem(itemName, overlapCount);

        // 해당 아이템을 인벤토리에 넣습니다.
        AddItem(itemInfo);

        return true;
    }

    /// <summary>
    /// 인벤토리의 목록에 *없는* 아이템을 새롭게 생성하고 인벤토리 목록에 추가합니다.<br/>
    /// 인자로 아이템 이름과 중첩 수량 묶음 구조체를 전달받습니다.<br/><br/>
    /// 오브젝트 단위로 여러 아이템을 생성할 수 있습니다.<br/>
    /// 비잡화아이템의 경우 ItemPair의 overlapcount는 무시됩니다.<br/>
    /// </summary>
    /// <returns>인벤토리의 슬롯에 아이템 여러 개를 생성할 공간이 충분하지 않다면 false를 반환, 성공한 경우 true를 반환</returns>
    public bool AddItem(ItemPair[] itemPairs)
    {
        return true;
    }









}
