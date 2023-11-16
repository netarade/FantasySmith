using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1102_최원준>
 * 1- 최초작성 및 주석처리
 * 
 * <v2.0 - 2023_1103_최원준>
 * 1- 주석 수정
 * 2- 이미지 컴포넌트 잡는 구문을 Start메서드에서 OnEnable로 변경
 * 인스턴스가 생성되어 이미지 컴포넌트를 잡기 시작하면 OnItemAdded와 호출 시점이 동시성을 가져서 스프라이트 이미지가 변경되지 않는다.
 * 
 * <v3.0 - 2023-1105_최원준>
 * 1- 개념아이템 변수인 item을 프로퍼티화 시켜서 set이 호출되었을 때 OnItemChanged()가 호출되도록 변경 
 * OnItemAdded는 private처리 및 내부 예외처리 구문 삭제
 *
 *<v4.0 - 2023_1108_최원준>
 *1- 아이템이 파괴될 때 정보를 저장하도록 구현하였으나, 모든아이템의 기록이 저장되지 않는 문제발생
 *=> 아이템쪽에서 파괴될떄마다 딕셔너리를 생성해서 CraftManager쪽에서 마지막에 한번 미리 생성해주도록 변경
 *
 *2- OnItemChanged 메서드 주석추가
 *
 *3- UpdateCountTxt 메서드 추가
 * 아이템 수량이 변경 될때 동적으로 텍스트를 수정해주도록 하였음.
 * item쪽에서 메서드를 가지고 있음으로 해서 편리한 접근성 확보.
 *
 *<v5.0 - 2023_1112_최원준>
 *1- OnItemAdded메서드 추가수정. (CreateManager쪽에서 중복코드 사용하고 있던 점 수정 및 통합, 주석 추가) 
 *
 *<v6.0 - 2023_1114_최원준>
 *1- OnItemAdded메서드를 OnItemChanged로 이름변경
 *2- ItemInfo 클래스 설명 주석 추가
 *3- private 메서드 public 메서드로 변경
 *4- 멤버 변수 item이 public되어있던 점을 private 처리. 반드시 프로퍼티를 통한 초기화를 위해
 *
 *<v7.0 - 2023_1116_최원준>
 *1- ItemInfo 클래스가 ItemImageCollection 멤버변수를 포함하여 외부이미지를 참조하도록 설정하였습니다. 
 *(CreateManager에 있던 참조 변수를 옮겨옴.)
 *
 *2- UpdateImage메서드를 수정하였습니다.
 *기존의 아이템 클래스가 ImageCollection 구조체 변수를 멤버로 포함하고 있던 점에서 ImageReferenceIndex 구조체 변수를 멤버로 포함하도록 바꾸었기 때문에
 *item의 ImageReferenceIndex 멤버변수로 부터 인덱스값을 받아와서 ImageCollection 변수에 접근하여 오브젝트에 이미지를 넣어주도록 수정.
 *
 */


/// <summary>
/// 게임 상의 아이템 오브젝트는 이 클래스를 컴포넌트로 가져야합니다.<br/><br/>
/// 
/// ItemInfo 스크립트가 컴포넌트로 붙은 아이템 오브젝트의 자체적인 기능은 다음과 같습니다.<br/>
/// (ItemInfo의 개념 아이템 인스턴스인 item이 할당될 때 자동으로 이루어집니다.)<br/><br/>
/// 
/// 1.오브젝트의 이미지를 개념 아이템의 정보와 대조하여 채웁니다.<br/>
/// 2.잡화아이템의 경우 중첩횟수를 아이템정보와 비교하여 표시하여 줍니다. 비잡화 아이템의 경우 텍스트를 끕니다.<br/>
/// 3.인벤토리 슬롯 상의 포지션을 아이템 정보와 대조하여 해당 슬롯에 위치시킵니다.<br/><br/>
/// 
/// 주의) 아이템의 내부 정보가 바뀔 때 마다 최신 정보를 오브젝트에 반영해야 합니다.<br/>
/// 1,2,3의 경우 각 메서드를 따로 호출 할 수 있으며 모든 것을 한번에 호출하는  OnItemChanged메서드가 있습니다.<br/>
/// </summary>
public class ItemInfo : MonoBehaviour
{
    private Item item;               // 모든 아이템 클래스를 관리 가능한 변수
    public Image innerImage;        // 아이템이 인벤토리에서 보여질 이미지 (오브젝트의 이미지 컴포넌트를 말한다.)
    public Text countTxt;           // 잡화 아이템의 수량을 반영할 텍스트
    public Transform slotList;      // 아이템이 놓이게 될 슬롯 들의 부모인 슬롯리스트를 참조

    [SerializeField] ItemImageCollection iicMiscBase;           // 인스펙터 뷰 상에서 등록할 잡화 기본 아이템 이미지 집합
    [SerializeField] ItemImageCollection iicMiscAdd;            // 인스펙터 뷰 상에서 등록할 잡화 추가 아이템 이미지 집합
    [SerializeField] ItemImageCollection iicMiscOther;          // 인스펙터 뷰 상에서 등록할 잡화 기타 아이템 이미지 집합
    [SerializeField] ItemImageCollection iicWeaponSword;        // 인스펙터 뷰 상에서 등록할 무기 검 아이템 이미지 집합
    [SerializeField] ItemImageCollection iicWeaponBow;          // 인스펙터 뷰 상에서 등록할 무기 활 아이템 이미지 집합

    public void Start()
    {
        countTxt = GetComponentInChildren<Text>();
        slotList = GameObject.Find("Inventory").transform.GetChild(0);
        
        // 인스펙터뷰 상에서 달아놓은 스프라이트 이미지 집합을 참조한다.
        iicMiscBase=GameObject.Find( "ImageCollections" ).transform.GetChild( 0 ).GetComponent<ItemImageCollection>();
        iicMiscAdd=GameObject.Find( "ImageCollections" ).transform.GetChild( 1 ).GetComponent<ItemImageCollection>();
        iicMiscOther=GameObject.Find( "ImageCollections" ).transform.GetChild( 2 ).GetComponent<ItemImageCollection>();
        iicWeaponSword=GameObject.Find( "ImageCollections" ).transform.GetChild( 3 ).GetComponent<ItemImageCollection>();
        iicWeaponBow=GameObject.Find( "ImageCollections" ).transform.GetChild( 4 ).GetComponent<ItemImageCollection>();
    }


    /// <summary>
    /// 클론 한 Item 인스턴스를 저장하고, 저장 되어있는 인스턴스를 불러올 수 있습니다.<br/> 
    /// 아이템이 저장될 때 자동으로 OnItemChanged()메서드를 호출하여 오브젝트 상의 정보를 반영합니다.
    /// </summary>
    public Item Item                // 외부에서 개념아이템을 참조시킬 때 호출해야할 프로퍼티
    {
        set {
                item =  value;
                OnItemChanged();      // 개념아이템 참조값이 들어오면 내부에서 자동 호출해준다. 
            }
        get {return item;}
    }


    /// <summary>
    /// 이미지 컴포넌트를 잡는 우선순위를 높이기 위해 OnEnable 사용
    /// </summary>
    private void OnEnable()
    {
        innerImage = GetComponent<Image>();
        countTxt = GetComponentInChildren<Text>();
        countTxt.enabled = false;
    }

    /// <summary>
    /// 오브젝트에 item의 참조가 이루어졌다면 item이 가지고 있는 이미지를 반영하고 잡화아이템의 경우 중첩 횟수까지 최신화 합니다.<br/>
    /// 오브젝트의 아이템 값을 입력할 때는 내부 자동 호출이 이루어지는 메서드이기에 아이템 정보 변동이 있을 때만 따로 호출하시면 됩니다.
    /// </summary>
    public void OnItemChanged()
    {
        UpdateImage();
        UpdateCountTxt();
        UpdatePosition();
    }

    /// <summary>
    /// 아이템의 이미지 정보를 받아와서 오브젝트에 반영합니다.<br/>
    /// Item 클래스는 정의될 때 외부에서 참조할 이미지 인덱스를 저장하고 있습니다.<br/>
    /// 해당 인덱스를 참고하여 인스펙터뷰에 등록된 이미지를 참조합니다.
    /// </summary>
    public void UpdateImage()
    {
        switch( item.Type ) // 아이템의 메인타입을 구분합니다.
        {
            case ItemType.Weapon:
                WeaponType weaponType = ((ItemWeapon)item).EnumWeaponType;  // 아이템의 서브타입을 구분합니다.

                if(weaponType==WeaponType.Sword)
                    innerImage.sprite = iicWeaponSword.icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;  
                else if(weaponType==WeaponType.Bow)
                    innerImage.sprite =iicWeaponBow.icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;
                break;

                // 아이템 오브젝트 이미지를 인스펙터뷰에 직렬화되어 있는 ItemImageCollection 클래스의
                // 내부 구조체 배열 ImageColection[]에 개념아이템이 고유 정보로 가지고 있는 ImageReferenceIndex 구조체의 인덱스를 가져옵니다.                
                
            case ItemType.Misc:
                MiscType miscType = ((ItemMisc)item).EnumMiscType;

                if(miscType==MiscType.Basic)
                    innerImage.sprite = iicMiscBase.icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;
                else if(miscType==MiscType.Additive)
                    innerImage.sprite = iicMiscAdd.icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;
                else
                    innerImage.sprite = iicMiscOther.icArrImg[item.sImageRefIndex.innerImgIdx].innerSprite;
                break;
        }
    }


    /// <summary>
    /// 잡화 아이템의 중첩횟수를 동적으로 수정합니다. 잡화 아이템의 수량이 변경될 때 마다 호출해 주십시오.
    /// </summary>
    public void UpdateCountTxt()
    {
        if( item.Type==ItemType.Misc )                // 잡화 아이템의 중첩 갯수를 표시합니다.
        {
            countTxt.enabled=true;
            countTxt.text = ((ItemMisc)item).InventoryCount.ToString();
        }
        else
            countTxt.enabled = false;                // 잡화아이템이 아니라면 중첩 텍스트를 비활성화합니다.
    }


    // 새로운 아이템 정보 참조 시 (Item인스턴스를 꺼내서 새로운 오브젝트 만들어서 Item인스턴스를 저장했을 때)


    // 수량 변경으로 0이 되었을 때 파괴 행위. (현재 어디서 검사하고있음? => CraftManagement에서 CraftManager.instance.UpdateInventoryText(true); 를 호출하고 있다)
    // 문제점. 일시적으로 -가되었을 때 파괴시킬 것인가
    // 제거는 오브젝트 뿐만 아니라 인벤토리의 리스트에서도 사라져야 한다. 제거된 참조값이 남아있으면 안된다.

    /// <summary>
    /// 
    /// </summary>
    /// <returns>반환 값은 아이템의 슬롯 넘버입니다.</returns>
    public int RemoveItemObject()
    {

    }


    // 아이템의 위치정보 반영  (현재 어디서 아이템 위치를 참조해서 슬롯에 넣어주고 있는가? => CreateManager에서 Instantiate할때 미리 붙인다.)
    public void UpdatePosition()
    {
        transform.SetParent( slotList.GetChild(item.SlotIndex) );  // 오브젝트의 현 부모를 아이템 정보상의 슬롯 위치로 변경한다.
        transform.localPosition = Vector3.zero;                    // 로컬위치를 부모기준으로부터 0,0,0으로 맞춘다.
    }





    /// <summary>
    /// 아이템이 파괴 되기전 정보를 넘겨주기 위한 메서드 (현재 임시로 이름과 수량만 넘겨주고 새로운 아이템을 생성하는 형식으로 되어 있다.)
    /// </summary>
    private void OnDestroy()
    {
        if( item.Type==ItemType.Misc )
        {
            CraftManager.instance.miscSaveDic.Add( item.Name, ( (ItemMisc)item ).InventoryCount );    //이름과 수량을 저장
            Debug.Log( item.Name );
        }
        else if( item.Type==ItemType.Weapon )
        {
            CraftManager.instance.weapSaveDic.Add( item.Name, 0 );    //이름을 저장
            Debug.Log( item.Name );
        }
    }

}