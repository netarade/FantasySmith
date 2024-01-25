using CreateManagement;
using DataManagement;
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
* <v2.2 - 2024_0125_최원준>
* 1- RegisterToWorld 작성
* 인벤토리 소유자의 Transform 정보를 받아서 소유자id를 변경 후 월드 인벤토리에 저장하는 역할
* 
* <v2.3 - 2024_0125_최원준>
* 1- RegisterToWorld메서드 인자를 InventoryInfo를 기반으로 오버로딩, 기존 메서드는 오버로딩 메서드를 재활용, 주석 수정
* 
* <v3.0 - 2024_0126_최원준>
* 1- 플레이어가 UserInfo 스크립트에서 Id를 할당받고 시작함에 따라서, UserInfo를 매개변수로 통일 해야 하므로,
* RegisterToWorld 오버로딩 메서드(InventoryInfo 및 Transform)를 삭제하고 UserInfo를 전달받음.
* 
* 2- RegisterToWorld메서드명을 RegisterWorldInvetory로 변경
* 
* 3- SetPrivateId메서드 정의하여 아이템이 고유 식별 번호(Id)를 외부로부터 부여받을 수 있게 하였음.
* 
* 4- isRegistered 속성을 정의하여 월드인벤토리에 등록되었는지 여부를 기록하고, 해당 정보를 토대로 SetPrivateId를 추가호출 가능성 여부를 판단.
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


    bool isRegisterdToWorld = false;    // 월드 인벤토리에 등록되었는지 여부

    /// <summary>
    /// 아이템을 어떠한 상태도 전환 하지 않고 월드 인벤토리에 등록만 합니다.<br/>
    /// 즉, 3D상태의 아이템이 월드 인벤토리에 추가되어도 2D상태로 변경되지 않습니다.<br/><br/>
    /// 인자로 아이템의 소유자 정보를 전달해야 합니다.<br/><br/>
    /// *** 이 메서드를 통해 아이템이 월드 인벤토리에 등록되어도 고유식별번호는 따로 할당 받지 않습니다. ***
    /// </summary>
    /// <returns>자기자신의 참조값을 그대로 반환합니다. 추가로 다른 메서드를 호출할 수 있습니다</returns>
    public ItemInfo RegisterWorldInvetory(UserInfo ownerInfo)
    {
        // 아이템 소유자 식별번호를 유저의 식별번호로 지정합니다.
        item.OwnerId = ownerInfo.UserId;    

        // 아이템 소유자 명을 유저의 프리팹 명으로 지정합니다.
        item.OwnerName = ownerInfo.UserPrefabName;

        // 아이템 소유자 위치를 저장합니다.
        ownerTr = ownerInfo.transform;

        // 월드 인벤토리에 저장합니다.
        worldInventoryInfo.inventory.AddItem(this);

        // 등록되었음을 활성화합니다.
        isRegisterdToWorld = true;

        return this;
    }

    /// <summary>
    /// 아이템의 고유 Id를 할당받습니다.<br/>
    /// CreateManager가 부여한 고유 ID를 전달해야합니다.<br/><br/>
    /// *** 아이템이 월드에 등록된 상태가 아니라면 예외가 발생합니다. ***
    /// </summary>
    public void SetPrivateId(int id)
    {
        // 월드 인벤토리에 등록되지 않았다면 예외처리
        if( !isRegisterdToWorld )
            throw new Exception("월드 인벤토리에 등록된 상태가 아닙니다.");

        item.Id = id;
    }








    /// <summary>
    /// 디버그 출력용 메서드 - 아이템 정보를 확인합니다.
    /// </summary>
    public void PrintDebugInfo()
    {
        item.ItemDeubgInfo();
    }






}
