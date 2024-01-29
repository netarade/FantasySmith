using CreateManagement;
using DataManagement;
using ItemData;
using System;
using UnityEngine;
using UnityEngine.UIElements;

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
* <v3.1 - 2024_0126_최원준>
* 1- OnItemEquip메서드를 기본적으로 인자로 전달받은 Transform의 계층 하위에만 속하게 해주고 
* 위치정보를 받아가지 않도록 하였음. 아이템에 내부적으로 저장되어있는 STransform을 참조하여 변환정보를 결정.
* 
* + 주석 보완
* 
* 2- OnItemEquip 메서드가 아이템이 원래 가지고 변환 정보를 기록하도록 STransform equipPrevTr 변수 추가
* 
* 3- OnItemUnEquip메서드 작성 - 다시 원래의 변환정보로 돌아가게 하는 기능
* 
* 4- 아이템을 장착할 때 하위 2d오브젝트의 스케일 값이 부모 3d스케일 값에 따라 변하므로 다시 원래대로 돌려주는 기능 추가
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




    /// <summary>
    /// ItemStatus구조체를 전달받아 해당 수치만큼 수량에 변화를 주는 메서드의 대리자
    /// </summary>
    public delegate void StatusChangeAction(ItemStatus status);

    /// <summary>
    /// 아이템의 종류가 ItemFood인 경우에 호출가능한 메서드입니다.<br/>
    /// 아이템을 섭취하여 캐릭터의 스테이터스에 영향을 줍니다.<br/><br/>
    /// </summary>
    /// <returns>아이템 섭취에 성공하면 true를, 실패하면 false를 반환</returns>
    public bool OnItemEat(StatusChangeAction StatusChange)
    {        
        ItemFood itemFood = item as ItemFood;

        if( itemFood==null )
            throw new Exception("아이템의 종류가 음식이 아닙니다.");
        
        if(StatusChange == null)
            throw new Exception("스테이터스를 변경시키는 메서드가 전달되지 않았습니다.");

        // 수량감소에 성공하면 스테이터스 변화 메서드를 호출하고 true를 반환
        if( inventoryInfo.ReduceItem( this, 1 ) )
        {
            StatusChange( StatusInfo );
            return true;
        }
        // 수량감소에 실패하면 false를 반환
        else
            return false;
    }






    bool isEquip = false;                       // 아이템의 현재 장착여부
    STransform equipPrevTr = new STransform();   // 아이템이 장착 이전에 가지고 있던 변환정보 


    /// <summary>
    /// 아이템을 장착할 때 호출해줘야 하는 메서드입니다.<br/>
    /// 인자로 전달된 Transform의 계층하위 자식으로서 속하게 되며,<br/>
    /// 아이템이 내부적으로 보유하고 있는 STransform 정보와 동기화 합니다.<br/><
    /// *** 장비 아이템이 아니라면 예외가 발생합니다. ***
    /// </summary>
    public void OnItemEquip(Transform equipTr)
    {
        if( !(item is ItemEquip) )
            throw new Exception("아이템이 장착가능한 타입이 아닙니다.");
        if( equipTr==null )
            throw new Exception("장비 아이템을 장착할 계층정보를 전달해야 합니다.");
        if( isEquip )
            throw new Exception("이 아이템은 이미 장착 상태이므로 장착을 할 수 없습니다.");

        // 아이템이 계층정보가 변경되기 전의 절대 크기를 기록합니다.
        Vector3 prevItem2dLossyScale = itemRectTr.lossyScale;

        // 아이템의 계층정보가 변하기 전(2D상태에서) 원본 로컬 변환정보를 기록합니다.
        equipPrevTr.Serialize(Item3dTr, true);        

        // 월드에 아이템을 방출하고 equipTr의 계층에 종속시킵니다.
        OnItemWorldDrop(equipTr, TrApplyRange.Pos, true);
        
        // 아이템이 기본적으로 가지고 있는 STransform 값을 Item3dTr의 로컬정보에 적용시켜줍니다.
        SerializedTr.Deserialize(Item3dTr, true);
                
        // 2d스케일이 하위 자식상태에서 변동이 일어나므로 원래의 값으로 맞추어줍니다.
        // 그렇지 않으면 다시 슬롯에 돌아갈 때 스케일이 변형된 상태가 되므로 이미지 사이즈가 변하게 될 것입니다
        STransform.SetLossyScale(itemRectTr, prevItem2dLossyScale);

        // 아이템의 기본 콜라이더를 비활성화 합니다.
        ItemCol.enabled = false;

        // 장착상태를 활성화합니다.
        isEquip = true;
    }

   






    /// <summary>
    /// 아이템 장착을 해제할 때 호출해줘야 하는 메서드입니다.<br/>
    /// 장착 이전 아이템의 변환 정보로 값을 회귀합니다.<br/><br/>
    /// </summary>
    public void OnItemUnequip( QuickSlot quickSlot, int slotIndex )
    {
        if( !isEquip )
            throw new Exception( "장착 상태가 아니므로 장착을 해제할 수 없습니다." );

        if( quickSlot==null )
            throw new Exception( "퀵슬롯 스크립트 정보가 없습니다." );
        if( slotIndex<0 || slotIndex >= quickSlot.slotLen )
            throw new Exception( "슬롯 인덱스 정보가 잘못되었습니다." );

        
        // 장착한 아이템을 지정 슬롯에 다시 추가합니다.
        quickSlot.AddItemToSlot( this, slotIndex, quickSlot.interactive.IsActiveTabAll );
                      
        // 아이템이 보유하고 있는 이전 로컬 변환정보를 Item3DTr에 입력합니다.
        equipPrevTr.Deserialize( Item3dTr, true );

        // 아이템의 기본 콜라이더를 활성화 합니다.
        ItemCol.enabled = true;

        // 장착 상태를 비활성화 합니다.
        isEquip = false;
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
