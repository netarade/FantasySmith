using ItemData;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/*
* [작업 사항]
* 
* <v1.0 - 2023_1102_최원준>
* 1- 아이템의 SetOverlapCount, Remove, IsEnoughOverlapCount메서드 ItemInfo클래스에서 옮겨옴
* 아이템 수량을 증감시키거나, 삭제시키거나, 수량이 충분환지 확인하는 기능
* 
* 2- 아이템의 SetOverlapCount, IsEnoughOverlapCount 메서드 삭제
* 이유는 아이템 자체적인 삭제나, 정보검색 기능을 넣게되면, 인벤토리가 있는 상태와 없는상태를 구분해서 코드를 넣어야 하기 때문이고,
* 인벤토리를 통하지 않으면 정보의 동기화 오류가 발생할 가능성이 크기 때문
* 
* 3- Remove메서드도 삭제
* Inventory 내부적으로 제거후 ItemInfo를 반환하면 그다음 삭제하면 딜레이를 줄 필요가 없기때문 
* 
* 
* <v2.0 - 2024_0104_최원준>
* 1- SlotDrop에 관한 관련 메서드들을 ItemInfo.cs에서 옮겨옴
* 
* <v2.1 - 2024_0105_최원준>
* 1- MoveSlotToAnotherListSlot메서드 내부 IsSlotEnough를 ItemType기반 호출에서 ItemInfo기반 호출로 변경
* => 잡화아이템의 경우 슬롯이 필요하지 않은 경우가 있기 때문
* 
* 2- SlotDrop 이벤트 발생 시 MoveSlotInSameListSlot내에서 isActiveTabAll일 때 slotIndex에 값을 넣던 점을 slotIndexAll로 변경
* 
* <2.2 - 2024_0106_최원준
* 1- ItemPointerStatusWindow에 존재하던 코드를 일부 옮겨옴.
* 아이템이 상태창 코드를 들고 있던것을 상태창으로 옮기고 포인터 이벤트 시에만 해당 상태창코드를 호출하는 방식으로 변경
* 
* 2- Pointer Enter와 PointerExit이벤트 상속 후 아이템의 포인터 접근이 일어날 때마다 상태창의 메서드를 호출
* 
* <v2.3 -2024_0112_최원준>
* 1- 테스트용 메서드 PrintDebugInfo 삭제
* 
* <v2.4 - 2024_0116_최원준>
* 1- MoveSlotToAnotherListSlot메서드 내부에
* 슬롯 충분 여부 검사 메서드를 IsSlotEnough를 IsSlotEnoughCertain으로 변경
* 
* <v2.5 - 2024_0116_최원준>
* 1- MoveSlotToAnotherListSlot메서드에서 
* 아이템의 이전인벤토리의 탭정보와 옮길 인벤토리의 탭정보가 일치하지 않으면 실패하도록 처리
* 
* 2- MoveSlotToAnotherListSlot메서드에서 지정 슬롯에 드랍되도록 AddItem메서드를 AddItemToSlot으로 변경
* 
* <v3.0 - 2024_0123_최원준>
* 1- 메서드명 변경 
* MoveSlotInSameListSlot    -> MoveSlotToSlotSameInventory
* MoveSlotToAnotherListSlot ->  MoveSlotToSlotAnotherInventory
* 
* 2- MoveSlotToSlotAnotherInventory메서드에서 타 인벤토리 간 이동 조건추가
* 탭일치(옮긴 것 보여주기 위함) + 종류일치(해당아이템 보관가능 해야하기 때문)
* 
* 
* <v3.1 - 2024_0129_최원준>
* 1- MoveSlotToSlotSameInventory 메서드 내부 인덱스의 결정을
* InventoryInfo의 IsShareAll(전체 슬롯 공유)옵션에 따라 달라지도록 설정
* 
* 
* <v3.3 - 2024_0223_최원준>
* 1- MoveSlotToSlotAnotherInventory메서드에서 타 인벤토리 드랍 시 
* 이전 인벤토리에서 RemoveItem할 때 두번째 인자로 isUnregister옵션을 활성화시켜서 2d상태 그대로 목록에서 제거해주었음.
* => 이유는 월드상태로 제거하게 되면 DimensionShift가 일어나게되고 월드상태가 된 후 다시 AddItem에서 DimensionShift로 2D상태를 회복하게 되는데
* 이 때 아이템의 캔버스그룹이 ItemSelect스크립트에서 셀렉팅 시 꺼놓은 상태가 Add됨으로서 다시 회복되버리기 때문에 드랍 시 재 셀렉팅이 일어나게됨.
* 
* 2- MoveSlotToSlotAnotherInventory메서드에서 스왑 시 상태창이 표시되고 남아있는 문제가 있어서 종료하는 코드를 추가
* 
* <3.4 - 2024_0223_최원준>
* 1- OnItemSlotDrop메서드에서 이동할 인벤토리의 퀵슬롯드롭 조건 검사문을 추가
* => 원래는 ItemSelect에서 두번 검사를 따로하는 형태로 되어있었으나, 
* 아이템 스왑시에도 본인 인벤토리에 스왑할 아이템이 퀵슬롯드롭요건에 맞는지 검사해야 하기 때문
* 
* 2- OnItemSlotDrop메서드의 매개변수 callerSlotTr를 nextDropSlotTr로 변경
* 
* 3- 슬롯 드롭이 어떤 아이템에서 일어났는지 알기 위해 디버그메시지에 itemRectTr의 인스턴스ID를 추가
* 
* <v3.5 - 2024_0224_최원준>
* 1- MoveSlotToSlotSameInventory 및 MoveSlotToSlotAnotherInventory메서드의 내부 코드를 더욱 세밀하게 부분 메서드화
* 기존의 로직을 처리하는 부분 메서드 MoveThisItemInSameInventory, SwapNextItemInSameInventory로 나누었으며,
* 반복 처리 메서드 IsAbleToFill, FillSameItemOverlapCount 를 신규로 작성하였음.
* 
* 2- 퀵슬롯드롭요건과 기본슬롯드롭요건을 통합한 메서드로 조건을 검사하고,
* 현재 아이템과 스왑할 아이템 양방향 검사형태로 구현
* 
* 3- 드롭하기전에 드롭할 공간에 아이템이 존재한다면, 상태창을 끄고 상대 아이템 셀렉팅을 막는 코드를 추가
* 또한, 장착중인 아이템은 더미가 검색되기 때문에 실제 아이템정보를 가져올수 있는 메서드를 호출하는 것으로 구현
* 
* 4- 드롭 실패 시 원위치로 돌려주기 전에, 퀵슬롯이었던 경우 추가로 원래자리정보를 초기화해주는 코드호출
* 
* 5- 스왑하지 않고 동일 잡화아이템을 채우는 경우에는 드롭을 성공처리하지만, 이전슬롯드롭을 최신화하지 못하도록 상태를 하나 더 구현
* => 이는 prevDropSlotTr을 조건분기로 해서 동일 인벤토리 슬롯드롭이냐 다른 인벤토리 슬롯드롭이냐가 나눠지기 때문에
* 다른 인벤토리에서 중첩시킨 경우에 이전슬롯드롭 최신화시키게 되면 옮겨진 것으로 판단되어, 다음 슬롯 드롭 시 바뀐인벤토리를 기준으로 슬롯드롭이 되버림. 
* 
* 
* 
*/


public partial class ItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string strItemDropSpace = "ItemDropSpace";  // 슬롯이 가지고 있는 태그    
        
    /// <summary>
    /// 아이템에서 커서를 대는 순간 자동으로 아이템 스테이터스 창을 띄워줍니다.
    /// </summary>
    public void OnPointerEnter( PointerEventData eventData )
    {
        if(statusInteractive==null)
            return;

        statusInteractive.OnItemPointerEnter(this);

        
        // +++ (협업코드_신혜성) 상태창 등장 사운드  
        // SoundManager.instance.UISound(SoundManager.UI.MainTab_Click, ownerTr, 1.0f);
    }

    /// <summary>
    /// 아이템에서 커서를 떼는 순간 자동으로 아이템 스테이터스 창이 사라집니다.
    /// </summary>
    public void OnPointerExit( PointerEventData eventData )
    {
        if(statusInteractive==null)
            return;

        statusInteractive.OnItemPointerExit();
    }



    /// <summary>
    /// 아이템의 슬롯 드롭이 발생할 때 아이템을 이동시키고 정보를 이전하기 위하여 호출해줘야 하는 메서드입니다.<br/>
    /// 동일 혹은 타 인벤토리의 슬롯 간 드랍 발생시 사용합니다.<br/><br/>
    /// 슬롯->같은 인벤토리 슬롯 : 실패하지 않습니다. 기존 아이템이 있다면 위치를 교환합니다.<br/>
    /// 슬롯->다른 인벤토리 슬롯 : 빈자리가 없다면 실패합니다. 기존 아이템이 있다면 실패합니다.<br/>
    /// </summary>
    /// <returns>슬롯 드롭에 성공 시 true를 실패 시 false를 반환합니다.</returns>
    public bool OnItemSlotDrop( Transform nextDropSlotTr )
    {        
        // 호출 인자가 전달되지 않았는지 검사
        if( nextDropSlotTr==null)
            throw new Exception("슬롯의 참조가 전달되지 않았습니다. 올바른 슬롯 드랍이벤트 호출인지 확인하여 주세요.");

        bool isNextSlot = (nextDropSlotTr.tag == strItemDropSpace);
        bool isPrevSlot = (prevDropSlotTr.tag == strItemDropSpace);
        
        // 호출자가 슬롯인지 검사
        if( !isNextSlot )
            throw new Exception("전달인자가 슬롯이 아닙니다. 올바른 슬롯 드랍이벤트 호출인지 확인하여 주세요.");
        // 이전 호출자 정보가 슬롯인지 검사
        else if( !isPrevSlot )
            throw new Exception("월드->슬롯의 드랍이벤트가 발생하였습니다. 올바른 슬롯 드랍이벤트 호출인지 확인하여 주세요.");
                

        // 현재 슬롯 호출자와 이전 드랍이벤트 호출자가 같다면,
        if(nextDropSlotTr==prevDropSlotTr)        
        {   
            print( "transformID: " + itemRectTr.GetInstanceID() + "동일 인벤토리 동일 슬롯 간 드롭 발생");
            return MoveSlotToSlotSameInventory(nextDropSlotTr);            // 동일한 인벤토리의 동일한 슬롯->슬롯으로의 이동
        }
        else
        {
            // 이전 드랍이벤트 호출자와 부모가 같다면(동일한 슬롯 리스트에서의 이동이라면)
            if( nextDropSlotTr.parent == prevDropSlotTr.parent)
            {
                print( "transformID: " + itemRectTr.GetInstanceID() + "동일 인벤토리 타 슬롯 간 드롭 발생");
                return MoveSlotToSlotSameInventory(nextDropSlotTr);       // 같은 인벤토리 타 슬롯 간 이동
            }
            else
            {
                print( "transformID: " + itemRectTr.GetInstanceID() + "타 인벤토리 타 슬롯 간 드롭 발생");
                return MoveSlotToSlotAnotherInventory(nextDropSlotTr);    // 타 인벤토리 슬롯으로의 이동
            }
        }   
    }







    /// <summary>
    /// 아이템을 기존 슬롯에서 다른 슬롯으로 이동시켜 주는 메서드입니다.<br/>
    /// 슬롯의 자식인덱스를 읽어 들여 아이템에 적용시켜 주고, 아이템 오브젝트의 위치를 업데이트 합니다.<br/>
    /// 만약 슬롯에 이미 다른 아이템 오브젝트가 있다면 서로의 인덱스 정보와 위치를 교환합니다.<br/>
    /// </summary>
    /// <returns>다른 인벤토리로의 이동 성공 시 true를 반환, 실패 시 false를 반환</returns>
    private bool MoveSlotToSlotSameInventory(Transform nextSlotTr)
    {
        // 인벤토리 내부 이동에 최종 성공여부를 나타내는 상태 변수로 성공처리 되어져야 합니다. (기본값: 실패)
        bool isSuccess = false;      
                
        // 잡화 아이템을 채우기를 실행했는 지 여부를 기록합니다. (기본값: 채우지 않음)
        bool isFilled = false;
        
        // 현재 아이템이 셀렉팅 이전에 위치했던 슬롯의 Transform 참조값을 구합니다.
        Transform thisItemSlotTr = inventoryInfo.SlotListTr.GetChild(this.SlotIndexActiveTab);

        // 드랍할 슬롯에 담겨있는 바꿀 아이템의 정보를 캐싱합니다.
        ItemInfo swapItemInfo = null;

        // 옮길 슬롯에 아이템이 이미 존재한다면,
        if( nextSlotTr.childCount==1 )
        {
            // 슬롯에 담긴 실제 아이템 정보를 가져옵니다. (퀵슬롯의 경우 더미가 아닌 실제 아이템 정보를 참조) 
            swapItemInfo = inventoryInfo.GetSlotItemInfo( nextSlotTr );
                        
            // 상대 아이템의 셀렉팅을 일시적으로 막습니다.
            swapItemInfo.ItemSelect.SelectPreventTemporary();
                        
            // 상대 아이템의 상태창을 종료합니다. 
            swapItemInfo.statusInteractive.OnItemPointerExit();
        }



        // 현재 아이템의 다음 인벤토리의 슬롯으로의 드롭 조건 통과를 검사합니다.
        if( inventoryInfo.IsAbleToSlotDrop(this, nextSlotTr) )
        {
            // 슬롯이 비어있다면,
            if( nextSlotTr.childCount==0 )
            {
                // 현재 인벤토리가 퀵슬롯이라면 현재 아이템에 대한 퀵슬롯의 정보를 최신화 합니다.
                if(inventoryInfo.IsQuickSlot)
                    inventoryInfo.ThisQuickSlot.OnQuickSlotDrop(this, nextSlotTr);

                // 다음 슬롯으로 이동처리 합니다.
                MoveThisItemInSameInventory( nextSlotTr );

                // 성공처리 합니다.
                isSuccess=true;
            }


            // 슬롯이 비어있지 않다면,
            else if( nextSlotTr.childCount==1 )
            {                     
                // 스왑 할 아이템이 현재 아이템의 인벤토리의 슬롯 자리로 드랍 가능 여부를 판단합니다.
                if( inventoryInfo.IsAbleToSlotDrop( swapItemInfo, thisItemSlotTr ) )
                {
                    // 서로 중첩가능한 잡화아이템이라면,
                    if( this.IsAbleToFill(swapItemInfo) )
                    {
                        // 동일한 아이템을 중첩시킨 후 남은 수량이 존재하면, 셀렉팅을 다시 시작합니다. 
                        if( FillSameItemOverlapCount( swapItemInfo )!=0 )
                            this.itemSelect.ReselectUntilDeselect();

                        // 채움상태를 활성화 합니다.
                        isFilled = true;

                        // 성공처리 합니다.
                        isSuccess=true;
                    }

                    // 서로 중첩가능한 잡화아이템이 아니라면, 
                    else
                    {
                        // 현재 인벤토리가 퀵슬롯이라면 스왑할 아이템에 대한 퀵슬롯의 정보를 최신화 합니다.
                        if( inventoryInfo.IsQuickSlot )
                        {
                            inventoryInfo.ThisQuickSlot.OnQuickSlotDrop( swapItemInfo, thisItemSlotTr );
                            inventoryInfo.ThisQuickSlot.OnQuickSlotDrop( this, nextSlotTr );
                        }

                        // 다음 아이템과 현재 아이템의 스왑을 처리합니다.
                        SwapNextItemInSameInventory( swapItemInfo, nextSlotTr );

                        //성공처리 합니다.
                        isSuccess=true;
                    }
                }

            }

            // 슬롯에 자식이 2개 이상인 경우 - 예외 처리
            else  
                throw new Exception( "슬롯에 자식이 2개 이상 겹쳐있습니다. 확인하여 주세요." );
        }


        // 아이템 이동에 성공한 경우
        if( isSuccess )
        {                        
            // 채우지 않은 경우에만, 이전 슬롯을 최신화합니다.
            if( !isFilled )
                prevDropSlotTr=nextSlotTr;
        }
        // 아이템 이동에 실패한 경우
        else
        {   
            // 현재 아이템이 담긴 원래 인벤토리가 퀵슬롯인 경우 현재 아이템에 대한 자리 정보를 다시 초기화 합니다. 
            if(inventoryInfo.IsQuickSlot)
                inventoryInfo.ThisQuickSlot.OnQuickSlotDrop(this, thisItemSlotTr);

            // 아이템을 원위치로 되돌립니다.
            UpdatePositionInfo();  
        }
        
        // 성공여부를 반환합니다.
        return isSuccess;          
    }


    /// <summary>
    /// 현재 아이템을 동일 인벤토리 내의 다른 슬롯으로 이동처리해주는 메서드입니다.<br/>
    /// 이동할 슬롯의 Transform 참조 값을 전달해야 합니다.
    /// </summary>
    private void MoveThisItemInSameInventory( Transform nextSlotTr )
    {
        // 해당 슬롯의 자식번호를 참조하여 다음 슬롯 인덱스를 구합니다.
        int nextSlotIdx = nextSlotTr.GetSiblingIndex();

        // 전체 슬롯 공유 옵션이 활성화 되어있는 경우의 인덱스 설정
        if( inventoryInfo.IsShareAll )
        {
            // (퀘스트 아이템을 제외하고) 전체 슬롯인덱스와 개별 슬롯 인덱스를 동일하게 맞춰줍니다.
            if( item.Type!=ItemType.Quest )
                item.SlotIndexAll=nextSlotIdx;
            item.SlotIndexEach=nextSlotIdx;
        }
        // 전체 슬롯 공유 옵션이 비활성화 되어있는 경우의 인덱스 설정
        else
        {
            // 활성화 중인 탭에 따른 이 아이템의 슬롯 인덱스 정보를 수정합니다.
            if( isActiveTabAll )
                item.SlotIndexAll=nextSlotIdx;
            else
                item.SlotIndexEach=nextSlotIdx;
        }

        // 해당 정보로 위치정보를 업데이트 합니다.
        UpdatePositionInfo();
    }


    /// <summary>
    /// 동일 인벤토리 내부에 있는 아이템을 서로 교환 처리하는 로직의 메서드입니다.<br/>
    /// 교환을 진행할 아이템 정보와 교환할 슬롯 정보를 전달해야 합니다.
    /// </summary>
    private void SwapNextItemInSameInventory( ItemInfo swapItemInfo, Transform nextSlotTr )
    {
        // 해당 슬롯의 자식번호를 참조하여 다음 슬롯 인덱스를 구합니다.
        int nextSlotIdx = nextSlotTr.GetSiblingIndex();

        // 전체 슬롯 공유 옵션이 활성화 되어있는 경우의 인덱스 설정 
        if( inventoryInfo.IsShareAll )
        {
            // (퀘스트 아이템을 제외하고) 전체 슬롯인덱스와 개별 슬롯 인덱스를 동일하게 맞춰줍니다.
            swapItemInfo.SlotIndexEach=item.SlotIndexEach;
            item.SlotIndexEach=nextSlotIdx;

            if( swapItemInfo.Type!=ItemType.Quest )
            {
                swapItemInfo.SlotIndexAll=item.SlotIndexAll;
                item.SlotIndexAll=nextSlotIdx;
            }
        }
        // 전체 슬롯 공유 옵션이 비활성화 되어있는 경우의 인덱스 설정
        else
        {
            // 활성화 중인 탭에 따른 두 아이템의 인덱스 정보를 수정합니다.
            if( isActiveTabAll )
            {
                swapItemInfo.SlotIndexAll=item.SlotIndexAll;  // 바꿀아이템의 인덱스를 이 아이템의 이전 슬롯의 인덱스로 넣어줍니다.      
                item.SlotIndexAll=nextSlotIdx;                // 이 아이템의 인덱스를 바뀔 슬롯의 위치로 수정합니다.
            }
            else
            {
                swapItemInfo.SlotIndexEach=item.SlotIndexEach;
                item.SlotIndexEach=nextSlotIdx;
            }
        }

        swapItemInfo.UpdatePositionInfo();      // 바꿀 아이템의 위치 정보를 업데이트 합니다.
        UpdatePositionInfo();                   // 이 아이템의 위치 정보를 업데이트 합니다.
    }






    /// <summary>
    /// 아이템을 기존 인벤토리의 슬롯에서 다른 인벤토리의 슬롯으로 이동시켜주는 메서드입니다.<br/>
    /// 해당 인벤토리에 남는 자리가 있는지 옮기기 전에 확인하여 자리가 충분하다면<br/>
    /// 이전의 인벤토리 목록에서 이 아이템을 제거하고, 옮길 인벤토리로 모든 정보를 최신화하여 줍니다.<br/>
    /// 이는 버튼 등으로 아이템을 옮겨야 할 경우가 있기 때문입니다.<br/>
    /// </summary>
    /// <returns>새로운 인벤토리 슬롯에 남는 자리가 있는경우 true를, 남는자리가 없거나 해당 슬롯에 기존의 아이템이 있다면 false를 반환</returns>
    private bool MoveSlotToSlotAnotherInventory(Transform nextSlotTr)
    {        
        // 반환할 드롭 성공 상태를 초기화합니다 (기본값: 실패)
        bool isSucess = false;
                
        // 잡화 아이템을 채우기를 실행했는 지 여부를 기록합니다. (기본값: 채우지 않음)
        bool isFilled = false;

        // 현재 아이템이 셀렉팅 이전에 위치했던 슬롯의 Transform 참조값을 캐싱합니다.
        Transform thisItemSlotTr = inventoryInfo.SlotListTr.GetChild( this.SlotIndexActiveTab );

        // 인자로 전달한 슬롯의 계층정보를 기반으로 다음 인벤토리 참조 값을 캐싱합니다.
        Transform nextInventoryTr = nextSlotTr.parent.parent.parent.parent;
        InventoryInfo nextInventoryInfo = nextInventoryTr.GetComponent<InventoryInfo>();

        // 드랍할 슬롯에 담겨있는 바꿀 아이템의 정보를 캐싱합니다.
        ItemInfo swapItemInfo = null;
        

        // 옮길 슬롯에 아이템이 이미 존재한다면,
        if( nextSlotTr.childCount==1 )
        {
            // 슬롯에 담긴 실제 아이템 정보를 가져옵니다. (퀵슬롯의 경우 더미가 아닌 실제 아이템 정보를 참조) 
            swapItemInfo = nextInventoryInfo.GetSlotItemInfo( nextSlotTr );
                                    
            // 상대 아이템의 셀렉팅을 일시적으로 막습니다.
            swapItemInfo.ItemSelect.SelectPreventTemporary();
                        
            // 스왑 할 아이템의 상태창을 종료합니다. 
            swapItemInfo.statusInteractive.OnItemPointerExit();
        }



        // 현재 아이템의 다른 인벤토리로의 드랍 가능 여부를 판단합니다.
        if( nextInventoryInfo.IsAbleToSlotDrop( this, nextSlotTr ) )
        {
            // 다음 인벤토리 슬롯에 아이템이 담겨있지 않다면,
            if( nextSlotTr.childCount==0 )
            {
                // 이전 인벤토리에서 현재 아이템을 2D상태로 제거합니다.
                inventoryInfo.RemoveItem( this, true );
                                
                // 옮길 인벤토리가 퀵슬롯이라면 현재 아이템에 대한 퀵슬롯의 정보를 최신화합니다.
                if( nextInventoryInfo.IsQuickSlot )
                    nextInventoryInfo.ThisQuickSlot.OnQuickSlotDrop( this, nextSlotTr );

                // 현재 아이템을 새로운 인벤토리의 해당 슬롯에 추가합니다.
                nextInventoryInfo.AddItemToSlot( this, nextSlotTr, false );

                // 드롭 상태를 성공으로 설정합니다.
                isSucess=true;
            }

            // 다음 인벤토리 슬롯에 다른 아이템이 담겨있다면,
            else
            {
               
                // 스왑 할 아이템이 현재 아이템의 인벤토리의 슬롯 자리로 드랍 가능 여부를 판단합니다.
                if( inventoryInfo.IsAbleToSlotDrop( swapItemInfo, thisItemSlotTr ) )
                {
                    // 서로 중첩가능한 아이템이라면, 중첩을 진행합니다.
                    if( this.IsAbleToFill( swapItemInfo ) )
                    {
                        // 동일 이름의 아이템을 채우고 남은 수량이 존재한다면, 셀렉팅을 다시 시작해 줍니다.
                        if( FillSameItemOverlapCount( swapItemInfo )!=0 )
                            this.itemSelect.ReselectUntilDeselect();
                        
                        // 채움상태를 활성화합니다.
                        isFilled = true;

                        // 드롭 상태를 성공으로 설정합니다.
                        isSucess=true;

                    }
                    // 서로 중첩가능한 아이템이 아니라면, 스왑을 진행합니다.
                    else
                    {
                        // 현재 인벤토리가 퀵슬롯이라면 스왑할 아이템에 대한 퀵슬롯의 정보를 최신화 합니다.
                        if( inventoryInfo.IsQuickSlot )
                            inventoryInfo.ThisQuickSlot.OnQuickSlotDrop( swapItemInfo, thisItemSlotTr );
                        
                        // 옮길 인벤토리가 퀵슬롯이라면 현재 아이템에 대한 퀵슬롯의 정보를 최신화합니다.
                        if( nextInventoryInfo.IsQuickSlot )
                            nextInventoryInfo.ThisQuickSlot.OnQuickSlotDrop( this, nextSlotTr );


                        // (아이템 추가 시 슬롯정보가 지워지므로) 교환할 아이템이 자리한 슬롯의 활성탭 인덱스와 비활성탭 인덱스를 기록합니다. 
                        int nextSlotIndexActiveTab = swapItemInfo.SlotIndexActiveTab;
                        int nextSlotIndexInactiveTab = swapItemInfo.SlotIndexInactiveTab;

                        // (아이템 제거 시 인벤토리 정보가 지워지므로) 현재 아이템이 담긴 인벤토리를 기록합니다.
                        InventoryInfo rInventoryInfo = this.inventoryInfo;
                        
                        // 양쪽 인벤토리 목록에서 각 아이템을 2D상태 그대로 제거합니다.
                        rInventoryInfo.RemoveItem( this, true );
                        nextInventoryInfo.RemoveItem( swapItemInfo, true );

                        // 양쪽 인벤토리에 서로 아이템을 바꿔서 기존 아이템 자리에 그대로 추가합니다.
                        rInventoryInfo.AddItemToSlot( swapItemInfo, this.SlotIndexActiveTab, this.SlotIndexInactiveTab, false );
                        nextInventoryInfo.AddItemToSlot( this, nextSlotIndexActiveTab, nextSlotIndexInactiveTab, false );

                        // 드롭 상태를 성공으로 설정합니다.
                        isSucess=true;
                    }

                }
            }
        }




        // 아이템 이동에 성공한 경우
        if( isSucess )
        {
            // 채우지 않은 경우에만, 이전 슬롯을 최신화합니다.
            if( !isFilled )
                prevDropSlotTr=nextSlotTr;  
        }
        // 아이템 이동에 실패한 경우
        else
        {            
            // 현재 아이템이 담긴 원래 인벤토리가 퀵슬롯인 경우 현재 아이템에 대한 자리 정보를 다시 초기화 합니다. 
            if(inventoryInfo.IsQuickSlot)
                inventoryInfo.ThisQuickSlot.OnQuickSlotDrop(this, thisItemSlotTr);

            // 원위치로 되돌립니다.
            UpdatePositionInfo();   
        }

        // 성공여부를 반환합니다.
        return isSucess;
    }


    /// <summary>
    /// 두 아이템이 중첩 가능한 지 여부를 반환합니다.
    /// </summary>
    /// <returns>두 아이템이 모두 잡화아이템이고, 이름이 동일하다면 true를 반환, 아니라면 false를 반환</returns>
    private bool IsAbleToFill(ItemInfo targetItemInfo)
    {        
        if( this.Type==ItemType.Misc && targetItemInfo.Type==ItemType.Misc
                        && this.Name==targetItemInfo.Name )
            return true;
        else
            return false;
    }



    /// <summary>
    /// 이름이 동일한 잡화아이템의 중첩수량을 채워주는 메서드입니다.<br/>
    /// 현재 아이템의 중첩수량을 감소시켜 인자로 전달한 아이템의 중첩수량에 채워준 후,
    /// 텍스트 정보를 최신화 하고, 파괴여부를 체크합니다.
    /// </summary>
    /// <returns>현재 아이템의 중첩수량을 인자로 전달한 목표 아이템에 채우고 남은 수량을 반환</returns>
    private int FillSameItemOverlapCount( ItemInfo destItemInfo )
    {
        if(destItemInfo==null)
            throw new Exception("인자로 null참조값이 전달되었습니다.");

        ItemMisc srcItemMisc = this.item as ItemMisc;
        ItemMisc destItemMisc = destItemInfo.item as ItemMisc;

        if (srcItemMisc == null || destItemMisc == null )
            throw new Exception("잡화 아이템이 아닙니다.");

        if(srcItemMisc.Name != destItemMisc.Name)
            throw new Exception("이름이 동일하지 않은 아이템은 중첩할 수 없습니다.");


        // 처음 시작 수량을 기록합니다.
        int startCount = srcItemMisc.OverlapCount;

        // 중첩할 아이템의 수량을 최대 허용치까지 채우고 남은 수량을 반환받습니다.
        int lastCount = destItemMisc.AccumulateOverlapCount( startCount );

        // 줄어든 수량만큼 기존 아이템의 수량을 감소시켜 줍니다.
        srcItemMisc.AccumulateOverlapCount( lastCount-startCount );


        /*** 채우고 난 이후 수량변동 정보 업데이트 ***/

        // 중첩할 아이템의 텍스트 정보를 업데이트 합니다. 
        destItemInfo.UpdateTextInfo();

        // 셀렉팅 중인 아이템의 텍스트 정보를 업데이트하고 파괴여부를 체크합니다.
        this.UpdateTextInfo(); 
        this.CheckDestroyInfo();

        return lastCount;
    }



}
