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
 * <v1.1 - 2024_0130_최원준>
 * 1- 직렬화 클래스명을 EquipmentTransform으로 하고, 스크립트명을 EquipmentInfo로 변경
 * 2- 무기 이외의 장비 아이템 - Helmet장착지점 매핑
 * 
 * <v1.2 - 2024_0130_최원준>
 * 1- GetEquipTr 메서드명 GetEquipParentTr로 변경
 * 
 */




public class EquipmentInfo : MonoBehaviour
{

    [SerializeField] EquipmentTransform equipTr;
    PlayerStatus playerStatus;

    private void Awake()
    {
        playerStatus = GetComponent<PlayerStatus>();
    }


    public Transform GetEquipParentTr(ItemInfo itemInfo)
    { 
        if(itemInfo == null)
            throw new Exception("아이템 정보가 전달되지 않았습니다.");

        ItemEquip itemEquip = itemInfo.Item as ItemEquip;
        if( itemEquip==null )
            throw new Exception("착용 가능한 장비가 아닙니다.");


        ItemType itemType = itemInfo.Item.Type;

        // 무기인 경우 세부타입을 기준으로 장착지점 반환
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
    /// 퀘스트 아이템 - 후드를 착용 및 해제를 하는 메서드입니다.
    /// </summary>
    public void EquipHoodSwitch(bool isEquip)
    {
        for(int i=0; i<equipTr.hoodTr.Length; i++)
        {
            equipTr.hoodTr[i].gameObject.SetActive(isEquip);
        }

        playerStatus.OnHoodEquip(isEquip);
    }






}








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

    [Header("후드-퀘스트용")]
    public Transform[] hoodTr;
}
