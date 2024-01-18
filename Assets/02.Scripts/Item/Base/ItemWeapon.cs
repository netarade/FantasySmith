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
 * <v4.2 - 2024_0108_�ֿ���>
 * 1- ItemImageCollection�� ItemVisualCollection��Ī ��������
 * �������� �Ű������� imgRefIndex�� visualRefIndex ����
 * 
 * <v5.0 - 2024_0111_�ֿ���>
 * 1- ũ������ �帣�� �°� Ŭ���� ���� ����
 * 
 * <v5.1 - 2024_0112_�ֿ���>
 * 1- ItemWeaponŬ������ �θ� Item�� �ƴ϶� ItemEquip���� ����
 * ������ �� �� ���밡���� ������ Ŭ������ ���� �����ϱ� ����
 * 
 * <V5.2 - 2024_0117_�ֿ���>
 * 1- WeaponType�� PICKAX�� NONE �߰�
 * 
 * <v5.3 - 2024_0118_�ֿ���>
 * 1- WeaponType�� PICKAX�� NONE ������ Pickax, None���� ���� �� �ּ� ����
 * 2- ������Ƽ Power�� Durability�� ���������ϵ��� �����Ͽ���. 
 * ������ InfoŬ�������� ���� �����ؾ� �ϹǷ�
 * 
 * (��������)
 * 1- Pickax�� �����ϰ� Production �Ǵ� Tool���� ������ ���� (����迭 �Ǵ� ���� �������̹Ƿ�)
 * (ItemInfo_4.cs�� weaponTypeString�� �����ؾ���)
 * 
 */
namespace ItemData
{    
    // ItemInfo_4.cs�� weaponTypeString�� �����Ұ�!
    /// <summary>
    /// ���� �������� �� �з� (��, â, ����, �б�, Ȱ, ���, ����, ����, ��Ÿ, ����)
    /// </summary>
    public enum WeaponType { Sword, Spear, Axe, Blunt, Bow, Pickax, Production, Tool, Etc, None } 


    /// <summary>
    /// �Ϲ� ���� ������
    /// </summary>
    [Serializable]
    public class ItemWeapon : ItemEquip
    {       
        /*** �⺻ ���� ���� ***/
        [JsonProperty] protected WeaponType eWeaponType;   // ���� �Һз� Ÿ�� (��,â,Ȱ...)
        [JsonProperty] protected int iPower;               // ���ݷ�
        [JsonProperty] protected int iDurability;          // ����

        /** ������ ���׷��̵� ���� **/
        [JsonProperty] protected float fPowerMult;         // ��� �⺻ ���� (�Ϲ� ����-100, ���۹���-���ۿ����� ��ȭ)
                    


        /// <summary>
        /// ���� ������ �Һз� Ÿ�� - Sword, Blade, Spear, Dagger ���� �ֽ��ϴ�.
        /// </summary>
        [JsonIgnore] public WeaponType WeaponType { get { return eWeaponType; } }
        
        /// <summary>
        /// �������� ���ݷ¿� �������� ��ġ�Դϴ�. �⺻���� 1�̸�, ���۹����� ��� �� ���ɿ� ������ �� �� �ֽ��ϴ�.
        /// </summary>
        [JsonIgnore] public float PowerMult { get{ return fPowerMult; } set{ fPowerMult=value; } }

        
        /// <summary>
        /// �������� ���ݷ��Դϴ�. ���������� �����Դϴ�.
        /// </summary>
        [JsonIgnore] public int Power { 
                get { return (int)Mathf.Round(iPower*PowerMult); }  
                set { iPower=value; } 
            }

        /// <summary>
        /// �������� ������ �Դϴ�. ���������� �����Դϴ�.
        /// </summary>
        [JsonIgnore] public int Durability { 
                get { return iDurability; }
                set { iDurability=value; }
            }
        
        

        /// <summary>
        /// ���� �������� ���� ������ ���� ������ �ɼ�
        /// </summary>
        public ItemWeapon(

            // ������ �⺻ ���� 
            ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, 

            // ���� ���� ����
            WeaponType subType, int power, int durability, string desc
            
        ) : base( mainType, No, name, visualRefIndex, desc )
        {
            eWeaponType = subType;
            iPower = power;
            iDurability = durability;
            fPowerMult = 1f;                               
        }

    }
}
