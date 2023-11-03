using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

/*
 * [�۾� ����]
 * 
 * <v1.0 - 2023_1101_�ֿ���>
 * 1 - �����ۼ� �� �׽�Ʈ �Ϸ�
 *  
 * <v1.1 - 2023_1102_�ֿ���>  
 * 1 - ���ϸ��� Item���� ItemData�� �����Ͽ���. (���� Ŭ������ �����ϱ� ����)
 * 
 * 2 - ItemŬ������ ItemType�� ��з� �׸����� ����.
 * ��ӹ��� Ŭ������ �ش�Ŭ������ �°� �ߺз� Type�� �������� �Ͽ���.
 * ���� Ŭ������ �ߺз� Type ���� �� �����ڵ� ����.
 * 
 * 3 - ������ ����ȭ
 * �������� ������ �̸� ����� ���� �� 
 * �̹����� ���� �ʴ� �����ڳ�, ����Ʈ �����ڴ� ������� ���� �����̹Ƿ�.
 * ��, ��� �����Ϳ� �̹������� �ְ� �������� ����.
 * 
 * 4 - Item Ŭ������ ICloneable �������̽� �߻����·� ���� �� �� �ڽ� Ŭ�������� ���� �����ϵ��� ����.
 * ������ ������ �������� ���Ӱ� �����ϴ� ���̹Ƿ�, �Ϲ����� ����������δ� ��ü�� �̸� ������ �������� �Ѵ�.
 * ������ ���� ���� ������ ���ο� ��ü�� �Ҵ���� �� �ְ� �Ѵ�.
 * 
 * <v2.0 - 2023_1103_�ֿ���>
 * 1 - �������� ���� �����ִ� �κ��丮�� ���� �ε����� ������ ������ �ֵ��� �Ͽ���.
 * �κ��丮 ����Ʈ�� �������� ���� �� ���������� �־ �����ϸ�, 
 * �κ��丮���� �������� ������ ������ ��, ���� ������ �ε��� ����Ʈ�� �����ϴ� �ͺ��� �� ���� ���� ���� ����.
 *  
 * 2 - ItemDebugInfo �޼��� �߰�. ȣ�� �� ������ ������ ����׻����� ǥ��
 * 
 */

namespace ItemData
{
    public enum ItemType { Misc, Weapon };
    public enum MiscType { Basic, Additive, Fire, Attribute, Engraving, Etc } // �⺻���, �߰����, ����, �Ӽ���, ���μ�, ��Ÿ
    public enum WeaponType { Sword, Blade, Spear, Dagger, Thin, Axe, Blunt, Bow, Crossbow, Claw, Whip } //��,��,â,�ܰ�,����,Ȱ,����,Ŭ��,ä��

    
    /// <summary>
    /// �⺻ ������ �߻� Ŭ���� - �ν��Ͻ��� �������� ���մϴ�. �ݵ�� ����Ͽ� ����ϼ���. ����� Ŭ������ ICloneable�� �����ؾ��մϴ�.
    /// </summary>
    public abstract class Item : ICloneable
    {
        protected ItemType enumType;        // Ÿ��
        protected string strNo;             // ��ȣ
        protected string strName;           // �̸�
        protected float fPrice;             // ����   
        protected ImageCollection icImage;  // �������� �κ��丮 ���� �̹���, ����â �̹��� ���� �����Ѵ�.
        protected int slotIndex;            // �������� �ڽ��� �����ִ� �κ��丮�� ���� �ε��� ������ ������.
                
        public ItemType Type { get { return enumType; } }
        public string Name { get { return strName; } }
        public float Price { get { return fPrice; } }
        
        /// <summary>
        /// �������� �̹����� ǥ���ϴ� ����ü�� ��� �����Դϴ�. 
        /// </summary>
        public ImageCollection Image { 
            get { return icImage;} 
            set { icImage = value; }
        }
        
        /// <summary>
        /// �ش� �������� ��� ���� �ε��� �����Դϴ�. �������� ������ �̵��� �� ���� �� ������ �����ؾ� �մϴ�.
        /// </summary>
        public int SlotIndex 
        { 
            set { slotIndex = value; }
            get { return slotIndex; } 
        }

        public Item( ItemType type, string No, string name, float price, ImageCollection image ) 
        {
            enumType = type;
            strName = name;
            strNo = No;
            fPrice = price; 
            icImage = image;
        }

        public abstract object Clone();
        public void ItemDeubgInfo()
        {
            Debug.Log("Type : " + enumType);
            Debug.Log("No : " + strNo);
            Debug.Log("Name : " + strName);
            Debug.Log("Price : " + fPrice);
            Debug.Log("SlotIndex : " + slotIndex);
            Debug.Log("ImageDesc : " + icImage.itemDesc);
        }
    }


    /// <summary>
    /// ��ȭ ������ - �⺻ �����۰� �ٸ����� �κ��丮�� �ߺ��ؼ� ���� �� �ִٴ� �� (count�� ����)
    /// </summary>
    public sealed class ItemMisc : Item
    {        
        private int inventoryCount;  // �κ��丮 ��ø Ƚ��

        /// <summary>
        /// ��ȭ �������� ��øȽ���� ǥ���մϴ�. �ʱ� ��ųʸ����� 0�� ��������� �ݵ�� ������ �����ϰų� �������ּ���.
        /// </summary>
        public int InventoryCount { 
            set { inventoryCount = value; }
            get { return inventoryCount; }
        }

        private MiscType enumMiscType;

        /// <summary>
        /// ��ȭ������ �Һз� Ÿ�� - Basic, Additive, Fire ���� �ֽ��ϴ�.
        /// </summary>
        public MiscType EnumMiscType { get { return enumMiscType; } }

        public ItemMisc( ItemType mainType, MiscType subType, string No, string name, float price, ImageCollection image )
            : base( mainType, No, name, price, image ) 
        { 
            inventoryCount = 0;
            enumMiscType = subType;
        }

        /// <summary>
        /// ��ȭ �������� ��ü�� ���� �����Ͽ� �ݴϴ�.
        /// </summary>
        public override object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>
    /// ���� ������
    /// </summary>
    public class ItemWeapon : Item
    {
        protected WeaponType enumWeaponType;    // ���� �Һз� Ÿ��
        /// <summary>
        /// ���� ������ �Һз� Ÿ�� - Sword, Blade, Spear, Dagger ���� �ֽ��ϴ�.
        /// </summary>
        public WeaponType EnumWeaponType { get { return enumWeaponType; } }

        public ItemWeapon(ItemType mainType, WeaponType subType, string No, string name, float price, ImageCollection image )
            : base( mainType, No, name, price, image ) 
        {
            enumWeaponType = subType;
        }

        /// <summary>
        /// ���� �������� ��ü�� ���� �����Ͽ� �ݴϴ�.
        /// </summary>
        public override object Clone()
        {
            return this.MemberwiseClone();
        }
    }


    /// <summary>
    /// ���� ���õ� ���� �Ӽ����� ��Ƴ��� Ŭ�����μ� �÷��̾ �������� �����ϰ� �Ǵ� ��� ������ �־�� �� ��Ͽ� ���Ե� �����Դϴ�.
    /// </summary>
    public class CraftProficiency
    {        
        private int proficiency;
        /// <summary>
        /// ����� ���� ���õ��� ���
        /// </summary>
        public int Proficiency
        {
            set {  proficiency = Mathf.Clamp(value, 0, 100); }    
            get { return proficiency; }
        }

        private int recipieHitCount;
        /// <summary>
        /// �����ǰ� ��Ȯ�ϰ� ���� Ƚ��
        /// </summary>
        public int RecipieHitCount 
        { 
            get { return recipieHitCount; } 
            set { recipieHitCount = value; }
        }

        /// <summary>
        /// ����� ��� ���� 0���� �ʱ�ȭ�Ͽ� �����մϴ�.
        /// </summary>
        public CraftProficiency()
        {
            proficiency = 0;
            RecipieHitCount = 0;
        }
    }



}
