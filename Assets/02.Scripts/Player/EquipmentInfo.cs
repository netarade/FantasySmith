using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * [�۾� ����]
 * <v1.0 - 2024_0130_�ֿ���>
 * 1- �����Կ��� EquipmentTransform�� �̵��Ͽ� �ۼ�
 * �÷��̾� �ʿ� ���ȭ���Ѽ� �޾Ƶΰ� �����ϱ� ����.
 * 
 * <v1.1 - 2024_0130_�ֿ���>
 * 1- ����ȭ Ŭ�������� EquipmentTransform���� �ϰ�, ��ũ��Ʈ���� EquipmentInfo�� ����
 * 2- ���� �̿��� ��� ������ - Helmet�������� ����
 * 
 * <v1.2 - 2024_0130_�ֿ���>
 * 1- GetEquipTr �޼���� GetEquipParentTr�� ����
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
            throw new Exception("������ ������ ���޵��� �ʾҽ��ϴ�.");

        ItemEquip itemEquip = itemInfo.Item as ItemEquip;
        if( itemEquip==null )
            throw new Exception("���� ������ ��� �ƴմϴ�.");


        ItemType itemType = itemInfo.Item.Type;

        // ������ ��� ����Ÿ���� �������� �������� ��ȯ
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

        throw new Exception("������ġ ������ ���ε��� �ʾҽ��ϴ�.");
    }


    /// <summary>
    /// ����Ʈ ������ - �ĵ带 ���� �� ������ �ϴ� �޼����Դϴ�.
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
    [Header("2-����")]
    public Transform axeTr;

    [Header("3-���")]
    public Transform pickaxTr;
    
    [Header("4-â")]
    public Transform spearTr;
        
    [Header("5-Ȱ")]
    public Transform bowTr;

    [Header("�ĵ�-����Ʈ��")]
    public Transform[] hoodTr;
}
