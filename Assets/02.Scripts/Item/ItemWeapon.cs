using Newtonsoft.Json;
using System;
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
 * 1- System �ڷ����� ��ġ�� ����� Enhancement �� EnumAttribute�� ����
 * 
 * <v1.2 - 2023_1106_�ֿ���>
 * 1- ��ȭ Ƚ���� float�� �̾��� ���� int�� ����
 * 2- �Ӽ� ����� �ʱ⿡ false�� ���ִ� ���� true�� ����
 * 3- LastPerformance - ���������� ���� ���� ���� ǥ���ǵ��� if���� ������ ����
 * 
 * <v1.3 - 2023_1116_�ֿ���>
 * 1- ������ Ŭ���� ���� �������� ���� �����ڳ����� �̹��� ���� ������ �̹��� �ε��� ������ ����
 * 
 * <v2.0 - 2023_1216_�ֿ���>
 * 1- ���ι迭�� �ִ� ���� iMaxEngraveNum�� �߰��ϰ� �����ڿ��� �ִ� ������ �ʱ�ȭ
 * 2- ���� �������� ������ ������ ��ȯ�ϴ� EquipEngraveNum ������Ƽ �߰�
 * 3- Engrave�޼��忡 3-iRemainEngraveNum �ε����� �����ִ� ���� EquipEngraveNum�� ����. (�ε��� ���� ������ ����)
 * 4- Engrave�޼��忡 EquipEngraveArrInfo�� �����Ͽ� ���� ���� �ϴ� ���� ieArrEquipEngrave�� �����Ͽ� �����ϴ� ������ ����
 * 5- IsAttrUnlocked ������Ƽ�� �Ӽ� ���濩�θ� ������ �� �ִ� ���� �����ϰ� ���ο� �޼��带 ����.
 * 6- ������ EnumAttribute�� AttributeType���� ����, enumAttribute�������� eAttribute�� ����
 * 
 * <v3.0 - 2023_1222_�ֿ���>
 * 1- private������ ����ȭ�ϱ� ���� [JsonProperty] ��Ʈ����Ʈ�� �߰��Ͽ���
 * 
 * <v3.1 - 2023_1224_�ֿ���>
 * 1- Clone�޼��� ���� (ItemŬ�������� ���� ����� ����ϹǷ�)
 * 2- �⺻ protected ���� enumWeaponType�� enumBasicGrade�� �����ϰ�, 
 * get�� ������ �ڵ� ���� ������Ƽ eWeaponType, eBasicGrade���� ����
 *  
 * <v4.0 - 2023_1226_�ֿ���>
 * 1- �ڵ����� ������Ƽ�� �Ϲ�������Ƽ�� ����, ���κ��� JsonPropertyó�� �� ������Ƽ JsonIgnoreó��
 * (������Ƽ�� ������� ���� �� �ε�� ������Ƽ�� set�� ���� ������ ������ �ݿ����� �ʱ� ����)
 * 
 * <v4.1 - 2023_1229_�ֿ���>
 * 1- ���ϸ� ���� ItemData_Weapon->ItemWeapon
 * 
 * 
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
    public enum AttributeType { Water, Gold, Earth, Fire, Wind, None }
   
    /// <summary>
    /// ����� ���� ��� - �븻, ����, ����, ����, ����ũ, ������
    /// </summary>
    public enum Rarity { Normal, Magic, Rare, Epic, Unique, Legend }
    


    /// <summary>
    /// �Ϲ� ���� ������
    /// </summary>
    [Serializable]
    public class ItemWeapon : Item
    {       
        /*** �⺻ ���� ���� ***/
        [JsonProperty] protected WeaponType eWeaponType;               // ���� �Һз� Ÿ�� (��,â,Ȱ...)
        [JsonProperty] protected ItemGrade eBasicGrade;                // ���� �⺻ �з� Ÿ�� (�ʱ�, �߱�, ���)
        [JsonProperty] protected int iPower;                           // ���ݷ�
        [JsonProperty] protected int iDurability;                      // ����
        [JsonProperty] protected float fSpeed;                         // ���ݼӵ�
        [JsonProperty] protected int iWeight;                          // ����
        [JsonProperty] protected AttributeType eAttribute;             // ���� �Ӽ�


        /** ������ ���׷��̵� ���� **/
        [JsonProperty] protected int iEnhanceNum;                      // ���� ��ȭ Ƚ��
        [JsonProperty] protected bool bIsAttrUnlocked;                 // �Ӽ� �ع� ����       
        [JsonProperty] protected int iRemainEngraveNum;                // ���� ���� Ƚ�� 
        [JsonProperty] protected int iMaxEngraveNum;                   // �ִ� ���� Ƚ�� (���� �з� ��޿� ���� ����)
        [JsonProperty] protected ItemEngraving[] sArrEquipEngrave;     // ���� ���� ����
        [JsonProperty] protected float fBasePerformance;               // ��� �⺻ ���� (�Ϲ� ����-100, ���۹���-���ۿ����� ��ȭ)
                    


        /// <summary>
        /// ���� ������ �Һз� Ÿ�� - Sword, Blade, Spear, Dagger ���� �ֽ��ϴ�.
        /// </summary>
        [JsonIgnore] public WeaponType WeaponType { get { return eWeaponType; } }
        /// <summary>
        /// ���� �������� �⺻ �з� Ÿ�� - �ʱ�, �߱�, ���
        /// </summary>
        [JsonIgnore] public ItemGrade BasicGrade { get { return eBasicGrade; } }
        
        /// <summary>
        /// ���� ���� Ƚ���� �ڵ����� �����ؼ� ��ȯ�մϴ�.
        /// </summary>
        [JsonIgnore] public int RemainEngraveNum { get{ return iRemainEngraveNum; } }
        
        /// <summary>
        /// ���� ������ ������ �����Դϴ�.
        /// </summary>
        [JsonIgnore] public int EquipEngraveNum { get{ return iMaxEngraveNum-iRemainEngraveNum; } }
        
        /// <summary>
        /// ���� �������� ���� �迭 ������ ��ȯ�մϴ�. <br/>
        /// �� ������ ��쿡�� ����ü�� �Ҵ�Ǿ� �����Ƿ�, ���� �迭�� ������ EquipEngraveNum�� ����ؼ� Ȯ���Ͽ��� �մϴ�.
        /// </summary>
        [JsonIgnore] public ItemEngraving[] EquipEngraveArrInfo
        {
            get{ return sArrEquipEngrave;} 
        }

        /// <summary>
        /// ���⿡ ������ �����ϴ� �޼��� �Դϴ�. �����ϰ��� �� ���� �̸��� �ʿ��ϸ�, �����ִ� ���� ������ ���ٸ� false�� ��ȯ�մϴ�.
        /// </summary>
        public bool Engrave(string engravingName)
        {
            if(iRemainEngraveNum==0)
                return false;

            sArrEquipEngrave[EquipEngraveNum] = new ItemEngraving(engravingName);
            iRemainEngraveNum--;
            return true;
        }




        /// <summary>
        /// �������� �⺻ �����Դϴ�. �⺻���� 100�̸�, ���۹����� ��� �� ���ɿ� ������ �־�� �մϴ�.
        /// </summary>
        [JsonIgnore] public float BasePerformance { get{ return fBasePerformance; } set{ fBasePerformance=value; } }

        /// <summary>
        /// �������� ���� ������ �ڵ����� �����Ͽ� ��ȯ�մϴ�. �⺻ ���ɿ� �Ӽ�(1.5f)�� ����(1.1f,1.15f,1.2f)�� ���� ���� �������� ���� ��ġ�Դϴ�.
        /// </summary>
        [JsonIgnore] public float LastPerformance {
            get{ 
                float iLastMult = fBasePerformance; // �⺻���ɿ���
                float attributeMult = bIsAttrUnlocked? 1f : 1.5f;    // true �� ��(������� ��) 1
                
                iLastMult *= attributeMult;         // �Ӽ����� ������ ���Ѵ�.
                               
                for(int i=0; i<EquipEngraveNum; i++)
                    iLastMult *= sArrEquipEngrave[i].PerformMult;  // �� ���μ��� ������ ���Ѵ�.

                return iLastMult;                 
               }            
         }
        
        /// <summary>
        /// ����� ���� ���ɿ� ���� ���� ����Դϴ�. ( �븻, ����, ����, ����, ����ũ, ������ )<br/>
        /// </summary>
        public Rarity LastGrade
        {
            get{                
                if( LastPerformance>1800f)
                    return Rarity.Legend;                
                else if( LastPerformance>1300f )
                    return Rarity.Unique;                
                else if( LastPerformance>800f )
                    return Rarity.Epic;            
                else if( LastPerformance>500f )
                    return Rarity.Rare;            
                else if( LastPerformance>300f )
                    return Rarity.Magic;
                else if( LastPerformance>=100f )
                    return Rarity.Normal;
                else
                    Debug.Log("������ġ�� 100�̸� �Դϴ�. Ȯ���Ͽ� �ּ���.");
                
                return Rarity.Normal;
            }
        }



        
        /// <summary>
        /// ���ݷ��� ���������� ������� 1:1���踦 �����ϴ�.
        /// </summary>
        [JsonIgnore] public int Power { get{return (int)Mathf.Round(iPower*LastPerformance/100f);} }

        /// <summary>
        /// �������� ���������� ������� 1:0.5���踦 �����ϴ�. 
        /// </summary>
        [JsonIgnore] public int Durability { get{return (int)Mathf.Round(iPower*LastPerformance/200f);} }
        [JsonIgnore] public float Speed { get{return fSpeed;} }
        [JsonIgnore] public int Weight { get{return iWeight;} }  

        /// <summary>
        /// ������ ���� �Ӽ��Դϴ�. ���� ���ο� ����� �����ϴ�.
        /// </summary>
        [JsonIgnore] public AttributeType OrinalAttribute { get{return eAttribute;} }  

        /// <summary>
        /// ���� ������ �Ӽ��Դϴ�. ������� �ʾҴٸ� ���Ӽ��Դϴ�.
        /// </summary>
        [JsonIgnore] public AttributeType CurrentAttribute { 
            get{ 
                if(bIsAttrUnlocked)
                    return AttributeType.None;
                else
                    return eAttribute;                
                }             
        }
                
        /// <summary>
        /// ����� �Ӽ����� ���θ� �� �� �ֽ��ϴ�.
        /// </summary>
        [JsonIgnore] public bool IsAttrUnlocked { get{ return bIsAttrUnlocked; } }
                


        public bool AttributeUnlock(AttributeType attribute)
        {
            if(IsAttrUnlocked)          // ���Ⱑ �̹� �ع�Ǿ��ִٸ� �������� ����.
                return false;

            if(attribute == eAttribute) // ���� �Ӽ��� �����Ӽ��� ��ġ�Ѵٸ� ����
            {
                bIsAttrUnlocked = true;
                return true;
            }
            else                        // ���� �Ӽ��� �����Ӽ��� ��ġ���� �ʴ´ٸ� ����.
                return false;           
        }






        /// <summary>
        /// ��� �������� ���� ��ȭ Ƚ���Դϴ�. �÷��̾��� ��ȣ�ۿ뿡 ���� ������ �����մϴ�.
        /// </summary>
        [JsonIgnore] public int EnhanceNum { 
            get{return iEnhanceNum;}
            set{
                    if( 0<=iEnhanceNum && iEnhanceNum<=9 )  //��ȭ�� ���� ����� 0���� 9����
                        iEnhanceNum = value;
               }
        }


        /// <summary>
        /// ���� �������� ���� ������ ���� ������ �ɼ�
        /// </summary>
        public ItemWeapon(ItemType mainType, WeaponType subType, string No, string name, float price, ImageReferenceIndex imgRefIdx // ������ �⺻ ���� 
            ,ItemGrade basicGrade, int power, int durability, float speed, int weight, AttributeType attribute                      // ���� ���� ����
        ) : base( mainType, No, name, price, imgRefIdx )
        {
            eWeaponType = subType;
            eBasicGrade = basicGrade;
            iPower = power;
            iDurability = durability;
            fSpeed = speed;
            iWeight = weight;
            eAttribute = attribute;

            iEnhanceNum = 0;                                        // �⺻���� ��ȭ �Ǿ����� ����.
            bIsAttrUnlocked = true;                                  // �⺻���� �Ӽ��� �������.
        
            iMaxEngraveNum = (int)basicGrade + 1;                   // �ִ� ����Ƚ���� ������ ���� (�ʱ�:1, �߱�:2, ���:3)
            iRemainEngraveNum = iMaxEngraveNum;                     // ���� ����Ƚ���� ������ ���� �� �ϳ� �� �پ��
            sArrEquipEngrave = new ItemEngraving[iMaxEngraveNum];  // ���� ���� �迭�� ũ��� �ִ� ���� Ƚ���� ����(�⺻ ��� �� ����)
            fBasePerformance = 100f;                                // ���۾������� �ƴ� ��� �⺻ ���� 100f
        }

    }
}
