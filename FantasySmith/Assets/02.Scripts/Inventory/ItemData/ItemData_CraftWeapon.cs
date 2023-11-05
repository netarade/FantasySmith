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
    }
        
    /// <summary>
    /// �������� ���� - ��, ��, ��, ��, ��, ��, ��
    /// </summary>
    public enum Recipie { Ga,Na,Da,Ra,Ma,Eu,Ee }


    
    /// <summary>
    /// ���� ���� ������
    /// </summary>
    public class ItemCraftWeapon : ItemWeapon
    {
        /*** ���� ���� �Ӽ� ***/
        private float fCraftChance;                 // ���� Ȯ�� (�⺻ ��޿� ���� ���� 90%, 60%, 25%)
        Recipie enumRecipie;                        // 2�ܰ� ������
        CraftMaterial[] cmArrBaseMaterial;          // ���� �� �ʿ��� �⺻ ���
        CraftMaterial[] cmArrAdditiveMaterial;      // ���� �� �ʿ��� �߰� ���
        protected int iFirePower;                   // 2�ܰ� ���ۿ� �ʿ��� ȭ��


        public ItemCraftWeapon(ItemType mainType, WeaponType subType, string No, string name, float price, ImageCollection image // ������ �⺻ ���� 
            , ItemGrade basicGrade, int power, int durability, float speed, int weight, Attribute attribute                      // ���� ���� ����
            , CraftMaterial[] baseMaterial, CraftMaterial[] additiveMaterial, Recipie recipie                                    // ���� ���� ����   
        ) : base( mainType, subType, No, name, price, image, basicGrade, power, durability, speed, weight, attribute )
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
