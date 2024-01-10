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
 * <v6.4 - 2024_0108_�ֿ���>
 * 1- ItemImageCollection�� ItemVisualCollection��Ī ��������
 * �������� �Ű������� imgRefIndex�� visualRefIndex ����
 * 
 * <v7.0 - 2024_0111_�ֿ���>
 * 1- ũ������ �帣�� �°� Ŭ���� ���� ����
 * 2- �� �з� Ÿ�� �� ����Ʈ ������ ��� Ŭ���� �߰�
 * 
 */


namespace ItemData
{   
    
    /// <summary>
    /// ��ȭ �������� �� �з�
    /// </summary>
    public enum MiscType { Basic, Essential, Craft, Building, Tool, Potion } // �⺻, �ʼ�, ����, �Ǽ�, ����, ����
        

    /// <summary>
    /// ����Ʈ ������ - ��ȭ �������� ����մϴ�
    /// </summary>
    [Serializable]
    public class ItemQuest : ItemMisc
    {
        public ItemQuest( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, MiscType subType, string desc )
            : base( mainType, No, name, visualRefIndex, subType, desc ) 
        { 
            
        }
    }


    /// <summary>
    /// ��ȭ ������ - �⺻ �����۰� �ٸ����� �κ��丮�� �ߺ��ؼ� ���� �� �ִٴ� �� (count�� ����)
    /// </summary>
    [Serializable]
    public class ItemMisc : Item
    {        
        [JsonProperty] private int iOverlapCount = 0;   // �κ��丮 ��ø Ƚ��        
        [JsonProperty] MiscType eMiscType;              // ����Ÿ�� (��ȭ �Һз� Ÿ��)

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


        public ItemMisc( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, MiscType subType, string desc )
            : base( mainType, No, name, visualRefIndex, desc )
        { 
            iOverlapCount = 1;
            eMiscType = subType;            
        }

        
    }



    







}
