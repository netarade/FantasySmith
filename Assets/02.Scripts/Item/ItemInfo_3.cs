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
* <v3.2 - 2024_0129_최원준>
* 1- 아이템 장착시 리지드바디의 중력을 끄고, 장착 해제 시 중력을 다시 키도록 구현 (무기가 떨어지므로)
* 
* <v3.3 - 2024_0130_최원준>
* 1- 변수명 equipPrevTr을 prevEquipLocalTr로 변경
* 
* 2- OnItemEquip OnItemUnequip메서드 대폭 수정
* a.매개변수명 equipTr을 equipParentTr로 변경
* b.더미 정보를 업데이트 하는 코드 추가
* c.반환 형식을 void에서 bool로 변경하여, 예외처리를 실패로 바꾸고, 실패에도 호출될 수 있도록 변경
* d.장착 시 아이템에 연결된 장착액션 대리자를 호출하도록 하였음.
* e.콜라이더와 리지드바디 null 검사문 추가
* 
* 
* <v4.0 - 2024_0214_최원준>
* 1- 메서드명 OnItemEquip과 OnItemUnequip을 EquipTransfer와 UnEquipTransfer로 바꾼 후 
* 접근제한을 public에서 private로 변경, 그리고 EquipSwitch 신규 메서드에 포함시킴.
* 
* 이유는 장착 기능을 3D 오브젝트를 전송시켜서 장착하는 종류와 기존의 3D오브젝트를 플레이어에 미리 넣어두고 활성화만 시켜주는 기능 2가지로 구분하기 위함
* 
* 2- EquipTransfer와 UnequipTransfer메서드에서 EquipAction 대리자 관련 코드와 isLoad관련 코드를 삭제하였음
* 
* 이유는 아이템 착용 시 플레이어 관련 추가 액션을 실행해주거나, 로드 시 상태를 대입해주기 위한 용도로 사용하고 있었는데,
* 
* 장착 메서드를 2종류로 구분하면서 
* 숨김 장착에서는 대리자 액션을 따로 등록할 필요없이 플레이어의 equipInfo의 메서드를 호출해야하고,
* 전송 장착에서는 로드 시에만 대리자 액션을 호출해주면 되는데 이를 게임 진행 중에 계속 들고 있거나, 개별 착용 메서드에서 복잡하게 코드로 분기하기보다
* 
* 개별 착용 메서드에서는 아이템의 착용만 담당해주고 통합 착용 메서드에서 분기하며,
* 플레이어 상태는 로드할 때만 예외적으로 다른 스크립트의 설정 메서드를 호출해주거나, 인벤토리에서 로드하면서 같이 호출해주는 것이 효율적이기 때문
* 
* 3- EquipTransfer, UnequipTransfer메서드를 호출 시 IsEquip 변수를 강제로 반전시켜주는 코드를 삭제하고, 
* EquipTransferSwitch메서드를 작성하여 해당 메서드를 항상 호출할 수 있도록 하여, 여기서 IsEquip 상태를 관리하도록하였음.
* 이유는 isLoad옵션을 넣어서 로드 시 상태 변화를 주지 않고 착용만 시켜주도록 하기 위함. 
* (기존에는 착용 메서드만 호출해도 상태가 반전되어버리기 때문에 호출 시 상태를 변경할지 말지를 구분하여 호출할 필요가 있음)
* 
* 4- EquipTransfer, UnequipTransfer를 void형으로 변경하고 성공 실패여부 반환 코드 삭제
* 이유는 EquipSwitch메서드에서 착용 종류와 상태를 직접 관리하며, 
* 로드 시에는 상태반전을 시도하지 않기 때문에, 동일한 상태에서 호출했을 때 동일 상태여부 검사 조건에 걸려서 실패하면 안되기 때문
* 
* 5- EquipSwitch 통합 메서드 호출을 통해 착용타입을 분기해서 전송 장착 메서드와 숨김 장착 메서드를 분기해서 호출해주고,
* 현재 착용상태를 관리하며, 로드 옵션을 선택인자로 넣어서 착용상태를 반영하지 않고 강제로 착용시킬 수 있게끔 구현
* 
* <v4.1 - 2024_0214_최원준>
* 1- EquipTransfer, UnequipTransfer메서드 내부에 dummyInfo 관련 메서드를 직접변경하던 것에서 OnItemEquipSwitch 통합 메서드 호출로 대체
* 
* <v4.2 - 2024_0216_최원준>
* 1- EquipSwitch메서드의 기존 isLoad 선택인자명을 isRemainEquipState로 변경하고,
* isForceUserState 선택인자를 추가하여 유저 상태 강제 반영 옵션과 장착 상태 유지 옵션을 구분하여 호출할 수 있게 해주었음
* 
* 이유는 로드 시 강제 장착 및 해제 하는 경우와 게임 진행 중(퀵슬롯 셀렉팅 시) 강제 장착 및 해제하는 경우가 구분되기 때문
* 
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


    /// <summary>
    /// 아이템을 착용 전환하는 메서드입니다.<br/>
    /// 착용 종류에 따라 다른 방식으로 착용을 진행합니다.<br/>
    /// 아이템에 따라 플레이어 계층에 숨겨진 아이템을 착용하는 방식과 3D 오브젝트를 전송하여 착용하는 방식으로 구분합니다.<br/><br/>
    /// 선택 인자로 유저 상태 강제 반영 옵션과 장착 상태 유지 옵션을 설정가능합니다.<br/>
    /// 무기를 강제로 장착 또는 해제 하는 경우 유저상태를 강제 반영 옵션을 활성화 해야 하며,<br/>
    /// 장착 상태 유지 옵션의 경우 로드 시에 장착 상태를 유지한 채로 장착 및 해제를 해야 합니다.
    /// </summary>
    /// <returns>착용전환 성공 시 true를, 실패 시 false를 반환</returns>
    public void EquipSwitch( bool isForceUserState = false, bool isRemainEquipState = false )
    {
        // 장착 상태 유지 옵션이 걸려있지 않은 경우에만 현재의 장착 상태를 반전시킵니다.
        if( !isRemainEquipState )
            IsEquip=!IsEquip;

        
        // 퀘스트 아이템이면서 머리에 착용하는 아이템인 경우
        if( EquipType==EquipType.Helmet && Type==ItemType.Quest )
        {
            // 현재 착용 상태를 기준으로 equipInfo의 숨김방식의 착용메서드를 호출합니다.
            equipInfo.EquipHoodSwitch( IsEquip );
        }


        // 무기로 착용하는 아이템인 경우
        else if( EquipType==EquipType.Weapon )
        {
            // 현재 착용상태를 기준으로 전송방식의 착용메서드를 호출합니다.
            if( IsEquip )
                EquipTransfer();    // 아이템을 전송하여 착용합니다.            
            else
                UnequipTransfer();  // 아이템을 전송하여 착용 해제합니다.


            //  유저 상태 강제 반영 옵션이 걸려있는 경우 (협업코드)
            if( isForceUserState )
            {
                PlayerWeapon playerWeapon = ownerTr.GetComponent<PlayerWeapon>();

                if( playerWeapon!=null )
                {
                    if(IsEquip)
                        playerWeapon.ChangeWeaponDirectly( this.WeaponType );
                    else
                        playerWeapon.ChangeWeaponDirectly( WeaponType.None );
                }
            }


        }



        else
            throw new Exception( "현재 EquipType에 관한 메서드가 매핑되지 않았습니다." );
    }




    STransform prevEquipLocalTr = new STransform();   // 아이템이 장착 이전에 가지고 있던 변환정보 

    /// <summary>
    /// 아이템을 장착할 때 호출해줘야 하는 메서드입니다.<br/>
    /// 인자로 전달된 Transform의 계층하위 자식으로서 속하게 되며,<br/>
    /// 아이템이 내부적으로 보유하고 있는 STransform 정보와 동기화 합니다.<br/><br/>
    /// *** 장비 아이템이 아니라면 예외가 발생합니다. ***
    /// </summary>
    private void EquipTransfer()
    {
        ItemEquip itemEquip = item as ItemEquip;

        if( itemEquip==null )
            throw new Exception( "아이템이 장착가능한 타입이 아닙니다." );


        // 아이템이 계층정보가 변경되기 전의 절대 크기를 기록합니다.
        Vector3 prevItem2dLossyScale = itemRectTr.lossyScale;

        // 아이템의 계층정보가 변하기 전(2D상태에서) 원본 로컬 변환정보를 기록합니다.
        prevEquipLocalTr.Serialize(Item3dTr, true);        
                

        // 장착 지점을 반환받습니다.
        Transform equipParentTr = equipInfo.GetEquipParentTr(this);

        // 인벤토리에 등록된 상태로 장착 지점에 아이템을 방출하고 계층에 종속시킵니다.
        OnItemWorldDrop(equipParentTr, TrApplyRange.Pos, true, true);
        
        // 아이템의 장착 지점을 기준으로한 로컬 STransform 값을 Item3dTr의 로컬정보에 적용시켜줍니다.
        itemEquip.EquipLocalTr.Deserialize(Item3dTr, true);
        
        
        // 2d스케일이 하위 자식상태에서 변동이 일어나므로 원래의 값으로 맞추어줍니다.
        // 그렇지 않으면 다시 슬롯에 돌아갈 때 스케일이 변형된 상태가 되므로 이미지 사이즈가 변하게 될 것입니다
        STransform.SetLossyScale(itemRectTr, prevItem2dLossyScale);
        
        // 아이템의 기본 콜라이더를 비활성화 합니다.
        if(itemCol!=null)
            itemCol.enabled = false;
        
        // 아이템의 중력을 해제합니다.
        if(itemRb!=null)
            itemRb.useGravity = false;

        // 아이템 장착에 따른 더미 정보를 업데이트합니다.
        dummyInfo.OnItemEquip(true); 
    }



    /// <summary>
    /// 아이템 장착을 해제할 때 호출해줘야 하는 메서드입니다.<br/>
    /// 장착 이전 아이템의 변환 정보로 값을 회귀합니다.<br/><br/>
    /// *** 장비 아이템이 아니라면 예외가 발생합니다. ***
    /// </summary>
    private void UnequipTransfer()
    {
        ItemEquip itemEquip = item as ItemEquip;
                
        if( itemEquip==null )
            throw new Exception( "아이템이 장착가능한 타입이 아닙니다." );
                        
        // 장착한 아이템 구조를 2D상태로 변경합니다.
        DimensionShift(false);

        // 아이템의 2D 포지션을 업데이트합니다.
        UpdatePositionInfo();

        // 아이템이 보유하고 있던 이전 로컬 변환정보를 Item3DTr에 입력하여 원상복귀 시켜줍니다.
        prevEquipLocalTr.Deserialize( Item3dTr, true );

        // 아이템의 기본 콜라이더를 활성화 합니다.
        if( itemCol!=null )
            itemCol.enabled = true;

        // 아이템의 중력을 활성화합니다.
        if( itemRb!=null )
            itemRb.useGravity = true;
        
        // 아이템 장착에 따른 더미 정보를 업데이트합니다.
        dummyInfo.OnItemEquip(false);

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
        worldInventoryInfo.Inventory.AddItem(this);

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
