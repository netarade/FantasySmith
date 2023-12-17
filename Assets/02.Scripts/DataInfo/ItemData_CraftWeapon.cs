using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * [�۾� ����]
 * 
 * <v1.0 - 2023_1105_�ֿ���>
 * 1- ���۹���(ItemCraftWeapon) Ŭ���� ���� �и�.
 * 2- �� �� �⺻ ���� �� ������Ƽ, �����ڿ� �޼��� ����
 * 3- �ּ�ó��
 * 
 * <v1.1 - 2023_1106_�ֿ���>
 * 1- CraftMaterial ����ü ������ �߰�
 * 2- �� �Ӽ��� ������ �� �� �ִ� ������Ƽ �߰�
 * 3- ������Ƽ �� ���� 
 * 
 * <v1.2 - 2023_1116_�ֿ���>
 * 1- ������ Ŭ���� ���� �������� ���� �����ڳ����� �̹��� ���� ������ �̹��� �ε��� ������ ����
 */
namespace ItemData
{
    /// <summary>
    /// ���� ���ۿ� �ʿ��� ��� - ��� �̸��� ����
    /// </summary>
    public struct CraftMaterial
    {
        public string name;
        public int count;

        public CraftMaterial(string name, int count)
        {
            this.name = name;
            this.count = count;
        }
    }
        
    /// <summary>
    /// �������� ���� - ��, ��, ��, ��, ��, ��, ��
    /// </summary>
    public enum Recipie { Ga,Na,Da,Ra,Ma,Eu,Ee }


    
    /// <summary>
    /// ���� ���� ������
    /// </summary>
    public sealed class ItemCraftWeapon : ItemWeapon
    {
        /*** ���� ���� �Ӽ� ***/
        private float fCraftChance;                 // ���� Ȯ�� (�⺻ ��޿� ���� ���� 90%, 60%, 25%)
        private Recipie enumRecipie;                        // 2�ܰ� ������
        private CraftMaterial[] cmArrBaseMaterial;          // ���� �� �ʿ��� �⺻ ���
        private CraftMaterial[] cmArrAdditiveMaterial;      // ���� �� �ʿ��� �߰� ���
        private int iFirePower;                   // 2�ܰ� ���ۿ� �ʿ��� ȭ��

        /// <summary>
        /// ���� ������ �⺻ ���� Ȯ���Դϴ�.
        /// </summary>
        public float BaseCraftChance { get{return fCraftChance;} }

        /// <summary>
        /// ���� ������ ������ �Դϴ�.
        /// </summary>
        public Recipie EnumRecipie { get{return enumRecipie;} }
        
        /// <summary>
        /// ���� ������ �⺻ ����Դϴ�. 2�ܰ� ���ۿ� ���Ǹ�, ����ü �迭�� �����ϰ� �ֽ��ϴ�.
        /// </summary>
        public CraftMaterial[] BaseMaterials { get{return cmArrBaseMaterial;} }

        /// <summary>
        /// ���� ������ �߰� ����Դϴ�. 3�ܰ� ���ۿ� ���Ǹ�, ����ü �迭�� �����ϰ� �ֽ��ϴ�.
        /// </summary>
        public CraftMaterial[] AdditiveMaterals { get{return cmArrAdditiveMaterial;} }

        /// <summary>
        /// ���ۿ� �ʿ��� ȭ���Դϴ�. 2�ܰ迡 ���ۿ� ���˴ϴ�.
        /// </summary>
        public int FirePower { get{return iFirePower;} }


        public ItemCraftWeapon(ItemType mainType, WeaponType subType, string No, string name, float price, ImageReferenceIndex imgRefIdx // ������ �⺻ ���� 
            , ItemGrade basicGrade, int power, int durability, float speed, int weight, AttributeType attribute                         // ���� ���� ����
            , CraftMaterial[] baseMaterial, CraftMaterial[] additiveMaterial, Recipie recipie                                           // ���� ���� ����   
        ) : base( mainType, subType, No, name, price, imgRefIdx, basicGrade, power, durability, speed, weight, attribute )
        {          

            /*** ���� ���� ���� ***/
            switch(basicGrade)
            {
                case ItemGrade.Low :
                    fCraftChance = 0.9f;
                    break;
                case ItemGrade.Medium :
                    fCraftChance = 0.6f;
                    break;
                case ItemGrade.High :
                    fCraftChance = 0.25f;
                    break;
            }

            cmArrBaseMaterial = baseMaterial;
            cmArrAdditiveMaterial = additiveMaterial;
            enumRecipie = recipie;
            
            iFirePower = 0;
            for(int i=0; i<baseMaterial.Length; i++)
                iFirePower += baseMaterial[i].count;        // 2�ܰ� ȭ�� - �⺻ ����� ���� ���� ���� 
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
