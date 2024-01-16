using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
 * 
 *  
 */


[Serializable]
public class EquipmentTransform
{
    [Header("아이템을 장착할 위치정보")]

    [Header("도끼")]
    public Transform axeTr;

    [Header("둔기")]
    public Transform bluntTr;
    
    [Header("창")]
    public Transform spearTr;
    
    [Header("검")]
    public Transform swordTr;
    
    [Header("활")]
    public Transform bowTr;        
}




public class QuickSlot : InventoryInfo
{
    
    [SerializeField] WeaponType[] equipWeaponType;  // 고정 무기 슬롯
    [SerializeField] EquipmentTransform equipTr;    // 장비를 장착할 트랜스폼 정보


    Transform[] dummyTr;        // 더미용 오브젝트의 Transform
    Image[] dummyImg;           // 더미용 이미지
    ItemInfo[] slotItemInfo;    // 슬롯에 자리한 아이템의 정보

    /// <summary>
    /// 슬롯의 길이를 반환합니다.
    /// </summary>
    public int slotLen;

    /// <summary>
    /// 아이템이 해당 인덱스의 슬롯에 자리했는지 여부를 반환합니다.
    /// </summary>
    bool[] isItemPlaced;

    bool[] isItemEquipped;
    
    /// <summary>
    /// 해당 슬롯의 아이템이 장착상태 인지를 반환합니다.
    /// </summary>
    public bool[] IsItemEquipped { get { return isItemEquipped; } }







    void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        base.Start();

        if( initializer.dicTypes.Length != 1 )
            throw new Exception("하나의 종류 딕셔너리만 선택가능합니다.");

        if( initializer.dicTypes[0].itemType != ItemType.Weapon )
            throw new Exception("전용 퀵슬롯이 무기 종류가 아닙니다.");
        
        if( equipWeaponType.Length != initializer.dicTypes.Length )
            throw new Exception("인스펙터 창에서 고정 무기를 모두 지정하지 않았습니다.");
        
        // 슬롯 길이 설정
        slotLen = equipWeaponType.Length;

        // 설정한 길이만큼 배열을 생성합니다.
        dummyTr = new Transform[slotLen];
        dummyImg = new Image[slotLen];
        isItemPlaced = new bool[slotLen];
        isItemEquipped = new bool[slotLen];
        slotItemInfo = new ItemInfo[slotLen];
        
        for( int i = 0; i<slotLen; i++ )
        {
            // 더미오브젝트를 만들고 이미지 컴포넌트를 만든 후 emptyList에 배치합니다.
            dummyTr[i] = new GameObject( "dummy"+ i.ToString() ).transform;
            dummyTr[i].SetParent( emptyListTr );
            dummyImg[i] = dummyTr[i].gameObject.AddComponent<Image>();
            

            // 슬롯 장착 상태 및 아이템 착용 상태 초기화
            isItemPlaced[i] = false;
            isItemEquipped[i] = false;
        }
        

    }


    /// <summary>
    /// 슬롯을 작동할 때 호출해줘야할 메서드입니다.<br/>
    /// 장비를 착용시켜줌과 동시에 더미 이미지를 슬롯에 보여줘서 
    /// 기존 슬롯에 아이템이 그대로 있는 것 처럼 보이게 합니다.<br/>
    /// </summary>
    public void SlotActivate(int slotNo)
    {
        // 아이템이 자리하지 않았거나, 아이템을 장착중이라면 더 이상 실행하지 않습니다.
        if( !isItemPlaced[slotNo] || isItemEquipped[slotNo] )
            return;

        // 해당 슬롯의 아이템 정보를 참조합니다.
        slotItemInfo[slotNo] = slotListTr.GetChild(slotNo-1).GetComponent<ItemInfo>();

        // 아이템을 장착할 계층을 설정합니다.
        Transform equipTr = GetEquipTr(slotItemInfo[slotNo]);

        // 아이템을 장착합니다.
        slotItemInfo[slotNo].OnItemEquip(equipTr);

        // 더미 오브젝트를 해당 슬롯으로 보냅니다.
        dummyTr[slotNo].SetParent( slotListTr.GetChild(slotNo), false );

        // 더미 이미지를 보여줍니다.
        dummyImg[slotNo].sprite = slotItemInfo[slotNo].innerSprite;
        
        // 아이템 장착상태 활성화
        isItemEquipped[slotNo] = true;
    }



    /// <summary>
    /// 슬롯을 작동해제할 때 호출해줘야할 메서드입니다.<br/>
    /// 슬롯에 보여지고 있던 더미 이미지를 빈리스트로 보낸 후,
    /// 착용되어있던 장비를 해제하여 슬롯에 놓아주는 역할을 합니다.<br/>
    /// </summary>
    public void SlotDeactivate(int slotNo)
    {
        // 아이템이 장착중이지 않은 상태라면 실행하지 않습니다.
        if( !isItemEquipped[slotNo] )
            return;
        
        // 더미를 다시 빈리스트로 보냅니다
        dummyTr[slotNo].SetParent(emptyListTr);

        // 지정 슬롯에 추가합니다.
        AddItemToSlot(slotItemInfo[slotNo], slotNo, interactive.IsActiveTabAll);
                
        // 아이템 장착상태 해제
        isItemEquipped[slotNo] = false;
    }






    public Transform GetEquipTr(ItemInfo itemInfo)
    {
        if( ! (itemInfo.Item is ItemEquip) )
            throw new Exception("착용 가능한 장비가 아닙니다.");


        ItemType itemType = itemInfo.Item.Type;

        if( itemType==ItemType.Weapon )
        {
            WeaponType weaponType = ( (ItemWeapon)itemInfo.Item ).WeaponType;

            switch(weaponType)
            {
                case WeaponType.Axe:
                    return equipTr.axeTr;

                case WeaponType.Blunt:
                    return equipTr.bluntTr;

                case WeaponType.Spear:
                    return equipTr.spearTr;

                case WeaponType.Sword:
                    return equipTr.swordTr;

                case WeaponType.Bow:
                    return equipTr.bowTr;                    
            }
        }

        throw new Exception("착용위치 정보가 맵핑되지 않았습니다.");
    }




    /// <summary>
    /// 슬롯 드롭이벤트가 일어날 때 드롭을 발생시키는 쪽에서 호출해줘야 할 메서드입니다.<br/>
    /// 드롭이 일어나는 슬롯정보를 전달해줘야 합니다.
    /// </summary>
    public void OnSlotDropped( ItemInfo droppedItemInfo, int slotNo )
    {
        // 슬롯 장착상태 활성화
        isItemPlaced[slotNo] = true;

        // 슬롯 아이템 정보 활성화
        slotItemInfo[slotNo] = droppedItemInfo;


    }

    /// <summary>
    /// 더미 아이템이 셀렉트 될 때 더미 쪽에서 호출해줘야 할 메서드입니다.<br/>
    /// 드롭이 일어나는 슬롯정보를 전달해줘야 합니다.
    /// </summary>
    public void OnDummySelected(int slotNo )
    {
        // 슬롯 장착상태 해제
        isItemPlaced[slotNo] = false;

        // 슬롯아이템 정보 해제
        slotItemInfo[slotNo] = null;

        // 더미를 잡고 있다. 

    }




}


