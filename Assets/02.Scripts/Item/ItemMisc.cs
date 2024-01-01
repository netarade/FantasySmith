using Newtonsoft.Json;
using System;

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
 * 
 * <v5.1 - 2023_1224_�ֿ���>
 * 1- Clone�޼��� ���� (ItemŬ�������� ���� ����� ����ϹǷ�)
 * 
 * <v6.0 - 2023_1226_�ֿ���>
 * 1- OverlapCount ������Ƽ�� ��ø������ ��������(+=)�ϴ� �Ϳ��� ���Կ���(=)���� ����
 * ���̺��ϰ� �ε�� ������ȭ �޼��尡 ȣ��Ǹ鼭 ������ �־��� �� �ش� ������Ƽ�� ȣ���� �̷�� ���µ� 
 * �̶� ���� iOverlapCount�� ���� ���� �ٽ� �����ع����� ������ 2�谡 �Ǿ������ ������ �߻��ϱ� ����
 * 
 * 2- �ڵ�����������Ƽ ������ ���κ��� �ϳ� �� ����� �Ϲ�������Ƽ�� ���� �� JsonProperty�� JsonIgnoreó��
 * (������Ƽ�� ������� ���� �� �ε�� ������Ƽ�� set�� ���� ������ ������ �ݿ����� �ʱ� ����)
 * 
 * <v6.1 - 2023_1229_�ֿ���>
 * 1- ���ϸ� ���� ItemData_Misc->ItemMisc
 * 
 * <v6.2 - 2023_1230_�ֿ���>
 * 1- MaxCount�� MaxOverlapCount�� �̸�����
 * 
 * <v6.3 - 2023_1231_�ֿ���>
 * 1- OverlapCount������Ƽ�� �б����� ������ �����ϰ� ������ ������ �� SetOverlapCount �޼��带 ȣ���ϵ��� ����
 * 2- SetOverlapCount�޼��带 �������ڸ� �޵��� ����, �Ű����� count�� inCount�� ����
 * 
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
        [JsonProperty] private int iOverlapCount = 0;   // �κ��丮 ��ø Ƚ��        
        [JsonProperty] MiscType eMiscType;              // ����Ÿ�� (��ȭ �Һз� Ÿ��)
        [JsonProperty] ItemEngraving eEngraveInfo;      // ���� ����
        [JsonProperty] AttributeType eAttributeType;    // �Ӽ��� ����
        [JsonProperty] public int iFirePower;           // ȭ��

        [JsonIgnore] public readonly int MaxOverlapCount = 99; // ��ȭ ������ �ִ� ����


        /// <summary>
        /// ��ȭ �������� ��ø ������ �˷��ݴϴ�. <br/>
        /// �б������̹Ƿ�, ������ �����ϱ� ���ؼ��� SetOverlapCount�޼��带 ȣ���Ͽ��� �մϴ�. <br/>
        /// </summary>
        [JsonIgnore] public int OverlapCount { get { return iOverlapCount; } }

        /// <summary>
        /// ��ȭ �������� ��øȽ���� �����մϴ�.<br/>
        /// ���� ���ڰ� ���޵Ǹ� ������ ������ �����ϰ�, ���� ���ڰ� ���޵Ǹ� ������ �����մϴ�.<br/><br/>
        /// 
        /// ���� ���ڰ� ���޵Ǿ��� �� �ִ���� �̻��� �ʰ��ϴ� ��� �ش� �ʰ� ������ ��ȯ�Ͽ� �ݴϴ�<br/>
        /// ���� ���ڰ� ���޵Ǿ��� �� ���� �� ������ �� ���� ���(���� ������ 0�̵� ���) ������ �ʰ� ������ ��ȯ�մϴ�.<br/>
        /// </summary>
        /// <param name="inCount"></param>
        /// <returns>���ڷ� ���޵� ������ �������� ���� �� �ִ� �ִ�, �ּ� ������ �ʰ��ϴ� ��� ���� ���� ���ڸ� ��ȯ�մϴ�.</returns>
        public int SetOverlapCount(int inCount)
        {
            int remainCount;

            if( inCount>0 )     // ���� ���ڷ� ���� ���� ���� ���
            {
                // ��ȯ�Ǵ� ���� ���� : ���� ����+���� ����-�ִ����
                remainCount = iOverlapCount + inCount - MaxOverlapCount; 

                if( remainCount > 0 )                   // ���¼����� ���������� ������ �� �ִ� ������ �ʰ��Ѵٸ�
                {
                    iOverlapCount = MaxOverlapCount;    // ���� �������� ������ �ִ�������� �����ݴϴ�
                    return remainCount;                 // ������ ������ ��ȯ�մϴ�.
                }
                else if(remainCount<=0)                 // ��ȯ�� �������� ���ٸ�
                {
                    iOverlapCount += inCount;           // ���������� ���� ������ �����ݴϴ�
                    return 0;                           // 0�� ��ȯ�մϴ�
                }
            }
            else if( inCount<0 )    // ���� ���ڷ� ���� ���� ���� ���
            {
                // ��ȯ�Ǵ� ���� ���� : �������� - ������
                remainCount = iOverlapCount + inCount;

                if( remainCount>=0 )            // ��ȯ�� �������� ���� ���
                {
                    iOverlapCount += inCount;   // ���������� ���� ������ ���ݴϴ�.
                    return 0;                   // 0�� ��ȯ�մϴ�.
                }
                else if( remainCount<0 )        // ��ȯ�� �������� �ִ� ���
                {
                    iOverlapCount = 0;          // ���������� 0���� ������ݴϴ�.
                    return remainCount;         // ������ ������ ��ȯ�մϴ�.
                }
            }
            
            return 0;
        }



        
        /// <summary>
        /// ��ȭ������ �Һз� Ÿ�� - Basic, Additive, Fire ���� �ֽ��ϴ�.
        /// </summary>
        [JsonIgnore] public MiscType MiscType { get{ return eMiscType; } }


        /// <summary>
        /// ��ȭ �������� ������ �����̶�� engraveInfo�� �����ϴ�. �� ������ �����Ͽ� �߰� ������ Ȯ�ΰ����մϴ�.
        /// </summary>
        [JsonIgnore] public ItemEngraving EngraveInfo { get{ return eEngraveInfo; } }

        /// <summary>
        /// ��ȭ �������� ������ �Ӽ����̶�� eAttributeType�� �����ϴ�.
        /// </summary>
        [JsonIgnore] public AttributeType AttributeType { get{ return eAttributeType; } }


        /// <summary>
        /// ��ȭ �������� ������ ������ ���� ������ ȭ���� �����ϴ�. ���ᰡ �ƴ϶�� 0�Դϴ�.
        /// </summary>
        [JsonIgnore] public int FirePower { get{ return iFirePower; } }

        public ItemMisc( ItemType mainType, MiscType subType, string No, string name, float price, ImageReferenceIndex imgRefIdx )
            : base( mainType, No, name, price, imgRefIdx ) 
        { 
            iOverlapCount = 1;
            eMiscType = subType;
            iFirePower = 0;

            if(subType == MiscType.Engraving)           // ���μ��̶�� �̸��� ����â �̹��� �ε����� �־ ����ü ������ ������ �մϴ�.
                eEngraveInfo = new ItemEngraving(name, imgRefIdx.statusImgIdx); 
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
                        iFirePower = 1;
                        break;
                    case "��ź" :
                        iFirePower = 5;
                        break;
                    case "����" :
                        iFirePower = 10;
                        break;
                }
            }
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
        
        [JsonProperty] string sName;                        // �̸�
        [JsonProperty] ItemGrade eGrade;                    // ���
        [JsonProperty] StatType eStatusType;                // �����̻� Ÿ��
        [JsonProperty] float fLastIncrVal;                  // ���� ���� ��ġ
        [JsonProperty] string sDesc;                        // ����
        [JsonProperty] float fPerformMult;                  // ���� ���� ������
        [JsonProperty] int iStatusImageIdx;                 // ����â �̹��� �ε��� ��ȣ

        /// <summary>
        /// ������ ��ü �̸� (ex. �ʱ� ������ ����)
        /// </summary>
        [JsonIgnore] public string Name { get{ return sName;} }

        /// <summary>
        /// ���� ��� - �ʱ�,�߱�,���
        /// </summary>
        [JsonIgnore] public ItemGrade Grade { get{ return eGrade;} }              

        /// <summary>
        /// ���� ���� - ����,����,����,���,����
        /// </summary>
        [JsonIgnore] public StatType StatusType { get{ return eStatusType;} }
        
        /// <summary>
        /// ���� �� �ش� ���� ���� ���� ��ġ
        /// </summary>
        [JsonIgnore] public float LastIncrVal { get{ return fLastIncrVal;} }           


        /// <summary>
        /// ����â�� ǥ�� �� ����
        /// </summary>
        [JsonIgnore] public string Desc { get{ return sDesc;} }                 

        /// <summary>
        /// ���� ��� �� ��� �������� ���� ���� ������ 1.1f, 1.15f, 1.2f�� ���� �����ϴ�.
        /// </summary>
        [JsonIgnore] public float PerformMult { get{ return fPerformMult;} }               
        
        /// <summary>
        /// ������ ������ ����â �̹����� �ε��� ��ȣ
        /// </summary>
        [JsonIgnore] public int StatusImageIdx { get{ return iStatusImageIdx;} }


        /// <summary>
        /// ����â �̹��� �ε����� ���ڷ� �־� ���� ���� ����ü�� �����մϴ�.
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="statusImageIndex"></param>
        public ItemEngraving( string fullName, int statusImageIndex ) : this(fullName)            
        {
            iStatusImageIdx = statusImageIndex; // ����â �̹��� �ε��� ����                         
        }

        /// <summary>
        /// ���ϴ� ���� �̸��� �����Ͽ� �����ϸ� ��� ������ ������ ���̺� �°� �ʱ�ȭ�Ͽ� �ݴϴ�.<br/>
        /// * ���� �� ��ġ�ϴ� �̸��� �ƴ϶�� ���ܸ� �����ϴ�. *
        /// </summary>
        public ItemEngraving(string fullName)
        {            
            sName = fullName;
            string strGrade = fullName.Split(" ")[0];                   // Ǯ������ ù�κ�
            string strType = fullName.Split(" ")[1].Substring(0, 2);    // Ǯ������ ��° �κп��� '��'�� ������ �� �α���
                        
            switch( strGrade )
            {
                case "�ʱ�" :
                    eGrade = ItemGrade.Low;
                    fMultiplier = 1;
                    fPerformMult = 1.1f;
                    break;
                
                case "�߱�":
                    eGrade = ItemGrade.Medium;
                    fMultiplier = 2;
                    fPerformMult = 1.15f;
                    break;
                case "���":
                    eGrade = ItemGrade.High;
                    fMultiplier = 3;
                    fPerformMult = 1.2f;
                    break;
                    
                default :
                    throw new Exception("���μ��� �̸��� ��ġ�ϴ� ����� �����ϴ�. �ʱ�,�߱�,���");
            }

            switch(strType)
            {
                case "����" :
                    eStatusType = StatType.Power;
                    fIncreaseValue = 10f;
                    fLastIncrVal = fIncreaseValue * fMultiplier;
                    sDesc = string.Format( $"���� ���� {fLastIncrVal} ����" );
                    iStatusImageIdx = 0; // ����â �̹��� �ε��� ���� ����
                    break;
                case "����" :
                    eStatusType = StatType.Speed;
                    fIncreaseValue = 0.1f;
                    fLastIncrVal = fIncreaseValue * fMultiplier;
                    sDesc = string.Format( $"���� �ӵ� {fLastIncrVal}% ����" );
                    iStatusImageIdx = 1; // ����â �̹��� �ε��� ���� ����
                    break;
                case "����" :
                    eStatusType = StatType.Leech;
                    fIncreaseValue = 0.1f;
                    fLastIncrVal = fIncreaseValue * fMultiplier;
                    sDesc = string.Format( $"������ {fLastIncrVal}% ����" );
                    iStatusImageIdx = 2; // ����â �̹��� �ε��� ���� ����
                    break;
                case "���" :
                    eStatusType = StatType.Range;
                    fIncreaseValue = 0.15f;
                    fLastIncrVal = fIncreaseValue * fMultiplier;
                    sDesc = string.Format( $"���� ��Ÿ� {fLastIncrVal}% ����" );
                    iStatusImageIdx = 3; // ����â �̹��� �ε��� ���� ����
                    break;
                case "����" :
                    eStatusType = StatType.Splash;
                    fIncreaseValue = 0.1f;
                    fLastIncrVal = fIncreaseValue * fMultiplier;
                    sDesc = string.Format( $"���� ���� {fLastIncrVal}% ����" );
                    iStatusImageIdx = 4; // ����â �̹��� �ε��� ���� ����
                    break;
                    
                default :
                    throw new Exception("���μ��� �̸��� ��ġ�ϴ� ������ �����ϴ�. ����,����,����,���,����");
            }

        }
    }







}
