using DataManagement;
using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* [�۾� ����]
* <v1.0 - 2024_0118_�ֿ���>
* 1- �������� ������ ���� �а� ���� ���� �پ� �� ������ �޼��带 �����Ͽ���.
* ���� item�� ���� �ǵ帮�������� �ʰ� InfoŬ�������� �ǵ帮���� �԰� ���ÿ�, 
* �ʿ� �� ���� �޼��� ȣ���� �ڵ����� �̷�� �ְ� �ϱ� ����.
* 
* <v1.1 - 2024_0118_�ֿ���>
* 1- Durability������Ƽ ���ο� itemBuilding���Ŀ� itemWeapon�� Durability�� �����ϰ� �־ null�� ������ �ߴ� �κ� ����
* 
* <v1.2 - 2024_0124_�ֿ���>
* 1- ������������ �Ӽ��� ��ȯ�ϴ� IsDecoration ������Ƽ�� �߰�
* 
* <v1.3 - 2024_0125_�ֿ���>
* 1- ���������� Ŭ���� ���� ����(ItemMisc���->Item���)���� ���� GetItemSpec�޼��� ���� ����
* 
* <v1.4 - 2024_0126_�ֿ���>
* 1- SerializedTr������Ƽ ���� ItemWeapon�� ItemEquip���� ����
* ������ ����Ʈ �����۵� ItemEquip�� ����ϵ��� �����Ͽ��� ����
* 
* 2- GetItemSpec�޼��� string�� �����̹� ������Ʈ�� �°� ����ȭ
* 
* 3- ������ ������� ������Ƽ IsEquip, EquipSlotIndex�� �߰�
* 
* <v1.5 - 2024_0130_�ֿ���>
* 1- IsEquip������Ƽ�� set��� �߰�
* 
* 2- EquipType ������Ƽ �߰�
* 
* 
*/
public partial class ItemInfo : MonoBehaviour
{
    
    
    string strSpec;    // ������ ���� (���� ���� �� ���� ����)

    /// <summary>
    /// �������� �̸��Դϴ�.
    /// </summary>
    public string Name { get { return item.Name; } }

    /// <summary>
    /// �������� ������ �����Դϴ�.
    /// </summary>
    public string Desc { get { return item.Desc;} }

    /// <summary>
    /// ������ ������ ��ġ�� ���ڿ��� �����Ͽ� ��ȯ�մϴ�.<br/>
    /// ������ ������ ���� ǥ�õǴ� ���ڿ��� Ʋ���ϴ�.
    /// </summary>
    public string Spec { get { return strSpec;} }

    /// <summary>
    /// �������� ��з��� ��ȭ, ����, ���, ����Ʈ 3���� ������ �ֽ��ϴ�.
    /// </summary>
    public ItemType Type { get { return item.Type; } }
        

    


    /// <summary>
    /// ���� �������� �� �з��Դϴ�. (�ش� ������ : ����)<br/><br/>
    /// ��, â, ����, �б�, Ȱ, ���, ��Ÿ ���� ������ �ֽ��ϴ�.<br/><br/>
    /// get - ������ ������ ������ �� Ÿ���� ��ȯ�ϸ�, Ʋ���� WeaponType.None�� ��ȯ�մϴ�.<br/><br/>
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
    /// ��ȭ �������� �� �з��Դϴ�. (�ش� ������ : ��ȭ)<br/><br/>
    /// �⺻, ����, �Ǽ�, ����, ���� ���� ������ �ֽ��ϴ�.<br/><br/>
    /// get - ������ ������ ������ �� Ÿ���� ��ȯ�ϸ�, Ʋ���� MiscType.None�� ��ȯ�մϴ�.<br/><br/>
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
    /// ���� �������� ���ݷ��Դϴ�. (�ش� ������ : ����)<br/><br/>
    /// get - ������ ������ ������ ���ݷ��� ��ȯ�ϸ�, Ʋ���� ���� ����(-1)�� ��ȯ�մϴ�.<br/><br/>
    /// 
    /// set - ���ݷ� ��ġ�� ������ �� ������, ������ ������ ���� �ʴ� ��� ���ܰ� �߻��մϴ�.
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
                UpdateTextInfo();   // �ؽ�Ʈ ������ �����մϴ�.
            }
            else
                throw new Exception( "�������� ������ ���� �ʽ��ϴ�." );
        }
    }

    /// <summary>
    /// ���� �� �Ǽ���� �������� �������Դϴ�. (�ش� ������ : ����, �Ǽ�)<br/><br/>
    /// get - ������ ������ ������ �������� ��ȯ�ϸ�, Ʋ���� ���� ����(-1)�� ��ȯ�մϴ�.<br/><br/>
    /// 
    /// set - ������ ��ġ�� ������ �� ������, ������ ������ ���� �ʴ� ��� ���ܰ� �߻��մϴ�.<br/>
    /// ������ ��ġ�� 0���ϰ� �� ��� �������� �ı��˴ϴ�.
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
                UpdateTextInfo();   // �ؽ�Ʈ ������ �����մϴ�.
                CheckDestroyInfo(); // �ı� ���θ� üũ�մϴ�.
            }
            else if(itemBuilding != null)
            {
                itemBuilding.Durability=value;
                UpdateTextInfo();   // �ؽ�Ʈ ������ �����մϴ�.
                CheckDestroyInfo(); // �ı� ���θ� üũ�մϴ�.
            }
            else
                throw new Exception( "�������� ������ ���� �ʽ��ϴ�." );
        }
    }


    /// <summary>
    /// ��ȭ �������� ��ø �����Դϴ�. (�ش� ������ : ��ȭ)<br/><br/>
    /// get - ������ ������ ������ ��ø������ ��ȯ�ϸ�, Ʋ���� ���� ����(-1)�� ��ȯ�մϴ�.<br/><br/>
    /// 
    /// set - ��ø���� ��ġ�� ������ �� ������, ������ ������ ���� �ʴ� ��� ���ܰ� �߻��մϴ�.<br/>
    /// �ִ������ �����ϸ� �� �̻� �������� ������, �ּҼ���(0)�� ������ ��� �ı��˴ϴ�.
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
                itemMisc.AccumulateOverlapCount(value);    // ���������� �����մϴ�.
                UpdateTextInfo();   // �ؽ�Ʈ ������ �����մϴ�.
                CheckDestroyInfo(); // �ı� ���θ� üũ�մϴ�.
            }
            else
                throw new Exception( "�������� ������ ���� �ʽ��ϴ�." );
        }
    }


    
    /// <summary>
    /// �Ǽ� �������� �� �з� ������ ��ȯ�մϴ�. (�ش� ������ : �Ǽ�)<br/><br/>
    /// ��������, ��Ŀ������, �κ��丮������, ���� ���� ������ �ֽ��ϴ�.<br/><br/>
    /// get - ������ ������ ������ �� Ÿ���� ��ȯ�ϸ�, Ʋ���� BuildingType.None�� ��ȯ�մϴ�.<br/><br/>
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
    /// �����ۿ� ����� STransform�� ��ȯ�մϴ�.  (�ش� ������ : ����, �Ǽ�)<br/><br/>
    /// get - ItemEquip�� ����ϴ� ����, ����Ʈ �������� ��� EquipTr��, ItemBuilding�� ��� WorldTr��, �̿��� Ÿ���� null�� ��ȯ�մϴ�.
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
    /// �������� ĳ���Ϳ� ������ �ִ� �������ͽ� ��ġ�� ��ȯ�մϴ�.(�ش� ������ : ����)<br/><br/>
    /// get - ItemFood�� ��� ����Ǿ��ִ� ItemStatus �ν��Ͻ���, �� ���� Ÿ���� default(ItemStatus)�� ��ȯ�մϴ�.
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
    /// ����� ���� ���¸� ��ȯ�մϴ�.<br/><br/>
    /// get - �������� ���� ���� ���̶�� true��, �������� �ƴϰų� ����Ұ����� �������� ��� false�� ��ȯ�մϴ�.<br/>
    /// set - �������� ���� ���¸� �����մϴ�.
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
    /// �ش� �������� ���� ���� ���� ������ ��ȯ�մϴ�.<br/><br/>
    /// get - ���� ������ �������� ��� �ش� EquipType�� ��ȯ, ���� �Ұ����� �������� ��� EquipType.None�� ��ȯ�մϴ�.
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
    /// ��� ���� ���� ������ �ε����� ��ȯ�մϴ�.<br/><br/>
    /// �������� ���� ���� ���̶�� ���� ������, �������� �ƴϰų� ����Ұ����� �������� ��� -1�� ��ȯ�մϴ�.
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










    // ������ ������ ������ �ε����� ���缭 ���ڿ��� ��ȯ�Ͽ� ǥ���մϴ� (WeaponType������ ���� �����ؾ� �մϴ�.)
    private readonly string[] weaponTypeString = { "Sword", "Spear", "Axe", "Blunt", "Bow", "Pickaxe", "Etc", "None" };
    


    /// <summary>
    /// ������ ������ ��ġ�� ���ڿ��� ��ȯ�Ͽ� ǥ���Ͽ� �ݴϴ�.<br/>
    /// ������ ������ ���� ǥ�õǴ� ���ڿ��� Ʋ���ϴ�.
    /// </summary>
    /// <returns>������ ������ ��ġ�� ���ڿ��� ��ȯ</returns>
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
