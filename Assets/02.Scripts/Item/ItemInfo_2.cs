using ItemData;
using System;
using UnityEngine;


public partial class ItemInfo : MonoBehaviour
{
    /*
     * [작업 사항]
     * 
     * <v1.0 - 2023_1102_최원준>
     * 1- 아이템의 SetOverlapCount, Remove, IsEnoughOverlapCount메서드 정의
     * 아이템 수량을 증감시키거나, 삭제시키거나, 수량이 충분환지 확인하는 기능
     * 
     * 
     */

    
    /// <summary>
    /// 해당 잡화 아이템의 중첩수량을 조절하여 줍니다.<br/><br/>
    /// 양의 인자를 전달 했을 때, 해당 아이템의 최대 중첩수량을 초과하는 경우 나머지 초과수량을 반환합니다.<br/><br/>
    /// 음의 인자를 전달 했을 때, 해당 아이템의 최소 중첩수량(0)을 초과하는 경우 
    /// 아이템을 파괴 및 인벤토리 목록에서 제거하고, 나머지 수량을 반환합니다.<br/><br/>
    /// ***** 해당 아이템이 잡화아이템이 아니라면 예외를 던집니다. *****<br/>
    /// ** 단순히 아이템의 수량을 확인하고 싶은 경우 IsEnough메서드를 사용하기를 바랍니다. **<br/>
    /// </summary>
    /// <param name="inCount"></param>
    /// <returns>반환되는 수량은 증가 또는 감소시키고 최대, 최소 한계에 도달하여 남은 수량이며, 0이거나, 양의 정수이거나, 음의 정수입니다.</returns>
    public int SetOverlapCount(int inCount)
    {
        if( item.Type!=ItemType.Misc )
            throw new Exception( "잡화아이템이 아닙니다. 확인하여주세요." );

        int remainCount=0;
        ItemMisc itemMisc = (ItemMisc)item;

        remainCount = itemMisc.SetOverlapCount( inCount );
                
        if(itemMisc.OverlapCount==0)    // 아이템의 수량이 0이 되었다면,
            Remove();                   // 이 아이템의 삭제 메서드를 호출합니다.
        
        return remainCount;             // 남은 수량을 반환합니다.
    }


    /// <summary>
    /// 아이템을 인벤토리 목록에서 제거하고, 삭제해주는 메서드입니다.<br/>
    /// 파괴시키기 이전에 아이템 내부의 로직을 처리하기 위해 딜레이 시간을 인자로 줄 수 있습니다. (기본 삭제딜레이 시간은 0.001초입니다.)<br/>
    /// </summary>
    public void Remove(float timeToRemove=0.001f)
    {
        inventoryInfo.RemoveItem(this);             // 아이템을 현재 인벤토리 목록에서 제거합니다.   
        itemRectTr.SetParent(emptyListTr, false);   // 삭제하기 전 빈공간으로 바로 옮깁니다.         
        Destroy( this.gameObject, timeToRemove );   // 다른 로직 호출을 위하여 인자로 들어온 시간 만큼 약간 늦게 파괴합니다.
    }





    /// <summary>
    /// 이 아이템의 중첩수량이 충분한 지 확인하는 메서드입니다.<br/><br/>
    /// ** 호출 한 아이템이 잡화 종류가 아니라면 예외를 발생시킵니다. **
    /// </summary>
    /// <returns>인자로 들어온 수량보다 크거나 같다면 true를 작다면 false를 반환합니다.</returns>
    public bool IsEnoughOverlapCount(int overlapCount)
    {
        if(Item.Type != ItemType.Misc ) 
            throw new Exception("아이템의 종류가 잡화아이템이 아닙니다. 확인하여 주세요.");
        
        ItemMisc itemMisc = (ItemMisc)item;

        // 아이템의 중첩수량이 인자로 들어온 수량보다 같거나 크다면 충분한것으로 판단
        if(itemMisc.OverlapCount >= overlapCount)   
            return true;
        else
            return false;
    }



    

}
