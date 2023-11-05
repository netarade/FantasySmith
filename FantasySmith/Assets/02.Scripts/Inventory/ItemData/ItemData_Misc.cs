using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * [�۾� ����]
 * 
 * <v1.0 - 2023_1105_�ֿ���>
 * 1- ItemMisc Ŭ���� ���� ����ȭ - ������Ƽ ó��
 * 2- ������ ��Ÿ���� ����ü ItemEngraving �߰�
 * 3- ItemMisc Ŭ���� ���ο� ItemEngraving�� FirePower�� ���� ������ �����ϵ��� �Ͽ���.
 * ������ ���� �� �Է� �� ���� Ÿ�԰� �̸��� ���� ������ �˾Ƽ� ����ֵ��� ����.
 * 
 */


namespace ItemData
{   
    
    /// <summary>
    /// ��ȭ �������� �� �з�
    /// </summary>
    public enum MiscType { Basic, Additive, Fire, Attribute, Engraving, Etc } // �⺻���, �߰����, ����, �Ӽ���, ���μ�, ��Ÿ
        
    /// <summary>
    /// ���μ��� ���� - ����,����,����,���,����
    /// </summary>
    public enum StatType { Power, Speed, Leech, Range, Splash }
    

    /// <summary>
    /// ��ȭ ������ - �⺻ �����۰� �ٸ����� �κ��丮�� �ߺ��ؼ� ���� �� �ִٴ� �� (count�� ����)
    /// </summary>
    public sealed class ItemMisc : Item
    {        
        private int iInventoryCount;  // �κ��丮 ��ø Ƚ��

        /// <summary>
        /// ��ȭ �������� ��øȽ���� ǥ���մϴ�. �⺻ ���� 1�Դϴ�.<br/>
        /// **�κ��丮 ������ 99�� �ʰ��ϸ� ���ܸ� �߻���ŵ�ϴ�.**
        /// </summary>
        public int InventoryCount { 
            set { 
                    iInventoryCount = value; 

                    if(iInventoryCount>99)
                        throw new Exception("�κ��丮 ������ 99�� �ʰ��Ͽ����ϴ�.");                    
                }
            get { return iInventoryCount; }
        }

        /// <summary>
        /// ��ȭ������ �Һз� Ÿ�� - Basic, Additive, Fire ���� �ֽ��ϴ�.
        /// </summary>
        public MiscType EnumMiscType { get; }

        /// <summary>
        /// ��ȭ �������� ������ �����̶�� engraveInfo�� �����ϴ�. �� ������ �����Ͽ� �߰� ������ Ȯ�ΰ����մϴ�.
        /// </summary>
        public ItemEngraving EngraveInfo {get;}

        /// <summary>
        /// ��ȭ �������� ������ ������ ���� ������ ȭ���� �����ϴ�. ���ᰡ �ƴ϶�� 0�Դϴ�.
        /// </summary>
        public int FirePower {get;}

        public ItemMisc( ItemType mainType, MiscType subType, string No, string name, float price, ImageCollection image )
            : base( mainType, No, name, price, image ) 
        { 
            iInventoryCount = 1;
            EnumMiscType = subType;
            FirePower = 0;

            if(subType == MiscType.Engraving)           // ���μ��̶�� �̸��� �״�� �־ ����ü ������ ������ �մϴ�.
                EngraveInfo = new ItemEngraving(name); 
            else if( subType == MiscType.Fire )         // ������ ȭ���� �����մϴ�.
            {
                switch(name)
                {
                    case "����" :
                        FirePower = 1;
                        break;
                    case "��ź" :
                        FirePower = 5;
                        break;
                    case "����" :
                        FirePower = 20;
                        break;
                }
            }
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
    /// ���μ� - ���⸦ ���ν�ų �� ���
    /// </summary>
    public struct ItemEngraving             
    {
        private float fIncreaseValue;        // ���� ��ġ
        private float fMultiplier;           // ���� ����
        
        /// <summary>
        /// ������ ��ü �̸� (ex. �ʱ� ������ ����)
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// ���� ��� - �ʱ�,�߱�,���
        /// </summary>
        public ItemGrade Grade { get; }              

        /// <summary>
        /// ���� ���� - ����,����,����,���,����
        /// </summary>
        public StatType StatusType { get; }
        
        /// <summary>
        /// ���� �� �ش� ���� ���� ���� ��ġ
        /// </summary>
        public float LastIncrVal { get; }           


        /// <summary>
        /// ����â�� ǥ�� �� ����
        /// </summary>
        public string Desc { get; }                 

        /// <summary>
        /// ���� ��� �� ��� �������� ���� ���� ������ 1.1f, 1.15f, 1.2f�� ���� �����ϴ�.
        /// </summary>
        public float PerformMult { get; }               
        


        /// <summary>
        /// ���ϴ� ���� �̸��� �����Ͽ� �����ϸ� ��� ������ ������ ���̺� �°� �ʱ�ȭ�Ͽ� �ݴϴ�.<br/>
        /// * ���� �� ��ġ�ϴ� �̸��� �ƴ϶�� ���ܸ� �����ϴ�. *
        /// </summary>
        public ItemEngraving(string fullName)
        {
            Name = fullName;
            string strGrade = fullName.Split(" ")[0];                   // Ǯ������ ù�κ�
            string strType = fullName.Split(" ")[1].Substring(0, 2);    // Ǯ������ ��° �κп��� '��'�� ������ �� �α���
            
            switch( strGrade )
            {
                case "�ʱ�" :
                    Grade = ItemGrade.Low;
                    fMultiplier = 1;
                    PerformMult = 1.1f;
                    break;
                
                case "�߱�":
                    Grade = ItemGrade.Medium;
                    fMultiplier = 2;
                    PerformMult = 1.15f;
                    break;
                case "���":
                    Grade = ItemGrade.High;
                    fMultiplier = 3;
                    PerformMult = 1.2f;
                    break;
                    
                default :
                    throw new Exception("���μ��� �̸��� ��ġ�ϴ� ����� �����ϴ�. �ʱ�,�߱�,���");
            }

            switch(strType)
            {
                case "����" :
                    StatusType = StatType.Power;
                    fIncreaseValue = 10f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"���� ���� {LastIncrVal} ����" );
                    break;
                case "����" :
                    StatusType = StatType.Speed;
                    fIncreaseValue = 0.1f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"���� �ӵ� {LastIncrVal}% ����" );
                    break;
                case "����" :
                    StatusType = StatType.Leech;
                    fIncreaseValue = 0.1f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"������ {LastIncrVal}% ����" );
                    break;
                case "���" :
                    StatusType = StatType.Range;
                    fIncreaseValue = 0.15f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"���� ��Ÿ� {LastIncrVal}% ����" );
                    break;
                case "����" :
                    StatusType = StatType.Splash;
                    fIncreaseValue = 0.1f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"���� ���� {LastIncrVal}% ����" );
                    break;
                    
                default :
                    throw new Exception("���μ��� �̸��� ��ġ�ϴ� ������ �����ϴ�. ����,����,����,���,����");
            }

        }
    }







}
