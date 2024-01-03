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
    /// 해당 이름의 아이템을 인벤토리의 목록에서 제거후에 목록에서 제거한 아이템의 ItemInfo 참조값을 반환합니다.<br/>
    /// 제거 후 바로 파괴하려면 두번 째 인자를 true로 만들어야 합니다. (기본적으로 목록에서 제거만 될 뿐 파괴되지 않습니다.)<br/><br/>
    /// 반환 받은 아이템을 월드에 내보내기 위해서는 해당 ItemInfo참조의 OnItemWroldDrop메서드를 사용해야 하며,<br/>
    /// 다른 인벤토리로 주기 위해서는 다른 InventoryInfo참조의 AddItem메서드를 사용해 ItemInfo를 전달해야 합니다.<br/><br/>
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
    /// 인벤토리의 목록에 월드에 존재하는 아이템을 추가하거나<br/>
    /// 다른 인벤토리에 존재하는 아이템을 추가합니다.<br/>
    /// *** 인자로 들어온 컴포넌트 참조값이 없으면 예외를 발생시킵니다. ***<br/>
    /// </summary>
    public bool AddItem(ItemInfo itemInfo)
    {        
        if( itemInfo==null )
            throw new Exception( "전달 받은 아이템 정보의 참조 값이 존재하지 않습니다." );  

        ItemType inItemType = itemInfo.Item.Type;
        string inItemName = itemInfo.Item.Name;

        // 잡화 아이템 여부와 내부 인벤토리에 해당 잡화아이템과 동일한 잡화아이템이 있는지 검사
        if( inItemType == ItemType.Misc && inventory.IsExist(inItemName) )
        {            
            ItemMisc inItemMisc = (ItemMisc)itemInfo.Item;
            int inCount = inItemMisc.OverlapCount; 
            
            List<GameObject> existItemObjList = inventory.GetItemObjectList(inItemName);
            


            inventory.SetOverlapCount(inItemName, inCount);


        }


        // 비잡화 아이템이거나, 동일한 이름의 잡화아이템이 없는 경우
        return inventory.AddItem( itemInfo );
    }



    /// <summary>
    /// 인벤토리의 목록에 없는 아이템을 새롭게 생성을 요청합니다.
    /// </summary>
    public bool CreateItemToNearstSlot()
    {
        return true;
    }





}
