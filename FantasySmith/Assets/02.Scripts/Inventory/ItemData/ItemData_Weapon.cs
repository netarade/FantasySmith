using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * [�۾� ����]
 * 
 * <v1.0 - 2023_1105_�ֿ���>
 * 1- �Ϲݹ���(ItemWeapon) Ŭ���� ���� �и�.
 * 2- �� �� �⺻ ���� �� ������Ƽ, �����ڿ� �޼��� ����
 * 3- �ּ�ó��
 * 
 * <v1.1 - 2023_1106_�ֿ���>
 * 1- System �ڷ����� ��ġ�� ����� Attribute �� EnumAttribute�� ����
 */
namespace ItemData
{    
    /// <summary>
    /// ���� �������� �� �з�
    /// </summary>
    public enum WeaponType { Sword, Blade, Spear, Dagger, Thin, Axe, Blunt, Bow, Crossbow, Claw, Whip } //��,��,â,�ܰ�,����,Ȱ,����,Ŭ��,ä��

    /// <summary>
    /// 6�� �Ӽ� - ��,��,��,ȭ,ǳ,��
    /// </summary>
    public enum EnumAttribute { Water, Gold, Earth, Fire, Wind, None }
   
    /// <summary>
    /// ����� ���� ��� - �븻, ����, ����, ����, ����ũ, ������
    /// </summary>
    public enum Rarity { Normal, Magic, Rare, Epic, Unique, Legend }
    


    /// <summary>
    /// �Ϲ� ���� ������
    /// </summary>
    public class ItemWeapon : Item
    {       
        /*** �⺻ ���� ���� ***/
        protected WeaponType enumWeaponType;    // ���� �Һз� Ÿ��
        protected ItemGrade enumBasicGrade;     // �⺻ �з� ��� (�ʱ�, �߱�, ���)
        protected int iPower;                   // ���ݷ�
        protected int iDurability;              // ����
        protected float fSpeed;                 // ���ݼӵ�
        protected int iWeight;                  // ����
        protected EnumAttribute enumAttribute;      // ���� �Ӽ�


        /** ������ ���׷��̵� ���� **/
        protected float iEnhanceNum;                    // ���� ��ȭ Ƚ��
        protected bool isAttrUnlocked;                  // �Ӽ� �ع� ����       
        protected int iRemainEngraveNum;                   // ���� ���� Ƚ�� (���� �з� ��޿� ���� ����)
        protected ItemEngraving[] ieArrEquipEngrave;    // ���� ���� ����
        protected float fBasePerformance;               // ��� �⺻ ���� (�Ϲ� ����-100, ���۹���-���ۿ����� ��ȭ)
                    
        
        /// <summary>
        /// ���� ���� Ƚ���� �ڵ����� �����ؼ� ��ȯ�մϴ�.
        /// </summary>
        public int RemainEngraveNum { get{ return iRemainEngraveNum; } }
        
        /// <summary>
        /// ���� �������� ���� �迭�Դϴ�.
        /// </summary>
        public ItemEngraving[] EquipEngrave { get{ return ieArrEquipEngrave;} }

        /// <summary>
        /// ���⿡ ������ �����ϴ� �޼��� �Դϴ�. �����ϰ��� �� ���� �̸��� �ʿ��ϸ�, �����ִ� ���� ������ ���ٸ� false�� ��ȯ�մϴ�.
        /// </summary>
        public bool Engrave(string engravingName)
        {
            if(iRemainEngraveNum==0)
                return false;

            EquipEngrave[3-iRemainEngraveNum] = new ItemEngraving(engravingName);
            iRemainEngraveNum--;
            return true;
        }




        /// <summary>
        /// �������� �⺻ �����Դϴ�. �⺻���� 100�̸�, ���۹����� ��� �� ���ɿ� ������ �־�� �մϴ�.
        /// </summary>
        public float BasePerformance { get{ return fBasePerformance; } set{ fBasePerformance=value; } }

        /// <summary>
        /// �������� ���� ������ �ڵ����� �����Ͽ� ��ȯ�մϴ�. �⺻ ���ɿ� �Ӽ�(1.5f)�� ����(1.1f,1.15f,1.2f)�� ���� ���� �������� ���� ��ġ�Դϴ�.
        /// </summary>
        public float LastPerformance {
            get{ 
                float iLastMult = fBasePerformance; // �⺻���ɿ���
                float attributeMult = isAttrUnlocked? 1f : 1.5f;
                
                iLastMult *= attributeMult;         // �Ӽ����� ������ ���Ѵ�.

                for(int i=0; i<ieArrEquipEngrave.Length-RemainEngraveNum; i++)
                    iLastMult *= ieArrEquipEngrave[i].PerformMult;  // �� ���μ��� ������ ���Ѵ�.

                return iLastMult;                 
               }            
         }
        
        /// <summary>
        /// ����� ���� ���ɿ� ���� ���� ����Դϴ�. ( �븻, ����, ����, ����, ����ũ, ������ )<br/>
        /// </summary>
        public Rarity LastGrade
        {
            get{                
                if( LastPerformance<100)
                    Debug.Log("������ġ�� 100�̸� �Դϴ�. Ȯ���Ͽ� �ּ���.");

                if( LastPerformance>=100 )
                    return Rarity.Normal;
                else if( LastPerformance>300 )
                    return Rarity.Magic;
                else if( LastPerformance>500 )
                    return Rarity.Rare;
                else if( LastPerformance>800 )
                    return Rarity.Epic;
                else if( LastPerformance>1300 )
                    return Rarity.Unique;
                else
                    return Rarity.Legend;
            }
        }



        /// <summary>
        /// ���� ������ �Һз� Ÿ�� - Sword, Blade, Spear, Dagger ���� �ֽ��ϴ�.
        /// </summary>
        public WeaponType EnumWeaponType { get {return enumWeaponType;} }
        /// <summary>
        /// ���� �������� �⺻ �з� Ÿ�� - �ʱ�, �߱�, ���
        /// </summary>
        public ItemGrade BasicGrade { get {return enumBasicGrade;} }

        /// <summary>
        /// ���ݷ��� ���������� ������� 1:1���踦 �����ϴ�.
        /// </summary>
        public int Power { get{return (int)Mathf.Round(iPower*LastPerformance/100f);} }

        /// <summary>
        /// �������� ���������� ������� 1:0.5���踦 �����ϴ�. 
        /// </summary>
        public int Durability { get{return (int)Mathf.Round(iPower*LastPerformance/200f);} }
        public float Speed { get{return fSpeed;} }
        public int Weight { get{return iWeight;} }  

        /// <summary>
        /// ������ ���� �Ӽ��Դϴ�. ���� ���ο� ����� �����ϴ�.
        /// </summary>
        public EnumAttribute OriAttribute { get{return enumAttribute;} }  

        /// <summary>
        /// ���� ������ �Ӽ��Դϴ�. ������� �ʾҴٸ� ���Ӽ��Դϴ�.
        /// </summary>
        public EnumAttribute CurAttribute { 
            get{ 
                if(isAttrUnlocked)
                    return EnumAttribute.None;
                else
                    return enumAttribute;                
                }             
        }
                
        /// <summary>
        /// ����� �Ӽ����� ���θ� ���ų� �����մϴ�.
        /// </summary>
        public bool IsAttrUnlocked { get{ return isAttrUnlocked; } set{ isAttrUnlocked=value; } }
                
        /// <summary>
        /// ��� �������� ���� ��ȭ Ƚ���Դϴ�. �÷��̾��� ��ȣ�ۿ뿡 ���� ������ �����մϴ�.
        /// </summary>
        public float EnhanceNum { 
            get{return iEnhanceNum;}
            set{
                    if( 0<=iEnhanceNum && iEnhanceNum<=9 )  //��ȭ�� ���� ����� 0���� 9����
                        iEnhanceNum = value; 
               }
        }



        public ItemWeapon(ItemType mainType, WeaponType subType, string No, string name, float price, ImageCollection image   // ������ �⺻ ���� 
            ,ItemGrade basicGrade, int power, int durability, float speed, int weight, EnumAttribute attribute                    // ���� ���� ����
        ) : base( mainType, No, name, price, image )
        {
            enumWeaponType = subType;
            enumBasicGrade = basicGrade;
            iPower = power;
            iDurability = durability;
            fSpeed = speed;
            iWeight = weight;
            enumAttribute = attribute;

            iEnhanceNum = 0;                                            // �⺻���� ��ȭ �Ǿ����� ����.
            isAttrUnlocked = false;                                     // �⺻���� �Ӽ��� ����Ǿ� ���� ����.
            iRemainEngraveNum = (int)basicGrade + 1;                    // ���� ����Ƚ�� (�ʱ�:1, �߱�:2, ���:3)
            ieArrEquipEngrave = new ItemEngraving[iRemainEngraveNum];   // ���� �迭�� ũ��� ���� ���� Ƚ���� ����(�⺻ ��� �� ����)
            fBasePerformance = 100f;                                    // ���۾������� �ƴ� ��� �⺻ ���� 100f
        }


        /// <summary>
        /// ���� �������� ��ü�� ���� �����Ͽ� �ݴϴ�.
        /// </summary>
        public override object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
