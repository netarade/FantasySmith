using Newtonsoft.Json;
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
 * <v1.1 - 2023_1109_�ֿ���>
 * 1- ��ȭ���� �߰��ϱ� ���� MiscType�� Attribute�� Enhancement�� ����
 * 
 * <v2.0 - 2023_1112_�ֿ���>
 * 1- �κ��丮 ������ 99�� �ʰ��� ��� ���ܸ� �߻���Ű�� ���� �ƴ϶� ���ο� ���Կ� �����ǰ� �����Ͽ���.
 * �׿� ���� CreateManager�� Create�޼��嵵 ����
 * 
 * <v2.1 - 2023_1115_�ֿ���>
 * 1- ��ȭ �������� �ִ� ������ static Ŭ���� ���� MaxCount�� ���� �� �ּ� �� ���� ����
 * 
 * <v2.2 - 2023_1116_�ֿ���>
 * 1- ������ Ŭ���� ���� �������� ���� �����ڳ����� �̹��� ���� ������ �̹��� �ε��� ������ ����
 * 
 * <v3.0 - 2023_1216_�ֿ���>
 * 1- MiscType EnumMiscType �������� eMiscType���� ���� 
 * 
 * 2- MiscType�� Attribute �߰�. ������ �Ӽ��� �������� ������� �� �Ӽ������� �ְ� �ͱ� ����
 * 
 * 3- AttributeType eAttributeType ������ �߰��ϰ� �����ڿ��� �Ӽ��� ������ ���� ������ �б��ؼ� ������ �Ͽ���.
 * 
 * 4- ���μ� ���� ����ü���� ����â �̹��� �ε��� ������ �������� �ϰ�,
 * ������ �����ڿ��� ���μ��� �� �� ����â �̹��� �ε����� �߰��� �޵��� �����Ͽ��� 
 * (������ ����â�� ���� �� �� ���μ� ���� ����ü�� �ش� �̹��� ������ ���� ���� ������ �̸��� ���ؼ� ������ �̹����� ã�ƾ� �ϱ� ����
 * 
 * 5- ���μ� ���� ����ü�� �����ڸ� �̸��� �޴� �����ڿ� �̸��� ����â �̹��� �ε����� �޴� ������ �ѷ� ��������.
 * ������ ItemMisc������ ���� �̹��� �ε����� �־��� �� ������, ItemWeapon�� Engrave ��ɿ����� �̸��� �� �� �ֱ� ����
 * 
 * 
 * <v4.0 - 2023_1219_�ֿ���>
 * 1- ��ø������ ������Ƽ iInventoryCount�� InventoryCount�� iOverlapCount, OverlapCount�� �̸�����
 * 2- static�����̴� MaxCount�� readonly�� ����
 * 3- ������ �ʰ����� 
 * 
 * <v4.1 - 2023_1220_�ֿ���>
 * 1- ��ø������ �����ϴ� �޼��� SetOverlapCount �߰��Ͽ� ������ �Է� �� �ʰ������� ��ȯ�޵��� �Ͽ���
 * 
 * <v5.0 - 2023_1222_�ֿ���>
 * 1- private������ ����ȭ�ϱ� ���� [JsonProperty] ��Ʈ����Ʈ�� �߰��Ͽ���
 * 2- SetOverlapCount �޼��� ���� ���ǽ� ==0���� <=0���� ����
 */


namespace ItemData
{   
    
    /// <summary>
    /// ��ȭ �������� �� �з�
    /// </summary>
    public enum MiscType { Basic, Additive, Fire, Attribute, Enhancement, Engraving, Etc } // �⺻���, �߰����, ����, �Ӽ���, ��ȭ��, ���μ�, ��Ÿ
        
    /// <summary>
    /// ���μ��� ���� - ����,����,����,���,����
    /// </summary>
    public enum StatType { Power, Speed, Leech, Range, Splash }
    
    /// <summary>
    /// ��ȭ ������ - �⺻ �����۰� �ٸ����� �κ��丮�� �ߺ��ؼ� ���� �� �ִٴ� �� (count�� ����)
    /// </summary>
    [Serializable]
    public sealed class ItemMisc : Item
    {        
        [JsonProperty] private int iOverlapCount = 0;  // �κ��丮 ��ø Ƚ��
        public readonly int MaxCount = 99;
        
        /// <summary>
        /// ��ȭ �������� ��øȽ���� ǥ���մϴ�. �⺻ ���� 1�Դϴ�.<br/>
        /// **�ش� ������Ƽ�� ���� ��øȽ���� ������ �� ItemMisc.MaxCount�� �ʰ��ϸ� ���ܸ� �߻���ŵ�ϴ�.**
        /// SetOverlapCount�� ���� ���� ������ ��ȯ�޾ƾ� �մϴ�.
        /// </summary>
        public int OverlapCount { 
            set { 
                    iOverlapCount += value;         // ���� ������ �������� �ݴϴ�.

                    if(iOverlapCount > MaxCount)    // ��øȽ���� �ִ밹������ ũ�ٸ�
                        throw new Exception("�ִ� ������ �����Ͽ����ϴ�.");
                }
            get { return iOverlapCount; } //��ø������ ��ȯ�մϴ�
        }


        /// <summary>
        /// ��ȭ �������� ��øȽ���� �����մϴ�. ������ ���ڷ� ������ �ʰ������� ��ȯ�Ͽ� �ݴϴ�
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public int SetOverlapCount(int count)
        {
            int remainCount = iOverlapCount+count-MaxCount; // ��ȯ�Ǵ� ���� ���� : ���� ����+���� ����-�ִ����

            if( remainCount > 0 )                           // ���¼����� ���������� ������ �� �ִ� ������ �ʰ��Ѵٸ�
            {
                iOverlapCount=MaxCount;                     // ���� �������� ������ �ִ�������� �����ݴϴ�
                return remainCount;                         // �������� ��ȯ���ݴϴ�
            }
            else if(remainCount<=0)                         // ��ȯ�� �������� ���ٸ�
            {
                iOverlapCount += count;                     // ���������� ���� ������ �����ݴϴ�
            }

            return 0;                                       // 0�� ��ȯ�մϴ�
        }




        /// <summary>
        /// ��ȭ������ �Һз� Ÿ�� - Basic, Additive, Fire ���� �ֽ��ϴ�.
        /// </summary>
        public MiscType eMiscType { get; }

        /// <summary>
        /// ��ȭ �������� ������ �����̶�� engraveInfo�� �����ϴ�. �� ������ �����Ͽ� �߰� ������ Ȯ�ΰ����մϴ�.
        /// </summary>
        public ItemEngraving EngraveInfo {get;}

        /// <summary>
        /// ��ȭ �������� ������ �Ӽ����̶�� eAttributeType�� �����ϴ�.
        /// </summary>
        public AttributeType eAttributeType { get; }


        /// <summary>
        /// ��ȭ �������� ������ ������ ���� ������ ȭ���� �����ϴ�. ���ᰡ �ƴ϶�� 0�Դϴ�.
        /// </summary>
        public int FirePower {get;}

        public ItemMisc( ItemType mainType, MiscType subType, string No, string name, float price, ImageReferenceIndex imgRefIdx )
            : base( mainType, No, name, price, imgRefIdx ) 
        { 
            iOverlapCount = 1;
            eMiscType = subType;
            FirePower = 0;

            if(subType == MiscType.Engraving)           // ���μ��̶�� �̸��� ����â �̹��� �ε����� �־ ����ü ������ ������ �մϴ�.
                EngraveInfo = new ItemEngraving(name, imgRefIdx.statusImgIdx); 
            else if( subType == MiscType.Attribute )    // �Ӽ����̶�� '-'������ �̸��� �����Ͽ� ������ ������ �մϴ� 
            {
                switch( name.Split('-')[1] )
                {
                    case "��":
                        eAttributeType = AttributeType.Water;
                        break;
                    case "��":
                        eAttributeType = AttributeType.Gold;
                        break;                    
                    case "��":
                        eAttributeType = AttributeType.Earth; 
                        break;                        
                    case "ȭ":
                        eAttributeType = AttributeType.Fire; 
                        break;
                    case "ǳ":
                        eAttributeType = AttributeType.Wind; 
                        break;
                    default:
                        throw new Exception("�Ӽ����� �̸��� ����ġ �ʽ��ϴ�.");
                }
            }
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
                        FirePower = 10;
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
    [Serializable]
    public struct ItemEngraving             
    {
        [JsonProperty] private float fIncreaseValue;        // ���� ��ġ
        [JsonProperty] private float fMultiplier;           // ���� ����
        
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
        /// ������ ������ ����â �̹����� �ε��� ��ȣ
        /// </summary>
        public int StatusImageIdx {get;}


        /// <summary>
        /// ����â �̹��� �ε����� ���ڷ� �־� ���� ���� ����ü�� �����մϴ�.
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="statusImageIndex"></param>
        public ItemEngraving( string fullName, int statusImageIndex ) : this(fullName)            
        {
            StatusImageIdx = statusImageIndex; // ����â �̹��� �ε��� ����                         
        }

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
                    StatusImageIdx = 0; // ����â �̹��� �ε��� ���� ����
                    break;
                case "����" :
                    StatusType = StatType.Speed;
                    fIncreaseValue = 0.1f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"���� �ӵ� {LastIncrVal}% ����" );
                    StatusImageIdx = 1; // ����â �̹��� �ε��� ���� ����
                    break;
                case "����" :
                    StatusType = StatType.Leech;
                    fIncreaseValue = 0.1f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"������ {LastIncrVal}% ����" );
                    StatusImageIdx = 2; // ����â �̹��� �ε��� ���� ����
                    break;
                case "���" :
                    StatusType = StatType.Range;
                    fIncreaseValue = 0.15f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"���� ��Ÿ� {LastIncrVal}% ����" );
                    StatusImageIdx = 3; // ����â �̹��� �ε��� ���� ����
                    break;
                case "����" :
                    StatusType = StatType.Splash;
                    fIncreaseValue = 0.1f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"���� ���� {LastIncrVal}% ����" );
                    StatusImageIdx = 4; // ����â �̹��� �ε��� ���� ����
                    break;
                    
                default :
                    throw new Exception("���μ��� �̸��� ��ġ�ϴ� ������ �����ϴ�. ����,����,����,���,����");
            }

        }
    }







}
