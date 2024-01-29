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
            throw new Exception("������ ������ ���޵��� �ʾҽ��ϴ�.");

        ItemEquip itemEquip = itemInfo.Item as ItemEquip;
        if( itemEquip==null )
            throw new Exception("���� ������ ��� �ƴմϴ�.");


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

        throw new Exception("������ġ ������ ���ε��� �ʾҽ��ϴ�.");
    }




}








[Serializable]
public class EquipTr
{
    [Header("2-����")]
    public Transform axeTr;

    [Header("3-���")]
    public Transform pickaxTr;
    
    [Header("4-â")]
    public Transform spearTr;
        
    [Header("5-Ȱ")]
    public Transform bowTr;

    [Header("���")]
    public Transform helmetTr;
    

}
