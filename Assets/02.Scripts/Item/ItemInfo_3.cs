using ItemData;
using System;
using UnityEngine;

/*
* [작업 사항]
* <v1.0 - 2024_0112_최원준>
* 1- 아이템 사용시 처리를 위한 분할 클래스 작성 
* 
* <v1.1 - 2024_0114_최원준>
* 1- OnItemEquip 메서드 작성
* OnItemWorldDrop의 setParent형태로 이루어지게 되며, 
* 다시 자기참조값을 반환하여 메서드를 사용한 다음 한줄에 저장하기 쉽도록 구현
* 
* <v1.2 -2024_0116_최원준>
* 1- PrintDebugInfo메서드 작성
* 
* <v2.0 - 2024_0118_최원준>
* 1- CheckToDestroy메서드를 작성
* Info클래스에서 외부 사용자가 아이템 정보를 변동시키게 만들 예정이므로,
* 아이템 내부정보가 바뀔 때마다 파괴여부를 체크해서 자동으로 파괴시켜버리도록 하기위함.
* 
* <v2.1 - 2024_0118_최원준>
* 1- CheckDestroyInfo메서드에서 2D 상태일 때는 2D 오브젝트를 파괴해야하고, 3D 상태일때는 3D오브젝트를 파괴하도록 수정
* 
*/

public partial class ItemInfo : MonoBehaviour
{    
    
    /// <summary>
    /// 아이템의 정보 변동으로 인한 파괴여부를 체크합니다.<br/>
    /// 내구도 조건에 따른 파괴, 중첩수량에 따른파괴 
    /// </summary>
    public void CheckDestroyInfo()
    {
        bool isRemove = false;
        ItemWeapon itemWeapon = item as ItemWeapon;
        ItemBuilding itemBuilding = item as ItemBuilding;
        ItemMisc itemMisc = item as ItemMisc;

        // 무기와 건설재료아이템의 경우 내구도를 체크합니다.
        if( itemWeapon!=null )
        {
            if( itemWeapon.Durability<=0 )
                isRemove=true;
        }
        else if( itemBuilding!=null )
        {
            if( itemBuilding.Durability<=0 )
                isRemove=true;
        }
        
        // 잡화아이템의 경우 중첩수량을 체크합니다.
        if( itemMisc!=null )
        {
            if( itemMisc.OverlapCount<=0 )
                isRemove = true;
        }

        // 파괴여부가 활성화된 경우
        if( isRemove )
        {
            // 인벤토리가 있다면 목록에서 제거
            if( inventoryInfo!=null )
                inventoryInfo.RemoveItem( this );   
            
            /*** 월드 상태에 따라서 아이템 파괴 ***/
            if(isWorldPositioned)
                Destroy( transform.parent.gameObject );     // 부모 3D 아이템 오브젝트를 파괴합니다.  
            else
                Destroy( this.gameObject );                 // 2D 아이템 오브젝트를 파괴합니다.
        }
    }







    public void OnItemUse()
    {
            
    }



    public void OnItemDrink()
    {

    }


    /// <summary>
    /// 장비 아이템을 장착합니다.<br/>
    /// 인자로 전달된 Transform과 동일한 위치와 회전정보를 가지며, 계층하위 자식으로서 속하게 됩니다.<br/><br/>
    /// *** 장비 아이템이 아니라면 예외가 발생합니다. ***
    /// </summary>
    /// <returns>해당 아이템의 ItemInfo 값을 다시 반환</returns>
    public ItemInfo OnItemEquip(Transform equipTr)
    {
        if( !(item is ItemEquip) )
            throw new Exception("아이템이 장착가능한 타입이 아닙니다.");

        OnItemWorldDrop(equipTr, true);
        return this;
    }













    public void PrintDebugInfo()
    {
        string debugInfo = "";
        
        debugInfo += string.Format($"이름 : {item.Name}\n" );
        debugInfo += string.Format($"종류 : {item.Type}\n" );
        debugInfo += string.Format($"전체 탭 인덱스 : {item.SlotIndexAll}\n" );
        debugInfo += string.Format($"개별 탭 인덱스 : {item.SlotIndexEach}\n" );

        print(debugInfo);
    }






}
