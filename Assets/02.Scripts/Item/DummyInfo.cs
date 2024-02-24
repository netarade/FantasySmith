using InventoryManagement;
using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * [작업사항]
 * 
 * <v1.0 - 2024_0130_최원준>
 * 1- 스크립트 작성 목적
 * 아이템 장착시 Item2dTr하위에 있는 더미 오브젝트가 계층분리해서 대신 자리잡고 있어야 하며,
 * 기존 아이템의 정보를 가지고 있어서 외부에 장착되어있는 오브젝트 대신 아이템 정보를 알려주는 역할을 하게됨
 * 
 * 2- 아이템 정보가 입력될 때 더미 아이템의 UpdateInfo를 호출해주도록 구현
 * 
 * 3- 더미아이템의 UpdatePositionInfo가 기존 아이템의 UpdatePositionInfo와 비슷한 역할을 하도록 구현
 * 
 * 3- 더미아이템의 2D기능을 중단하는 SwitchAppearAs2D메서드 구현
 * 
 * 4- 더미아이템도 포인터팅으로 기존 아이템 정보를 바탕으로 상태창이 열고 닫히도록 콜백 메서드 구현
 * 
 * <v1.1 - 2024_0131_최원준>
 * (이슈) 더미 이미지가 켜져 있으면 이미지가 중첩되어 커져버리므로 시작 시 끈 상태로 있어야 하며,
 * 장착 시점에만 켜줘야 한다.
 * 
 * <v1.2 - 2024_0214_최원준>
 * 1- 더미 오브젝트에 버튼을 추가하고 초기화하는 구문 추가
 * 아이템 셀렉팅을 판단하여 장착을 해제하고 원래의 이미지가 셀렉팅이 일어나도록 하기 위함
 * 
 * 2- 메서드명 UpdateInfo를 InitItemInfo로 변경, 주석 입력
 * 
 * 3- OnItemEquip 신규 메서드를 작성
 * 아이템 착용이 일어나는 순간 더미 컴포넌트 활성화 비활성화 및 위치정보를 업데이트 해주는 통합 메서드
 * 
 * <v1.3 - 2024_0222_최원준>
 * 1- SwitchAppearAs2D메서드의 매개변수명 isWorldPositioned를 isOperateAs2d로 변경 및 인자 전달도 반대로 수정
 * (아이템의 3D상태 뿐만아니라 인벤토리 온오프에따라 수동으로 조절하는 경우가 있으므로)
 * 
 * 2- 메서드 호출 시 dummyBtn.interactable속성도 같이 조절하도록 변경
 *
 * 3- SwitchAppearAs2D메서드명을 OperateSwitchAs2d로 변경
 * 
 * <v1.4 - 2024_0223_최원준>
 * 1- DummyBtn 읽기전용 프로퍼티 추가
 * ItemSelect스크립트에서 일시적으로 착용중인 아이템 더미의 Interactable속성을 해제하기 위함 
 * 
 */

public class DummyInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image dummyImg;                 // 현재 아이템이 장착 중일 때 보여지는 더미 이미지입니다.
    RectTransform dummyRectTr;      // 현재 아이템이 장착 중일 때 보여지는 더미 오브젝트의 RectTransform 참조값입니다.   
    ItemInfo equipItemInfo;         // 더미가 대신할 원본 아이템 정보입니다
    Button dummyBtn;                // 더미 오브젝트를 셀렉팅할 수 있는 버튼



    /// <summary>
    /// 더미 이미지의 RectTransform을 반환합니다.
    /// </summary>
    public RectTransform DummyRectTr { get { return dummyRectTr; } }
     

    /// <summary>
    /// 현재 장착 중인 아이템 정보를 반환
    /// </summary>
    public ItemInfo EquipItemInfo { get { return equipItemInfo; } }
    

    /// <summary>
    /// 더미 이미지 컴포넌트 참조 값을 반환합니다.
    /// </summary>
    public Image DummyImg {  get { return dummyImg; } }

    /// <summary>
    /// 더미 버튼 컴포넌트 참조 값을 반환합니다.
    /// </summary>
    public Button DummyBtn { get { return dummyBtn; } }

    private void Awake()
    {
        dummyRectTr = GetComponent<RectTransform>();
        dummyImg = GetComponent<Image>();
        dummyBtn = GetComponent<Button>();

        // 시작 시 더미 이미지와 버튼을 비활성화합니다.
        dummyImg.enabled = false;           
        dummyBtn.enabled = false;
    }


    /// <summary>
    /// 아이템이 생성될 때 호출해줘야 할 메서드입니다.<br/>
    /// 더미에 아이템 정보를 전달하여 초기화를 진행합니다.
    /// </summary>
    public void InitItemInfo(ItemInfo itemInfo)
    {
        equipItemInfo = itemInfo;
        dummyImg.sprite = itemInfo.innerSprite;
    }


    /// <summary>
    /// 더미 오브젝트의 2D 기능을 차단하거나 활성화합니다.<br/>
    /// 인자로 2D 동작 여부를 전달받습니다.
    /// </summary>
    public void OperateSwitchAs2d(bool isOperateAs2d)
    {
        dummyImg.raycastTarget = isOperateAs2d;    // 이미지의 레이캐스팅 여부를 전환합니다.
        dummyBtn.interactable = isOperateAs2d;     // 버튼의 상호작용 여부를 전환합니다.
    }


    /// <summary>
    /// 아이템 장착 및 해제 시 호출해야 할 메서드입니다.<br/>
    /// 더미 관련 모든 정보를 업데이트 합니다.
    /// </summary>
    public void OnItemEquip(bool isEquip)
    {
        // 아이템이 착용상태가 되었다면,
        if(isEquip)
        {                        
            DummyImg.enabled = true;    // 더미 이미지를 활성화합니다.
            dummyBtn.enabled = true;    // 더미 버튼을 활성화합니다.
        }

        // 아이템이 착용 해제 상태가 되었다면,
        else
        {
            DummyImg.enabled = false;   // 더미 이미지를 비활성화합니다.
            dummyBtn.enabled = false;   // 더미 버튼을 비활성화합니다.    
        }

        // 더미 오브젝트의 포지션을 업데이트합니다.
        UpdatePositionInfo();
    }




    /// <summary>
    /// 현재 아이템이 슬롯 리스트에 속해있다면 더미 아이템의 위치를 슬롯 인덱스에 맞게 최신화시켜줍니다.<br/>
    /// 슬롯 인덱스가 잘못되어 있다면 다른 위치로 이동 할 수 있습니다.<br/><br/>
    /// ** 아이템이 월드 상에 있거나 슬롯 참조가 안 잡힌 경우 예외를 던집니다. **<br/>
    /// </summary>
    public void UpdatePositionInfo()
    {
        // 슬롯 리스트에 슬롯이 생성되어있지 않다면 하위로직을 실행하지 않습니다.
        if( equipItemInfo.SlotListTr.childCount==0 )
        {
            Debug.Log( "현재 슬롯이 생성되지 않은 상태입니다." );
            return;
        }

        // 아이템의 타입을 받아옵니다.
        ItemType itemType = equipItemInfo.Item.Type;


        /**** 아이템을 탭에 표시 하지 않을 조건 ****/
        // 1. 전체탭의 경우 퀘스트 아이템을 제외하고 표시합니다.
        // 2. 전체탭이 아니라면 현재활성화 탭과 아이템이 속한 탭이 일치해야 표시합니다.

        // 아이템이 장착상태가 아닌 경우 원위치로 돌아갑니다.
        if( !equipItemInfo.IsEquip )
        {
            MoveToItem2dTr();
            return;
        }

        // 현재 활성화탭이 전체 탭인경우
        if(equipItemInfo.CurActiveTab == TabType.All)
        {
            // 퀘스트 아이템이라면, EmptyList로 이동합니다.
            if( itemType==ItemType.Quest )
            {
                MoveToItem2dTr(); // 이 아이템을 빈 리스트로 이동시킵니다.
                return;
            }
        }
        // 아이템이 전체탭이 아닌 경우, 아이템의 탭타입이 현재활성화 탭이 아니라면
        else if( Inventory.ConvertItemTypeToTabType(equipItemInfo.Item.Type) != equipItemInfo.CurActiveTab )
        {
            MoveToItem2dTr();      // 이 아이템을 빈 리스트로 이동시킵니다.
            return;
        }


        MoveToSlot();   // 아이템을 슬롯으로 이동시킵니다.



    }


    
    /// <summary>
    /// 더미 아이템을 원래의 아이템 2d 오브젝트 계층으로 이동시키는 메서드입니다.
    /// </summary>
    /// <exception cref="Exception"></exception>
    private void MoveToItem2dTr()
    {
        if(equipItemInfo.IsWorldPositioned)
            throw new Exception("월드 상태일 때는 슬롯으로 이동할 수 없습니다.");

        dummyRectTr.SetParent(equipItemInfo.Item2dTr, false);
    }

    /// <summary>
    /// 아이템의 인덱스 정보를 참조하여 해당되는 슬롯으로 더미 아이템을 이동시켜주는 메서드입니다.<br/>
    /// 현재 활성화 중인 탭 정보에 따라 이동시켜야 하므로 유의해서 사용해야 합니다.<br/>
    /// </summary>
    private void MoveToSlot()
    {
        if(equipItemInfo.IsWorldPositioned)
            throw new Exception("월드 상태일 때는 슬롯으로 이동할 수 없습니다.");
        if(equipItemInfo.SlotListTr==null)
            throw new Exception("슬롯 참조가 잡혀있지 않습니다. 확인하여 주세요.");
                        
        // 현재 활성화 중인 탭을 기반으로 어떤 인덱스를 참조할지 설정합니다.
        int activeTabSlotIndex = equipItemInfo.SlotIndexActiveTab;
        
        // 아이템의 크기를 슬롯리스트의 cell크기와 동일하게 맞춥니다.(슬롯의 크기와 동일하게 맞춥니다.)
        dummyRectTr.sizeDelta = equipItemInfo.InventoryInfo.CellSize;

        // 아이템의 부모를 해당 슬롯으로 설정합니다.
        dummyRectTr.SetParent( equipItemInfo.SlotListTr.GetChild(activeTabSlotIndex) );  

        // 위치와 회전값을 수정합니다.
        dummyRectTr.localPosition = Vector3.zero;
        dummyRectTr.localRotation = Quaternion.identity;
    }



    /// <summary>
    /// 더미 아이템 포인팅 접근에 따른 콜백 메서드 장착 아이템의 정보를 바탕으로 상태창을 열거나 해제합니다
    /// </summary>
    public void OnPointerEnter( PointerEventData eventData )
    {
        
        if(equipItemInfo.StatusWindowInteractive==null)
            return;

        equipItemInfo.StatusWindowInteractive.OnItemPointerEnter(equipItemInfo);
    }


    
    /// <summary>
    /// 더미 아이템 포인팅 접근에 따른 콜백 메서드 장착 아이템의 정보를 바탕으로 상태창을 열거나 해제합니다
    /// </summary>
    public void OnPointerExit( PointerEventData eventData )
    {
        if(equipItemInfo.StatusWindowInteractive==null)
            return;

        equipItemInfo.StatusWindowInteractive.OnItemPointerExit();
    }
}
