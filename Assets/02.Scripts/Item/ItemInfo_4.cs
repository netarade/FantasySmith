using DataManagement;
using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* [작업 사항]
* <v1.0 - 2024_0118_최원준>
* 1- 아이템의 정보를 쉽게 읽고 쓰기 위해 다양 한 변수와 메서드를 선언하였음.
* 내부 item을 직접 건드리도록하지 않고 Info클래스에서 건드리도록 함과 동시에, 
* 필요 시 관련 메서드 호출을 자동으로 이룰수 있게 하기 위함.
* 
* <v1.1 - 2024_0118_최원준>
* 1- Durability프로퍼티 내부에 itemBuilding이후에 itemWeapon의 Durability를 수정하고 있어서 null값 오류가 뜨는 부분 수정
* 
* <v1.2 - 2024_0124_최원준>
* 1- 빌딩아이템의 속성을 반환하는 IsDecoration 프로퍼티를 추가
* 
* <v1.3 - 2024_0125_최원준>
* 1- 빌딩아이템 클래스 설계 변경(ItemMisc상속->Item상속)으로 인해 GetItemSpec메서드 내부 수정
* 
* <v1.4 - 2024_0126_최원준>
* 1- SerializedTr프로퍼티 내부 ItemWeapon을 ItemEquip으로 변경
* 이유는 퀘스트 아이템도 ItemEquip을 상속하도록 변경하였기 때문
* 
* 2- GetItemSpec메서드 string값 서바이벌 프로젝트에 맞게 영문화
* 
* 3- 아이템 착용관련 프로퍼티 IsEquip, EquipSlotIndex를 추가
* 
* <v1.5 - 2024_0130_최원준>
* 1- IsEquip프로퍼티의 set기능 추가
* 
* 2- EquipType 프로퍼티 추가
* 
* 
*/
public partial class ItemInfo : MonoBehaviour
{
    
    
    string strSpec;    // 아이템 스펙 (내부 수정 및 저장 변수)

    /// <summary>
    /// 아이템의 이름입니다.
    /// </summary>
    public string Name { get { return item.Name; } }

    /// <summary>
    /// 아이템의 간단한 설명입니다.
    /// </summary>
    public string Desc { get { return item.Desc;} }

    /// <summary>
    /// 아이템 고유의 수치를 문자열로 조합하여 반환합니다.<br/>
    /// 아이템 종류에 따라서 표시되는 문자열이 틀립니다.
    /// </summary>
    public string Spec { get { return strSpec;} }

    /// <summary>
    /// 아이템의 대분류로 잡화, 무기, 장비, 퀘스트 3가지 종류가 있습니다.
    /// </summary>
    public ItemType Type { get { return item.Type; } }
        

    


    /// <summary>
    /// 무기 아이템의 상세 분류입니다. (해당 아이템 : 무기)<br/><br/>
    /// 검, 창, 도끼, 둔기, 활, 곡괭이, 기타 등의 종류가 있습니다.<br/><br/>
    /// get - 아이템 종류가 맞으면 상세 타입을 반환하며, 틀리면 WeaponType.None을 반환합니다.<br/><br/>
    /// </summary>
    public WeaponType WeaponType 
    { 
        get 
        { 
            ItemWeapon itemWeapon = item as ItemWeapon;
            
            if(itemWeapon != null)
                return itemWeapon.WeaponType;
            else
                return WeaponType.None; 
        } 
    }

    /// <summary>
    /// 잡화 아이템의 상세 분류입니다. (해당 아이템 : 잡화)<br/><br/>
    /// 기본, 제작, 건설, 도구, 포션 등의 종류가 있습니다.<br/><br/>
    /// get - 아이템 종류가 맞으면 상세 타입을 반환하며, 틀리면 MiscType.None을 반환합니다.<br/><br/>
    /// </summary>
    public MiscType MiscType
    {
        get 
        { 
            ItemMisc itemMisc = item as ItemMisc;
            
            if(itemMisc != null)
                return itemMisc.MiscType;
            else
                return MiscType.None; 
        } 
    }







    /// <summary>
    /// 무기 아이템의 공격력입니다. (해당 아이템 : 무기)<br/><br/>
    /// get - 아이템 종류가 맞으면 공격력을 반환하며, 틀리면 음의 정수(-1)를 반환합니다.<br/><br/>
    /// 
    /// set - 공격력 수치를 수정할 수 있으며, 아이템 종류가 맞지 않는 경우 예외가 발생합니다.
    /// </summary>
    public int Power
    {
        get
        {
            ItemWeapon itemWeapon = item as ItemWeapon;
            
            if(itemWeapon != null)
                return itemWeapon.Power;
            else
                return -1;        
        }
        set
        {
            ItemWeapon itemWeapon = item as ItemWeapon;
            
            if(itemWeapon != null)
            {                
                itemWeapon.Power=value;
                UpdateTextInfo();   // 텍스트 정보를 수정합니다.
            }
            else
                throw new Exception( "아이템의 종류가 맞지 않습니다." );
        }
    }

    /// <summary>
    /// 무기 및 건설재료 아이템의 내구도입니다. (해당 아이템 : 무기, 건설)<br/><br/>
    /// get - 아이템 종류가 맞으면 내구도를 반환하며, 틀리면 음의 정수(-1)를 반환합니다.<br/><br/>
    /// 
    /// set - 내구도 수치를 수정할 수 있으며, 아이템 종류가 맞지 않는 경우 예외가 발생합니다.<br/>
    /// 내구도 수치가 0이하가 된 경우 아이템이 파괴됩니다.
    /// </summary>
    public int Durability
    {
        get
        {            
            ItemWeapon itemWeapon = item as ItemWeapon;
            ItemBuilding itemBuilding = item as ItemBuilding;


            if(itemWeapon != null)
                return itemWeapon.Durability;
            else if(itemBuilding != null)
                return itemBuilding.Durability;
            else
                return -1;            
        }
        set
        {
            ItemWeapon itemWeapon = item as ItemWeapon;
            ItemBuilding itemBuilding = item as ItemBuilding;
                       
            if(itemWeapon != null)
            {                
                itemWeapon.Durability=value;
                UpdateTextInfo();   // 텍스트 정보를 수정합니다.
                CheckDestroyInfo(); // 파괴 여부를 체크합니다.
            }
            else if(itemBuilding != null)
            {
                itemBuilding.Durability=value;
                UpdateTextInfo();   // 텍스트 정보를 수정합니다.
                CheckDestroyInfo(); // 파괴 여부를 체크합니다.
            }
            else
                throw new Exception( "아이템의 종류가 맞지 않습니다." );
        }
    }


    /// <summary>
    /// 잡화 아이템의 중첩 수량입니다. (해당 아이템 : 잡화)<br/><br/>
    /// get - 아이템 종류가 맞으면 중첩수량을 반환하며, 틀리면 음의 정수(-1)를 반환합니다.<br/><br/>
    /// 
    /// set - 중첩수량 수치를 수정할 수 있으며, 아이템 종류가 맞지 않는 경우 예외가 발생합니다.<br/>
    /// 최대수량에 도달하면 더 이상 증가되지 않으며, 최소수량(0)에 도달한 경우 파괴됩니다.
    /// </summary>
    public int OverlapCount
    {
        get 
        { 
            ItemMisc itemMisc = item as ItemMisc;
            
            if(itemMisc != null)
                return itemMisc.OverlapCount;
            else
                return -1; 
        } 
        set
        {
            ItemMisc itemMisc = item as ItemMisc;
                       
            if(itemMisc != null)
            {                
                itemMisc.AccumulateOverlapCount(value);    // 수량정보를 수정합니다.
                UpdateTextInfo();   // 텍스트 정보를 수정합니다.
                CheckDestroyInfo(); // 파괴 여부를 체크합니다.
            }
            else
                throw new Exception( "아이템의 종류가 맞지 않습니다." );
        }
    }


    
    /// <summary>
    /// 건설 아이템의 상세 분류 정보를 반환합니다. (해당 아이템 : 건설)<br/><br/>
    /// 재료아이템, 장식용아이템, 인벤토리아이템, 없음 등의 종류가 있습니다.<br/><br/>
    /// get - 아이템 종류가 맞으면 상세 타입을 반환하며, 틀리면 BuildingType.None을 반환합니다.<br/><br/>
    /// </summary>
    public BuildingType BuildingType
    {
        get
        {
            ItemBuilding itemBuilding = item as ItemBuilding;

            if( itemBuilding!=null )
                return itemBuilding.buildingType;

            return BuildingType.None;
        }
    }

    
    /// <summary>
    /// 아이템에 저장된 STransform을 반환합니다.  (해당 아이템 : 무기, 건설)<br/><br/>
    /// get - ItemEquip을 상속하는 무기, 퀘스트 아이템인 경우 EquipTr을, ItemBuilding인 경우 WorldTr을, 이외의 타입은 null을 반환합니다.
    /// </summary>
    public STransform SerializedTr {  
        get 
        { 
            ItemEquip itemEquip = item as ItemEquip;
            ItemBuilding itemBuilding = item as ItemBuilding;
            
            if(itemBuilding!=null)
                return itemBuilding.WorldTr;

            else if(itemEquip!=null)
                return itemEquip.EquipLocalTr;

            return null;
        } 

    }



    /// <summary>
    /// 아이템이 캐릭터에 영향을 주는 스테이터스 수치를 반환합니다.(해당 아이템 : 음식)<br/><br/>
    /// get - ItemFood인 경우 저장되어있는 ItemStatus 인스턴스를, 이 외의 타입은 default(ItemStatus)를 반환합니다.
    /// </summary>
    public ItemStatus StatusInfo
    {
        get
        {
            ItemFood itemFood = item as ItemFood;
            
            if( itemFood!=null )
                return itemFood.Status;
            else
                return default(ItemStatus);
        }

    }


    /// <summary>
    /// 장비의 착용 상태를 반환합니다.<br/><br/>
    /// get - 아이템이 현재 착용 중이라면 true를, 착용중이 아니거나 착용불가능한 아이템의 경우 false를 반환합니다.<br/>
    /// set - 아이템의 착용 상태를 수정합니다.
    /// </summary>
    public bool IsEquip
    {
        get
        {
            ItemEquip itemEquip = item as ItemEquip;

            if(itemEquip != null )
                return itemEquip.isEquip;
            else
                return false;
        }

        set
        {
            ItemEquip itemEquip = item as ItemEquip;
            if( itemEquip != null )
                itemEquip.isEquip = value;            
        }

    }


    /// <summary>
    /// 해당 아이템의 장착 지점 종류 정보를 반환합니다.<br/><br/>
    /// get - 장착 가능한 아이템의 경우 해당 EquipType을 반환, 장착 불가능한 아이템의 경우 EquipType.None을 반환합니다.
    /// </summary>
    public EquipType EquipType
    {
        get
        {
            ItemEquip itemEquip = item as ItemEquip;

            if(itemEquip != null )
                return itemEquip.EquipType;
            else
                return EquipType.None;
        }
    }





    /// <summary>
    /// 장비가 착용 중인 슬롯의 인덱스를 반환합니다.<br/><br/>
    /// 아이템이 현재 착용 중이라면 양의 정수를, 착용중이 아니거나 착용불가능한 아이템의 경우 -1을 반환합니다.
    /// </summary>
    public int EquipSlotIndex
    {
        get
        {
            ItemEquip itemEquip = item as ItemEquip;

            if(itemEquip != null )
                return itemEquip.EquipSlotIndex;
            else
                return -1;
        }
    }










    // 무기의 종류를 열거형 인덱스에 맞춰서 문자열로 변환하여 표시합니다 (WeaponType수정시 같이 수정해야 합니다.)
    private readonly string[] weaponTypeString = { "Sword", "Spear", "Axe", "Blunt", "Bow", "Pickaxe", "Etc", "None" };
    


    /// <summary>
    /// 아이템 고유의 수치를 문자열로 반환하여 표시하여 줍니다.<br/>
    /// 아이템 종류에 따라서 표시되는 문자열이 틀립니다.
    /// </summary>
    /// <returns>아이템 고유의 수치를 문자열로 반환</returns>
    private string GetItemSpec()
    {        
        ItemWeapon itemWeapon = item as ItemWeapon;
        
        if(itemWeapon!=null)
            return string.Format(
            $"Type : {weaponTypeString[(int)(itemWeapon.WeaponType)]}\n" +
            $"Power : {itemWeapon.Power}\n" +
            $"Durability : {itemWeapon.Durability}"
            );

        ItemQuest itemQuest = item as ItemQuest;
        if(itemQuest!=null)
            return string.Format(
            $"Type : Quest\n"
            );
        
        ItemBuilding itemBuilding = item as ItemBuilding;  
        if(itemBuilding!=null)
            return string.Format(
            $"Type : Building\n" +
            $"Durability : {itemBuilding.Durability}"
            );


        ItemMisc itemMisc = item as ItemMisc;
        if( itemMisc!=null )
        {
            string strMiscType;
            MiscType miscType = itemMisc.MiscType;
            if( miscType == MiscType.Food)
                strMiscType = "Food";
            else
                strMiscType = "Misc";

            return string.Format(
            $"Type : {strMiscType}\n"+
            $"Count : {itemMisc.OverlapCount}"
            );
        }
        return null;
    }



}
