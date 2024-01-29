using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * [작업 사항]
 * <v1.0 - 2024_0130_최원준>
 * 1- 퀵슬롯에서 EquipmentTransform을 이동하여 작성
 * 플레이어 쪽에 모듈화시켜서 달아두고 참조하기 위함.
 * 
 * 
 * 
 * 
 */




public class EquipmentTransform : MonoBehaviour
{

    [SerializeField] EquipTr equipTr;


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
        else if(itemType==ItemType.Quest)
        {
            


        }

        throw new Exception("착용위치 정보가 맵핑되지 않았습니다.");
    }




}








[Serializable]
public class EquipTr
{
    [Header("2-도끼")]
    public Transform axeTr;

    [Header("3-곡괭이")]
    public Transform pickaxTr;
    
    [Header("4-창")]
    public Transform spearTr;
        
    [Header("5-활")]
    public Transform bowTr;

    [Header("헬멧")]
    public Transform helmetTr;
    

}
