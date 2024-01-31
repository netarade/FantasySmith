using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
 * <v3.1 - 2024_0123_최원준>
 * 1- 아이템 장착 및 해제 시 3D오브젝트 최상위에있는 기본 콜라이더를 비활성화 활성화하도록 변경
 * 
 * <v3.2 - 2024_0124_최원준>
 * 1- Start내부에 playerInventory참조를 잡는 부분을  GetChild(0) 직접참조에서 GetComponentInChildren으로 변경
 * 이유는 인벤토리 내부에 크래프팅 UI창이 공존해야 하는 경우 상태창보다 계층이 앞서야 하므로 
 * 캔버스 하위 인벤토리 오브젝트보다 위로 배치해야하기 때문
 * 
 * <v3.3 - 2024_0125_최원준>
 * 1- Awake문의 saveFileName을 다르게 오버라이딩하는 부분 삭제
 * 부모 인벤토리 스크립트에서 저장파일 이름을 자동으로 설정되게 하였으므로 
 * 
 * 2- Start문에서 서버여부와 ownerId가 플레이어 인벤토리와 틀린지 검사해서 예외처리하는 부분 추가
 * 
 * <v3.4 - 2024_0126_최원준>
 * 1- SlotActivate 및 Deactivate시 itemInfo의 ItemCol을 현 스크립트에서 설정해주던 부분을
 * ItemInfo의 OnItemEquip, OnItemUnequip메서드로 옮김
 * 
 * <v3.5 - 2024_0127_최원준>
 * 1- SlotDeactivate메서드에서 AddItemToSlot메서드의 호출을 해당 아이템의 OnItemUnequip에서 호출하도록 맡김
 * 이유는 이미지 스케일의 변형 문제가 있어서 호출 시점을 한번에 조절하기 위함.
 * 
 * <v3.6 - 2024_0129_최원준>
 * 1- 장착아이템 저장 시 사라지는 이슈가 있어서 OnApplicationQuit 오버라이딩하여 장착중인 아이템을 종료시 해제후 종료하도록 하였음.
 * 
 * <v4.0 - 2024_0130_최원준>
 * 1- OnApplicationQuit 오버라이딩 삭제
 * ItemInfo에서 WorldDrop시 인벤토리에 등록된 상태로 방출할 수 있는 옵션을 만들어 두었기 때문에 따로 아이템 해제를 할 필요가 없음.
 * 
 * 2- 기존의 더미 이미지 관련 속성 및 관련 코드를 모두 삭제하였음. (더미 생성코드도 삭제)
 * 이유는 각 아이템 마다 Item2D 오브젝트 하위에 더미 오브젝트를 가지도록 구조를 변경하였으며, ItemInfo에서 이를 관리할 수 있기 때문.
 * 
 * 3- SlotActivate 및 SlotDeactivate 메서드 간소화
 * 해당 슬롯의 아이템의 OnItemEquip과 OnItemUnEquip메서드를 호출해주기만 하면됨.
 * 
 * <v4.1 - 2024_0130_최원준>
 * 1- 퀵슬롯에 겹쳐져 있는 배경아이콘 이미지 배열 backgroundIcon 변수를 추가하고,
 * 슬롯 갯수만큼 초기화 및 장착 및 해제시 이미지 OnOff스위칭이 이루어질 수 있도록 함. 로드시에도 변경  
 * 
 * 
 */






public class QuickSlot : InventoryInfo
{
    
    [Header("슬롯별 고정 무기 종류")]
    [SerializeField] WeaponType[] equipWeaponType;  // 고정 무기 슬롯

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



    InventoryInfo playerInventory;  // 퀵슬롯과 연결되어 서버가 되는 플레이어의 인벤토리

    Image[] backgroundIcon;         // 퀵슬롯에 겹쳐져 있는 배경아이콘



    protected override void Awake()
    {
        base.Awake();

        playerInventory = GetComponent<InventoryInfo>();

        if(ownerId != playerInventory.OwnerId)
            throw new Exception("같은 계층의 인벤토리 소유자 식별번호는 동일해야합니다.");

        if(isServer)
            throw new Exception("중심 인벤토리가 될 수 없습니다.");

    }

    protected override void Start()
    {
        base.Start();



        if( initializer.dicTypes.Length!=1 )
            throw new Exception( "하나의 종류 딕셔너리만 선택가능합니다." );

        if( initializer.dicTypes[0].itemType!=ItemType.Weapon )
            throw new Exception( "전용 퀵슬롯이 무기 종류가 아닙니다." );



        // 슬롯 길이 설정 (딕셔너리의 슬롯제한 수)
        slotLen=initializer.dicTypes[0].slotLimit;

        // 배열 사용 전 초기화        
        isItemPlaced=new bool[slotLen];
        slotItemInfo=new ItemInfo[slotLen];
        backgroundIcon = new Image[slotLen];


        for(int i=0; i<slotLen; i++)
            backgroundIcon[i] = inventoryTr.parent.GetChild(3).GetChild(0).GetChild(i).GetComponent<Image>();
        

        // 아이템 착용 상태 초기화
        isItemEquipped=false;

        // 그래픽 레이캐스팅이 가능하도록 플레이어 인벤토리와 연동상태로 시작합니다.
        playerInventory=inventoryTr.parent.GetComponentInChildren<InventoryInfo>();
        this.RegisterInventoryLink( playerInventory );

        // 창을 항상 열어둡니다.
        InitOpenState( true );



        // 아이템이 로드시 슬롯에 아이템이 존재하는지 확인
        for( int i = 0; i<slotLen; i++ )
        {
            if( slotListTr.GetChild( i ).childCount>0 )
            {
                isItemPlaced[i]=true;
                backgroundIcon[i].enabled = false; // 배경아이콘 이미지를 비활성화합니다.
                slotItemInfo[i] = slotListTr.GetChild( i ).GetChild( 0 ).GetComponent<ItemInfo>();

                if( slotItemInfo[i]==null )
                {
                    slotItemInfo[i]=slotListTr.GetChild( i ).GetChild( 0 ).GetComponent<DummyInfo>().EquipItemInfo;

                    // 더미 정보가 있는 경우 
                    if( slotItemInfo[i]!=null )
                    {
                        isItemEquipped = true;      // 아이템을 착용 상태로 설정
                        equipSlotIndex = i;         // 착용 아이템 슬롯 인덱스를 해당 인덱스로 설정
                    }
                }            



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

        // 장착할 계층 정보를 전달하여 해당 슬롯의 아이템을 장착합니다.
        slotItemInfo[slotIndex].OnItemEquip();
        

        // 장착 슬롯넘버로 설정합니다.
        equipSlotIndex = slotIndex;
                
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
        
        // 아이템의 장착을 해제합니다.
        slotItemInfo[equipSlotIndex].OnItemUnequip();


        // 아이템 장착 상태를 해제
        isItemEquipped = false;

        return true;
    }






    



    /// <summary>
    /// 슬롯 드롭이벤트가 일어날 때 드롭을 발생시키는 쪽에서 추가적으로 호출해줘야 할 메서드이며, 
    /// 드롭 가능여부를 반환합니다.<br/>
    /// 드롭이 일어나는 아이템과 슬롯의 정보를 전달해야 합니다.
    /// </summary>
    /// <returns>퀵슬롯에 드롭이 가능하면 true를, 불가능하면 false를 반환</returns>
    public bool OnQuickSlotDrop( ItemInfo droppedItemInfo, Transform slotTr )
    {
        int slotIndex = slotTr.GetSiblingIndex();
                
        // 슬롯의 인덱스가 고정무기 배열의 마지막 인덱스를 초과한다면 실패조건을 반환합니다.
        if(slotIndex >= equipWeaponType.Length)
            return false;

        // 드롭할 아이템의 무기타입이 일치하지 않으면 실패조건을 반환합니다.
        if(droppedItemInfo.WeaponType !=equipWeaponType[slotIndex] )
            return false;

        
        // 배경아이콘 이미지를 비활성화 합니다.
        backgroundIcon[slotIndex].enabled = false;

        // 슬롯 자리상태 활성화
        isItemPlaced[slotIndex] = true;

        // 슬롯 아이템 정보 활성화
        slotItemInfo[slotIndex] = droppedItemInfo;

        // 성공을 반환합니다.
        return true;
    }


    /// <summary>
    /// 퀵 슬롯에서 아이템이 셀렉팅이 일어날 때 호출해줘야하는 메서드입니다.<br/>
    /// 배경 아이콘을 다시 원상 복귀해주는 기능을 가지고 있습니다.<br/>
    /// 아이템 정보를 전달해야 합니다.
    /// </summary>
    public void OnQuickSlotSelect(ItemInfo itemInfo)
    {        
        // 배경아이콘 이미지를 활성화 합니다.
        backgroundIcon[itemInfo.SlotIndexTab].enabled = true;
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


