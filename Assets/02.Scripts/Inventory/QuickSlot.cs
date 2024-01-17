using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/*
 * [작업 사항]  
 * <v1.0 - 2024_0114_최원준>
 * 1- 고정 슬롯을 사용자가 지정할 수 있도록 기능 설계 진행
 * 
 * 2- 특정 슬롯을 선택했을 때의 호출할 메서드 구현 (SlotActivate메서드)
 * 
 * 3- 슬롯을 착용 시 임시 이미지를 보여줄 더미 오브젝트와 이미지 변수 설정.
 * 
 * 4- 슬롯에 관한 정보를 알려주는 slotItemInfo배열, slotLen, isItemPlaced배열, isItemEquipped배열 선언후 초기화
 * 
 * <v1.1 - 2024_0114_최원준>
 * 1- SlotActivate메서드 반환 조건추가 (아이템 장착이 안되있거나, 이미 착용중이라면)
 * 
 * 2- SlotActivate메서드에서 
 * (아이템 장착 시 AddItem이 자동으로 이루어진 상태에서) 착용 시 RemoveItem을 하여 플레이어의 Equip장소로 보내면서
 * 더미를 emptyList에서 퀵슬롯으로 보내어 해당 아이템의 2D 이미지를 보여주는 방식으로 구현
 * 
 * 3- SlotActivate메서드에서 Hand클릭 시 Uequip이 이루어지면서 더미를 다시 emptyList로 보내고,
 * 플레이어 Equip장소에서 Quick슬롯으로 AddItem시키도록 구현
 * 
 * 
 * [이슈]
 * 슬롯드롭 튜닝 - isItemPlaced true 설정
 * 
 * 
 * <v2.0 - 2024_0117_최원준>
 * 1- EquipmentTransform클래스 헤더 어트리뷰트 설정
 * 2- 변수명 weaponTypes에서 equipWeaponType로 변경
 * 3- Start문에서 더미를 각 슬롯에 놓아두던 것을 emptyList에 놓아두는 것으로 변경
 * 이유는 아이템을 장착할 때 더미가 슬롯에 자리해야하기 때문
 * 
 * <v2.1 - 2024_0117_최원준>
 * 1- 더미를 모두 생성하는 방식에서 하나만 생성해놓고 나머지는 복제하는 방식으로 변경(스크립트 붙여진 더미를 만들기 위해)
 * 
 * 2- isItemEquipped배열을 삭제하고 변수 하나만 설정
 * (이유는 현 시점에서는 슬롯 6개의 아이템 중에서 단 1개만 장착가능하도록 할것이므로)
 * 
 * 3- 어떤 슬롯의 아이템이 장착되었는지를 나타내는 변수 equpSlotIndex추가
 * 
 * 4- 메서드명 OnSlotDropped를 OnQuickSlotDrop으로 변경, 매개변수를 슬롯인덱스에서 슬롯 트랜스폼으로 변경
 * 
 * 5- SlotActivate에 isEquip 및 equipSlotIndex에 대한 검사 조건 추가
 * 매개변수 slotNo를 내부에서 slotIndex로 변환하는 코드 추가
 * 
 * 6- Start문에서 기존 퀵 로드가 이루어지고 난 이후 isItemPlace[]와 slotItemInfo[]를 초기화 하도록 수정
 * 
 * 7- SlotActivate에서 slotItemInfo를 따로 구하던 점 삭제 (퀵슬롯 드랍 및 로드시 자동으로 지정하므로 새롭게 읽어들일 필요가 없음)
 * 
 * 8- equipWeaponType.Length != initializer.dicTypes.Length 검사문 주석처리
 * (무기타입은 세부타입이기에 하나의 딕셔너리에 여러 고정무기가 들어가기 때문)
 * 
 * (구현예정)
 * 장착시 slotIndex를 -1로 만들고, 로드 시 slotIndex가 -1일 때 다시 장착하게 만들기
 * (장착할 아이템이 하나밖에 없으므로 가능하지만 추후에 여러 아이템 장착시 해당 파츠에 대한 장착정보를 따로 저장해야함)
 * 로드 시 isEquipped 정보를 true로 만들어주기
 * 
 * <v3.0 - 2024_0118_최원준>
 * 1- slotLen을 equipWeaponType의 길이에서 initializer.dicTypes[0]의 길이로 변경
 * 슬롯의 갯수에 정확히 맞춰서해야 IsItemPlaced[]의 각 상태를 슬롯별로 설정할 수 있음
 * 
 * 2- dummyTr을 dummyRectTr로 변경, cell사이즈를 변경하기 위함. 
 * (이미지 사이즈를 슬롯의 셀사이즈에 맞게 축소시켜야 하므로,)
 * 
 */


[Serializable]
public class EquipmentTransform
{
    [Header("2-도끼")]
    public Transform axeTr;

    [Header("3-곡괭이")]
    public Transform pickaxTr;
    
    [Header("4-창")]
    public Transform spearTr;
        
    [Header("5-활")]
    public Transform bowTr;        
}




public class QuickSlot : InventoryInfo
{
    
    [Header("슬롯별 고정 무기 종류")]
    [SerializeField] WeaponType[] equipWeaponType;  // 고정 무기 슬롯

    
    [Header("아이템을 장착할 위치정보")]
    [SerializeField] EquipmentTransform equipTr;    // 장비를 장착할 트랜스폼 정보


    RectTransform[] dummyRectTr;    // 더미용 오브젝트의 RectTransform
    Image[] dummyImg;               // 더미용 이미지
    ItemInfo[] slotItemInfo;        // 슬롯에 자리한 아이템의 정보

    /// <summary>
    /// 슬롯의 길이를 반환합니다.
    /// </summary>
    public int slotLen;

    /// <summary>
    /// 아이템이 해당 인덱스의 슬롯에 자리했는지 여부를 반환합니다.
    /// </summary>
    bool[] isItemPlaced;

    bool isItemEquipped;
    int equipSlotIndex;


    /// <summary>
    /// 해당 슬롯의 아이템이 장착상태 인지를 반환합니다.
    /// </summary>
    public bool IsItemEquipped { get { return isItemEquipped; } }



    InventoryInfo playerInventory;  // 플레이어 인벤토리




    protected override void Awake()
    {
        base.Awake();

        saveFileName = inventoryTr.parent.parent.name + "_QuickSlot";
    }

    protected override void Start()
    {
        base.Start();

        if( initializer.dicTypes.Length != 1 )
            throw new Exception("하나의 종류 딕셔너리만 선택가능합니다.");

        if( initializer.dicTypes[0].itemType != ItemType.Weapon )
            throw new Exception("전용 퀵슬롯이 무기 종류가 아닙니다.");
        
        //if( equipWeaponType.Length != initializer.dicTypes.Length )
        //    throw new Exception("인스펙터 창에서 고정 무기를 모두 지정하지 않았습니다.");
        
        
        // 슬롯 길이 설정 (딕셔너리의 슬롯제한 수)
        slotLen = initializer.dicTypes[0].slotLimit;

        // 설정한 길이만큼 배열을 생성합니다.
        dummyRectTr = new RectTransform[slotLen];
        dummyImg = new Image[slotLen];
        isItemPlaced = new bool[slotLen];
        slotItemInfo = new ItemInfo[slotLen];


        // 미리 만들어진 더미 하나의 Transform을 참조합니다.
        dummyRectTr[0] = emptyListTr.GetChild(0).GetComponent<RectTransform>();

        for( int i = 0; i<slotLen; i++ )
        {
            // 더미오브젝트를 복제하여 emptyList에 배치하고, 참조값을 저장합니다.
            dummyRectTr[i] = Instantiate( dummyRectTr[0].gameObject, emptyListTr ).GetComponent<RectTransform>();            
            dummyImg[i] = dummyRectTr[i].GetComponent<Image>();            
        }       
        
        // 아이템 착용 상태 초기화
        isItemEquipped = false;

        // 그래픽 레이캐스팅이 가능하도록 플레이어 인벤토리와 연동상태로 시작합니다.
        playerInventory = inventoryTr.parent.GetChild(0).GetComponent<InventoryInfo>();
        this.RegisterInventoryLink(playerInventory);

        // 창을 항상 열어둡니다.
        InitOpenState(true);


        // 아이템이 로드시 슬롯에 아이템이 존재하는지 확인
        for( int i = 0; i<slotLen; i++ )
        {
            if( slotListTr.GetChild( i ).childCount>0 )
            {
                isItemPlaced[i]=true;
                slotItemInfo[i]=slotListTr.GetChild( i ).GetChild( 0 ).GetComponent<ItemInfo>();
            }
        }
    }






    /// <summary>
    /// 슬롯에 담긴 장비를 착용하고 해제합니다.<br/>
    /// 아이템 착용시 - 아이템이 슬롯에 자리하고 있지 않거나, 이미 착용하고 있거나,<br/>
    /// 아이템 착용해제시 - 착용한 상태가 아니거나 등의 이유로 실패가 반환될 수 있습니다.<br/><br/>
    /// 현재 1~5번까지의 동작이 맵핑 되어 있으며, 1은 착용해제, 나머지는 착용동작을 하게됩니다.<br/>
    /// *** 맵핑되어있는 값이외의 입력이 들어오면 예외를 던집니다. ***
    /// </summary>
    /// <returns>아이템 착용 및 착용 해제 성공 시 true를, 실패 시 false를 반환</returns>
    public bool ItemEquipSwitch(int keyPadNum)
    {
        if(keyPadNum<=0 || keyPadNum>=6 )
            throw new Exception("슬롯 입력값 맵핑이 되어있지 않습니다.");

        if(keyPadNum==1)
            return SlotDeactivate();
        else
            return SlotActivate(keyPadNum-1);
    }





    /// <summary>
    /// 슬롯을 작동할 때 호출해줘야할 메서드입니다.<br/>
    /// 장비를 착용시켜줌과 동시에 더미 이미지를 슬롯에 보여줘서 
    /// 기존 슬롯에 아이템이 그대로 있는 것 처럼 보이게 합니다.<br/>
    /// </summary>
    /// <returns>아이템 착용 성공 시 true, 실패 시 false를 반환</returns>
    private bool SlotActivate(int slotIndex)
    {
        // 아이템이 자리하지 않았거나, 동일한 아이템을 장착중이라면 더 이상 실행하지 않습니다.
        if( !isItemPlaced[slotIndex] )
            return false;
        
        // 아이템이 착용중인 상태라면,
        if( isItemEquipped )
        {
            // 동일한 아이템을 착용중이라면 작동하지 않습니다
            if(equipSlotIndex == slotIndex)
                return false;
            
            // 다른 아이템을 착용중이라면 장착아이템을 해제합니다.
            SlotDeactivate();
        }

        // 아이템을 장착할 계층 정보를 받아옵니다.
        Transform equipTr = GetEquipTr(slotItemInfo[slotIndex]);

        // 아이템을 장착합니다.
        slotItemInfo[slotIndex].OnItemEquip(equipTr);

        // 장착 슬롯넘버로 설정합니다.
        equipSlotIndex = slotIndex;


        
        // 더미 이미지를 설정합니다.
        dummyImg[slotIndex].sprite = slotItemInfo[slotIndex].innerSprite;

        // 더미 오브젝트를 해당 슬롯으로 보냅니다.
        dummyRectTr[slotIndex].SetParent( slotListTr.GetChild(slotIndex), false );

        // 더미오브젝트의 크기를 슬롯리스트의 cell크기와 동일하게 맞춥니다.(슬롯의 크기와 동일하게 맞춥니다.)
        dummyRectTr[slotIndex].sizeDelta = slotListTr.GetComponent<GridLayoutGroup>().cellSize;

        
        // 아이템 장착상태 활성화
        isItemEquipped = true;
                
        return true;
    }



    /// <summary>
    /// 슬롯을 작동해제할 때 호출해줘야할 메서드입니다.<br/>
    /// 슬롯에 보여지고 있던 더미 이미지를 빈리스트로 보낸 후,
    /// 착용되어있던 장비를 해제하여 슬롯에 놓아주는 역할을 합니다.<br/>
    /// </summary>
    /// <returns>아이템 착용 해제 성공 시 true, 실패 시 false를 반환</returns>
    private bool SlotDeactivate()
    {
        // 아이템이 장착중이지 않은 상태라면 실행하지 않습니다.
        if( !isItemEquipped )
            return false;
        
        // 자리한 더미를 다시 빈리스트로 보냅니다
        dummyRectTr[equipSlotIndex].SetParent(emptyListTr, false);

        // 장착한 아이템을 지정 슬롯에 다시 추가합니다.
        AddItemToSlot( slotItemInfo[equipSlotIndex], equipSlotIndex, interactive.IsActiveTabAll );
                
        // 아이템 장착상태 해제
        isItemEquipped = false;

        return true;
    }






    public Transform GetEquipTr(ItemInfo itemInfo)
    { 
        if(itemInfo == null)
            throw new Exception("아이템 정보가 전달되지 않았습니다.");

        ItemEquip itemEquip = itemInfo.Item as ItemEquip;
        if( itemEquip==null )
            throw new Exception("착용 가능한 장비가 아닙니다.");


        ItemType itemType = itemInfo.Item.Type;

        if( itemType==ItemType.Weapon )
        {
            WeaponType weaponType = ( (ItemWeapon)itemInfo.Item ).WeaponType;
            
            switch(weaponType)
            {
                case WeaponType.Axe:
                    return equipTr.axeTr;

                case WeaponType.Pickax:
                    return equipTr.pickaxTr;

                case WeaponType.Spear:
                    return equipTr.spearTr;

                case WeaponType.Bow:
                    return equipTr.bowTr;                    
            }
        }
        throw new Exception("착용위치 정보가 맵핑되지 않았습니다.");
    }




    /// <summary>
    /// 슬롯 드롭이벤트가 일어날 때 드롭을 발생시키는 쪽에서 추가적으로 호출해줘야 할 메서드입니다.<br/>
    /// 드롭이 일어나는 아이템과 슬롯의 정보를 전달해야 합니다.
    /// </summary>
    public void OnQuickSlotDrop( ItemInfo droppedItemInfo, Transform slotTr )
    {
        int slotIndex = slotTr.GetSiblingIndex();

        // 슬롯 자리상태 활성화
        isItemPlaced[slotIndex] = true;

        // 슬롯 아이템 정보 활성화
        slotItemInfo[slotIndex] = droppedItemInfo;
    }

    /// <summary>
    /// 더미 아이템이 셀렉트 될 때 더미 쪽에서 호출해줘야 할 메서드입니다.<br/>
    /// 드롭이 일어나는 슬롯정보를 전달해줘야 합니다.
    /// </summary>
    public void OnDummySelected(Transform slotTr)
    {       
        int slotIndex = slotTr.GetSiblingIndex();

        // 아이템이 장착중이지 않거나, 장착슬롯의No가 일치하지 않으면 종료합니다.
        if( !isItemEquipped || equipSlotIndex!=slotIndex )
            throw new Exception("더미를 선택할 수 없는 상태입니다.");

        // 아이템 장착을 해제합니다.
        SlotDeactivate();

        // 슬롯아이템 정보 해제
        slotItemInfo[slotIndex] = null;
    }




}


