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
    /// ���� �� �Ǽ���� �������� �������Դϴ�. (�ش� ������ : ����, �Ǽ����)<br/><br/>
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
                itemWeapon.Durability=value;
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
                itemMisc.SetOverlapCount(value);    // ���������� �����մϴ�.
                UpdateTextInfo();   // �ؽ�Ʈ ������ �����մϴ�.
                CheckDestroyInfo(); // �ı� ���θ� üũ�մϴ�.
            }
            else
                throw new Exception( "�������� ������ ���� �ʽ��ϴ�." );
        }
    }








    // ������ ������ ������ �ε����� ���缭 ���ڿ��� ��ȯ�Ͽ� ǥ���մϴ� (WeaponType������ ���� �����ؾ� �մϴ�.)
    private readonly string[] weaponTypeString = { "��", "â", "����", "�б�", "Ȱ", "���", "��Ÿ", "����" };
    


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
            $"���� : {weaponTypeString[(int)(itemWeapon.WeaponType)]}\n" +
            $"���ݷ� : {itemWeapon.Power}\n" +
            $"������ : {itemWeapon.Durability}"
            );

        ItemQuest itemQuest = item as ItemQuest;
        if(itemQuest!=null)
            return string.Format(
            $"���� : ����Ʈ\n"
            );
        
        ItemBuilding itemBuilding = item as ItemBuilding;  
        if(itemBuilding!=null)
            return string.Format(
            $"���� : �Ǽ� ���\n" +
            $"���� : {itemBuilding.OverlapCount}" +
            $"������ : {itemBuilding.Durability}"
            );


        ItemMisc itemMisc = item as ItemMisc;
        if(itemMisc!=null)
            return string.Format(
            $"���� : ��ȭ\n" +
            $"���� : {itemMisc.OverlapCount}"
            );   
        
        return null;
    }



}
