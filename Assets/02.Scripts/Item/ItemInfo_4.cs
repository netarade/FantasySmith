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
    /// 무기 및 건설재료 아이템의 내구도입니다. (해당 아이템 : 무기, 건설재료)<br/><br/>
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
                itemWeapon.Durability=value;
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
                itemMisc.SetOverlapCount(value);    // 수량정보를 수정합니다.
                UpdateTextInfo();   // 텍스트 정보를 수정합니다.
                CheckDestroyInfo(); // 파괴 여부를 체크합니다.
            }
            else
                throw new Exception( "아이템의 종류가 맞지 않습니다." );
        }
    }








    // 무기의 종류를 열거형 인덱스에 맞춰서 문자열로 변환하여 표시합니다 (WeaponType수정시 같이 수정해야 합니다.)
    private readonly string[] weaponTypeString = { "검", "창", "도끼", "둔기", "활", "곡괭이", "기타", "없음" };
    


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
            $"종류 : {weaponTypeString[(int)(itemWeapon.WeaponType)]}\n" +
            $"공격력 : {itemWeapon.Power}\n" +
            $"내구도 : {itemWeapon.Durability}"
            );

        ItemQuest itemQuest = item as ItemQuest;
        if(itemQuest!=null)
            return string.Format(
            $"종류 : 퀘스트\n"
            );
        
        ItemBuilding itemBuilding = item as ItemBuilding;  
        if(itemBuilding!=null)
            return string.Format(
            $"종류 : 건설 재료\n" +
            $"수량 : {itemBuilding.OverlapCount}" +
            $"내구도 : {itemBuilding.Durability}"
            );


        ItemMisc itemMisc = item as ItemMisc;
        if(itemMisc!=null)
            return string.Format(
            $"종류 : 잡화\n" +
            $"수량 : {itemMisc.OverlapCount}"
            );   
        
        return null;
    }



}
